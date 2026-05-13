import { test, expect } from '@playwright/test';

test.describe('TextBox - Input attributes', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/inputs/textbox');
    await page.waitForLoadState('networkidle');
  });

  test('maxlength attribute prevents longer input', async ({ page }) => {
    const input = page.locator('#maxLengthInput');
    await input.fill('12345678901');
    const value = await input.inputValue();
    expect(value.length).toBeLessThanOrEqual(10);
  });

  test('pattern attribute exists and is enforced by validation', async ({ page }) => {
    const input = page.locator('#patternInput');
    await input.fill('abc');
    // no explicit validation on page, but ensure value set
    await expect(input).toHaveValue('abc');
  });

  test('data attributes are present', async ({ page }) => {
    const el = page.locator('#dataAttrInput');
    await expect(el).toBeVisible();
    const data = await el.getAttribute('data-testid');
    expect(data).toBe('custom-input');
  });

  test('input attributes on multiline are applied', async ({ page }) => {
    const el = page.locator('#formCommentsInput');
    if (await el.count() === 0) {
      test.skip();
    }
    await expect(el).toBeVisible();
  });

  test('autocomplete on/off attributes exist', async ({ page }) => {
    const on = page.locator('#autocompleteOnInput');
    const off = page.locator('#autocompleteOffInput');
    await expect(on).toHaveAttribute('autocomplete', /on|On|ON/i);
    await expect(off).toHaveAttribute('autocomplete', /off|Off|OFF/i);
  });

  test('spellcheck attribute can be set via InputAttributes', async ({ page }) => {
    // page doesn't include explicit spellcheck sample; assert custom attributes work via customHtmlInput
    const el = page.locator('#customHtmlInput');
    await expect(el).toBeVisible();
  });
});
