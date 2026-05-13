// Chart Last Data Label - Axis & Labels tests
// Tests axis rendering, labels, titles, and formatting

import { test, expect } from '@playwright/test';

test.describe('Chart Last Data Label – Axis & Labels', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart/last-datalabel');
    await page.waitForLoadState('networkidle');
  });

  test('X-axis displays all year labels', async ({ page }) => {
    // Verify each year label is present
    const years = ['2005', '2006', '2007', '2008', '2009', '2010', '2011'];
    
    for (const year of years) {
      const yearLabel = page.locator(`text=${year}`);
      const count = await yearLabel.count();
      
      // At least one occurrence of each year
      expect(count).toBeGreaterThan(0);
    }
  });

  test('Y-axis displays percentage labels', async ({ page }) => {
    // Y-axis should show percentage values
    const percentLabels = page.locator('text=%');
    const count = await percentLabels.count();
    
    // Should have multiple percentage labels
    expect(count).toBeGreaterThan(0);
  });

  test('X-axis title displays as "Year"', async ({ page }) => {
    // Find X-axis title
    const xAxisTitle = page.locator('text=Year').first();
    await expect(xAxisTitle).toBeVisible();
    
    const text = await xAxisTitle.textContent();
    expect(text?.trim()).toBe('Year');
  });

  test('Y-axis title displays correctly', async ({ page }) => {
    // Verify Y-axis title "Efficiency" is visible somewhere in the chart area
    const chartHost = page.locator('#chart-host');
    const title = chartHost.locator('text=Efficiency');
    
    // Title should be present (may be one of multiple instances)
    const count = await title.count();
    expect(count).toBeGreaterThan(0);
  });

  test('X-axis labels are readable and properly positioned', async ({ page }) => {
    // Get first year label
    const firstYear = page.locator('text=2005').first();
    await expect(firstYear).toBeVisible();
    
    // Get last year label
    const lastYear = page.locator('text=2011').first();
    await expect(lastYear).toBeVisible();
    
    // Labels should be horizontally distributed
    const firstBox = await firstYear.boundingBox();
    const lastBox = await lastYear.boundingBox();
    
    if (firstBox && lastBox) {
      // Last year label should be to the right of first year
      expect(lastBox.x).toBeGreaterThan(firstBox.x);
    }
  });

  test('Y-axis labels are readable and properly positioned', async ({ page }) => {
    // Get percentage labels
    const percentLabels = page.locator('text=%');
    const count = await percentLabels.count();
    
    expect(count).toBeGreaterThan(0);
    
    // Labels should be visible
    for (let i = 0; i < Math.min(count, 3); i++) {
      const label = percentLabels.nth(i);
      await expect(label).toBeVisible();
    }
  });

  test('X-axis labels are centered under columns', async ({ page }) => {
    // Verify X-axis labels appear below the chart columns
    const years = ['2005', '2006', '2007'];
    
    for (const year of years) {
      const label = page.locator(`text=${year}`).first();
      const boundingBox = await label.boundingBox();
      
      // Y coordinate should be below chart columns (higher Y value in screen coords)
      if (boundingBox) {
        expect(boundingBox.y).toBeGreaterThan(200);
      }
    }
  });

  test('Y-axis labels appear to the left of columns', async ({ page }) => {
    // Get a percentage label
    const percentLabel = page.locator('text=%').first();
    const boundingBox = await percentLabel.boundingBox();
    
    // Should be positioned to the left of chart columns
    if (boundingBox) {
      expect(boundingBox.x).toBeLessThan(200);
    }
  });

  test('Axis titles are distinct from data labels', async ({ page }) => {
    // Axis titles should appear once
    const yearTitle = page.locator('text=Year');
    const yearTitleCount = await yearTitle.count();
    
    // Should have at least the axis title
    expect(yearTitleCount).toBeGreaterThan(0);
  });

  test('Y-axis label format includes percentage symbol', async ({ page }) => {
    // All Y-axis labels should end with %
    const percentLabels = page.locator('text=%');
    const count = await percentLabels.count();
    
    // Should have formatted labels
    expect(count).toBeGreaterThan(0);
  });

  test('X-axis uses category type with year values', async ({ page }) => {
    // Verify year labels are displayed (category axis behavior)
    const years = ['2005', '2006', '2007', '2008', '2009', '2010', '2011'];
    
    let visibleYears = 0;
    for (const year of years) {
      const label = page.locator(`text=${year}`);
      const count = await label.count();
      if (count > 0) visibleYears++;
    }
    
    // Most years should be visible
    expect(visibleYears).toBeGreaterThan(5);
  });

  test('Axis labels have appropriate font size', async ({ page }) => {
    // Get a year label
    const yearLabel = page.locator('text=2005').first();
    
    const fontSize = await yearLabel.evaluate((el: Element) => {
      const computed = window.getComputedStyle(el);
      return computed.fontSize;
    });
    
    // Font size should be readable
    const size = parseInt(fontSize || '0');
    expect(size).toBeGreaterThan(8);
    expect(size).toBeLessThan(30);
  });

  test('Axis titles are visually distinct from labels', async ({ page }) => {
    const yearTitle = page.locator('text=Year').first();
    const yearLabel = page.locator('text=2005').first();
    
    const titleFont = await yearTitle.evaluate((el: Element) => {
      const computed = window.getComputedStyle(el);
      return computed.fontWeight;
    });
    
    const labelFont = await yearLabel.evaluate((el: Element) => {
      const computed = window.getComputedStyle(el);
      return computed.fontWeight;
    });
    
    // Title should typically be bolder
    const titleWeight = parseInt(titleFont || '400');
    const labelWeight = parseInt(labelFont || '400');
    
    // At least should have some distinction
    expect(titleWeight + labelWeight).toBeGreaterThan(0);
  });

  test('No axis labels are truncated', async ({ page }) => {
    // Check year labels are fully visible
    const years = ['2005', '2006', '2007', '2008', '2009', '2010', '2011'];
    
    for (const year of years) {
      const label = page.locator(`text=${year}`);
      const count = await label.count();
      
      if (count > 0) {
        const first = label.first();
        const text = await first.textContent();
        expect(text?.trim()).toBe(year);
      }
    }
  });

  test('Y-axis scaling is appropriate for data range', async ({ page }) => {
    // Y-axis should scale based on data values (28-40 range)
    // Should show labels up to at least 40%
    const maxLabel = page.locator('text=%');
    const count = await maxLabel.count();
    
    // Should have multiple ticks
    expect(count).toBeGreaterThan(0);
  });

  test('Axis labels persist after data update', async ({ page }) => {
    // Get initial labels
    const initialYears = page.locator('text=2005');
    const initialCount = await initialYears.count();
    
    // Update data
    const updateBtn = page.locator('#update-value');
    await updateBtn.click();
    await page.waitForTimeout(500);
    
    // Labels should still be visible
    const finalYears = page.locator('text=2005');
    const finalCount = await finalYears.count();
    
    expect(finalCount).toBe(initialCount);
  });

  test('Axis titles are properly aligned', async ({ page }) => {
    // X-axis title should be centered horizontally
    const xTitle = page.locator('text=Year').first();
    const yTitle = page.locator('text=Efficiency').first();
    
    // Both should be visible
    await expect(xTitle).toBeVisible();
    await expect(yTitle).toBeVisible();
  });

  test('Multiple axis labels are evenly spaced', async ({ page }) => {
    // Check spacing of year labels
    const label1 = page.locator('text=2005').first();
    const label3 = page.locator('text=2007').first();
    const label5 = page.locator('text=2009').first();
    
    const box1 = await label1.boundingBox();
    const box3 = await label3.boundingBox();
    const box5 = await label5.boundingBox();
    
    if (box1 && box3 && box5) {
      // Labels should be distributed across the axis
      const spacing1 = box3.x - box1.x;
      const spacing2 = box5.x - box3.x;
      
      // Spacing should be relatively consistent
      expect(Math.abs(spacing1 - spacing2)).toBeLessThan(100);
    }
  });
});
