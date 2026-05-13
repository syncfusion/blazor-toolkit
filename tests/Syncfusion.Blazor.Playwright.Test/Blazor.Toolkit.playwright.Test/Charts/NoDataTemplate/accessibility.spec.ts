import { test, expect } from '@playwright/test';

test.describe('Chart – No Data Template › Accessibility', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart/no-data-template');
    await page.waitForLoadState('networkidle');
  });

  test('Load Data button is keyboard accessible', async ({ page }) => {
    await page.focus('#load-data');
    await page.keyboard.press('Enter');
  });

  test('NoDataTemplate content is readable text', async ({ page }) => {
    await expect(
      page.locator('.no-data-template strong')
    ).toHaveText('No data available to display.');
  });

});
