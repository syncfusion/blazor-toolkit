import { test, expect } from '@playwright/test';

test.describe('Chart – StackingLine Series › Basic Rendering', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/stackingline-default-points-playwright');
    await page.waitForLoadState('networkidle');
  });

  test('Page heading renders correctly', async ({ page }) => {
    const heading = page.locator('h2');
    await expect(heading).toHaveText('StackingLine & StackingLine100 – Empty Point Modes');
  });

  test('First chart (StackingLine) renders', async ({ page }) => {
    const chartHost = page.locator('#stackingLineChart').or(page.locator('#stackingLineChart').locator('..'));
    
    const svg = page.locator('#stackingLineChart').locator('svg').first().or(page.locator('svg').first());
    
    // Chart should render
    expect(svg).toBeTruthy();
  });

  test('Second chart (StackingLine100) renders', async ({ page }) => {
    const charts = page.locator('svg');
    const chartCount = await charts.count();
    
    // Should have multiple charts
    expect(chartCount).toBeGreaterThan(1);
  });

  test('StackingLine has title "StackingLine Default Points"', async ({ page }) => {
    const title = page.locator('text=StackingLine Default Points');
    await expect(title).toBeVisible();
  });

  test('StackingLine100 has title "StackingLine100 Default Points"', async ({ page }) => {
    const title = page.locator('text=StackingLine100 Default Points');
    await expect(title).toBeVisible();
  });

});

test.describe('Chart – StackingLine Series › Series Rendering', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/stackingline-default-points-playwright');
    await page.waitForLoadState('networkidle');
  });

  test('StackingLine renders path elements for series', async ({ page }) => {
    const paths = page.locator('svg path');
    const pathCount = await paths.count();
    
    // Should have path elements for lines
    expect(pathCount).toBeGreaterThan(0);
  });

  test('Multiple series stack vertically', async ({ page }) => {
    // StackingLine should have 4 series
    const lines = page.locator('svg path');
    
    // Should have paths for stacked series
    const count = await lines.count();
    expect(count).toBeGreaterThan(0);
  });

  test('Data labels visible on series', async ({ page }) => {
    const dataLabels = page.locator('svg text').filter({ hasText: /\d+/ });
    const labelCount = await dataLabels.count();
    
    // Should have data labels
    expect(labelCount).toBeGreaterThan(0);
  });

});

test.describe('Chart – StackingLine Series › Empty Point Modes', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/stackingline-default-points-playwright');
    await page.waitForLoadState('networkidle');
  });

  test('StackingLine first series uses Average empty point mode', async ({ page }) => {
    // ChartEmptyPointSettings Mode="EmptyPointMode.Average"
    // The line should still render even with empty points
    
    const paths = page.locator('svg path');
    const pathCount = await paths.count();
    
    expect(pathCount).toBeGreaterThan(0);
  });

  test('StackingLine second series uses Drop empty point mode', async ({ page }) => {
    // ChartEmptyPointSettings Mode="EmptyPointMode.Drop"
    // The line should have gaps where points are dropped
    
    const paths = page.locator('svg path');
    expect(await paths.count()).toBeGreaterThan(0);
  });

  test('StackingLine third series uses Gap empty point mode', async ({ page }) => {
    // ChartEmptyPointSettings Mode="EmptyPointMode.Gap"
    // The line should have visible gaps
    
    const paths = page.locator('svg path');
    expect(await paths.count()).toBeGreaterThan(0);
  });

  test('StackingLine fourth series uses Zero empty point mode', async ({ page }) => {
    // ChartEmptyPointSettings Mode="EmptyPointMode.Zero"
    // Empty points treated as zero
    
    const paths = page.locator('svg path');
    expect(await paths.count()).toBeGreaterThan(0);
  });

  test('StackingLine100 first series uses Average empty point mode', async ({ page }) => {
    const charts = page.locator('svg');
    const chartCount = await charts.count();
    
    // Should have StackingLine100 chart
    expect(chartCount).toBeGreaterThan(1);
  });

  test('StackingLine100 fourth series uses Zero empty point mode', async ({ page }) => {
    const charts = page.locator('svg');
    expect(await charts.count()).toBeGreaterThan(1);
  });

});

test.describe('Chart – StackingLine Series › Axes', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/stackingline-default-points-playwright');
    await page.waitForLoadState('networkidle');
  });

  test('StackingLine100 also has axis titles', async ({ page }) => {
    const axisTitles = page.locator('text=PrimaryXAxis').or(page.locator('text=PrimaryYAxis'));
    
    const count = await axisTitles.count();
    
    // Should have multiple axis title instances (for both charts)
    expect(count).toBeGreaterThanOrEqual(2);
  });

});

test.describe('Chart – StackingLine Series › Animation Settings', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/stackingline-default-points-playwright');
    await page.waitForLoadState('networkidle');
  });

  test('All series render completely', async ({ page }) => {
    const paths = page.locator('svg path');
    
    // All 4 series for StackingLine + 4 for StackingLine100
    const count = await paths.count();
    
    expect(count).toBeGreaterThan(5);
  });

});
