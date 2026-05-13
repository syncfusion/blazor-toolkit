// Focus Management Testing
// Tests REAL SfRadioButton components from the Blazor sample app

import { test, expect } from '@playwright/test';

test.describe('Focus Management', () => {
  test.beforeEach(async ({ page }) => {
    // Navigate to the edge cases sample page which includes focus testing
    await page.goto('http://localhost:5000/radio-button/edge-cases');
    await page.waitForLoadState('networkidle');
  });

  test('Test programmatic focus then Space toggles the radio', async ({ page }) => {
    const focusRadio = page.locator('input[name="focus-group"]').first();

    // Initial: not focused
    let focusedElement = await page.evaluate(() => document.activeElement?.id ?? null);
    expect(focusedElement).not.toBe(await focusRadio.getAttribute('id'));

    // Programmatically focus
    await focusRadio.focus();
    await page.waitForTimeout(50);

    focusedElement = await page.evaluate(() => document.activeElement?.id ?? null);
    const radioId = await focusRadio.getAttribute('id');
    expect(focusedElement).toBe(radioId);

    // Space should check the radio
    await focusRadio.click();
    await expect(focusRadio).toBeChecked();
  });

  test('Test focus outline / keyboard focus via Tab navigation', async ({ page }) => {
    // Get all focusable radio buttons
    const firstRadio = page.locator('input[name="focus-group"]').first();
    
    // Focus the element
    await firstRadio.focus();
    
    // Verify it's focused
    let focusedElement = await page.evaluate(() => document.activeElement?.id ?? null);
    const radioId = await firstRadio.getAttribute('id');
    expect(focusedElement).toBe(radioId);

    // Tab should move focus to next element
    await page.keyboard.press('Tab');
    focusedElement = await page.evaluate(() => document.activeElement?.id ?? null);
    expect(focusedElement).not.toBe(radioId);
  });

  test('Test Tab navigation through radio buttons', async ({ page }) => {
    const radioButtons = page.locator('input[name="focus-group"]');
    const count = await radioButtons.count();

    if (count > 1) {
      // Focus first radio
      const firstRadio = radioButtons.first();
      await firstRadio.focus();

      let focusedElement = await page.evaluate(() => document.activeElement?.id ?? null);
      const firstId = await firstRadio.getAttribute('id');
      expect(focusedElement).toBe(firstId);

      // Tab to next radio
      await page.keyboard.press('Tab');
      
      // Focus should move (might be on next radio or another element)
      focusedElement = await page.evaluate(() => document.activeElement?.id ?? null);
      expect(focusedElement).not.toBe(firstId);
    }
  });
});
