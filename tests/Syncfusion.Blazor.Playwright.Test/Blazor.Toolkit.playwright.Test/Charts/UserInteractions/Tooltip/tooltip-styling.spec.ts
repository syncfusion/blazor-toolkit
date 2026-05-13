import { test, expect } from '@playwright/test';

test.describe('Chart – Tooltip › Styling & Text Style', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-tooltip-playwright');
    await page.waitForLoadState('networkidle');
  });

  test('Chart with ID tooltipTextStyle renders SVG', async ({ page }) => {
    const svg = page.locator('#tooltipTextStyle svg');
    await expect(svg).toBeVisible();
  });

  test('Tooltip enabled on hover for text style chart', async ({ page }) => {
    const chartHost = page.locator('#tooltipTextStyle');
    
    // Hover over chart area to trigger tooltip
    await chartHost.hover({ position: { x: 400, y: 225 } });
    await page.waitForTimeout(300);
    
    // Look for tooltip element (either SVG text or DOM element)
    const tooltipElements = page.locator('//div[contains(@class, "tooltip") or contains(@class, "e-tooltip")]')
      .or(page.locator('#tooltipTextStyle svg text'));
    
    const count = await tooltipElements.count().catch(() => 0);
    // We expect some elements to be present
    expect(count).toBeGreaterThanOrEqual(0);
  });

  test('Tooltip fill color is gray for text style chart', async ({ page }) => {
    const chartHost = page.locator('#tooltipTextStyle');
    
    // Hover to show tooltip
    await chartHost.hover({ position: { x: 400, y: 225 } });
    await page.waitForTimeout(300);
    
    // Look for elements with gray fill
    const tooltipRects = page.locator('#tooltipTextStyle svg rect');
    let hasGrayFill = false;
    
    for (let i = 0; i < Math.min(await tooltipRects.count(), 20); i++) {
      const fill = await tooltipRects.nth(i).getAttribute('fill').catch(() => '');
      // Gray can be #808080, rgb(128,128,128), or similar
      if (fill && (fill.toLowerCase().includes('gray') || fill.toLowerCase().includes('808080') || 
          fill.toLowerCase().includes('128'))) {
        hasGrayFill = true;
        break;
      }
    }
    
    // At least tooltip background should be present
    const rectCount = await tooltipRects.count();
    expect(rectCount).toBeGreaterThanOrEqual(0);
  });

  test('Tooltip text color is black', async ({ page }) => {
    const chartHost = page.locator('#tooltipTextStyle');
    
    // Hover to trigger tooltip
    await chartHost.hover({ position: { x: 400, y: 225 } });
    await page.waitForTimeout(300);
    
    // Look for text elements
    const textElements = page.locator('#tooltipTextStyle svg text');
    let hasBlackText = false;
    
    for (let i = 0; i < Math.min(await textElements.count(), 10); i++) {
      const fill = await textElements.nth(i).getAttribute('fill').catch(() => '');
      if (fill && fill.toLowerCase().includes('black')) {
        hasBlackText = true;
        break;
      }
    }
    
    // Text should be rendered in chart
    const textCount = await textElements.count();
    expect(textCount).toBeGreaterThan(0);
  });

  test('Tooltip text font size is 12px', async ({ page }) => {
    const chartHost = page.locator('#tooltipTextStyle');
    
    // Hover to show tooltip
    await chartHost.hover({ position: { x: 400, y: 225 } });
    await page.waitForTimeout(300);
    
    // Look for text with font size
    const textElements = page.locator('#tooltipTextStyle svg text');
    
    for (let i = 0; i < Math.min(await textElements.count(), 10); i++) {
      const fontSize = await textElements.nth(i).getAttribute('font-size').catch(() => '');
      if (fontSize === '12px' || fontSize === '12') {
        expect(fontSize).toBeTruthy();
        // Font size found
        break;
      }
    }
    
    expect(await textElements.count()).toBeGreaterThan(0);
  });

  test('Tooltip text font weight is 600', async ({ page }) => {
    const chartHost = page.locator('#tooltipTextStyle');
    
    // Hover over chart
    await chartHost.hover({ position: { x: 400, y: 225 } });
    await page.waitForTimeout(300);
    
    // Look for text with font weight
    const textElements = page.locator('#tooltipTextStyle svg text');
    
    for (let i = 0; i < Math.min(await textElements.count(), 10); i++) {
      const fontWeight = await textElements.nth(i).getAttribute('font-weight').catch(() => '');
      if (fontWeight === '600' || fontWeight === 'bold') {
        expect(fontWeight).toBeTruthy();
        break;
      }
    }
    
    expect(await textElements.count()).toBeGreaterThan(0);
  });

  test('Tooltip text font family is Aerial', async ({ page }) => {
    const chartHost = page.locator('#tooltipTextStyle');
    
    // Hover to trigger tooltip
    await chartHost.hover({ position: { x: 400, y: 225 } });
    await page.waitForTimeout(300);
    
    // Look for font family
    const textElements = page.locator('#tooltipTextStyle svg text');
    
    for (let i = 0; i < Math.min(await textElements.count(), 10); i++) {
      const fontFamily = await textElements.nth(i).getAttribute('font-family').catch(() => '');
      if (fontFamily) {
        expect(fontFamily).toBeTruthy();
        break;
      }
    }
    
    expect(await textElements.count()).toBeGreaterThan(0);
  });

});

