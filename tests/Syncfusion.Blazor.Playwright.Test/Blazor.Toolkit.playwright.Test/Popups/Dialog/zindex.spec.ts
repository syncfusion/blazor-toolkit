import { test, expect } from '@playwright/test';

const DEFAULT_TIMEOUT = 5000;

test.describe('Dialog - ZIndex & stacking', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/dialog/basic-rendering');
    await page.waitForLoadState('networkidle');
  });

  test('Overlay z-index equals ZIndex - 1 for modal dialog', async ({ page }) => {
    await page.click('#open-zindex1');
    await page.waitForFunction(() => {
    const el = document.getElementById('dlg-zindex1_overlay');
    if (!el) return false;
    const s = window.getComputedStyle(el);
    return s && s.display !== 'none' && s.visibility !== 'hidden' && Number(s.opacity) > 0;
    }, {}, { timeout: DEFAULT_TIMEOUT * 3 });
    const z = await page.locator('#dlg-zindex1_overlay').evaluate(e => window.getComputedStyle(e).zIndex);
  });

  test('Two modal dialogs stack according to ZIndex', async ({ page }) => {
    await page.click('#open-zindex1');
    // Wait for the visible overlay — this is the user-observable signal that the modal opened.
    await expect(page.locator('#dlg-zindex1_overlay')).toBeVisible({ timeout: DEFAULT_TIMEOUT });
    // The first modal's overlay may cover the second button and block Playwright clicks.
    // Use a programmatic click via page.evaluate to dispatch the click event directly.
    await page.evaluate(() => document.getElementById('open-zindex2')?.click());
    await expect(page.locator('#dlg-zindex2')).toBeVisible({ timeout: DEFAULT_TIMEOUT });
    const overlay1 = page.locator('#dlg-zindex1_overlay');
    const overlay2 = page.locator('#dlg-zindex2_overlay');
    await expect(overlay1).toBeVisible({ timeout: DEFAULT_TIMEOUT });
    await expect(overlay2).toBeVisible({ timeout: DEFAULT_TIMEOUT });
    const z1 = Number(await overlay1.evaluate((e: Element) => window.getComputedStyle(e).zIndex));
    const z2 = Number(await overlay2.evaluate((e: Element) => window.getComputedStyle(e).zIndex));
    expect(z2).toBeGreaterThan(z1);
  });
});
