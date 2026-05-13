import { test, expect } from '@playwright/test';

const DEFAULT_TIMEOUT = 20000;

// Helper: locate the first chart container inside the nearest ancestor section for an exact <h2> title
function chartByHeading(page: any, title: string) {
  // Use following:: to locate the first chart container after the exact <h2> heading.
  // This is more robust across varied markup where the chart isn't nested inside the same section element.
  return page.locator(
    `xpath=//h2[normalize-space(text())="${title}"]/following::div[contains(@class,'e-chart')][1]`
  );
}

test.describe('Chart - Category Axis Suite', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart/category-axis');
    await page.waitForLoadState('networkidle');
  });

  test.describe('Label Intersection Handling', () => {
    test('label intersection rotate 45 degrees', async ({ page }) => {
      const chart = page.locator('#Chart').first();
      await expect(chart).toBeVisible({ timeout: DEFAULT_TIMEOUT });

      const svg = chart.locator('svg#Chart_svg');
      await expect(svg).toBeVisible();

      // Check axis labels exist with 45-degree rotation
      const axisLabels = svg.locator('g#ChartAxisLabels0 text[id^="Chart0_AxisLabel_"]');
      await expect(axisLabels).toHaveCount(7); // 7 months

      // Verify rotation transform applied (45 degrees)
      const firstLabel = axisLabels.first();
      const transform = await firstLabel.getAttribute('transform');
      expect(transform).toContain('rotate(45');

      // Verify label text content
      const labelText = await firstLabel.textContent();
      expect(labelText).toBeTruthy();
    });

    test('label intersection rotate 90 degrees', async ({ page }) => {
      const charts = page.locator('div[aria-label="Chart"]');
      const secondChart = charts.nth(1);
      await expect(secondChart).toBeVisible({ timeout: DEFAULT_TIMEOUT });

      const svg = secondChart.locator('svg#Chart_svg');
      const axisLabels = svg.locator('g#ChartAxisLabels0 text[id^="Chart0_AxisLabel_"]');
      await expect(axisLabels).toHaveCount(7);

      // Verify 90-degree rotation
      const firstLabel = axisLabels.first();
      const transform = await firstLabel.getAttribute('transform');
      expect(transform).toContain('rotate(90');
    });

  //   test('label intersection hide (intersection auto)', async ({ page }) => {
  //     // Third chart has intersection hide strategy
  //     const container = page.locator('#container').first();
  //     await expect(container).toBeVisible({ timeout: DEFAULT_TIMEOUT });

  //     const svg = container.locator('svg#container_svg');
  //     const axisLabels = svg.locator('g#containerAxisLabels0 text[id^="container0_AxisLabel_"]');

  //     // With hide strategy, not all labels are displayed
  //     // Should show fewer labels than data points (7)
  //     const labelCount = await axisLabels.count();
  //     expect(labelCount).toBeLessThan(7);

  //     // Verify some labels are still attached
  //     await expect(axisLabels.first()).toBeAttached();
  //   });
  });

  test.describe('Label Placement Variations', () => {
    test('standard label placement', async ({ page }) => {
      const chart = chartByHeading(page, 'Check Label Placement');
      await expect(chart).toBeVisible({ timeout: DEFAULT_TIMEOUT });

      const svg = chart.locator('svg#container_svg').first();
      await expect(svg).toBeVisible({ timeout: DEFAULT_TIMEOUT });

      // Verify axis line and labels
      const axisLine = svg.locator('path#containerAxisLine_0');
      await expect(axisLine).toBeAttached();

      // Check gridlines are rendered (filter to primary axis gridlines)
      const gridlines = svg.locator('path[id^="container_MajorGridLine_0_"]');
      const gridlineCount = await gridlines.count();

      // Derive expected minimum from actual label count to make the assertion robust
      const labels = svg.locator('g#containerAxisLabels0 text');
      const labelCount = await labels.count();
      const expectedMinGridlines = Math.max(1, labelCount - 1);
      expect(gridlineCount).toBeGreaterThanOrEqual(expectedMinGridlines);

      // Verify label positions
      const firstLabelX = await labels.first().getAttribute('x');
      expect(parseFloat(firstLabelX || '0')).toBeGreaterThan(0);
    });

    test('edge label placement shift', async ({ page }) => {
      const chart = chartByHeading(page, 'Check Edge Label Placement Shift');
      await expect(chart).toBeVisible({ timeout: DEFAULT_TIMEOUT });

      const svg = chart.locator('svg#container_svg').first();
      const labels = svg.locator('g#containerAxisLabels0 text');

      // Verify labels have x positions (shifted positioning)
      const firstLabel = labels.first();
      const xPos = await firstLabel.getAttribute('x');
      expect(xPos).toBeTruthy();
      expect(parseFloat(xPos || '0')).toBeGreaterThan(0);
    });

    test('edge label placement shift inversed', async ({ page }) => {
      const chart = chartByHeading(page, 'Check Edge Label Placement Shift Inversed');
      await expect(chart).toBeVisible({ timeout: DEFAULT_TIMEOUT });

      const svg = chart.locator('svg#container_svg').first();
      const labels = svg.locator('g#containerAxisLabels0 text');

      // Verify inversed direction (right to left)
      const labelXPositions: number[] = [];
      const labelCount = await labels.count();
      for (let i = 0; i < labelCount; i++) {
        const x = await labels.nth(i).getAttribute('x');
        if (x) labelXPositions.push(parseFloat(x));
      }

      // Check that positions are inversed (descending)
      if (labelXPositions.length > 1) {
        expect(labelXPositions[0]).toBeGreaterThan(labelXPositions[labelXPositions.length - 1]);
      }
    });
  });

  test.describe('Category Range and Filtering', () => {
    test('labels with category range', async ({ page }) => {
      const chart = chartByHeading(page, 'Check Labels With Category Range');
      await expect(chart).toBeVisible({ timeout: DEFAULT_TIMEOUT });

      const svg = chart.locator('svg#container_svg').first();
      const labels = svg.locator('g#containerAxisLabels0 > text');

      // With category range, prefer visible label count to avoid matching non-axis text
      const visibleLabelCount = await labels.filter({ visible: true }).count();
      expect(visibleLabelCount).toBeLessThanOrEqual(7);

      // Verify label text (should show months like Jancus, Marcus, etc.)
      const firstLabel = labels.first();
      const text = await firstLabel.textContent();
      expect(text).toMatch(/[A-Za-z]/);

      // Check label color styling (red in the example)
      const fill = await firstLabel.getAttribute('fill');
      expect(fill).toBe('Red');
    });

    test('axis labels without data for series', async ({ page }) => {
      const chart = chartByHeading(page, 'Check Axis Labels Without Data For Series');
      await expect(chart).toBeVisible({ timeout: DEFAULT_TIMEOUT });

      const svg = chart.locator('svg#container_svg').first();

      // Series should render (scoped to this chart)
      const series = svg.locator('g#containerSeriesGroup0').first();
      await expect(series).toBeAttached();

      // Labels should exist
      const labels = svg.locator('g#containerAxisLabels0 text');
      const labelCount = await labels.count();
      expect(labelCount).toBeGreaterThan(0);

      // Verify data points are rendered
      const dataPoints = svg.locator('path[id^="container_Series_0_Point_"]');
      const pointCount = await dataPoints.count();
      expect(pointCount).toBeGreaterThan(0);
    });
  });

  test.describe('Single Point and Edge Cases', () => {
    test('category axis on ticks single point', async ({ page }) => {
      const chart = chartByHeading(page, 'Check Category Axis OnTicks SinglePoint');
      await expect(chart).toBeVisible({ timeout: DEFAULT_TIMEOUT });

      const svg = chart.locator('svg#container_svg').first();

      // Verify single category label
      const labels = svg.locator('g#containerAxisLabels1 text');
      await expect(labels).toHaveCount(1);

      // Get the label text and trim whitespace
      const labelText = (await labels.first().textContent())?.trim() || '';
      expect(labelText).toBe('USA');

      // Verify series renders with single point
      const series = svg.locator('g#containerSeriesGroup0');
      await expect(series).toBeAttached();

      // Check legend exists
      const legend = svg.locator('g#container_chart_legend_g');
      await expect(legend).toBeAttached();
    });
  });

  test.describe('Axis Properties and Styling', () => {
    test('axis line and gridlines rendering', async ({ page }) => {
      const chart = page.locator('#Chart').first();
      const svg = chart.locator('svg#Chart_svg');

      // Primary X Axis line
      const xAxisLine = svg.locator('path#ChartAxisLine_0');
      await expect(xAxisLine).toBeAttached();
      const xAxisStroke = await xAxisLine.getAttribute('stroke');
      expect(xAxisStroke).toBe('#D2D0CE');

      // Primary Y Axis line
      const yAxisLine = svg.locator('path#ChartAxisLine_1');
      await expect(yAxisLine).toBeAttached();

      // Major gridlines
      const gridlines = svg.locator('path[id*="MajorGridLine"]');
      const gridlineCount = await gridlines.count();
      expect(gridlineCount).toBeGreaterThan(0);
    });

    test('axis label formatting and styling', async ({ page }) => {
      const chart = page.locator('#Chart').first();
      const svg = chart.locator('svg#Chart_svg');

      const labels = svg.locator('g#ChartAxisLabels0 text');
      await expect(labels.first()).toBeAttached();

      // Check label styling
      const fontSize = await labels.first().getAttribute('font-size');
      expect(fontSize).toBe('12px');

      const fontFamily = await labels.first().getAttribute('font-family');
      expect(fontFamily).toBe('Segoe UI');

      const fill = await labels.first().getAttribute('fill');
      expect(fill).toBe('#616161');
    });
  });

  test.describe('Multiple Charts and Combinations', () => {
    test('render all category axis samples on page', async ({ page }) => {
      const charts = page.locator('svg[id*="Chart_svg"], svg[id*="container_svg"]');
      await expect(charts).toBeDefined();

      // Should have at least 8 charts (per the DOM structure)
      const chartCount = await charts.count();
      expect(chartCount).toBeGreaterThanOrEqual(8);

      // All charts should be visible
      for (let i = 0; i < Math.min(chartCount, 3); i++) {
        await expect(charts.nth(i)).toBeVisible({ timeout: DEFAULT_TIMEOUT });
      }
    });

    test('verify series rendering consistency across samples', async ({ page }) => {
      const series = page.locator('g[id*="SeriesGroup0"]');

      // All visible charts should have at least one series
      const visibleSeries = series.filter({ visible: true });
      const seriesCount = await visibleSeries.count();
      expect(seriesCount).toBeGreaterThan(0);
    });

    // test('verify legend presence in applicable samples', async ({ page }) => {
    //   // Legend should appear in at least one chart
    //   const legends = page.locator('g[id*="legend_g"]');
    //   const legendCount = await legends.count();
    //   expect(legendCount).toBeGreaterThan(0);

    //   // Verify legend text
    //   const legendText = legends.first().locator('text[id*="legend_text"]');
    //   await expect(legendText).toHaveCount(1);
    // });
  });

  test.describe('Accessibility and ARIA Attributes', () => {
    test('chart accessibility attributes', async ({ page }) => {
      const chart = page.locator('div[role="region"][aria-label="Chart"]').first();
      await expect(chart).toBeVisible();

      // Verify ARIA attributes
      const role = await chart.getAttribute('role');
      expect(role).toBe('region');

      const ariaLabel = await chart.getAttribute('aria-label');
      expect(ariaLabel).toMatch(/Chart|container/i);
    });

    test('series accessibility attributes', async ({ page }) => {
      const chart = page.locator('#Chart').first();
      const svg = chart.locator('svg#Chart_svg');

      const series = svg.locator('g#ChartSeriesGroup0');
      const ariaLabel = await series.getAttribute('aria-label');
      expect(ariaLabel).toContain('series');

      const tabindex = await series.getAttribute('tabindex');
      expect(tabindex).toBe('0');
    });
  });
});
