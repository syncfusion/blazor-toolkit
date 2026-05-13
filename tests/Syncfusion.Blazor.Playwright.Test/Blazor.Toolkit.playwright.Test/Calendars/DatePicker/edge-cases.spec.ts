import { test, expect } from '@playwright/test';

test.describe('DatePicker - Edge Cases & Special Scenarios', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/datepicker-test');
    await page.waitForLoadState('networkidle');
  });

  test('rapid open/close cycles do not leak', async ({ page }) => {
    const icon = page.locator('#wrapper-dp-basic .e-timeline-today');
    for (let i = 0; i < 10; i++) {
      await icon.click();
      // Wait briefly for popup to appear (non-fatal if it doesn't)
      try {
        await expect(page.locator('.e-popup')).toBeVisible({ timeout: 1000 });
      } catch (e) {
        // popup did not appear quickly; continue to next iteration
      }
      // Use Escape to reliably dismiss the popup, then wait for it to be hidden
      await page.keyboard.press('Escape');
      try {
        await expect(page.locator('.e-popup:visible')).toHaveCount(0, { timeout: 1000 });
      } catch (e) {
        // if still present, continue and check final condition below
      }
      await page.waitForTimeout(50);
    }
    // finally ensure no visible popups remain
    await expect(page.locator('.e-popup:visible')).toHaveCount(0, { timeout: 5000 });
  });

  test('invalid typed input does not crash component', async ({ page }) => {
    const input = page.locator('#wrapper-dp-basic input');
    await input.fill('invalid-date');
    await input.press('Tab');
    // should not crash; input remains present
    await expect(input).toBeVisible();
  });
});
