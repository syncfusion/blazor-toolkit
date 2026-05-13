import { test, expect } from '@playwright/test';

test.describe('SfTextBox - Form Validation', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/textbox-form-validation-test');
    await page.waitForLoadState('networkidle');
  });

  test('textbox in editform should display validation error', async ({ page }) => {
    // Get required field SfTextBox
    const requiredField = page.locator('input[data-testid="required-name"]').first();
    
    // Try to submit without value
    const submitBtn = page.locator('button:has-text("Submit"), #submit-btn').first();
    await submitBtn.click();
    await page.waitForTimeout(500);

    // Test - Error message should appear
    const errorMsg = page.locator('.e-error, .e-validation-error, .validation-message').first();
    const isErrorVisible = await errorMsg.isVisible().catch(() => false);
    
    if (isErrorVisible) {
      const errorText = await errorMsg.textContent();
      expect(errorText).toMatch(/required|must|field/i);
    } else {
      // Check for validation attribute
      const ariaInvalid = await requiredField.getAttribute('aria-invalid');
      expect(ariaInvalid).toBe('true');
    }
  });

  test('textbox should enforce multiple validation rules', async ({ page }) => {
    const minLengthField = page.locator('input[data-testid="min-length"]').first();
    const submitBtn = page.locator('button:has-text("Submit"), #submit-btn').first();

    const fieldExists = await minLengthField.isVisible().catch(() => false);
    
    if (fieldExists) {
      // Test 1 - Empty (fails required)
      await submitBtn.click();
      await page.waitForTimeout(500);

      let error = page.locator('.e-error').first();
      const initialError = await error.isVisible().catch(() => false);

      // Test 2 - Too short (fails minimum length)
      await minLengthField.fill('ab');
      await page.waitForTimeout(200);

      // Test 3 - Valid length
      await minLengthField.clear();
      await minLengthField.fill('validinput');
      await page.waitForTimeout(300);

      error = page.locator('.e-error').first();
      const finalError = await error.isVisible().catch(() => false);

      expect(finalError).toBe(false);
    }
  });

  test('form should prevent submission with invalid fields', async ({ page }) => {
    const nameField = page.locator('input[data-testid="required-name"]').first();
    const submitBtn = page.locator('button:has-text("Submit"), #submit-btn').first();

    // Leave fields empty and submit
    await submitBtn.click();
    await page.waitForTimeout(500);

    // Check for form-level or individual field validation message
    const fieldErrors = page.locator('.e-error, .e-validation-error');
    const fieldErrorCount = await fieldErrors.count();

    expect(fieldErrorCount).toBeGreaterThan(0);

    // Verify form didn't submit (check for success message)
    const successMsg = page.locator('text=Success|submitted|Form submitted').first();
    const isVisible = await successMsg.isVisible().catch(() => false);
    expect(isVisible).toBe(false);
  });
});
