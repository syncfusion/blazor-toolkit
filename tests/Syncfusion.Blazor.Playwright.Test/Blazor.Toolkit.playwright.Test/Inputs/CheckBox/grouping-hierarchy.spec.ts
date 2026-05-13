import { test, expect } from '@playwright/test';

test.describe('Grouping & Hierarchical Selection', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/checkbox/hierarchical-selection');
    await page.waitForLoadState('networkidle');
  });

  test('Select all functionality', async ({ page }) => {
    const parent = page.locator('#parent-flow');
    const children = page.locator('#cf1, #cf2, #cf3');

    await children.nth(0).check();
    await children.nth(1).check();
    await children.nth(2).check();

    await expect(parent).toBeChecked();
  });

  test('Partial child selection does not check parent', async ({ page }) => {
    const parent = page.locator('#parent-flow');
    const child = page.locator('#cf1');

    await child.check();

    // ❌ native input.indeterminate is NOT supported
    // ✅ valid assertion: parent not fully checked
    await expect(parent).not.toBeChecked();
  });

  test('Parent click syncs children', async ({ page }) => {
    const parent = page.locator('#parent-flow');
    const children = page.locator('#cf1, #cf2, #cf3');

    await parent.click();
    for (let i = 0; i < 3; i++) {
      await expect(children.nth(i)).toBeChecked();
    }

    await parent.click();
    for (let i = 0; i < 3; i++) {
      await expect(children.nth(i)).not.toBeChecked();
    }
  });
});
