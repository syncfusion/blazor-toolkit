// Form Integration Test for SfSwitch Component
// Tests form submission, Name and Value attributes, EditForm binding

import { test, expect } from '@playwright/test';

test.describe('Switch - Form Integration', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/switch/form-integration');
    await page.waitForLoadState('networkidle');
  });

  test('Switch inputs have correct name and value attributes', async ({ page }) => {
    const newsletterInput = page.locator('#switch-newsletter');
    const termsInput = page.locator('#switch-terms');
    
    // Verify name attributes
    await expect(newsletterInput).toHaveAttribute('name', 'newsletter');
    await expect(termsInput).toHaveAttribute('name', 'terms');
    
    // Verify value attributes
    await expect(newsletterInput).toHaveAttribute('value', 'enabled');
    await expect(termsInput).toHaveAttribute('value', 'accepted');
  });

  test('Disabled form switch cannot be toggled', async ({ page }) => {
    const disabledInput = page.locator('#switch-disabled-form');
    
    // Verify disabled state
    const isDisabled = await disabledInput.isDisabled();
    expect(isDisabled).toBe(true);
    
    // Try to click wrapper (should have no effect)
    const wrapper = page.locator('.e-switch-wrapper', { has: disabledInput });
    await wrapper.click({ force: true });
    
    // Verify state unchanged
    await expect(disabledInput).not.toBeChecked();
  });

  test('Switch reflects form state changes', async ({ page }) => {
    const notificationsInput = page.locator('#switch-notifications');
    const wrapper = page.locator('.e-switch-wrapper', { has: notificationsInput });
    
    // Initial state - unchecked
    await expect(notificationsInput).not.toBeChecked();
    
    // Toggle on
    await wrapper.click();
    await expect(notificationsInput).toBeChecked();
    
    // Toggle off
    await wrapper.click();
    await expect(notificationsInput).not.toBeChecked();
  });

  test('All form switches have correct role attribute', async ({ page }) => {
    const switches = page.locator('#switch-newsletter, #switch-terms, #switch-notifications, #switch-disabled-form');
    const count = await switches.count();
    
    expect(count).toBe(4);
    
    // Verify each has role="switch"
    for (let i = 0; i < count; i++) {
      const switchElement = switches.nth(i);
      await expect(switchElement).toHaveAttribute('role', 'switch');
    }
  });

  test('Multiple form switches can be toggled independently', async ({ page }) => {
    const allSwitches = {
      newsletter: page.locator('#switch-newsletter'),
      terms: page.locator('#switch-terms'),
      notifications: page.locator('#switch-notifications'),
      disabled: page.locator('#switch-disabled-form')
    };
    
    // Toggle newsletter
    let wrapper = page.locator('.e-switch-wrapper', { has: allSwitches.newsletter });
    await wrapper.click();
    await expect(allSwitches.newsletter).toBeChecked();
    
    // Toggle terms
    wrapper = page.locator('.e-switch-wrapper', { has: allSwitches.terms });
    await wrapper.click();
    await expect(allSwitches.terms).toBeChecked();
    
    // Notifications should still be unchecked
    await expect(allSwitches.notifications).not.toBeChecked();
    
    // Disabled should remain unchecked
    await expect(allSwitches.disabled).not.toBeChecked();
  });
});