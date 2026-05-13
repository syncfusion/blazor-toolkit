import { test, expect } from '@playwright/test';

test.describe('Chart – Linear Trendline › Accessibility', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/linear-trendline-playwright');
    await page.waitForLoadState('networkidle');
  });

  test('Page heading is readable for assistive tools', async ({ page }) => {
    await expect(
      page.locator('h2')
    ).toHaveText('Linear Trendline – Playwright Converted Sample');
  });

});