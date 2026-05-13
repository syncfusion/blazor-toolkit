import { Page, Locator } from '@playwright/test';

// Helper utilities for interacting with the sample calendar page
export function rootLocator(page: Page, id = 'calendar-test'): Locator {
  return page.locator(`#${id}`);
}

export function eventLog(page: Page) {
  return page.locator('#event-log');
}

// Format a JS Date to local day number string
export function dayString(dt: Date) {
  return String(dt.getDate());
}

// Return a locator for a visible month-day cell (ignores other-month cells)
export function getDayCellLocator(page: Page, date: Date, id = 'calendar-test') {
  const day = dayString(date);
  // Select rendered day cells with the custom marker and exclude 'other-month' cells
  return rootLocator(page, id).locator(`.e-custom-day-cell:not(.e-other-month)`, { hasText: day });
}

// Click a date (waits for event log update if expectedText provided)
export async function selectDate(page: Page, date: Date, opts?: { id?: string; expectLogContains?: string }) {
  const id = opts?.id ?? 'calendar-test';
  const loc = getDayCellLocator(page, date, id);
  await loc.first().click();
  if (opts?.expectLogContains) {
    await page.locator('#event-log').waitFor({ state: 'visible' });
    await page.waitForFunction(
      (selector, text) => document.querySelector(selector)?.innerHTML.includes(text),
      '#event-log',
      opts.expectLogContains
    );
  } else {
    // Prefer waiting for selection attribute/class change on the clicked element, but
    // fall back to watching the event log for any change to reduce flakiness across DOM variants.
    const beforeLog = await page.locator('#event-log').innerHTML();
    const handle = await loc.first().elementHandle();
    if (handle) {
      try {
        await page.waitForFunction((el: any) => {
          if (!el) return false;
          const aria = el.getAttribute('aria-selected');
          const cls = el.className || el.parentElement?.className || '';
          return aria === 'true' || /e-selected|e-active/.test(cls);
        }, handle, { timeout: 2000 });
        return;
      } catch {
        // continue to fallback
      }
    }
    // Fallback: wait until event log changes (some builds log selection via callbacks)
    await page.waitForFunction((before) => document.querySelector('#event-log')?.innerHTML !== before, beforeLog, { timeout: 5000 });
  }
}

// Navigate calendar via UI button toggles or component API if present on page
export async function toggleMultiSelect(page: Page) {
  await page.click('#toggle-multiselect');
}

export async function togglePersistence(page: Page) {
  await page.click('#toggle-persistence');
}

// Assert a date cell is disabled (presence of e-disable on its ancestor cell)
export async function assertDisabledDate(page: Page, date: Date, id = 'calendar-test') {
  const cell = getDayCellLocator(page, date, id).first();
  const cls = await cell.evaluate((el) => el.className || el.parentElement?.className || '');
  if (!/e-disable/.test(cls)) {
    throw new Error(`Expected date ${date.toDateString()} to be disabled (class e-disable)`);
  }
}

// Navigate to a view using the sample's navigate-year button (sample-only convenience)
export async function navigateYearView(page: Page) {
  await page.click('#navigate-year');
}
