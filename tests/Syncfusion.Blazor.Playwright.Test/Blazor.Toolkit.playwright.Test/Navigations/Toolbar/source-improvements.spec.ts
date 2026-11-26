// Source improvements: RTL icon order stays after click, the bar has no outer border,
// and scrollable navigation animates unless reduced motion is requested.

import { test, expect } from '@playwright/test';
import { openToolbarPage } from './helpers';

test.describe('Toolbar source improvements @toolbar', () => {
  test('TB-RTL-CLICK icon stays after text after click in an RTL toolbar', async ({ page }) => {
    await openToolbarPage(page, '/toolbar/visual');
    const item = page.locator('#vr-new');
    const order = async () => item.evaluate((el) => {
      const icon = el.querySelector('[data-testid="toolbar-item-icon"]');
      const text = el.querySelector('[data-testid="toolbar-item-text"]');
      if (!icon || !text) {
        return 'missing';
      }
      return icon.compareDocumentPosition(text) & Node.DOCUMENT_POSITION_FOLLOWING ? 'icon-then-text' : 'text-then-icon';
    });

    await expect.poll(order).toBe('text-then-icon');
    const boxes = async () => item.evaluate((el) => {
      const icon = el.querySelector('[data-testid="toolbar-item-icon"]')!.getBoundingClientRect();
      const text = el.querySelector('[data-testid="toolbar-item-text"]')!.getBoundingClientRect();
      return { icon: Math.round(icon.x), text: Math.round(text.x) };
    });
    const before = await boxes();
    await item.locator('.e-btn').click();
    await expect.poll(order).toBe('text-then-icon');
    expect(await boxes()).toEqual(before);
  });

  test('TB-VIS-BORDER the toolbar has no outer border', async ({ page }) => {
    await openToolbarPage(page, '/toolbar/visual');
    const border = await page.locator('#toolbar-visual-basic').evaluate((el) => getComputedStyle(el).borderTopWidth);
    expect(border).toBe('0px');
    await page.locator('#shot-basic').screenshot({ path: 'test-results/toolbar-borderless.png' });
  });

  test('TB-SCR-ANIM scrollable click moves the content and reduced motion is immediate', async ({ page }) => {
    await page.setViewportSize({ width: 420, height: 800 });
    await openToolbarPage(page, '/toolbar/visual');
    const scroller = page.locator('#toolbar-visual-scroll [data-testid="toolbar-content"]');
    const end = page.locator('#toolbar-visual-scroll [data-testid="toolbar-scroll-end"]');
    await expect(end).toBeEnabled({ timeout: 8000 });
    await expect(end.locator('.e-btn-icon')).toHaveClass(/e-chevron-right/);

    await end.dispatchEvent('pointerdown');
    await end.dispatchEvent('pointerup');
    await end.dispatchEvent('click');
    await expect.poll(async () => scroller.evaluate((el) => el.scrollLeft), { timeout: 4000 }).toBeGreaterThan(0);

    await page.emulateMedia({ reducedMotion: 'reduce' });
    const start = page.locator('#toolbar-visual-scroll [data-testid="toolbar-scroll-start"]');
    await start.click();
    await expect.poll(async () => scroller.evaluate((el) => Math.abs(el.scrollLeft))).toBeLessThan(2);
  });
});
