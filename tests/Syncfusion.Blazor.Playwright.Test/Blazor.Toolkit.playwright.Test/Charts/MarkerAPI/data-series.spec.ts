import { test, expect } from '@playwright/test';

test.describe('Chart Marker API – Series & Data', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart/marker/api');
    await page.waitForLoadState('networkidle');
  });

  test('Line series is rendered', async ({ page }) => {
    // Paths represent line series
    const paths = page.locator('svg path');
    expect(await paths.count()).toBeGreaterThan(0);
  });

});
