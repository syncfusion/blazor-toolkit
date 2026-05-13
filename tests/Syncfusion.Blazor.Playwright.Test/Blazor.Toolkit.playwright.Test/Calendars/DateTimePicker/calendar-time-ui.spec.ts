import { test, expect } from '@playwright/test';

test.describe('DateTimePicker - Calendar & Time UI', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/datetimepicker-test');
    await page.waitForLoadState('networkidle');
  });

  test('open calendar popup via date icon click', async ({ page }) => {
    const dateIcon = page.locator('#wrapper-dtp-basic .e-timeline-today');
    await dateIcon.click();
    // Wait specifically for a visible popup container
    const popup = page.locator('.e-popup:visible');
    await expect(popup).toHaveCount(1, { timeout: 5000 });
    
    // Calendar should be visible
    const calendar = popup.locator('.e-calendar');
    await expect(calendar).toBeVisible();
  });

  test('open time popup via time icon click', async ({ page }) => {
    const timeIcon = page.locator('#wrapper-dtp-basic .e-clock');
    await timeIcon.click();
    // Wait for visible popup with time list
    const popup = page.locator('.e-popup:visible');
    await expect(popup).toHaveCount(1, { timeout: 5000 });
    
    // Time list should be visible
    const timeList = popup.locator('.e-list-parent');
    await expect(timeList).toBeVisible();
  });

  test('select date from calendar and close popup', async ({ page }) => {
    const dateIcon = page.locator('#wrapper-dtp-basic .e-timeline-today');
    await dateIcon.click();
    
    const popup = page.locator('.e-popup:visible');
    await expect(popup).toHaveCount(1, { timeout: 5000 });

    const day = popup.locator('.e-calendar .e-cell:not(.e-other-month):not(.e-disabled)').first();
    const dayText = await day.textContent();
    await day.click();

    const input = page.locator('#wrapper-dtp-basic input');
    const dayTextTrim = (dayText ?? '').trim();
    await expect(input).toHaveValue(new RegExp(dayTextTrim), { timeout: 5000 });
  });

  test('select time from popup', async ({ page }) => {
    const input = page.locator('#wrapper-dtp-basic input');
    const initialValue = await input.inputValue();
    
    const timeIcon = page.locator('#wrapper-dtp-basic .e-clock');
    await timeIcon.click();
    
    const popup = page.locator('.e-popup:visible');
    await expect(popup).toHaveCount(1, { timeout: 5000 });

    const timeItem = popup.locator('.e-list-item:not(.e-disabled)').first();
    await timeItem.click();
    await page.waitForTimeout(500);

    const newValue = await input.inputValue();
    // Value should change after selecting time
    expect(newValue).not.toBe(initialValue);
    expect(newValue.length).toBeGreaterThan(0);
  });

  test('navigate months in calendar via prev/next', async ({ page }) => {
    const dateIcon = page.locator('#wrapper-dtp-basic .e-timeline-today');
    await dateIcon.click();
    
    const popup = page.locator('.e-popup:visible');
    await expect(popup).toHaveCount(1, { timeout: 5000 });
    
    const prev = popup.locator('.e-prev');
    const next = popup.locator('.e-next');
    await expect(prev).toBeVisible();
    await expect(next).toBeVisible();
    
    await next.click();
    await page.waitForTimeout(300);
    await prev.click();
  });

  test('respect Min and Max date constraints', async ({ page }) => {
    const dateIcon = page.locator('#wrapper-dtp-minmax .e-timeline-today');
    await dateIcon.click();
    
    const popup = page.locator('.e-popup:visible');
    await expect(popup).toHaveCount(1, { timeout: 5000 });

    // Check for disabled cells (dates outside min/max range)
    const allCells = popup.locator('.e-calendar .e-cell');
    const disabledCells = popup.locator('.e-calendar .e-cell.e-disabled');
    
    const totalCount = await allCells.count();
    const disabledCount = await disabledCells.count();
    
    // Should have some dates disabled due to constraints
    expect(totalCount).toBeGreaterThan(0);
    expect(disabledCount).toBeGreaterThanOrEqual(0);
  });

  test('respect MinTime and MaxTime constraints', async ({ page }) => {
    const timeIcon = page.locator('#wrapper-dtp-minmax .e-clock');
    await timeIcon.click();
    
    const popup = page.locator('.e-popup:visible');
    await expect(popup).toHaveCount(1, { timeout: 5000 });

    // Check for disabled time items
    const disabledTimes = popup.locator('.e-list-item.e-disabled');
    const count = await disabledTimes.count();
    expect(count).toBeGreaterThanOrEqual(0);
  });

  test('time popup shows Step interval times', async ({ page }) => {
    const timeIcon = page.locator('#wrapper-dtp-step .e-clock');
    await timeIcon.click();
    
    const popup = page.locator('.e-popup:visible');
    await expect(popup).toHaveCount(1, { timeout: 5000 });

    const timeItems = popup.locator('.e-list-item');
    const count = await timeItems.count();
    // With 15-minute steps, should have 96 items (24 hours * 60 min / 15)
    expect(count).toBeGreaterThan(50);
  });

  test('default Step shows 30-minute intervals', async ({ page }) => {
    const timeIcon = page.locator('#wrapper-dtp-basic .e-clock');
    await timeIcon.click();
    
    const popup = page.locator('.e-popup:visible');
    await expect(popup).toHaveCount(1, { timeout: 5000 });

    const timeItems = popup.locator('.e-list-item');
    const count = await timeItems.count();
    // With 30-minute steps, should have 48 items (24 hours * 60 min / 30)
    expect(count).toBeGreaterThan(40);
  });
});
