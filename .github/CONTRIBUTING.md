# Contributing

Contributions to the Syncfusion Toolkit for Blazor are welcome. For a larger
change or new component, **open an issue or discuss it with a maintainer before
implementation** to agree scope and avoid duplicated effort. The toolkit is
Chart-first; a Grid is not currently included. Proposals for future components
are welcome, but a roadmap entry is not a delivery commitment.

## Code of conduct and private reports

Follow the [Code of Conduct](CODE_OF_CONDUCT.md); its private reporting contact
is **legal@syncfusion.com**. Report vulnerabilities through
[SECURITY.md](SECURITY.md), not a public issue. See [SUPPORT.md](SUPPORT.md) for
best-effort support channels.

## Choose work and set up

- Search [open issues](https://github.com/syncfusion/blazor-toolkit/issues) and
  the [roadmap](../ROADMAP.md). Comment on an existing issue to coordinate work;
  an unassigned item is not automatically approved for implementation.
- Follow [DEVELOPMENT.md](DEVELOPMENT.md) for SDK 10, the .NET 8 test runtime,
  Node 22.13+, theme generation, CycloneDX and gallery workload prerequisites.
- Read [TEST-MATRIX.md](TEST-MATRIX.md) before describing test coverage. The bUnit
  project runs on .NET 8; SDK-labelled CI jobs do not prove three-runtime coverage.

## Coding and compatibility

Use the existing file's conventions and keep unrelated formatting out of a PR.
The project's traditional C# guidance follows the
[.NET coding style](https://github.com/dotnet/runtime/blob/main/docs/coding-guidelines/coding-style.md),
with omitted `private` on members where it is the default and hard tabs as local
conventions. Existing files are mixed; do not bulk-reformat them to enforce those
preferences.

Keep changes focused, document public behavior and add regression tests for code
fixes. Review API and nullability changes explicitly: there is no tracked shipped
API baseline or verified automated compatibility gate. Preserve the dated analyzer
and BL0007 decisions in [DEVELOPMENT.md](DEVELOPMENT.md) rather than broadly
suppressing warnings. Do not weaken security rules or alter manual release policy
as a side effect of a contribution.

## Pull requests

Target **`main`** and use the [pull request template](PULL_REQUEST_TEMPLATE.md).
Include:

- the related issue, purpose, scope and root cause where applicable;
- compatibility or migration impact, including public nullability contracts;
- checks actually run, exact project/framework and relevant results;
- screenshots for visible changes, plus keyboard/accessibility observations;
- unrun checks and known gaps, rather than assuming CI or all browsers passed.

Docs-only changes do not need builds/tests unless they affect executable behavior;
check paths, links, factual claims and examples instead. For code changes, run the
relevant checks described in the development guide. Treat generated assets and
local evidence as such, not as proof of released-package contents or compliance.

Where appropriate, enabling "Allow edits from maintainers" lets maintainers help
with small fixes; review that permission for your fork before enabling it.
Never include credentials, signing material, personal data or confidential logs.
Signing and publication remain manual release-owner responsibilities.

## Proposals and planned work

Use the [feature request form](https://github.com/syncfusion/blazor-toolkit/issues/new?template=feature_request.yml).
Describe the problem, proposed API/behavior and alternatives. Link related issues
or roadmap entries. Planned work can still benefit from new use cases; contribute
them to the existing discussion rather than creating a duplicate. There is no
`spec.md` issue template in the current repository.

## Review and merge

Maintainers review scope, correctness, compatibility, tests and documentation before
merging. Review availability is best-effort; this document does not guarantee a
reviewer count, assignment time, acceptance or release date. Respond to feedback
or let maintainers know if you cannot continue. Reactions indicate interest but
do not automatically prioritize proposals. Actual repository protection settings
and maintainer approval determine merge requirements.
