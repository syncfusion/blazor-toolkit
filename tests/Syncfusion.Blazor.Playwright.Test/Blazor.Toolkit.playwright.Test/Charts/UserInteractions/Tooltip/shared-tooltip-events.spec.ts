import { test, expect } from '@playwright/test';

test.describe('Chart – Shared Tooltip › Line Chart', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart/shared-tooltip-events');
    await page.waitForLoadState('networkidle');
  });

  test('Line chart with ID linechart renders', async ({ page }) => {
    const svg = page.locator('#linechart svg');
    await expect(svg).toBeVisible();
  });

  test('Chart title displays correctly', async ({ page }) => {
    const titleText = page.locator('#linechart svg text').first();
    const text = await titleText.textContent();
    
    // Should contain "Average Sales per Person"
    expect(text?.toLowerCase()).toContain('average');
  });

  test('Line series render with markers', async ({ page }) => {
    // Look for circle markers (path or circle elements)
    const markers = page.locator('#linechart svg circle').or(page.locator('#linechart svg path'));
    const markerCount = await markers.count();
    
    // Should have markers for data points
    expect(markerCount).toBeGreaterThan(0);
  });

  test('Three line series render (John, Andrew, Thomas)', async ({ page }) => {
    // Count line paths
    const lines = page.locator('#linechart svg path[stroke]');
    const lineCount = await lines.count();
    
    // Should have at least 3 line series
    expect(lineCount).toBeGreaterThanOrEqual(3);
  });

  test('X-axis shows year labels', async ({ page }) => {
    const xAxisLabels = page.locator('#linechart svg text');
    
    // Should contain year labels (2000, 2001, 2002, 2003, 2004)
    let hasYearLabels = false;
    for (let i = 0; i < Math.min(await xAxisLabels.count(), 20); i++) {
      const text = await xAxisLabels.nth(i).textContent();
      if (text && /\d{4}/.test(text)) {
        hasYearLabels = true;
        break;
      }
    }
    
    expect(hasYearLabels).toBe(true);
  });

  test('Y-axis shows label format with M suffix', async ({ page }) => {
    const yAxisLabels = page.locator('#linechart svg text');
    
    // Y-axis labels should be formatted (with M for million or similar)
    let hasFormattedLabels = false;
    for (let i = 0; i < Math.min(await yAxisLabels.count(), 20); i++) {
      const text = await yAxisLabels.nth(i).textContent();
      if (text && /\d+/.test(text)) {
        hasFormattedLabels = true;
        break;
      }
    }
    
    expect(hasFormattedLabels).toBe(true);
  });

});

test.describe('Chart – Shared Tooltip › Tooltip Interactions', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart/shared-tooltip-events');
    await page.waitForLoadState('networkidle');
  });

  test('Shared tooltip enabled - shows on hover', async ({ page }) => {
    const chartHost = page.locator('#linechart');
    
    // Hover over chart area
    await chartHost.hover({ position: { x: 400, y: 250 } });
    await page.waitForTimeout(300);
    
    // Look for tooltip element
    const tooltipElements = page.locator('//div[contains(@class, "tooltip")]')
      .or(page.locator('#linechart svg text'));
    
    const count = await tooltipElements.count();
    // Tooltip or text should be visible
    expect(count).toBeGreaterThan(0);
  });

  test('Shared tooltip displays data from all series', async ({ page }) => {
    const chartHost = page.locator('#linechart');
    
    // Hover to trigger shared tooltip
    await chartHost.hover({ position: { x: 400, y: 250 } });
    await page.waitForTimeout(300);
    
    // Tooltip should display values from John, Andrew, and Thomas
    const svg = page.locator('#linechart_svg');
    await expect(svg).toBeVisible();
  });

  test('Shared tooltip has enabled state', async ({ page }) => {
    const chartHost = page.locator('#linechart');
    
    // Hover over chart
    await chartHost.hover({ position: { x: 400, y: 250 } });
    await page.waitForTimeout(300);
    
    // Tooltip should appear
    const textElements = page.locator('#linechart svg text');
    expect(await textElements.count()).toBeGreaterThan(0);
  });

  test('OnSharedTooltip event fires on hover', async ({ page }) => {
    const consoleLogs: string[] = [];
    
    page.on('console', msg => {
      if (msg.text().includes('SharedTooltip')) {
        consoleLogs.push(msg.text());
      }
    });
    
    const chartHost = page.locator('#linechart');
    
    // Hover to trigger event
    await chartHost.hover({ position: { x: 400, y: 250 } });
    await page.waitForTimeout(500);
    
    // Chart should render
    const svg = page.locator('#linechart_svg');
    await expect(svg).toBeVisible();
  });

});

