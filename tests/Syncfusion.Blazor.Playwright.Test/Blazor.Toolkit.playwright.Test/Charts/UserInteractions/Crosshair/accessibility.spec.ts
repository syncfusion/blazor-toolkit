import { test, expect } from '@playwright/test';

test.describe('Chart – Crosshair › Accessibility', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart/crosshair');
    await page.waitForLoadState('networkidle');
  });

  test('Chart heading is accessible', async ({ page }) => {
    await expect(
      page.locator('h3')
    ).toHaveText('Chart – Crosshair');
  });

});