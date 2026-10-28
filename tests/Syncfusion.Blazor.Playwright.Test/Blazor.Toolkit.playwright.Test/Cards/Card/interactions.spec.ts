// Card interaction tests for SfCard.
// Drives the REAL component from the Blazor sample host (Pages/Cards/Card/Interactions.razor).

import { test, expect, type Page } from '@playwright/test';

const URL = 'http://localhost:5000/card/interactions';

async function openCardInteractions(page: Page, url = URL) {
  await page.goto(url, { waitUntil: 'commit' });
  await expect(page.locator('#interactive-ready')).toHaveAttribute('data-ready', 'true', { timeout: 15000 });
  await expect(page.locator('#card-clickable')).toBeVisible();
}

test.describe('Card interactions @card', () => {
  test.beforeEach(async ({ page }) => {
    await openCardInteractions(page);
  });

  test('P-CARD-003 click activates a clickable card once', async ({ page }) => {
    await page.locator('#card-clickable').click();
    await expect(page.locator('#card-click-count')).toHaveText('Card clicks: 1');
  });

  test('P-CARD-003 Enter activates, Space activates on key up, other keys do nothing', async ({ page }) => {
    const card = page.locator('#card-clickable');
    await card.focus();
    await expect(card).toBeFocused();

    await page.keyboard.press('Enter');
    await expect(page.locator('#card-click-count')).toHaveText('Card clicks: 1');

    await page.keyboard.press('Space');
    await expect(page.locator('#card-click-count')).toHaveText('Card clicks: 2');

    await page.keyboard.press('a');
    await page.keyboard.press('ArrowDown');
    await expect(page.locator('#card-click-count')).toHaveText('Card clicks: 2');
  });

  test('P-CARD-003 Tab keydown is not canceled', async ({ page }) => {
    const card = page.locator('#card-clickable');
    await card.focus();
    const prevented = await card.evaluate((el) => {
      const event = new KeyboardEvent('keydown', { key: 'Tab', bubbles: true, cancelable: true });
      el.dispatchEvent(event);
      return event.defaultPrevented;
    });
    expect(prevented).toBe(false);
  });

  test('Tab moves focus into and out of the card (no keyboard trap)', async ({ page }) => {
    await page.locator('#before-card').focus();
    await expect(page.locator('#before-card')).toBeFocused();

    await page.keyboard.press('Tab');
    await expect(page.locator('#card-clickable')).toBeFocused();

    await page.keyboard.press('Tab');
    await expect(page.locator('#after-card')).toBeFocused();

    await page.keyboard.press('Shift+Tab');
    await expect(page.locator('#card-clickable')).toBeFocused();
  });

  test('P-CARD-002 focus-visible draws an outline on a keyboard-focused card', async ({ page }) => {
    await page.locator('#before-card').focus();
    await expect(page.locator('#before-card')).toBeFocused();
    await page.keyboard.press('Tab');
    const card = page.locator('#card-clickable');
    await expect(card).toBeFocused();

    const outlineWidth = await card.evaluate((el) => getComputedStyle(el).outlineWidth);
    expect(outlineWidth).not.toBe('0px');
  });

  test('clickable card exposes button semantics and its accessible name', async ({ page }) => {
    const card = page.locator('#card-clickable');
    await expect(card).toHaveAttribute('role', 'button');
    await expect(card).toHaveAttribute('tabindex', '0');
    await expect(card).toHaveAttribute('aria-label', 'Open Trail Runner');
  });

  test('P-CARD-004 a nested action fires once and never activates the card', async ({ page }) => {
    await page.locator('#card-action-btn').click();
    await expect(page.locator('#action-click-count')).toHaveText('Action clicks: 1');
    await expect(page.locator('#host-click-count')).toHaveText('Host clicks: 0');

    await page.locator('#card-action-btn').focus();
    await page.keyboard.press('Enter');
    await expect(page.locator('#action-click-count')).toHaveText('Action clicks: 2');
    await expect(page.locator('#host-click-count')).toHaveText('Host clicks: 0');

    // The card body still activates it.
    await page.locator('#card-actions-host').getByText('Product').click();
    await expect(page.locator('#host-click-count')).toHaveText('Host clicks: 1');
  });

  test('P-CARD-005 loading card ignores input, is busy and makes actions unreachable', async ({ page }) => {
    const card = page.locator('#card-loading');
    await expect(card).toHaveAttribute('aria-busy', 'true');
    await expect(card).toHaveAttribute('aria-disabled', 'true');
    await expect(page.locator('#card-loading [data-testid="card-actions"]')).toHaveAttribute('inert', '');

    await card.focus();
    await page.keyboard.press('Enter');
    await page.keyboard.press('Space');
    await card.click({ force: true });
    await expect(page.locator('#loading-click-count')).toHaveText('Loading card clicks: 0');

    await page.locator('#toggle-loading').click();
    await expect(card).not.toHaveAttribute('aria-busy', 'true');
    await expect(page.locator('#card-loading [data-testid="card-actions"]')).not.toHaveAttribute('inert', '');

    await card.click();
    await expect(page.locator('#loading-click-count')).toHaveText('Loading card clicks: 1');
  });

  test('P-CARD-001 regions are styled through scoped CSS (::deep reaches child components)', async ({ page }) => {
    const title = page.locator('#card-clickable [data-testid="card-title"]');
    await expect(title).toBeVisible();
    expect(await title.evaluate((el) => getComputedStyle(el).fontWeight)).toBe('600');

    const content = page.locator('#card-clickable [data-testid="card-content"]');
    expect(await content.evaluate((el) => getComputedStyle(el).paddingLeft)).toBe('16px');
  });

  test('static card has no role, tabindex or interactive handlers', async ({ page }) => {
    const card = page.locator('#card-static');
    await expect(card).not.toHaveAttribute('role', /.+/);
    await expect(card).not.toHaveAttribute('tabindex', /.+/);
    expect(await card.evaluate((el) => getComputedStyle(el).cursor)).not.toBe('pointer');
  });

  test('media image has alt text and is laid out', async ({ page }) => {
    const img = page.locator('#card-static img');
    await expect(img).toHaveAttribute('alt', 'Canada map');
    const box = await img.boundingBox();
    expect(box).not.toBeNull();
    expect(box!.height).toBeGreaterThan(0);
  });

  test('P-CARD-006 horizontal card lays out regions in a row with media beside content', async ({ page }) => {
    const card = page.locator('#card-horizontal');
    expect(await card.evaluate((el) => getComputedStyle(el).flexDirection)).toBe('row');

    const media = await page.locator('#card-horizontal-media').boundingBox();
    const content = await page.locator('#card-horizontal [data-testid="card-content"]').boundingBox();
    expect(media).not.toBeNull();
    expect(content).not.toBeNull();
    expect(media!.width).toBeGreaterThan(0);
    expect(content!.x).toBeGreaterThanOrEqual(media!.x + media!.width - 1);
  });

  test('P-CARD-003 held Space activates on key up and does not repeat', async ({ page }) => {
    const card = page.locator('#card-clickable');
    await card.focus();
    await expect(card).toBeFocused();
    await page.keyboard.down(' ');
    await expect(page.locator('#card-click-count')).toHaveText('Card clicks: 0');
    await page.keyboard.up(' ');
    await expect(page.locator('#card-click-count')).toHaveText('Card clicks: 1');
  });

  test('P-CARD-003 keyboard activation does not scroll the page', async ({ page }) => {
    const card = page.locator('#card-clickable');
    await card.focus();
    await expect(card).toBeFocused();
    await page.evaluate(() => window.scrollTo(0, document.body.scrollHeight));
    const before = await page.evaluate(() => window.scrollY);
    await page.keyboard.press('Enter');
    await expect(page.locator('#card-click-count')).toHaveText('Card clicks: 1');
    const after = await page.evaluate(() => window.scrollY);
    expect(Math.abs(after - before)).toBeLessThanOrEqual(1);
    await page.keyboard.press(' ');
    await expect(page.locator('#card-click-count')).toHaveText('Card clicks: 2');
    const afterSpace = await page.evaluate(() => window.scrollY);
    expect(Math.abs(afterSpace - before)).toBeLessThanOrEqual(1);
  });

  test('P-CARD-002 pointer focus does not draw the keyboard outline', async ({ page }) => {
    const card = page.locator('#card-clickable');
    await card.click();
    await expect(card).toBeFocused();
    const outline = await card.evaluate((el) => getComputedStyle(el).outlineStyle);
    expect(outline).toBe('none');
  });

  test('elevation 0 draws no shadow and elevation 5 draws one', async ({ page }) => {
    const flat = await page.locator('#card-elev-0').evaluate((el) => getComputedStyle(el).boxShadow);
    const deep = await page.locator('#card-elev-5').evaluate((el) => getComputedStyle(el).boxShadow);
    expect(flat).toBe('none');
    expect(deep).not.toBe('none');
  });
});

test.describe('Card RTL @card', () => {
  test('P-CARD-007 horizontal card places media on the inline-start (right) side in RTL', async ({ page }) => {
    await openCardInteractions(page, `${URL}?rtl=true`);

    const host = page.locator('#card-horizontal-host');
    await expect(host).toHaveAttribute('dir', 'rtl');
    expect(await host.evaluate((el) => getComputedStyle(el).direction)).toBe('rtl');
    const card = page.locator('#card-horizontal');

    const media = await page.locator('#card-horizontal-media').boundingBox();
    const content = await page.locator('#card-horizontal [data-testid="card-content"]').boundingBox();
    expect(media).not.toBeNull();
    expect(content).not.toBeNull();
    // Media is first in markup, so in RTL it sits to the right of the content.
    expect(media!.x).toBeGreaterThan(content!.x);
  });
});

test.describe('Card mobile @card', () => {
  test.use({ viewport: { width: 375, height: 700 } });

  test('P-CARD-008 cards fit a narrow viewport without horizontal overflow', async ({ page }) => {
    await openCardInteractions(page);
    const overflow = await page.evaluate(() => document.documentElement.scrollWidth - document.documentElement.clientWidth);
    expect(overflow).toBeLessThanOrEqual(0);
  });
});
