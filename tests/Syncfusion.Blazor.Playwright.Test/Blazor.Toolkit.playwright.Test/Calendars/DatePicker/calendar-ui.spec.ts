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
    const popup = page.locator('.e-popup:visible');
    await expect(popup).toHaveCount(1, { timeout: 10000 });
    const title = popup.locator('.e-title');
    await expect(title).toBeVisible();
    const initialTitle = (await title.textContent())?.trim() ?? '';
    const next = popup.locator('.e-next');
    const prev = popup.locator('.e-prev');
    await expect(next).toBeVisible();
    await expect(prev).toBeVisible();
    await next.click();
    await expect(title).not.toHaveText(initialTitle, { timeout: 5000 });
    await prev.click();
    await expect(title).toHaveText(initialTitle, { timeout: 5000 });
  });
});
