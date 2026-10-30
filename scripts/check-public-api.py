#!/usr/bin/env python
import argparse
import shutil
import subprocess
import sys
import tempfile
from pathlib import Path


def write_evidence(evidence_dir: str, text: str) -> None:
    if not evidence_dir:
        return
    evidence_path = Path(evidence_dir)
    evidence_path.mkdir(parents=True, exist_ok=True)
    (evidence_path / "public-api.txt").write_text(text, encoding="utf-8")


def run(command: list[str], cwd: Path) -> subprocess.CompletedProcess[str]:
    return subprocess.run(command, cwd=cwd, text=True, capture_output=True)


def find_built_assembly(repo_root: Path, project: Path, framework: str) -> Path | None:
    dll_name = f"{project.stem}.dll"
    candidates = [
        repo_root / "src" / "bin" / "Debug" / framework / dll_name,
        repo_root / "src" / "bin" / "Release" / framework / dll_name,
    ]
    for candidate in candidates:
        if candidate.exists():
            return candidate

    for candidate in (repo_root / "src" / "bin").rglob(dll_name):
        if framework in candidate.parts:
            return candidate
    return None


def get_public_surface(assembly_path: Path, framework: str) -> str:
    temp_dir = Path(tempfile.mkdtemp(prefix="public-api-dumper-"))
    try:
        csproj = temp_dir / "PublicApiDumper.csproj"
        csproj.write_text(
            f"""<Project Sdk=\"Microsoft.NET.Sdk\">\n  <PropertyGroup>\n    <OutputType>Exe</OutputType>\n    <TargetFramework>{framework}</TargetFramework>\n    <ImplicitUsings>enable</ImplicitUsings>\n    <Nullable>enable</Nullable>\n  </PropertyGroup>\n</Project>\n""",
            encoding="utf-8",
        )

        program = temp_dir / "Program.cs"
        program.write_text(
            """using System;\nusing System.Collections.Generic;\nusing System.Linq;\nusing System.Reflection;\nusing System.Runtime.CompilerServices;\n\nif (args.Length != 1)\n{\n    Console.Error.WriteLine(\"Expected assembly path argument.\");\n    return 1;\n}\n\nvar asm = Assembly.LoadFrom(args[0]);\nvar lines = new SortedSet<string>(StringComparer.Ordinal);\n\nforeach (var type in asm.GetExportedTypes().OrderBy(t => t.FullName, StringComparer.Ordinal))\n{\n    if (type.GetCustomAttributes(typeof(CompilerGeneratedAttribute), false).Length > 0)\n    {\n        continue;\n    }\n\n    lines.Add(FormatType(type));\n\n    const BindingFlags flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.DeclaredOnly;\n    foreach (var member in type.GetMembers(flags).OrderBy(MemberKey, StringComparer.Ordinal))\n    {\n        if (member is Type)\n        {\n            continue;\n        }\n\n        if (member is ConstructorInfo ctor && ctor.IsStatic)\n        {\n            continue;\n        }\n\n        if (member.GetCustomAttributes(typeof(CompilerGeneratedAttribute), false).Length > 0)\n        {\n            continue;\n        }\n\n        if (member is MethodInfo method && method.IsSpecialName)\n        {\n            continue;\n        }\n\n        if (member is PropertyInfo or EventInfo or FieldInfo or ConstructorInfo or MethodInfo)\n        {\n            lines.Add(\"  \" + FormatMember(type, member));\n        }\n    }\n}\n\nConsole.Write(string.Join(\"\\n\", lines) + \"\\n\");\nreturn 0;\n\nstatic string MemberKey(MemberInfo member) => member.MemberType + \":\" + member.Name + \":\" + member;\n\nstatic string FormatType(Type type)\n{\n    var kind = type.IsInterface ? \"interface\" : type.IsEnum ? \"enum\" : type.IsValueType ? \"struct\" : type.IsAbstract && type.IsSealed ? \"static class\" : type.IsAbstract ? \"abstract class\" : \"class\";\n    return kind + \" \" + Display(type);\n}\n\nstatic string FormatMember(Type declaringType, MemberInfo member)\n{\n    return member switch\n    {\n        ConstructorInfo constructor => \"ctor \" + Display(declaringType) + Params(constructor.GetParameters()),\n        MethodInfo method => \"method \" + TypeName(method.ReturnType) + \" \" + Display(declaringType) + \".\" + method.Name + TypeArgs(method) + Params(method.GetParameters()),\n        PropertyInfo property => \"property \" + TypeName(property.PropertyType) + \" \" + Display(declaringType) + \".\" + property.Name + \" {\" + (property.CanRead ? \"get;\" : string.Empty) + (property.CanWrite ? \"set;\" : string.Empty) + \"}\",\n        EventInfo @event => \"event \" + TypeName(@event.EventHandlerType!) + \" \" + Display(declaringType) + \".\" + @event.Name,\n        FieldInfo field => \"field \" + TypeName(field.FieldType) + \" \" + Display(declaringType) + \".\" + field.Name,\n        _ => member.MemberType + \" \" + member.Name\n    };\n}\n\nstatic string TypeArgs(MethodInfo method)\n{\n    if (!method.IsGenericMethodDefinition && !method.IsGenericMethod)\n    {\n        return string.Empty;\n    }\n\n    var args = method.GetGenericArguments();\n    return \"<\" + string.Join(\", \", args.Select(a => a.Name)) + \">\";\n}\n\nstatic string Params(ParameterInfo[] parameters) =>\n    \"(\" + string.Join(\", \", parameters.Select(parameter => TypeName(parameter.ParameterType) + \" \" + parameter.Name)) + \")\";\n\nstatic string Display(Type type)\n{\n    if (type.IsGenericType)\n    {\n        var name = type.Name;\n        var tick = name.IndexOf('`');\n        if (tick >= 0)\n        {\n            name = name[..tick];\n        }\n\n        var args = type.GetGenericArguments().Select(argument => argument.IsGenericParameter ? argument.Name : Display(argument));\n        return (type.Namespace is { Length: > 0 } ns ? ns + \".\" : string.Empty) + name + \"<\" + string.Join(\", \", args) + \">\";\n    }\n\n    return type.FullName ?? type.Name;\n}\n\nstatic string TypeName(Type type)\n{\n    if (type.IsByRef)\n    {\n        return TypeName(type.GetElementType()!) + \"&\";\n    }\n\n    if (type.IsPointer)\n    {\n        return TypeName(type.GetElementType()!) + \"*\";\n    }\n\n    if (type.IsArray)\n    {\n        return TypeName(type.GetElementType()!) + \"[]\";\n    }\n\n    return type switch\n    {\n        _ when type == typeof(void) => \"void\",\n        _ when type == typeof(string) => \"string\",\n        _ when type == typeof(object) => \"object\",\n        _ when type == typeof(bool) => \"bool\",\n        _ when type == typeof(byte) => \"byte\",\n        _ when type == typeof(sbyte) => \"sbyte\",\n        _ when type == typeof(short) => \"short\",\n        _ when type == typeof(ushort) => \"ushort\",\n        _ when type == typeof(int) => \"int\",\n        _ when type == typeof(uint) => \"uint\",\n        _ when type == typeof(long) => \"long\",\n        _ when type == typeof(ulong) => \"ulong\",\n        _ when type == typeof(float) => \"float\",\n        _ when type == typeof(double) => \"double\",\n        _ when type == typeof(decimal) => \"decimal\",\n        _ when type == typeof(char) => \"char\",\n        _ => Display(type)\n    };\n}\n""",
            encoding="utf-8",
        )

        result = run(["dotnet", "run", "--project", str(csproj), "--framework", framework, "--", str(assembly_path)], temp_dir)
        if result.returncode != 0:
            raise RuntimeError(result.stderr.strip() or result.stdout.strip() or "public API dumper failed")
        return result.stdout.replace("\r\n", "\n")
    finally:
        shutil.rmtree(temp_dir, ignore_errors=True)


