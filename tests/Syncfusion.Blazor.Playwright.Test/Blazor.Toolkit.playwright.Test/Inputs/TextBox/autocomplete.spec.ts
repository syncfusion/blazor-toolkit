import { test, expect } from '@playwright/test';

test.describe('TextBox - Autocomplete behavior', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/inputs/textbox');
    await page.waitForLoadState('networkidle');
  });

  test('Autocomplete On input has attribute', async ({ page }) => {
    const el = page.locator('#autocompleteOnInput');
    await expect(el).toBeVisible();
    await expect(el).toHaveAttribute('autocomplete', /on|On|ON/i);
  });

  test('Autocomplete Off input has attribute', async ({ page }) => {
    const el = page.locator('#autocompleteOffInput');
    await expect(el).toBeVisible();
    await expect(el).toHaveAttribute('autocomplete', /off|Off|OFF/i);
  });
});
