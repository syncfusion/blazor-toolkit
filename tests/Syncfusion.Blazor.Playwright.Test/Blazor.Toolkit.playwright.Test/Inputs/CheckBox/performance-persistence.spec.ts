import { test, expect } from '@playwright/test';

test.describe('Performance & State Persistence', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/checkbox/performance-persistence');
    await page.waitForLoadState('networkidle');
  });

  test('Many checkboxes render and are interactive', async ({ page }) => {
    const checkboxes = page.locator('input[type="checkbox"]');

    const count = await checkboxes.count();
    expect(count).toBeGreaterThan(10);

    await checkboxes.nth(0).check();
    await checkboxes.nth(5).check();

    await expect(checkboxes.nth(0)).toBeChecked();
    await expect(checkboxes.nth(5)).toBeChecked();
  });

  test('Rapid state changes do not break checkbox behavior', async ({ page }) => {
    const checkbox = page.locator('input[type="checkbox"]').first();

    for (let i = 0; i < 5; i++) {
      await checkbox.click();
    }

    await expect(checkbox).toBeVisible();
    await expect(checkbox).toBeEnabled();
  });

  test('Multiple checkbox interactions complete within reasonable time', async ({ page }) => {
    const checkboxes = page.locator('input[type="checkbox"]');

    const startTime = Date.now();

    for (let i = 0; i < 20; i++) {
      await checkboxes.nth(i % (await checkboxes.count())).click();
    }

    const duration = Date.now() - startTime;
    expect(duration).toBeLessThan(10000);
  });

  test('Dynamic checkbox addition enables new interactions', async ({ page }) => {
    const addBtn = page.locator('button:has-text("Add 10 Checkboxes")');

    const beforeCount = await page.locator('input[type="checkbox"]').count();

    await addBtn.click();

    await page.waitForFunction(
      (prev) =>
        document.querySelectorAll('input[type="checkbox"]').length > prev,
      beforeCount
    );

    const afterCount = await page.locator('input[type="checkbox"]').count();
    expect(afterCount).toBeGreaterThan(beforeCount);
  });

  test('Multiple checkboxes maintain independent state', async ({ page }) => {
    const checkboxes = page.locator('input[type="checkbox"]');

    await checkboxes.nth(1).check();
    await checkboxes.nth(3).check();

    await expect(checkboxes.nth(1)).toBeChecked();
    await expect(checkboxes.nth(3)).toBeChecked();
  });
});