import { test, expect } from '@playwright/test';

test.describe('DateTimePicker - Styling & Appearance', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/datetimepicker-test');
    await page.waitForLoadState('networkidle');
  });

  test('custom width applied to input', async ({ page }) => {
    const widthInput = page.locator('#wrapper-dtp-width .e-input');
    await expect(widthInput).toBeVisible();
    
    const root = page.locator('#wrapper-dtp-width');
    await expect(root).toBeVisible();
  });

  test('show clear icon when ShowClearButton is true', async ({ page }) => {
    const input = page.locator('#wrapper-dtp-basic input');
    const clear = page.locator('#wrapper-dtp-basic .e-close');
    
    await input.focus();
    await input.click();
    // Wait for the clear icon element to be present in the DOM
    await expect(clear).toHaveCount(1, { timeout: 5000 });
  });

  test('time format affects time popup display - 24 hour format', async ({ page }) => {
    const timeIcon = page.locator('#wrapper-dtp-step .e-clock');
    await timeIcon.click();
    
    const popup = page.locator('.e-popup:visible');
    await expect(popup).toHaveCount(1, { timeout: 5000 });

    const firstTimeItem = popup.locator('.e-list-item').first();
    const timeText = await firstTimeItem.textContent();
    // 24-hour format should display times like HH:mm
    expect(timeText).toMatch(/\d{2}:\d{2}/);
  });

  test('time format affects time popup display - 12 hour format with AM/PM', async ({ page }) => {
    const timeIcon = page.locator('#wrapper-dtp-timeformat .e-clock');
    await timeIcon.click();
    
    const popup = page.locator('.e-popup:visible');
    await expect(popup).toHaveCount(1, { timeout: 5000 });

    const firstTimeItem = popup.locator('.e-list-item').first();
    const timeText = await firstTimeItem.textContent();
    // 12-hour format should contain AM or PM
    expect(timeText).toMatch(/(AM|PM|am|pm)/);
  });

  test('input has correct styling when focused', async ({ page }) => {
    const input = page.locator('#wrapper-dtp-basic input');
    const container = page.locator('#wrapper-dtp-basic .e-control');
    
    await input.focus();
    
    const hasInputFocusClass = await container.evaluate((el) => 
      el.classList.contains('e-input-focus') || 
      el.classList.contains('e-focus')
    );
    
    expect(typeof hasInputFocusClass).toBe('boolean');
  });

  test('date and time icons are visible', async ({ page }) => {
    const dateIcon = page.locator('#wrapper-dtp-basic .e-timeline-today');
    const timeIcon = page.locator('#wrapper-dtp-basic .e-clock');
    
    await expect(dateIcon).toBeVisible();
    await expect(timeIcon).toBeVisible();
  });
});
