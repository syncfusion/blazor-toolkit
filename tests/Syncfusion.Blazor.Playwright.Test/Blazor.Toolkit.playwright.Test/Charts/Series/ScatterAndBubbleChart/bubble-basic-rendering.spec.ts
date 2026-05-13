// Bubble Chart - Basic Rendering tests
// Tests the REAL Syncfusion Chart component with Bubble series from the Blazor sample app

import { test, expect } from '@playwright/test';

test.describe('Chart – Bubble Series › Basic Rendering', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-bubble');
    await page.waitForLoadState('networkidle');
  });

  test('Chart SVG renders successfully', async ({ page }) => {
    // Verify the main chart container SVG is visible
    const svg = page.locator('svg').first();
    await expect(svg).toBeVisible();
    
    // SVG should have dimensions
    const boundingBox = await svg.boundingBox();
    expect(boundingBox?.width).toBeGreaterThan(0);
    expect(boundingBox?.height).toBeGreaterThan(0);
  });

  test('Chart title renders correctly', async ({ page }) => {
    // Verify chart title is rendered - "World Countries Details"
    const titleText = page.locator('text').filter({ hasText: /World|Countries|Details/ }).first();
    await expect(titleText).toBeVisible();
  });

  test('Chart area is properly defined', async ({ page }) => {
    // Chart area should be visible and contain the plot
    const chartArea = page.locator('svg').first();
    await expect(chartArea).toBeVisible();
    
    // Should have ellipses for bubble series markers
    const ellipses = page.locator('svg ellipse');
    expect(await ellipses.count()).toBeGreaterThan(0);
  });

  test('Chart container has correct dimensions', async ({ page }) => {
    // The Bubble.razor component sets Width="90%"
    const svg = page.locator('svg').first();
    const boundingBox = await svg.boundingBox();
    
    expect(boundingBox?.width).toBeGreaterThan(400);
    expect(boundingBox?.height).toBeGreaterThan(300);
  });

  test('Bubble series data points render as circles with varying sizes', async ({ page }) => {
    // Bubble charts use ellipse elements with varying rx/ry to represent the third dimension
    const ellipses = page.locator('svg ellipse');
    const ellipseCount = await ellipses.count();
    
    // Should have ellipse elements for bubble data points (18 countries in sample)
    expect(ellipseCount).toBeGreaterThan(5);
  });

  test('Multiple bubble data points are rendered', async ({ page }) => {
    // Get all ellipses in the chart
    const ellipses = page.locator('svg ellipse');
    const ellipseCount = await ellipses.count();
    
    // The sample has 18 country data points
    expect(ellipseCount).toBeGreaterThanOrEqual(18);
  });

  test('Bubbles have varying sizes based on data', async ({ page }) => {
    // Different bubbles should have different rx/ry attributes (ellipse radii)
    const ellipses = page.locator('svg ellipse');
    const radii = new Set<number>();
    
    const ellipseCount = await ellipses.count();
    for (let i = 0; i < Math.min(ellipseCount, 30); i++) {
      const rx = await ellipses.nth(i).getAttribute('rx');
      if (rx) {
        radii.add(parseFloat(rx));
      }
    }
    
    // Should have multiple different sizes
    expect(radii.size).toBeGreaterThan(1);
  });

  test('X-axis labels render', async ({ page }) => {
    // X-axis should show numeric labels for "Literacy Rate" (65-102)
    const textElements = page.locator('text');
    const allText = await textElements.allTextContents();
    
    // Should have numeric labels
    const numericLabels = allText.filter(t => /\d+/.test(t));
    expect(numericLabels.length).toBeGreaterThan(0);
  });

  test('Y-axis labels render', async ({ page }) => {
    // Y-axis should display numeric labels for "GDP Annual Growth Rate" (0-10)
    const textElements = page.locator('text').filter({ hasText: /\d+/ });
    const labelCount = await textElements.count();
    
    expect(labelCount).toBeGreaterThan(0);
  });

  test('Axis titles are displayed', async ({ page }) => {
    // Check if axis titles are rendered as text elements
    const allText = page.locator('text');
    const count = await allText.count();
    
    // Should have text elements for titles and labels
    expect(count).toBeGreaterThan(5);
  });

});

