import { test, expect, Page } from '@playwright/test';

const base = 'http://localhost:5000/numerictextbox';

test.describe('NumericTextBox — Events & Data Binding', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto(`${base}/default-functionality`);
    await page.waitForLoadState('networkidle');
  });

  test('programmatic set and binding update', async ({ page }) => {
    const input = page.locator('#defaultNumeric.e-input');
    const log = page.locator('#value-log');

    await page.click('button:has-text("Set value programmatically")');
    await page.waitForTimeout(150);
    await expect(log).toContainText('1234.5');
    await expect(input).toHaveValue(/.+/);
  });

  test('data-binding sample Increment and Set null', async ({ page }) => {
    await page.goto(`${base}/data-binding`);
    await page.waitForLoadState('networkidle');
    const input = page.locator('#bindingNumeric.e-input');
    const log = page.locator('#binding-log');

    await expect(log).toContainText('10');
    await page.click('button:has-text("Increment")');
    await page.waitForTimeout(120);
    await expect(log).toContainText('11');

    await page.click('button:has-text("Set null")');
    await page.waitForTimeout(120);
    await expect(log).toContainText('(null)');
  });
});
