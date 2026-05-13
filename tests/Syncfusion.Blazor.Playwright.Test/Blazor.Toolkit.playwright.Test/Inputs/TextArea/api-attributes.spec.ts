import { test, expect } from '@playwright/test';

test.describe('TextArea - API Attributes', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/inputs/textarea');
    await page.waitForLoadState('networkidle');
    await page.locator('textarea').first().waitFor({ state: 'visible', timeout: 10000 });
  });

  test('aria-label attribute is applied correctly', async ({ page }) => {
    const textarea = page.locator('textarea#ariaLabelTextArea');
    await expect(textarea).toBeVisible();
    const ariaLabel = await textarea.getAttribute('aria-label');
    expect(ariaLabel).toBeTruthy();
  });

  test('aria-multiline is set to true', async ({ page }) => {
    const textarea = page.locator('textarea#defaultTextArea');
    await expect(textarea).toBeVisible();
    await expect(textarea).toHaveAttribute('aria-multiline', 'true');
  });

  test('data-testid attribute is present', async ({ page }) => {
    const textarea = page.locator('textarea#dataTestIdTextArea');
    await expect(textarea).toBeVisible();
    const dataTestId = await textarea.getAttribute('data-testid');
    expect(dataTestId).toBe('textarea-dataid');
  });

  test('custom CSS class is applied', async ({ page }) => {
    const textarea = page.locator('textarea#customCssTextArea');
    await expect(textarea).toBeVisible();
    
    // Check the parent container for custom classes
    const container = textarea.locator('..');
    const containerClass = await container.getAttribute('class');
    const textareaClass = await textarea.getAttribute('class');
    
    // Either the textarea or its container should have the custom class
    const hasCustomClass = containerClass?.includes('custom-textarea-style') || 
                           textareaClass?.includes('custom-textarea-style');
    
    expect(hasCustomClass).toBeTruthy();
  });

  test('textarea with custom attributes is accessible', async ({ page }) => {
    const textarea = page.locator('textarea[data-testid="textarea-dataid"]');
    await expect(textarea).toBeVisible();
    
    await textarea.fill('test');
    await page.waitForTimeout(300);
    await expect(textarea).toHaveValue('test');
  });

  test('multiple HTML attributes can be combined', async ({ page }) => {
    const textarea = page.locator('textarea#ariaLabelTextArea');
    await expect(textarea).toBeVisible();

    const id = await textarea.getAttribute('id');
    const ariaLabel = await textarea.getAttribute('aria-label');
    const ariaMultiline = await textarea.getAttribute('aria-multiline');

    expect(id).toBe('ariaLabelTextArea');
    expect(ariaLabel).toBeTruthy();
    expect(ariaMultiline).toBe('true');
  });

  test('accessibility attributes do not impact functionality', async ({ page }) => {
    const textarea = page.locator('textarea#ariaLabelTextArea');
    await expect(textarea).toBeVisible();

    await textarea.fill('accessibility test');
    await page.waitForTimeout(300);
    await expect(textarea).toHaveValue('accessibility test');

    await textarea.focus();
    await page.waitForTimeout(200);
    await expect(textarea).toBeFocused();
  });

});