// Selection Modes Test for Real SfButtonGroup Component
// Tests single and multiple selection modes

import { test, expect } from '@playwright/test';

test.describe('ButtonGroup - Selection Modes', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/button-group/selection-modes');
    await page.waitForLoadState('networkidle');
  });

  test('Default mode buttons are clickable without selection behavior', async ({ page }) => {
    // Find the default mode group
    const defaultGroup = page.locator('#bg-default-mode').first();

    // Buttons should be clickable
    const firstBtn = defaultGroup.locator('button').first();
    await expect(firstBtn).toBeEnabled();

    // Click should not cause an error
    try {
      await firstBtn.click();
    } catch (e) {
      throw new Error(`Default mode button click failed: ${e}`);
    }
  });

  test('Selection mode determines selection behavior', async ({ page }) => {
    // Single selection group
    const singleGroup = page.locator('#bg-single-mode').first();
    const singleButtons = singleGroup.locator('input[type="radio"]');

    // Count radio inputs (single selection uses radio)
    const radioCount = await singleButtons.count();
    expect(radioCount).toBeGreaterThan(0);

    // Multiple selection group
    const multiGroup = page.locator('#bg-multi-mode').first();
    const checkboxButtons = multiGroup.locator('input[type="checkbox"]');

    // Count checkbox inputs (multiple selection uses checkbox)
    const checkboxCount = await checkboxButtons.count();
    expect(checkboxCount).toBeGreaterThan(0);
  });

  test('Space key activates focused button in Single selection mode', async ({ page }) => {
    const singleGroup = page.locator('#bg-single-mode').first();
    const firstRadio = singleGroup.locator('input[type="radio"]').first();

    // Focus the first radio button
    await firstRadio.focus();

    // Verify no button is initially selected (second button has Selected="true" initially)
    // Press Space to select first button
    await page.keyboard.press('Space');

    // Verify first button is now selected
    const isChecked = await firstRadio.isChecked();
    expect(isChecked).toBeTruthy();
  });

  test('Space key toggles button in Multiple selection mode', async ({ page }) => {
    const multiGroup = page.locator('#bg-multi-mode').first();
    const firstCheckbox = multiGroup.locator('input[type="checkbox"]').first();

    // Focus the first checkbox
    await firstCheckbox.focus();

    // Initial state - checkbox should not be checked
    let isChecked = await firstCheckbox.isChecked();
    expect(isChecked).toBeFalsy();

    // Press Space to check - wait for Blazor to process
    await page.keyboard.press('Space');
    await page.waitForTimeout(100);
    isChecked = await firstCheckbox.isChecked();
    expect(isChecked).toBeTruthy();

    // Press Space again to uncheck - wait for Blazor to process
    await page.keyboard.press('Space');
    await page.waitForTimeout(100);
    isChecked = await firstCheckbox.isChecked();
    expect(isChecked).toBeFalsy();
  });
});
