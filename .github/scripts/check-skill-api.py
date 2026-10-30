#!/usr/bin/env python3
"""Fail when AI skill documentation references members that the shipped package does not expose.

The public API surface is read from the XML documentation file that ships inside the
packed .nupkg (lib/<tfm>/Syncfusion.Blazor.Toolkit.xml, generated from the compiled
assembly's public members because GenerateDocumentationFile=true).

Checks, per fenced code block in .github/skills/**/*.md:
  1. Parameters / events on <SfXxx ...> and child-component tags must exist as a public
     member on some documented type (inherited members resolve through the global name set).
  2. `@ref` variables: `x.Method(...)` must be a public method of the referenced component
     type, or a method that exists on some documented type (inherited members).
  3. Attribute values written as `Type.Member` (for example Position="TooltipPosition.TopCenter")
     must name a documented type.

A line containing the marker "❌" or "skill-api-check: ignore" is skipped, so intentional
"wrong vs. right" illustrations can stay in the docs.

Usage: check-skill-api.py --xml <Syncfusion.Blazor.Toolkit.xml | path.nupkg> [--skills DIR ...] [--warn-only]
"""
from __future__ import annotations

import argparse
import re
import sys
import xml.etree.ElementTree as ET
import zipfile
from pathlib import Path

NAMESPACE_PREFIX = "Syncfusion.Blazor.Toolkit."
IGNORE_MARKERS = ("❌", "skill-api-check: ignore")
FENCE_LANGS = {"razor", "csharp", "cs", "html", "cshtml", ""}

# Attributes that are framework / generic-type plumbing rather than component API.
FRAMEWORK_ATTRS = {"TValue", "TItem", "TChart", "TData", "TChecked"}
# BCL / framework types that may legitimately appear as `Type.Member` in attribute values.
KNOWN_EXTERNAL_TYPES = {
    "DateTime", "DateOnly", "TimeOnly", "TimeSpan", "DateTimeOffset", "String", "Math",
    "CultureInfo", "TimeZoneInfo", "Environment", "Guid", "Color", "Colors", "Enumerable",
    "Convert", "Path", "File", "Console", "Task", "Regex", "Utils", "Model", "Models",
    "CalendarWeekRule", "DayOfWeek", "NumberFormatInfo", "DateTimeKind",
}


def load_members(source: Path) -> tuple[dict[str, set[str]], set[str]]:
    if source.suffix == ".nupkg":
        with zipfile.ZipFile(source) as pkg:
            candidates = sorted(
                n for n in pkg.namelist()
                if n.startswith("lib/") and n.endswith("Syncfusion.Blazor.Toolkit.xml")
            )
            if not candidates:
                sys.exit(f"::error::No Syncfusion.Blazor.Toolkit.xml found in {source}")
            # Use the highest TFM; the public surface is identical across TFMs by design.
            data = pkg.read(candidates[-1])
        root = ET.fromstring(data)
    else:
        root = ET.parse(source).getroot()

    types: dict[str, set[str]] = {}
    all_members: set[str] = set()
    for member in root.iter("member"):
        name = member.get("name", "")
        kind, _, full = name.partition(":")
        full = re.sub(r"\(.*$", "", full)          # drop parameter list
        full = re.sub(r"`\d+", "", full)           # drop generic arity
        if not full.startswith(NAMESPACE_PREFIX):
            continue
        rest = full[len(NAMESPACE_PREFIX):]
        if kind == "T":
            types.setdefault(rest.split(".")[-1], set())
        elif kind in {"P", "M", "F", "E"}:
            owner, _, member_name = rest.rpartition(".")
            owner_simple = owner.split(".")[-1]
            types.setdefault(owner_simple, set()).add(member_name)
            all_members.add(member_name)
    return types, all_members


def code_blocks(text: str):
    """Yield (start_line, lang, lines) for each fenced block."""
    lines = text.splitlines()
    i = 0
    while i < len(lines):
        m = re.match(r"^\s*```(\w*)", lines[i])
        if m:
            lang, start, body = m.group(1).lower(), i + 1, []
            i += 1
            while i < len(lines) and not lines[i].lstrip().startswith("```"):
                body.append(lines[i])
                i += 1
            yield start + 1, lang, body
        i += 1


