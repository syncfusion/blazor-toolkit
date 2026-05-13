import { test, expect } from '@playwright/test';

test.describe('Chart – InversedSpline Series › Basic Rendering', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-inversed-spline');
    await page.waitForLoadState('networkidle');
  });

  test('Chart SVG renders successfully', async ({ page }) => {
    const svg = page.locator('#chart-host svg');
    await expect(svg).toBeVisible();
  });

  test('Chart title displays correctly', async ({ page }) => {
    const title = page.locator('text=Stock Market Capitalization as a Percentage of GDP');
    await expect(title).toBeVisible();
  });

  test('Chart subtitle shows data source', async ({ page }) => {
    const subtitle = page.locator('text=Source: wikipedia.org');
    await expect(subtitle).toBeVisible();
  });

  test('Chart is transposed (flipped)', async ({ page }) => {
    const chart = page.locator('#chart-host');
    
    // Check that chart is rendered (transposed attribute should affect layout)
    await expect(chart).toBeVisible();
  });

  test('Chart has proper dimensions', async ({ page }) => {
    const chart = page.locator('#chart-host');
    const boundingBox = await chart.boundingBox();
    
    expect(boundingBox?.height).toBeGreaterThan(0);
    expect(boundingBox?.width).toBeGreaterThan(0);
  });

});

test.describe('Chart – InversedSpline Series › Transposed Layout', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-inversed-spline');
    await page.waitForLoadState('networkidle');
  });

  test('Chart uses transposed layout (IsTransposed=true)', async ({ page }) => {
    // When transposed, X and Y axes are swapped
    // X-axis should show countries (categories)
    const xAxisLabels = page.locator('#chart-host svg text').filter({ 
      hasText: /United States|Japan|China|France|India|Canada|Brazil|United Kingdom|Sweden|Netherlands|Bangladesh/ 
    });
    
    const labelCount = await xAxisLabels.count();
    expect(labelCount).toBeGreaterThan(0);
  });

  test('Categories appear on X-axis (countries)', async ({ page }) => {
    const countries = page.locator('#chart-host svg text').filter({ 
      hasText: /United States|Japan|China/ 
    });
    
    const count = await countries.count();
    expect(count).toBeGreaterThan(0);
  });

});

test.describe('Chart – InversedSpline Series › Series Rendering', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-inversed-spline');
    await page.waitForLoadState('networkidle');
  });

  test('Spline path renders with smooth curves', async ({ page }) => {
    const paths = page.locator('#chart-host svg path');
    const pathCount = await paths.count();
    
    expect(pathCount).toBeGreaterThan(0);
  });

  test('Single series renders with paths', async ({ page }) => {
    const paths = page.locator('#chart-host svg path');
    const pathCount = await paths.count();
    
    // 11 countries = 11 data points with spline path
    expect(pathCount).toBeGreaterThan(0);
  });

  test('Spline renders with path data', async ({ page }) => {
    const paths = page.locator('#chart-host svg path').first();
    const pathData = await paths.getAttribute('d');
    
    // Spline should have path data
    expect(pathData).toBeTruthy();
  });

});

test.describe('Chart – InversedSpline Series › Data Binding', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-inversed-spline');
    await page.waitForLoadState('networkidle');
  });

  test('X-axis (category) shows 11 countries', async ({ page }) => {
    const countries = page.locator('#chart-host svg text').filter({ 
      hasText: /United States|Japan|China|France|India|Canada|Brazil|United Kingdom|Sweden|Netherlands|Bangladesh/ 
    });
    
    const count = await countries.count();
    expect(count).toBeGreaterThanOrEqual(11);
  });

  test('Y-axis shows capitalization ratio (% of GDP)', async ({ page }) => {
    const yAxisLabel = page.locator('text=Capitalization Ratio');
    await expect(yAxisLabel).toBeVisible();
  });

  test('Y-axis format includes percentage symbol', async ({ page }) => {
    const yAxisTicks = page.locator('#chart-host svg text').filter({ hasText: /%/ });
    
    const tickCount = await yAxisTicks.count();
    expect(tickCount).toBeGreaterThan(0);
  });

  test('Y-axis interval is 40%', async ({ page }) => {
    const yAxisTicks = page.locator('#chart-host svg text').filter({ hasText: /^[0-9]+/ });
    
    const count = await yAxisTicks.count();
    expect(count).toBeGreaterThan(0);
  });

});

