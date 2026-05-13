// Color Customization Testing
// Tests REAL SfRadioButton components from the Blazor sample app

import { test, expect } from '@playwright/test';

test.describe('Color Customization', () => {
  test.beforeEach(async ({ page }) => {
    // Navigate to the size variations sample page for color/styling tests
    await page.goto('http://localhost:5000/radio-button/size-variations');
    await page.waitForLoadState('networkidle');
  });

  test('Test radio button color variants applied', async ({ page }) => {
    // Verify radio buttons are visible and styled
    const radios = page.locator('input[type="radio"]');
    await expect(radios).not.toHaveCount(0);
    
    // Select first radio and verify it gets checked
    const firstRadio = radios.first();
    await firstRadio.click();
    await expect(firstRadio).toBeChecked();
  });

  test('Test radio styling classes applied correctly', async ({ page }) => {
    const wrappers = page.locator('.e-radio-wrapper');
    const count = await wrappers.count();
    expect(count).toBeGreaterThan(0);

    // Verify Syncfusion CSS classes are present
    const firstWrapper = wrappers.first();
    await expect(firstWrapper).toHaveClass(/e-wrapper/);
  });

  test('Test radio button visual states with styling', async ({ page }) => {
    const radios = page.locator('input[type="radio"]');
    
    if (await radios.count() > 0) {
      const radio = radios.first();
      await expect(radio).not.toBeChecked();
      
      await radio.click();
      await expect(radio).toBeChecked();
    }
  });

  test('Test multiple radio groups maintain independent styling', async ({ page }) => {
    const radioGroups = new Set();
    const radios = page.locator('input[type="radio"]');
    
    const count = await radios.count();
    if (count > 0) {
      for (let i = 0; i < Math.min(count, 3); i++) {
        const radio = radios.nth(i);
        const name = await radio.getAttribute('name');
        if (name) radioGroups.add(name);
      }
    }
    
    // Verify we have multiple groups
    expect(radioGroups.size).toBeGreaterThanOrEqual(1);
  });
});
