import { test, expect } from '@playwright/test';

test.describe('Chart – ChartTitleStyle › Page Accessibility', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-title-style-playwright');
    await page.waitForLoadState('networkidle');
  });

  test('Headings are readable for assistive technologies', async ({ page }) => {
    await expect(
      page.locator('h2')
    ).toHaveText('ChartTitleStyle – Playwright Sample');
  });

});