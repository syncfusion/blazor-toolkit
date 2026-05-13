import { test, expect } from '@playwright/test';

test.describe('Chart – Exponential Trendline › Axis Rendering', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart/exponential');
    await page.waitForLoadState('networkidle');
  });

  test('Primary X axis title renders', async ({ page }) => {
    await expect(
      page.locator('text=Months')
    ).toBeVisible();
  });

  test('Primary Y axis title renders', async ({ page }) => {
    await expect(
      page.locator('text=Rupees against Dollars')
    ).toBeVisible();
  });

  test('DateTime X-axis labels are rendered', async ({ page }) => {
    const axisLabels = page.locator('svg text');
    await expect(axisLabels).toContainText(['2000', '2001', '2002']);
  });

});
