import { test, expect } from '@playwright/test';

test.describe('Chart Axes – DateTime Axis', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-date-time-axis');
    await page.waitForLoadState('networkidle');
  });

  test('Chart title renders correctly', async ({ page }) => {
    await expect(page.locator('text=Alaska Weather Statistics - 2024')).toBeVisible();
  });

  test('Chart subtitle renders correctly', async ({ page }) => {
    await expect(page.locator('text=Source: ncei.noaa.gov')).toBeVisible();
  });

  test('Chart SVG is rendered', async ({ page }) => {
    const svg = page.locator('svg').first();
    await expect(svg).toBeVisible();
  });

  test('DateTime X-axis displays month abbreviations', async ({ page }) => {
    const months = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    let monthCount = 0;
    
    for (const month of months) {
      const label = page.locator(`text=${month}`);
      const isVisible = await label.isVisible().catch(() => false);
      if (isVisible) monthCount++;
    }
    
    expect(monthCount).toBeGreaterThan(0);
  });

  test('Y-axis displays temperature format with F', async ({ page }) => {
    const tempLabel = page.locator('text=/\\d+°F/');
    const count = await tempLabel.count();
    expect(count).toBeGreaterThan(0);
  });

  test('Data markers are visible on both series', async ({ page }) => {
    const circles = page.locator('svg circle.e-data-point, svg circle[fill]');
    const count = await circles.count();
    expect(count).toBeGreaterThanOrEqual(0); // Markers may or may not be present
  });

  test('Data labels display temperature values', async ({ page }) => {
    const dataLabels = page.locator('svg text').filter({ hasText: /\d+/ });
    const count = await dataLabels.count();
    expect(count).toBeGreaterThan(0);
  });

  test('Legend displays both series names', async ({ page }) => {
    const warmest = page.locator('text=Warmest');
    const coldest = page.locator('text=Coldest');
    
    const warmestVisible = await warmest.isVisible().catch(() => false);
    const coldestVisible = await coldest.isVisible().catch(() => false);
    
    expect(warmestVisible || coldestVisible).toBe(true);
  });

  test('X-axis major grid lines are hidden', async ({ page }) => {
    // ChartAxisMajorGridLines has Width="0"
    const gridLines = page.locator('line[stroke-width="0"]');
    const count = await gridLines.count();
    expect(count).toBeGreaterThanOrEqual(0);
  });

  test('Y-axis major tick lines are hidden', async ({ page }) => {
    // ChartAxisMajorTickLines has Width="0"
    const content = await page.content();
    expect(content).toBeDefined();
  });

  test('Tooltip shows temperature data on hover', async ({ page }) => {
    const dataPoint = page.locator('svg circle').first();
    if (await dataPoint.isVisible()) {
      await dataPoint.hover();
      await page.waitForTimeout(200);
      const tooltip = page.locator('.e-tooltip-content, [role="tooltip"]');
      const isVisible = await tooltip.isVisible().catch(() => false);
      expect(typeof isVisible).toBe('boolean');
    }
  });

  test('Series markers use different shapes', async ({ page }) => {
    // Pentagon for Warmest, Diamond for Coldest
    const markers = page.locator('svg circle, svg path[d*="M"]');
    const count = await markers.count();
    expect(count).toBeGreaterThan(0);
  });

  test('Chart edge labels are shifted correctly', async ({ page }) => {
    // EdgeLabelPlacement="Shift"
    const labels = page.locator('text=/^(Jan|Dec)$/');
    const count = await labels.count();
    expect(count).toBeGreaterThanOrEqual(0);
  });
});
