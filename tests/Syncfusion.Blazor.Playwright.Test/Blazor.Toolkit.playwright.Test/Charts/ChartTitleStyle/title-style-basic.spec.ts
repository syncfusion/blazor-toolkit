import { test, expect } from '@playwright/test';

test.describe('Chart – ChartTitleStyle › Basic Properties', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-title-style-playwright');
    await page.waitForLoadState('networkidle');
  });

  test('First chart title renders with custom styling', async ({ page }) => {
    const title = page
      .locator('#chartTitleStyle1 svg text')
      .filter({ hasText: 'Sales Data' });

    await expect(title).toBeVisible();

    const color = await title.first().evaluate(el => getComputedStyle(el).fill);
    expect(color).toBeTruthy();
  });

});
