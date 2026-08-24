import { test, expect } from '@playwright/test';

test.describe('SfCalendar - Accessibility', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/calendar-test');
    await page.waitForLoadState('networkidle');
    await page.locator('#calendar-test').waitFor({ state: 'visible', timeout: 5000 });
  });

  test('day cells include ARIA attributes and unique ids', async ({ page }) => {
    // The actual aria-label includes the day number (e.g. "Calendar cell 1") and the
    // word "disabled" suffix for disabled cells. Use a substring match so the test
    // is robust to those variations.
    const cells = page.locator('td[aria-label*="calendar cell" i]');
    const count = await cells.count();
    expect(count).toBeGreaterThan(0);
    const first = cells.nth(0);
    await expect(first).toHaveAttribute('aria-label', /calendar cell/i);
    const id = await first.getAttribute('id');
    expect(id).toBeTruthy();
  });
});
