import { test, expect } from '@playwright/test';

test.describe('Checkbox – Size Variations', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/checkbox/size-variations');
    await page.waitForLoadState('networkidle');
  });

  test('Small checkbox applies e-small class', async ({ page }) => {
    const wrapper = page.locator('.e-checkbox-wrapper').first();
    await expect(wrapper).toHaveClass(/e-small/);
  });

  test('Large checkbox applies e-bigger class', async ({ page }) => {
    const wrapper = page.locator('.e-checkbox-wrapper').nth(2);
    await expect(wrapper).toHaveClass(/e-bigger/);
  });

});