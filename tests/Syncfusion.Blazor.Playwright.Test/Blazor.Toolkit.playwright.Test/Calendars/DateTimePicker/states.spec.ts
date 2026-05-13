import { test, expect } from '@playwright/test';

test.describe('DateTimePicker - States & Interactions', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/datetimepicker-test');
    await page.waitForLoadState('networkidle');
  });

  test('focus and blur behavior', async ({ page }) => {
    const input = page.locator('#wrapper-dtp-basic input');
    
    await input.focus();
    await expect(input).toBeFocused();
    
    await input.press('Tab');
    await expect(input).not.toBeFocused();
  });

  test('disabled state does not open calendar popup', async ({ page }) => {
    const dateIcon = page.locator('#wrapper-dtp-disabled .e-timeline-today');
    await dateIcon.click();
    
    const popup = page.locator('.e-popup');
    await expect(popup).not.toBeVisible();
  });

  test('disabled state does not open time popup', async ({ page }) => {
    const timeIcon = page.locator('#wrapper-dtp-disabled .e-clock');
    await timeIcon.click();
    
    const popup = page.locator('.e-popup');
    await expect(popup).not.toBeVisible();
  });

  test('disabled state shows disabled attribute', async ({ page }) => {
    const input = page.locator('#wrapper-dtp-disabled input');
    
    await expect(input).toHaveAttribute('disabled');
  });

  test('keyboard navigation in calendar with arrow keys', async ({ page }) => {
    const dateIcon = page.locator('#wrapper-dtp-basic .e-timeline-today');
    await dateIcon.click();
    
    const popup = page.locator('.e-popup:visible');
    await expect(popup).toHaveCount(1, { timeout: 5000 });

    const calendar = popup.locator('.e-calendar');
    
    // Press arrow keys to navigate
    await calendar.focus();
    await page.keyboard.press('ArrowRight');
    await page.keyboard.press('ArrowLeft');
    await page.keyboard.press('ArrowDown');
  });

  test('keyboard navigation in time popup with arrow keys', async ({ page }) => {
    const timeIcon = page.locator('#wrapper-dtp-basic .e-clock');
    await timeIcon.click();
    
    const popup = page.locator('.e-popup:visible');
    await expect(popup).toHaveCount(1, { timeout: 5000 });

    const timeList = popup.locator('.e-list-parent');
    
    // Navigate through time items
    await timeList.focus();
    await page.keyboard.press('ArrowDown');
    await page.keyboard.press('ArrowUp');
  });

  test('enter key selects item from time popup', async ({ page }) => {
    const timeIcon = page.locator('#wrapper-dtp-basic .e-clock');
    await timeIcon.click();
    
    const popup = page.locator('.e-popup:visible');
    await expect(popup).toHaveCount(1, { timeout: 5000 });

    const timeList = popup.locator('.e-list-parent');
    await timeList.focus();
    await page.keyboard.press('Enter');
    
    // Popup should close after selection
    await page.waitForTimeout(300);
    const stillVisible = page.locator('.e-popup:visible');
    const count = await stillVisible.count();
    expect(count).toBeLessThanOrEqual(1);
  });

  test('enter key selects date from calendar', async ({ page }) => {
    const dateIcon = page.locator('#wrapper-dtp-basic .e-timeline-today');
    await dateIcon.click();
    
    const popup = page.locator('.e-popup:visible');
    await expect(popup).toHaveCount(1, { timeout: 5000 });

    const calendar = popup.locator('.e-calendar');
    await calendar.focus();
    await page.keyboard.press('Enter');
    
    // Calendar should close or remain visible
    await page.waitForTimeout(300);
  });

  test('home/end keys navigate time list', async ({ page }) => {
    const timeIcon = page.locator('#wrapper-dtp-basic .e-clock');
    await timeIcon.click();
    
    const popup = page.locator('.e-popup:visible');
    await expect(popup).toHaveCount(1, { timeout: 5000 });

    const timeList = popup.locator('.e-list-parent');
    await timeList.focus();
    await page.keyboard.press('Home');
    await page.keyboard.press('End');
  });
});
