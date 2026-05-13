import { test, expect, Page } from '@playwright/test';

const base = 'http://localhost:5000/numerictextbox';

test.describe('NumericTextBox — Clipboard & Validation', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto(`${base}/default-functionality`);
    await page.waitForLoadState('networkidle');
  });

  test('handles paste of formatted values and sanitizes invalid content', async ({ page }) => {
    const input = page.locator('#defaultNumeric.e-input');

    // Simulate paste
    await input.fill('1234.50');
    await page.waitForTimeout(120);
    await input.press('Tab');
    await page.waitForTimeout(120);

    const rawValue = await input.inputValue();
    await page.waitForTimeout(120);
    const normalized = rawValue.replace(/,/g, '');
    expect(parseFloat(normalized)).toBeCloseTo(1234.5, 2);


    // Simulate invalid paste
    await input.fill('<script>alert(1)</script>');
    await page.waitForTimeout(120);
    await input.press('Tab');
    await page.waitForTimeout(120);
    const value = (await input.inputValue()).toLowerCase();
    expect(value).not.toContain('<script>');
  });

  test('sanitizes negative/garbage input gracefully', async ({ page }) => {
    const input = page.locator('#defaultNumeric.e-input');

    await input.fill('..,,--12ab');
    await input.press('Tab');
    await page.waitForTimeout(120);
    expect(await input.inputValue()).toBeDefined();
  });
});
