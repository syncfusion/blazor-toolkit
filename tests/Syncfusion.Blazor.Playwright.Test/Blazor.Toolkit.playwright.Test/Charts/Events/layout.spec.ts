// Chart Events & Keyboard - Layout tests
// Tests the REAL Syncfusion Chart component layout from the Blazor sample app

import { test, expect } from '@playwright/test';

test.describe('Chart Events & Keyboard › Layout', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-events-keyboard-playwright');
    await page.waitForLoadState('networkidle');
  });

  test('Second chart SVG stays within container bounds', async ({ page }) => {
    const hostBox = await page.locator('#chartKeyboard').boundingBox();
    const svgBox = await page.locator('#chartKeyboard svg').boundingBox();

    expect(svgBox!.width).toBeLessThanOrEqual(hostBox!.width);
    expect(svgBox!.height).toBeLessThanOrEqual(hostBox!.height);
  });

  test('Both charts have proper spacing between them', async ({ page }) => {
    const chart1 = page.locator('#chartEvents');
    const chart2 = page.locator('#chartKeyboard');

    const box1 = await chart1.boundingBox();
    const box2 = await chart2.boundingBox();

    // Verify charts are stacked vertically with gap
    if (box1 && box2) {
      expect(box2.y).toBeGreaterThan(box1.y + box1.height);
    }
  });

  test('Charts are properly aligned', async ({ page }) => {
    const chart1 = page.locator('#chartEvents');
    const chart2 = page.locator('#chartKeyboard');

    const box1 = await chart1.boundingBox();
    const box2 = await chart2.boundingBox();

    // Both charts should have same width
    expect(box1?.width).toBe(box2?.width);
  });
});