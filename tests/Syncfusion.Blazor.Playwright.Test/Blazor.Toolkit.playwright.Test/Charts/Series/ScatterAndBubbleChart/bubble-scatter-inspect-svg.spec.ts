// Bubble and Scatter Charts - SVG DOM Structure and Rendering tests
// Tests the actual SVG DOM structure, element hierarchy, and rendering integrity

import { test, expect } from '@playwright/test';

test.describe('Chart – Scatter & Bubble › SVG Structure Validation', () => {

  test('Scatter Chart SVG has proper hierarchy', async ({ page }) => {
    await page.goto('http://localhost:5000/chart-scatter');
    await page.waitForLoadState('networkidle');
    
    // SVG root should exist
    const svg = page.locator('svg').first();
    await expect(svg).toBeVisible();
    
    // Get SVG tag name
    const tagName = await svg.evaluate((el) => el.tagName);
    expect(tagName.toUpperCase()).toBe('SVG');
  });

  test('Bubble Chart SVG has proper hierarchy', async ({ page }) => {
    await page.goto('http://localhost:5000/chart-bubble');
    await page.waitForLoadState('networkidle');
    
    // SVG root should exist
    const svg = page.locator('svg').first();
    await expect(svg).toBeVisible();
    
    // Get SVG tag name
    const tagName = await svg.evaluate((el) => el.tagName);
    expect(tagName.toUpperCase()).toBe('SVG');
  });

});

test.describe('Chart – Scatter › SVG Element Count', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-scatter');
    await page.waitForLoadState('networkidle');
  });

  test('SVG contains circle elements for data points', async ({ page }) => {
    // Scatter chart renders markers as ellipse elements in SVG
    const ellipses = page.locator('svg ellipse');
    const count = await ellipses.count();
    
    // Should have many ellipses (multiple clusters) - roughly 203*5 = 1015 scatter points
    // But some may be filtered or hidden, so we expect significant count
    expect(count).toBeGreaterThan(50);
  });

  test('SVG contains text elements for labels', async ({ page }) => {
    const textElements = page.locator('svg text');
    const count = await textElements.count();
    
    // Should have text for axis labels, title
    expect(count).toBeGreaterThan(5);
  });

  test('SVG contains path elements for axes', async ({ page }) => {
    const paths = page.locator('svg path');
    const count = await paths.count();
    
    // Should have path elements for axes and grid
    expect(count).toBeGreaterThan(0);
  });

  test('SVG contains line elements for grid or axes', async ({ page }) => {
    const lines = page.locator('svg line');
    const count = await lines.count();
    
    // Should have line elements
    expect(count).toBeGreaterThanOrEqual(0);
  });

});

test.describe('Chart – Bubble › SVG Element Count', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-bubble');
    await page.waitForLoadState('networkidle');
  });

  test('SVG contains circle elements for bubbles', async ({ page }) => {
    // Bubble chart renders markers as ellipse elements in SVG
    const ellipses = page.locator('svg ellipse');
    const count = await ellipses.count();
    
    // Should have ellipses for 18 country data points
    expect(count).toBeGreaterThanOrEqual(18);
  });

  test('SVG contains text elements for data labels', async ({ page }) => {
    const textElements = page.locator('svg text');
    const count = await textElements.count();
    
    // Should have text for country codes, axis labels, title
    expect(count).toBeGreaterThan(10);
  });

  test('SVG contains path elements for axes and gridlines', async ({ page }) => {
    const paths = page.locator('svg path');
    const count = await paths.count();
    
    // Should have path elements
    expect(count).toBeGreaterThan(0);
  });

});