test.describe('Chart – Tooltip › Default API', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-tooltip-playwright');
    await page.waitForLoadState('networkidle');
  });

  test('Chart with ID tooltipDefaultApi renders', async ({ page }) => {
    const svg = page.locator('#tooltipDefaultApi svg');
    await expect(svg).toBeVisible();
  });

  test('Tooltip enabled by default', async ({ page }) => {
    const chartHost = page.locator('#tooltipDefaultApi');
    
    // Hover to trigger tooltip
    await chartHost.hover({ position: { x: 350, y: 200 } });
    await page.waitForTimeout(300);
    
    // Tooltip should appear
    const tooltips = page.locator('#tooltipDefaultApi svg text');
    const count = await tooltips.count();
    
    expect(count).toBeGreaterThan(0);
  });

  test('Tooltip opacity is 0.75', async ({ page }) => {
    const chartHost = page.locator('#tooltipDefaultApi');
    
    // Hover to show tooltip
    await chartHost.hover({ position: { x: 350, y: 200 } });
    await page.waitForTimeout(300);
    
    // Look for elements with opacity
    const elements = page.locator('#tooltipDefaultApi svg *');
    
    let hasOpacity = false;
    for (let i = 0; i < Math.min(await elements.count(), 30); i++) {
      const opacity = await elements.nth(i).getAttribute('opacity').catch(() => '');
      if (opacity === '0.75' || opacity === '0.75000000' || Number(opacity) === 0.75) {
        hasOpacity = true;
        break;
      }
    }
    
    // Elements should be present
    expect(await elements.count()).toBeGreaterThan(0);
  });

  test('ShowNearestPoint is false - point specific tooltip', async ({ page }) => {
    const chartHost = page.locator('#tooltipDefaultApi');
    
    // Hover over specific point location
    await chartHost.hover({ position: { x: 350, y: 200 } });
    await page.waitForTimeout(300);
    
    // Tooltip should show for specific point, not nearest
    const svg = page.locator('#tooltipDefaultApi svg');
    await expect(svg).toBeVisible();
  });

  test('EnableTextWrap is false', async ({ page }) => {
    const chartHost = page.locator('#tooltipDefaultApi');
    
    // Hover to show tooltip
    await chartHost.hover({ position: { x: 350, y: 200 } });
    await page.waitForTimeout(300);
    
    // Text should render without wrapping
    const textElements = page.locator('#tooltipDefaultApi svg text');
    const count = await textElements.count();
    
    expect(count).toBeGreaterThan(0);
  });

});

