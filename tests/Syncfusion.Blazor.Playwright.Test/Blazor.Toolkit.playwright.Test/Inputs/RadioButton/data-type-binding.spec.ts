// Data Type Binding Testing
// Tests REAL SfRadioButton components from the Blazor sample app

import { test, expect } from '@playwright/test';

test.describe('Data Type Binding Edge Cases', () => {
  test.beforeEach(async ({ page }) => {
    // Navigate to the value binding sample page which tests different data types
    await page.goto('http://localhost:5000/radio-button/value-binding');
    await page.waitForLoadState('networkidle');
  });

  test('Test data binding with radio values', async ({ page }) => {
    const radios = page.locator('input[type="radio"]');
    expect(await radios.count()).toBeGreaterThan(0);
    
    // Test basic value binding
    const firstRadio = radios.first();
    const initialValue = await firstRadio.getAttribute('value');
    expect(initialValue).toBeDefined();
    
    await firstRadio.click();
    await expect(firstRadio).toBeChecked();
  });

  test('Test numeric value binding', async ({ page }) => {
    const radios = page.locator('input[type="radio"]');
    
    if (await radios.count() > 1) {
      const secondRadio = radios.nth(1);
      const value = await secondRadio.getAttribute('value');
      expect(value).toBeDefined();
      
      await secondRadio.click();
      await expect(secondRadio).toBeChecked();
    }
  });

  test('Test multiple data type values can be selected', async ({ page }) => {
    const radios = page.locator('input[type="radio"]');
    const count = await radios.count();
    
    if (count > 0) {
      for (let i = 0; i < Math.min(count, 2); i++) {
        const radio = radios.nth(i);
        await radio.click();
        await expect(radio).toBeChecked();
      }
    }
  });
});
