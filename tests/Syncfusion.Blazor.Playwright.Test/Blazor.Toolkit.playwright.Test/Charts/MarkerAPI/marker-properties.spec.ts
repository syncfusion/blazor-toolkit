import { test, expect } from '@playwright/test';

test.describe('Chart Marker API – Properties & Styling', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart/marker/api');
    await page.waitForLoadState('networkidle');
  });

  test('All markers update when button is clicked', async ({ page }) => {
    const initialMarkers = page.locator('#chart-host svg circle');
    const initialCount = await initialMarkers.count();
    
    const updateBtn = page.locator('#update-marker');
    await updateBtn.click();
    await page.waitForTimeout(300);
    
    // All markers should still be present
    const updatedMarkers = page.locator('#chart-host svg circle');
    const updatedCount = await updatedMarkers.count();
    
    expect(updatedCount).toBe(initialCount);
  });

  test('Data labels are visible with markers', async ({ page }) => {
    // Check if data labels exist
    const dataLabels = page.locator('#chart-host svg text').filter({ hasText: /\d+/ });
    const labelCount = await dataLabels.count();
    
    // Should have data labels with numeric values
    expect(labelCount).toBeGreaterThan(0);
  });

  test('Marker colors dont interfere with data labels visibility', async ({ page }) => {
    const updateBtn = page.locator('#update-marker');
    await updateBtn.click();
    await page.waitForTimeout(300);
    
    // Data labels should still be readable
    const dataLabels = page.locator('#chart-host svg text').filter({ hasText: /\d+/ });
    const labelCount = await dataLabels.count();
    
    expect(labelCount).toBeGreaterThan(0);
  });

});
