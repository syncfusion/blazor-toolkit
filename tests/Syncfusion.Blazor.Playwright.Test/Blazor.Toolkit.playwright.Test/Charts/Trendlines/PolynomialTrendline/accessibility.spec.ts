import { test, expect } from '@playwright/test';

test.describe('Chart – Polynomial Trendline › Accessibility', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/polynomial-trendline-playwright');
    await page.waitForLoadState('networkidle');
  });

  test('Page heading is accessible', async ({ page }) => {
    await expect(
      page.locator('h2')
    ).toHaveText('Polynomial Trendline – Playwright Sample');
  });

});