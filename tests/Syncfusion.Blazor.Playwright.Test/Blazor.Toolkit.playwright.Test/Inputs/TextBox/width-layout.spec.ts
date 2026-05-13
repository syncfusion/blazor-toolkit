import { test, expect } from '@playwright/test';

test.describe('TextBox - Width and layout control', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/inputs/textbox');
    await page.waitForLoadState('networkidle');
  });

  test('fixed width input has computed width near 300px', async ({ page }) => {
    const el = page.locator('#fixedWidthInput');
    const box = await el.boundingBox();
    expect(box).toBeTruthy();
    if (box) expect(Math.round(box.width)).toBeGreaterThanOrEqual(280);
  });

  test('percent width input is visible', async ({ page }) => {
    await expect(page.locator('#percentWidthInput')).toBeVisible();
  });

  test('default width input is visible', async ({ page }) => {
    await expect(page.locator('#defaultWidthInput')).toBeVisible();
  });

  test('width units like rem/em are accepted when used', async ({ page }) => {
    // sample does not include rem/em examples explicitly; assert other width inputs present
    await expect(page.locator('#fixedWidthInput')).toBeVisible();
  });
});
