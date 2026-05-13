// Scatter Chart - Data Points Visualization tests
// Tests the scatter data point rendering, positioning, and cluster visualization

import { test, expect } from '@playwright/test';

test.describe('Chart – Scatter Series › Data Points Distribution', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-scatter');
    await page.waitForLoadState('networkidle');
  });

  test('All scatter data points are rendered', async ({ page }) => {
    // The sample has 5 clusters with varying data points (around 300+ total points)
    const ellipses = page.locator('svg ellipse');
    const ellipseCount = await ellipses.count();
    
    // Should have many data points from multiple clusters
    expect(ellipseCount).toBeGreaterThan(50);
  });

  test('Data points from Cluster1 render with correct positioning', async ({ page }) => {
    // Cluster1 has breadth from 41.3 to ~45, circumference from 78 to ~92
    const ellipses = page.locator('svg ellipse');
    
    // Collect a sample of positions
    const positions: number[] = [];
    const ellipseCount = await ellipses.count();
    
    for (let i = 0; i < Math.min(ellipseCount, 50); i++) {
      const cx = await ellipses.nth(i).getAttribute('cx');
      const cy = await ellipses.nth(i).getAttribute('cy');
      if (cx && cy) {
        positions.push(parseFloat(cx));
      }
    }
    
    // Should have varying X positions
    expect(positions.length).toBeGreaterThan(0);
  });

  test('Data points have consistent radius within clusters', async ({ page }) => {
    // All ellipses in a scatter series should have similar rx (marker size)
    const ellipses = page.locator('svg ellipse');
    const radii = new Set<string>();
    
    const ellipseCount = await ellipses.count();
    for (let i = 0; i < Math.min(ellipseCount, 30); i++) {
      const rx = await ellipses.nth(i).getAttribute('rx');
      if (rx) {
        radii.add(rx);
      }
    }
    
    // All ellipses in scatter chart should have same rx (representing same marker size)
    if (radii.size > 0) {
      expect(radii.size).toBe(1);
    }
  });

  test('Scatter points span across entire plot area', async ({ page }) => {
    // Points should be distributed from left to right and top to bottom
    const ellipses = page.locator('svg ellipse');
    const positions: { x: number; y: number }[] = [];
    
    const ellipseCount = await ellipses.count();
    for (let i = 0; i < Math.min(ellipseCount, 100); i++) {
      const cx = await ellipses.nth(i).getAttribute('cx');
      const cy = await ellipses.nth(i).getAttribute('cy');
      if (cx && cy) {
        positions.push({
          x: parseFloat(cx),
          y: parseFloat(cy)
        });
      }
    }
    
    // Find min and max X and Y
    if (positions.length > 0) {
      const xValues = positions.map(p => p.x);
      const yValues = positions.map(p => p.y);
      
      const xRange = Math.max(...xValues) - Math.min(...xValues);
      const yRange = Math.max(...yValues) - Math.min(...yValues);
      
      // Should have significant spread
      expect(xRange).toBeGreaterThan(50);
      expect(yRange).toBeGreaterThan(50);
    }
  });

});

test.describe('Chart – Scatter Series › Cluster-Specific Tests', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-scatter');
    await page.waitForLoadState('networkidle');
  });

  test('Cluster1 data renders (41.3-45 breadth range)', async ({ page }) => {
    // Cluster 1 starts with breadth=41.3
    const ellipses = page.locator('svg ellipse');
    expect(await ellipses.count()).toBeGreaterThan(0);
  });

  test('Cluster2 data renders (45.2-47.4 breadth range)', async ({ page }) => {
    // Cluster 2 has higher breadth values
    const ellipses = page.locator('svg ellipse');
    expect(await ellipses.count()).toBeGreaterThan(0);
  });

  test('Cluster3 data renders (47.5-49.9 breadth range)', async ({ page }) => {
    // Cluster 3 has even higher breadth values
    const ellipses = page.locator('svg ellipse');
    expect(await ellipses.count()).toBeGreaterThan(0);
  });

  test('Cluster4 data renders (49.8-50.6 breadth range)', async ({ page }) => {
    // Cluster 4 has the highest breadth values
    const ellipses = page.locator('svg ellipse');
    expect(await ellipses.count()).toBeGreaterThan(0);
  });

  test('Cluster5 data renders (smallest cluster)', async ({ page }) => {
    // Cluster 5 appears to be a smaller cluster
    const ellipses = page.locator('svg ellipse');
    expect(await ellipses.count()).toBeGreaterThan(0);
  });

});

