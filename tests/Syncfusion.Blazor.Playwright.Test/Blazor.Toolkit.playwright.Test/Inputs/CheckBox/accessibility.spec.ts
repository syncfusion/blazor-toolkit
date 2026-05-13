import { test, expect } from '@playwright/test';

test.describe('Accessibility & ARIA', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/checkbox/label-positioning');
    await page.waitForLoadState('networkidle');
  });

  test('HTML structure uses native checkbox semantics', async ({ page }) => {
    const input = page.locator('input[type="checkbox"]').first();
    const label = page.locator('label').first();

    // Native checkbox input
    await expect(input).toBeVisible();
    await expect(input).toHaveAttribute('type', 'checkbox');

    // Label exists (visual or accessible)
    await expect(label).toBeVisible();

    // Syncfusion styling is applied
    const classList = await input.getAttribute('class');
    expect(classList ?? '').toContain('e-checkbox');
  });

  test('Checkbox is operable via mouse interaction', async ({ page }) => {
    const checkbox = page.locator('input[type="checkbox"]').first();

    await expect(checkbox).not.toBeChecked();

    await checkbox.click();

    await expect(checkbox).toBeChecked();
  });

  test('Checkbox is operable via keyboard (Space key)', async ({ page }) => {
    const checkbox = page.locator('input[type="checkbox"]').first();

    await checkbox.focus();

    await checkbox.press('Space');

    await expect(checkbox).toBeChecked();
  });

  test('Checkbox remains focusable for accessibility', async ({ page }) => {
    const checkbox = page.locator('input[type="checkbox"]').first();

    await checkbox.focus();

    const tagName = await checkbox.evaluate(el => document.activeElement === el);
    expect(tagName).toBe(true);
  });

});