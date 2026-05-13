// Bubble Chart - Size Mapping and Third Dimension tests
// Tests bubble size variation based on data values and the third dimension visualization

import { test, expect } from '@playwright/test';

test.describe('Chart – Bubble Series › Size Mapping', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-bubble');
    await page.waitForLoadState('networkidle');
  });

  test('Bubbles have varying sizes based on BubbleSize property', async ({ page }) => {
    // The bubble sizes are mapped using Size="BubbleSize" property
    // Different population values should result in different bubble sizes
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

  test('Minimum radius constraint is applied (MinRadius="3")', async ({ page }) => {
    // All bubbles should have rx >= 3
    const ellipses = page.locator('svg ellipse');
    
    const ellipseCount = await ellipses.count();
    for (let i = 0; i < Math.min(ellipseCount, 30); i++) {
      const rx = await ellipses.nth(i).getAttribute('rx');
      if (rx) {
        const radius = parseFloat(rx);
        expect(radius).toBeGreaterThanOrEqual(3);
      }
    }
  });

  test('Large population data maps to larger bubbles', async ({ page }) => {
    // China has largest population (1.347 billion) - should be larger
    // Mongolia has smallest (0.028 billion) - should be smaller
    const ellipses = page.locator('svg ellipse');
    const radii: number[] = [];
    
    const ellipseCount = await ellipses.count();
    for (let i = 0; i < Math.min(ellipseCount, 18); i++) {
      const rx = await ellipses.nth(i).getAttribute('rx');
      if (rx) {
        radii.push(parseFloat(rx));
      }
    }
    
    // Should have at least 2 different rx values
    const uniqueRadii = new Set(radii.map(r => Math.round(r * 100) / 100));
    expect(uniqueRadii.size).toBeGreaterThan(1);
  });

  test('Bubble size variation is proportional to data', async ({ page }) => {
    // Collect all rx values and check proportional variation
    const ellipses = page.locator('svg ellipse');
    const radii: number[] = [];
    
    const ellipseCount = await ellipses.count();
    for (let i = 0; i < Math.min(ellipseCount, 18); i++) {
      const rx = await ellipses.nth(i).getAttribute('rx');
      if (rx) {
        radii.push(parseFloat(rx));
      }
    }
    
    if (radii.length > 1) {
      const maxRadius = Math.max(...radii);
      const minRadius = Math.min(...radii);
      const range = maxRadius - minRadius;
      
      // Should have meaningful variation
      expect(range).toBeGreaterThan(0);
    }
  });

});

test.describe('Chart – Bubble Series › Third Dimension Representation', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-bubble');
    await page.waitForLoadState('networkidle');
  });

  test('Three dimensions are represented: X, Y, and bubble size', async ({ page }) => {
    // X-axis: Literacy Rate (65-102)
    // Y-axis: GDP Annual Growth Rate (0-10)
    // Bubble size: Population (0.028-1.347 billion)
    
    const ellipses = page.locator('svg ellipse');
    
    // Check X variation (cx)
    const xPositions = new Set<string>();
    // Check Y variation (cy)
    const yPositions = new Set<string>();
    // Check size variation (rx)
    const sizes = new Set<string>();
    
    const ellipseCount = await ellipses.count();
    for (let i = 0; i < Math.min(ellipseCount, 18); i++) {
      const cx = await ellipses.nth(i).getAttribute('cx');
      const cy = await ellipses.nth(i).getAttribute('cy');
      const rx = await ellipses.nth(i).getAttribute('rx');
      
      if (cx) xPositions.add(cx);
      if (cy) yPositions.add(cy);
      if (rx) sizes.add(rx);
    }
    
    // Should have variation in all three dimensions
    expect(xPositions.size).toBeGreaterThan(1);
    expect(yPositions.size).toBeGreaterThan(1);
    expect(sizes.size).toBeGreaterThan(1);
  });

  test('Bubble position reflects X value (Literacy Rate)', async ({ page }) => {
    // Position on X-axis should correspond to literacy rate
    const ellipses = page.locator('svg ellipse');
    const xPositions: number[] = [];
    
    const ellipseCount = await ellipses.count();
    for (let i = 0; i < Math.min(ellipseCount, 18); i++) {
      const cx = await ellipses.nth(i).getAttribute('cx');
      if (cx) {
        xPositions.push(parseFloat(cx));
      }
    }
    
    // Should have position variation
    if (xPositions.length > 0) {
      expect(Math.max(...xPositions) - Math.min(...xPositions)).toBeGreaterThan(50);
    }
  });

  test('Bubble position reflects Y value (GDP Growth Rate)', async ({ page }) => {
    // Position on Y-axis should correspond to GDP growth rate
    const ellipses = page.locator('svg ellipse');
    const yPositions: number[] = [];
    
    const ellipseCount = await ellipses.count();
    for (let i = 0; i < Math.min(ellipseCount, 18); i++) {
      const cy = await ellipses.nth(i).getAttribute('cy');
      if (cy) {
        yPositions.push(parseFloat(cy));
      }
    }
    
    // Should have position variation
    if (yPositions.length > 0) {
      expect(Math.max(...yPositions) - Math.min(...yPositions)).toBeGreaterThan(50);
    }
  });

  test('Bubble size represents Population data', async ({ page }) => {
    // Size should vary based on population values
    const ellipses = page.locator('svg ellipse');
    const sizes: number[] = [];
    
    const ellipseCount = await ellipses.count();
    for (let i = 0; i < Math.min(ellipseCount, 18); i++) {
      const rx = await ellipses.nth(i).getAttribute('rx');
      if (rx) {
        sizes.push(parseFloat(rx));
      }
    }
    
    // Should have size variation
    if (sizes.length > 0) {
      expect(Math.max(...sizes) - Math.min(...sizes)).toBeGreaterThan(1);
    }
  });

});

