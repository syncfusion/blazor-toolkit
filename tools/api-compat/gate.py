"""Fail-closed release/API/metadata gate. Never generates or updates approvals."""
import argparse
import gzip
import hashlib
import json
import pathlib
import subprocess
import sys
import urllib.request
import xml.etree.ElementTree as ET
import zipfile

HERE = pathlib.Path(__file__).resolve().parent
ROOT = HERE.parents[1]


def verify_digest(data, expected):
    actual = hashlib.sha256(data).hexdigest()
    if actual != expected:
        raise ValueError(f"Baseline SHA-256 mismatch: {actual}")


def validate_baseline(baseline):
    if baseline.get("frameworks") != ["net8.0", "net9.0", "net10.0"]:
        raise ValueError("Baseline must cover exactly net8.0/net9.0/net10.0")


def compare_contract(expected, actual):
    required = {"identity", "surface", "parameters", "callbacks", "wire", "generated"}
    for value in [expected, actual]:
        if set(value) != required or not isinstance(value.get("surface"), list) or not value["surface"]:
            raise ValueError("Incomplete contract schema or empty surface inventory")
        if any(not isinstance(value[key], dict) for key in required - {"surface"}):
            raise ValueError("Invalid contract category type")
    for key in sorted(required):
        # Canonical JSON preserves booleans versus numbers (Python True == 1 would not).
        left = json.dumps(expected[key], sort_keys=True, separators=(",", ":"), allow_nan=False)
        right = json.dumps(actual[key], sort_keys=True, separators=(",", ":"), allow_nan=False)
        if left != right:
            raise ValueError(f"Unreviewed {key} contract change; inspect retained JSON, do not auto-rebaseline")


def run(command, log, cwd=None):
    result = subprocess.run([str(x) for x in command], cwd=cwd, stdout=subprocess.PIPE,
                            stderr=subprocess.STDOUT, text=True)
    log.write_text(result.stdout, encoding="utf-8")
    if result.returncode:
        raise RuntimeError(f"Command failed ({result.returncode}); see {log}")
    return result.stdout


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--assemblies", type=pathlib.Path, required=True,
                        help="Fresh library output root containing net8.0/net9.0/net10.0")
    parser.add_argument("--probe", type=pathlib.Path, required=True,
                        help="Freshly built ApiContractProbe.dll")
    parser.add_argument("--output", type=pathlib.Path, required=True)
    parser.add_argument("--package", type=pathlib.Path,
                        help="Optional offline exact baseline package (always hash-verified)")
    args = parser.parse_args()
    output = args.output.resolve()
    output.mkdir(parents=True, exist_ok=True)
    baseline = json.loads((HERE / "baseline.json").read_text())
    validate_baseline(baseline)
    project = ET.parse(ROOT / "src/Syncfusion.Blazor.Toolkit.csproj")
    if project.findtext(".//Version") != baseline["candidateVersion"]:
        raise ValueError("Source version differs from reviewed candidate version")
    if not args.probe.is_file():
        raise FileNotFoundError(args.probe)
    package = args.package
    if package is None:
        package = output / "baseline.nupkg"
        with urllib.request.urlopen(baseline["url"], timeout=60) as response:
            package.write_bytes(response.read())
    verify_digest(package.read_bytes(), baseline["sha256"])
    run(["dotnet", "nuget", "verify", package, "--all"], output / "signature.log")
    extracted = output / "baseline"
    with zipfile.ZipFile(package) as archive:
        for member in archive.infolist():
            path = pathlib.PurePosixPath(member.filename)
            if path.is_absolute() or ".." in path.parts or "\\" in member.filename:
                raise ValueError("Unsafe baseline ZIP entry")
        archive.extractall(extracted)
    nuspec = ET.parse(extracted / f'{baseline["package"]}.nuspec')
    metadata = next(e for e in nuspec.getroot() if e.tag.endswith("metadata"))
    identity = {e.tag.split("}")[-1]: e.text for e in metadata}
    if identity.get("id") != baseline["package"] or identity.get("version") != baseline["version"]:
        raise ValueError("Baseline nuspec identity mismatch")
    tools = output / "tools"
    executable = tools / ("apicompat.exe" if sys.platform == "win32" else "apicompat")
    if not executable.exists():
        run(["dotnet", "tool", "install", "Microsoft.DotNet.ApiCompat.Tool", "--version",
             baseline["apiCompatVersion"], "--tool-path", tools], output / "tool-install.log")
    version = run([executable, "--version"], output / "tool-version.log").strip()
    if version.split("+")[0] != baseline["apiCompatVersion"]:
        raise ValueError(f"Unexpected APICompat version: {version}")
    for tfm in baseline["frameworks"]:
        left = extracted / "lib" / tfm / "Syncfusion.Blazor.Toolkit.dll"
        right = args.assemblies.resolve() / tfm / "Syncfusion.Blazor.Toolkit.dll"
        if not left.is_file() or not right.is_file():
            raise FileNotFoundError(f"Missing nonzero framework input: {tfm}")
        run([executable, "-l", left, "-r", right, "--enable-rule-cannot-change-parameter-name",
             "--enable-rule-attributes-must-match", "--suppression-file", HERE / "approved-breaks.xml"],
            output / f"apicompat-{tfm}.log")
        text = run(["dotnet", args.probe.resolve(), right], output / f"contracts-{tfm}.json")
        actual = json.loads(text)
        identity = {"name": baseline["package"], "version": baseline["candidateVersion"] + ".0",
                    "framework": ".NETCoreApp,Version=v" + tfm[3:]}
        if actual.get("identity") != identity:
            raise ValueError(f"Candidate assembly identity/TFM mismatch: {tfm}")
        with gzip.open(HERE / f"reviewed-{tfm}.json.gz", "rt", encoding="utf-8") as stream:
            expected = json.load(stream)
        compare_contract(expected, actual)
        print(f"PASS {tfm}: pinned 1.0.2 baseline, exact approved exceptions, reviewed surface/parameters/callbacks/fixtures")
    print("This gate is not package/runtime, CSS, accessibility, security or universal persisted-state verification.")


if __name__ == "__main__":
    try:
        main()
    except (ValueError, RuntimeError, OSError, ET.ParseError, zipfile.BadZipFile) as error:
        print(f"API compatibility gate FAILED: {error}", file=sys.stderr)
        sys.exit(1)
