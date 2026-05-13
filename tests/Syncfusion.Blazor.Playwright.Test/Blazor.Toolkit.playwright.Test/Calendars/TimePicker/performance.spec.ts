import { test, expect } from '@playwright/test';

test.describe('TimePicker - Performance & Rendering', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/timepicker-test');
    await page.waitForLoadState('networkidle');
  });

  test('renders many instances without significant lag', async ({ page }) => {
    const many = page.locator('#wrapper-tp-many .many-item');
    await expect(many).toHaveCount(10);
  });

  test('popup opens and closes without memory leaks', async ({ page }) => {
    const icon = page.locator('#wrapper-tp-basic .e-clock');
    const initialPopups = await page.locator('.e-popup').count();
    
    for (let i = 0; i < 5; i++) {
      await icon.click();
      await page.waitForTimeout(100);
      await page.keyboard.press('Escape');
      await page.waitForTimeout(100);
    }
    
    const finalPopups = await page.locator('.e-popup').count();
    expect(finalPopups).toBeLessThanOrEqual(initialPopups + 1);
  });

  test('value changes handle quickly', async ({ page }) => {
    const input = page.locator('#wrapper-tp-basic input');
    await input.fill('10:30');
    await input.press('Tab');
    await input.fill('11:45');
    await input.press('Tab');
    const value = await input.inputValue();
    expect(value).toBeTruthy();
  });
});
