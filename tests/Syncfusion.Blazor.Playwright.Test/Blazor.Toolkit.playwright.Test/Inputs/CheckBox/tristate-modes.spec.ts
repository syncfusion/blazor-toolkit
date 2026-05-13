import { test, expect } from '@playwright/test';

test.describe('Tri-State Mode & Advanced States', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/checkbox/indeterminate-state');
    await page.waitForLoadState('networkidle');
  });

  test('Tri-state checkbox remains stable across state changes', async ({ page }) => {
    const checkbox = page.locator('input[type="checkbox"]').nth(1);

    await checkbox.click();
    await checkbox.click();
    await checkbox.click();

    await expect(checkbox).toBeVisible();
    await expect(checkbox).toBeEnabled();
  });

  test('Tri-state nullable binding allows repeated state transitions', async ({ page }) => {
    const checkbox = page.locator('input[type="checkbox"]').nth(4);

    await checkbox.click();
    await checkbox.click();
    await checkbox.click();

    await expect(checkbox).toBeVisible();
    await expect(checkbox).toBeEnabled();
  });
});
