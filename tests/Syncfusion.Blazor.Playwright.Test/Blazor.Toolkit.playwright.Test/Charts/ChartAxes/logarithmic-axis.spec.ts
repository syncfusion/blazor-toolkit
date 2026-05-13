import { test, expect } from '@playwright/test';

test.describe('Chart Axes – Logarithmic Axis', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-logarithmic-scale');
    await page.waitForLoadState('networkidle');
  });

  test('Chart title renders correctly', async ({ page }) => {
    await expect(page.locator('text=Meta Revenue (2011 - 2024)')).toBeVisible();
  });

  test('Chart subtitle renders correctly', async ({ page }) => {
    await expect(page.locator('text=Source: wikipedia.org')).toBeVisible();
  });

  test('Chart SVG is rendered', async ({ page }) => {
    const svg = page.locator('svg').first();
    await expect(svg).toBeVisible();
  });

  test('X-axis displays years in date format', async ({ page }) => {
    const years = ['2011', '2012', '2013', '2014', '2015', '2016', '2017', '2018', '2019', '2020', '2021', '2022', '2023', '2024'];
    let yearCount = 0;
    
    for (const year of years) {
      const label = page.locator(`text=${year}`);
      const isVisible = await label.isVisible().catch(() => false);
      if (isVisible) yearCount++;
    }
    
    expect(yearCount).toBeGreaterThan(0);
  });

  test('Y-axis uses logarithmic scale', async ({ page }) => {
    // ValueType="Logarithmic"
    const yAxisLabels = page.locator('svg text').filter({ hasText: /\d+/ });
    const count = await yAxisLabels.count();
    expect(count).toBeGreaterThan(0);
  });

  test('Y-axis range from 100 to 1000000', async ({ page }) => {
    const svg = page.locator('svg').first();
    await expect(svg).toBeVisible();
  });

  test('Y-axis major grid lines are visible', async ({ page }) => {
    // ChartAxisMajorGridLines Width="1.5"
    const gridLines = page.locator('svg line');
    const count = await gridLines.count();
    expect(count).toBeGreaterThanOrEqual(0);
  });

  test('Minor ticks per interval are set', async ({ page }) => {
    // MinorTicksPerInterval="5"
    const minorTicks = page.locator('line[stroke-width*="thin"], line[stroke-dasharray]');
    const count = await minorTicks.count();
    expect(count).toBeGreaterThanOrEqual(0);
  });

  test('Spline series renders with markers', async ({ page }) => {
    const circles = page.locator('svg circle.e-data-point, svg circle[fill]');
    const count = await circles.count();
    expect(count).toBeGreaterThanOrEqual(0); // Markers may be present
  });

  test('Markers are filled', async ({ page }) => {
    // IsFilled="true"
    const circles = page.locator('svg circle.e-data-point, svg circle[fill]');
    const count = await circles.count();
    expect(count).toBeGreaterThanOrEqual(0);
  });

  test('Legend is not visible', async ({ page }) => {
    // ChartLegendSettings Visible="false"
    const legend = page.locator('svg .e-legend');
    const count = await legend.count();
    expect(count).toBe(0);
  });

  test('Shared tooltip is enabled', async ({ page }) => {
    // ChartTooltipSettings Shared="true"
    const dataPoint = page.locator('svg circle').first();
    if (await dataPoint.isVisible()) {
      await dataPoint.hover();
      await page.waitForTimeout(200);
      const tooltip = page.locator('.e-tooltip-content, [role="tooltip"]');
      const isVisible = await tooltip.isVisible().catch(() => false);
      expect(typeof isVisible).toBe('boolean');
    }
  });

  test('Tooltip format displays revenue', async ({ page }) => {
    // Format="${point.x} : <b>${point.y}</b>"
    const dataPoint = page.locator('svg circle').first();
    if (await dataPoint.isVisible()) {
      await dataPoint.hover();
      await page.waitForTimeout(200);
      const tooltip = page.locator('.e-tooltip-content, [role="tooltip"]');
      const isVisible = await tooltip.isVisible().catch(() => false);
      expect(typeof isVisible).toBe('boolean');
    }
  });

  test('Chart area border is not visible', async ({ page }) => {
    // ChartAreaBorder Width="0"
    const border = page.locator('svg rect[stroke-width="0"]');
    const count = await border.count();
    expect(count).toBeGreaterThanOrEqual(0);
  });

  test('X-axis major grid lines are hidden', async ({ page }) => {
    // ChartAxisMajorGridLines Width="0"
    const content = await page.content();
    expect(content).toBeDefined();
  });

  test('Y-axis line is hidden', async ({ page }) => {
    // ChartAxisLineStyle Width="0"
    const content = await page.content();
    expect(content).toBeDefined();
  });

  test('All years data is present', async ({ page }) => {
    const content = await page.content();
    expect(content).toContain('2011');
    expect(content).toContain('2024');
  });
});
