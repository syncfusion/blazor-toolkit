#!/usr/bin/env bash
# NuGet vulnerability gate (SEC-06).
#
# Fails closed:
#   * the `dotnet list package --vulnerable` command must exit 0 (restore / advisory-feed /
#     command failures fail the job instead of being masked),
#   * its output must be valid JSON,
#   * the JSON must not report any scan problem (for example an unreachable advisory source),
#   * only then are the structured vulnerability results evaluated against --min-severity.
#
# Usage: check-vulnerabilities.sh --project <csproj> [--min-severity Low|Moderate|High|Critical]
#                                 [--report <markdown file>]
#        check-vulnerabilities.sh --json-file <saved scan output> ...   (evaluate only; used by tests)
set -euo pipefail

project=""
min_severity="High"
report="vuln-report.md"
json_file=""

while [ $# -gt 0 ]; do
  case "$1" in
    --project) project="$2"; shift 2 ;;
    --min-severity) min_severity="$2"; shift 2 ;;
    --report) report="$2"; shift 2 ;;
    --json-file) json_file="$2"; shift 2 ;;
    *) echo "::error::Unknown argument: $1"; exit 2 ;;
  esac
done

rank() {
  case "${1,,}" in
    low) echo 1 ;;
    moderate) echo 2 ;;
    high) echo 3 ;;
    critical) echo 4 ;;
    *) echo 4 ;; # unknown severity: fail closed
  esac
}

case "${min_severity,,}" in
  low|moderate|high|critical) ;;
  *) echo "::error::--min-severity must be Low, Moderate, High or Critical"; exit 2 ;;
esac
threshold="$(rank "$min_severity")"

scan_json="$(mktemp)"
scan_err="$(mktemp)"
trap 'rm -f "$scan_json" "$scan_err"' EXIT

if [ -n "$json_file" ]; then
  cp "$json_file" "$scan_json"
else
  [ -n "$project" ] || { echo "::error::--project or --json-file is required"; exit 2; }
  # No `|| true`: a non-zero exit (restore failure, unreachable advisory source, ...) fails the job.
  if ! dotnet list "$project" package --vulnerable --include-transitive --format json \
        > "$scan_json" 2> "$scan_err"; then
    echo "::error::'dotnet list package --vulnerable' failed; the scan result is not trustworthy"
    cat "$scan_err" || true
    cat "$scan_json" || true
    exit 1
  fi
fi

if ! jq -e . "$scan_json" > /dev/null 2>&1; then
  echo "::error::Vulnerability scan did not return valid JSON"
  cat "$scan_json" || true
  exit 1
fi

# Scan problems (e.g. NU1900 "unable to load the service index for the advisory source").
problems="$(jq -r '[.problems[]?, (.projects[]?.problems[]?)] | map("\(.level // "error"): \(.text // .)") | .[]' "$scan_json")"
if [ -n "$problems" ]; then
  echo "::error::Vulnerability scan reported problems; refusing to treat the result as a pass"
  echo "$problems"
  exit 1
fi

# One row per vulnerable package: id, resolved version, severity, advisory URL.
findings="$(jq -r '
  [ .projects[]?.frameworks[]?
    | (.topLevelPackages[]?, .transitivePackages[]?) as $p
    | $p.vulnerabilities[]?
    | { id: $p.id, version: ($p.resolvedVersion // $p.requestedVersion // "?"),
        severity: (.severity // "Unknown"), url: (.advisoryurl // "") } ]
  | unique | .[] | [.id, .version, .severity, .url] | @tsv' "$scan_json")"

{
  echo "## NuGet Vulnerability Report"
  echo
  echo "Minimum failing severity: **${min_severity}**"
  echo
  if [ -z "$findings" ]; then
    echo "No vulnerable packages reported."
  else
    echo "| Package | Version | Severity | Advisory |"
    echo "|---|---|---|---|"
    while IFS=$'\t' read -r id version severity url; do
      echo "| ${id} | ${version} | ${severity} | ${url} |"
    done <<< "$findings"
  fi
} > "$report"

failing=0
if [ -n "$findings" ]; then
  while IFS=$'\t' read -r id version severity url; do
    if [ "$(rank "$severity")" -ge "$threshold" ]; then
      echo "::error::${id} ${version} has a ${severity} vulnerability ${url}"
      failing=$((failing + 1))
    else
      echo "::warning::${id} ${version} has a ${severity} vulnerability ${url}"
    fi
  done <<< "$findings"
fi

if [ "$failing" -gt 0 ]; then
  echo "::error::${failing} vulnerable package(s) at or above ${min_severity}"
  exit 1
fi

echo "Vulnerability scan completed successfully; nothing at or above ${min_severity}."
