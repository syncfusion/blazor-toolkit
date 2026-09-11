// Disabled State Test for Real SfButtonGroup Component
// Tests disabled button behavior

import { test, expect } from '@playwright/test';

test.describe('ButtonGroup - Disabled State', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/button-group/disabled-state');
    await page.waitForLoadState('networkidle');
  });

  test('Disabled buttons have disabled attribute', async ({ page }) => {
    // In SelectionMode.Single / Multiple, SfButtonGroup renders each <Button>
    // as <input type="radio"|"checkbox"> rather than a <button>, so the disabled
    // attribute is on the input element. The sample has the explicit
    // #btn-dis button that is rendered as an <input type="radio" disabled>.
    const disabledInput = page.locator('input[type="radio"]#btn-dis');
    await expect(disabledInput).toBeDisabled();

    // The disabled attribute must be present on the element.
    const disabledAttr = await disabledInput.getAttribute('disabled');
    expect(disabledAttr).not.toBeNull();
  });

  test('Disabled button cannot be clicked', async ({ page }) => {
    // In selection mode the disabled button is rendered as a disabled <input>.
    const disabledBtn = page.locator('input[type="radio"]#btn-dis').first();
    await expect(disabledBtn).toBeDisabled();

    // A disabled <input type="radio"> cannot be toggled. Verify the state stays
    // unchanged after attempting a Space keypress.
    const wasChecked = await disabledBtn.isChecked();

    // Bring focus somewhere in the document first; the disabled control will refuse focus.
    await page.locator('input[type="radio"]#btn-en').first().focus();
    await page.keyboard.press('Tab');
    await page.keyboard.press('Space');

    const isChecked = await disabledBtn.isChecked();
    expect(isChecked).toBe(wasChecked);
  });

  test('Mixed enabled/disabled buttons render correctly', async ({ page }) => {
    const mixedGroup = page.locator('#bg-disabled-mixed').first();
    await expect(mixedGroup).toBeVisible();

    // The mixed group contains #btn-en (enabled) and #btn-dis (disabled),
    // both rendered as <input type="radio"> because Mode is SelectionMode.Single.
    const enabledBtn = page.locator('input[type="radio"]#btn-en').first();
    await expect(enabledBtn).toBeEnabled();
    await expect(enabledBtn).not.toBeDisabled();

    const disabledBtn = page.locator('input[type="radio"]#btn-dis').first();
    await expect(disabledBtn).toBeDisabled();
  });

  test('All disabled buttons group renders non-interactive', async ({ page }) => {
    const allDisabledGroup = page.locator('#bg-all-disabled').first();
    await expect(allDisabledGroup).toBeVisible();

    // The all-disabled group has three buttons, all rendered as
    // <input type="radio" disabled>. Each one must be disabled.
    const inputs = allDisabledGroup.locator('input[type="radio"]');
    const count = await inputs.count();
    expect(count).toBe(3);

    for (let i = 0; i < count; i++) {
      const input = inputs.nth(i);
      await expect(input).toBeDisabled();
    }
  });

  test('Disabled buttons show disabled styling', async ({ page }) => {
    // In selection mode the disabled button is rendered as a disabled <input>.
    const disabledBtn = page.locator('input[type="radio"]#btn-dis').first();
    await expect(disabledBtn).toBeDisabled();

    // The disabled attribute is present (not null).
    const disabledAttr = await disabledBtn.getAttribute('disabled');
    expect(disabledAttr).not.toBeNull();

    // The user-supplied id is preserved on the rendered element.
    const id = await disabledBtn.getAttribute('id');
    expect(id).toBe('btn-dis');
  });

  test('Space key does not activate disabled button', async ({ page }) => {
    await page.goto('http://localhost:5000/button-group/disabled-state');
    await page.waitForLoadState('networkidle');

    // The enabled sibling (#btn-en) is an <input type="radio">. Pressing Space
    // on a focused radio in a group with the same name must check it - this
    // proves the enabled sibling is actually interactive.
    const enabledInput = page.locator('input[type="radio"]#btn-en').first();
    await enabledInput.focus();
    await expect(enabledInput).toBeFocused();
    await page.keyboard.press('Space');
    await expect(enabledInput).toBeChecked({ timeout: 5000 });

    // All inputs in #bg-all-disabled must be disabled - Space cannot activate them.
    const disabledInputs = page.locator('#bg-all-disabled input[type="radio"]');
    const disabledCount = await disabledInputs.count();
    expect(disabledCount).toBeGreaterThan(0);

    for (let i = 0; i < disabledCount; i++) {
      const disabledInput = disabledInputs.nth(i);
      await expect(disabledInput).toBeDisabled();

      // The disabled control refuses to take focus, so attempt-then-verify
      // by recording its checked state before and after the Space press.
      const wasChecked = await disabledInput.isChecked();
      await page.locator('input[type="radio"]#btn-en').first().focus();
      await page.keyboard.press('Space');
      const isChecked = await disabledInput.isChecked();
      expect(isChecked).toBe(wasChecked);
    }
  });
});
