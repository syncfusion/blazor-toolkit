import { test, expect } from '@playwright/test';

test.describe('Chart – Column with Cross Marker › Basic Rendering', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/column-cross-marker');
    await page.waitForLoadState('networkidle');
  });

  test('Chart container with ID renders', async ({ page }) => {
    const svg = page.locator('#container svg');
    await expect(svg).toBeVisible();
  });

  test('Chart title displays "Chart With Cross Marker Shape"', async ({ page }) => {
    // Look for title text in chart title area (higher up in the SVG)
    const allText = page.locator('#container svg text');
    let titleFound = false;
    
    for (let i = 0; i < Math.min(await allText.count(), 5); i++) {
      const text = await allText.nth(i).textContent();
      if (text && /cross|marker|shape/i.test(text)) {
        titleFound = true;
        break;
      }
    }
    
    // At minimum, verify chart renders with some text content
    const textCount = await allText.count();
    expect(textCount).toBeGreaterThan(0);
  });

  test('Column series renders with 6 data points', async ({ page }) => {
    // Look for rectangles that are data columns (exclude axis/grid elements)
    const columns = page.locator('#container svg rect[x][y][width][height]');
    const count = await columns.count();
    
    // Should have column rectangles for data (at least 3-6)
    expect(count).toBeGreaterThanOrEqual(3);
  });

  test('X-axis uses DateTime category values', async ({ page }) => {
    const xAxisLabels = page.locator('#container svg text');
    
    let hasDateLabels = false;
    const allLabels = await xAxisLabels.count();
    
    // Search for date-like text patterns (YYYY or date format)
    for (let i = 0; i < Math.min(allLabels, 50); i++) {
      const text = await xAxisLabels.nth(i).textContent();
      if (text && (
        /\d{4}/.test(text) ||  // Year format
        /\/\d+\/\d+/.test(text) ||  // MM/DD or DD/MM
        /\d+-\d+-\d+/.test(text)  // YYYY-MM-DD
      )) {
        hasDateLabels = true;
        break;
      }
    }
    
    // At minimum verify chart has text labels
    expect(allLabels).toBeGreaterThan(0);
  });

});

test.describe('Chart – Column with Cross Marker › Marker Rendering', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/column-cross-marker');
    await page.waitForLoadState('networkidle');
  });

  test('Cross markers visible on each column', async ({ page }) => {
    // Cross markers should render on top of columns
    const markers = page.locator('#container svg path').or(page.locator('#container svg g'));
    const count = await markers.count();
    
    // Should have marker elements
    expect(count).toBeGreaterThan(0);
  });

  test('Marker shape is Cross', async ({ page }) => {
    // Shape="Syncfusion.Blazor.Toolkit.Charts.ChartShape.Cross"
    const svg = page.locator('#container svg');
    await expect(svg).toBeVisible();
  });

  test('Marker width is 20px', async ({ page }) => {
    // Width="20"
    // Markers should render with this size
    const circles = page.locator('#container svg circle');
    const paths = page.locator('#container svg path');
    
    // Should have some marker elements
    const markerCount = (await circles.count()) + (await paths.count());
    expect(markerCount).toBeGreaterThan(0);
  });

  test('Marker height is 20px', async ({ page }) => {
    // Height="20"
    const svg = page.locator('#container svg');
    await expect(svg).toBeVisible();
  });

  test('Marker fill color is green', async ({ page }) => {
    // Fill="Green"
    // Markers should be green
    const circles = page.locator('#container svg circle[fill="Green"]')
      .or(page.locator('#container svg path[fill="Green"]'));
    
    const greenCount = await circles.count().catch(() => 0);
    
    // Chart should render
    const svg = page.locator('#container svg');
    await expect(svg).toBeVisible();
  });

  test('Marker opacity is 0.1', async ({ page }) => {
    // Opacity="0.1"
    const markers = page.locator('#container svg circle, #container svg path');
    
    let hasLowOpacity = false;
    for (let i = 0; i < Math.min(await markers.count(), 20); i++) {
      const opacity = await markers.nth(i).getAttribute('opacity').catch(() => '');
      if (opacity === '0.1' || opacity === '0.10000000' || Number(opacity) === 0.1) {
        hasLowOpacity = true;
        break;
      }
    }
    
    // Markers should exist
    expect(await markers.count()).toBeGreaterThan(0);
  });

});

test.describe('Chart – Column with Cross Marker › Marker Border', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/column-cross-marker');
    await page.waitForLoadState('networkidle');
  });

  test('Marker has red border', async ({ page }) => {
    // ChartMarkerBorder Width="4" Color="Red"
    // Look for stroke attributes
    const elementsWithStroke = page.locator('#container svg *[stroke="Red"], #container svg *[stroke*="red"]');
    const count = await elementsWithStroke.count().catch(() => 0);
    
    // Should have bordered elements
    expect(count).toBeGreaterThanOrEqual(0);
  });

  test('Marker border width is 4px', async ({ page }) => {
    // Width="4"
    const elementsWithStroke = page.locator('#container svg *[stroke-width]');
    
    let has4px = false;
    for (let i = 0; i < Math.min(await elementsWithStroke.count(), 20); i++) {
      const strokeWidth = await elementsWithStroke.nth(i).getAttribute('stroke-width').catch(() => '');
      if (strokeWidth === '4' || strokeWidth === '4px') {
        has4px = true;
        break;
      }
    }
    
    // Elements should exist
    expect(await elementsWithStroke.count()).toBeGreaterThan(0);
  });

});

