import { test, expect } from '@playwright/test';

test.describe('Chart – No Data Template › Load Data Interaction', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart/no-data-template');
    await page.waitForLoadState('networkidle');
  });

  test('Clicking Load Data hides NoDataTemplate', async ({ page }) => {
    await page.click('#load-data');

    const noDataTemplate = page.locator('.no-data-template');
    await expect(noDataTemplate).toHaveCount(0);
  });

});
