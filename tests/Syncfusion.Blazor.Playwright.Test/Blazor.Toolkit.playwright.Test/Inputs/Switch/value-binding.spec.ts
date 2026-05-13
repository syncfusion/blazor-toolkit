// Value Binding Test for Real SfSwitch Component
// Tests two-way binding with @bind-Checked

import { test, expect } from '@playwright/test';

test.describe('Switch - Value Binding', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/switch/value-binding');
    await page.waitForLoadState('networkidle');
  });

  test('Switch binds value correctly', async ({ page }) => {
    const switchInput = page.locator('input[id="switch-binding"]').first();
    const switchWrapper = page.locator('.e-switch-wrapper', { has: switchInput });
    const valueDisplay = page.locator('#binding-value');

    // Initial state: false (OFF)
    let displayValue = await valueDisplay.textContent();
    expect(displayValue?.toLowerCase()).toContain('false');

    // Toggle switch by clicking wrapper
    await switchWrapper.click();

    // Wait for value to update
    await page.waitForTimeout(100);

    // Display value should now be true
    displayValue = await valueDisplay.textContent();
    expect(displayValue?.toLowerCase()).toContain('true');
  });

  test('Programmatic value change updates switch', async ({ page }) => {
    const switchInput = page.locator('input[id="switch-binding"]').first();
    const setTrueBtn = page.locator('#set-true');
    const setFalseBtn = page.locator('#set-false');

    // Initial state: unchecked
    await expect(switchInput).not.toBeChecked();

    // Click "Set True" button
    await setTrueBtn.click();
    await page.waitForTimeout(100);

    // Switch should now be checked
    await expect(switchInput).toBeChecked();

    // Click "Set False" button
    await setFalseBtn.click();
    await page.waitForTimeout(100);

    // Switch should now be unchecked
    await expect(switchInput).not.toBeChecked();
  });

test('Two-way binding updates on user interaction', async ({ page }) => {
  const switchInput = page.locator('#switch-binding');
  const switchWrapper = page.locator('.e-switch-wrapper', { has: switchInput });
  const valueDisplay = page.locator('#binding-value');

  const toggleCount = 3;

  for (let i = 0; i < toggleCount; i++) {
    const wasChecked = await switchInput.isChecked();
    const previousValue = await valueDisplay.textContent();

    await switchWrapper.click();

    // ✅ wait on real end state
    if (wasChecked) {
      await expect(switchInput).not.toBeChecked();
    } else {
      await expect(switchInput).toBeChecked();
    }

    // ✅ binding updated
    await expect(valueDisplay).not.toHaveText(previousValue!);
  }
});

  test('Rapid clicks are processed correctly', async ({ page }) => {
    const switchInput = page.locator('input[id="switch-binding"]').first();
    const switchWrapper = page.locator('.e-switch-wrapper', { has: switchInput });
    const valueDisplay = page.locator('#binding-value');

    // Perform 3 rapid clicks on wrapper
    await switchWrapper.click();
    await switchWrapper.click();
    await switchWrapper.click();

    // Wait for processing
    await page.waitForTimeout(100);

    // After odd number of clicks, should be ON (true)
    await expect(switchInput).toBeChecked();

    const displayValue = await valueDisplay.textContent();
    expect(displayValue?.toLowerCase()).toContain('true');
  });
});
