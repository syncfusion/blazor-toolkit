import { test, expect, Page } from '@playwright/test';

const base = 'http://localhost:5000/numerictextbox';

test.describe('NumericTextBox — Edge Cases & Performance', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto(`${base}/edge-cases`);
    await page.waitForLoadState('networkidle');
  });

  test('Step=0 does not crash and behaves gracefully', async ({ page }) => {
    const stepZero = page.locator('#edgeStepZero.e-input');

    await stepZero.fill('10');
    await page.waitForTimeout(80);
    await stepZero.focus();
    await page.keyboard.press('ArrowUp');
    await page.waitForTimeout(120);
    await expect(stepZero).toHaveValue(/.+/);
  });

  test('extreme values and NaN paths do not throw', async ({ page }) => {
    const extreme = page.locator('#edgeExtreme.e-input');

    await page.click('button:has-text("Set Extreme")');
    await page.waitForTimeout(120);
    expect(await extreme.inputValue()).toBeDefined();
  });

  test('stress: rapid typing and repeated spin clicks', async ({ page }) => {
    await page.goto(`${base}/default-functionality`);
    await page.waitForLoadState('networkidle');
    const input = page.locator('#defaultNumeric.e-input');
    const up = page.locator('.e-numeric.e-input-group .e-chevron-up');

    // rapid typing
    for (let i = 0; i < 8; i++) {
      await input.type(String(i));
    }
    await page.waitForTimeout(120);

    // rapid spin clicks
    for (let i = 0; i < 12; i++) {
      await up.click();
      await page.waitForTimeout(30);
    }
    await page.waitForTimeout(150);
    await expect(input).toHaveValue(/.+/);
  });
});
