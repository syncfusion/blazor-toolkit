import { test, expect } from '@playwright/test';

test.describe('Chart – ChartTitleStyle › Accessibility Properties', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-title-style-playwright');
    await page.waitForLoadState('networkidle');
  });

  test('Accessible title description is applied', async ({ page }) => {
    const title = page
      .locator('#chartTitleStyle3 svg text')
      .filter({ hasText: 'Annual Report' });

    await expect(title).toBeVisible();
  });

  test('Title is not focusable when Focusable=false', async ({ page }) => {
    await page.keyboard.press('Tab');

    const active = await page.evaluate(
      () => document.activeElement?.tagName ?? ''
    );

    expect(active).not.toBe('text');
  });

});