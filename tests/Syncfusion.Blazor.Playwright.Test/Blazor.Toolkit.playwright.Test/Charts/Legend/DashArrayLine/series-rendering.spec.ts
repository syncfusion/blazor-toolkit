import { test, expect } from '@playwright/test';

test.describe('Chart – DashArray (Line Variants) › Series Rendering', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart/dasharray/line');
    await page.waitForLoadState('networkidle');
  });

  test('Stacking and stacking‑100 line variants render without breaking', async ({ page }) => {
    const seriesPaths = page.locator('#chart-host svg path.e-series');

    // Validate that all rendered series are visible
    const count = await seriesPaths.count();

    for (let i = 0; i < count; i++) {
      await expect(seriesPaths.nth(i)).toBeVisible();
    }
  });

});