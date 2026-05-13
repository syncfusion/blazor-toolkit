// Basic rendering spec for SfRadioButton components
// Tests the REAL Syncfusion RadioButton component from the Blazor sample app

import { test, expect } from '@playwright/test';

test.describe('Basic Rendering and UI', () => {
  test.beforeEach(async ({ page }) => {
    // Navigate to the real Blazor sample page
    await page.goto('http://localhost:5000/radio-button/basic-rendering');
    // Wait for the page to load properly
    await page.waitForLoadState('networkidle');
  });

  test('Verify radio button basic structure rendering', async ({ page }) => {
    // Test the first radio button in the group
    const firstRadioWrapper = page.locator('.e-radio-wrapper').first();
    const inputElement = firstRadioWrapper.locator('input[type="radio"]');
    const labelElement = firstRadioWrapper.locator('label');

    await expect(firstRadioWrapper).toHaveClass(/e-radio-wrapper/);
    await expect(firstRadioWrapper).toHaveClass(/e-wrapper/);

    await expect(inputElement).toHaveAttribute('type', 'radio');
    await expect(inputElement).toHaveClass(/e-control/);
    await expect(inputElement).toHaveClass(/e-radio/);
    await expect(inputElement).toHaveClass(/e-lib/);

    // Label should exist and have text
    await expect(labelElement).toBeVisible();
    const labelText = (await labelElement.textContent())?.trim();
    expect(labelText).toBeTruthy();

    // Label should be connected to input via 'for' attribute
    const inputId = await inputElement.getAttribute('id');
    const labelFor = await labelElement.getAttribute('for');
    expect(inputId).toBe(labelFor);

    // First radio button should be checked by default (since "opt0" is the default selected value)
    await expect(inputElement).toBeChecked();
  });

  test('Verify multiple radio buttons in group', async ({ page }) => {
    // Test that we have all three radio buttons rendered
    const radioWrappers = page.locator('.e-radio-wrapper');
    await expect(radioWrappers).toHaveCount(3);

    // Check all radio buttons have proper attributes
    const radioInputs = page.locator('.e-radio-wrapper input[type="radio"]');
    for (let i = 0; i < 3; i++) {
      const input = radioInputs.nth(i);
      await expect(input).toHaveAttribute('name', 'basic-group');
      await expect(input).toHaveAttribute('value', new RegExp(`opt${i}`));
      await expect(input).toHaveClass(/e-control/);
      await expect(input).toHaveClass(/e-radio/);
    }
  });

  test('Verify label text is rendered correctly', async ({ page }) => {
    const labels = page.locator('.e-radio-wrapper label');
    await expect(labels.first()).toContainText('Option 0');
    await expect(labels.nth(1)).toContainText('Option 1');
    await expect(labels.nth(2)).toContainText('Option 2');
  });

  test('Verify radio button click changes selection', async ({ page }) => {
    // Initially, Option 0 should be selected
    const firstInput = page.locator('.e-radio-wrapper input[value="opt0"]');
    await expect(firstInput).toBeChecked();

    // Click on Option 1
    const secondLabel = page.locator('.e-radio-wrapper label').nth(1);
    await secondLabel.click();

    // Now Option 1 should be checked
    const secondInput = page.locator('.e-radio-wrapper input[value="opt1"]');
    await expect(secondInput).toBeChecked();

    // Option 0 should no longer be checked
    await expect(firstInput).not.toBeChecked();
  });

  test('Verify radio button with label is accessible', async ({ page }) => {
    const firstRadioWrapper = page.locator('.e-radio-wrapper').first();
    const inputElement = firstRadioWrapper.locator('input[type="radio"]');
    const labelElement = firstRadioWrapper.locator('label');

    // Get the id and for attributes
    const inputId = await inputElement.getAttribute('id');
    const labelFor = await labelElement.getAttribute('for');
    
    // They should match for accessibility
    expect(inputId).toBe(labelFor);
    expect(inputId).toBeTruthy();
  });
});
