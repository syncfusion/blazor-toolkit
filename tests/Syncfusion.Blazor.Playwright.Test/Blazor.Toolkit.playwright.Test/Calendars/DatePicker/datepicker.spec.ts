import { test, expect } from '@playwright/test';

test.describe('SfDatePicker - basic flows', () => {
  // Ensure the sample app is running (default base URL configured in playwright.config.ts)
  const pagePath = '/datepicker-test';

  test('renders placeholder, opens popup, selects a date, clears value', async ({ page, baseURL }) => {
    await page.goto((baseURL ?? 'http://localhost:5000') + pagePath);
    await page.waitForLoadState('networkidle');
    // input identified by ARIA label per component spec
    const input = page.locator('input[aria-label="datepicker"]').first();
    await expect(input).toBeVisible({ timeout: 5000 });

    // placeholder should be visible when no value
    await expect(input).toHaveAttribute('placeholder', 'Select a date');

    // open calendar popup by clicking the date icon
    const dateIcon = page.locator('.e-timeline-today').first();
    await expect(dateIcon).toBeVisible({ timeout: 3000 });
    await dateIcon.click();

    // popup should appear — wait for the visible popup container to avoid hidden duplicates
    const popup = page.locator('.e-popup:visible');
    await expect(popup).toHaveCount(1, { timeout: 5000 });

    // pick first enabled numeric day cell inside the visible popup
    const dayCell = popup
      .locator('.e-calendar td:not(.e-other-month)')
      .filter({ hasText: /^\d+$/ })
      .first();
    await expect(dayCell).toBeVisible({ timeout: 5000 });
    const dayText = await dayCell.textContent();
    await dayCell.click();

    // input value should update (formatted) — wait for value change with retry
    await expect(input).not.toHaveValue('', { timeout: 5000 });
    const value = await input.inputValue();
    expect(value).toContain((dayText ?? '').trim());

    // clicking clear icon should remove the value
    const clearIcon = page.locator('.e-close').first();
    await expect(clearIcon).toHaveCount(1);
    await clearIcon.click();
    await page.waitForTimeout(100);
    const cleared = await input.inputValue();
    expect(cleared === '' || cleared === null).toBeTruthy();
  });
});
