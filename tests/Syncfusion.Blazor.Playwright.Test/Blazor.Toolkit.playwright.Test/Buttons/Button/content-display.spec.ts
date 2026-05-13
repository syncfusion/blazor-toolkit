// Content & Display tests for SfButton
// Tests the REAL Syncfusion Button component from the Blazor sample app

import { test, expect } from '@playwright/test';

test.describe('Content & Display', () => {
  test.beforeEach(async ({ page }) => {
    // Navigate to the real Blazor sample page
    await page.goto('http://localhost:5000/button/basic-rendering');
    // Wait for the page to load properly
    await page.waitForLoadState('networkidle');
  });

  test('Render button with string content', async ({ page }) => {
    // Verify button renders successfully with text content
    const button = page.locator('#btn-string-content');
    await expect(button).toBeVisible();
    
    // Verify button displays text 'Click Me'
    await expect(button).toHaveText('Click Me');
    
    // Verify button is visible on the page
    const isVisible = await button.isVisible();
    expect(isVisible).toBe(true);
    
    // Verify button has proper padding and height (check bounding box)
    const boundingBox = await button.boundingBox();
    expect(boundingBox?.height).toBeGreaterThan(0);
    expect(boundingBox?.width).toBeGreaterThan(0);
  });

  test('Render button with empty content', async ({ page }) => {
    // Verify button renders with no visible text
    const button = page.locator('#btn-empty-content');
    await expect(button).toBeVisible();
    
    // Verify button still has height and width
    const boundingBox = await button.boundingBox();
    expect(boundingBox?.height).toBeGreaterThan(0);
    expect(boundingBox?.width).toBeGreaterThan(0);
    
    // Verify button element exists in DOM
    const count = await page.locator('button').count();
    expect(count).toBeGreaterThan(0);
    
    // Verify button is clickable even without content
    await button.click();
    await expect(button).toBeFocused();
  });

  test('Render button with child HTML content', async ({ page }) => {
    // Verify button renders with custom HTML child content
    const button = page.locator('#btn-html-content');
    await expect(button).toBeVisible();
    
    // Verify button text content
    await expect(button).toContainText('Save Document');
    
    // Verify custom HTML elements display correctly inside button
    const icon = button.locator('.e-icons');
    await expect(icon).toBeVisible();
    
    // Verify button layout adapts to child content size
    const boundingBox = await button.boundingBox();
    expect(boundingBox?.width).toBeGreaterThan(50);
  });

  test('Icon-only button renders correctly', async ({ page }) => {
    // Verify icon-only button renders
    const button = page.locator('#btn-icon-only');
    await expect(button).toBeVisible();
    
    // Verify icon is present
    const icon = button.locator('.e-icons');
    await expect(icon).toBeVisible();
    
    // Verify button has proper dimensions
    const boundingBox = await button.boundingBox();
    expect(boundingBox?.height).toBeGreaterThan(0);
    expect(boundingBox?.width).toBeGreaterThan(0);
  });
});