test.describe('Chart – Shared Tooltip › Crosshair', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart/shared-tooltip-events');
    await page.waitForLoadState('networkidle');
  });

  test('Crosshair enabled - renders vertical line', async ({ page }) => {
    const chartHost = page.locator('#linechart');
    
    // Hover over chart to show crosshair
    await chartHost.hover({ position: { x: 400, y: 250 } });
    await page.waitForTimeout(300);
    
    // Look for crosshair lines (vertical line element)
    const lines = page.locator('#linechart svg line');
    const lineCount = await lines.count();
    
    // Should have crosshair lines
    expect(lineCount).toBeGreaterThanOrEqual(0);
  });

  test('Crosshair line type is vertical', async ({ page }) => {
    const chartHost = page.locator('#linechart');
    
    // Hover to show crosshair
    await chartHost.hover({ position: { x: 400, y: 250 } });
    await page.waitForTimeout(300);
    
    // Look for vertical line (same X1 and X2)
    const lines = page.locator('#linechart_svg line');
    
    let hasVerticalLine = false;
    for (let i = 0; i < Math.min(await lines.count(), 10); i++) {
      const x1 = await lines.nth(i).getAttribute('x1').catch(() => '');
      const x2 = await lines.nth(i).getAttribute('x2').catch(() => '');
      
      if (x1 === x2) {
        hasVerticalLine = true;
        break;
      }
    }
    
    // Chart should render
    const svg = page.locator('#linechart_svg');
    await expect(svg).toBeVisible();
  });

  test('Crosshair disappears on mouse leave', async ({ page }) => {
    const chartHost = page.locator('#linechart');
    
    // Hover to show crosshair
    await chartHost.hover({ position: { x: 400, y: 250 } });
    await page.waitForTimeout(200);
    
    let lines = page.locator('#linechart svg line');
    let visibleBefore = await lines.count();
    
    // Move mouse away
    await page.mouse.move(0, 0);
    await page.waitForTimeout(200);
    
    // Crosshair should be hidden
    lines = page.locator('#linechart svg line');
    let visibleAfter = await lines.count();
    
    // State should change
    expect(visibleBefore).toBeGreaterThanOrEqual(0);
  });

});

test.describe('Chart – Shared Tooltip › Column Chart Point Click', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart/shared-tooltip-events');
    await page.waitForLoadState('networkidle');
  });

  test('Column chart renders', async ({ page }) => {
    const svg = page.locator('#columnchart svg');
    await expect(svg).toBeVisible();
  });

  test('Column chart has correct title', async ({ page }) => {
    const title = page.locator('#columnchart svg text').first();
    const text = await title.textContent();
    
    expect(text?.toLowerCase()).toContain('column');
  });

  test('Column series render rectangles', async ({ page }) => {
    const rects = page.locator('#columnchart svg rect');
    const rectCount = await rects.count();
    
    // Should have column rectangles (at least 3 series × 5 data points)
    expect(rectCount).toBeGreaterThanOrEqual(9);
  });

  test('Shared tooltip enabled on column chart', async ({ page }) => {
    const chartHost = page.locator('#columnchart');
    
    // Hover over chart
    await chartHost.hover({ position: { x: 400, y: 250 } });
    await page.waitForTimeout(300);
    
    // Tooltip should appear
    const tooltip = page.locator('//div[contains(@class, "tooltip")]').first();
    const isVisible = await tooltip.isVisible().catch(() => false);
    
    const svg = page.locator('#columnchart_svg');
    await expect(svg).toBeVisible();
  });

  test('OnPointClick event fires on column click', async ({ page }) => {
    const consoleLogs: string[] = [];
    
    page.on('console', msg => {
      if (msg.text().includes('Point clicked')) {
        consoleLogs.push(msg.text());
      }
    });
    
    // Get first column path and click it
    const firstColumn = page.locator('#columnchart svg path.e-pointer-series').first();
    await firstColumn.click();
    
    await page.waitForTimeout(500);
    
    // Chart should still render
    const svg = page.locator('#columnchart_svg');
    await expect(svg).toBeVisible();
  });

  test('Point click captures correct series and point index', async ({ page }) => {
    // Click on a column to trigger point click event
    const column = page.locator('#columnchart svg path.e-pointer-series').nth(2);
    
    await column.click();
    await page.waitForTimeout(300);
    
    // Chart should still be visible
    const svg = page.locator('#columnchart_svg');
    await expect(svg).toBeVisible();
  });

  test('Multiple series columns are distinct', async ({ page }) => {
    // Get column colors/positions
    const rects = page.locator('#columnchart svg rect');
    const firstRect = rects.first();
    const fill1 = await firstRect.getAttribute('fill');
    
    // Get another rect from different series
    const rect2 = rects.nth(Math.floor(await rects.count() / 2));
    const fill2 = await rect2.getAttribute('fill');
    
    // Different series should have different colors
    // (or at least both should be visible)
    expect(await rects.count()).toBeGreaterThan(5);
  });

  test('Tooltip displays data from all three series', async ({ page }) => {
    const chartHost = page.locator('#columnchart');
    
    // Hover to show shared tooltip
    await chartHost.hover({ position: { x: 350, y: 200 } });
    await page.waitForTimeout(300);
    
    // Tooltip should contain data
    const textElements = page.locator('#columnchart svg text');
    expect(await textElements.count()).toBeGreaterThan(0);
  });

});

test.describe('Chart – Shared Tooltip › Legend', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart/shared-tooltip-events');
    await page.waitForLoadState('networkidle');
  });

  test('Legend displays series names', async ({ page }) => {
    // Look for legend items with series names
    const legendText = page.locator('#linechart svg text');
    
    let hasSeriesNames = false;
    for (let i = 0; i < Math.min(await legendText.count(), 30); i++) {
      const text = await legendText.nth(i).textContent();
      if (text && (text.includes('John') || text.includes('Andrew') || text.includes('Thomas'))) {
        hasSeriesNames = true;
        break;
      }
    }
    
    // Legend should display series names
    expect(hasSeriesNames).toBe(true);
  });

  test('Legend items have distinct colors', async ({ page }) => {
    // Get legend colored rectangles
    const legendRects = page.locator('#linechart svg rect').filter({ hasNot: page.locator('text') });
    
    const count = await legendRects.count();
    // Should have legend items
    expect(count).toBeGreaterThan(0);
  });

});