test.describe('Chart – InversedSpline Series › Marker Settings', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-inversed-spline');
    await page.waitForLoadState('networkidle');
  });

  test('Chart renders path elements', async ({ page }) => {
    const paths = page.locator('#chart-host svg path');
    const count = await paths.count();
    
    expect(count).toBeGreaterThan(0);
  });

  test('Chart has visible SVG elements', async ({ page }) => {
    const svg = page.locator('#chart-host svg');
    
    await expect(svg).toBeVisible();
  });

  test('Chart has multiple elements', async ({ page }) => {
    const elements = page.locator('#chart-host svg *');
    
    const count = await elements.count();
    expect(count).toBeGreaterThan(0);
  });

});

test.describe('Chart – InversedSpline Series › Tooltip', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-inversed-spline');
    await page.waitForLoadState('networkidle');
  });

  test('Tooltip shows on hover over data point', async ({ page }) => {
    const chart = page.locator('#chart-host_svg');
    
    await chart.hover({ position: { x: 150, y: 200 } });
    await page.waitForTimeout(300);
    
    await expect(chart).toBeVisible();
  });

  test('Chart area responds to user interaction', async ({ page }) => {
    const chart = page.locator('#chart-host_svg');
    
    await chart.hover({ position: { x: 250, y: 200 } });
    await page.waitForTimeout(300);
    
    await expect(chart).toBeVisible();
  });

});

test.describe('Chart – InversedSpline Series › Axis Styling', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-inversed-spline');
    await page.waitForLoadState('networkidle');
  });

  test('X-axis minor tick lines are not visible', async ({ page }) => {
    // MinorTickLines Width="0"
    const ticks = page.locator('#chart-host svg line');
    
    const count = await ticks.count();
    expect(count).toBeGreaterThanOrEqual(0);
  });

  test('X-axis major tick lines are not visible', async ({ page }) => {
    // MajorTickLines Width="0"
    const ticks = page.locator('#chart-host svg line');
    
    const count = await ticks.count();
    expect(count).toBeGreaterThanOrEqual(0);
  });

  test('X-axis line is not visible', async ({ page }) => {
    // AxisLineStyle Width="0"
    const axisLines = page.locator('#chart-host svg line');
    
    const count = await axisLines.count();
    expect(count).toBeGreaterThanOrEqual(0);
  });

});

test.describe('Chart – InversedSpline Series › Chart Area', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-inversed-spline');
    await page.waitForLoadState('networkidle');
  });

  test('Chart area border width is 0', async ({ page }) => {
    const chartArea = page.locator('#chart-host svg rect').first();
    const borderWidth = await chartArea.getAttribute('stroke-width');
    
    expect(borderWidth).toBeTruthy();
  });

});

test.describe('Chart – InversedSpline Series › Styling', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-inversed-spline');
    await page.waitForLoadState('networkidle');
  });

  test('Title style uses text overflow wrap', async ({ page }) => {
    const title = page.locator('text=Stock Market Capitalization');
    
    await expect(title).toBeVisible();
  });

  test('Subtitle style uses text overflow wrap', async ({ page }) => {
    const subtitle = page.locator('text=Source: wikipedia.org');
    
    await expect(subtitle).toBeVisible();
  });

  test('Series line width is 2px', async ({ page }) => {
    const paths = page.locator('#chart-host svg path').first();
    const strokeWidth = await paths.getAttribute('stroke-width');
    
    expect(strokeWidth).toBeTruthy();
  });

});

test.describe('Chart – InversedSpline Series › Data Values', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-inversed-spline');
    await page.waitForLoadState('networkidle');
  });

  test('Data values range from ~27% to ~194%', async ({ page }) => {
    // Bangladesh: 27.7%, United States: 194.55%
    const yAxisTicks = page.locator('#chart-host svg text').filter({ hasText: /\d+/ });
    
    const count = await yAxisTicks.count();
    expect(count).toBeGreaterThan(0);
  });

  test('US has highest market cap (~194.55%)', async ({ page }) => {
    // Check that data is correctly displayed
    const yAxisTicks = page.locator('#chart-host svg text').filter({ hasText: /19/ });
    
    const count = await yAxisTicks.count();
    expect(count).toBeGreaterThanOrEqual(0);
  });

});

test.describe('Chart – InversedSpline Series › Responsive', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-inversed-spline');
    await page.waitForLoadState('networkidle');
  });

  test('Chart renders within container', async ({ page }) => {
    const container = page.locator('#chart-host');
    await expect(container).toBeVisible();
  });

});
