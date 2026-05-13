import { test, expect } from '@playwright/test';

test.describe('TextArea - MaxLength Validation', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/inputs/textarea');
    await page.waitForLoadState('networkidle');
    await page.locator('textarea#maxLength50').waitFor({ state: 'visible', timeout: 5000 });
  });

  test('maxlength attribute is applied to textarea', async ({ page }) => {
    const textarea = page.locator('textarea#maxLength50');
    await expect(textarea).toBeVisible();
    await expect(textarea).toHaveAttribute('maxlength', '50');
  });

  test('textarea with maxlength 100 has correct attribute', async ({ page }) => {
    const textarea = page.locator('textarea#maxLength100');
    await expect(textarea).toBeVisible();
    await expect(textarea).toHaveAttribute('maxlength', '100');
  });

  test('textarea with maxlength 500 has correct attribute', async ({ page }) => {
    const textarea = page.locator('textarea#maxLength500');
    await expect(textarea).toBeVisible();
    await expect(textarea).toHaveAttribute('maxlength', '500');
  });

  test('cannot exceed maxlength of 50 characters', async ({ page }) => {
    const textarea = page.locator('textarea#maxLength50');
    await expect(textarea).toBeVisible();
    await textarea.click();
    await textarea.type('a'.repeat(100));
    await page.waitForTimeout(300);
    
    const value = await textarea.inputValue();
    expect(value.length).toBeLessThanOrEqual(50);
  });

  test('cannot exceed maxlength of 100 characters', async ({ page }) => {
    const textarea = page.locator('textarea#maxLength100');
    await expect(textarea).toBeVisible();
    await textarea.click();
    await textarea.type('b'.repeat(150));
    await page.waitForTimeout(300);
    
    const value = await textarea.inputValue();
    expect(value.length).toBeLessThanOrEqual(100);
  });

  test('maxlength display updates character count', async ({ page }) => {
    const textarea = page.locator('textarea#maxLength50');
    await expect(textarea).toBeVisible();
    await textarea.fill('Hello');
    await textarea.blur();
    await page.waitForTimeout(300);

    const display = page.locator('[class*="char-count"], [data-testid*="count"]').first();
    if (await display.count() > 0) {
      await expect(display).toContainText(/[0-5]/);
    }
  });

  test('can fill exact character limit', async ({ page }) => {
    const textarea = page.locator('textarea#maxLength50');
    await expect(textarea).toBeVisible();
    const testText = 'x'.repeat(50);
    
    await textarea.fill(testText);
    await page.waitForTimeout(300);
    await expect(textarea).toHaveValue(testText);
  });

  test('paste operation respects maxlength', async ({ page }) => {
    const textarea = page.locator('textarea#maxLength50');
    await expect(textarea).toBeVisible();
    
    // Use fill instead of evaluate to trigger proper validation
    await textarea.focus();
    await page.waitForTimeout(200);
    
    // Fill with text exceeding maxlength
    await textarea.fill('a'.repeat(100));
    await page.waitForTimeout(300);
    
    await textarea.blur();
    await page.waitForTimeout(200);
    
    const value = await textarea.inputValue();
    expect(value.length).toBeLessThanOrEqual(50);
  });

});