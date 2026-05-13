import { test, expect } from '@playwright/test';

test.describe('Chart – No Data Template › Initial No-Data State', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart/no-data-template');
    await page.waitForLoadState('networkidle');
  });

  test('NoDataTemplate content is visible initially', async ({ page }) => {
    const noDataTemplate = page.locator('.no-data-template');
    await expect(noDataTemplate).toBeVisible();
    await expect(noDataTemplate).toContainText(
      'No data available to display.'
    );
  });

  test('Chart title and subtitle still render', async ({ page }) => {
    await expect(
      page.locator('text=Milk Production in US - 2025')
    ).toBeVisible();

    await expect(
      page.locator('text=Jan 2025 - Apr 2025')
    ).toBeVisible();
  });

});
