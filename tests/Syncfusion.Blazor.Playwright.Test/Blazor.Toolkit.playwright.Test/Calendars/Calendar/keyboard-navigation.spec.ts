import { test, expect } from '@playwright/test';

test.describe('SfCalendar - Keyboard Navigation', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/calendar-test');
    await page.waitForLoadState('networkidle');
    await page.locator('#calendar-test').waitFor({ state: 'visible', timeout: 5000 });
  });

  test('arrow keys move focus and Enter selects', async ({ page }) => {
    // Target a stable current-month cell (exclude previous/next-month padding
    // cells which render without `aria-current` and whose IDs may not be honoured
    // by the Select action).
    const calendar = page.locator('#calendar-test');
    const currentMonthCells = calendar.locator('td.e-cell:not(.e-other-month)');
    await currentMonthCells.first().waitFor({ state: 'visible', timeout: 5000 });

    // Capture the id of the cell we are about to click so we can detect when
    // the focus has actually moved off of it (and prevent reading stale DOM).
    const initialCell = currentMonthCells.first();
    const initialId = await initialCell.getAttribute('id');

    await initialCell.click();                      // focus an actual date cell
    await expect(initialCell).toBeVisible();

    await page.keyboard.press('ArrowRight');        // move focus

    // Wait until the focused cell is no longer the initial cell. This avoids
    // racing the arrow-key handler that triggers a Blazor re-render before
    // we assert the new focused element.
    await expect
      .poll(async () => {
        const focused = calendar.locator('td.e-focused-date').first();
        const id = await focused.getAttribute('id');
        return id;
      }, { timeout: 5000 })
      .not.toBe(initialId);

    const focused = calendar.locator('td.e-focused-date').first();
    await expect(focused).toBeVisible();

    // Capture the focused cell id one more time AFTER the focus has clearly
    // settled on a different cell. Use that handle to assert the selection
    // state below, so we don't accidentally re-query a stale reference.
    const focusedIdBeforeEnter = await focused.getAttribute('id');

    await page.keyboard.press('Enter');             // select the focused cell

    // Wait until the cell we just stepped onto either becomes selected (the
    // expected outcome) OR keeps focus, whichever indicates the keyboard
    // selection handler ran. The SfCalendar exposes the selection state via
    // both the `e-selected` class and the `aria-selected` attribute, but
    // `aria-selected` is only rendered when the cell template re-renders,
    // so poll on the cell whose id we captured after focus moved.
    await expect
      .poll(
        async () => {
          if (!focusedIdBeforeEnter) return false as const;
          const cell = page.locator(`#calendar-test td[id="${focusedIdBeforeEnter}"]`);
          const cls = (await cell.getAttribute('class')) ?? '';
          const aria = await cell.getAttribute('aria-selected');
          return /e-selected/.test(cls) || aria === 'true';
        },
        { timeout: 10_000, intervals: [100, 200, 500, 1000] }
      )
      .toBe(true);

    // Final sanity assertion on the attribute so a regression in the
    // aria-selected binding is also caught.
    const finalCell = page.locator(
      `#calendar-test td[id="${focusedIdBeforeEnter!}"]`
    );
    await expect(finalCell).toHaveAttribute('aria-selected', 'true', {
      timeout: 5_000
    });
  });
});
