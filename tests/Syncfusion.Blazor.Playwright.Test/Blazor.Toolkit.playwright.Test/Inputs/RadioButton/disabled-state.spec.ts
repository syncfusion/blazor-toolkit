// Disabled State Testing
// Tests REAL SfRadioButton components from the Blazor sample app

import { test, expect } from '@playwright/test';

test.describe('Disabled State', () => {
  test.beforeEach(async ({ page }) => {
    // Navigate to the disabled state sample page
    await page.goto('http://localhost:5000/radio-button/disabled-state');
    await page.waitForLoadState('networkidle');
  });

  test('Test disabled radio button cannot be interacted with', async ({ page }) => {
    // Find a disabled radio button in the "Enabled and Disabled States" section
    const disabledButton = page.locator('input[type="radio"][disabled]').first();
    
    await expect(disabledButton).toBeDisabled();

    // Initial state
    const initialState = await disabledButton.isChecked();
    expect(initialState).toBe(false);

    // Try to click label for disabled button
    const disabledWrapper = disabledButton.locator('..');
    const label = disabledWrapper.locator('label');
    await label.click({ force: true });
    
    // Should remain unchecked
    await expect(disabledButton).not.toBeChecked({ timeout: 1000 });

    // Final sanity: still disabled
    await expect(disabledButton).toBeDisabled();
  });

  test('Test disabled and enabled buttons in same group', async ({ page }) => {
    // The page has a group with enabled and disabled options
    const enabledRadio = page.locator('input[type="radio"][name="state-group"][value="enabled"]');
    const disabledRadio = page.locator('input[type="radio"][name="state-group"][disabled]');

    // Enabled should be clickable
    await expect(enabledRadio).not.toBeDisabled();
    
    // Disabled should be disabled
    await expect(disabledRadio).toBeDisabled();

    // Select enabled button
    await enabledRadio.click();
    await expect(enabledRadio).toBeChecked();
    await expect(disabledRadio).not.toBeChecked();
  });

  test('Test dynamic disable/enable functionality', async ({ page }) => {
    // Navigate to dynamic disable test section
    const disableToggleButton = page.locator('button').filter({ hasText: /Disable|Enable/ }).first();
    const dynamicGroup = page.locator('input[name="dynamic-group"]');

    // Initially they should be enabled
    const firstDynamic = dynamicGroup.first();
    await expect(firstDynamic).not.toBeDisabled();

    // Click the toggle button to disable them
    await disableToggleButton.click();
    
    // Now they should be disabled
    await expect(firstDynamic).toBeDisabled();

    // Click again to enable
    await disableToggleButton.click();
    
    // Should be enabled again
    await expect(firstDynamic).not.toBeDisabled();
  });

  test('Test enabled button displays correctly', async ({ page }) => {
    const enabledButton = page.locator('input[type="radio"][name="state-group"][value="enabled"]');
    
    // Should not have disabled attribute
    await expect(enabledButton).toBeEnabled();
    const disabledAttr = await enabledButton.getAttribute('disabled');
    expect(disabledAttr).toBeNull();

    // Click works
    await enabledButton.click();
    await expect(enabledButton).toBeChecked();
  });
});
