// Card interaction tests for SfCard.
// Drives the real component from the Blazor sample host.

import { test, expect, type Page } from '@playwright/test';

const URL = 'http://localhost:5000/card/interactions';

async function openCardInteractions(page: Page, url = URL) {
  await page.goto(url, { waitUntil: 'commit' });
  await expect(page.locator('#interactive-ready')).toHaveAttribute('data-ready', 'true', { timeout: 15000 });
  await expect(page.locator('#card-clickable')).toBeVisible();
  // Keyboard activation is attached by card.js after the first interactive render.
  await page.waitForFunction(() => Boolean((document.querySelector('#card-clickable') as { _blazorCardInstance?: unknown } | null)?._blazorCardInstance));
}

// Dispatches a cancelable key event on the element and reports whether a handler canceled it.
function keyPrevented(page: Page, selector: string, type: 'keydown' | 'keyup', init: KeyboardEventInit) {
  return page.locator(selector).evaluate((el, [eventType, eventInit]) => {
    const event = new KeyboardEvent(eventType as string, { ...(eventInit as KeyboardEventInit), bubbles: true, cancelable: true });
    el.dispatchEvent(event);
    return event.defaultPrevented;
  }, [type, init] as const);
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

  test('P-CARD-003 Enter and Space key events on a clickable card are canceled', async ({ page }) => {
    // Without this, Space scrolls the page and Enter can submit an enclosing form.
    expect(await keyPrevented(page, '#card-clickable', 'keydown', { key: 'Enter', code: 'Enter' })).toBe(true);
    expect(await keyPrevented(page, '#card-clickable', 'keydown', { key: ' ', code: 'Space' })).toBe(true);
    expect(await keyPrevented(page, '#card-clickable', 'keyup', { key: ' ', code: 'Space' })).toBe(true);
    expect(await keyPrevented(page, '#card-clickable', 'keydown', { key: 'a', code: 'KeyA' })).toBe(false);
    expect(await keyPrevented(page, '#card-clickable', 'keydown', { key: 'ArrowDown', code: 'ArrowDown' })).toBe(false);
  });

  test('B-CARD-111 repeated Enter and Space key downs do not activate the card', async ({ page }) => {
    expect(await keyPrevented(page, '#card-clickable', 'keydown', { key: 'Enter', code: 'Enter', repeat: true })).toBe(true);
    // A repeated Space key down still cancels the scroll but does not arm activation.
    expect(await keyPrevented(page, '#card-clickable', 'keydown', { key: ' ', code: 'Space', repeat: true })).toBe(true);
    await keyPrevented(page, '#card-clickable', 'keyup', { key: ' ', code: 'Space' });
    // Events reach the server in order: once this click is counted, any activation from the keys would be too.
    await page.locator('#card-clickable').click();
    await expect(page.locator('#card-click-count')).toHaveText('Card clicks: 1');
  });

  test('B-CARD-109 / 110 Enter key up and an unmatched Space key up do not activate again', async ({ page }) => {
    const card = page.locator('#card-clickable');
    await card.focus();
    await page.keyboard.down('Enter');
    await expect(page.locator('#card-click-count')).toHaveText('Card clicks: 1');
    await page.keyboard.up('Enter');
    await page.keyboard.up(' ');
    await card.click();
    await expect(page.locator('#card-click-count')).toHaveText('Card clicks: 2');
  });

  test('a loading card cancels the key but does not activate', async ({ page }) => {
    // Keyboard activation goes through the same click path, so the loading guard applies to it too.
    const card = page.locator('#card-loading');
    await card.focus();
    await page.keyboard.press('Enter');
    await page.keyboard.press(' ');
    // Ending loading is processed after the keys, so the count below already reflects them.
    await page.locator('#toggle-loading').click();
    await expect(card).not.toHaveAttribute('aria-busy', 'true');
    await expect(page.locator('#loading-click-count')).toHaveText('Loading card clicks: 0');
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
    // Space scrolls down, so the page must have room below the card or the check cannot fail.
    await page.setViewportSize({ width: 1280, height: 400 });
    const card = page.locator('#card-clickable');
    await card.focus();
    await expect(card).toBeFocused();
    await page.evaluate(() => window.scrollTo(0, 0));
    const before = await page.evaluate(() => window.scrollY);
    const roomBelow = await page.evaluate(() => document.documentElement.scrollHeight - window.innerHeight - window.scrollY);
    expect(roomBelow).toBeGreaterThan(100);
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
