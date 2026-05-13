import { test, expect } from '@playwright/test';

test.describe('TextArea - Value Binding', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/inputs/textarea');
    await page.waitForLoadState('networkidle');
    await page.locator('textarea#programmaticTextArea').waitFor({ state: 'visible', timeout: 5000 });
  });

  test('textarea value can be set programmatically', async ({ page }) => {
    const setBtn = page.getByRole('button', { name: /set value/i }).first();
    const textarea = page.locator('textarea#programmaticTextArea');

    await expect(setBtn).toBeVisible();
    await setBtn.click();
    await page.waitForTimeout(500);

    const value = await textarea.inputValue();
    expect(value).toContain('Updated value from code');
  });

  test('textarea value can be cleared programmatically', async ({ page }) => {
    const textarea = page.locator('textarea#programmaticTextArea');
    const setBtn = page.getByRole('button', { name: /set value/i }).first();
    const clearBtn = page.getByRole('button', { name: /clear value/i }).first();

    await expect(setBtn).toBeVisible();
    await setBtn.click();
    await page.waitForTimeout(300);
    await expect(textarea).not.toHaveValue('');

    await expect(clearBtn).toBeVisible();
    await clearBtn.click();
    await page.waitForTimeout(300);
    await expect(textarea).toHaveValue('');
  });

  test('initial value is displayed on page load', async ({ page }) => {
    const textarea = page.locator('textarea#initialValueTextArea');
    await expect(textarea).toBeVisible();
    const value = await textarea.inputValue();
    expect(value).toBe('Syncfusion TextArea Component');
  });

  test('empty textarea starts with empty string', async ({ page }) => {
    const textarea = page.locator('textarea#emptyValueTextArea');
    await expect(textarea).toBeVisible();
    await expect(textarea).toHaveValue('');
  });

  test('two-way binding updates textarea value', async ({ page }) => {
    const textarea = page.locator('textarea#programmaticTextArea');

    await expect(textarea).toBeVisible();
    await textarea.fill('manual input');
    await textarea.blur();
    await page.waitForTimeout(300);

    const display = page.locator('div:has-text("Value")').first();
    if (await display.count() > 0) {
      const text = await display.textContent();
      expect(text).toContain('manual input');
    }
  });

  test('value binding persists across focus/blur', async ({ page }) => {
    const textarea = page.locator('textarea#programmaticTextArea');

    await expect(textarea).toBeVisible();
    await textarea.fill('test value');
    await textarea.focus();
    await page.waitForTimeout(200);
    await textarea.blur();
    await page.waitForTimeout(300);

    await expect(textarea).toHaveValue('test value');
  });

  test('multiline value binding works correctly', async ({ page }) => {
    const textarea = page.locator('textarea#programmaticTextArea');

    await expect(textarea).toBeVisible();
    await textarea.fill('line 1\nline 2\nline 3');
    await textarea.blur();
    await page.waitForTimeout(300);

    const value = await textarea.inputValue();
    expect(value).toContain('line 1');
    expect(value).toContain('line 2');
    expect(value).toContain('line 3');
  });

  test('special characters in value binding', async ({ page }) => {
    const textarea = page.locator('textarea#programmaticTextArea');

    await expect(textarea).toBeVisible();
    const specialChars = '!@#$%^&*()_+-=[]{}|;:\'",.<>?/';
    await textarea.fill(specialChars);
    await textarea.blur();
    await page.waitForTimeout(300);

    const value = await textarea.inputValue();
    expect(value).toBe(specialChars);
  });

});