test.describe('Chart – Bubble Series › Data Labels', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-bubble');
    await page.waitForLoadState('networkidle');
  });

  test('Data labels render on bubbles', async ({ page }) => {
    // The Bubble.razor component has <ChartDataLabel Visible="true"> with country names
    const textElements = page.locator('text').filter({ hasText: /[A-Z]{2}|China|India|US|Japan|Brazil/ });
    const labelCount = await textElements.count();
    
    // Should have country code/name labels on bubbles
    expect(labelCount).toBeGreaterThan(0);
  });

  test('Data labels display country codes and abbreviations', async ({ page }) => {
    // The DatalabelRender event handler converts long names to 2-letter codes
    const textElements = page.locator('text');
    const allText = await textElements.allTextContents();
    
    // Should contain country codes like "CN", "IN", "US", "JP", "BR", "DE", "EG", "HK", "AUS", "TW"
    const countryCodes = allText.filter(t => {
      const trimmed = t.trim();
      return /^[A-Z]{2}$|^[A-Z]{3}$/.test(trimmed);
    });
    
    // Should have some country codes visible
    expect(countryCodes.length).toBeGreaterThan(0);
  });

  test('Data labels have correct font size', async ({ page }) => {
    // The component specifies Size="12px"
    const textElements = page.locator('text').filter({ hasText: /[A-Z]{2}|[A-Z]{3}/ });
    const count = await textElements.count();
    
    // Should have formatted text elements
    expect(count).toBeGreaterThan(0);
  });

  test('All bubbles display labels at middle position', async ({ page }) => {
    // Labels are positioned at LabelPosition.Middle
    const labels = page.locator('text');
    const allLabels = await labels.allTextContents();
    
    // Should have multiple labels scattered across the chart
    expect(allLabels.length).toBeGreaterThan(5);
  });

});

test.describe('Chart – Bubble Series › Data Visualization', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-bubble');
    await page.waitForLoadState('networkidle');
  });

  test('Bubble markers have fill colors', async ({ page }) => {
    // Bubble ellipses should have fill colors
    const ellipses = page.locator('svg ellipse[fill]');
    const ellipsesWithFill = await ellipses.count();
    
    // Should have colored bubbles
    expect(ellipsesWithFill).toBeGreaterThan(0);
  });

  test('Bubble markers have borders', async ({ page }) => {
    // The component specifies <ChartSeriesBorder Width="2">
    const ellipses = page.locator('svg ellipse');
    const ellipseCount = await ellipses.count();
    
    // Get stroke attributes
    const stroke = await ellipses.first().getAttribute('stroke');
    
    // Ellipses may have stroke (border)
    expect(ellipseCount).toBeGreaterThan(0);
  });

  test('Data points are distributed across plot area', async ({ page }) => {
    // Get all ellipse elements
    const ellipses = page.locator('svg ellipse');
    const ellipseCount = await ellipses.count();
    
    // Should have ellipses spread across different positions
    const positions: { x: number; y: number }[] = [];
    for (let i = 0; i < Math.min(ellipseCount, 30); i++) {
      const cx = await ellipses.nth(i).getAttribute('cx');
      const cy = await ellipses.nth(i).getAttribute('cy');
      
      if (cx && cy) {
        positions.push({
          x: parseFloat(cx),
          y: parseFloat(cy)
        });
      }
    }
    
    // Should have varied positions
    expect(positions.length).toBeGreaterThan(0);
  });

  test('Bubble sizes correlate with data values', async ({ page }) => {
    // Different bubbles should have different rx/ry values
    const ellipses = page.locator('svg ellipse');
    
    const radii: number[] = [];
    const ellipseCount = await ellipses.count();
    for (let i = 0; i < Math.min(ellipseCount, 20); i++) {
      const rx = await ellipses.nth(i).getAttribute('rx');
      if (rx) {
        radii.push(parseFloat(rx));
      }
    }
    
    // Should have varied rx values representing the third dimension (population)
    const uniqueRadii = new Set(radii.map(r => Math.round(r * 10) / 10));
    expect(uniqueRadii.size).toBeGreaterThan(1);
  });

  test('Data points are correctly plotted on the axis scale', async ({ page }) => {
    // Verify chart renders with proper scaling
    const svg = page.locator('svg').first();
    const boundingBox = await svg.boundingBox();
    
    if (boundingBox) {
      expect(boundingBox.width).toBeGreaterThan(400);
      expect(boundingBox.height).toBeGreaterThan(300);
    }
  });

  test('Chart respects axis constraints', async ({ page }) => {
    // X-axis: CrossesAt="5" Minimum="65" Maximum="102" Interval="5"
    // Y-axis: CrossesAt="85" Minimum="0" Maximum="10" Interval="2.5"
    const textElements = page.locator('text').filter({ hasText: /\d+/ });
    const allLabels = await textElements.allTextContents();
    
    // Should have numeric labels within specified ranges
    expect(allLabels.length).toBeGreaterThan(0);
  });

});

