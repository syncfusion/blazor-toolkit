import { test, expect } from '@playwright/test';

test.describe('SfSpinner – Multiple Instances', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/spinner/multiple');
    await page.waitForSelector('#multiple-ready');
  });

  test('multiple spinner instances render independently', async ({ page }) => {
    await expect(page.locator('.e-spinner-pane')).toHaveCount(3);
  });

});