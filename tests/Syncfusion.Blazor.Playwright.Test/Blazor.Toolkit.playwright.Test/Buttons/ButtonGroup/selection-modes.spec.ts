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
});
