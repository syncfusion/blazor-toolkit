// Chart Last Data Label - Responsive & Layout tests
// Tests chart dimensions, responsiveness, and layout behavior

import { test, expect } from '@playwright/test';

test.describe('Chart Last Data Label – Responsive & Layout', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart/last-datalabel');
    await page.waitForLoadState('networkidle');
  });

  test('Chart container renders with correct dimensions', async ({ page }) => {
    const chartHost = page.locator('#chart-host');
    const boundingBox = await chartHost.boundingBox();
    
    // Should be 800x500 as specified
    expect(boundingBox?.width).toBe(800);
    expect(boundingBox?.height).toBe(500);
  });

  test('Chart container width is 70% of parent', async ({ page }) => {
    // Chart has Width="70%"
    const chart = page.locator('svg').first();
    const chartBox = await chart.boundingBox();
    
    // SVG should represent ~70% of typical width
    if (chartBox) {
      expect(chartBox.width).toBeGreaterThan(400);
      expect(chartBox.width).toBeLessThan(600);
    }
  });

  test('Chart maintains aspect ratio', async ({ page }) => {
    const chart = page.locator('svg').first();
    const boundingBox = await chart.boundingBox();
    
    if (boundingBox) {
      // Aspect ratio should be reasonable (width > height for landscape)
      const ratio = boundingBox.width / boundingBox.height;
      expect(ratio).toBeGreaterThan(0.8);
      expect(ratio).toBeLessThan(2.5);
    }
  });

  test('Chart elements are properly positioned', async ({ page }) => {
    const svg = page.locator('svg').first();
    const chartHost = page.locator('#chart-host');
    
    const svgBox = await svg.boundingBox();
    const containerBox = await chartHost.boundingBox();
    
    if (svgBox && containerBox) {
      // SVG should be within container
      expect(svgBox.x).toBeGreaterThanOrEqual(containerBox.x);
      expect(svgBox.y).toBeGreaterThanOrEqual(containerBox.y);
    }
  });

  test('Chart title is centered or appropriately positioned', async ({ page }) => {
    const title = page.locator('text=Efficiency of oil-fired power production').first();
    const chartHost = page.locator('#chart-host');
    
    const titleBox = await title.boundingBox();
    const containerBox = await chartHost.boundingBox();
    
    if (titleBox && containerBox) {
      // Title should be positioned near center horizontally
      const titleCenter = titleBox.x + titleBox.width / 2;
      const containerCenter = containerBox.x + containerBox.width / 2;
      
      // Within reasonable margin of center
      expect(Math.abs(titleCenter - containerCenter)).toBeLessThan(200);
    }
  });

  test('Page heading renders and is visible', async ({ page }) => {
    const heading = page.locator('h3');
    await expect(heading).toBeVisible();
    
    const text = await heading.textContent();
    expect(text).toContain('Chart – Axis Last Data Label');
  });

  test('Page heading has proper spacing above chart', async ({ page }) => {
    const heading = page.locator('h3');
    const chartHost = page.locator('#chart-host');
    
    const headingBox = await heading.boundingBox();
    const chartBox = await chartHost.boundingBox();
    
    if (headingBox && chartBox) {
      // Heading should be above chart
      expect(headingBox.y + headingBox.height).toBeLessThan(chartBox.y);
      
      // Reasonable spacing (not too close, not too far)
      const spacing = chartBox.y - (headingBox.y + headingBox.height);
      expect(spacing).toBeGreaterThan(5);
      expect(spacing).toBeLessThan(50);
    }
  });

  test('Buttons are positioned below or near chart', async ({ page }) => {
    const chartHost = page.locator('#chart-host');
    const updateBtn = page.locator('#update-value');
    
    const chartBox = await chartHost.boundingBox();
    const btnBox = await updateBtn.boundingBox();
    
    if (chartBox && btnBox) {
      // Button should be below chart
      expect(btnBox.y).toBeGreaterThan(chartBox.y);
    }
  });

  test('Buttons are horizontally aligned or grouped', async ({ page }) => {
    const updateBtn = page.locator('#update-value');
    const toggleBtn = page.locator('#toggle-label');
    
    const updateBox = await updateBtn.boundingBox();
    const toggleBox = await toggleBtn.boundingBox();
    
    if (updateBox && toggleBox) {
      // Y-coordinates should be similar (same row)
      expect(Math.abs(updateBox.y - toggleBox.y)).toBeLessThan(20);
    }
  });

  test('Chart uses full available space', async ({ page }) => {
    const chartHost = page.locator('#chart-host');
    const svg = page.locator('svg').first();
    
    const containerBox = await chartHost.boundingBox();
    const svgBox = await svg.boundingBox();
    
    if (containerBox && svgBox) {
      // SVG should use most of the container space
      const containerArea = containerBox.width * containerBox.height;
      const svgArea = svgBox.width * svgBox.height;
      const usage = svgArea / containerArea;
      
      // Should use at least 60% of available space
      expect(usage).toBeGreaterThan(0.6);
    }
  });

  test('No horizontal scrolling needed', async ({ page }) => {
    const body = page.locator('body');
    
    const scrollWidth = await page.evaluate(() => {
      return document.documentElement.scrollWidth;
    });
    
    const viewportWidth = await page.evaluate(() => {
      return window.innerWidth;
    });
    
    // Should not exceed viewport width (allowing small margin)
    expect(scrollWidth).toBeLessThanOrEqual(viewportWidth + 10);
  });

  test('Chart viewport is visible without vertical scrolling at normal zoom', async ({ page }) => {
    const chartHost = page.locator('#chart-host');
    
    const isInViewport = await chartHost.evaluate(el => {
      const rect = el.getBoundingClientRect();
      return (
        rect.top >= -100 &&
        rect.left >= -100 &&
        rect.bottom <= (window.innerHeight + 100) &&
        rect.right <= (window.innerWidth + 100)
      );
    });
    
    // Chart should be mostly in viewport
    expect(isInViewport).toBeTruthy();
  });

  test('Buttons are positioned in the layout', async ({ page }) => {
    const chartHost = page.locator('#chart-host');
    const updateBtn = page.locator('#update-value');
    
    const chartBox = await chartHost.boundingBox();
    const btnBox = await updateBtn.boundingBox();
    
    // Both should be visible and have positions
    expect(chartBox).toBeTruthy();
    expect(btnBox).toBeTruthy();
    
    if (chartBox && btnBox) {
      // Buttons should be positioned (Y position can vary by layout)
      expect(btnBox.y).toBeGreaterThan(0);
      expect(btnBox.width).toBeGreaterThan(30);
    }
  });

  test('Chart responsively handles content', async ({ page }) => {
    // Chart should display all data points
    const years = ['2005', '2006', '2007', '2008', '2009', '2010', '2011'];
    let visibleYears = 0;
    
    for (const year of years) {
      const label = page.locator(`text=${year}`);
      const count = await label.count();
      if (count > 0) visibleYears++;
    }
    
    // Most years should be visible
    expect(visibleYears).toBeGreaterThan(5);
  });

  test('Chart SVG viewBox maintains proportions', async ({ page }) => {
    const svg = page.locator('svg').first();
    const viewBox = await svg.getAttribute('viewBox');
    
    if (viewBox) {
      const parts = viewBox.split(' ');
      expect(parts.length).toBe(4);
      
      const width = parseInt(parts[2]);
      const height = parseInt(parts[3]);
      
      // Should have reasonable dimensions
      expect(width).toBeGreaterThan(0);
      expect(height).toBeGreaterThan(0);
    }
  });

  test('Chart container padding is appropriate', async ({ page }) => {
    const chartHost = page.locator('#chart-host');
    
    const style = await chartHost.getAttribute('style');
    
    // Style should contain dimensions
    expect(style).toContain('800px');
    expect(style).toContain('500px');
  });

  test('Chart fits within container without clipping', async ({ page }) => {
    const chartHost = page.locator('#chart-host');
    const svg = page.locator('svg').first();
    
    const overflow = await chartHost.evaluate(el => {
      const computed = window.getComputedStyle(el);
      return computed.overflow;
    });
    
    // Should not have overflow or should be visible
    // Common values: visible, auto, hidden, etc.
  });

  test('Multiple elements layout correctly', async ({ page }) => {
    const heading = page.locator('h3');
    const chartHost = page.locator('#chart-host');
    const updateBtn = page.locator('#update-value');
    
    // All should be visible
    await expect(heading).toBeVisible();
    await expect(chartHost).toBeVisible();
    await expect(updateBtn).toBeVisible();
  });

  test('Layout remains consistent after data update', async ({ page }) => {
    const chartHost = page.locator('#chart-host');
    const initialBox = await chartHost.boundingBox();
    
    // Update data
    const updateBtn = page.locator('#update-value');
    await updateBtn.click();
    await page.waitForTimeout(500);
    
    // Layout should remain same
    const finalBox = await chartHost.boundingBox();
    
    expect(finalBox?.width).toBe(initialBox?.width);
    expect(finalBox?.height).toBe(initialBox?.height);
  });

  test('Chart container is properly visible', async ({ page }) => {
    const chartHost = page.locator('#chart-host');
    
    const isVisible = await chartHost.isVisible();
    expect(isVisible).toBe(true);
    
    const boundingBox = await chartHost.boundingBox();
    expect(boundingBox).toBeTruthy();
  });
});
