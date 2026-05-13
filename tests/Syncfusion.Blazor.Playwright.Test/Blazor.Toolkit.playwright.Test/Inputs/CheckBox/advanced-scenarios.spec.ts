import { test, expect } from '@playwright/test';

test.describe('Label Accessibility & Special Cases', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/checkbox/advanced-scenarios');
    await page.waitForLoadState('networkidle');
  });

  test('No label with accessibility aria-label', async ({ page }) => {
    const checkbox = page.locator('#no-label-cb');

    await expect(checkbox).toBeVisible();
    await expect(checkbox).toBeEnabled();

    await checkbox.click();
    await expect(checkbox).toBeChecked();
  });

  test('Long label text handling', async ({ page }) => {
    const checkbox = page.locator('input[type="checkbox"]').nth(1);
    const label = checkbox.locator('../..').locator('label').first();

    const labelText = await label.textContent();
    expect((labelText ?? '').length).toBeGreaterThan(100);

    await checkbox.click();
    await expect(checkbox).toBeChecked();
  });

  test('Label with special characters', async ({ page }) => {
    const checkbox = page.locator('input[type="checkbox"]').nth(2);
    const label = checkbox.locator('../..').locator('label').first();

    const labelText = await label.textContent();
    expect(labelText).toContain('©');
    expect(labelText).toContain('&');

    await checkbox.click();
    await expect(checkbox).toBeChecked();
  });
});

test.describe('Tri-State Cycle Scenarios', () => {
  test('Tri-state checkbox remains stable across multiple clicks', async ({ page }) => {
    await page.goto('http://localhost:5000/checkbox/advanced-scenarios');
    await page.waitForLoadState('networkidle');

    const checkbox = page.locator('#cycle-cb');
    await checkbox.click();
    await checkbox.click();
    await checkbox.click();

    await expect(checkbox).toBeVisible();
    await expect(checkbox).toBeEnabled();
  });
});

test.describe('Custom Attributes & Advanced Properties', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/checkbox/advanced-scenarios');
    await page.waitForLoadState('networkidle');
  });

  test('Custom attributes do not break checkbox functionality', async ({ page }) => {
    const checkbox = page.locator('#complex-cb');

    await expect(checkbox).toBeVisible();

    await checkbox.check();
    await expect(checkbox).toBeChecked();

    await checkbox.uncheck();
    await expect(checkbox).not.toBeChecked();
  });
});