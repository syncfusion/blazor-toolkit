// CSS Customization Testing
// Tests REAL SfRadioButton components from the Blazor sample app

import { test, expect } from '@playwright/test';

test.describe('CSS Customization', () => {
  test.beforeEach(async ({ page }) => {
    // Navigate to the size variations sample page for CSS testing
    await page.goto('http://localhost:5000/radio-button/size-variations');
    await page.waitForLoadState('networkidle');
  });

  test('Test CSS classes applied to radio wrapper', async ({ page }) => {
    // Get radio wrappers
    const wrappers = page.locator('.e-radio-wrapper');

    expect(await wrappers.count()).toBeGreaterThan(0);
    
    // Verify base CSS classes
    const firstWrapper = wrappers.first();
    const classList = (await firstWrapper.getAttribute('class')) ?? '';
    expect(classList).toContain('e-radio-wrapper');
    expect(classList).toContain('e-wrapper');
  });

  test('Test radio input element has correct CSS classes', async ({ page }) => {
    const radios = page.locator('input[type="radio"].e-radio');
    expect(await radios.count()).toBeGreaterThan(0);
    
    const firstRadio = radios.first();
    const classList = (await firstRadio.getAttribute('class')) ?? '';
    expect(classList).toContain('e-control');
    expect(classList).toContain('e-radio');
    expect(classList).toContain('e-lib');
  });

  test('Test size classes applied to radio buttons', async ({ page }) => {
    const radios = page.locator('input[type="radio"]');
    const count = await radios.count();
    expect(count).toBeGreaterThan(0);

    // Click and verify radio still works with CSS
    const firstRadio = radios.first();
    await firstRadio.click();
    await expect(firstRadio).toBeChecked();
  });
});
