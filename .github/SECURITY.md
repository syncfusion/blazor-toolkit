# Security Policy

## Reporting a vulnerability

Report suspected vulnerabilities privately to **security@syncfusion.com**, rather than in a public issue. Include:

- affected package version, component, render mode, and environment;
- reproducible steps or a minimal example, without credentials or personal data;
- expected and observed behavior, impact, and any relevant evidence.

Please coordinate public disclosure with the security contact and wait for confirmation that the issue has been addressed before announcing exploit details publicly.

## Response and servicing status

The private reporting route above is retained. Acknowledgement, investigation, remediation, and disclosure timing must be confirmed with the security contact for the specific report. This repository does not substantiate a contractual support or remediation SLA, supported-version window, or fixed servicing schedule.

**Legacy claims superseded:** earlier text promised acknowledgement within two business days, 7/30/90-day remediation, immediate out-of-band patches, and monthly servicing on a specified Wednesday. Approval and delivery evidence for those commitments was not established in this documentation review; they must not be treated as current guarantees.

Public release information is available on the [GitHub Releases page](https://github.com/syncfusion/blazor-toolkit/releases). A release listing alone does not verify remediation of a particular vulnerability.

## Evidence and limits

The [threat model](THREAT-MODEL.md) records trust boundaries, configured checks, and risks requiring disposition. [Render-mode security](RENDER-MODE-SECURITY.md) explains the division between library behavior and host responsibilities.

- Security analysis and vulnerability checks are configured in the repository; their presence is not a clean scan result or an attestation about a published package.
- CI is configured to produce unsigned review packages. Signing and publishing remain a manual release-maintainer process outside public CI; completion and signature status for any particular release are unverified here.
- SBOM generation configuration and snapshot limitations are documented in the [SBOM evidence notes](../artifacts/sbom/README.md).
- Host applications remain responsible for authentication, authorization, endpoint validation, content trust, and deployment security.

This is an evidence-grounded repository reference, **not a security certification, maintainer sign-off, or risk acceptance**. Earlier self-attestation language is superseded; no approval is recorded by this update.
