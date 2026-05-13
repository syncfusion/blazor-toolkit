// Basic Rendering Test for Real SfSwitch Component
// Tests REAL Syncfusion Switch component from the Blazor sample app

import { test, expect } from '@playwright/test';

test.describe('Switch - Basic Rendering', () => {
  test.beforeEach(async ({ page }) => {
    // Navigate to the real Blazor sample page
    await page.goto('http://localhost:5000/switch/basic-rendering');
    // Wait for the page to load properly
    await page.waitForLoadState('networkidle');
  });

  test('Verify switch wrapper and input structure', async ({ page }) => {
    // Verify switch wrapper exists
    const switchWrapper = page.locator('.e-switch-wrapper').first();
    await expect(switchWrapper).toBeVisible();

    // Verify it has expected classes
    await expect(switchWrapper).toHaveClass(/e-wrapper/);

    // Verify input element inside
    const switchInput = switchWrapper.locator('input[type="checkbox"]').first();
    await expect(switchInput).toHaveClass(/e-control/);
    await expect(switchInput).toHaveClass(/e-switch/);
    await expect(switchInput).toHaveClass(/e-lib/);

    // Verify role attribute
    await expect(switchInput).toHaveAttribute('role', 'switch');
  });

  test('Verify switch handle and inner elements', async ({ page }) => {
    const switchWrapper = page.locator('.e-switch-wrapper').first();

    // Verify e-switch-inner exists
    const inner = switchWrapper.locator('.e-switch-inner');
    await expect(inner).toBeVisible();

    // Verify e-switch-handle exists
    const handle = switchWrapper.locator('.e-switch-handle');
    await expect(handle).toBeVisible();
  });

  test('Verify initial unchecked state', async ({ page }) => {
    const switchInput = page.locator('input[class*="e-switch"]').first();
    
    // Check initial state is unchecked (OFF)
    await expect(switchInput).not.toBeChecked();
    
    // Verify aria-checked attribute
    const ariaChecked = await switchInput.getAttribute('aria-checked');
    expect(ariaChecked).toBe('false');
  });

  test('Switch has correct tabindex for keyboard navigation', async ({ page }) => {
    const switchInput = page.locator('input[class*="e-switch"]').first();
    
    // Verify tabindex is set (should be 0 for focusable elements)
    const tabindex = await switchInput.getAttribute('tabindex');
    expect(tabindex).toBeTruthy();
  });

  test('Verify aria-label or aria-labelledby exists', async ({ page }) => {
    const switchInput = page.locator('input[class*="e-switch"]').first();
    
    // Check for either aria-label or aria-labelledby
    const ariaLabel = await switchInput.getAttribute('aria-label');
    const ariaLabelledBy = await switchInput.getAttribute('aria-labelledby');
    
    expect(ariaLabel || ariaLabelledBy).toBeTruthy();
  });

  test('Verify aria-disabled attribute set correctly', async ({ page }) => {
    const switchInput = page.locator('input[class*="e-switch"]').first();
    
    // Check if disabled attribute is not set (which means it's not disabled)
    const isDisabled = await switchInput.isDisabled();
    expect(isDisabled).toBe(false);
    
    // aria-disabled attribute may not always be set; if it is, it should be false
    const ariaDisabled = await switchInput.getAttribute('aria-disabled');
    if (ariaDisabled !== null) {
      expect(ariaDisabled).toBe('false');
    }
  });
});