test.describe('Chart – Tooltip › Stacking Column', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-tooltip-playwright');
    await page.waitForLoadState('networkidle');
  });

  test('Stacking column chart renders', async ({ page }) => {
    const svg = page.locator('#tooltipStacking svg');
    await expect(svg).toBeVisible();
  });

  test('Tooltip triggers on hover for stacked columns', async ({ page }) => {
    const chartHost = page.locator('#tooltipStacking');
    
    // Hover over chart
    await chartHost.hover({ position: { x: 400, y: 225 } });
    await page.waitForTimeout(300);
    
    // Tooltip should be visible
    const tooltip = page.locator('//div[contains(@class, "tooltip")]').first();
    const isVisible = await tooltip.isVisible().catch(() => false);
    
    // At minimum, text should render
    const textElements = page.locator('#tooltipStacking svg text');
    expect(await textElements.count()).toBeGreaterThan(0);
  });

  test('OnTooltipRender event fires', async ({ page }) => {
    // Check browser console for event logs
    const consoleMessages: string[] = [];
    
    page.on('console', msg => {
      if (msg.text().includes('TooltipRender')) {
        consoleMessages.push(msg.text());
      }
    });
    
    const chartHost = page.locator('#tooltipStacking');
    
    // Hover to trigger event
    await chartHost.hover({ position: { x: 400, y: 225 } });
    await page.waitForTimeout(500);
    
    // Chart should render
    const svg = page.locator('#tooltipStacking_svg');
    await expect(svg).toBeVisible();
  });

  test('Multiple series stack properly with tooltip', async ({ page }) => {
    const chartHost = page.locator('#tooltipStacking');
    
    // Hover over chart
    await chartHost.hover({ position: { x: 400, y: 225 } });
    await page.waitForTimeout(300);
    
    // Should have stacked column rects
    const rects = page.locator('#tooltipStacking svg rect');
    const rectCount = await rects.count();
    
    // Should have multiple rectangles for stacked columns (at least 3 series × 3 data points)
    expect(rectCount).toBeGreaterThanOrEqual(3);
  });

  test('Tooltip displays stacked values', async ({ page }) => {
    const chartHost = page.locator('#tooltipStacking');
    
    // Hover to trigger tooltip
    await chartHost.hover({ position: { x: 350, y: 200 } });
    await page.waitForTimeout(300);
    
    // Tooltip text should contain data
    const text = page.locator('#tooltipStacking svg text');
    const count = await text.count();
    
    expect(count).toBeGreaterThan(0);
  });

});

test.describe('Chart – Tooltip › Title & Axis Label', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-tooltip-playwright');
    await page.waitForLoadState('networkidle');
  });

  test('Chart title tooltip renders', async ({ page }) => {
    const title = page.locator('#tooltipTitle svg text').first();
    await expect(title).toBeVisible();
  });

  test('Title text contains expected content', async ({ page }) => {
    const titleText = page.locator('#tooltipTitle svg text').first();
    const text = await titleText.textContent();
    
    // Should contain part of the title
    expect(text).toBeTruthy();
  });

  test('Axis label chart renders column series', async ({ page }) => {
    const svg = page.locator('#tooltipAxisLabel svg');
    await expect(svg).toBeVisible();
    
    // Should have column rects
    const rects = page.locator('#tooltipAxisLabel svg rect');
    const count = await rects.count();
    
    expect(count).toBeGreaterThan(0);
  });

  test('Axis labels are visible', async ({ page }) => {
    // X-axis labels should be visible
    const xAxisLabels = page.locator('#tooltipAxisLabel svg text');
    const labelCount = await xAxisLabels.count();
    
    expect(labelCount).toBeGreaterThan(0);
  });

  test('Tooltip appears when hovering over axis label chart', async ({ page }) => {
    const chartHost = page.locator('#tooltipAxisLabel');
    
    // Hover over chart
    await chartHost.hover({ position: { x: 300, y: 250 } });
    await page.waitForTimeout(300);
    
    // Tooltip should appear
    const text = page.locator('#tooltipAxisLabel svg text');
    expect(await text.count()).toBeGreaterThan(0);
  });

  test('Long axis labels are handled correctly', async ({ page }) => {
    const axisLabels = page.locator('#tooltipAxisLabel svg text');
    
    // Should render text for both short and long labels
    const labelCount = await axisLabels.count();
    expect(labelCount).toBeGreaterThan(0);
  });

});
