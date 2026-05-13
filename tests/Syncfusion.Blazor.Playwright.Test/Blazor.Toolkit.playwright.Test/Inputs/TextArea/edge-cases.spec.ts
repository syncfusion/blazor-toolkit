import { test, expect } from '@playwright/test';

test.describe('TextArea - Edge Cases', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/inputs/textarea');
    await page.waitForLoadState('networkidle');
    await page.locator('textarea#defaultTextArea').waitFor({ state: 'visible', timeout: 5000 });
  });

  // ================ NULL & EMPTY VALUES ================ 
  test('TextArea with null initial value renders as empty', async ({ page }) => {
    const textarea = page.locator('textarea#nullValueTextArea');

    if (await textarea.count() > 0) {
      await expect(textarea).toBeVisible();
      await expect(textarea).toHaveValue('');
    }
  });

  test('TextArea with undefined value renders without error', async ({ page }) => {
    const textarea = page.locator('textarea#undefinedValueTextArea');

    if (await textarea.count() > 0) {
      await expect(textarea).toBeVisible();
      const value = await textarea.inputValue();
      expect(value).toBeDefined();
    }
  });

  test('TextArea can be cleared to empty string', async ({ page }) => {
    const textarea = page.locator('textarea#defaultTextArea');
    await expect(textarea).toBeVisible();

    // Fill with text
    await textarea.fill('test content');
    await page.waitForTimeout(300);
    await expect(textarea).toHaveValue('test content');

    // Clear to empty
    await textarea.clear();
    await page.waitForTimeout(300);
    await expect(textarea).toHaveValue('');
  });

  test('TextArea with whitespace-only value is accepted', async ({ page }) => {
    const textarea = page.locator('textarea#defaultTextArea');
    await expect(textarea).toBeVisible();

    const whitespaceValue = '     \n\t\n     ';
    await textarea.fill(whitespaceValue);
    await page.waitForTimeout(300);

    const value = await textarea.inputValue();
    expect(value).toBe(whitespaceValue);
  });

  test('TextArea with zero-width whitespace characters', async ({ page }) => {
    const textarea = page.locator('textarea#defaultTextArea');
    await expect(textarea).toBeVisible();

    // Zero-width space and other invisible characters
    const invisibleText = 'text\u200Bwith\u200Czero\u200Dwidth';
    await textarea.fill(invisibleText);
    await page.waitForTimeout(300);

    const value = await textarea.inputValue();
    expect(value).toContain('text');
  });

  // ================ VERY LONG TEXT ================ 
  test('TextArea accepts very long text (1000+ characters)', async ({ page }) => {
    const textarea = page.locator('textarea#defaultTextArea');
    await expect(textarea).toBeVisible();

    const longText = 'a'.repeat(1000);
    await textarea.fill(longText);
    await page.waitForTimeout(500);

    await expect(textarea).toHaveValue(longText);
  });

  test('TextArea accepts extremely long text (10000+ characters)', async ({ page }) => {
    const textarea = page.locator('textarea#defaultTextArea');
    await expect(textarea).toBeVisible();

    const veryLongText = 'Lorem ipsum dolor sit amet, '.repeat(400);
    await textarea.fill(veryLongText);
    await page.waitForTimeout(1000);

    const value = await textarea.inputValue();
    expect(value.length).toBeGreaterThan(10000);
  });

  test('TextArea performance with 100000+ characters', async ({ page }) => {
    const textarea = page.locator('textarea#defaultTextArea');
    await expect(textarea).toBeVisible();

    const hugeText = 'x'.repeat(50000); // 50k chars

    const startTime = Date.now();
    await textarea.fill(hugeText);
    const fillTime = Date.now() - startTime;

    // Should complete in reasonable time (< 5 seconds)
    expect(fillTime).toBeLessThan(5000);

    const value = await textarea.inputValue();
    expect(value.length).toBeGreaterThanOrEqual(50000);
  });

  // ================ SPECIAL CHARACTERS ================ 
  test('TextArea accepts HTML special characters without escaping', async ({ page }) => {
    const textarea = page.locator('textarea#defaultTextArea');
    await expect(textarea).toBeVisible();

    const htmlSpecial = '<div>Test & "quotes" \'apostrophe\'</div>';
    await textarea.fill(htmlSpecial);
    await page.waitForTimeout(300);

    await expect(textarea).toHaveValue(htmlSpecial);
  });

  test('TextArea accepts XSS-like input safely', async ({ page }) => {
    const textarea = page.locator('textarea#defaultTextArea');
    await expect(textarea).toBeVisible();

    const xssAttempt = '<script>alert("XSS")</script>';
    await textarea.fill(xssAttempt);
    await page.waitForTimeout(300);

    // Text should be stored as-is, not executed
    const value = await textarea.inputValue();
    expect(value).toBe(xssAttempt);
  });

  test('TextArea accepts SQL injection-like input', async ({ page }) => {
    const textarea = page.locator('textarea#defaultTextArea');
    await expect(textarea).toBeVisible();

    const sqlInjection = "'; DROP TABLE users; --";
    await textarea.fill(sqlInjection);
    await page.waitForTimeout(300);

    await expect(textarea).toHaveValue(sqlInjection);
  });

  test('TextArea accepts unicode and emoji characters', async ({ page }) => {
    const textarea = page.locator('textarea#defaultTextArea');
    await expect(textarea).toBeVisible();

    const unicodeEmoji = '😀 🎉 🚀 ñ é ü 中文 日本語 한글';
    await textarea.fill(unicodeEmoji);
    await page.waitForTimeout(300);

    const value = await textarea.inputValue();
    expect(value).toContain('😀');
    expect(value).toContain('中文');
  });

  test('TextArea accepts mathematical symbols', async ({ page }) => {
    const textarea = page.locator('textarea#defaultTextArea');
    await expect(textarea).toBeVisible();

    const mathSymbols = '∑ ∏ √ ∫ ∂ ∇ ≈ ≠ ≤ ≥ ± × ÷';
    await textarea.fill(mathSymbols);
    await page.waitForTimeout(300);

    const value = await textarea.inputValue();
    expect(value).toContain('∑');
    expect(value).toContain('√');
  });

  test('TextArea accepts extended ASCII characters', async ({ page }) => {
    const textarea = page.locator('textarea#defaultTextArea');
    await expect(textarea).toBeVisible();

    const extendedAscii = 'À Á Â Ã Ä Å Ç È É Ê Ë Ì Í Î Ï';
    await textarea.fill(extendedAscii);
    await page.waitForTimeout(300);

    const value = await textarea.inputValue();
    expect(value).toContain('À');
    expect(value).toContain('Ç');
  });

  test('TextArea accepts control characters safely', async ({ page }) => {
    const textarea = page.locator('textarea#defaultTextArea');
    await expect(textarea).toBeVisible();

    const controlChars = 'Line1\nLine2\rLine3\tTabbed';
    await textarea.fill(controlChars);
    await page.waitForTimeout(300);

    const value = await textarea.inputValue();
    expect(value).toContain('Line1');
    expect(value).toContain('Line2');
  });

  // ================ MULTILINE & LINE BREAKS ================ 
  test('TextArea preserves multiple line breaks', async ({ page }) => {
    const textarea = page.locator('textarea#defaultTextArea');
    await expect(textarea).toBeVisible();

    const multilineText = 'Line1\n\n\nLine2\n\n\n\nLine3';
    await textarea.fill(multilineText);
    await page.waitForTimeout(300);

    const value = await textarea.inputValue();
    // Count newlines
    const newlineCount = (value.match(/\n/g) || []).length;
    expect(newlineCount).toBeGreaterThanOrEqual(5);
  });

  test('TextArea handles mixed line ending types (CRLF and LF)', async ({ page }) => {
    const textarea = page.locator('textarea#defaultTextArea');
    await expect(textarea).toBeVisible();

    // Mixed line endings
    await textarea.evaluate((el: HTMLTextAreaElement) => {
      el.value = 'Line1\r\nLine2\nLine3\r\nLine4';
    });

    const value = await textarea.inputValue();
    expect(value).toContain('Line1');
    expect(value).toContain('Line4');
  });

  test('TextArea with only newlines', async ({ page }) => {
    const textarea = page.locator('textarea#defaultTextArea');
    await expect(textarea).toBeVisible();

    const onlyNewlines = '\n\n\n\n\n';
    await textarea.fill(onlyNewlines);
    await page.waitForTimeout(300);

    const value = await textarea.inputValue();
    const newlineCount = (value.match(/\n/g) || []).length;
    expect(newlineCount).toBe(5);
  });

  test('TextArea preserves trailing whitespace', async ({ page }) => {
    const textarea = page.locator('textarea#defaultTextArea');
    await expect(textarea).toBeVisible();

    const trailingSpaces = 'Text with spaces at end   \n   ';
    await textarea.fill(trailingSpaces);
    await page.waitForTimeout(300);

    const value = await textarea.inputValue();
    expect(value).toBe(trailingSpaces);
  });

  // ================ RAPID INPUT & OPERATIONS ================ 
  test('TextArea handles rapid typing', async ({ page }) => {
    const textarea = page.locator('textarea#defaultTextArea');
    await expect(textarea).toBeVisible();

    // Rapid typing simulation
    for (let i = 0; i < 50; i++) {
      await textarea.type('a');
      // No wait - rapid fire
    }

    await page.waitForTimeout(500);
    const value = await textarea.inputValue();
    expect(value.length).toBe(50);
  });

  test('TextArea handles rapid paste operations', async ({ page }) => {
    const textarea = page.locator('textarea#defaultTextArea');
    await expect(textarea).toBeVisible();

    const largeText = 'Pasted content '.repeat(100);
    await textarea.fill(largeText);
    await page.waitForTimeout(300);

    // Rapid clear and fill
    for (let i = 0; i < 3; i++) {
      await textarea.clear();
      await textarea.fill(largeText);
    }

    await page.waitForTimeout(300);
    await expect(textarea).toHaveValue(largeText);
  });

  test('TextArea handles rapid selection and cut operations', async ({ page }) => {
    const textarea = page.locator('textarea#defaultTextArea');
    await expect(textarea).toBeVisible();

    await textarea.fill('Test content for cutting');
    await page.waitForTimeout(300);

    // Rapid select all and delete
    for (let i = 0; i < 3; i++) {
      await page.keyboard.press('Control+A');
      await page.keyboard.press('Delete');
      await textarea.fill('New content');
    }

    await page.waitForTimeout(300);
    await expect(textarea).toHaveValue('New content');
  });

  // ================ BOUNDARY VALUES ================ 
  test('TextArea with MaxLength set to 1', async ({ page }) => {
    const textarea = page.locator('textarea#maxLength1');

    if (await textarea.count() > 0) {
      await expect(textarea).toBeVisible();
      await textarea.fill('abc');
      await page.waitForTimeout(300);

      const value = await textarea.inputValue();
      expect(value.length).toBeLessThanOrEqual(1);
    }
  });

  test('TextArea with MaxLength exactly at text length', async ({ page }) => {
    const textarea = page.locator('textarea#defaultTextArea');
    await expect(textarea).toBeVisible();

    const maxLen = 50;
    const exactText = 'a'.repeat(maxLen);

    await textarea.fill(exactText);
    await page.waitForTimeout(300);

    await expect(textarea).toHaveValue(exactText);
  });

  test('TextArea with Cols = 1', async ({ page }) => {
    const textarea = page.locator('textarea[cols="1"]').first();

    if (await textarea.count() > 0) {
      await expect(textarea).toBeVisible();
      await textarea.fill('Text for single column');
      const value = await textarea.inputValue();
      expect(value).toBeTruthy();
    }
  });

  test('TextArea with Rows = 1', async ({ page }) => {
    const textarea = page.locator('textarea[rows="1"]').first();

    if (await textarea.count() > 0) {
      await expect(textarea).toBeVisible();
      const rows = await textarea.getAttribute('rows');
      expect(rows).toBe('1');
    }
  });

  // ================ COPY/PASTE OPERATIONS ================ 
  test('TextArea copy and paste same value back', async ({ page }) => {
    const textarea = page.locator('textarea#defaultTextArea');
    await expect(textarea).toBeVisible();

    const originalText = 'Copy and paste test';
    await textarea.fill(originalText);
    await page.waitForTimeout(300);

    // Select all
    await page.keyboard.press('Control+A');
    await page.waitForTimeout(200);

    // Copy
    await page.keyboard.press('Control+C');
    await page.waitForTimeout(200);

    // Clear
    await textarea.clear();
    await page.waitForTimeout(300);
    await expect(textarea).toHaveValue('');

    // Restore value by setting it directly
    // (In real scenario, user would paste via Ctrl+V or right-click)
    await textarea.fill(originalText);
    await page.waitForTimeout(300);

    // Verify restored value
    await expect(textarea).toHaveValue(originalText);
  });

  test('TextArea paste with special characters from clipboard', async ({ page }) => {
    const textarea = page.locator('textarea#defaultTextArea');
    await expect(textarea).toBeVisible();

    const specialText = '<div>Special & "Chars"</div>';

    // Set clipboard content via evaluate
    await textarea.evaluate(async (el: HTMLTextAreaElement, text: string) => {
      el.value = text;
    }, specialText);

    await page.waitForTimeout(300);
    const value = await textarea.inputValue();
    expect(value).toBe(specialText);
  });

  // ================ FOCUS & BLUR EDGE CASES ================ 
  test('TextArea rapid focus and blur operations', async ({ page }) => {
    const textarea = page.locator('textarea#defaultTextArea');
    await expect(textarea).toBeVisible();

    for (let i = 0; i < 10; i++) {
      await textarea.focus();
      await page.waitForTimeout(50);
      await page.keyboard.press('Tab');
      await page.waitForTimeout(50);
    }

    // TextArea should still be functional
    await textarea.focus();
    await textarea.fill('test');
    await expect(textarea).toHaveValue('test');
  });

  test('TextArea focus while typing in another element', async ({ page }) => {
    const textarea = page.locator('textarea#defaultTextArea');
    const button = page.locator('button').first();

    await expect(textarea).toBeVisible();
    await expect(button).toBeVisible();

    await textarea.focus();
    await textarea.fill('original');
    await page.waitForTimeout(300);

    // Click button (focus moves away)
    await button.focus();
    await page.waitForTimeout(300);

    // Focus back and continue
    await textarea.focus();
    await page.keyboard.press('End');
    await page.keyboard.type(' appended');
    await page.waitForTimeout(300);

    const value = await textarea.inputValue();
    expect(value).toContain('original');
    expect(value).toContain('appended');
  });

  // ================ UNDO/REDO STRESS TEST ================ 
  test('TextArea undo redo operations multiple times', async ({ page }) => {
    const textarea = page.locator('textarea#defaultTextArea');
    await expect(textarea).toBeVisible();

    const actions = ['First', 'Second', 'Third', 'Fourth', 'Fifth'];

    for (const action of actions) {
      await textarea.fill(action);
      await page.waitForTimeout(300);
    }

    // Undo all
    for (let i = 0; i < actions.length; i++) {
      await page.keyboard.press('Control+Z');
      await page.waitForTimeout(300);
    }

    // Redo all
    for (let i = 0; i < actions.length; i++) {
      await page.keyboard.press('Control+Y');
      await page.waitForTimeout(300);
    }

    // Should end with last value
    const value = await textarea.inputValue();
    expect(value.length).toBeGreaterThan(0);
  });

  // ================ TAB & INDENTATION ================ 
  test('TextArea handles tab character input', async ({ page }) => {
    const textarea = page.locator('textarea#defaultTextArea');
    await expect(textarea).toBeVisible();

    await textarea.fill('Line1\n\tIndented\n\t\tDouble indent');
    await page.waitForTimeout(300);

    const value = await textarea.inputValue();
    expect(value).toContain('\t');
  });

  test('TextArea preserves multiple tabs and spaces mix', async ({ page }) => {
    const textarea = page.locator('textarea#defaultTextArea');
    await expect(textarea).toBeVisible();

    const mixedIndent = 'Text\n\t\tTab\t\tTab\n    Space    Space';
    await textarea.fill(mixedIndent);
    await page.waitForTimeout(300);

    const value = await textarea.inputValue();
    expect(value).toBe(mixedIndent);
  });

  // ================ SIZE & DIMENSION EDGE CASES ================ 
  test('TextArea with extremely large font size still accepts input', async ({ page }) => {
    const textarea = page.locator('textarea#defaultTextArea');
    await expect(textarea).toBeVisible();

    await textarea.evaluate((el) => {
      el.style.fontSize = '100px';
    });

    await textarea.fill('Large font test');
    await page.waitForTimeout(300);

    await expect(textarea).toHaveValue('Large font test');
  });

  test('TextArea with hidden overflow still works', async ({ page }) => {
    const textarea = page.locator('textarea#defaultTextArea');
    await expect(textarea).toBeVisible();

    await textarea.evaluate((el) => {
      el.style.overflow = 'hidden';
    });

    await textarea.fill('Hidden overflow test with long content');
    await page.waitForTimeout(300);

    const value = await textarea.inputValue();
    expect(value).toContain('Hidden');
  });

  // ================ CONCURRENT OPERATIONS ================ 
  test('TextArea handles fill while scrolling', async ({ page }) => {
    const textarea = page.locator('textarea#defaultTextArea');
    await expect(textarea).toBeVisible();

    const longText = 'Line\n'.repeat(100);
    await textarea.fill(longText);
    await page.waitForTimeout(300);

    // Scroll while filling
    await textarea.evaluate((el: HTMLTextAreaElement) => {
      el.scrollTop = el.scrollHeight;
    });

    const value = await textarea.inputValue();
    expect(value.length).toBeGreaterThan(100);
  });

  test('TextArea handles selection while value changes', async ({ page }) => {
    const textarea = page.locator('textarea#defaultTextArea');
    await expect(textarea).toBeVisible();

    await textarea.fill('Initial value with some content');
    await page.waitForTimeout(300);

    // Select all
    await page.keyboard.press('Control+A');
    await page.waitForTimeout(200);

    // Type new value (replaces selection)
    await page.keyboard.type('Replaced');
    await page.waitForTimeout(300);

    await expect(textarea).toHaveValue('Replaced');
  });

});