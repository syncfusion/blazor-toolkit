import { test, expect } from '@playwright/test';

test.describe('TextArea - Basic Rendering & Initialization', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/inputs/textarea');
    await page.waitForLoadState('networkidle');
    await page.locator('textarea#defaultTextArea').waitFor({ state: 'visible', timeout: 5000 });
  });

  test('default textarea renders with correct element type', async ({ page }) => {
    const textarea = page.locator('textarea#defaultTextArea');
    await expect(textarea).toBeVisible();
    
    const tagName = await textarea.evaluate((el) => el.tagName.toLowerCase());
    expect(tagName).toBe('textarea');
  });

  test('textarea with id renders correctly', async ({ page }) => {
    const textarea = page.locator('textarea#defaultTextArea');
    await expect(textarea).toBeVisible();
    
    const id = await textarea.getAttribute('id');
    expect(id).toBe('defaultTextArea');
  });

  test('textarea has aria-multiline attribute', async ({ page }) => {
    const textarea = page.locator('textarea#defaultTextArea');
    await expect(textarea).toBeVisible();
    await expect(textarea).toHaveAttribute('aria-multiline', 'true');
  });

  test('placeholder text displays correctly', async ({ page }) => {
    const textarea = page.locator('textarea#defaultTextArea');
    await expect(textarea).toBeVisible();
    
    const placeholder = await textarea.getAttribute('placeholder');
    expect(placeholder).toBeTruthy();
    expect(typeof placeholder).toBe('string');
  });

  test('textarea wrapper has required classes', async ({ page }) => {
    const textarea = page.locator('textarea#defaultTextArea');
    await expect(textarea).toBeVisible();
    
    const wrapper = textarea.locator('..');
    const classList = await wrapper.getAttribute('class');
    
    expect(classList).toBeTruthy();
    expect(classList).toContain('e-');
  });

  test('multiple textarea elements render independently', async ({ page }) => {
    const allTextareas = await page.locator('textarea').all();
    
    expect(allTextareas.length).toBeGreaterThan(0);
    
    // Verify each has unique ID or different attributes
    for (const ta of allTextareas) {
      await expect(ta).toBeVisible();
      const id = await ta.getAttribute('id');
      expect(id).toBeTruthy();
    }
  });

  test('textarea is focusable', async ({ page }) => {
    const textarea = page.locator('textarea#defaultTextArea');
    await expect(textarea).toBeVisible();
    
    await textarea.focus();
    await page.waitForTimeout(300);
    
    await expect(textarea).toBeFocused();
  });

  test('textarea accepts text input', async ({ page }) => {
    const textarea = page.locator('textarea#defaultTextArea');
    await expect(textarea).toBeVisible();
    
    // Clear any existing value first
    await textarea.clear();
    await page.waitForTimeout(300);
    
    // Use fill() instead of type() for more reliable input
    await textarea.fill('Test input');
    await page.waitForTimeout(300);
    
    await expect(textarea).toHaveValue('Test input');
  });

  test('textarea default value is empty on page load', async ({ page }) => {
    const textarea = page.locator('textarea#defaultTextArea');
    await expect(textarea).toBeVisible();
    
    const value = await textarea.inputValue();
    expect(value).toBe('');
  });

  test('textarea can be cleared after input', async ({ page }) => {
    const textarea = page.locator('textarea#defaultTextArea');
    await expect(textarea).toBeVisible();
    
    // Fill with text
    await textarea.fill('Some text');
    await page.waitForTimeout(300);
    await expect(textarea).toHaveValue('Some text');
    
    // Clear
    await textarea.clear();
    await page.waitForTimeout(300);
    await expect(textarea).toHaveValue('');
  });

  test('textarea preserves input across focus/blur cycles', async ({ page }) => {
    const textarea = page.locator('textarea#defaultTextArea');
    await expect(textarea).toBeVisible();
    
    const testText = 'Text to preserve';
    
    // Fill text
    await textarea.fill(testText);
    await page.waitForTimeout(300);
    
    // Focus and blur cycles
    for (let i = 0; i < 3; i++) {
      await textarea.focus();
      await page.waitForTimeout(200);
      await page.keyboard.press('Tab');
      await page.waitForTimeout(200);
    }
    
    // Verify text is preserved
    await expect(textarea).toHaveValue(testText);
  });

  test('textarea wrapper has proper structure', async ({ page }) => {
    const textarea = page.locator('textarea#defaultTextArea');
    await expect(textarea).toBeVisible();
    
    const wrapper = textarea.locator('..');
    const wrapperClass = await wrapper.getAttribute('class');
    
    // Check for expected wrapper classes
    expect(wrapperClass).toMatch(/e-input|e-control/);
  });

  test('textarea has correct rows and cols defaults', async ({ page }) => {
    const textarea = page.locator('textarea#defaultTextArea');
    await expect(textarea).toBeVisible();
    
    const rows = await textarea.getAttribute('rows');
    const cols = await textarea.getAttribute('cols');
    
    // At least one should be defined
    const hasRowsOrCols = rows !== null || cols !== null;
    expect(hasRowsOrCols).toBeTruthy();
  });

  test('textarea is not disabled by default', async ({ page }) => {
    const textarea = page.locator('textarea#defaultTextArea');
    await expect(textarea).toBeVisible();
    
    await expect(textarea).not.toBeDisabled();
  });

  test('textarea is not readonly by default', async ({ page }) => {
    const textarea = page.locator('textarea#defaultTextArea');
    await expect(textarea).toBeVisible();
    
    const readonly = await textarea.getAttribute('readonly');
    expect(readonly).toBeNull();
  });

});