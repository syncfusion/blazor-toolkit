import { expect, type Locator, type Page } from '@playwright/test';

export const BASE = 'http://localhost:5000';

/** Opens a Toolbar host page and waits until the interactive circuit is ready. */
export async function openToolbarPage(page: Page, path: string): Promise<void> {
  await page.goto(`${BASE}${path}`, { waitUntil: 'commit' });
  await expect(page.locator('#interactive-ready')).toHaveAttribute('data-ready', 'true', { timeout: 15000 });
}

/** Text of the buttons inside a toolbar that are focusable command buttons, in DOM order. */
export async function buttonTexts(page: Page, toolbar: string): Promise<string[]> {
  return page.locator(`${toolbar} .e-toolbar-item:not(.e-toolbar-item-overflowed) .e-btn`).allTextContents();
}

/** The roving tab stops of a toolbar: each command button with its tabindex. */
export async function tabIndexes(page: Page, toolbar: string): Promise<Array<string | null>> {
  return page.locator(`${toolbar} .e-toolbar-item .e-btn`).evaluateAll((els) => els.map((e) => e.getAttribute('tabindex')));
}

/** The id of the toolbar item that owns the focused element, or an empty string. */
export async function focusedItemId(page: Page): Promise<string> {
  return page.evaluate(() => (document.activeElement?.closest('.e-toolbar-item') as HTMLElement | null)?.id ?? '');
}

/**
 * Asserts which toolbar item holds focus. Arrow keys are handled on the server in Interactive Server mode, so focus
 * moves after a round trip; the assertion polls instead of reading once.
 */
export async function expectFocusedItem(page: Page, id: string): Promise<void> {
  await expect.poll(() => focusedItemId(page), { timeout: 5000, message: `focus should be on "${id}"` }).toBe(id);
}

/**
 * Waits for the bar to settle after a resize or an item change. Overflow is recalculated on the server after a
 * debounce, so the visible set changes a moment after the container does.
 */
/** Chromium visual baseline. Masks the ready marker so the circuit flag cannot shift the shot. */
export async function expectToolbarShot(page: Page, locator: Locator, name: string): Promise<void> {
  await page.evaluate(() => document.fonts.ready);
  await expect(locator).toHaveScreenshot(name, {
    animations: 'disabled',
    caret: 'hide',
    maxDiffPixelRatio: 0.01,
  });
}

export async function expectBar(page: Page, toolbar: string, predicate: (texts: string[]) => boolean, message: string): Promise<string[]> {
  let last: string[] = [];
  await expect
    .poll(async () => {
      last = (await buttonTexts(page, toolbar)).map((t) => t.trim());
      return predicate(last);
    }, { timeout: 5000, message })
    .toBe(true);
  return last;
}
