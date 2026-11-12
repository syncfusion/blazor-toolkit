// Toolbar overflow tests: the measured popup, priority ordering, the overflow button, keyboard and focus return,
// container-based resizing, scrollable mode, dynamic items and RTL placement.

import { test, expect, type Page } from '@playwright/test';
import { openToolbarPage, buttonTexts, expectBar } from './helpers';

const TB = '#toolbar-overflow';
const MORE = `${TB} [data-testid="toolbar-overflow-button"]`;
const POPUP = `${TB} [data-testid="toolbar-overflow-popup"]`;

/**
 * Resizes the frame and waits until the overflow state has caught up. Recalculation runs on the server after a
 * debounce, so the container changes first and the visible set follows; waiting on the More button (or its absence)
 * makes the next step start from a settled layout instead of racing it.
 */
async function setWidth(page: Page, id: 'width-wide' | 'width-medium' | 'width-narrow', width: number, popupMode = true) {
  await page.locator(`#${id}`).click();
  await expect(page.locator('#frame-width')).toHaveText(`Width: ${width}`);
  if (!popupMode) {
    return;
  }
  const expectMore = width < 900;
  await expect(page.locator(MORE)).toHaveCount(expectMore ? 1 : 0, { timeout: 8000 });
  await settledOverflowCount(page);
}

/** The number of overflowed items once two consecutive reads agree. */
async function settledOverflowCount(page: Page): Promise<number> {
  const read = () => page.locator(`${TB} .e-toolbar-item`).evaluateAll((els) => els.filter((e) => e.classList.contains('e-toolbar-item-overflowed')).length);
  let previous = await read();
  let agreeing = 0;
  for (let i = 0; i < 40; i++) {
    await page.waitForTimeout(100);
    const current = await read();
    agreeing = current === previous ? agreeing + 1 : 0;
    previous = current;
    if (agreeing >= 3) {
      return current;
    }
  }
  throw new Error(`overflow count did not settle (last ${previous})`);
}