def main() -> int:
    parser = argparse.ArgumentParser()
    parser.add_argument("--evidence-dir", default="")
    parser.add_argument("--generate", action="store_true")
    parser.add_argument("--project", default="src/Syncfusion.Blazor.Toolkit.csproj")
    parser.add_argument("--framework", default="net10.0")
    parser.add_argument("--baseline", default="src/PublicAPI.Shipped.txt")
    args = parser.parse_args()

    repo_root = Path(__file__).resolve().parent.parent
    project_path = repo_root / args.project
    baseline_path = repo_root / args.baseline

    print(f"Building {args.project} ({args.framework}) for public API surface...")
    build = run(["dotnet", "build", args.project, "-f", args.framework, "--nologo", "-v:q"], repo_root)
    if build.returncode != 0:
        text = "FAIL: build failed"
        write_evidence(args.evidence_dir, text)
        sys.stderr.write(build.stdout)
        sys.stderr.write(build.stderr)
        sys.stderr.write("dotnet build failed; cannot compute public API.\n")
        return 1

    assembly_path = find_built_assembly(repo_root, project_path, args.framework)
    if assembly_path is None:
        text = f"FAIL: assembly not found for {args.framework}"
        write_evidence(args.evidence_dir, text)
        sys.stderr.write(f"Built assembly not found for {args.framework} ({project_path.stem}.dll).\n")
        return 1

    print(f"Dumping public surface from {assembly_path}")
    current = get_public_surface(assembly_path, args.framework)

    if args.generate:
        baseline_path.parent.mkdir(parents=True, exist_ok=True)
        baseline_path.write_text(current, encoding="utf-8")
        unshipped_path = repo_root / "src/PublicAPI.Unshipped.txt"
        if not unshipped_path.exists():
            unshipped_path.write_text("# Empty unshipped public API bucket\n", encoding="utf-8")
        text = f"GENERATED baseline at {args.baseline}\n{current}"
        write_evidence(args.evidence_dir, text)
        print(f"PASS: wrote baseline {args.baseline} ({len(current.splitlines())} lines)")
        return 0

    if not baseline_path.exists():
        text = f"FAIL: baseline missing at {args.baseline}. Seed with: python ./scripts/check-public-api.py --generate"
        write_evidence(args.evidence_dir, text)
        sys.stderr.write(text + "\n")
        return 1

    expected = baseline_path.read_text(encoding="utf-8").replace("\r\n", "\n")
    actual = current.replace("\r\n", "\n")
    if expected.rstrip() == actual.rstrip():
        text = f"PASS: public API matches {args.baseline}"
        write_evidence(args.evidence_dir, text)
        print(text)
        return 0

    expected_lines = [line for line in expected.split("\n") if line]
    actual_lines = [line for line in actual.split("\n") if line]
    expected_set = set(expected_lines)
    actual_set = set(actual_lines)
    added = [line for line in actual_lines if line not in expected_set]
    removed = [line for line in expected_lines if line not in actual_set]

    report_lines = [
        f"FAIL: public API drift against {args.baseline}",
        f"  added ({len(added)}):",
        *[f"    + {line}" for line in added[:50]],
        f"  removed ({len(removed)}):",
        *[f"    - {line}" for line in removed[:50]],
        "  Fix: intentional changes require updating the baseline via --generate after review.",
    ]
    report = "\n".join(report_lines)
    write_evidence(args.evidence_dir, report)
    print(report)
    return 1


if __name__ == "__main__":
    raise SystemExit(main())