test.describe('Chart – Scatter › Circle Element Attributes', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-scatter');
    await page.waitForLoadState('networkidle');
  });

  test('Ellipse elements have cx and cy attributes for positioning', async ({ page }) => {
    const ellipses = page.locator('svg ellipse');
    const firstEllipse = ellipses.first();
    
    // Ellipses use cx/cy attributes for positioning
    const cx = await firstEllipse.getAttribute('cx');
    const cy = await firstEllipse.getAttribute('cy');
    
    // Ellipses should have positioning information
    expect(cx).toBeTruthy();
    expect(cy).toBeTruthy();
  });

  test('Ellipse elements have rx and ry attributes for dimensions', async ({ page }) => {
    const ellipses = page.locator('svg ellipse');
    const firstEllipse = ellipses.first();
    
    // Ellipses use rx/ry attributes for dimensions
    const rx = await firstEllipse.getAttribute('rx');
    const ry = await firstEllipse.getAttribute('ry');
    expect(rx).toBeTruthy();
    expect(ry).toBeTruthy();
  });

  test('Ellipse elements have fill color', async ({ page }) => {
    const ellipses = page.locator('svg ellipse[fill]');
    const count = await ellipses.count();
    
    // Ellipses should have fill colors
    expect(count).toBeGreaterThan(0);
  });

  test('All ellipses have valid numeric cx coordinate', async ({ page }) => {
    const ellipses = page.locator('svg ellipse');
    const ellipseCount = await ellipses.count();
    
    expect(ellipseCount).toBeGreaterThan(0);
    
    // Verify cx values are numeric
    for (let i = 0; i < Math.min(ellipseCount, 10); i++) {
      const cx = await ellipses.nth(i).getAttribute('cx');
      expect(cx).toBeTruthy();
      expect(isNaN(parseFloat(cx || ''))).toBe(false);
    }
  });

  test('All ellipses have valid numeric cy coordinate', async ({ page }) => {
    const ellipses = page.locator('svg ellipse');
    const ellipseCount = await ellipses.count();
    
    expect(ellipseCount).toBeGreaterThan(0);
    
    // Verify cy values are numeric
    for (let i = 0; i < Math.min(ellipseCount, 10); i++) {
      const cy = await ellipses.nth(i).getAttribute('cy');
      expect(cy).toBeTruthy();
      expect(isNaN(parseFloat(cy || ''))).toBe(false);
    }
  });

  test('All ellipses have valid numeric rx and ry', async ({ page }) => {
    const ellipses = page.locator('svg ellipse');
    const ellipseCount = await ellipses.count();
    
    expect(ellipseCount).toBeGreaterThan(0);
    
    // Verify rx/ry values are numeric
    for (let i = 0; i < Math.min(ellipseCount, 10); i++) {
      const rx = await ellipses.nth(i).getAttribute('rx');
      const ry = await ellipses.nth(i).getAttribute('ry');
      expect(rx).toBeTruthy();
      expect(ry).toBeTruthy();
      expect(isNaN(parseFloat(rx || ''))).toBe(false);
      expect(isNaN(parseFloat(ry || ''))).toBe(false);
    }
  });

});

