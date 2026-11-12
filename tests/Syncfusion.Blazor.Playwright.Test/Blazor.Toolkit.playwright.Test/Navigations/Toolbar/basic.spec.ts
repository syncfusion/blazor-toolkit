// Toolbar structure, mouse and lifecycle tests. Drives the REAL component from the Blazor sample host
// (Components/Pages/Navigations/Toolbar/Basic.razor).

import { test, expect } from '@playwright/test';
import { openToolbarPage, tabIndexes } from './helpers';

const TB = '#toolbar-basic';

test.describe('Toolbar basic @toolbar', () => {
  test.beforeEach(async ({ page }) => {
    await openToolbarPage(page, '/toolbar/basic');
  });

  test('P-TB-001 renders a horizontal toolbar with role, orientation and name', async ({ page }) => {
    const root = page.locator(TB);
    await expect(root).toHaveAttribute('role', 'toolbar');
    await expect(root).toHaveAttribute('aria-orientation', 'horizontal');
    await expect(root).toHaveAttribute('aria-label', 'Main commands');
    await expect(root).toHaveClass(/e-toolbar-horizontal/);
  });

  test('P-TB-002 renders a vertical toolbar', async ({ page }) => {
    const root = page.locator('#toolbar-vertical');
    await expect(root).toHaveAttribute('aria-orientation', 'vertical');
    await expect(root).toHaveClass(/e-toolbar-vertical/);
    const boxes = await root.locator('.e-btn').evaluateAll((els) => els.map((e) => e.getBoundingClientRect().top));
    expect(boxes[1]).toBeGreaterThan(boxes[0]);
    expect(boxes[2]).toBeGreaterThan(boxes[1]);
  });

  test('P-TB-013 a click runs the item handler and then the toolbar handler', async ({ page }) => {
    await page.locator(`${TB} #cut button`).click();
    await expect(page.locator('#cut-count')).toHaveText('Cut clicks: 1');
    await expect(page.locator('#last-click')).toHaveText('Last click: cut');
  });

  test('P-TB-014 a toggle flips its pressed state and the bound value', async ({ page }) => {
    const bold = page.locator(`${TB} #bold button`);
    await expect(bold).toHaveAttribute('aria-pressed', 'false');
    await bold.click();
    await expect(bold).toHaveAttribute('aria-pressed', 'true');
    await expect(page.locator('#bold-state')).toHaveText('Bold: True');
    await bold.click();
    await expect(bold).toHaveAttribute('aria-pressed', 'false');
    await expect(page.locator('#bold-state')).toHaveText('Bold: False');
  });

  test('P-TB-023 a cancelled toggle keeps its state', async ({ page }) => {
    const locked = page.locator('#toolbar-cancel #locked button');
    await locked.click();
    await expect(locked).toHaveAttribute('aria-pressed', 'false');
    await expect(page.locator('#locked-state')).toHaveText('Locked: False');
  });

  test('P-TB-033 a disabled item is inert and does not raise a click', async ({ page }) => {
    const disabled = page.locator(`${TB} #disabled button`);
    await expect(disabled).toBeDisabled();
    await disabled.click({ force: true });
    await expect(page.locator('#last-click')).toHaveText('Last click: none');
  });

  test('P-TB-034 separators and spacers are not focusable', async ({ page }) => {
    const sep = page.locator(`${TB} [role="separator"]`);
    await expect(sep).toHaveCount(1);
    expect(await sep.evaluate((e) => e.tabIndex)).toBe(-1);
    const spacer = page.locator(`${TB} .e-toolbar-spacer`);
    await expect(spacer).toHaveAttribute('aria-hidden', 'true');
  });

  test('P-TB-032 a hidden item is not rendered', async ({ page }) => {
    await expect(page.locator(`${TB} #hidden`)).toHaveCount(0);
  });

  test('P-TB-009 exactly one item is a tab stop', async ({ page }) => {
    const stops = (await tabIndexes(page, TB)).filter((t) => t === '0');
    expect(stops.length).toBe(1);
  });

  test('P-TB-048 toolbar styles do not leak into the host page', async ({ page }) => {
    const before = page.locator('#before-toolbar');
    const styles = await before.evaluate((e) => {
      const s = getComputedStyle(e);
      return { display: s.display, paddingInlineStart: s.paddingInlineStart };
    });
    expect(styles.display).toBe('inline-block');
    expect(styles.paddingInlineStart).not.toBe('8px');
    const rootDisplay = await page.locator(TB).evaluate((e) => getComputedStyle(e).display);
    expect(rootDisplay).toBe('flex');
  });
});
