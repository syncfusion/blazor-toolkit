// Button Styling & Appearance tests for SfButton
// Tests the REAL Syncfusion Button component from the Blazor sample app

import { test, expect } from '@playwright/test';

test.describe('Button Styling & Appearance', () => {
  test.beforeEach(async ({ page }) => {
    // Navigate to the real Blazor sample page
    await page.goto('http://localhost:5000/button/styling');
    // Wait for the page to load properly
    await page.waitForLoadState('networkidle');
  });

  test('Render default button without CSS classes', async ({ page }) => {
    const button = page.locator('#btn-default');
    await expect(button).toBeVisible();
    
    // Verify button element has e-control, e-btn, e-lib classes applied
    const classes = await button.getAttribute('class');
    expect(classes).toContain('e-control');
    expect(classes).toContain('e-btn');
  });

  test('Render outlined button variant', async ({ page }) => {
    const button = page.locator('#btn-outline');
    await expect(button).toBeVisible();
    
    // Verify button has e-outline class
    const classes = await button.getAttribute('class');
    expect(classes).toContain('e-outline');
  });

  test('Render flat button variant', async ({ page }) => {
    const button = page.locator('#btn-flat');
    await expect(button).toBeVisible();
    
    // Verify button has e-flat class
    const classes = await button.getAttribute('class');
    expect(classes).toContain('e-flat');
  });

  test('Render round button variant', async ({ page }) => {
    const button = page.locator('#btn-round');
    await expect(button).toBeVisible();
    
    // Verify button has e-round class
    const classes = await button.getAttribute('class');
    expect(classes).toContain('e-round');
  });

  test('Apply semantic color - info', async ({ page }) => {
    const button = page.locator('#btn-info');
    await expect(button).toBeVisible();
    
    // Verify button has e-info class
    const classes = await button.getAttribute('class');
    expect(classes).toContain('e-info');
  });

  test('Apply semantic color - success', async ({ page }) => {
    const button = page.locator('#btn-success');
    await expect(button).toBeVisible();
    
    // Verify button has e-success class
    const classes = await button.getAttribute('class');
    expect(classes).toContain('e-success');
  });

  test('Apply semantic color - warning', async ({ page }) => {
    const button = page.locator('#btn-warning');
    await expect(button).toBeVisible();
    
    // Verify button has e-warning class
    const classes = await button.getAttribute('class');
    expect(classes).toContain('e-warning');
  });

  test('Apply semantic color - danger', async ({ page }) => {
    const button = page.locator('#btn-danger');
    await expect(button).toBeVisible();
    
    // Verify button has e-danger class
    const classes = await button.getAttribute('class');
    expect(classes).toContain('e-danger');
  });

  test('Combine button type with semantic color', async ({ page }) => {
    const button = page.locator('#btn-outline-success');
    await expect(button).toBeVisible();
    
    // Verify button has both outline and success classes
    const classes = await button.getAttribute('class');
    expect(classes).toContain('e-outline');
    expect(classes).toContain('e-success');
  });

  test('Apply primary button styling with IsPrimary property', async ({ page }) => {
    const button = page.locator('#btn-primary');
    await expect(button).toBeVisible();
    
    // Verify button has e-primary class
    const classes = await button.getAttribute('class');
    expect(classes).toContain('e-primary');
  });

  test('Apply custom CSS classes', async ({ page }) => {
    const button = page.locator('#btn-custom-class');
    await expect(button).toBeVisible();
    
    // Verify button applies custom CSS classes
    const classes = await button.getAttribute('class');
    expect(classes).toContain('custom-button-class');
  });

  test('Size variations - small, normal, large', async ({ page }) => {
    const smallBtn = page.locator('#btn-small');
    const normalBtn = page.locator('#btn-normal');
    const largeBtn = page.locator('#btn-large');
    
    await expect(smallBtn).toBeVisible();
    await expect(normalBtn).toBeVisible();
    await expect(largeBtn).toBeVisible();
    
    // Verify size classes
    const smallClasses = await smallBtn.getAttribute('class');
    expect(smallClasses).toContain('e-small');
    
    const largeClasses = await largeBtn.getAttribute('class');
    expect(largeClasses).toContain('e-large');
  });

  test('Full width button', async ({ page }) => {
    const button = page.locator('#btn-full-width');
    await expect(button).toBeVisible();
    
    const classes = await button.getAttribute('class');
    expect(classes).toContain('e-full-width');
  });
});
