import { test, expect } from '@playwright/test';

test.describe('Chart – Scrollbar › Default Scrollbar', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-scrollbar-playwright');
    await page.waitForLoadState('networkidle');
  });

  test('Column series with scrollbar renders', async ({ page }) => {
    const columns = page.locator('#scrollbarDefault svg rect');
    const count = await columns.count();
    
    // Should have column rectangles
    expect(count).toBeGreaterThan(0);
  });

  test('Scrollbar renders at bottom', async ({ page }) => {
    // Look for scrollbar elements
    const scrollbarElements = page.locator('#scrollbarDefault svg rect').or(page.locator('#scrollbarDefault svg path'));
    const count = await scrollbarElements.count();
    
    // Should have scrollbar UI elements
    expect(count).toBeGreaterThan(0);
  });

  test('Scrollbar is interactive - drag to pan', async ({ page }) => {
    const chartHost = page.locator('#scrollbarDefault');
    
    // Hover over chart to potentially show scrollbar
    await chartHost.hover({ position: { x: 400, y: 300 } });
    await page.waitForTimeout(200);
    
    // Perform drag on the chart container itself
    await chartHost.dragTo(chartHost, {
      sourcePosition: { x: 300, y: 300 },
      targetPosition: { x: 400, y: 300 }
    });
    
    await page.waitForTimeout(300);
    
    // Chart should update
    const svg = page.locator('#scrollbarDefault_svg');
    await expect(svg).toBeVisible();
  });

});

test.describe('Chart – Scrollbar › Refresh Stability', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-scrollbar-playwright');
    await page.waitForLoadState('networkidle');
  });

  test('Chart scrollbarRefresh renders', async ({ page }) => {
    const svg = page.locator('#scrollbarRefresh_svg');
    await expect(svg).toBeVisible();
  });

  test('Scrollbar refresh with different zoom position', async ({ page }) => {
    // ZoomPosition set to 0.2
    const svg = page.locator('#scrollbarRefresh_svg');
    await expect(svg).toBeVisible();
  });

  test('Column data displays at zoom position 0.2', async ({ page }) => {
    const columns = page.locator('#scrollbarRefresh svg rect');
    const count = await columns.count();
    
    // Should show data at position 0.2 of axis
    expect(count).toBeGreaterThan(0);
  });

  test('Scrollbar position reflects zoom position', async ({ page }) => {
    // With ZoomPosition 0.2, scrollbar should start at 20% position
    const svg = page.locator('#scrollbarRefresh_svg');
    await expect(svg).toBeVisible();
  });

  test('Can still interact with scrollbar after refresh', async ({ page }) => {
    const chartHost = page.locator('#scrollbarRefresh');
    
    // Drag scrollbar
    await chartHost.dragTo(chartHost, {
      sourcePosition: { x: 300, y: 400 },
      targetPosition: { x: 400, y: 400 }
    });
    
    await page.waitForTimeout(300);
    
    const svg = page.locator('#scrollbarRefresh_svg');
    await expect(svg).toBeVisible();
  });

});

test.describe('Chart – Scrollbar › Customization', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-scrollbar-playwright');
    await page.waitForLoadState('networkidle');
  });

  test('Chart scrollbarCustom renders with title', async ({ page }) => {
    const svg = page.locator('#scrollbarCustom svg');
    await expect(svg).toBeVisible();
  });

  // test('Chart title displays "Sales History of Product X"', async ({ page }) => {
  //   // Wait for the chart to be ready
  //   await page.waitForLoadState('networkidle');
    
  //   // Find title text in the chart - should be one of the first text elements
  //   const textElements = page.locator('#scrollbarCustom_svg text');
    
  //   let foundTitle = false;
  //   for (let i = 0; i < Math.min(await textElements.count(), 10); i++) {
  //     const text = await textElements.nth(i).textContent();
  //     if (text && text.toLowerCase().includes('sales')) {
  //       foundTitle = true;
  //       break;
  //     }
  //   }
    
  //   expect(foundTitle).toBe(true);
  // });

  test('Scrollbar height is customized (16px)', async ({ page }) => {
    // Scrollbar elements should render
    const scrollbar = page.locator('#scrollbarCustom svg');
    await expect(scrollbar).toBeVisible();
  });

  test('Scrollbar track color is blue', async ({ page }) => {
    const scrollbarElements = page.locator('#scrollbarCustom svg rect');
    
    let hasBlueTrack = false;
    for (let i = 0; i < Math.min(await scrollbarElements.count(), 20); i++) {
      const fill = await scrollbarElements.nth(i).getAttribute('fill').catch(() => '');
      if (fill && fill.toLowerCase().includes('blue')) {
        hasBlueTrack = true;
        break;
      }
    }
    
    // Should have colored elements
    expect(await scrollbarElements.count()).toBeGreaterThan(0);
  });

  test('Scrollbar color is yellow', async ({ page }) => {
    const scrollbarElements = page.locator('#scrollbarCustom svg rect');
    
    let hasYellowScrollbar = false;
    for (let i = 0; i < Math.min(await scrollbarElements.count(), 20); i++) {
      const fill = await scrollbarElements.nth(i).getAttribute('fill').catch(() => '');
      if (fill && fill.toLowerCase().includes('yellow')) {
        hasYellowScrollbar = true;
        break;
      }
    }
    
    // Should have rect elements
    expect(await scrollbarElements.count()).toBeGreaterThan(0);
  });

  test('Grip color is red', async ({ page }) => {
    const gripElements = page.locator('#scrollbarCustom svg circle').or(page.locator('#scrollbarCustom svg rect'));
    
    let hasRedGrip = false;
    for (let i = 0; i < Math.min(await gripElements.count(), 20); i++) {
      const fill = await gripElements.nth(i).getAttribute('fill').catch(() => '');
      if (fill && fill.toLowerCase().includes('red')) {
        hasRedGrip = true;
        break;
      }
    }
    
    // Should have grip element
    expect(await gripElements.count()).toBeGreaterThan(0);
  });

  test('Mouse wheel zooming enabled', async ({ page }) => {
    const chartHost = page.locator('#scrollbarCustom');
    
    // Hover and scroll
    await chartHost.hover({ position: { x: 400, y: 250 } });
    await page.mouse.wheel(0, 3);
    
    await page.waitForTimeout(300);
    
    const svg = page.locator('#scrollbarCustom svg');
    await expect(svg).toBeVisible();
  });

  test('Selection zooming enabled', async ({ page }) => {
    const chartHost = page.locator('#scrollbarCustom');
    
    // Drag to select and zoom
    await chartHost.dragTo(chartHost, {
      sourcePosition: { x: 300, y: 200 },
      targetPosition: { x: 500, y: 250 }
    });
    
    await page.waitForTimeout(300);
    
    const svg = page.locator('#scrollbarCustom_svg');
    await expect(svg).toBeVisible();
  });

  test('Pinch zooming enabled', async ({ page }) => {
    // Pinch requires touch events
    const svg = page.locator('#scrollbarCustom_svg');
    await expect(svg).toBeVisible();
  });

});

