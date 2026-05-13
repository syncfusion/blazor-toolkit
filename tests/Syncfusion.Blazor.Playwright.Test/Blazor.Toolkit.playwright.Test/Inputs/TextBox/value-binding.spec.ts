import { test, expect } from '@playwright/test';

test.describe('TextBox - Value binding and programmatic updates', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/inputs/textbox');
    await page.waitForLoadState('networkidle');
  });

  test('Set programmatic value updates the input', async ({ page }) => {
    await page.click('text=Set Value');
    const input = page.locator('#programmaticInput');
    await expect(input).toHaveValue('Updated value from code');
  });

  test('Clear programmatic value clears the input', async ({ page }) => {
    await page.click('text=Set Value');
    await page.click('text=Clear Value');
    const input = page.locator('#programmaticInput');
    await expect(input).toHaveValue('');
  });
});
