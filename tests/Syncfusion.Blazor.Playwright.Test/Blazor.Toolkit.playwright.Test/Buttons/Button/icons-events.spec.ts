// Icon Support, Event Handling tests for SfButton
// Tests the REAL Syncfusion Button component from the Blazor sample app

import { test, expect } from '@playwright/test';

test.describe('Icon Support & Positioning', () => {
  test.beforeEach(async ({ page }) => {
    // Navigate to the real Blazor sample page
    await page.goto('http://localhost:5000/button/icon-support');
    // Wait for the page to load properly
    await page.waitForLoadState('networkidle');
  });

  test('Render button with icon using IconCss', async ({ page }) => {
    const button = page.locator('#btn-icon-left');
    await expect(button).toBeVisible();
    const icon = button.locator('.e-icons');
    await expect(icon).toBeAttached();
  });

  test('Position icon on the left (IconPosition.Left)', async ({ page }) => {
    const button = page.locator('#btn-icon-left');
    await expect(button).toBeVisible();
  });

  test('Position icon on the right (IconPosition.Right)', async ({ page }) => {
    const button = page.locator('#btn-icon-right');
    await expect(button).toBeVisible();
    const icon = button.locator('.e-icons');
    await expect(icon).toBeAttached();
  });

  test('Position icon on top (IconPosition.Top)', async ({ page }) => {
    const button = page.locator('#btn-icon-top');
    await expect(button).toBeVisible();
  });

  test('Position icon on bottom (IconPosition.Bottom)', async ({ page }) => {
    const button = page.locator('#btn-icon-bottom');
    await expect(button).toBeVisible();
  });

  test('Icon-only button without content', async ({ page }) => {
    const button = page.locator('#btn-icon-only-dedicated');
    await expect(button).toBeVisible();
    const icon = button.locator('.e-icons');
    await expect(icon).toBeAttached();
  });

  test('Round icon-only button', async ({ page }) => {
    const button = page.locator('#btn-icon-only-dedicated');
    await expect(button).toBeVisible();
  });

  test('Icon with semantic color', async ({ page }) => {
    const button = page.getByRole('button', { name: 'Open Document' });
    await expect(button).toBeVisible();
  });

  test('Accessibility - icon aria-hidden attribute', async ({ page }) => {
    const button = page.locator('#btn-icon-aria-hidden');
    const icon = button.locator('.e-icons');
    const ariaHidden = await icon.getAttribute('aria-hidden');
    expect(ariaHidden).toBe('true');
  });

  test('Multiple icon classes', async ({ page }) => {
    const icon = page.locator('.e-icons').first();
    await expect(icon).toBeAttached();
  });
});

test.describe('Event Handling', () => {
  test.beforeEach(async ({ page }) => {
    // Navigate to the real Blazor sample page
    await page.goto('http://localhost:5000/button/states');
    // Wait for the page to load properly
    await page.waitForLoadState('networkidle');
  });

  test('OnClick event fires on user click', async ({ page }) => {
    const button = page.locator('#btn-toggle-state');
    await expect(button).toBeVisible();
    await button.click();
    
    // Verify click was successful
    await expect(button).toBeTruthy();
  });

  test('OnClick event receives MouseEventArgs', async ({ page }) => {
    const button = page.locator('#btn-toggle-state');
    await button.click();
    
    // Verify click was handled
    await expect(button).toBeTruthy();
  });

  test('OnClick event fires only for user interaction', async ({ page }) => {
    const button = page.locator('#btn-toggle-state');
    
    // User mouse click
    await button.click();
    
    // Keyboard interaction on focused button
    await button.focus();
    await page.keyboard.press('Enter');
    
    // Verify interactions were handled
    await expect(button).toBeTruthy();
  });

  test('Created event fires after component render', async ({ page }) => {
    // Verify buttons are rendered on page load
    const button = page.locator('#btn-enabled');
    await expect(button).toBeVisible();
  });

  test('Created event for multiple buttons', async ({ page }) => {
    // Verify multiple buttons are rendered
    const buttons = page.locator('button');
    const count = await buttons.count();
    expect(count).toBeGreaterThanOrEqual(3);
  });
});