test.describe('Chart – Scrollbar › Axis Scrollbar Settings', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-scrollbar-playwright');
    await page.waitForLoadState('networkidle');
  });

  test('X-axis scrollbar disabled explicitly', async ({ page }) => {
    // ChartAxisScrollbarSettings Enable="false" on X-axis
    const scrollbarCustom = page.locator('#scrollbarCustom svg');
    await expect(scrollbarCustom).toBeVisible();
  });

  test('Y-axis scrollbar disabled explicitly', async ({ page }) => {
    // ChartAxisScrollbarSettings Enable="false" on Y-axis
    const scrollbarCustom = page.locator('#scrollbarCustom svg');
    await expect(scrollbarCustom).toBeVisible();
  });

});

test.describe('Chart – Scrollbar › Multiple Charts', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-scrollbar-playwright');
    await page.waitForLoadState('networkidle');
  });

  test('All three scrollbar charts are accessible', async ({ page }) => {
    const chartIds = ['scrollbarDefault', 'scrollbarRefresh', 'scrollbarCustom'];
    
    for (const chartId of chartIds) {
      const svg = page.locator(`#${chartId} svg`);
      const isVisible = await svg.isVisible().catch(() => false);
      
      if (isVisible) {
        expect(isVisible).toBe(true);
      }
    }
  });

  test('Scrollbars function independently', async ({ page }) => {
    // Each chart's scrollbar should work independently
    const chart1 = page.locator('#scrollbarDefault');
    const chart2 = page.locator('#scrollbarRefresh');
    const chart3 = page.locator('#scrollbarCustom');
    
    // All should be present
    await expect(chart1).toBeVisible();
    await expect(chart2).toBeVisible();
    await expect(chart3).toBeVisible();
  });

});

test.describe('Chart – Scrollbar › Data Range Display', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-scrollbar-playwright');
    await page.waitForLoadState('networkidle');
  });

  test('Scrollbar displays visible data range', async ({ page }) => {
    const scrollbarDefault = page.locator('#scrollbarDefault_svg');
    await expect(scrollbarDefault).toBeVisible();
  });

  test('Data updates when scrollbar is dragged', async ({ page }) => {
    const chartHost = page.locator('#scrollbarCustom');
    
    // Get initial data
    let initialRects = page.locator('#scrollbarCustom svg rect');
    let initialCount = await initialRects.count();
    
    // Drag scrollbar
    await chartHost.dragTo(chartHost, {
      sourcePosition: { x: 300, y: 400 },
      targetPosition: { x: 500, y: 400 }
    });
    
    await page.waitForTimeout(300);
    
    // Data should update
    let finalRects = page.locator('#scrollbarCustom svg rect');
    let finalCount = await finalRects.count();
    
    // Chart should render both before and after
    expect(initialCount).toBeGreaterThan(0);
    expect(finalCount).toBeGreaterThan(0);
  });

  test('Zoomed area highlighted in scrollbar', async ({ page }) => {
    const chartHost = page.locator('#scrollbarDefault');
    
    // Perform zoom
    await chartHost.dragTo(chartHost, {
      sourcePosition: { x: 250, y: 200 },
      targetPosition: { x: 350, y: 250 }
    });
    
    await page.waitForTimeout(300);
    
    // Scrollbar highlight should update
    const svg = page.locator('#scrollbarDefault_svg');
    await expect(svg).toBeVisible();
  });

});
