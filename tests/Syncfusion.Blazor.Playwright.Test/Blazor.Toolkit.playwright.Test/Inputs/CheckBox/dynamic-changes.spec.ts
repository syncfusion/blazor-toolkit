import { test, expect } from '@playwright/test';

test.describe('Checkbox – Dynamic Changes', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/checkbox/dynamic-changes');
    await page.waitForLoadState('networkidle');
  });

  test('Label text updates dynamically', async ({ page }) => {
    const label = page.locator('.e-label').first();
    const button = page.locator('button:has-text("Change Label")');

    await expect(label).toContainText('Original Label');

    await button.click();

    await expect(label).toContainText('Changed Label');
  });

  test('Dynamic disabled state prevents interaction', async ({ page }) => {
    const checkbox = page.locator('input[type="checkbox"]').nth(1);
    const button = page.locator('button', { hasText: /Disable|Enable/ });

    await button.click();
    await expect(checkbox).toBeDisabled();
  });

  test('CSS class updates dynamically', async ({ page }) => {
    const wrapper = page.locator('.e-checkbox-wrapper').nth(2);
    const button = page.locator('button:has-text("Make Smaller")');

    await button.click();

    await expect(wrapper).toHaveClass(/e-small/);
  });

});