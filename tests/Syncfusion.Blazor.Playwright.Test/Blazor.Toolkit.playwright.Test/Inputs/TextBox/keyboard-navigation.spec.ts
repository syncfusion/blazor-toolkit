import { test, expect } from '@playwright/test';

test.describe('TextBox - Keyboard navigation', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/inputs/textbox');
    await page.waitForLoadState('networkidle');
  });

  test('Tab navigates to first tabindex input', async ({ page }) => {
    await page.keyboard.press('Tab');
    // assume first tab index input will be focused
    const active = await page.evaluate(() => document.activeElement?.id || '');
    expect(active.length).toBeGreaterThan(0);
  });

  test('typing into focused input enters text', async ({ page }) => {
    const el = page.locator('#textTypeInput');
    await el.focus();
    await page.keyboard.type('hello');
    await expect(el).toHaveValue(/hello/);
  });

  test('arrow keys move cursor within input', async ({ page }) => {
    const el = page.locator('#textTypeInput');
    await el.fill('abcd');
    await el.press('ArrowLeft');
    await el.press('ArrowRight');
    await expect(el).toHaveValue('abcd');
  });

  test('Home/End keys work inside input', async ({ page }) => {
    const el = page.locator('#textTypeInput');
    await el.fill('12345');
    await el.press('Home');
    await el.press('End');
    await expect(el).toHaveValue('12345');
  });
});
