import { test, expect, Page } from '@playwright/test';

const base = 'http://localhost:5000/numerictextbox';

test.describe('NumericTextBox — Min/Max/Step and Keyboard', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto(`${base}/min-max-step`);
    await page.waitForLoadState('networkidle');
  });

  test('clamps values outside min/max on blur', async ({ page }) => {
    const input = page.locator('#minmaxNumeric.e-input');

    await page.click('button:has-text("Set above Max")');
    await page.waitForTimeout(120);
    await input.press('Tab');
    await page.waitForTimeout(200);
    const v = await input.inputValue();
    expect(v).toMatch(/100(\.|,)?0*/);
  });

  test('spin buttons respect step and bounds', async ({ page }) => {
    const input = page.locator('#minmaxNumeric.e-input');
    const up = page.locator('.e-numeric.e-input-group .e-chevron-up');
    const down = page.locator('.e-numeric.e-input-group .e-chevron-down');

    await input.fill('98');
    await up.click();
    await page.waitForTimeout(120);
    const v = Number((await input.inputValue()).replace(/,/g, ''));
    expect(v).toBeLessThanOrEqual(100);

    await down.click();
    await page.waitForTimeout(100);
  });

  test('arrow keys increment/decrement by step', async ({ page }) => {
    const input = page.locator('#minmaxNumeric.e-input');

    await input.fill('10');
    await input.focus();
    await page.keyboard.press('ArrowUp');
    await page.waitForTimeout(120);
    const v = Number((await input.inputValue()).replace(/,/g, ''));
    expect(v).toBe(12);
  });
});
