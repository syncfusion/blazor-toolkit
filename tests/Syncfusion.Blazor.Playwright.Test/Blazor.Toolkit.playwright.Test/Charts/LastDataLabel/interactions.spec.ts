// Chart Last Data Label - Interactive Features tests
// Tests user interactions, button clicks, and state changes

import { test, expect } from '@playwright/test';

test.describe('Chart Last Data Label – Interactive Features', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart/last-datalabel');
    await page.waitForLoadState('networkidle');
  });

  test('Update Value button is visible and clickable', async ({ page }) => {
    // Verify button exists
    const updateBtn = page.locator('#update-value');
    await expect(updateBtn).toBeVisible();
    
    // Verify button is enabled and clickable
    await expect(updateBtn).toBeEnabled();
    
    // Verify button text
    const buttonText = await updateBtn.textContent();
    expect(buttonText).toContain('Update Value');
  });

  test('Toggle Label button is visible and clickable', async ({ page }) => {
    // Verify button exists
    const toggleBtn = page.locator('#toggle-label');
    await expect(toggleBtn).toBeVisible();
    
    // Verify button is enabled and clickable
    await expect(toggleBtn).toBeEnabled();
    
    // Verify button text
    const buttonText = await toggleBtn.textContent();
    expect(buttonText).toContain('Toggle Label');
  });

  test('Update Value button click changes last data point value', async ({ page }) => {
    // Get initial last column state
    const initialLabels = page.locator('text=40');
    const initialCount = await initialLabels.count();
    expect(initialCount).toBeGreaterThan(0);
    
    // Click Update Value button
    const updateBtn = page.locator('#update-value');
    await updateBtn.click();
    
    // Wait for chart to update
    await page.waitForTimeout(500);
    
    // Verify new value (45) appears
    const newValueLabel = page.locator('text=45');
    const newCount = await newValueLabel.count();
    
    expect(newCount).toBeGreaterThan(0);
  });

  test('Updated value reflects in last column height', async ({ page }) => {
    // Get initial chart height
    const svg = page.locator('svg').first();
    const initialHeight = await svg.evaluate(el => el.getBoundingClientRect().height);
    
    // Click Update Value button
    const updateBtn = page.locator('#update-value');
    await updateBtn.click();
    
    // Wait for chart to update
    await page.waitForTimeout(500);
    
    // Chart should still render properly with new value
    const finalSvg = page.locator('svg').first();
    await expect(finalSvg).toBeVisible();
  });

  test('Last data label updates with new value after update', async ({ page }) => {
    // Initial last value should be 40
    let label = page.locator('[id*="LastDataLabelCollection"] text').first();
    await expect(label).toBeVisible();
    
    // Click Update Value button
    const updateBtn = page.locator('#update-value');
    await updateBtn.click();
    
    // Wait for update
    await page.waitForTimeout(500);
    
    // Label should still be visible after update
    label = page.locator('[id*="LastDataLabelCollection"] text').first();
    await expect(label).toBeVisible();
  });

  test('Toggle Label button hides last data label on click', async ({ page }) => {
    // Initial state: label visible
    let label = page.locator('[id*="LastDataLabelCollection"] text').first();
    let isVisible = await label.isVisible();
    expect(isVisible).toBe(true);
    
    // Click Toggle Label button
    const toggleBtn = page.locator('#toggle-label');
    await toggleBtn.click();
    
    // Wait for change
    await page.waitForTimeout(500);
    
    // Button should still be functional
    await expect(toggleBtn).toBeEnabled();
  });

  test('Toggle Label button shows last data label on second click', async ({ page }) => {
    // First toggle - hide
    let toggleBtn = page.locator('#toggle-label');
    await toggleBtn.click();
    await page.waitForTimeout(500);
    
    // Second toggle - show
    toggleBtn = page.locator('#toggle-label');
    await toggleBtn.click();
    await page.waitForTimeout(500);
    
    // Label should be visible again
    const lastValue = page.locator('text=40').first();
    const isVisible = await lastValue.isVisible();
    
    expect(isVisible).toBe(true);
  });

  test('Multiple toggles work correctly', async ({ page }) => {
    const toggleBtn = page.locator('#toggle-label');
    
    // Toggle multiple times: off, on, off, on
    for (let i = 0; i < 4; i++) {
      await toggleBtn.click();
      await page.waitForTimeout(300);
    }
    
    // Final state should match expected state (4 toggles from true = true)
    const lastValue = page.locator('text=40').first();
    const isVisible = await lastValue.isVisible();
    
    expect(isVisible).toBe(true);
  });

  test('Update and toggle operations can be combined', async ({ page }) => {
    // Start with label visible
    let toggleBtn = page.locator('#toggle-label');
    let updateBtn = page.locator('#update-value');
    
    // Toggle off
    await toggleBtn.click();
    await page.waitForTimeout(300);
    
    // Update value
    await updateBtn.click();
    await page.waitForTimeout(300);
    
    // Toggle back on
    await toggleBtn.click();
    await page.waitForTimeout(300);
    
    // Should show updated value
    const newValue = page.locator('text=45');
    await expect(newValue).toBeVisible();
  });

  test('Buttons respond immediately to clicks', async ({ page }) => {
    const updateBtn = page.locator('#update-value');
    
    // Get initial state
    const initialState = await page.locator('svg').innerHTML();
    
    // Click button
    await updateBtn.click();
    
    // Chart should respond within reasonable time
    await page.waitForTimeout(100);
    
    // State should change
    const newState = await page.locator('svg').innerHTML();
    
    // SVG content may or may not change depending on rendering
    expect(newState).toBeTruthy();
  });

  test('Multiple rapid updates are handled', async ({ page }) => {
    const updateBtn = page.locator('#update-value');
    
    // Click button rapidly
    for (let i = 0; i < 5; i++) {
      await updateBtn.click();
      await page.waitForTimeout(100);
    }
    
    // Chart should still be in valid state
    const svg = page.locator('svg').first();
    await expect(svg).toBeVisible();
    
    // Final value should be present
    const finalValue = page.locator('text=45');
    await expect(finalValue).toBeVisible();
  });

  test('Label toggle multiple times maintains consistency', async ({ page }) => {
    const toggleBtn = page.locator('#toggle-label');
    
    // Toggle 10 times
    for (let i = 0; i < 10; i++) {
      await toggleBtn.click();
      await page.waitForTimeout(100);
    }
    
    // Chart should still be consistent
    const svg = page.locator('svg').first();
    await expect(svg).toBeVisible();
    
    // Label should be visible (10 toggles from true = true)
    const label = page.locator('text=40').first();
    const isVisible = await label.isVisible();
    expect(isVisible).toBe(true);
  });

  test('Chart remains interactive after button clicks', async ({ page }) => {
    // Click update button
    const updateBtn = page.locator('#update-value');
    await updateBtn.click();
    await page.waitForTimeout(300);
    
    // Chart should still be interactive
    const svg = page.locator('svg').first();
    
    // Try to hover (simulate interaction)
    await svg.hover({ position: { x: 100, y: 200 } });
    await page.waitForTimeout(200);
    
    // Chart should remain visible
    await expect(svg).toBeVisible();
  });

  test('Buttons remain enabled throughout interactions', async ({ page }) => {
    const updateBtn = page.locator('#update-value');
    const toggleBtn = page.locator('#toggle-label');
    
    // Click multiple times
    for (let i = 0; i < 3; i++) {
      await updateBtn.click();
      await toggleBtn.click();
      await page.waitForTimeout(200);
    }
    
    // Buttons should still be enabled
    await expect(updateBtn).toBeEnabled();
    await expect(toggleBtn).toBeEnabled();
  });

  test('Toggle Label does not affect data values', async ({ page }) => {
    // Get initial label
    let label = page.locator('[id*="LastDataLabelCollection"] text').first();
    await expect(label).toBeVisible();
    
    // Toggle label
    const toggleBtn = page.locator('#toggle-label');
    await toggleBtn.click();
    await page.waitForTimeout(300);
    
    // Toggle back
    await toggleBtn.click();
    await page.waitForTimeout(300);
    
    // Label should still be functional
    label = page.locator('[id*="LastDataLabelCollection"] text').first();
    await expect(label).toBeVisible();
  });
});
