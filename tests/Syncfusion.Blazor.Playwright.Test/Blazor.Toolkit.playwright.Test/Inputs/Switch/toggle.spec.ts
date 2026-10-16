// Toggle/Click Test for Real SfSwitch Component
// Tests REAL Syncfusion Switch component toggle behavior

import { test, expect } from '@playwright/test';

test.describe('Switch - Toggle Functionality', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/switch/basic-rendering');
    await page.waitForLoadState('networkidle');
  });

  test('Switch toggles from OFF to ON when clicked', async ({ page }) => {
    const switchInput = page.locator('input[class*="e-switch"]').first();
    const switchWrapper = page.locator('.e-switch-wrapper').first();

    // Initial state: OFF (unchecked)
    await expect(switchInput).not.toBeChecked({ timeout: 10000 });
    
    // Click on the switch wrapper to toggle
    await switchWrapper.click();
    
    // State should be ON (checked)
    await expect(switchInput).toBeChecked({ timeout: 10000 });
    
    // Verify aria-checked updated
    await expect(switchInput).toHaveAttribute('aria-checked', 'true', { timeout: 10000 });
  });

  test('Switch toggles from ON to OFF when clicked again', async ({ page }) => {
    const switchInput = page.locator('input[class*="e-switch"]').first();
    const switchWrapper = page.locator('.e-switch-wrapper').first();

    // First toggle: OFF -> ON
    await switchWrapper.click();
    await expect(switchInput).toBeChecked({ timeout: 10000 });
    
    // Second toggle: ON -> OFF
    await switchWrapper.click();
    await expect(switchInput).not.toBeChecked({ timeout: 10000 });
    
    // Verify aria-checked is back to false
    await expect(switchInput).toHaveAttribute('aria-checked', 'false', { timeout: 10000 });
  });

  test('Multiple consecutive toggles work correctly', async ({ page }) => {
    const switchInput = page.locator('input[class*="e-switch"]').first();
    const switchWrapper = page.locator('.e-switch-wrapper').first();

    // Initial state: OFF
    await expect(switchInput).not.toBeChecked({ timeout: 10000 });
    
    // Toggle 1: OFF -> ON
    await switchWrapper.click();
    await expect(switchInput).toBeChecked({ timeout: 10000 });
    
    // Toggle 2: ON -> OFF
    await switchWrapper.click();
    await expect(switchInput).not.toBeChecked({ timeout: 10000 });
    
    // Toggle 3: OFF -> ON
    await switchWrapper.click();
    await expect(switchInput).toBeChecked({ timeout: 10000 });
    
    // Toggle 4: ON -> OFF
    await switchWrapper.click();
    await expect(switchInput).not.toBeChecked({ timeout: 10000 });
  });

  test('Switch toggles via keyboard interaction (accessibility)', async ({ page }) => {
    const switchInput = page.locator('input.e-switch').first();

    await expect(switchInput).not.toBeChecked({ timeout: 10000 });

    await switchInput.focus();
    await page.keyboard.press('Space');

    await expect(switchInput).toBeChecked({ timeout: 10000 });
  });

  test('Switch handle shows active state when ON', async ({ page }) => {
    const switchWrapper = page.locator('.e-switch-wrapper').first();
    const handle = switchWrapper.locator('.e-switch-handle');

    // Initial state: no active class
    const initialClasses = await handle.getAttribute('class');
    
    // Toggle ON
    await switchWrapper.click();
    
    // Check if handle has e-switch-active class
    await expect(handle).toHaveClass(/e-switch-active/, { timeout: 10000 });
  });

  test('Switch inner element shows active state when ON', async ({ page }) => {
    const switchWrapper = page.locator('.e-switch-wrapper').first();
    const inner = switchWrapper.locator('.e-switch-inner');

    // Toggle ON
    await switchWrapper.click();
    
    // Check if inner has e-switch-active class
    await expect(inner).toHaveClass(/e-switch-active/, { timeout: 10000 });
    
    // Toggle OFF
    await switchWrapper.click();
    
    // Check if inner no longer has e-switch-active class
    await expect(inner).not.toHaveClass(/e-switch-active/, { timeout: 10000 });
  });
});
