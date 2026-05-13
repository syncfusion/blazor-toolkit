import { test, expect } from '@playwright/test';

test.describe('Chart Marker API – Accessibility', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart/marker/api');
    await page.waitForLoadState('networkidle');
  });

  test('Button is keyboard accessible', async ({ page }) => {
    await page.focus('#update-marker');
    await page.keyboard.press('Enter');
  });

  test('Chart SVG has semantic presence', async ({ page }) => {
    const svg = page.locator('svg').first();
    await expect(svg).toBeVisible();
  });

});