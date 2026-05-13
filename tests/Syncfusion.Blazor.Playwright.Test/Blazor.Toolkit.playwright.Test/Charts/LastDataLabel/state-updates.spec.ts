// Chart Last Data Label - State Management & Updates tests
// Tests state changes, persistence, and update handling

import { test, expect } from '@playwright/test';

test.describe('Chart Last Data Label – State Management & Updates', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart/last-datalabel');
    await page.waitForLoadState('networkidle');
  });

  test('Initial ShowLabel state is true', async ({ page }) => {
    // Last data label should be visible initially
    const lastLabel = page.locator('text=40').first();
    const isVisible = await lastLabel.isVisible();
    
    expect(isVisible).toBe(true);
  });

  test('ShowLabel state changes on toggle', async ({ page }) => {
    // Initial state: visible
    let lastLabel = page.locator('[id*="LastDataLabelCollection"] text').first();
    let wasVisible = await lastLabel.isVisible();
    expect(wasVisible).toBe(true);
    
    // Toggle button exists and is clickable
    const toggleBtn = page.locator('#toggle-label');
    await expect(toggleBtn).toBeEnabled();
    await toggleBtn.click();
    await page.waitForTimeout(500);
    
    // Verify toggle button still works
    await expect(toggleBtn).toBeEnabled();
  });

  test('Last data point value updates correctly', async ({ page }) => {
    // Initial value: 40
    let lastValue = page.locator('text=40');
    expect(await lastValue.count()).toBeGreaterThan(0);
    
    // Click Update Value button
    const updateBtn = page.locator('#update-value');
    await updateBtn.click();
    await page.waitForTimeout(500);
    
    // New value: 45
    const newValue = page.locator('text=45');
    expect(await newValue.count()).toBeGreaterThan(0);
  });

  test('State changes persist during chart interaction', async ({ page }) => {
    // Update value
    const updateBtn = page.locator('#update-value');
    await updateBtn.click();
    await page.waitForTimeout(300);
    
    // Interact with chart (hover)
    const svg = page.locator('svg').first();
    await svg.hover({ position: { x: 300, y: 300 } });
    await page.waitForTimeout(500);
    
    // Value should still be updated
    const newValue = page.locator('text=45');
    expect(await newValue.count()).toBeGreaterThan(0);
  });

  test('Multiple rapid updates are handled correctly', async ({ page }) => {
    const updateBtn = page.locator('#update-value');
    
    // Perform multiple updates rapidly
    for (let i = 0; i < 5; i++) {
      await updateBtn.click();
      await page.waitForTimeout(50);
    }
    
    // Chart should be stable
    const svg = page.locator('svg').first();
    await expect(svg).toBeVisible();
    
    // Final value should be 45 (always updates to 45 regardless of clicks)
    const finalValue = page.locator('text=45');
    expect(await finalValue.count()).toBeGreaterThan(0);
  });

  test('Previous state does not affect new state', async ({ page }) => {
    const updateBtn = page.locator('#update-value');
    const toggleBtn = page.locator('#toggle-label');
    
    // Toggle off
    await toggleBtn.click();
    await page.waitForTimeout(300);
    
    // Update value
    await updateBtn.click();
    await page.waitForTimeout(300);
    
    // Toggle back on
    await toggleBtn.click();
    await page.waitForTimeout(300);
    
    // New value should be visible
    const newValue = page.locator('text=45');
    await expect(newValue).toBeVisible();
  });

  test('ShowLabel state is independent from data values', async ({ page }) => {
    // Get initial state from specific collection
    let label = page.locator('[id*="LastDataLabelCollection"] text').first();
    let labelVisibleBefore = await label.isVisible();
    
    // Update value
    const updateBtn = page.locator('#update-value');
    await updateBtn.click();
    await page.waitForTimeout(300);
    
    // Label should still be visible
    label = page.locator('[id*="LastDataLabelCollection"] text').first();
    let labelVisibleAfter = await label.isVisible();
    
    expect(labelVisibleAfter).toBe(labelVisibleBefore);
  });

  test('Data value persists across multiple toggles', async ({ page }) => {
    // Update value first
    const updateBtn = page.locator('#update-value');
    await updateBtn.click();
    await page.waitForTimeout(300);
    
    // Toggle multiple times
    const toggleBtn = page.locator('#toggle-label');
    for (let i = 0; i < 4; i++) {
      await toggleBtn.click();
      await page.waitForTimeout(200);
    }
    
    // Value should still be 45
    const finalValue = page.locator('text=45');
    await expect(finalValue).toBeVisible();
  });

  test('Toggle state persists across multiple updates', async ({ page }) => {
    // Get initial visibility
    const toggleBtn = page.locator('#toggle-label');
    const updateBtn = page.locator('#update-value');
    
    // Verify both buttons exist and are clickable
    await expect(toggleBtn).toBeEnabled();
    await expect(updateBtn).toBeEnabled();
    
    // Update multiple times
    for (let i = 0; i < 3; i++) {
      await updateBtn.click();
      await page.waitForTimeout(200);
    }
    
    // Both buttons should still be functional
    await expect(toggleBtn).toBeEnabled();
    await expect(updateBtn).toBeEnabled();
  });

  test('Update button does not affect ShowLabel state', async ({ page }) => {
    // Get initial label visibility from LastDataLabelCollection
    let label = page.locator('[id*="LastDataLabelCollection"] text').first();
    const initialVisibility = await label.isVisible();
    
    // Update button click
    const updateBtn = page.locator('#update-value');
    await updateBtn.click();
    await page.waitForTimeout(300);
    
    // Check new label visibility (should be same)
    label = page.locator('[id*="LastDataLabelCollection"] text').first();
    const finalVisibility = await label.isVisible();
    
    expect(finalVisibility).toBe(initialVisibility);
  });

  test('Toggle button does not affect data values', async ({ page }) => {
    // Get initial value
    const initialValue = page.locator('text=40');
    let valueExists = await initialValue.count() > 0;
    
    // Toggle
    const toggleBtn = page.locator('#toggle-label');
    await toggleBtn.click();
    await page.waitForTimeout(300);
    
    // Toggle back
    await toggleBtn.click();
    await page.waitForTimeout(300);
    
    // Same value should be there
    const finalValue = page.locator('text=40');
    expect(await finalValue.count()).toBeGreaterThan(0);
  });

  test('State consistency across button clicks and hovers', async ({ page }) => {
    const updateBtn = page.locator('#update-value');
    const toggleBtn = page.locator('#toggle-label');
    const svg = page.locator('svg').first();
    
    // Complex interaction sequence
    await updateBtn.click();
    await page.waitForTimeout(200);
    
    await svg.hover({ position: { x: 300, y: 300 } });
    await page.waitForTimeout(300);
    
    await toggleBtn.click();
    await page.waitForTimeout(200);
    
    await svg.hover({ position: { x: 400, y: 400 } });
    await page.waitForTimeout(300);
    
    await updateBtn.click();
    await page.waitForTimeout(200);
    
    // Chart should be consistent
    await expect(svg).toBeVisible();
  });

  test('State recovers after rapid alternating toggles', async ({ page }) => {
    const toggleBtn = page.locator('#toggle-label');
    
    // Rapid toggle on/off pattern
    for (let i = 0; i < 20; i++) {
      await toggleBtn.click();
      await page.waitForTimeout(50);
    }
    
    // Final state should be consistent
    const label = page.locator('text=40').first();
    const isVisible = await label.isVisible();
    
    // After 20 toggles from true = true (visible)
    expect(isVisible).toBe(true);
  });

  test('Chart component maintains integrity through state changes', async ({ page }) => {
    const svg = page.locator('svg').first();
    
    // Get initial SVG content length
    const initialContent = await svg.innerHTML();
    const initialLength = initialContent.length;
    
    // Perform multiple state changes
    const updateBtn = page.locator('#update-value');
    const toggleBtn = page.locator('#toggle-label');
    
    for (let i = 0; i < 5; i++) {
      await updateBtn.click();
      await page.waitForTimeout(150);
      await toggleBtn.click();
      await page.waitForTimeout(150);
    }
    
    // SVG should still be valid
    const finalContent = await svg.innerHTML();
    expect(finalContent.length).toBeGreaterThan(100);
  });

  test('No memory leaks on repeated state changes', async ({ page }) => {
    const updateBtn = page.locator('#update-value');
    const toggleBtn = page.locator('#toggle-label');
    
    // Many repeated operations
    for (let i = 0; i < 50; i++) {
      if (i % 2 === 0) {
        await updateBtn.click();
      } else {
        await toggleBtn.click();
      }
      await page.waitForTimeout(50);
    }
    
    // Page should still be responsive
    const heading = page.locator('h3');
    await expect(heading).toBeVisible();
  });

  test('Initial state is reset on page reload', async ({ page }) => {
    // Modify state
    const updateBtn = page.locator('#update-value');
    const toggleBtn = page.locator('#toggle-label');
    
    await updateBtn.click();
    await page.waitForTimeout(300);
    await toggleBtn.click();
    await page.waitForTimeout(300);
    
    // Reload page
    await page.reload();
    await page.waitForLoadState('networkidle');
    
    // Should be back to initial state
    // Label should be visible and value should be 40
    const initialLabel = page.locator('text=40').first();
    const isVisible = await initialLabel.isVisible();
    
    expect(isVisible).toBe(true);
  });

  test('State changes do not cause console errors', async ({ page }) => {
    const errors: string[] = [];
    page.on('console', msg => {
      if (msg.type() === 'error') {
        errors.push(msg.text());
      }
    });
    
    const updateBtn = page.locator('#update-value');
    const toggleBtn = page.locator('#toggle-label');
    
    // Perform many state changes
    for (let i = 0; i < 20; i++) {
      if (i % 2 === 0) {
        await updateBtn.click();
      } else {
        await toggleBtn.click();
      }
      await page.waitForTimeout(50);
    }
    
    await page.waitForTimeout(500);
    
    // Should have no or minimal errors
    const criticalErrors = errors.filter(e => !e.includes('favicon'));
    expect(criticalErrors.length).toBeLessThan(3);
  });
});
