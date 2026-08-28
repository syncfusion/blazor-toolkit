# Security Policy

## 1. Purpose

We prioritize our project’s security and encourage the community to responsibly report any vulnerabilities.

## 2. Reporting a Vulnerability

We take the security of our Syncfusion Blazor Toolkit project very seriously. If you discover a security vulnerability, please report it responsibly using the steps outlined below.

### 2.1 How to Report

#### 2.1.1 Contact us privately:
- Please email us at **security@syncfusion.com** with a detailed report of the vulnerability.
- Wait until we verify that the problem has been fixed before making the announcement in public.

#### 2.1.2 Provide details:
- Provide instructions on how to replicate the vulnerability.
- Share any possible impact or exploitation scenarios.

#### 2.1.3 Expect a response:
- We will acknowledge receipt of your report within **2 business days**.
- You will receive updates on the progress as we investigate and resolve the issue.

## 3. Security Patch Process

### 3.1 Severity classification and remediation SLAs

Once a vulnerability is verified, it is classified by severity. The following SLAs apply from the date of verification:

| Severity | Definition | Target remediation |
|---|---|---|
| **Critical** | Remote code execution, complete authentication bypass, or data exfiltration | ≤ 7 days |
| **High** | Significant privilege escalation, persistent XSS, or supply-chain compromise | ≤ 30 days |
| **Medium** | Limited-scope XSS, information disclosure, or denial of service | ≤ 90 days |
| **Low** | Hardening improvements or defence-in-depth gaps with no direct exploitability | Next planned release |

### 3.2 Response steps

After a vulnerability is reported and verified:
- We will assess its severity and assign a classification from the table above.
- A fix will be developed and tested internally against the applicable SLA target.

The resolution will be included in the next available release. For Critical or High findings, an out-of-band security patch will be issued immediately.

Affected users will be notified in the GitHub repository's **Releases** page and other relevant channels.

### 3.3 Monthly servicing

On a **monthly cadence** (targeting the second Wednesday of each month), the maintainers will:

1. Run `dotnet list package --vulnerable` against all projects and review any flagged advisories.
2. Review and apply dependency updates (NuGet and npm) with a focus on security-relevant changes.
3. Triage any open security issues or disclosures received since the last cycle.
4. Produce a patch release if Critical or High findings are present; bundle Medium and Low fixes into the next minor release.

## 4. Security Self-Attestation

This project maintains a current security reference in the repository's [THREAT-MODEL.md](../THREAT-MODEL.md) document. The project team has reviewed the current architecture, package surface, and release flow and has documented the principal risks and mitigations in good faith.

This attestation reflects the project’s current understanding as of 2026-08-21 and is intended to be updated as the toolkit evolves.
