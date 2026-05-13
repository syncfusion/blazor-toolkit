import { test, expect } from '@playwright/test';

test.describe('Chart – DashArray › Axis Rendering', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart/dasharray/line');
    await page.waitForLoadState('networkidle');
  });

  test('Primary X axis labels render', async ({ page }) => {
    const axisLabels = page.locator('svg text');
    await expect(axisLabels).toContainText(['Jan', 'Feb', 'Mar']);
  });

  test('Primary Y axis range is applied', async ({ page }) => {
    const axisLabels = page.locator('svg text');
    await expect(axisLabels).toContainText(['0', '50', '100', '150', '200']);
  });

});