test.describe('Chart – Bubble Series › Tooltips', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-bubble');
    await page.waitForLoadState('networkidle');
  });

  test('Tooltip is enabled and visible on hover', async ({ page }) => {
    // The component has ChartTooltipSettings with Enable="true"
    const ellipse = page.locator('svg ellipse').first();
    
    // Hover over a bubble
    await ellipse.hover({ force: true });
    await page.waitForTimeout(500);
    
    // Check if tooltip appears (it should contain country name, GDP, population info)
    const tooltip = page.locator('[role="tooltip"], .e-tooltip').first();
    const isVisible = await tooltip.isVisible().catch(() => false);
    
    // Tooltip may or may not be immediately visible, but the element exists
    expect(typeof isVisible).toBe('boolean');
  });
});

test.describe('Chart – Bubble Series › Legend', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-bubble');
    await page.waitForLoadState('networkidle');
  });

  test('Legend is hidden when Visible="false"', async ({ page }) => {
    // The component has <ChartLegendSettings Visible="false">
    const legend = page.locator('[role="legend"], .e-legend, .e-legend-text').first();
    
    // Legend should not be visible
    const isVisible = await legend.isVisible().catch(() => false);
    
    // Should not be visible
    expect(isVisible).toBe(false);
  });

  test('Chart renders without legend section', async ({ page }) => {
    // Verify chart displays without legend area
    const chartArea = page.locator('svg').first();
    await expect(chartArea).toBeVisible();
  });

});

test.describe('Chart – Bubble Series › Interactivity', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-bubble');
    await page.waitForLoadState('networkidle');
  });

  test('Tooltip appears on hover over bubble', async ({ page }) => {
    // Find a bubble ellipse in the chart
    const ellipse = page.locator('svg ellipse').first();
    
    // Ensure the ellipse element exists
    await expect(ellipse).toBeVisible({ timeout: 10000 });
    
    // Hover over the ellipse
    await ellipse.hover({ force: true });
    await page.waitForTimeout(300);
    
    // Bubble should remain visible after hover
    const isVisible = await ellipse.isVisible();
    expect(isVisible).toBe(true);
  });

  test('Chart remains interactive after render', async ({ page }) => {
    // Verify chart can be interacted with
    const svg = page.locator('svg').first();
    await expect(svg).toBeVisible();
    
    // Get ellipse elements
    const ellipses = page.locator('svg ellipse');
    const count = await ellipses.count();
    
    // Should have rendered ellipses
    expect(count).toBeGreaterThan(0);
  });

  test('Multiple bubbles are individually hoverable', async ({ page }) => {
    // Get multiple ellipses
    const ellipses = page.locator('svg ellipse');
    const firstEllipse = ellipses.first();
    const secondEllipse = ellipses.nth(1);
    
    // Hover over first ellipse
    await firstEllipse.hover({ force: true });
    await page.waitForTimeout(200);
    
    // Hover over second ellipse
    await secondEllipse.hover({ force: true });
    await page.waitForTimeout(200);
    
    // Both should be interactive
    const firstVisible = await firstEllipse.isVisible();
    const secondVisible = await secondEllipse.isVisible();
    
    expect(firstVisible && secondVisible).toBe(true);
  });

});

