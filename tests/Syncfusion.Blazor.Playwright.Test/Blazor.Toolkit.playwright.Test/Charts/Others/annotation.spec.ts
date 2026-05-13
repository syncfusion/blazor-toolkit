import { test, expect } from '@playwright/test';

test.describe('Chart - Annotation', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart/annotation');
    await page.waitForLoadState('networkidle');
  });

  test('should render two charts with annotations successfully', async ({ page }) => {
    // Verify both chart containers are rendered
    const charts = page.locator('.e-chart');
    await expect(charts).toHaveCount(2);

    // Verify first chart exists and has proper class
    const firstChart = page.locator('#container');
    await expect(firstChart).toBeVisible();
    await expect(firstChart).toHaveClass(/e-chart/);

    // Verify second chart exists and has proper class
    const secondChart = page.locator('#container2');
    await expect(secondChart).toBeVisible();
    await expect(secondChart).toHaveClass(/e-chart/);
  });

  test('should render chart annotation with content template', async ({ page }) => {
    // Verify first annotation is rendered
    const firstAnnotation = page.locator('#container_Annotation_0');
    await expect(firstAnnotation).toBeVisible();

    // Verify annotation has accessibility attributes
    const ariaLabel = await firstAnnotation.getAttribute('aria-label');
    expect(ariaLabel).toBe('Medal count');

    const role = await firstAnnotation.getAttribute('role');
    expect(role).toBe('count');

    // Verify annotation content is visible
    const annotationContent = firstAnnotation.locator('div');
    await expect(annotationContent).toContainText('Highest Medal Count');
  });

  test('should have chart series rendered', async ({ page }) => {
    // Verify both charts have series elements
    const firstChartSeries = page.locator('#containerSeriesGroup0');
    await expect(firstChartSeries).toBeVisible();

    const secondChartSeries = page.locator('#container2SeriesGroup0');
    await expect(secondChartSeries).toBeVisible();

    // Verify series have data points
    const firstChartPoints = page.locator('#container_Series_0_Point_0');
    await expect(firstChartPoints).toBeVisible();

    const secondChartPoints = page.locator('#container2_Series_0_Point_0');
    await expect(secondChartPoints).toBeVisible();
  });

  test('should render chart titles', async ({ page }) => {
    // Verify first chart title
    const firstTitle = page.locator('#container_ChartTitle');
    await expect(firstTitle).toBeVisible();
    await expect(firstTitle).toHaveText('Olympic Medals');

    // Verify second chart title
    const secondTitle = page.locator('#container2_ChartTitle');
    await expect(secondTitle).toBeVisible();
    await expect(secondTitle).toHaveText('Olympic Medals');
  });

  test('should have proper accessibility attributes on charts', async ({ page }) => {
    // Verify first chart accessibility
    const firstChart = page.locator('#container');
    const firstRole = await firstChart.getAttribute('role');
    const firstAriaLabel = await firstChart.getAttribute('aria-label');

    expect(firstRole).toBe('region');
    expect(firstAriaLabel).toBeTruthy();

    // Verify second chart accessibility
    const secondChart = page.locator('#container2');
    const secondRole = await secondChart.getAttribute('role');
    const secondAriaLabel = await secondChart.getAttribute('aria-label');

    expect(secondRole).toBe('region');
    expect(secondAriaLabel).toBeTruthy();
  });

  test('should render chart annotation SVG elements', async ({ page }) => {
    // Verify SVG elements are rendered for the charts
    const firstChartSvg = page.locator('#container_svg');
    await expect(firstChartSvg).toBeVisible();

    const secondChartSvg = page.locator('#container2_svg');
    await expect(secondChartSvg).toBeVisible();

    // Verify chart border elements
    const firstBorder = page.locator('#container_ChartBorder');
    await expect(firstBorder).toBeVisible();

    const secondBorder = page.locator('#container2_ChartBorder');
    await expect(secondBorder).toBeVisible();
  });

  test('annotation should be positioned correctly with Point coordinates', async ({ page }) => {
    // Get the first annotation which uses Point coordinates
    const annotation = page.locator('#container_Annotation_0');
    const style = await annotation.getAttribute('style');

    // Verify transform includes -50% for centering (translate(-50%, -50%))
    expect(style).toContain('translate(-50%, -50%)');
    expect(style).toContain('position: absolute');
    expect(style).toContain('z-index: 1');
  });
});
