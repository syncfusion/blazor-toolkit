import { test, expect } from '@playwright/test';

test.describe('Checkbox – Value Binding', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/checkbox/value-binding');
    await page.waitForLoadState('networkidle');
  });

  test('Bool binding updates value text', async ({ page }) => {
    const checkbox = page.locator('input[type="checkbox"]').first();

    // Scope explicitly to "Direct Bool Binding" section
    const boundValue = page.locator(
      'p:has-text("Bound Value") strong'
    ).first();

    await checkbox.check();

    await expect(boundValue).toHaveText('True');
  });

test('Nullable bool displays null initially', async ({ page }) => {
  const nullableValue = page
    .locator('p:has-text("Bound Value") strong')
    .nth(1);

  await expect(nullableValue).toContainText('null');
});

  test('Two-way binding syncs checkbox and native input', async ({ page }) => {
    const sfCheckbox = page.locator('input[type="checkbox"]').nth(2);
    const nativeCheckbox = page.locator('input[type="checkbox"]').nth(3);

    await sfCheckbox.check();

    await expect(nativeCheckbox).toBeChecked();
  });

});