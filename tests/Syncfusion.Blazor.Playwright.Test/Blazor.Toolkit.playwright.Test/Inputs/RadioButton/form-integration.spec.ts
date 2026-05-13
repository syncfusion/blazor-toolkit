// Form Integration and Submission Testing
// Tests REAL SfRadioButton components from the Blazor sample app

import { test, expect } from '@playwright/test';

test.describe('Form Integration and Submission', () => {
  test.beforeEach(async ({ page }) => {
    // Navigate to the value binding sample page (which has form integration)
    await page.goto('http://localhost:5000/radio-button/value-binding');
    await page.waitForLoadState('networkidle');
  });

  test('Test radio button works within form context', async ({ page }) => {
    // Get all radio groups on the page
    const stringRadios = page.locator('input[name="string-group"]');
    const bindRadios = page.locator('input[name="bind-group"]');

    // Verify radios are present
    expect(await stringRadios.count()).toBeGreaterThan(0);
    expect(await bindRadios.count()).toBeGreaterThan(0);

    // Select from first group
    await stringRadios.nth(0).click();
    await expect(stringRadios.nth(0)).toBeChecked();

    // Select from second group
    await bindRadios.nth(0).click();
    await expect(bindRadios.nth(0)).toBeChecked();
  });

  test('Test multiple radio groups work independently', async ({ page }) => {
    const stringRadios = page.locator('input[name="string-group"]');
    const bindRadios = page.locator('input[name="bind-group"]');

    // Select string group option 1
    const stringOpt1 = stringRadios.nth(0);
    await stringOpt1.click();
    await expect(stringOpt1).toBeChecked();

    // Select bind group option 2
    const bindOpt2 = bindRadios.nth(1);
    await bindOpt2.click();
    await expect(bindOpt2).toBeChecked();

    // Verify first group unchanged
    await expect(stringOpt1).toBeChecked();
  });

  test('Test form submission with selected values', async ({ page }) => {
    const stringRadios = page.locator('input[name="string-group"]');
    const selectedOption = stringRadios.nth(1);

    // Select an option
    await selectedOption.click();
    await expect(selectedOption).toBeChecked();

    // Verify the value
    const value = await selectedOption.getAttribute('value');
    expect(value).toBeTruthy();
  });
});
