import { test, expect } from '@playwright/test';

test.describe('Chart – No Data Template › Transition Validation', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart/no-data-template');
    await page.waitForLoadState('networkidle');
  });

  test('Chart does not break when transitioning from empty to populated data', async ({ page }) => {
    await page.click('#load-data');
    await page.waitForTimeout(300);

    const svg = page.locator('#chart-host svg');
    await expect(svg).toBeVisible();
  });

  test('NoDataTemplate does not reappear after data load', async ({ page }) => {
    await page.click('#load-data');
    await page.waitForTimeout(300);

    await expect(page.locator('.no-data-template')).toHaveCount(0);
  });

});