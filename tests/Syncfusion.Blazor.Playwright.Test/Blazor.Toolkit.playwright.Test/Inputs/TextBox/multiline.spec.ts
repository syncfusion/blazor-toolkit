import { test, expect } from '@playwright/test';

test.describe('TextBox - Multiline', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/inputs/textbox');
    await page.waitForLoadState('networkidle');
  });

  test('multiline input renders as a textarea element', async ({ page }) => {
    const tag = await page.$eval('#multilineInput', el => el.tagName);
    expect(tag.toLowerCase()).toBe('textarea');
  });

  test('typing multiple lines preserves line breaks', async ({ page }) => {
    const input = page.locator('#multilineInput');
    const lines = 'Line1\nLine2\nLine3';
    await input.fill(lines);
    const value = await input.inputValue();
    expect(value).toContain('Line2');
  });
});
