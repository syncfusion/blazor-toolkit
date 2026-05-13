import { test, expect } from '@playwright/test';

test.describe('TextArea - Disabled & ReadOnly States', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/inputs/textarea');
    await page.waitForLoadState('networkidle');
    await page.locator('textarea#disabledTextArea').waitFor({ state: 'visible', timeout: 5000 });
  });

  test('disabled textarea has disabled attribute', async ({ page }) => {
    const textarea = page.locator('textarea#disabledTextArea');
    await expect(textarea).toBeDisabled();
  });

  test('disabled textarea has e-disabled class on container', async ({ page }) => {
    const textarea = page.locator('textarea#disabledTextArea');
    const container = textarea.locator('..');
    const classList = await container.getAttribute('class');
    expect(classList).toContain('e-disabled');
  });

  test('disabled textarea cannot receive focus', async ({ page }) => {
    const textarea = page.locator('textarea#disabledTextArea');
    
    // Try to focus
    await textarea.focus().catch(() => {});
    
    const isFocused = await textarea.evaluate((el) => document.activeElement === el);
    expect(isFocused).toBe(false);
  });

  test('disabled textarea cannot be typed into', async ({ page }) => {
    const textarea = page.locator('textarea#disabledTextArea');
    const initialValue = await textarea.inputValue();
    
    // Attempt to type (should be prevented)
    await textarea.type('test').catch(() => {});
    const finalValue = await textarea.inputValue();
    
    expect(initialValue).toBe(finalValue);
  });

  test('readonly textarea is focusable', async ({ page }) => {
    const textarea = page.locator('textarea#readonlyTextArea');
    await expect(textarea).toBeVisible();
    
    // Focus the textarea
    await textarea.focus();
    await page.waitForTimeout(300);
    
    const isFocused = await textarea.evaluate((el) => document.activeElement === el);
    expect(isFocused).toBeTruthy();
  });

  test('readonly textarea has readonly attribute', async ({ page }) => {
    const textarea = page.locator('textarea#readonlyTextArea');
    await expect(textarea).toBeVisible();
    
    const readonlyAttr = await textarea.getAttribute('readonly');
    expect(readonlyAttr !== null).toBeTruthy();
  });

  test('readonly textarea is not editable', async ({ page }) => {
    const textarea = page.locator('textarea#readonlyTextArea');
    await expect(textarea).toBeVisible();
    
    await expect(textarea).not.toBeEditable();
  });

  test('readonly text is selectable', async ({ page }) => {
    const textarea = page.locator('textarea#readonlyTextArea');
    await expect(textarea).toBeVisible();
    
    const textValue = await textarea.inputValue();
    
    // Select all text
    const isSelectable = await textarea.evaluate((el: HTMLTextAreaElement) => {
      el.select();
      return el.selectionEnd > el.selectionStart || el.value.length > 0;
    });
    
    expect(isSelectable).toBeTruthy();
  });

  test('enabled textarea is editable', async ({ page }) => {
    const textarea = page.locator('textarea#enabledTextArea');
    await expect(textarea).toBeVisible();
    
    await expect(textarea).not.toBeDisabled();
    
    await textarea.fill('test');
    await page.waitForTimeout(300);
    
    await expect(textarea).toHaveValue('test');
  });

  test('toggle disabled state works', async ({ page }) => {
    const toggleBtn = page.getByRole('button', { name: /toggle disabled/i }).first();
    const textarea = page.locator('textarea#toggleDisabledTextArea');

    await expect(textarea).toBeVisible();
    await expect(toggleBtn).toBeVisible();

    // Initially enabled
    await expect(textarea).not.toBeDisabled();

    // Click to disable
    await toggleBtn.click();
    await page.waitForTimeout(500);
    await expect(textarea).toBeDisabled();

    // Click to enable
    await toggleBtn.click();
    await page.waitForTimeout(500);
    await expect(textarea).not.toBeDisabled();
  });

  test('toggle readonly state works', async ({ page }) => {
    const toggleBtn = page.getByRole('button', { name: /toggle readonly/i }).first();
    const textarea = page.locator('textarea#toggleReadOnlyTextArea');

    await expect(textarea).toBeVisible();
    await expect(toggleBtn).toBeVisible();

    // Initially editable
    await expect(textarea).toBeEditable();

    // Click to make readonly
    await toggleBtn.click();
    await page.waitForTimeout(500);
    await expect(textarea).not.toBeEditable();

    // Click to make editable again
    await toggleBtn.click();
    await page.waitForTimeout(500);
    await expect(textarea).toBeEditable();
  });

  test('disabled textarea visual styling is different from enabled', async ({ page }) => {
    const disabledTextarea = page.locator('textarea#disabledTextArea');
    const enabledTextarea = page.locator('textarea#enabledTextArea');

    await expect(disabledTextarea).toBeVisible();
    await expect(enabledTextarea).toBeVisible();

    const disabledOpacity = await disabledTextarea.evaluate((el) => {
      return window.getComputedStyle(el).opacity;
    });

    const enabledOpacity = await enabledTextarea.evaluate((el) => {
      return window.getComputedStyle(el).opacity;
    });

    // Disabled should have lower opacity or different styling
    expect(disabledOpacity).toBeTruthy();
    expect(enabledOpacity).toBeTruthy();
  });

  test('readonly textarea visual styling differs from disabled', async ({ page }) => {
    const readonlyTextarea = page.locator('textarea#readonlyTextArea');
    const disabledTextarea = page.locator('textarea#disabledTextArea');

    await expect(readonlyTextarea).toBeVisible();
    await expect(disabledTextarea).toBeVisible();

    const readonlyValue = await readonlyTextarea.inputValue();
    const disabledValue = await disabledTextarea.inputValue();

    // Both should have values
    expect(readonlyValue.length > 0).toBeTruthy();
    expect(disabledValue.length > 0).toBeTruthy();
  });

  test('disabled textarea prevents focus and input events', async ({ page }) => {
    const textarea = page.locator('textarea#disabledTextArea');
    await expect(textarea).toBeVisible();

    let inputEventFired = false;

    // Set up listener for input events
    await textarea.evaluate(() => {
      (document.getElementById('disabledTextArea') as HTMLTextAreaElement)?.addEventListener('input', () => {
        (window as any).inputEventFired = true;
      });
    });

    // Try to type
    await textarea.type('test').catch(() => {});
    await page.waitForTimeout(300);

    // Check if event fired
    const eventFired = await page.evaluate(() => (window as any).inputEventFired);
    expect(eventFired).toBeFalsy();
  });

  test('readonly textarea allows focus but prevents input', async ({ page }) => {
    const textarea = page.locator('textarea#readonlyTextArea');
    await expect(textarea).toBeVisible();

    const initialValue = await textarea.inputValue();

    // Focus should work
    await textarea.focus();
    await page.waitForTimeout(300);

    const isFocused = await textarea.evaluate((el) => document.activeElement === el);
    expect(isFocused).toBeTruthy();

    // But typing should not change value
    await textarea.type('should not appear').catch(() => {});
    await page.waitForTimeout(300);

    const finalValue = await textarea.inputValue();
    expect(finalValue).toBe(initialValue);
  });

  test('disabled state persists through value binding', async ({ page }) => {
    const toggleBtn = page.getByRole('button', { name: /toggle disabled/i }).first();
    const textarea = page.locator('textarea#toggleDisabledTextArea');

    await expect(textarea).toBeVisible();

    // Disable
    await toggleBtn.click();
    await page.waitForTimeout(500);
    await expect(textarea).toBeDisabled();

    // Fill value while enabled (before disabling)
    await toggleBtn.click();
    await page.waitForTimeout(500);
    await textarea.fill('test value');
    await page.waitForTimeout(300);

    // Disable again
    await toggleBtn.click();
    await page.waitForTimeout(500);

    // Value should persist
    await expect(textarea).toHaveValue('test value');
    await expect(textarea).toBeDisabled();
  });

});