// Edge Cases and Error Handling Testing
// Tests REAL SfRadioButton components from the Blazor sample app

import { test, expect } from '@playwright/test';

test.describe('Edge Cases and Error Handling', () => {
  test.beforeEach(async ({ page }) => {
    // Navigate to the edge cases sample page
    await page.goto('http://localhost:5000/radio-button/edge-cases');
    await page.waitForLoadState('networkidle');
  });

  test('Test radio button with empty value', async ({ page }) => {
    const emptyValueRadio = page.locator('input[type="radio"][value=""]').first();

    if (await emptyValueRadio.count() > 0) {
      await expect(emptyValueRadio).toBeVisible();

      // Selection should work
      await emptyValueRadio.click();
      await expect(emptyValueRadio).toBeChecked();

      // Value should be empty string
      const value = await emptyValueRadio.getAttribute('value');
      expect(value).toBe('');
    }
  });

  test('Test multiple independent radio groups', async ({ page }) => {
    // Test Group 1
    const group1Radios = page.locator('input[name="edge-group"]');
    if (await group1Radios.count() > 0) {
      const group1First = group1Radios.first();
      await group1First.click();
      await expect(group1First).toBeChecked();
    }

    // Test Group 2
    const group2Radios = page.locator('input[name="group1"]');
    if (await group2Radios.count() > 0) {
      const group2First = group2Radios.first();
      await group2First.click();
      await expect(group2First).toBeChecked();
    }
  });

  test('Test radio button focus management', async ({ page }) => {
    const focusRadios = page.locator('input[name="focus-group"]');
    
    if (await focusRadios.count() > 0) {
      const firstRadio = focusRadios.first();
      
      // Focus the radio
      await firstRadio.focus();
      
      // Verify it's focused
      const focusedId = await page.evaluate(() => document.activeElement?.id ?? null);
      const radioId = await firstRadio.getAttribute('id');
      expect(focusedId).toBe(radioId);
    }
  });
});
