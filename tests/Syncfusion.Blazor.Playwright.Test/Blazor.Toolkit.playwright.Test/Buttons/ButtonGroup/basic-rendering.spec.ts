// Basic Rendering Test for Real SfButtonGroup Component
// Tests REAL Syncfusion ButtonGroup component from the Blazor sample app

import { test, expect } from '@playwright/test';

test.describe('ButtonGroup - Basic Rendering', () => {
  test.beforeEach(async ({ page }) => {
    // Navigate to the real Blazor sample page
    await page.goto('http://localhost:5000/button-group/basic-rendering');
    // Wait for the page to load properly
    await page.waitForLoadState('networkidle');
  });

  test('ButtonGroup wrapper renders with correct structure', async ({ page }) => {
    // Verify ButtonGroup wrapper exists
    const groupWrapper = page.locator('[role="group"]').first();
    await expect(groupWrapper).toBeVisible();

    // Verify it has expected classes
    const classList = await groupWrapper.getAttribute('class');
    expect(classList).toContain('e-btn-group');
  });

  test('ButtonGroup contains multiple buttons', async ({ page }) => {
    const groupWrapper = page.locator('[role="group"]').first();

    // Count buttons in the first group (Basic Button Group)
    const buttons = groupWrapper.locator('button, label.e-btn');
    const count = await buttons.count();

    // Should have at least 3 buttons based on sample
    expect(count).toBeGreaterThanOrEqual(3);
  });

  test('Individual buttons render correctly', async ({ page }) => {
    // Check first button
    const firstButton = page.locator('#btn-left').first();
    await expect(firstButton).toBeVisible();
    await expect(firstButton).toContainText('Left');

    // Check second button
    const secondButton = page.locator('#btn-center').first();
    await expect(secondButton).toBeVisible();
    await expect(secondButton).toContainText('Center');

    // Check third button
    const thirdButton = page.locator('#btn-right').first();
    await expect(thirdButton).toBeVisible();
    await expect(thirdButton).toContainText('Right');
  });

  test('ButtonGroup has role attribute', async ({ page }) => {
    const groupWrapper = page.locator('[role="group"]').first();

    const role = await groupWrapper.getAttribute('role');
    expect(role).toBe('group');
  });

  test('Buttons are clickable', async ({ page }) => {
    const firstButton = page.locator('#btn-left').first();

    // Verify button is enabled
    await expect(firstButton).toBeEnabled();

    // Try to click
    await firstButton.click();
    // If click succeeds without error, button is clickable
  });
});
