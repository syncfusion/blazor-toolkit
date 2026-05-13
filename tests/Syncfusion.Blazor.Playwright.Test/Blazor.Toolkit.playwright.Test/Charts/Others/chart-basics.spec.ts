import { test, expect } from '@playwright/test';

const DEFAULT_TIMEOUT = 20000;

test.describe('Chart - Basic Rendering Suite', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart/chart-basics');
    await page.waitForLoadState('networkidle');
    // page contains multiple charts; ensure expected 4 charts rendered
    const charts = page.locator('.test-container .e-chart');
    await expect(charts).toHaveCount(4, { timeout: DEFAULT_TIMEOUT });
  });

  test('basic chart renders title, series and legend', async ({ page }) => {
    const chart = page.locator('#Chart');
    await expect(chart).toBeVisible({ timeout: DEFAULT_TIMEOUT });

    // svg and title
    const svg = chart.locator('svg#Chart_svg');
    await expect(svg).toBeVisible();
    const title = svg.locator('#Chart_ChartTitle');
    await expect(title).toHaveText('Month wise Sales Report');

    // series group & first data point visible
    await expect(svg.locator('g#ChartSeriesGroup0')).toBeVisible();
    await expect(svg.locator('#Chart_Series_0_Point_0')).toBeVisible();

    // legend text
    await expect(svg.locator('text#Chart_chart_legend_text_0')).toHaveText('Gold');
  });

  test('title sample shows styled title and subtitle and chart area styling', async ({ page }) => {
    const titleChart = page.locator('div[aria-label="container"]').filter({ has: page.locator('text=Olympic Medals') }).first();
    await expect(titleChart).toBeVisible({ timeout: DEFAULT_TIMEOUT });
    const svg = titleChart.locator('svg#container_svg');
    await expect(svg.locator('#container_ChartTitle')).toHaveText('Olympic Medals');
    await expect(svg.locator('#container_ChartSubTitle')).toHaveText('Medals');

    // Chart area styling (background, stroke, opacity)
    const area = svg.locator('rect#container_ChartAreaBorder');
    await expect(area).toBeVisible();
    const fill = await area.getAttribute('fill');
    expect(fill).toBe('skyblue');
    const stroke = await area.getAttribute('stroke');
    expect(stroke).toBe('gray');
    const opacity = await area.getAttribute('opacity');
    expect(opacity).toBe('0.8');
  });

  test('properties & methods sample: rtl, background image, striplines and multiple series', async ({ page }) => {
    // identify chart by its aria-label
    const propsChart = page.locator('div[aria-label="Medal details with line Chart"]').first();
    await expect(propsChart).toBeVisible({ timeout: DEFAULT_TIMEOUT });


    const svg = propsChart.locator('svg#container_svg');
    // background image present
    const bg = svg.locator('image#container_ChartBackground');
    await expect(bg).toBeVisible();
    const href = await bg.getAttribute('href');
    expect(href).toContain('Images/cloud.png');

    // striplines (behind rects) should exist (two entries expected)
    const striplineRects = svg.locator('rect[id^="container_stripline_Behind_rect_"]');
    await expect(striplineRects).toHaveCount(2);
    await expect(striplineRects.first()).toBeVisible();
    await expect(striplineRects.nth(1)).toBeVisible();
    // verify their fill colors (order: first -> red, second -> blue)
    await expect(striplineRects.first()).toHaveAttribute('fill', 'red');
    await expect(striplineRects.nth(1)).toHaveAttribute('fill', 'blue');

    // multiple series (line, spline, column) present
    await expect(svg.locator('g#containerSeriesGroup0')).toBeVisible();
    await expect(svg.locator('g#containerSeriesGroup1')).toBeVisible();
    await expect(svg.locator('g#containerSeriesGroup2')).toBeVisible();
  });

  test('dynamic area sample uses specified fill, border and dasharray', async ({ page }) => {
    const areaChart = page.locator('text', { hasText: 'Sales Report over the year 2025' }).first().locator('xpath=..').locator('xpath=..');
    // fallback: find svg by chart title text
    const svg = page.locator('svg#container_svg').filter({ has: page.locator('text=Sales Report over the year 2025') }).first();
    await expect(svg).toBeVisible({ timeout: DEFAULT_TIMEOUT });

    // series path (area) should have dasharray and fill
    const series = svg.locator('path#container_Series_0');
    await expect(series).toBeVisible();
    const dash = await series.getAttribute('stroke-dasharray');
    expect(dash).toBe('3, 2');
    const fill = await series.getAttribute('fill');
    expect(fill).toBe('blue');

    // border path should have stroke color and width
    const border = svg.locator('path#container_Series_0_Border');
    await expect(border).toBeVisible();
    expect(await border.getAttribute('stroke')).toBe('red');
    expect(await border.getAttribute('stroke-width')).toBe('5');
  });
});
