import { test, expect } from '@playwright/test';

test.describe('Checkbox – Basic Functionality', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/checkbox/basic-functionality');
    await page.waitForLoadState('networkidle');
  });

  test('Default checkbox toggles correctly', async ({ page }) => {
    const checkbox = page.locator('input[type="checkbox"]').first();

    await expect(checkbox).not.toBeChecked();
    await checkbox.click();
    await expect(checkbox).toBeChecked();
  });

  test('Pre-checked checkbox renders as checked', async ({ page }) => {
    const checkboxes = page.locator('input[type="checkbox"]');
    await expect(checkboxes.nth(2)).toBeChecked();
  });

  test('Each checkbox maintains independent state', async ({ page }) => {
    const checkboxes = page.locator('input[type="checkbox"]');

    await checkboxes.nth(0).check();
    await checkboxes.nth(1).check();

    await expect(checkboxes.nth(0)).toBeChecked();
    await expect(checkboxes.nth(1)).toBeChecked();
    await expect(checkboxes.nth(3)).not.toBeChecked();
  });

});
