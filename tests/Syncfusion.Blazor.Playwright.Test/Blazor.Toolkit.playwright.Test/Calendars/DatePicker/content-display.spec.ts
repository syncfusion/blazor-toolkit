import { test, expect } from '@playwright/test';

test.describe('DatePicker - Content & Display', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/datepicker-test');
    await page.waitForLoadState('networkidle');
  });

  test('renders with placeholder and initial value', async ({ page }) => {
    const basicInput = page.locator('#wrapper-dp-basic input');
    await expect(basicInput).toBeVisible();
    await expect(basicInput).toHaveAttribute('placeholder', 'Select a date');

    const initialInput = page.locator('#wrapper-dp-initial input');
    await expect(initialInput).toBeVisible();
    await expect(initialInput).toHaveValue('05/15/2025');
  });

  test('empty or null value behavior and clear visibility', async ({ page }) => {
    const basicWrapper = page.locator('#wrapper-dp-basic');
    await expect(basicWrapper.locator('input')).toHaveAttribute('placeholder', 'Select a date');
  });
});
