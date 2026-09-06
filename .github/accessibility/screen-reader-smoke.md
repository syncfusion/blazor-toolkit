# Screen-reader smoke results

This table summarises manual screen-reader smoke tests run against
the major Syncfusion Blazor Toolkit components. Results are
re-verified at every minor release.

| Component | NVDA 2024.x (Windows 11 / Edge) | JAWS 2025 (Windows 11 / Edge) | Narrator (Windows 11 / Edge) | Notes |
|---|---|---|---|---|
| `SfButton` | PASS — name, role, state announced | PASS | PASS | |
| `SfButtonGroup` | PASS — radiogroup / toolbar role announced | PASS | PASS | |
| `SfCheckBox` | PASS — checked / indeterminate / disabled all announced | PASS | PASS | |
| `SfRadioButton` | PASS — radiogroup + arrow-key navigation | PASS | PASS | |
| `SfSwitch` | PASS — switch role + on/off state | PASS | PASS | |
| `SfTextBox` | PASS — label announced, aria-invalid on validation error | PASS | PASS | |
| `SfTextArea` | PASS | PASS | PASS | |
| `SfNumericTextBox` | PASS — aria-valuenow + aria-valuemin/max on spin | PASS | PASS | |
| `SfUploader` | PARTIAL — file-input button name not localised automatically; status announcements work | PARTIAL | PASS — file-input is platform default | Tracked under [`accessibility` issues](https://github.com/syncfusion/blazor-toolkit/issues?q=is%3Aopen+is%3Aissue+label%3Aaccessibility) |
| `SfCalendar` | PARTIAL — arrow-key moves announced; month/year combobox changes not announced | PARTIAL | PASS | Tracked under [`#273`](https://github.com/syncfusion/blazor-toolkit/issues/273) |
| `SfDatePicker` | PASS | PASS | PASS | |
| `SfDateTimePicker` | PASS | PASS | PASS | |
| `SfTimePicker` | PASS | PASS | PASS | |
| `SfDialog` | PASS — modal role, focus-trap, label/described-by | PASS | PASS | |
| `SfTooltip` | PARTIAL — long tooltips don't surface as live region | PARTIAL | PASS | Tracked under [`#272`](https://github.com/syncfusion/blazor-toolkit/issues/272) |
| `SfSpinner` | PASS — `role=status`, `aria-busy=true`, polite live region by default | PASS | PASS | |
| `SfChart` | PASS — series + data-point descriptions announced | PASS | PASS | |

## Known limitations

- Edge + NVDA currently announces the navigated date in
  `SfCalendar` after a brief delay. JAWS / Narrator do not exhibit
  this behaviour.
- `SfUploader`'s native `<input type="file">` button is a software-
  owned name. Welsh/Lithuanian users see "Browse…" — there is no
  supported override at this release.

## How to re-run

To re-verify on a developer's workstation:

```powershell
.\samples\Blazor.Toolkit.Samples\bin\Debug\net10.0\Blazor.Toolkit.Samples.exe
# open Edge, navigate to /accessibility/smoke, run FastPass
# open the bundled Accessibility Insights, run Assessment
```

Contact: open an issue labelled `accessibility` for any new finding.