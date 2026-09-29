import { test, expect } from '@playwright/test';

test.describe('DatePicker - Edge Cases & Special Scenarios', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/datepicker-test');
    await page.waitForLoadState('networkidle');
  });

  test('rapid open/close cycles do not leak', async ({ page }) => {
    const icon = page.locator('#wrapper-dp-basic .e-timeline-today');
    const input = page.locator('#wrapper-dp-basic input');
    const popup = page.locator('#dp-basic_popup');
    for (let i = 0; i < 10; i++) {
      await icon.click();
      await expect(input).toHaveAttribute('aria-expanded', 'true');
      await expect(popup).toBeVisible();
      await expect(page.locator('.e-popup')).toHaveCount(1);
      await icon.click();
      await expect(input).toHaveAttribute('aria-expanded', 'false');
      await expect(popup).toBeHidden();
    }
    await expect(page.locator('.e-popup:visible')).toHaveCount(0);
  });

  test('invalid typed input does not crash component', async ({ page }) => {
    const input = page.locator('#wrapper-dp-basic input');
    await input.fill('invalid-date');
    await input.press('Tab');
    // should not crash; input remains present
    await expect(input).toBeVisible();
  });
});
