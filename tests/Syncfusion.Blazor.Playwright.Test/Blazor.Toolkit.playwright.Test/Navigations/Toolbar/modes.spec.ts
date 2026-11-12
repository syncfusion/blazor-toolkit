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
});