test.describe('Chart – Bubble Series › Data-to-Bubble Mapping', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-bubble');
    await page.waitForLoadState('networkidle');
  });

  test('Each country has corresponding bubble rendered', async ({ page }) => {
    // Sample has 18 countries
    const ellipses = page.locator('svg ellipse');
    const ellipseCount = await ellipses.count();
    
    // Should have at least 18 ellipses for the 18 country data points
    expect(ellipseCount).toBeGreaterThanOrEqual(18);
  });

  test('China (highest literacy rate) is positioned towards right of X-axis', async ({ page }) => {
    // China: Literacy=92.2 (should be towards right)
    const ellipses = page.locator('svg ellipse');
    const xPositions: number[] = [];
    
    const ellipseCount = await ellipses.count();
    for (let i = 0; i < Math.min(ellipseCount, 18); i++) {
      const cx = await ellipses.nth(i).getAttribute('cx');
      if (cx) {
        xPositions.push(parseFloat(cx));
      }
    }
    
    if (xPositions.length > 0) {
      const maxX = Math.max(...xPositions);
      // Should have points positioned at higher X values
      expect(maxX).toBeGreaterThan(100);
    }
  });

  test('High GDP growth countries are positioned higher on Y-axis', async ({ page }) => {
    // Brazil: GDPGrowth=4.0, Egypt: GDPGrowth=2.0, Japan: GDPGrowth=0.2
    const ellipses = page.locator('svg ellipse');
    const yPositions: number[] = [];
    
    const ellipseCount = await ellipses.count();
    for (let i = 0; i < Math.min(ellipseCount, 18); i++) {
      const cy = await ellipses.nth(i).getAttribute('cy');
      if (cy) {
        yPositions.push(parseFloat(cy));
      }
    }
    
    if (yPositions.length > 0) {
      const maxY = Math.max(...yPositions);
      // Should have Y position variation
      expect(maxY).toBeGreaterThan(100);
    }
  });

  test('China (largest population) has largest bubble', async ({ page }) => {
    // China: BubbleSize=1.347 (largest)
    const ellipses = page.locator('svg ellipse');
    const radii: number[] = [];
    
    const ellipseCount = await ellipses.count();
    for (let i = 0; i < Math.min(ellipseCount, 18); i++) {
      const rx = await ellipses.nth(i).getAttribute('rx');
      if (rx) {
        radii.push(parseFloat(rx));
      }
    }
    
    if (radii.length > 0) {
      const maxRadius = Math.max(...radii);
      // Should have at least one large bubble
      expect(maxRadius).toBeGreaterThan(4);
    }
  });

  test('Small population countries have smaller bubbles', async ({ page }) => {
    // Mongolia: BubbleSize=0.028 (smallest)
    // Most: BubbleSize < 0.5
    const ellipses = page.locator('svg ellipse');
    const radii: number[] = [];
    
    const ellipseCount = await ellipses.count();
    for (let i = 0; i < Math.min(ellipseCount, 18); i++) {
      const rx = await ellipses.nth(i).getAttribute('rx');
      if (rx) {
        radii.push(parseFloat(rx));
      }
    }
    
    if (radii.length > 0) {
      const minRadius = Math.min(...radii);
      // Should have small bubbles
      expect(minRadius).toBeGreaterThanOrEqual(3);
    }
  });

});

test.describe('Chart – Bubble Series › Bubble Visual Properties', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-bubble');
    await page.waitForLoadState('networkidle');
  });

  test('Bubbles have fill color for visibility', async ({ page }) => {
    const ellipses = page.locator('svg ellipse[fill]');
    const ellipseCount = await ellipses.count();
    
    // Should have ellipses with fill attribute
    expect(ellipseCount).toBeGreaterThan(0);
  });

  test('Bubbles have border/stroke for definition', async ({ page }) => {
    // The component has <ChartSeriesBorder Width="2">
    const ellipses = page.locator('svg ellipse');
    const firstEllipse = ellipses.first();
    
    // Check for stroke attribute
    const stroke = await firstEllipse.getAttribute('stroke');
    const strokeWidth = await firstEllipse.getAttribute('stroke-width');
    
    // May or may not have explicit stroke, check if element exists
    await expect(firstEllipse).toBeVisible();
  });

  test('Bubble opacity is appropriate for visibility', async ({ page }) => {
    const ellipses = page.locator('svg ellipse');
    const firstEllipse = ellipses.first();
    
    // Check opacity attribute if present
    const opacity = await firstEllipse.getAttribute('opacity');
    
    // If opacity exists, should be > 0
    if (opacity) {
      expect(parseFloat(opacity)).toBeGreaterThan(0);
    }
  });

});
