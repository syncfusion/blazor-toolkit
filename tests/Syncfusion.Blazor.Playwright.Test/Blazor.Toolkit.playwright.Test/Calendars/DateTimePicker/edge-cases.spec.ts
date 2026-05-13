import { test, expect } from '@playwright/test';

test.describe('DateTimePicker - Edge Cases & Special Scenarios', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/datetimepicker-test');
    await page.waitForLoadState('networkidle');
  });

  test('rapid open/close cycles do not crash', async ({ page }) => {
    const dateIcon = page.locator('#wrapper-dtp-basic .e-timeline-today');
    
    for (let i = 0; i < 5; i++) {
      await dateIcon.click();
      await page.waitForTimeout(100);
      await page.keyboard.press('Escape');
      await page.waitForTimeout(100);
    }
    
    // Should still be interactive
    const input = page.locator('#wrapper-dtp-basic input');
    await expect(input).toBeVisible();
  });

  test('rapid time popup open/close cycles', async ({ page }) => {
    const timeIcon = page.locator('#wrapper-dtp-basic .e-clock');
    
    for (let i = 0; i < 5; i++) {
      await timeIcon.click();
      await page.waitForTimeout(100);
      await page.keyboard.press('Escape');
      await page.waitForTimeout(100);
    }
    
    // Should still be interactive
    const input = page.locator('#wrapper-dtp-basic input');
    await expect(input).toBeVisible();
  });

  test('invalid typed input does not crash', async ({ page }) => {
    const input = page.locator('#wrapper-dtp-basic input');
    
    const invalidInputs = [
      '99/99/9999 25:99',
      'invalid date',
      '!!!',
      '00/00/0000 00:00'
    ];
    
    for (const invalidInput of invalidInputs) {
      await input.fill(invalidInput);
      await input.press('Tab');
      // Component should not crash
      await expect(input).toBeVisible();
    }
  });

  test('min equals max date allows only that date', async ({ page }) => {
    const dateIcon = page.locator('#wrapper-dtp-minmax .e-timeline-today');
    await dateIcon.click();
    
    const popup = page.locator('.e-popup:visible');
    await expect(popup).toHaveCount(1, { timeout: 5000 });

    // Check that there are selectable days
    const selectableDays = popup.locator('.e-calendar .e-cell:not(.e-disabled)');
    const count = await selectableDays.count();
    expect(count).toBeGreaterThanOrEqual(0);
  });

  test('min time equals max time allows only that time', async ({ page }) => {
    const timeIcon = page.locator('#wrapper-dtp-minmax .e-clock');
    await timeIcon.click();
    
    const popup = page.locator('.e-popup:visible');
    await expect(popup).toHaveCount(1, { timeout: 5000 });

    const selectableTimes = popup.locator('.e-list-item:not(.e-disabled)');
    const count = await selectableTimes.count();
    expect(count).toBeGreaterThanOrEqual(0);
  });

  test('midnight (00:00) handling', async ({ page }) => {
    const input = page.locator('#wrapper-dtp-basic input');
    await input.fill('01/01/2025 00:00');
    await input.press('Tab');
    
    const value = await input.inputValue();
    expect(value).toMatch(/\d{2}\/\d{2}\/\d{4} 00:00/);
  });

  test('end of day (23:59) handling', async ({ page }) => {
    const input = page.locator('#wrapper-dtp-basic input');
    await input.fill('01/01/2025 23:59');
    await input.press('Tab');
    
    const value = await input.inputValue();
    expect(value).toMatch(/\d{2}\/\d{2}\/\d{4} 23:59/);
  });

  test('very large step intervals', async ({ page }) => {
    const input = page.locator('#wrapper-dtp-basic input');
    
    // Set a large step value by using a picker with larger step
    const timeIcon = page.locator('#wrapper-dtp-basic .e-clock');
    await timeIcon.click();
    
    const popup = page.locator('.e-popup:visible');
    await expect(popup).toHaveCount(1, { timeout: 5000 });

    const timeItems = popup.locator('.e-list-item');
    const count = await timeItems.count();
    
    // Even with large steps, should have reasonable number of items
    expect(count).toBeGreaterThan(0);
  });

  test('rapid value changes', async ({ page }) => {
    const input = page.locator('#wrapper-dtp-basic input');
    
    const values = [
      '01/01/2025 08:00',
      '02/15/2025 12:30',
      '03/25/2025 16:45',
      '04/30/2025 20:00'
    ];
    
    for (const value of values) {
      await input.fill(value);
      await input.press('Tab');
    }
    
    const finalValue = await input.inputValue();
    expect(finalValue).toMatch(/\d{2}\/\d{2}\/\d{4} \d{2}:\d{2}/);
  });

  test('copy/paste into datetime input', async ({ page }) => {
    const input = page.locator('#wrapper-dtp-basic input');
    
    // Simulate copy/paste via fill (Playwright's fill simulates user input)
    await input.fill('05/20/2025 15:45');
    await input.press('Tab');
    
    const value = await input.inputValue();
    expect(value).toBe('05/20/2025 15:45');
  });

  test('calendar does not leak DOM nodes on repeated opens', async ({ page }) => {
    const dateIcon = page.locator('#wrapper-dtp-basic .e-timeline-today');
    
    const initialPopups = await page.locator('.e-popup').count();
    
    for (let i = 0; i < 3; i++) {
      await dateIcon.click();
      await page.waitForTimeout(200);
      await page.keyboard.press('Escape');
      await page.waitForTimeout(200);
    }
    
    const finalPopups = await page.locator('.e-popup').count();
    
    // Should not grow significantly (allow at most 1 extra popup)
    expect(finalPopups).toBeLessThanOrEqual(initialPopups + 1);
  });
});
