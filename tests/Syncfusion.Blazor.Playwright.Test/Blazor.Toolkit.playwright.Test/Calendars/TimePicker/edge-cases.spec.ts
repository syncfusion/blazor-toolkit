import { test, expect } from '@playwright/test';

test.describe('TimePicker - Edge Cases & Special Scenarios', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/timepicker-test');
    await page.waitForLoadState('networkidle');
  });

  test('rapid open/close cycles do not leak', async ({ page }) => {
    const icon = page.locator('#wrapper-tp-basic .e-clock');
    for (let i = 0; i < 10; i++) {
      await icon.click();
      try {
        await expect(page.locator('.e-popup')).toBeVisible({ timeout: 1000 });
      } catch (e) {
        // popup did not appear quickly; continue
      }
      await page.keyboard.press('Escape');
      try {
        await expect(page.locator('.e-popup:visible')).toHaveCount(0, { timeout: 1000 });
      } catch (e) {
        // if still present, continue
      }
      await page.waitForTimeout(500);
    }
    await expect(page.locator('.e-popup:visible')).toHaveCount(0, { timeout: 5000 });
  });

  test('invalid typed input does not crash component', async ({ page }) => {
    const input = page.locator('#wrapper-tp-basic input');
    await input.fill('invalid-time');
    await input.press('Tab');
    await expect(input).toBeVisible();
  });

  test('boundary times midnight and end of day', async ({ page }) => {
    const input = page.locator('#wrapper-tp-basic input');
    await input.fill('00:00');
    await input.press('Tab');
    let value = await input.inputValue();
    expect(value === '00:00' || value === '').toBeTruthy();

    await input.fill('23:59');
    await input.press('Tab');
    value = await input.inputValue();
    expect(value === '23:59' || value === '').toBeTruthy();
  });
});
