import { test, expect } from '@playwright/test';

test.describe('Chart – Legend Template › Template Rendering', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart/legend/template');
    await page.waitForLoadState('networkidle');
  });

  test('Custom legend templates are rendered', async ({ page }) => {
    await expect(page.locator('.legend-template.gold')).toBeVisible();
    await expect(page.locator('.legend-template.silver')).toBeVisible();
    await expect(page.locator('.legend-template.bronze')).toBeVisible();
  });

  test('Legend template text renders correctly', async ({ page }) => {
    const templates = page.locator('.legend-template');
    await expect(templates).toContainText([
      'Gold Medals',
      'Silver Medals',
      'Bronze Medals'
    ]);
  });

});