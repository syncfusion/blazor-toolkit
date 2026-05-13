// Selection States Testing
// Tests REAL SfRadioButton components from the Blazor sample app

import { test, expect } from '@playwright/test';

test.describe('Selection States', () => {
  test.beforeEach(async ({ page }) => {
    // Navigate to the selection states sample page
    await page.goto('http://localhost:5000/radio-button/selection-states');
    await page.waitForLoadState('networkidle');
  });

  test('Test checking an unchecked radio button', async ({ page }) => {
    const radioButtons = page.locator('input[name="selection-group"]');
    const firstButton = radioButtons.first();

    // Initially not checked (unless it's the default selected)
    const initialChecked = await firstButton.isChecked();
    
    if (!initialChecked) {
      // Click the button → becomes checked
      await firstButton.click();
      await expect(firstButton).toBeChecked();
    } else {
      // If already checked, select a different one
      const secondButton = radioButtons.nth(1);
      await secondButton.click();
      await expect(secondButton).toBeChecked();
      await expect(firstButton).not.toBeChecked();
    }
  });

  test('Test mutual exclusivity in radio group', async ({ page }) => {
    const radioButtons = page.locator('input[name="selection-group"]');

    // Check that only one can be selected at a time
    const firstButton = radioButtons.first();
    const secondButton = radioButtons.nth(1);

    // Select first button
    await firstButton.click();
    await expect(firstButton).toBeChecked();
    await expect(secondButton).not.toBeChecked();

    // Select second button
    await secondButton.click();
    await expect(secondButton).toBeChecked();
    await expect(firstButton).not.toBeChecked();
  });

  test('Test selection with reset functionality', async ({ page }) => {
    const resetButton = page.locator('button').filter({ hasText: 'Reset Selection' }).first();
    const selectedOption = page.locator('input[name="selection-group"][value="selected"]');

    // Reset to default
    await resetButton.click();
    await expect(selectedOption).toBeChecked();
  });
});
