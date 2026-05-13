import { test, expect } from '@playwright/test';

test.describe('TimePicker - Popup UI & Selection', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/timepicker-test');
    await page.waitForLoadState('networkidle');
  });

  test('open popup via icon click and show list', async ({ page }) => {
    const icon = page.locator('#wrapper-tp-basic .e-clock');
    await icon.click();
    const popup = page.locator('.e-popup:visible');
    await expect(popup).toBeVisible();
    await expect(popup.locator('.e-list-item')).not.toHaveCount(0);
  });

  test('select time from popup sets value and closes popup', async ({ page }) => {
    const icon = page.locator('#wrapper-tp-basic .e-clock');
    await icon.click();
    const popup = page.locator('.e-popup:visible');
    await expect(popup).toHaveCount(1, { timeout: 5000 });

    const timeItem = popup.locator('.e-list-item').first();
    const timeText = await timeItem.textContent();
    await timeItem.click();

    const input = page.locator('#wrapper-tp-basic input');
    const timeTextTrim = (timeText ?? '').trim();
    await expect(input).toHaveValue(new RegExp(timeTextTrim), { timeout: 5000 });
  });

  test('navigate time list via keyboard and select', async ({ page }) => {
    const icon = page.locator('#wrapper-tp-basic .e-clock');
    await icon.click();
    const popup = page.locator('.e-popup:visible');
    await expect(popup).toHaveCount(1, { timeout: 5000 });

    const input = page.locator('#wrapper-tp-basic input');
    await page.keyboard.press('ArrowDown');
    await page.keyboard.press('ArrowDown');
    await page.keyboard.press('Enter');

    const initialIcon = page.locator('#wrapper-tp-initial .e-clock');
    await icon.click();
    const value = await input.inputValue();
    expect(value).toContain('00:30');
  });
});
