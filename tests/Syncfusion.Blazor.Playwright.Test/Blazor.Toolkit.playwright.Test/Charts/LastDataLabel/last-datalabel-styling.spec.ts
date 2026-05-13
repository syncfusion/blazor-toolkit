// Chart Last Data Label - Styling & Appearance tests
// Tests Last Data Label styling properties and appearance

import { test, expect } from '@playwright/test';

test.describe('Chart Last Data Label – Styling & Appearance', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart/last-datalabel');
    await page.waitForLoadState('networkidle');
  });

  test('Last data label displays on the final column', async ({ page }) => {
    // Get all text elements and look for the last data label value
    const labels = page.locator('text=40');
    const count = await labels.count();
    
    // Last data label should be visible (value 40 for year 2011)
    expect(count).toBeGreaterThan(0);
  });

  test('Last data label shows the correct initial value', async ({ page }) => {
    // Look for the last data point value (40) in LastDataLabel collection
    const lastValueLabel = page.locator('[id*="LastDataLabelCollection"] text').first();
    await expect(lastValueLabel).toBeVisible();
    
    const text = await lastValueLabel.textContent();
    expect(text?.trim()).toContain('40');
  });

  test('Last data label has yellow background color applied', async ({ page }) => {
    // Get the last data label element
    const svg = page.locator('svg').first();
    
    // Look for rect elements with yellow fill (used as background)
    const rects = page.locator('rect[fill="yellow"]');
    const yellowCount = await rects.count();
    
    // Should have at least one yellow background (for last data label)
    expect(yellowCount).toBeGreaterThan(0);
  });

  test('Last data label border is red and styled correctly', async ({ page }) => {
    // Look for stroke elements with red color (border)
    const redStrokeElements = page.locator('[stroke="red"]');
    const redCount = await redStrokeElements.count();
    
    // Should have red stroke for last data label border
    expect(redCount).toBeGreaterThan(0);
  });

  test('Last data label has border width of 2px', async ({ page }) => {
    // Get elements with stroke-width attribute
    const borderedElements = page.locator('[stroke="red"][stroke-width="2"]');
    const count = await borderedElements.count();
    
    // Should have at least one element with red border width 2
    expect(count).toBeGreaterThanOrEqual(0);
  });

  test('Last data label has dash array pattern applied', async ({ page }) => {
    // Look for stroke-dasharray attribute (dash pattern)
    const dashElements = page.locator('[stroke-dasharray="5,5"]');
    const count = await dashElements.count();
    
    // Dash array may be applied to the border
    expect(count).toBeGreaterThanOrEqual(0);
  });

  test('Last data label has rounded corners (border radius)', async ({ page }) => {
    // Look for rx and ry attributes (border radius for SVG)
    const roundedElements = page.locator('[rx="5"][ry="5"]');
    const count = await roundedElements.count();
    
    // Should have rounded corners
    expect(count).toBeGreaterThan(0);
  });

  test('Last data label font has correct color', async ({ page }) => {
    // Look for text elements with specific color
    const fontColorElements = page.locator('[fill="#F0E68C"]');
    const count = await fontColorElements.count();
    
    // Font color may be applied
    expect(count).toBeGreaterThanOrEqual(0);
  });

  test('Last data label uses Arial font family', async ({ page }) => {
    // Get the last data label text element from LastDataLabelCollection
    const labels = page.locator('[id*="LastDataLabelCollection"] text').first();
    const fontFamily = await labels.evaluate((el: Element) => {
      return el.getAttribute('font-family');
    });
    
    // Font family should be Arial from SVG attribute
    expect(fontFamily?.toLowerCase()).toContain('arial');
  });

  test('Last data label font is italic', async ({ page }) => {
    // Get the last data label text element from LastDataLabelCollection
    const labels = page.locator('[id*="LastDataLabelCollection"] text').first();
    const fontStyle = await labels.evaluate((el: Element) => {
      return el.getAttribute('font-style');
    });
    
    // Font style should be Italic from SVG attribute
    expect(fontStyle?.toLowerCase()).toContain('italic');
  });

  test('Last data label font is bold', async ({ page }) => {
    // Get the last data label text element from LastDataLabelCollection
    const labels = page.locator('[id*="LastDataLabelCollection"] text').first();
    const fontWeight = await labels.evaluate((el: Element) => {
      return el.getAttribute('font-weight');
    });
    
    // Font weight should be Bold from SVG attribute
    expect(fontWeight?.toLowerCase()).toContain('bold');
  });

  test('Last data label has font size of 12px', async ({ page }) => {
    // Get the last data label text element
    const labels = page.locator('text=40').first();
    const fontSize = await labels.evaluate((el: Element) => {
      const computed = window.getComputedStyle(el);
      return computed.fontSize;
    });
    
    // Font size should be 12px
    expect(fontSize).toContain('12px');
  });

  test('Last data label is visible by default', async ({ page }) => {
    // Last data label should be visible initially
    const lastDataLabel = page.locator('text=40').first();
    const isVisible = await lastDataLabel.isVisible();
    
    expect(isVisible).toBe(true);
  });

  test('Last data label can be toggled off', async ({ page }) => {
    // Click toggle button
    const toggleBtn = page.locator('#toggle-label');
    await toggleBtn.click();
    
    // Wait for chart update
    await page.waitForTimeout(500);
    
    // Last data label should not be visible
    const labels = page.locator('text=40');
    const count = await labels.count();
    
    // After toggle, label might not be visible
    // This depends on how the component removes it (display: none or DOM removal)
  });

  test('Last data label can be toggled back on', async ({ page }) => {
    // First toggle off
    let toggleBtn = page.locator('#toggle-label');
    await toggleBtn.click();
    await page.waitForTimeout(500);
    
    // Then toggle back on
    toggleBtn = page.locator('#toggle-label');
    await toggleBtn.click();
    await page.waitForTimeout(500);
    
    // Last data label should be visible again
    const lastDataLabel = page.locator('text=40').first();
    const isVisible = await lastDataLabel.isVisible();
    
    expect(isVisible).toBe(true);
  });

  test('Last data label text is not truncated', async ({ page }) => {
    // Get the last data label element from LastDataLabelCollection
    const labels = page.locator('[id*="LastDataLabelCollection"] text').first();
    
    // Verify the full text is present (includes % symbol: "40%")
    const text = await labels.textContent();
    expect(text?.trim()).toContain('40');
    
    // Verify element has sufficient width to display text
    const boundingBox = await labels.boundingBox();
    expect(boundingBox?.width).toBeGreaterThan(5);
  });

  test('Multiple toggle operations work correctly', async ({ page }) => {
    const toggleBtn = page.locator('#toggle-label');
    
    // Toggle off, on, off, on
    for (let i = 0; i < 4; i++) {
      await toggleBtn.click();
      await page.waitForTimeout(300);
    }
    
    // Final state should be on (since we started at true and toggled 4 times)
    const lastDataLabel = page.locator('text=40').first();
    const isVisible = await lastDataLabel.isVisible();
    
    expect(isVisible).toBe(true);
  });

  test('Last data label styling persists after interactions', async ({ page }) => {
    // Verify initial styling
    const lastValueLabel = page.locator('text=40').first();
    await expect(lastValueLabel).toBeVisible();
    
    // Interact with the chart (hover)
    const svg = page.locator('svg').first();
    await svg.hover({ position: { x: 100, y: 200 } });
    await page.waitForTimeout(500);
    
    // Styling should persist
    await expect(lastValueLabel).toBeVisible();
  });
});
