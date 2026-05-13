// Label Placement and Positioning Testing
// Tests REAL SfRadioButton components from the Blazor sample app

import { test, expect } from '@playwright/test';

test.describe('Label Placement and Positioning', () => {
  test.beforeEach(async ({ page }) => {
    // Navigate to the label placement sample page
    await page.goto('http://localhost:5000/radio-button/label-placement');
    await page.waitForLoadState('networkidle');
  });

  test('Test label position After (default/right positioning)', async ({ page }) => {
    // Find the "Label After (Default)" radio button by its label text
    const afterLabel = page.locator('label').filter({ hasText: 'Label After (Default)' });
    await expect(afterLabel).toBeVisible();

    // Get the associated input from the wrapper
    const afterWrapper = afterLabel.locator('..');
    const afterInput = afterWrapper.locator('input[type="radio"]');
    await expect(afterInput).toBeVisible();

    // Verify proper association
    const inputId = await afterInput.getAttribute('id');
    const labelFor = await afterLabel.getAttribute('for');
    expect(inputId).toBe(labelFor);
  });

  test('Test label position Before (left positioning)', async ({ page }) => {
    // Find the "Label Before" radio button by its label text
    const beforeLabel = page.locator('label').filter({ hasText: 'Label Before' });
    await expect(beforeLabel).toBeVisible();

    // Get the associated input from the wrapper
    const beforeWrapper = beforeLabel.locator('..');
    const beforeInput = beforeWrapper.locator('input[type="radio"]');
    await expect(beforeInput).toBeVisible();

    // Verify proper association
    const inputId = await beforeInput.getAttribute('id');
    const labelFor = await beforeLabel.getAttribute('for');
    expect(inputId).toBe(labelFor);
  });

  test('Test clickable label selects radio', async ({ page }) => {
    const afterLabel = page.locator('label').filter({ hasText: 'Label After (Default)' }).first();
    const afterWrapper = afterLabel.locator('..');
    const correspondingRadio = afterWrapper.locator('input[type="radio"]').first();

    // Click label
    await afterLabel.click();

    // Radio should be checked
    await expect(correspondingRadio).toBeChecked();
  });

  test('Test label customization with special characters', async ({ page }) => {
    // Test label with special characters
    const specialLabel = page.locator('label').filter({ hasText: /special characters/ });
    await expect(specialLabel).toBeVisible();

    const correspondingRadio = specialLabel.locator('..');
    if (await correspondingRadio.count() > 0) {
      await specialLabel.click();
      const input = correspondingRadio.locator('input[type="radio"]');
      await expect(input).toBeChecked();
    }
  });
});
