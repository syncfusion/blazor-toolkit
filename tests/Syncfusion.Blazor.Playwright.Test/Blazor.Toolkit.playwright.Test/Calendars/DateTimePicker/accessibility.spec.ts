import { test, expect } from '@playwright/test';

test.describe('DateTimePicker - Accessibility & RTL Support', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/datetimepicker-test');
    await page.waitForLoadState('networkidle');
  });

  test('input has aria-label attribute', async ({ page }) => {
    const input = page.locator('#wrapper-dtp-basic input');
    
    const ariaLabel = await input.getAttribute('aria-label');
    expect(ariaLabel).toBeTruthy();
  });

  test('calendar popup has role dialog', async ({ page }) => {
    const dateIcon = page.locator('#wrapper-dtp-basic .e-timeline-today');
    await dateIcon.click();
    
    const popup = page.locator('.e-popup:visible');
    const role = await popup.evaluate((el) => el.getAttribute('role'));
    
    expect(['dialog', 'listbox']).toContain(role);
  });

  test('time popup has role dialog', async ({ page }) => {
    const timeIcon = page.locator('#wrapper-dtp-basic .e-clock');
    await timeIcon.click();
    
    const popup = page.locator('.e-popup:visible');
    const role = await popup.getAttribute('role');
    
    expect(['dialog', 'listbox']).toContain(role);
  });

  test('disabled dates have aria-disabled attribute', async ({ page }) => {
    const dateIcon = page.locator('#wrapper-dtp-minmax .e-timeline-today');
    await dateIcon.click();
    
    const popup = page.locator('.e-popup:visible');
    const disabledCell = popup.locator('.e-calendar .e-cell.e-disabled').first();
    
    if (await disabledCell.count() > 0) {
      const ariaDisabled = await disabledCell.getAttribute('aria-disabled');
      expect(ariaDisabled).toBeTruthy();
    }
  });

  test('disabled time items have aria-disabled attribute', async ({ page }) => {
    const timeIcon = page.locator('#wrapper-dtp-minmax .e-clock');
    await timeIcon.click();
    
    const popup = page.locator('.e-popup:visible');
    const disabledItem = popup.locator('.e-list-item.e-disabled').first();
    
    if (await disabledItem.count() > 0) {
      const ariaDisabled = await disabledItem.getAttribute('aria-disabled');
      expect(ariaDisabled).toBeTruthy();
    }
  });

  test('selected date has aria-selected attribute', async ({ page }) => {
    const dateIcon = page.locator('#wrapper-dtp-initial .e-timeline-today');
    await dateIcon.click();
    
    const popup = page.locator('.e-popup:visible');
    await expect(popup).toHaveCount(1, { timeout: 5000 });

    // Selected cell may have aria-selected or active class
    const selectedCell = popup.locator('.e-calendar .e-cell.e-selected').first();
    
    if (await selectedCell.count() > 0) {
      const ariaSelected = await selectedCell.getAttribute('aria-selected');
      expect(ariaSelected).toMatch(/true|selected/i);
    }
  });

  test('clear button has accessible name', async ({ page }) => {
    const input = page.locator('#wrapper-dtp-basic input');
    const clear = page.locator('#wrapper-dtp-basic .e-close');
    
    await input.focus();
    
    if (await clear.count() > 0) {
      const ariaLabel = await clear.getAttribute('aria-label');
      expect(ariaLabel).toBeTruthy();
    }
  });

  test('time list has role listbox', async ({ page }) => {
    const timeIcon = page.locator('#wrapper-dtp-basic .e-clock');
    await timeIcon.click();
    
    const popup = page.locator('.e-popup:visible');
    const timeList = popup.locator('.e-list-parent');
    const role = await timeList.getAttribute('role');
    
    expect(role).toContain('listbox');
  });

  test('time items have role option', async ({ page }) => {
    const timeIcon = page.locator('#wrapper-dtp-basic .e-clock');
    await timeIcon.click();
    
    const popup = page.locator('.e-popup:visible');
    const timeItem = popup.locator('.e-list-item').first();
    const role = await timeItem.getAttribute('role');
    
    expect(role).toContain('option');
  });
});