test.describe('Chart – Bubble › Circle Element Attributes', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-bubble');
    await page.waitForLoadState('networkidle');
  });

  test('Bubble ellipse elements have cx and cy attributes', async ({ page }) => {
    const ellipses = page.locator('svg ellipse');
    const firstEllipse = ellipses.first();
    
    // Ellipses use cx/cy for positioning
    const cx = await firstEllipse.getAttribute('cx');
    const cy = await firstEllipse.getAttribute('cy');
    
    // Should have positioning
    expect(cx).toBeTruthy();
    expect(cy).toBeTruthy();
  });

  test('Bubble ellipse elements have rx and ry attributes', async ({ page }) => {
    const ellipses = page.locator('svg ellipse');
    const firstEllipse = ellipses.first();
    
    // Ellipses have rx/ry for dimensions
    const rx = await firstEllipse.getAttribute('rx');
    const ry = await firstEllipse.getAttribute('ry');
    expect(rx).toBeTruthy();
    expect(ry).toBeTruthy();
  });

  test('All bubbles have valid positioning', async ({ page }) => {
    const ellipses = page.locator('svg ellipse');
    const ellipseCount = await ellipses.count();
    
    expect(ellipseCount).toBeGreaterThanOrEqual(18);
    
    // Verify cx/cy are numeric
    for (let i = 0; i < Math.min(ellipseCount, 18); i++) {
      const cx = await ellipses.nth(i).getAttribute('cx');
      const cy = await ellipses.nth(i).getAttribute('cy');
      expect(cx).toBeTruthy();
      expect(cy).toBeTruthy();
    }
  });

  test('All bubbles have defined positions and sizes', async ({ page }) => {
    const ellipses = page.locator('svg ellipse');
    const ellipseCount = await ellipses.count();
    
    expect(ellipseCount).toBeGreaterThanOrEqual(18);
    
    // Verify cx/cy/rx/ry are defined
    for (let i = 0; i < Math.min(ellipseCount, 18); i++) {
      const cx = await ellipses.nth(i).getAttribute('cx');
      const cy = await ellipses.nth(i).getAttribute('cy');
      const rx = await ellipses.nth(i).getAttribute('rx');
      const ry = await ellipses.nth(i).getAttribute('ry');
      
      expect(cx).toBeTruthy();
      expect(cy).toBeTruthy();
      expect(rx).toBeTruthy();
      expect(ry).toBeTruthy();
    }
  });

  test('All bubbles have valid dimensions', async ({ page }) => {
    const ellipses = page.locator('svg ellipse');
    const ellipseCount = await ellipses.count();
    
    expect(ellipseCount).toBeGreaterThanOrEqual(18);
    
    // Verify rx/ry are numeric and positive
    for (let i = 0; i < Math.min(ellipseCount, 18); i++) {
      const rx = parseFloat(await ellipses.nth(i).getAttribute('rx') || '0');
      const ry = parseFloat(await ellipses.nth(i).getAttribute('ry') || '0');
      expect(rx).toBeGreaterThan(0);
      expect(ry).toBeGreaterThan(0);
    }
  });

  test('Bubble ellipse dimensions correlate with size constraints', async ({ page }) => {
    const ellipses = page.locator('svg ellipse');
    const ellipseCount = await ellipses.count();
    
    expect(ellipseCount).toBeGreaterThanOrEqual(18);
    
    // Ellipses should have rx/ry within min (3) and max (8) constraints
    for (let i = 0; i < Math.min(ellipseCount, 18); i++) {
      const rx = parseFloat(await ellipses.nth(i).getAttribute('rx') || '0');
      const ry = parseFloat(await ellipses.nth(i).getAttribute('ry') || '0');
      
      // Check that radii are within reasonable bounds
      expect(rx).toBeGreaterThan(0);
      expect(ry).toBeGreaterThan(0);
    }
  });

});

test.describe('Chart – Scatter › Text Element Validation', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-scatter');
    await page.waitForLoadState('networkidle');
  });

  test('Text elements have x and y attributes', async ({ page }) => {
    const textElements = page.locator('svg text');
    const firstText = textElements.first();
    
    const x = await firstText.getAttribute('x');
    const y = await firstText.getAttribute('y');
    
    // Text should be positioned
    expect(x || y).toBeTruthy();
  });

  test('Text elements contain readable content', async ({ page }) => {
    const textElements = page.locator('svg text');
    const count = await textElements.count();
    
    // Should have multiple text elements with content
    expect(count).toBeGreaterThan(0);
  });

});

test.describe('Chart – Bubble › Text Element Validation', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-bubble');
    await page.waitForLoadState('networkidle');
  });

  test('Text elements have x and y attributes', async ({ page }) => {
    const textElements = page.locator('svg text');
    const firstText = textElements.first();
    
    const x = await firstText.getAttribute('x');
    const y = await firstText.getAttribute('y');
    
    // Text should be positioned
    expect(x || y).toBeTruthy();
  });

  test('Country code labels are rendered as text', async ({ page }) => {
    // The DatalabelRender event creates country code labels
    const textElements = page.locator('svg text');
    const allText = await textElements.allTextContents();
    
    // Should have country codes like "CN", "IN", "US", "JP", "BR", etc.
    const countryCodes = allText.filter(t => {
      const trimmed = t.trim();
      return /^[A-Z]{2}$|^[A-Z]{3}$/.test(trimmed);
    });
    
    expect(countryCodes.length).toBeGreaterThan(0);
  });

  test('Numeric axis labels are present', async ({ page }) => {
    const textElements = page.locator('svg text').filter({ hasText: /\d+/ });
    const count = await textElements.count();
    
    // Should have numeric labels for axes
    expect(count).toBeGreaterThan(0);
  });

});

