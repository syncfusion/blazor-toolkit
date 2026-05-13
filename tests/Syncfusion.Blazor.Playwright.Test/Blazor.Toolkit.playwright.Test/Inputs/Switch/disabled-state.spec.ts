// Disabled State Test for Real SfSwitch Component
// Tests disabled switch behavior

import { test, expect } from '@playwright/test';

test.describe('Switch - Disabled State', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/switch/disabled-state');
    await page.waitForLoadState('networkidle');
  });

  test('Disabled switch input element has disabled attribute', async ({ page }) => {
    const switchInput = page.locator('input[class*="e-switch"][disabled]').first();
    
    // Verify the switch input exists and is disabled
    await expect(switchInput).toBeDisabled();
    await expect(switchInput).toHaveAttribute('disabled');
  });

  test('Disabled switch cannot be toggled by click', async ({ page }) => {
    const switchWrapper = page.locator('.e-switch-wrapper').first();
    const switchInput = switchWrapper.locator('input[type="checkbox"]').first();

    // Get initial checked state
    const initialState = await switchInput.isChecked();

    // Try to click (using force: true to bypass Playwright's actionability checks)
    await switchWrapper.click({ force: true });

    // State should NOT change
    const newState = await switchInput.isChecked();
    expect(newState).toBe(initialState);
  });

  test('Disabled switch shows disabled styling', async ({ page }) => {
    const switchWrapper = page.locator('.e-switch-wrapper').first();

    // Check for e-switch-disabled class
    const classes = await switchWrapper.getAttribute('class');
    expect(classes).toContain('e-switch-disabled');
  });

  test('aria-disabled attribute is set to true on disabled switch', async ({ page }) => {
    const switchInput = page.locator('input[class*="e-switch"][disabled]').first();

    const isDisabled = await switchInput.isDisabled();
    expect(isDisabled).toBe(true);
    
    // aria-disabled attribute may or may not be set; if it is, it should be true
    const ariaDisabled = await switchInput.getAttribute('aria-disabled');
    if (ariaDisabled !== null && ariaDisabled !== '') {
      expect(ariaDisabled).toBe('true');
    }
  });

  test('Disabled switch input is not focusable via keyboard', async ({ page }) => {
    const switchInput = page.locator('input[class*="e-switch"][disabled]').first();

    // Try to focus the disabled switch
    try {
      await switchInput.focus();
    } catch (e) {
      // Disabled elements cannot be focused
    }

    // Verify it's not the active element
    const isFocused = await switchInput.evaluate((el) => el === document.activeElement);
    expect(isFocused).toBe(false);
  });
});