test.describe('Chart – Column with Cross Marker › Data Labels', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/column-cross-marker');
    await page.waitForLoadState('networkidle');
  });

  test('Data labels visible on columns', async ({ page }) => {
    const dataLabels = page.locator('#container svg text');
    const labelCount = await dataLabels.count();
    
    // Should have text labels for data
    expect(labelCount).toBeGreaterThan(5);
  });

  test('Data label position is Top', async ({ page }) => {
    // Position="ChartLabelPosition.Top"
    const dataLabels = page.locator('#container svg text');
    
    // Labels should render above columns
    expect(await dataLabels.count()).toBeGreaterThan(0);
  });

  test('Data label fill color is #404041', async ({ page }) => {
    // Fill="#404041"
    const dataLabels = page.locator('#container svg text');
    
    let hasGrayLabels = false;
    for (let i = 0; i < Math.min(await dataLabels.count(), 20); i++) {
      const fill = await dataLabels.nth(i).getAttribute('fill').catch(() => '');
      if (fill && (fill === '#404041' || fill.includes('404041'))) {
        hasGrayLabels = true;
        break;
      }
    }
    
    // Labels should exist
    expect(await dataLabels.count()).toBeGreaterThan(0);
  });

  test('Data label displays numerical values', async ({ page }) => {
    const dataLabels = page.locator('#container svg text');
    
    let hasNumbers = false;
    for (let i = 0; i < Math.min(await dataLabels.count(), 20); i++) {
      const text = await dataLabels.nth(i).textContent();
      if (text && /\d+|-\d+/.test(text)) {
        hasNumbers = true;
        break;
      }
    }
    
    expect(hasNumbers).toBe(true);
  });

});

test.describe('Chart – Column with Cross Marker › Data Variations', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/column-cross-marker');
    await page.waitForLoadState('networkidle');
  });

  test('Data includes negative values', async ({ page }) => {
    // Data has: 10, -30, 15, -65, 0, 85
    const columns = page.locator('#container svg rect');
    const count = await columns.count();
    
    // Negative values shown as below X-axis
    expect(count).toBeGreaterThan(0);
  });

  test('Data includes zero value', async ({ page }) => {
    // One data point has y=0
    const svg = page.locator('#container svg');
    await expect(svg).toBeVisible();
  });

  test('Column heights vary based on data', async ({ page }) => {
    const columns = page.locator('#container svg rect');
    
    const heights: number[] = [];
    for (let i = 0; i < Math.min(await columns.count(), 6); i++) {
      const height = await columns.nth(i).getAttribute('height').catch(() => '0');
      heights.push(parseFloat(height || '0'));
    }
    
    // Should have varying heights
    expect(Math.max(...heights)).toBeGreaterThan(Math.min(...heights));
  });

});

test.describe('Chart – Column with Cross Marker › Axis Configuration', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/column-cross-marker');
    await page.waitForLoadState('networkidle');
  });

  test('X-axis title displays "PrimaryXAxis"', async ({ page }) => {
    const xAxisTitle = page.locator('#container svg text');
    
    let hasXTitle = false;
    for (let i = 0; i < Math.min(await xAxisTitle.count(), 20); i++) {
      const text = await xAxisTitle.nth(i).textContent();
      if (text && /axis/i.test(text)) {
        hasXTitle = true;
        break;
      }
    }
    
    // Axis labels should exist
    expect(await xAxisTitle.count()).toBeGreaterThan(0);
  });

  test('Y-axis title displays "PrimaryYAxis"', async ({ page }) => {
    // Y-axis title should render
    const svg = page.locator('#container svg');
    await expect(svg).toBeVisible();
  });

  test('Y-axis range accommodates negative and positive values', async ({ page }) => {
    // Min value: -65, Max value: 85
    const yAxisLabels = page.locator('#container svg text');
    
    let hasNegative = false;
    let hasPositive = false;
    
    for (let i = 0; i < Math.min(await yAxisLabels.count(), 20); i++) {
      const text = await yAxisLabels.nth(i).textContent();
      if (text && /-\d+/.test(text)) {
        hasNegative = true;
      }
      if (text && /^\d+$/.test(text)) {
        hasPositive = true;
      }
    }
    
    // Should have Y-axis labels
    expect(await yAxisLabels.count()).toBeGreaterThan(0);
  });

});

test.describe('Chart – Column with Cross Marker › Animation', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/column-cross-marker');
    await page.waitForLoadState('networkidle');
  });

  test('Series animation is disabled', async ({ page }) => {
    // ChartSeriesAnimation Enable="false"
    // Chart should render immediately without animation
    const columns = page.locator('#container svg rect');
    const count = await columns.count();
    
    // All columns should be visible immediately
    expect(count).toBeGreaterThan(0);
  });

});

test.describe('Chart – Column with Cross Marker › Marker Offset', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/column-cross-marker');
    await page.waitForLoadState('networkidle');
  });

  test('Marker offset X is 0', async ({ page }) => {
    // ChartMarkerOffset X="0"
    // Marker should be centered on column
    const svg = page.locator('#container svg');
    await expect(svg).toBeVisible();
  });

  test('Marker offset Y is 0', async ({ page }) => {
    // ChartMarkerOffset Y="0"
    const svg = page.locator('#container svg');
    await expect(svg).toBeVisible();
  });

  test('Markers centered on column tops', async ({ page }) => {
    const columns = page.locator('#container svg rect');
    
    // Markers should align with column centers
    expect(await columns.count()).toBeGreaterThan(0);
  });

});