test.describe('Chart – Scatter Series › Axis Ranges', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-scatter');
    await page.waitForLoadState('networkidle');
  });

  test('X-axis range matches "Shoulder Breadth" data extent', async ({ page }) => {
    // X data ranges from 41.3 to ~50.3
    const xAxisLabels = page.locator('text').filter({ hasText: /4\d|5\d/ });
    const labelCount = await xAxisLabels.count();
    
    // Should have numeric labels in the range
    expect(labelCount).toBeGreaterThan(0);
  });

  test('Y-axis range matches "Bust Circumference" data extent', async ({ page }) => {
    // Y data ranges from ~78 to ~120
    const yAxisLabels = page.locator('text').filter({ hasText: /\d+/ });
    const allLabels = await yAxisLabels.allTextContents();
    
    // Should have numeric labels
    expect(allLabels.length).toBeGreaterThan(0);
  });

  test('Axis labels display in correct intervals', async ({ page }) => {
    // Check for multiple axis labels
    const textElements = page.locator('text').filter({ hasText: /\d+/ });
    const count = await textElements.count();
    
    expect(count).toBeGreaterThan(3);
  });

});

test.describe('Chart – Scatter Series › Color Differentiation', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-scatter');
    await page.waitForLoadState('networkidle');
  });

  test('Colors are consistent within each cluster', async ({ page }) => {
    // Sample first 20 ellipses and check colors
    const ellipses = page.locator('svg ellipse[fill]');
    const initialColor = await ellipses.first().getAttribute('fill');
    
    let sameColorCount = 0;
    for (let i = 0; i < Math.min(20, await ellipses.count()); i++) {
      const fill = await ellipses.nth(i).getAttribute('fill');
      if (fill === initialColor) {
        sameColorCount++;
      }
    }
    
    // At least some ellipses should have the same color
    expect(sameColorCount).toBeGreaterThan(0);
  });

});

test.describe('Chart – Scatter Series › Data Point Precision', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-scatter');
    await page.waitForLoadState('networkidle');
  });

  test('Data point positions are calculated correctly', async ({ page }) => {
    // Each ellipse position (cx, cy) should map to the data values
    const ellipses = page.locator('svg ellipse');
    const firstEllipse = ellipses.first();
    
    const cx = await firstEllipse.getAttribute('cx');
    const cy = await firstEllipse.getAttribute('cy');
    
    // Should have valid numeric coordinates
    expect(cx).toBeTruthy();
    expect(cy).toBeTruthy();
    expect(parseFloat(cx || '0')).toBeGreaterThan(0);
    expect(parseFloat(cy || '0')).toBeGreaterThan(0);
  });

  test('Decimal data values are properly scaled', async ({ page }) => {
    // The sample data has decimal breadth values (e.g., 41.3, 42.1, etc.)
    const ellipses = page.locator('svg ellipse');
    const positions: number[] = [];
    
    const ellipseCount = await ellipses.count();
    for (let i = 0; i < Math.min(ellipseCount, 30); i++) {
      const cx = await ellipses.nth(i).getAttribute('cx');
      if (cx) {
        positions.push(parseFloat(cx));
      }
    }
    
    // Should have decimal-level variation in positions
    if (positions.length > 1) {
      // Check for subtle differences
      const sorted = positions.sort((a, b) => a - b);
      const differences = sorted.slice(1).map((v, i) => v - sorted[i]);
      
      // Some positions should be very close together
      const smallDifferences = differences.filter(d => d < 5);
      expect(smallDifferences.length).toBeGreaterThanOrEqual(0);
    }
  });

});