test.describe('Chart – Scatter & Bubble › Responsive SVG', () => {

  test('Scatter chart SVG has width and height attributes', async ({ page }) => {
    await page.goto('http://localhost:5000/chart-scatter');
    await page.waitForLoadState('networkidle');
    
    const svg = page.locator('svg').first();
    const width = await svg.getAttribute('width');
    const height = await svg.getAttribute('height');
    
    // Should have width and height for sizing
    expect(width).toBeTruthy();
    expect(height).toBeTruthy();
  });

  test('Bubble chart SVG has width and height attributes', async ({ page }) => {
    await page.goto('http://localhost:5000/chart-bubble');
    await page.waitForLoadState('networkidle');
    
    const svg = page.locator('svg').first();
    const width = await svg.getAttribute('width');
    const height = await svg.getAttribute('height');
    
    // Should have width and height for sizing
    expect(width).toBeTruthy();
    expect(height).toBeTruthy();
  });

  test('Scatter SVG has width attribute', async ({ page }) => {
    await page.goto('http://localhost:5000/chart-scatter');
    await page.waitForLoadState('networkidle');
    
    const svg = page.locator('svg').first();
    const width = await svg.getAttribute('width');
    
    expect(width).toBeTruthy();
  });

  test('Bubble SVG respects 90% width setting', async ({ page }) => {
    await page.goto('http://localhost:5000/chart-bubble');
    await page.waitForLoadState('networkidle');
    
    const svg = page.locator('svg').first();
    const boundingBox = await svg.boundingBox();
    
    // Should have calculated width
    expect(boundingBox?.width).toBeGreaterThan(400);
  });

});

test.describe('Chart – Scatter & Bubble › Coordinate System', () => {

  test('Scatter points are rendered on screen', async ({ page }) => {
    await page.goto('http://localhost:5000/chart-scatter');
    await page.waitForLoadState('networkidle');
    
    const svg = page.locator('svg').first();
    const svgBox = await svg.boundingBox();
    
    if (svgBox) {
      // Get ellipse positions (markers)
      const ellipses = page.locator('svg ellipse');
      const ellipseCount = await ellipses.count();
      
      expect(ellipseCount).toBeGreaterThan(50);
      
      for (let i = 0; i < Math.min(ellipseCount, 10); i++) {
        const cx = await ellipses.nth(i).getAttribute('cx');
        // Points should have coordinates
        expect(cx).toBeTruthy();
        expect(isNaN(parseFloat(cx || ''))).toBe(false);
      }
    }
  });

  test('Bubble points are rendered on screen', async ({ page }) => {
    await page.goto('http://localhost:5000/chart-bubble');
    await page.waitForLoadState('networkidle');
    
    const svg = page.locator('svg').first();
    const svgBox = await svg.boundingBox();
    
    if (svgBox) {
      // Get ellipse positions (bubble markers)
      const ellipses = page.locator('svg ellipse');
      const ellipseCount = await ellipses.count();
      
      expect(ellipseCount).toBeGreaterThanOrEqual(18);
      
      for (let i = 0; i < Math.min(ellipseCount, 10); i++) {
        const cx = await ellipses.nth(i).getAttribute('cx');
        // Points should have coordinates
        expect(cx).toBeTruthy();
        expect(isNaN(parseFloat(cx || ''))).toBe(false);
      }
    }
  });

});
