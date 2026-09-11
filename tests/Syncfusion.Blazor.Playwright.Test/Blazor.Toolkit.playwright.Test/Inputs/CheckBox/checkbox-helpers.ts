// checkbox-helpers.ts
//
// SfCheckBox uses @onclick:preventDefault="true" and drives its state from
// HandleClickAsync. Playwright's Locator.check() / uncheck() click the input
// and then assert the native `checked` property, but they race with the
// Blazor re-render and fail with "Clicking the checkbox did not change its
// state" when the click handler has not yet completed.
//
// These helpers are the minimal replacement: a single .click() that drives
// the Blazor click handler directly, and a tri-state-aware locator-state
// reader used by the intermediate-state assertion in grouping-hierarchy.

import { Locator, expect } from '@playwright/test';

/**
 * Reads the current tri-state of an SfCheckBox.
 * Returns "indeterminate" when aria-checked is "mixed", otherwise the
 * boolean state of the underlying input.
 */
export async function getCheckboxState(
  checkbox: Locator,
): Promise<'checked' | 'unchecked' | 'indeterminate'> {
  const ariaChecked = await checkbox.getAttribute('aria-checked');
  if (ariaChecked === 'mixed') {
    return 'indeterminate';
  }
  const isChecked = await checkbox.isChecked();
  return isChecked ? 'checked' : 'unchecked';
}

/**
 * Ensures the checkbox is in the checked state.
 * If the underlying <input> is not yet checked, performs a single .click()
 * so Blazor's HandleClickAsync drives the state transition.
 */
export async function checkCheckbox(checkbox: Locator): Promise<void> {
  if (!(await checkbox.isChecked())) {
    await checkbox.click();
  }
}

/**
 * Ensures the checkbox is in the unchecked state.
 * If the underlying <input> is currently checked, performs a single
 * .click() so Blazor's HandleClickAsync drives the state transition.
 */
export async function uncheckCheckbox(checkbox: Locator): Promise<void> {
  if (await checkbox.isChecked()) {
    await checkbox.click();
  }
}

/**
 * Asserts the checkbox is in the indeterminate (mixed) state. This is the
 * only state in which SfCheckBox emits aria-checked="mixed".
 */
export async function expectIndeterminate(checkbox: Locator): Promise<void> {
  await expect(checkbox).toHaveAttribute('aria-checked', 'mixed');
}
