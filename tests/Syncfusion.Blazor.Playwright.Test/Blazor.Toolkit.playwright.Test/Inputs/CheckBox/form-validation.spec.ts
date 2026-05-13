import { test, expect } from '@playwright/test';

test.describe('Checkbox – Form Validation', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/checkbox/form-validation');
    await page.waitForLoadState('networkidle');
  });

  test('Form submission blocked when checkbox not checked', async ({ page }) => {
    const firstForm = page.locator('form').first();
    const submit = firstForm.locator('button[type="submit"]');
    const validation = firstForm.locator('.validation-message');

    await submit.click();

    await expect(validation).toHaveText(
      'You must accept the terms and conditions'
    );
  });

  test('Submit button enabled only after checkbox is checked', async ({ page }) => {
    const checkbox = page.locator('#enable-submit-cb');
    const submit = page.locator('#submit-btn-2');

    await expect(submit).toBeDisabled();

    await checkbox.check();

    await expect(submit).toBeEnabled();
  });

});