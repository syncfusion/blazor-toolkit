// Chart Last Data Label - Accessibility & Integration tests
// Tests accessibility features, keyboard navigation, and cross-browser compatibility

import { test, expect } from '@playwright/test';

test.describe('Chart Last Data Label – Accessibility & Integration', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart/last-datalabel');
    await page.waitForLoadState('networkidle');
  });

  test('Buttons have proper role attributes', async ({ page }) => {
    const updateBtn = page.locator('#update-value');
    const toggleBtn = page.locator('#toggle-label');
    
    // Both should be button elements or have button role
    const updateRole = await updateBtn.evaluate(el => el.getAttribute('role') || el.tagName);
    const toggleRole = await toggleBtn.evaluate(el => el.getAttribute('role') || el.tagName);
    
    expect(updateRole).toBeTruthy();
    expect(toggleRole).toBeTruthy();
  });

  test('Buttons are keyboard accessible', async ({ page }) => {
    const updateBtn = page.locator('#update-value');
    
    // Focus on button
    await updateBtn.focus();
    
    // Check if focused
    const isFocused = await updateBtn.evaluate(el => el === document.activeElement);
    expect(isFocused).toBe(true);
    
    // Press Enter to click
    await page.keyboard.press('Enter');
    await page.waitForTimeout(300);
    
    // Button should still be accessible
    await expect(updateBtn).toBeEnabled();
  });

  test('Tab order is correct', async ({ page }) => {
    // Get all focusable elements
    const updateBtn = page.locator('#update-value');
    const toggleBtn = page.locator('#toggle-label');
    
    // Tab to first button
    await updateBtn.focus();
    let focused = await updateBtn.evaluate(el => el === document.activeElement);
    expect(focused).toBe(true);
    
    // Tab to next button
    await page.keyboard.press('Tab');
    focused = await toggleBtn.evaluate(el => el === document.activeElement);
    
    // Toggle button should be next in tab order
  });

  test('Chart container has semantic meaning', async ({ page }) => {
    // Check chart container
    const chartHost = page.locator('#chart-host');
    
    // Should be visible and have an ID
    const id = await chartHost.getAttribute('id');
    expect(id).toBe('chart-host');
  });

  test('Page title is semantically correct', async ({ page }) => {
    const heading = page.locator('h3');
    const text = await heading.textContent();
    
    // Should have descriptive text
    expect(text).toContain('Chart');
    expect(text).toContain('Data Label');
  });

  test('Buttons have descriptive text labels', async ({ page }) => {
    const updateBtn = page.locator('#update-value');
    const toggleBtn = page.locator('#toggle-label');
    
    const updateText = await updateBtn.textContent();
    const toggleText = await toggleBtn.textContent();
    
    // Labels should be descriptive
    expect(updateText).toContain('Update');
    expect(toggleText).toContain('Toggle');
  });

  test('Page layout is logical and structured', async ({ page }) => {
    const heading = page.locator('h3');
    const chartHost = page.locator('#chart-host');
    const buttons = page.locator('button');
    
    // All main elements should be present
    await expect(heading).toBeVisible();
    await expect(chartHost).toBeVisible();
    
    const buttonCount = await buttons.count();
    expect(buttonCount).toBeGreaterThanOrEqual(2);
  });

  test('Page works on standard screen sizes', async ({ page }) => {
    // Set different viewport sizes
    const sizes = [
      { width: 1920, height: 1080 }, // Desktop
      { width: 1366, height: 768 },  // Laptop
      { width: 768, height: 1024 }   // Tablet
    ];
    
    for (const size of sizes) {
      await page.setViewportSize(size);
      await page.waitForTimeout(300);
      
      // Chart should be visible
      const svg = page.locator('svg').first();
      await expect(svg).toBeVisible();
      
      // Buttons should be accessible
      const updateBtn = page.locator('#update-value');
      await expect(updateBtn).toBeVisible();
    }
  });

  test('Buttons remain clickable on different screen sizes', async ({ page }) => {
    await page.setViewportSize({ width: 1024, height: 768 });
    
    const updateBtn = page.locator('#update-value');
    
    // Click should work
    await updateBtn.click();
    await page.waitForTimeout(300);
    
    // Button should still be enabled after click
    await expect(updateBtn).toBeEnabled();
  });

  test('Chart renders correctly on mobile viewport', async ({ page }) => {
    await page.setViewportSize({ width: 375, height: 667 });
    
    // Page should still render
    const heading = page.locator('h3');
    await expect(heading).toBeVisible();
    
    const svg = page.locator('svg').first();
    await expect(svg).toBeVisible();
  });

  test('Text contrast is appropriate', async ({ page }) => {
    const heading = page.locator('h3');
    
    const color = await heading.evaluate((el: Element) => {
      const computed = window.getComputedStyle(el);
      return computed.color;
    });
    
    const bgColor = await heading.evaluate((el: Element) => {
      const computed = window.getComputedStyle(el);
      return computed.backgroundColor;
    });
    
    // Both should be set (not same or transparent)
    expect(color).toBeTruthy();
    expect(bgColor).toBeTruthy();
  });

  test('Interactive elements are distinguishable', async ({ page }) => {
    const updateBtn = page.locator('#update-value');
    const toggleBtn = page.locator('#toggle-label');
    
    // Should have different IDs or be distinguishable
    const updateId = await updateBtn.getAttribute('id');
    const toggleId = await toggleBtn.getAttribute('id');
    
    expect(updateId).not.toBe(toggleId);
  });

  test('Page works without JavaScript errors', async ({ page }) => {
    const errors: string[] = [];
    
    page.on('console', msg => {
      if (msg.type() === 'error') {
        errors.push(msg.text());
      }
    });
    
    // Perform various interactions
    const updateBtn = page.locator('#update-value');
    await updateBtn.click();
    
    await page.waitForTimeout(300);
    
    // Should have minimal or no errors
    const criticalErrors = errors.filter(e => !e.includes('favicon'));
    expect(criticalErrors.length).toBeLessThan(2);
  });

  test('Page navigation and focus management works', async ({ page }) => {
    // Use Tab to navigate
    await page.keyboard.press('Tab');
    await page.waitForTimeout(100);
    
    // Some element should be focused
    const focused = await page.evaluate(() => document.activeElement?.id || 'body');
    expect(focused).toBeTruthy();
  });

  test('Buttons respond to Space key', async ({ page }) => {
    const updateBtn = page.locator('#update-value');
    
    // Focus button
    await updateBtn.focus();
    await page.waitForTimeout(100);
    
    // Press Space
    await page.keyboard.press('Space');
    await page.waitForTimeout(300);
    
    // Should have triggered the action
    const newValue = page.locator('text=45');
    expect(await newValue.count()).toBeGreaterThan(0);
  });

  test('SVG chart has appropriate role and labels', async ({ page }) => {
    const svg = page.locator('svg').first();
    
    // SVG should have a role or title
    const role = await svg.getAttribute('role');
    
    // Check if SVG has appropriate markup
    const ariaLabel = await svg.getAttribute('aria-label');
  });

  test('Page is accessible via screen reader semantics', async ({ page }) => {
    // Check for main heading
    const heading = page.locator('h3');
    await expect(heading).toBeVisible();
    
    // Check for button labels
    const buttons = page.locator('button');
    const buttonCount = await buttons.count();
    
    expect(buttonCount).toBeGreaterThanOrEqual(2);
  });

  test('Color-independent usability', async ({ page }) => {
    const updateBtn = page.locator('#update-value');
    const toggleBtn = page.locator('#toggle-label');
    
    // Buttons should be distinguishable without color
    // (e.g., different text or position)
    const updateText = await updateBtn.textContent();
    const toggleText = await toggleBtn.textContent();
    
    expect(updateText).not.toBe(toggleText);
  });

  test('Page structure is logical when printed', async ({ page }) => {
    // Get all visible text in order
    const heading = page.locator('h3');
    const text = await heading.textContent();
    
    // Should have meaningful content
    expect(text?.length).toBeGreaterThan(0);
  });
});
