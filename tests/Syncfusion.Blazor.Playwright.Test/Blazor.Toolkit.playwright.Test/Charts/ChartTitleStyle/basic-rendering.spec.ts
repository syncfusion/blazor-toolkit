import { test, expect } from '@playwright/test';

test.describe('Chart – ChartTitleStyle › Basic Rendering', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-title-style-playwright');
    await page.waitForLoadState('networkidle');
  });

  test('Page heading renders', async ({ page }) => {
    await expect(
      page.locator('h2:has-text("ChartTitleStyle – Playwright Sample")')
    ).toBeVisible();
  });

  test('All chart containers render', async ({ page }) => {
    await expect(page.locator('#chartTitleStyle1')).toBeVisible();
    await expect(page.locator('#chartTitleStyle2')).toBeVisible();
    await expect(page.locator('#chartTitleStyle3')).toBeVisible();
  });

  test('All charts render SVG output', async ({ page }) => {
    const svgs = page.locator('svg');
    expect(await svgs.count()).toBeGreaterThanOrEqual(3);
  });

});
