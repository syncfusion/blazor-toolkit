// P-TB-031. MultiRow wraps. Extended reveals the second row from an expand control.

import { test, expect } from '@playwright/test';
import { openToolbarPage } from './helpers';

test.describe('Toolbar overflow modes @toolbar', () => {
  test('P-TB-031 MultiRow wraps and Extended expands', async ({ page }) => {
    await page.setViewportSize({ width: 768, height: 800 });
    await openToolbarPage(page, '/toolbar/modes');

    const multi = page.locator('#toolbar-multirow');
    await expect(multi).toHaveAttribute('data-overflow-mode', 'MultiRow');
    await expect(multi.locator('[data-testid="toolbar-overflow-button"]')).toHaveCount(0);
    const wrap = await multi.evaluate((el) => getComputedStyle(el).flexWrap);
    expect(wrap).toBe('wrap');

    const extended = page.locator('#toolbar-extended');
    const expand = extended.locator('[data-testid="toolbar-expand-button"]');
    await expect(expand).toHaveCount(1);
    const before = await extended.evaluate((el) => el.getBoundingClientRect().height);
    await expand.click();
    await expect.poll(async () => extended.evaluate((el) => el.getBoundingClientRect().height)).toBeGreaterThan(before);
  });

  test('vertical Extended background covers wrapped columns and resizes with available height', async ({ page }) => {
    await page.setViewportSize({ width: 768, height: 800 });
    await openToolbarPage(page, '/toolbar/modes');

    const frame = page.locator('#frame-extended-vertical');
    const toolbar = page.locator('#toolbar-extended-vertical');
    const expand = toolbar.locator('[data-testid="toolbar-expand-button"]');
    await expand.click();

    const layout = () => toolbar.evaluate((root) => {
      const items = Array.from(root.querySelectorAll<HTMLElement>('.e-toolbar-content .e-toolbar-item'))
        .filter((item) => getComputedStyle(item).display !== 'none');
      const rootRect = root.getBoundingClientRect();
      const itemRects = items.map((item) => item.getBoundingClientRect());

      return {
        background: getComputedStyle(root).backgroundColor,
        width: rootRect.width,
        rightmostItem: Math.max(...itemRects.map((rect) => rect.right)),
        columns: new Set(itemRects.map((rect) => Math.round(rect.left))).size
      };
    });

    await expect.poll(async () => {
      const result = await layout();
      return result.columns >= 2 && result.rightmostItem <= (await toolbar.evaluate((root) => root.getBoundingClientRect().right)) + 1;
    }).toBe(true);

    const shortLayout = await layout();
    expect(shortLayout.background).not.toBe('rgba(0, 0, 0, 0)');

    await frame.evaluate((element) => { element.style.height = '640px'; });
    await expect.poll(async () => (await layout()).width).toBeLessThan(shortLayout.width);
  });
});
