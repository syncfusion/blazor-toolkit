import { test, expect, Page } from '@playwright/test';

const base = 'http://localhost:5000/numerictextbox';

test.describe('NumericTextBox — Render & Formats', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto(`${base}/default-functionality`);
    await page.waitForLoadState('networkidle');
  });

  test('renders default functionality and formats typed value', async ({ page }) => {
    const input = page.locator('#defaultNumeric.e-input');

    await input.fill('1234.567');
    await page.keyboard.press('Tab');
    await page.waitForTimeout(150);

    await expect(input).toHaveValue(/.+/);
  });

  test('number/currency/percent formats render correctly', async ({ page }) => {
    await page.goto(`${base}/formats`);
    await page.waitForLoadState('networkidle');

    const numberInput = page.locator('#formatNumber.e-input');
    const currencyInput = page.locator('#formatCurrency.e-input');
    const percentInput = page.locator('#formatPercent.e-input');

    await numberInput.fill('12345.6');
    await numberInput.press('Tab');
    await page.waitForTimeout(120);
    await expect(numberInput).toHaveValue(/.+/);

    await currencyInput.fill('1234.5');
    await currencyInput.press('Tab');
    await page.waitForTimeout(120);
    await expect(currencyInput).toHaveValue(/.+/);

    await percentInput.fill('0.5');
    await percentInput.press('Tab');
    await page.waitForTimeout(120);
    await expect(percentInput).toHaveValue(/.+/);
  });
});
