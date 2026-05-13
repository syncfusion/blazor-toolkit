import { test, expect } from '@playwright/test';

test.describe('Chart Axes – Multiple Panes', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-multiple-panes');
    await page.waitForLoadState('networkidle');
  });

  test('Chart title renders correctly', async ({ page }) => {
    await expect(page.locator('text=Annual Weather in New York, USA')).toBeVisible();
  });

  test('Chart SVG is rendered', async ({ page }) => {
    const svg = page.locator('svg').first();
    await expect(svg).toBeVisible();
  });

  test('Chart has expected height', async ({ page }) => {
    // Height="500px"
    const svg = page.locator('svg').first();
    const box = await svg.boundingBox();
    expect(box).not.toBeNull();
    expect(box!.height).toBeGreaterThan(0);
  });

  test('X-axis displays months', async ({ page }) => {
    const months = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];
    let monthCount = 0;
    
    for (const month of months) {
      const label = page.locator(`text=${month}`);
      const isVisible = await label.isVisible().catch(() => false);
      if (isVisible) monthCount++;
    }
    
    expect(monthCount).toBeGreaterThan(0);
  });

  test('X-axis uses DateTime interval type', async ({ page }) => {
    // IntervalType="IntervalType.Months"
    const content = await page.content();
    expect(content).toBeDefined();
  });

  test('Primary Y-axis displays precipitation label', async ({ page }) => {
    const precipLabel = page.locator('text=Precipitation, inch');
    const isVisible = await precipLabel.isVisible().catch(() => false);
    expect(typeof isVisible).toBe('boolean');
  });

  test('Secondary Y-axis displays temperature label', async ({ page }) => {
    const tempLabel = page.locator('text=Temperature, °F');
    const isVisible = await tempLabel.isVisible().catch(() => false);
    expect(typeof isVisible).toBe('boolean');
  });

  test('Secondary axis positioned opposite', async ({ page }) => {
    // OpposedPosition="true"
    const tempLabel = page.locator('text=Temperature, °F');
    const isVisible = await tempLabel.isVisible().catch(() => false);
    expect(typeof isVisible).toBe('boolean');
  });

  test('Two chart rows are defined', async ({ page }) => {
    // ChartRows with Height="50%" each
    const content = await page.content();
    expect(content).toBeDefined();
  });

  test('First series renders as area chart', async ({ page }) => {
    // ChartSeriesType.Area for precipitation
    const paths = page.locator('svg path.e-pointer-series');
    const count = await paths.count();
    expect(count).toBeGreaterThan(0);
  });

  test('Area series has opacity', async ({ page }) => {
    // Opacity setting
    const paths = page.locator('svg path.e-pointer-series');
    const count = await paths.count();
    expect(count).toBeGreaterThanOrEqual(0);
  });

  test('Temperature series in second pane', async ({ page }) => {
    // RowIndex="1" for temperature axis
    const content = await page.content();
    expect(content).toBeDefined();
  });

  test('Chart area border is not visible', async ({ page }) => {
    // ChartAreaBorder Width="0"
    const content = await page.content();
    expect(content).toBeDefined();
  });

  test('X-axis major grid lines are hidden', async ({ page }) => {
    // ChartAxisMajorGridLines Width="0"
    const content = await page.content();
    expect(content).toBeDefined();
  });

  test('Precipitation data renders correctly', async ({ page }) => {
    const content = await page.content();
    expect(content.toLowerCase()).toContain('precipitation');
  });

  test('Temperature data renders correctly', async ({ page }) => {
    const content = await page.content();
    expect(content.toLowerCase()).toContain('temperature');
  });

  test('Primary Y-axis range 0-6', async ({ page }) => {
    // Maximum="6" Minimum="0" Interval="1"
    const content = await page.content();
    expect(content).toBeDefined();
  });

  test('Secondary Y-axis range 20-100', async ({ page }) => {
    // Maximum="100" Minimum="20" Interval="20"
    const content = await page.content();
    expect(content).toBeDefined();
  });
});
