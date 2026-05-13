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
    await expect(switchInput).not.toBeChecked();
    
    // Click on the switch wrapper to toggle
    await switchWrapper.click();
    
    // State should be ON (checked)
    await expect(switchInput).toBeChecked();
    
    // Verify aria-checked updated
    const ariaChecked = await switchInput.getAttribute('aria-checked');
    expect(ariaChecked).toBe('true');
  });

  test('Switch toggles from ON to OFF when clicked again', async ({ page }) => {
    const switchInput = page.locator('input[class*="e-switch"]').first();
    const switchWrapper = page.locator('.e-switch-wrapper').first();

    // First toggle: OFF -> ON
    await switchWrapper.click();
    await expect(switchInput).toBeChecked();
    
    // Second toggle: ON -> OFF
    await switchWrapper.click();
    await expect(switchInput).not.toBeChecked();
    
    // Verify aria-checked is back to false
    const ariaChecked = await switchInput.getAttribute('aria-checked');
    expect(ariaChecked).toBe('false');
  });

  test('Multiple consecutive toggles work correctly', async ({ page }) => {
    const switchInput = page.locator('input[class*="e-switch"]').first();
    const switchWrapper = page.locator('.e-switch-wrapper').first();

    // Initial state: OFF
    await expect(switchInput).not.toBeChecked();
    
    // Toggle 1: OFF -> ON
    await switchWrapper.click();
    await expect(switchInput).toBeChecked();
    
    // Toggle 2: ON -> OFF
    await switchWrapper.click();
    await expect(switchInput).not.toBeChecked();
    
    // Toggle 3: OFF -> ON
    await switchWrapper.click();
    await expect(switchInput).toBeChecked();
    
    // Toggle 4: ON -> OFF
    await switchWrapper.click();
    await expect(switchInput).not.toBeChecked();
  });

test('Switch toggles via keyboard interaction (accessibility)', async ({ page }) => {
  const switchInput = page.locator('input.e-switch').first();

  await expect(switchInput).not.toBeChecked();

  await switchInput.focus();
  await page.keyboard.press('Space');

  await expect(switchInput).toBeChecked();
});

  test('Switch handle shows active state when ON', async ({ page }) => {
    const switchWrapper = page.locator('.e-switch-wrapper').first();
    const handle = switchWrapper.locator('.e-switch-handle');

    // Initial state: no active class
    const initialClasses = await handle.getAttribute('class');
    
    // Toggle ON
    await switchWrapper.click();
    
    // Check if handle has e-switch-active class
    await expect(handle).toHaveClass(/e-switch-active/);
  });

  test('Switch inner element shows active state when ON', async ({ page }) => {
    const switchWrapper = page.locator('.e-switch-wrapper').first();
    const inner = switchWrapper.locator('.e-switch-inner');

    // Toggle ON
    await switchWrapper.click();
    
    // Check if inner has e-switch-active class
    await expect(inner).toHaveClass(/e-switch-active/);
    
    // Toggle OFF
    await switchWrapper.click();
    
    // Check if inner no longer has e-switch-active class
    await expect(inner).not.toHaveClass(/e-switch-active/);
  });
});
