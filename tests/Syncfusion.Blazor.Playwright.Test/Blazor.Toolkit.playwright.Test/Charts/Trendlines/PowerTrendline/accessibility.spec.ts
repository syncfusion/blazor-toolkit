import { test, expect } from '@playwright/test';

test.describe('Chart – Power Trendline › Accessibility', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/power-trendline-playwright');
    await page.waitForLoadState('networkidle');
  });

  test('Page heading is accessible', async ({ page }) => {
    await expect(
      page.locator('h2')
    ).toHaveText('Power Trendline – Playwright Sample');
  });

});