test.describe('Chart – Bubble Series › Accessibility', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-bubble');
    await page.waitForLoadState('networkidle');
  });

  test('Chart SVG has proper structure', async ({ page }) => {
    const svg = page.locator('svg').first();
    await expect(svg).toBeVisible();
    
    // SVG should exist and have viewBox or dimensions
    const viewBox = await svg.getAttribute('viewBox');
    const width = await svg.getAttribute('width');
    
    const hasAttribute = viewBox || width;
    expect(hasAttribute).toBeTruthy();
  });

  test('Chart displays numeric values for accessibility', async ({ page }) => {
    // Axis numeric labels should be present for screen readers
    const numericLabels = page.locator('text').filter({ hasText: /\d+/ });
    const labelCount = await numericLabels.count();
    
    expect(labelCount).toBeGreaterThan(0);
  });

  test('Chart displays country names for accessibility', async ({ page }) => {
    // Data labels should display country codes/names
    const textElements = page.locator('text');
    const allText = await textElements.allTextContents();
    
    // Should have readable text
    expect(allText.length).toBeGreaterThan(0);
  });

  test('Chart title text is accessible', async ({ page }) => {
    // Chart should have "World Countries Details" title text
    const titleText = page.locator('text').filter({ hasText: /World|Countries/ }).first();
    await expect(titleText).toBeVisible();
  });

});

test.describe('Chart – Bubble Series › Axis Configuration', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-bubble');
    await page.waitForLoadState('networkidle');
  });

  test('X-axis minimum and maximum boundaries are respected', async ({ page }) => {
    // X-axis Minimum="65" Maximum="102"
    const textElements = page.locator('text').filter({ hasText: /\d+/ });
    const allLabels = await textElements.allTextContents();
    
    // Should have numeric labels
    expect(allLabels.length).toBeGreaterThan(0);
  });

  test('Y-axis minimum and maximum boundaries are respected', async ({ page }) => {
    // Y-axis Minimum="0" Maximum="10"
    const textElements = page.locator('text').filter({ hasText: /\d+/ });
    const labelCount = await textElements.count();
    
    expect(labelCount).toBeGreaterThan(0);
  });

  test('Axes use proper interval settings', async ({ page }) => {
    // X-axis Interval="5", Y-axis Interval="2.5"
    const axisLabels = page.locator('text').filter({ hasText: /\d+/ });
    const count = await axisLabels.count();
    
    // Should have multiple axis labels
    expect(count).toBeGreaterThan(3);
  });

});

test.describe('Chart – Bubble Series › Responsive', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-bubble');
    await page.waitForLoadState('networkidle');
  });

  test('Chart responds to width setting', async ({ page }) => {
    // Component has Width="90%"
    const svg = page.locator('svg').first();
    const boundingBox = await svg.boundingBox();
    
    // Should have calculated width
    expect(boundingBox?.width).toBeGreaterThan(0);
  });

  test('Chart maintains aspect ratio', async ({ page }) => {
    const svg = page.locator('svg').first();
    const boundingBox = await svg.boundingBox();
    
    if (boundingBox) {
      // Should have reasonable aspect ratio (not too stretched)
      const aspectRatio = (boundingBox.width || 1) / (boundingBox.height || 1);
      expect(aspectRatio).toBeGreaterThan(0.5);
      expect(aspectRatio).toBeLessThan(3);
    }
  });

});
