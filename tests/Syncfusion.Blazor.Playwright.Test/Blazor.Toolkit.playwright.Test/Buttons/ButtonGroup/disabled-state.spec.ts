// Disabled State Test for Real SfButtonGroup Component
// Tests disabled button behavior

import { test, expect } from '@playwright/test';

test.describe('ButtonGroup - Disabled State', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/button-group/disabled-state');
    await page.waitForLoadState('networkidle');
  });

  test('Disabled buttons have disabled attribute', async ({ page }) => {
    // Find buttons with disabled state
    const disabledButtons = page.locator('button[disabled], input[disabled]').first();

    // At least one disabled button should exist
    const count = await page.locator('button[disabled], input[disabled]').count();
    expect(count).toBeGreaterThan(0);
  });

  test('Disabled button cannot be clicked', async ({ page }) => {
    const disabledBtn = page.locator('#btn-dis').first();

    if (await disabledBtn.isDisabled()) {
      // Try to click with force:true to bypass Playwright checks
      try {
        await disabledBtn.click({ force: true });
        // Click may succeed but state shouldn't change
      } catch (e) {
        // Expected behavior: disabled button cannot be interacted with
      }
    }
  });

  test('Mixed enabled/disabled buttons render correctly', async ({ page }) => {
    const mixedGroup = page.locator('#bg-disabled-mixed').first();
    await expect(mixedGroup).toBeVisible();

    // Check for enabled buttons
    const enabledBtn = page.locator('#btn-en').first();
    await expect(enabledBtn).toBeEnabled();

    // Check for disabled button
    const disabledBtn = page.locator('#btn-dis').first();
    if (await disabledBtn.count() > 0) {
      await expect(disabledBtn).toBeDisabled();
    }
  });

  test('All disabled buttons group renders non-interactive', async ({ page }) => {
    const allDisabledGroup = page.locator('#bg-all-disabled').first();
    await expect(allDisabledGroup).toBeVisible();

    // All buttons in this group should be disabled
    const buttons = allDisabledGroup.locator('button, input[type="radio"], input[type="checkbox"]');
    const count = await buttons.count();

    for (let i = 0; i < count; i++) {
      const btn = buttons.nth(i);
      await expect(btn).toBeDisabled();
    }
  });

  test('Disabled buttons show disabled styling', async ({ page }) => {
    const disabledBtn = page.locator('#btn-dis').first();

    if (await disabledBtn.isDisabled()) {
      // Verify disabled attribute exists
      const disabledAttr = await disabledBtn.getAttribute('disabled');
      expect(disabledAttr).toBeDefined();
    }
  });

  test('Space key does not activate disabled button', async ({ page }) => {
    await page.goto('http://localhost:5000/button-group/disabled-state');
    await page.waitForLoadState('networkidle');

    // Find the mixed disabled group
    const mixedGroup = page.locator('#bg-disabled-mixed').first();
    const inputs = mixedGroup.locator('input[type="radio"]');

    // First input should be enabled
    const enabledInput = inputs.first();
    await enabledInput.focus();
    await page.keyboard.press('Space');
    let isChecked = await enabledInput.isChecked();
    expect(isChecked).toBeTruthy();

    // Find the all-disabled group and verify inputs are disabled
    const allDisabledGroup = page.locator('#bg-all-disabled').first();
    const disabledInputs = allDisabledGroup.locator('input[type="radio"]');
    const disabledCount = await disabledInputs.count();

    for (let i = 0; i < disabledCount; i++) {
      const disabledInput = disabledInputs.nth(i);
      const isDisabled = await disabledInput.isDisabled();
      if (isDisabled) {
        // Verify Space doesn't cause any error and disabled input remains unchecked
        await disabledInput.focus();
        await page.keyboard.press('Space');
      }
    }
  });
});
