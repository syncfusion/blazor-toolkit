import { test, expect } from '@playwright/test';

test.describe('SfSpinner - Styling and Size', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/spinner/styling');
    await page.waitForSelector('#styling-ready');
  });

  test('CssClass is applied to spinner pane', async ({ page }) => {
    await expect(
      page.locator('.e-spinner-pane.custom-spinner')
    ).toHaveCount(1);
  });

  test('ZIndex is applied via inline style', async ({ page }) => {
    const style = await page.locator('.e-spinner-pane').getAttribute('style');
    expect(style).toContain('z-index');
  });

  test('Label is rendered when provided', async ({ page }) => {
    await expect(page.locator('.e-spin-label')).toHaveCount(1);
    await expect(page.locator('.e-spin-label')).toContainText('Processing');
  });

});