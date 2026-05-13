import { test, expect } from '@playwright/test';

test.describe('Chart – Trendline LineStyle & Legend › Accessibility', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/trendline-line-style-playwright');
    await page.waitForLoadState('networkidle');
  });

  test('Page heading is accessible', async ({ page }) => {
    await expect(
      page.locator('h2')
    ).toHaveText('Trendline – LineStyle & Legend (Playwright Sample)');
  });

});