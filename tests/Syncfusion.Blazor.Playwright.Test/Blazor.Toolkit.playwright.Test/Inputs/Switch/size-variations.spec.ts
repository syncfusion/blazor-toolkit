// Size Variations Test for Real SfSwitch Component
// Tests different CSS size classes

import { test, expect } from '@playwright/test';

test.describe('Switch - Size Variations', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/switch/size-variations');
    await page.waitForLoadState('networkidle');
  });

  test('Small switch renders with e-small class', async ({ page }) => {
    // Find the switch with e-small class
    const smallSwitch = page.locator('.e-switch-wrapper.e-small').first();

    // Verify it exists and is visible
    await expect(smallSwitch).toBeVisible();

    // Verify the input inside
    const switchInput = smallSwitch.locator('input[type="checkbox"]');
    await expect(switchInput).toBeVisible();
  });

  test('Large switch renders with e-bigger class', async ({ page }) => {
    // Find the switch with e-bigger class
    const largeSwitch = page.locator('.e-switch-wrapper.e-bigger').first();

    // Verify it exists and is visible
    await expect(largeSwitch).toBeVisible();

    // Verify the input inside
    const switchInput = largeSwitch.locator('input[type="checkbox"]');
    await expect(switchInput).toBeVisible();
  });

  test('Default switch renders without size modifier', async ({ page }) => {
    // Find the switch wrapper without size classes
    const defaultSwitch = page.locator('.e-switch-wrapper:not(.e-small):not(.e-bigger)').first();

    // Verify it exists
    await expect(defaultSwitch).toBeVisible();
  });

  test('Different size switches function independently', async ({ page }) => {
    const smallSwitch = page.locator('.e-switch-wrapper.e-small').first();
    const defaultSwitch = page.locator('.e-switch-wrapper:not(.e-small):not(.e-bigger)').first();
    const largeSwitch = page.locator('.e-switch-wrapper.e-bigger').first();

    const smallInput = smallSwitch.locator('input[type="checkbox"]');
    const defaultInput = defaultSwitch.locator('input[type="checkbox"]');
    const largeInput = largeSwitch.locator('input[type="checkbox"]');

    // Toggle each independently
    await smallSwitch.click();
    await expect(smallInput).toBeChecked();
    await expect(defaultInput).not.toBeChecked();
    await expect(largeInput).not.toBeChecked();

    await defaultSwitch.click();
    await expect(smallInput).toBeChecked();
    await expect(defaultInput).toBeChecked();
    await expect(largeInput).not.toBeChecked();

    await largeSwitch.click();
    await expect(smallInput).toBeChecked();
    await expect(defaultInput).toBeChecked();
    await expect(largeInput).toBeChecked();
  });

  test('Size modification does not affect toggle functionality', async ({ page }) => {
    const smallSwitch = page.locator('.e-switch-wrapper.e-small').first();
    const smallInput = smallSwitch.locator('input[type="checkbox"]');

    // Initial OFF
    await expect(smallInput).not.toBeChecked();

    // Toggle ON
    await smallSwitch.click();
    await expect(smallInput).toBeChecked();

    // Toggle OFF
    await smallSwitch.click();
    await expect(smallInput).not.toBeChecked();
  });
});
