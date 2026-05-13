import { test, expect } from '@playwright/test';

test.describe('Chart – ChartTitleStyle › Multiple Charts', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-title-style-playwright');
    await page.waitForLoadState('networkidle');
  });

  test('Each chart renders its own title independently', async ({ page }) => {
    await expect(
      page.locator('text=Sales Data')
    ).toBeVisible();

    await expect(
      page.locator('text=Revenue Overview')
    ).toBeVisible();

    await expect(
      page.locator('text=Annual Report')
    ).toBeVisible();
  });

});