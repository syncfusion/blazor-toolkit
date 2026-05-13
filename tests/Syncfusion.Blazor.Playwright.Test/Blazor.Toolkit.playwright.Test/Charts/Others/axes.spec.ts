import { test, expect } from '@playwright/test';

const DEFAULT_TIMEOUT = 20000;

test.describe('Chart - Axes', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart/axes');
    // samples under axes.razor are slow to render; allow a longer idle timeout
    await page.waitForLoadState('networkidle');
    // wait for charts to appear (there are multiple charts on the page)
    const charts = page.locator('.test-container .e-chart');
    await expect(charts).toHaveCount(6, { timeout: DEFAULT_TIMEOUT });
  });

  test('renders all axes charts and svgs', async ({ page }) => {
    const charts = page.locator('.test-container .e-chart');
    await expect(charts).toHaveCount(6, { timeout: DEFAULT_TIMEOUT });

    const svgs = page.locator('svg#container_svg');
    // each chart renders an svg; ensure visibility for each
    const count = await svgs.count();
    for (let i = 0; i < count; i++) {
      await expect(svgs.nth(i)).toBeVisible({ timeout: DEFAULT_TIMEOUT });
      // axis line(s) should exist inside the svg (SVG primitives may be reported hidden during render/animation)
      const axisLine = svgs.nth(i).locator('path[id^="containerAxisLine_"]');
      await expect(axisLine.first()).toBeAttached();
      // major tick and grid lines should be present (attach-check is more reliable for SVG paths)
      await expect(svgs.nth(i).locator('path[id^="container_MajorTickLine_"]').first()).toBeAttached();
      await expect(svgs.nth(i).locator('path[id^="container_MajorGridLine_"]').first()).toBeAttached();
      // axis labels (text elements) should exist
      const texts = await svgs.nth(i).locator('text').allTextContents();
      const hasNumber = texts.some(t => /\d/.test(t));
      expect(hasNumber).toBeTruthy();
    }
  });

  test('page contains charts with expected axis label sets (0 and 2 present)', async ({ page }) => {
    const svgs = page.locator('svg#container_svg');
    const count = await svgs.count();
    let foundZero = false;
    let foundTwo = false;
    for (let i = 0; i < count; i++) {
      const texts = await svgs.nth(i).locator('text').allTextContents();
      if (texts.some(t => t.trim() === '0')) foundZero = true;
      if (texts.some(t => t.trim() === '2')) foundTwo = true;
    }
    // At least one chart should show axis labels starting from 0 (default)
    expect(foundZero).toBeTruthy();
    // At least one chart uses higher axis start (e.g. HighValue) and shows '2'
    expect(foundTwo).toBeTruthy();
  });

  test('AxisLine styles applied for AxisLine sample', async ({ page }) => {
    // look for the styled axis line (stroke color and dasharray set in sample)
    // horizontal axis style
    const styledH = page.locator('path[id^="containerAxisLine_"][stroke="#FBAF4F"]');
    await expect(styledH.first()).toBeAttached({ timeout: DEFAULT_TIMEOUT });
    const dashH = await styledH.first().getAttribute('stroke-dasharray');
    expect(dashH).toBe('5,1');

    // vertical axis style
    const styledV = page.locator('path[id^="containerAxisLine_"][stroke="#0D97D4"]');
    await expect(styledV.first()).toBeAttached({ timeout: DEFAULT_TIMEOUT });
    const dashV = await styledV.first().getAttribute('stroke-dasharray');
    expect(dashV).toBe('3,2');
  });
});
