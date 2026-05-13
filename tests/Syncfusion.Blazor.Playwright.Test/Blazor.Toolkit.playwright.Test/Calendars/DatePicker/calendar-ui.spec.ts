import { test, expect } from '@playwright/test';

test.describe('DatePicker - Calendar UI & Selection', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/datepicker-test');
    await page.waitForLoadState('networkidle');
  });

  test('open calendar popup via icon click and select date', async ({ page }) => {
    const icon = page.locator('#wrapper-dp-basic .e-timeline-today');
    await icon.click();
    // Wait specifically for a visible popup container (ignore hidden DOM nodes)
    const popup = page.locator('.e-popup:visible');
    await expect(popup).toHaveCount(1, { timeout: 5000 });

    const day = popup.locator('.e-calendar .e-cell.e-weekend:not(.e-other-month)').first();
    const dayText = await day.textContent();
    await day.click();

    const input = page.locator('#wrapper-dp-basic input');
    const dayTextTrim = (dayText ?? '').trim();
    await expect(input).toHaveValue(new RegExp(dayTextTrim), { timeout: 5000 });
  });

  test('navigate months via prev/next', async ({ page }) => {
    const icon = page.locator('#wrapper-dp-basic .e-timeline-today');
    await icon.click();
    // wait for the popup to be visible and query buttons from that visible popup
    const popup = page.locator('.e-popup:visible');
    await expect(popup).toHaveCount(1, { timeout: 5000 });
    const prev = popup.locator('.e-prev');
    const next = popup.locator('.e-next');
    await expect(prev).toBeVisible();
    await expect(next).toBeVisible();
    await next.click();
    await prev.click();
  });
});
