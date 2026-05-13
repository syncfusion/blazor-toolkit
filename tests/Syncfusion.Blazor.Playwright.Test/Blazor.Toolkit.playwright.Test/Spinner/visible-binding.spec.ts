import { test, expect } from '@playwright/test';

test.describe('SfSpinner – Visible Binding', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/spinner-visible-binding');
    await page.waitForSelector('#visible-binding-ready');
  });

  test('Visible binding does not remove spinner DOM', async ({ page }) => {
    await page.locator('#btn-toggle').click({ force: true });
    await page.locator('#btn-toggle').click({ force: true });

    await expect(page.locator('.e-spinner-pane')).toHaveCount(1);
  });

});