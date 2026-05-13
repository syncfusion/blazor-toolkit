import { test, expect } from '@playwright/test';

test.describe('SfCalendar - Keyboard Navigation', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/calendar-test');
    await page.waitForLoadState('networkidle');
    await page.locator('#calendar-test').waitFor({ state: 'visible', timeout: 5000 });
  });

  test('arrow keys move focus and Enter selects', async ({ page }) => {
    const firstCell = page.locator('#calendar-test td.e-cell').first();
    await firstCell.click();                       // focus an actual date cell
    await expect(firstCell).toBeVisible();

    await page.keyboard.press('ArrowRight');      // move focus
    const focused = page.locator('td.e-focused-date').first();
    await expect(focused).toBeVisible();

    await page.keyboard.press('Enter');           // select the focused cell
    await expect(focused).toHaveAttribute('aria-selected', 'true', { timeout: 5000 });
  });
});