test.describe('Toolbar overflow @toolbar', () => {
  test('P-TB-039 with room for every item there is no overflow button', async ({ page }) => {
    await openToolbarPage(page, '/toolbar/overflow');
    await expect(page.locator(MORE)).toHaveCount(0);
    expect((await buttonTexts(page, TB)).length).toBe(7);
  });

  test('P-TB-016 a narrow container moves items into a popup behind a More button', async ({ page }) => {
    await openToolbarPage(page, '/toolbar/overflow');
    await setWidth(page, 'width-narrow', 180);
    await expect(page.locator(MORE)).toHaveCount(1);
    await expect(page.locator(MORE)).toHaveAttribute('aria-expanded', 'false');
    await page.locator(MORE).click();
    await expect(page.locator(POPUP)).toBeVisible();
    await expect(page.locator(MORE)).toHaveAttribute('aria-expanded', 'true');
  });

  test('P-TB-049 the More button stays aligned with the first item at intermediate widths', async ({ page }) => {
    await openToolbarPage(page, '/toolbar/overflow-layout');
    const toolbar = page.locator('#toolbar-overflow-layout');
    const more = toolbar.locator('[data-testid="toolbar-overflow-button"]');
    await expect(more).toHaveCount(1);
    await expect(toolbar.locator('.e-toolbar-item-overflowed')).not.toHaveCount(0);

    const sharesFirstRow = await more.evaluate((button) => {
      const toolbar = button.closest('[role="toolbar"]');
      const firstItem = toolbar?.querySelector<HTMLElement>('.e-toolbar-item:not(.e-toolbar-item-overflowed)');
      if (!toolbar || !firstItem) {
        throw new Error('The toolbar or its first visible item was not rendered.');
      }

      const buttonBounds = button.getBoundingClientRect();
      const itemBounds = firstItem.getBoundingClientRect();
      return buttonBounds.top < itemBounds.bottom && buttonBounds.bottom > itemBounds.top;
    });
    expect(sharesFirstRow).toBe(true);
  });

  test('P-TB-017 the popup holds the overflowed items and the bar keeps the others', async ({ page }) => {
    await openToolbarPage(page, '/toolbar/overflow');
    await setWidth(page, 'width-medium', 320);
    await expectBar(page, TB, (t) => t.length < 7, 'some items should have overflowed');
    await page.locator(MORE).click();
    await expect(page.locator(POPUP)).toBeVisible();
    await expect(page.locator(`${POPUP} [role="menuitem"]`).first()).toBeVisible();
    const inPopup = await page.locator(`${POPUP} [role="menuitem"]`).allTextContents();
    const inBar = await buttonTexts(page, TB);
    expect(inPopup.length).toBeGreaterThan(0);
    for (const text of inPopup) {
      expect(inBar.map((t) => t.trim())).not.toContain(text.trim());
    }
    expect(inPopup.length + inBar.length).toBe(7);
  });

  test('P-TB-020 Show items stay longest and Hide items overflow first as the container narrows', async ({ page }) => {
    await openToolbarPage(page, '/toolbar/overflow');
    await setWidth(page, 'width-medium', 320);
    const bar = await expectBar(page, TB, (t) => !t.includes('Find') && !t.includes('Print'), 'Hide items should overflow first');
    expect(bar).toContain('Save');
    expect(bar).toContain('Undo');
    await setWidth(page, 'width-narrow', 180);
    const narrow = await expectBar(page, TB, (t) => t.length < bar.length, 'a narrower container should keep fewer items');
    expect(narrow).toContain('Save');
  });

  test('P-TB-018 selecting an entry runs it and closes the popup', async ({ page }) => {
    await openToolbarPage(page, '/toolbar/overflow');
    await setWidth(page, 'width-narrow', 180);
    await page.locator(MORE).click();
    await page.locator(`${POPUP} [role="menuitem"]`).first().click();
    await expect(page.locator(POPUP)).toHaveCount(0);
    await expect(page.locator('#last-click')).not.toHaveText('Last click: none');
  });

  test('P-TB-008 Escape closes the popup and focus returns to the More button', async ({ page }) => {
    await openToolbarPage(page, '/toolbar/overflow');
    await setWidth(page, 'width-narrow', 180);
    await page.locator(MORE).click();
    await expect(page.locator(POPUP)).toBeVisible();
    await page.keyboard.press('Escape');
    await expect(page.locator(POPUP)).toHaveCount(0);
    await expect(page.locator(MORE)).toBeFocused();
  });

  test('P-TB-018 clicking outside closes the popup', async ({ page }) => {
    await openToolbarPage(page, '/toolbar/overflow');
    await setWidth(page, 'width-narrow', 180);
    await page.locator(MORE).click();
    await expect(page.locator(POPUP)).toBeVisible();
    await page.locator('#after-toolbar').click({ position: { x: 2, y: 2 }, force: true });
    await expect(page.locator(POPUP)).toHaveCount(0);
  });

  test('P-TB-012 opening the popup moves focus into it and arrows move between entries', async ({ page }) => {
    await openToolbarPage(page, '/toolbar/overflow');
    await setWidth(page, 'width-narrow', 180);
    await page.locator(MORE).focus();
    await page.keyboard.press('Enter');
    await expect(page.locator(POPUP)).toBeVisible();
    const entries = page.locator(`${POPUP} [role="menuitem"]`);
    await expect(entries.first()).toBeFocused();
    await page.keyboard.press('ArrowDown');
    await expect(entries.nth(1)).toBeFocused();
    await page.keyboard.press('ArrowUp');
    await expect(entries.first()).toBeFocused();
  });

  test('P-TB-036 the More button aria-expanded follows the popup state', async ({ page }) => {
    await openToolbarPage(page, '/toolbar/overflow');
    await setWidth(page, 'width-narrow', 180);
    const more = page.locator(MORE);
    await expect(more).toHaveAttribute('aria-haspopup', 'menu');
    await more.click();
    await expect(more).toHaveAttribute('aria-expanded', 'true');
    await expect(more).toHaveAttribute('aria-controls', /.+/);
    await page.keyboard.press('Escape');
    await expect(more).toHaveAttribute('aria-expanded', 'false');
  });

  test('P-TB-020 the container width, not the viewport, decides what overflows', async ({ page }) => {
    await page.setViewportSize({ width: 1400, height: 900 });
    await openToolbarPage(page, '/toolbar/overflow');
    await setWidth(page, 'width-narrow', 180);
    await expect(page.locator(MORE)).toHaveCount(1);
    await setWidth(page, 'width-wide', 900);
    await expect(page.locator(MORE)).toHaveCount(0);
  });

  test('P-TB-021 adding an item while items overflow recalculates', async ({ page }) => {
    await openToolbarPage(page, '/toolbar/overflow');
    await setWidth(page, 'width-medium', 320);
    const before = (await page.locator(`${TB} .e-toolbar-item:not(.e-toolbar-item-overflowed)`).count());
    await page.locator('#add-item').click();
    await expect(page.locator(`${TB} #o-extra`)).toHaveCount(1);
    const total = await page.locator(`${TB} .e-toolbar-item`).count();
    expect(total).toBe(8);
    const visible = await page.locator(`${TB} .e-toolbar-item:not(.e-toolbar-item-overflowed)`).count();
    expect(visible).toBeLessThanOrEqual(before + 1);
  });

  test('P-TB-038 Scrollable mode keeps one line, scrolls and has no overflow button', async ({ page }) => {
    await openToolbarPage(page, '/toolbar/overflow');
    await page.locator('#mode-scroll').click();
    await setWidth(page, 'width-narrow', 180, false);
    await expect(page.locator(MORE)).toHaveCount(0);
    const metrics = await page.locator(`${TB} .e-toolbar-content`).evaluate((e) => ({
      scrollable: e.scrollWidth > e.clientWidth,
      overflowX: getComputedStyle(e).overflowX,
      wrap: getComputedStyle(e).flexWrap
    }));
    expect(metrics.wrap).toBe('nowrap');
    expect(metrics.scrollable).toBe(true);
    expect(['auto', 'scroll']).toContain(metrics.overflowX);
  });

  test('P-TB-028 in RTL the More button sits at the left (logical end) edge', async ({ page }) => {
    await openToolbarPage(page, '/toolbar/overflow?rtl=true');
    await setWidth(page, 'width-narrow', 180);
    const toolbar = await page.locator(TB).boundingBox();
    const more = await page.locator(MORE).boundingBox();
    expect(toolbar).not.toBeNull();
    expect(more).not.toBeNull();
    const toolbarMid = toolbar!.x + toolbar!.width / 2;
    expect(more!.x + more!.width / 2).toBeLessThan(toolbarMid);
  });

  test('P-TB-029 in RTL the popup aligns with the overflow trigger', async ({ page }) => {
    await openToolbarPage(page, '/toolbar/overflow?rtl=true');
    await setWidth(page, 'width-narrow', 180);
    await page.locator(MORE).click();
    await expect(page.locator(POPUP)).toBeVisible();
    const placement = await page.evaluate(() => {
      const popup = document.querySelector<HTMLElement>('[data-testid="toolbar-overflow-popup"]');
      const anchor = document.querySelector<HTMLElement>('[data-testid="toolbar-overflow-button"]');
      if (!popup || !anchor) {
        throw new Error('The popup and overflow button must be rendered.');
      }
      const popupBounds = popup.getBoundingClientRect();
      const anchorBounds = anchor.getBoundingClientRect();
      return {
        position: getComputedStyle(popup).position,
        left: popupBounds.left,
        top: popupBounds.top,
        bottom: popupBounds.bottom,
        anchorLeft: anchorBounds.left,
        anchorTop: anchorBounds.top,
        anchorBottom: anchorBounds.bottom
      };
    });
    expect(placement.position).toBe('fixed');
    expect(Math.abs(placement.left - placement.anchorLeft)).toBeLessThanOrEqual(1);
    expect(
      Math.abs(placement.top - placement.anchorBottom) <= 1 ||
      Math.abs(placement.bottom - placement.anchorTop) <= 1
    ).toBe(true);
  });

  test('P-TB-045 interactive targets are at least 24 by 24 CSS px', async ({ page }) => {
    await openToolbarPage(page, '/toolbar/overflow');
    const boxes = await page.locator(`${TB} .e-btn`).evaluateAll((els) =>
      els.map((e) => {
        const r = e.getBoundingClientRect();
        return { w: r.width, h: r.height };
      }));
    expect(boxes.length).toBeGreaterThan(0);
    for (const b of boxes) {
      expect(b.w).toBeGreaterThanOrEqual(24);
      expect(b.h).toBeGreaterThanOrEqual(24);
    }
  });

  test('P-TB-047 the overflow popup is not clipped by an overflow:hidden ancestor', async ({ page }) => {
    await openToolbarPage(page, '/toolbar/overflow');
    await setWidth(page, 'width-narrow', 180);
    await page.locator(TB).evaluate((toolbar) => {
      const host = document.createElement('div');
      host.id = 'clip-hidden';
      host.style.cssText = 'overflow:hidden;max-height:48px;border:1px solid transparent;';
      toolbar.parentElement?.insertBefore(host, toolbar);
      host.appendChild(toolbar);
    });
    await page.locator(MORE).click();
    await expect(page.locator(POPUP)).toBeVisible();
    const clipped = await page.evaluate(() => {
      const host = document.getElementById('clip-hidden')!.getBoundingClientRect();
      const popup = document.querySelector('[data-testid="toolbar-overflow-popup"]')!.getBoundingClientRect();
      const inside = popup.top >= host.top - 1 && popup.bottom <= host.bottom + 1 && popup.left >= host.left - 1 && popup.right <= host.right + 1;
      const position = getComputedStyle(document.querySelector('[data-testid="toolbar-overflow-popup"]')!).position;
      return { inside, position, popupHeight: popup.height, hostHeight: host.height };
    });
    expect(clipped.position).toBe('fixed');
    expect(clipped.inside).toBe(false);
    expect(clipped.popupHeight).toBeGreaterThan(clipped.hostHeight);
  });

  test('P-TB-048 the overflow popup is not clipped by an overflow:auto ancestor and follows a scroll', async ({ page }) => {
    await openToolbarPage(page, '/toolbar/overflow');
    await setWidth(page, 'width-narrow', 180);
    await page.locator(TB).evaluate((toolbar) => {
      const host = document.createElement('div');
      host.id = 'clip-auto';
      host.style.cssText = 'overflow:auto;height:64px;border:1px solid transparent;';
      const spacer = document.createElement('div');
      spacer.style.height = '240px';
      toolbar.parentElement?.insertBefore(host, toolbar);
      host.appendChild(toolbar);
      host.appendChild(spacer);
    });
    await page.locator(MORE).click();
    await expect(page.locator(POPUP)).toBeVisible();
    const before = await page.locator(POPUP).boundingBox();
    await page.locator('#clip-auto').evaluate((host) => { host.scrollTop = 40; });
    await page.waitForTimeout(50);
    const after = await page.locator(POPUP).boundingBox();
    const escaped = await page.evaluate(() => {
      const host = document.getElementById('clip-auto')!.getBoundingClientRect();
      const popup = document.querySelector('[data-testid="toolbar-overflow-popup"]')!.getBoundingClientRect();
      return popup.bottom > host.bottom + 1;
    });
    expect(before).not.toBeNull();
    expect(after).not.toBeNull();
    expect(escaped).toBe(true);
    expect(Math.abs(after!.y - before!.y)).toBeGreaterThan(10);
  });

  test('P-TB-046 interactive targets are at least 44 by 44 CSS px on a mobile viewport', async ({ page }) => {
    await page.setViewportSize({ width: 375, height: 700 });
    await openToolbarPage(page, '/toolbar/overflow');
    const boxes = await page.locator(`${TB} .e-toolbar-item:not(.e-toolbar-item-overflowed) .e-btn`).evaluateAll((els) =>
      els.map((e) => {
        const r = e.getBoundingClientRect();
        return { w: r.width, h: r.height };
      }));
    expect(boxes.length).toBeGreaterThan(0);
    for (const b of boxes) {
      expect(b.h).toBeGreaterThanOrEqual(44);
      expect(b.w).toBeGreaterThanOrEqual(44);
    }
  });
});
