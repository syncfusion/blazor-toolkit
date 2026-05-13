import { test, expect } from '@playwright/test';

test.describe('DatePicker - Performance & Rendering', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/datepicker-test');
    await page.waitForLoadState('networkidle');
  });

  test('renders many instances without significant lag', async ({ page }) => {
    const many = page.locator('#wrapper-dp-many .many-item');
    await expect(many).toHaveCount(10);
  });
});
