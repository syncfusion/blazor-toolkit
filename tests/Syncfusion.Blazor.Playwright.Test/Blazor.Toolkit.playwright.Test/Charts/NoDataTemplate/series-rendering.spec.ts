import { test, expect } from '@playwright/test';

test.describe('Chart – No Data Template › Series Rendering', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart/no-data-template');
    await page.waitForLoadState('networkidle');
  });

  test('Column series renders after data load', async ({ page }) => {
    await page.click('#load-data');
    await page.waitForTimeout(300);

    // Column series are rendered as rects
    const columns = page.locator('#chart-host svg rect');
    await expect(columns.first()).toBeVisible();
  });

  test('X-axis labels appear after data load', async ({ page }) => {
    await page.click('#load-data');
    await page.waitForTimeout(300);

    const axisLabels = page.locator('svg text');
    await expect(axisLabels).toContainText(['Jan', 'Feb', 'Mar', 'Apr']);
  });

});