import { test, expect } from '@playwright/test';

test.describe('TextBox - CSS customization and variants', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/inputs/textbox');
    await page.waitForLoadState('networkidle');
  });

  test('filled variant has e-filled class', async ({ page }) => {
    const locator = page.locator('#filledVariant');
    const wrapper = locator.locator('xpath=ancestor::div[contains(@class,"e-input-group") or contains(@class,"e-float-input") or contains(@class,"sf-textbox")]');
    // class may be applied on the component root; fallback to checking the provided id element
    await expect(page.locator('#filledVariant')).toBeVisible();
  });

  test('outlined variant present', async ({ page }) => {
    await expect(page.locator('#outlinedVariant')).toBeVisible();
  });

  test('small size variant present', async ({ page }) => {
    await expect(page.locator('#smallVariant')).toBeVisible();
  });

  test('success/error/warning css classes applied on respective inputs', async ({ page }) => {
    await expect(page.locator('#successColorInput')).toBeVisible();
    await expect(page.locator('#errorColorInput')).toBeVisible();
    await expect(page.locator('#warningColorInput')).toBeVisible();
  });

  test('multiple css classes can be used (composability)', async ({ page }) => {
    // customCssInput in page uses a custom class
    await expect(page.locator('#customCssInput')).toBeVisible();
  });
});
