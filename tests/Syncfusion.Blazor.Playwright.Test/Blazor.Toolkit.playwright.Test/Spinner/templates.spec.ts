import { test, expect } from '@playwright/test';

test.describe('SfSpinner – Templates', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/spinner/templates');
    await page.waitForSelector('#templates-ready');
  });

  test('custom spinner template renders', async ({ page }) => {
    await expect(page.locator('.sf-spin-custom')).toHaveCount(1);
  });

  test('default SVG is not rendered inside templated spinner', async ({ page }) => {
    const template = page.locator('.sf-spin-custom');
    await expect(template.locator('svg')).toHaveCount(0);
  });

});