def iter_tags(block_lines: list[str]):
    """Yield (line_offset, tag_name, [(attr, value)]) for each Razor element, quote-aware."""
    text = "\n".join(block_lines)
    pos = 0
    while True:
        m = re.compile(r"<([A-Z]\w*)\b").search(text, pos)
        if not m:
            return
        name, i = m.group(1), m.end()
        attrs: list[tuple[str, str | None]] = []
        depth = 0
        while i < len(text):
            c = text[i]
            if depth == 0 and c == ">":
                break
            if c.isspace() or c == "/":
                i += 1
                continue
            am = re.compile(r"[@\w:\-.]+").match(text, i)
            if not am:
                i += 1
                continue
            attr, i = am.group(0), am.end()
            value = None
            if i < len(text) and text[i] == "=":
                i += 1
                if i < len(text) and text[i] in "\"'":
                    q = text[i]
                    if text.startswith("@(", i + 1):
                        # Razor expression: balance parentheses so inner quotes do not end the value.
                        level, j = 0, i + 2
                        while j < len(text):
                            level += text[j] == "("
                            level -= text[j] == ")"
                            j += 1
                            if level < 0:
                                break
                        k = text.find(q, j)
                        k = len(text) if k == -1 else k
                    else:
                        k = text.find(q, i + 1)
                        k = len(text) if k == -1 else k
                    value, i = text[i + 1:k], k + 1
                elif text.startswith("@(", i):
                    level, j = 0, i + 1
                    while j < len(text):
                        level += text[j] == "("
                        level -= text[j] == ")"
                        j += 1
                        if level == 0:
                            break
                    value, i = text[i:j], j
                else:
                    vm = re.compile(r"[^\s>]+").match(text, i)
                    value, i = (vm.group(0), vm.end()) if vm else ("", i)
            attrs.append((attr, value))
        line_offset = text.count("\n", 0, m.start())
        yield line_offset, name, attrs
        pos = i + 1


def check_file(path: Path, types: dict[str, set[str]], all_members: set[str]) -> list[str]:
    errors: list[str] = []
    # The documentation XML lists a member only under its declaring type, so inherited methods
    # (SfDateTimePicker.FocusAsync from SfDatePicker, DisposeAsync from SfBaseComponent, ...)
    # are accepted when ANY public type in the package declares a method of that name.
    base_members = all_members
    text = path.read_text(encoding="utf-8")
    refs: dict[str, str] = {}
    blocks = [b for b in code_blocks(text) if b[1] in FENCE_LANGS]

    for start, _, body in blocks:
        for _, name, attrs in iter_tags(body):
            for attr, value in attrs:
                if attr == "@ref" and name in types and value:
                    refs[value] = name

    for start, _, body in blocks:
        for offset, name, attrs in iter_tags(body):
            line_no = start + offset
            if body[offset] and any(mk in body[offset] for mk in IGNORE_MARKERS):
                continue
            if name not in types:
                continue
            for attr, value in attrs:
                bare = re.sub(r"^@bind-", "", attr).split(":")[0]
                if attr.startswith("@") and not attr.startswith("@bind-"):
                    continue
                if not bare or not bare[0].isupper() or bare in FRAMEWORK_ATTRS:
                    continue
                if bare not in types[name] and bare not in all_members:
                    errors.append(
                        f"{path}:{line_no}: <{name}> uses '{bare}', which is not a public member in the shipped package")
                if value:
                    vm = re.fullmatch(r"@?([A-Z]\w+)\.([A-Z]\w+)", value.strip())
                    if vm and vm.group(1) not in types and vm.group(1) not in KNOWN_EXTERNAL_TYPES:
                        errors.append(
                            f"{path}:{line_no}: <{name} {attr}=\"{value}\"> references unknown type '{vm.group(1)}'")

        for idx, line in enumerate(body):
            if any(mk in line for mk in IGNORE_MARKERS):
                continue
            for var, method in re.findall(r"\b([a-z_]\w*)[!?]?\.([A-Z]\w*)\s*\(", line):
                comp = refs.get(var)
                if comp and method not in types[comp] and method not in base_members:
                    errors.append(
                        f"{path}:{start + idx}: '{var}.{method}()' is not a public method of {comp} in the shipped package")
    return errors


def main() -> int:
    ap = argparse.ArgumentParser()
    ap.add_argument("--xml", required=True, type=Path, help="Documentation XML or .nupkg")
    ap.add_argument("--skills", type=Path, nargs="+", default=[Path(".github/skills")],
                    help="Skill directories to check (default: .github/skills)")
    ap.add_argument("--warn-only", action="store_true",
                    help="Report findings as warnings and exit 0 (advisory runs)")
    args = ap.parse_args()
    level = "warning" if args.warn_only else "error"

    types, all_members = load_members(args.xml)
    if not types:
        print("::error::No public API members were read from the package metadata", file=sys.stderr)
        return 2

    errors: list[str] = []
    files = sorted(f for d in args.skills for f in d.rglob("*.md"))
    for f in files:
        errors += check_file(f, types, all_members)

    if errors:
        print(f"Skill documentation references {len(errors)} member(s) that do not exist in the shipped API:")
        for e in errors:
            print(f"::{level}::{e}")
        return 0 if args.warn_only else 1
    print(f"Skill API check passed: {len(files)} files, {len(types)} public types.")
    return 0


if __name__ == "__main__":
    sys.exit(main())
