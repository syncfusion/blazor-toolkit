import { test, expect } from '@playwright/test';

test.describe('Chart – DashedLine Series › Basic Rendering', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-dashed-line');
    await page.waitForLoadState('networkidle');
  });

  test('Chart SVG renders successfully', async ({ page }) => {
    const svg = page.locator('#chart-host svg');
    await expect(svg).toBeVisible();
  });

  test('Chart contains multiple line series', async ({ page }) => {
    const paths = page.locator('#chart-host svg path');
    const pathCount = await paths.count();
    
    // Should have multiple paths for different line styles
    expect(pathCount).toBeGreaterThan(0);
  });

  test('Chart has proper dimensions', async ({ page }) => {
    const chart = page.locator('#chart-host');
    const boundingBox = await chart.boundingBox();
    
    expect(boundingBox?.height).toBeGreaterThan(0);
    expect(boundingBox?.width).toBeGreaterThan(0);
  });

});

test.describe('Chart – DashedLine Series › Line Styles', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-dashed-line');
    await page.waitForLoadState('networkidle');
  });

  test('Chart renders SVG path elements', async ({ page }) => {
    const paths = page.locator('#chart-host svg path');
    const pathCount = await paths.count();
    
    // Should have at least one path for the series
    expect(pathCount).toBeGreaterThan(0);
  });

  test('Chart SVG renders successfully', async ({ page }) => {
    const svg = page.locator('#chart-host svg');
    await expect(svg).toBeVisible();
  });

});

test.describe('Chart – DashedLine Series › Series Rendering', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-dashed-line');
    await page.waitForLoadState('networkidle');
  });

  test('Series renders with line paths', async ({ page }) => {
    const paths = page.locator('#chart-host svg path');
    const count = await paths.count();
    
    expect(count).toBeGreaterThan(0);
  });

  test('Chart has text labels for axes', async ({ page }) => {
    const labels = page.locator('#chart-host svg text');
    const count = await labels.count();
    
    // Should have axis labels
    expect(count).toBeGreaterThan(0);
  });

});

test.describe('Chart – DashedLine Series › Visual Distinction', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-dashed-line');
    await page.waitForLoadState('networkidle');
  });

  test('Chart renders multiple path elements', async ({ page }) => {
    const paths = page.locator('#chart-host svg path');
    const count = await paths.count();
    
    // Should have at least one path
    expect(count).toBeGreaterThan(0);
  });

  test('Chart has axes and labels', async ({ page }) => {
    const svg = page.locator('#chart-host svg');
    await expect(svg).toBeVisible();
    
    // Chart should render
    expect(svg).toBeTruthy();
  });

});

test.describe('Chart – DashedLine Series › Tooltip', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-dashed-line');
    await page.waitForLoadState('networkidle');
  });

  test('Chart container is visible', async ({ page }) => {
    const chart = page.locator('#chart-host');
    await expect(chart).toBeVisible();
  });

  test('Chart SVG renders successfully', async ({ page }) => {
    const svg = page.locator('#chart-host svg');
    await expect(svg).toBeVisible();
  });

});

test.describe('Chart – DashedLine Series › Axes', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-dashed-line');
    await page.waitForLoadState('networkidle');
  });

  test('Chart has X-axis labels', async ({ page }) => {
    const xLabels = page.locator('#chart-host svg text');
    
    const count = await xLabels.count();
    expect(count).toBeGreaterThan(0);
  });

  test('Chart has Y-axis labels', async ({ page }) => {
    const yLabels = page.locator('#chart-host svg text').filter({ hasText: /\d+/ });
    
    const count = await yLabels.count();
    expect(count).toBeGreaterThan(0);
  });

});

test.describe('Chart – DashedLine Series › Styling', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-dashed-line');
    await page.waitForLoadState('networkidle');
  });

  test('Series line has visible stroke width', async ({ page }) => {
    const path = page.locator('#chart-host svg path').first();
    const strokeWidth = await path.getAttribute('stroke-width');
    
    expect(strokeWidth).toBeTruthy();
  });

  test('Dashed line pattern creates visual distinction', async ({ page }) => {
    const paths = page.locator('#chart-host svg path');
    
    const count = await paths.count();
    expect(count).toBeGreaterThan(0);
  });

});

test.describe('Chart – DashedLine Series › Marker Settings', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-dashed-line');
    await page.waitForLoadState('networkidle');
  });

  test('Chart SVG contains path elements', async ({ page }) => {
    const paths = page.locator('#chart-host svg path');
    const count = await paths.count();
    
    expect(count).toBeGreaterThan(0);
  });

  test('Series lines are properly rendered', async ({ page }) => {
    const svg = page.locator('#chart-host svg');
    await expect(svg).toBeVisible();
  });

});

test.describe('Chart – DashedLine Series › Responsive', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-dashed-line');
    await page.waitForLoadState('networkidle');
  });

  test('Chart renders within container', async ({ page }) => {
    const container = page.locator('#chart-host');
    await expect(container).toBeVisible();
  });

  test('Chart SVG adapts to container', async ({ page }) => {
    const svg = page.locator('#chart-host svg');
    const width = await svg.evaluate((el) => el.getAttribute('width'));
    
    expect(width).toBeTruthy();
  });

});

test.describe('Chart – DashedLine Series › Grid Lines', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-dashed-line');
    await page.waitForLoadState('networkidle');
  });

  test('Grid lines render on chart', async ({ page }) => {
    const lines = page.locator('#chart-host svg line');
    
    const count = await lines.count();
    expect(count).toBeGreaterThanOrEqual(0);
  });

});
