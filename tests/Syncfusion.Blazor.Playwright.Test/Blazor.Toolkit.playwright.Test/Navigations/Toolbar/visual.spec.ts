// Visual baselines for the Toolbar (V-TB-001 to V-TB-015). Chromium is the committed baseline.
// The reference image is a quality target, not a pixel spec.

import { test, expect, type Locator, type Page } from '@playwright/test';
import { openToolbarPage } from './helpers';

async function shot(page: Page, locator: Locator, name: string) {
  await expect(locator).toBeVisible();
  await page.evaluate(() => document.fonts.ready);
  await expect(locator).toHaveScreenshot(name, {
    animations: 'disabled',
    caret: 'hide',
    maxDiffPixelRatio: 0.01,
  });
}

test.describe('Toolbar visual baselines @toolbar', () => {
  test.beforeEach(async ({ page }) => {
    await openToolbarPage(page, '/toolbar/visual');
  });

  test('V-TB-001 toolbar-basic-horizontal', async ({ page }) => {
    await shot(page, page.locator('#shot-basic'), 'toolbar-basic-horizontal.png');
  });

  test('V-TB-014 toolbar-empty', async ({ page }) => {
    await shot(page, page.locator('#shot-empty'), 'toolbar-empty.png');
  });

  test('V-TB-002 toolbar-vertical', async ({ page }) => {
    await shot(page, page.locator('#shot-vertical'), 'toolbar-vertical.png');
  });

  test('V-TB-003 toolbar-overflow-popup', async ({ page }) => {
    const more = page.locator('#toolbar-visual-overflow [data-testid="toolbar-overflow-button"]');
    await expect(more).toHaveCount(1, { timeout: 8000 });
    await more.click();
    await expect(page.locator('#toolbar-visual-overflow [data-testid="toolbar-overflow-popup"]')).toBeVisible();
    await shot(page, page.locator('#shot-overflow'), 'toolbar-overflow-popup.png');
  });

  test('V-TB-004 toolbar-overflow-scrollable', async ({ page }) => {
    await expect(page.locator('#toolbar-visual-scroll [data-testid="toolbar-overflow-button"]')).toHaveCount(0);
    await shot(page, page.locator('#shot-scroll'), 'toolbar-overflow-scrollable.png');
  });

  test('V-TB-006 toolbar-disabled', async ({ page }) => {
    await shot(page, page.locator('#shot-disabled'), 'toolbar-disabled.png');
  });

  test('V-TB-007 toolbar-rtl', async ({ page }) => {
    await shot(page, page.locator('#shot-rtl'), 'toolbar-rtl.png');
  });

  test('V-TB-009 toolbar-dark', async ({ page }) => {
    await shot(page, page.locator('#shot-dark'), 'toolbar-dark.png');
  });

  test('V-TB-008 toolbar-highcontrast', async ({ page }) => {
    await page.emulateMedia({ forcedColors: 'active' });
    await shot(page, page.locator('#shot-hc'), 'toolbar-highcontrast.png');
  });

  test('V-TB-010 toolbar-mobile', async ({ page }) => {
    await page.setViewportSize({ width: 375, height: 700 });
    await shot(page, page.locator('#shot-mobile'), 'toolbar-mobile.png');
  });

  test('V-TB-005 toolbar-multirow', async ({ page }) => {
    const toolbar = page.locator('#toolbar-visual-multirow');
    const itemCount = await toolbar.locator('.e-toolbar-item').count();
    expect(itemCount).toBe(5);
    const rowCount = await toolbar.locator('.e-toolbar-item').evaluateAll((items) =>
      new Set(items.map((item) => Math.round(item.getBoundingClientRect().top))).size);
    expect(rowCount).toBeGreaterThan(1);
    await shot(page, page.locator('#shot-multirow'), 'toolbar-multirow.png');
    await shot(page, page.locator('#shot-extended'), 'toolbar-extended.png');
  });

  test('V-TB-011 toolbar-with-menus', async ({ page }) => {
    await page.locator('#toolbar-visual-menus [data-testid="toolbar-split-toggle"]').click();
    await expect(page.locator('#toolbar-visual-menus [data-testid="toolbar-nested-popup"]')).toBeVisible();
    await shot(page, page.locator('#shot-menus'), 'toolbar-with-menus.png');
  });

  test('V-TB-013 toolbar-groups', async ({ page }) => {
    await shot(page, page.locator('#shot-groups'), 'toolbar-groups.png');
  });

  test('V-TB-015 toolbar-all-overflowed', async ({ page }) => {
    const more = page.locator('#toolbar-visual-all [data-testid="toolbar-overflow-button"]');
    await expect(more).toHaveCount(1, { timeout: 8000 });
    await more.click();
    await expect(page.locator('#toolbar-visual-all [data-testid="toolbar-overflow-popup"]')).toBeVisible();
    await shot(page, page.locator('#shot-all-overflow'), 'toolbar-all-overflowed.png');
  });

  test('V-TB-012 toolbar-templates', async ({ page }) => {
    await shot(page, page.locator('#shot-template'), 'toolbar-templates.png');
  });

  test('P-TB-026 dark mode changes the toolbar surface color', async ({ page }) => {
    const light = await page.locator('#toolbar-visual-basic').evaluate((el) => getComputedStyle(el).backgroundColor);
    const dark = await page.locator('#toolbar-visual-dark').evaluate((el) => getComputedStyle(el).backgroundColor);
    expect(dark).not.toBe(light);
  });

  test('P-TB-027 forced colors keep a visible toolbar border', async ({ page }) => {
    await page.emulateMedia({ forcedColors: 'active' });
    const border = await page.locator('#toolbar-visual-hc').evaluate((el) => getComputedStyle(el).borderTopColor);
    expect(border).not.toBe('rgba(0, 0, 0, 0)');
    expect(border.length).toBeGreaterThan(0);
  });
});
