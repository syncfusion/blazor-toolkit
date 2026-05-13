import { test, expect } from '@playwright/test';

test.describe('TextBox - Events', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/inputs/textbox');
    await page.waitForLoadState('networkidle');
  });

  test('Input event updates the EventMessage area', async ({ page }) => {
    const input = page.locator('#eventTextBox');
    await input.fill('abc');
    // the page updates EventMessage to include the input value; match more loosely and allow time
    await expect(page.locator('text=Input event - Value')).toContainText('abc', { timeout: 3000 });
  });
});
