import { test, expect } from '@playwright/test';

test.describe('Chart – Crosshair › Interaction', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart/crosshair');
    await page.waitForLoadState('networkidle');
  });

  test('Crosshair updates without breaking when mouse moves', async ({ page }) => {
    const svg = page.locator('#chart-host svg');
    await expect(svg).toBeVisible();

    await svg.hover({ position: { x: 150, y: 220 } });
    await svg.hover({ position: { x: 400, y: 220 } });

    // Stability check — SVG remains rendered
    await expect(svg).toBeVisible();
  });

});