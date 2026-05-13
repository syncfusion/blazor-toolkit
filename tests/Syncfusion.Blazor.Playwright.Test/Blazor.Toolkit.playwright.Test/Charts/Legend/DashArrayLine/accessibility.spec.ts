import { test, expect } from '@playwright/test';

test.describe('Chart – DashArray › Accessibility', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart/dasharray/line');
    await page.waitForLoadState('networkidle');
  });

  test('Chart heading is readable by screen readers', async ({ page }) => {
    await expect(
      page.locator('h3')
    ).toHaveText('Chart – DashArray (Line Variants)');
  });

  test('SVG is present in DOM for assistive tools', async ({ page }) => {
    await expect(
      page.locator('#chart-host svg')
    ).toBeVisible();
  });

});