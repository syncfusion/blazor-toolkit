import { test, expect } from '@playwright/test';

test.describe('TextBox - Clear button', () => {
  test.beforeEach(async ({ page }) => {    
    await page.goto('http://localhost:5000/inputs/textbox');
    await page.waitForLoadState('networkidle');
  });

  test('clear button appears when typing and clears the value when clicked', async ({ page }) => {
    const input = page.locator('#clearButtonInput');
    await input.fill('to be cleared');
    // locate a clear icon inside the input's ancestor
    const wrapper = input.locator('..');
    const clearIcon = wrapper.locator('.e-close');
    await expect(clearIcon).toBeVisible({ timeout: 2000 });
    await clearIcon.click();
    await expect(input).toHaveValue('');
  });
});
