import { test, expect } from '@playwright/test';

test.describe('Chart – ChartTitleStyle › Custom Position', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-title-style-playwright');
    await page.waitForLoadState('networkidle');
  });

  test('Second chart title renders with custom position', async ({ page }) => {
    const title = page
      .locator('#chartTitleStyle2 svg text')
      .filter({ hasText: 'Revenue Overview' });

    await expect(title).toBeVisible();

    const x = await title.first().getAttribute('x');
    const y = await title.first().getAttribute('y');

    expect(x).not.toBeNull();
    expect(y).not.toBeNull();
  });

});
