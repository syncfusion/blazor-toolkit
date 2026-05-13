import { test, expect } from '@playwright/test';

test.describe('Chart – Power Trendline › Basic Rendering', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/power-trendline-playwright');
    await page.waitForLoadState('networkidle');
  });

  test('Page heading renders correctly', async ({ page }) => {
    await expect(
      page.locator('h2')
    ).toHaveText('Power Trendline – Playwright Sample');
  });

  test('All charts render SVG output', async ({ page }) => {
    const svgs = page.locator('svg');
    expect(await svgs.count()).toBeGreaterThanOrEqual(9);
  });

});