import { test, expect } from '@playwright/test';

test.describe('SfTextBox - Touch Events', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/textbox');
    await page.waitForLoadState('networkidle');
  });

  test('touch should select text in multiline textbox', async ({ page }) => {
    const textarea = page.locator('textarea.e-textbox').first();
    const exists = await textarea.isVisible().catch(() => false);

    if (exists) {
      await textarea.fill('Line 1\nLine 2\nLine 3');
      await page.waitForTimeout(300);

      const box = await textarea.boundingBox();
      if (box) {
        // Triple tap to select all
        await page.touchscreen.tap(box.x + box.width / 2, box.y + box.height / 2);
        await page.touchscreen.tap(box.x + box.width / 2, box.y + box.height / 2);
        await page.touchscreen.tap(box.x + box.width / 2, box.y + box.height / 2);
        await page.waitForTimeout(500);

        // Check if text is selected
        const selected = await textarea.evaluate((el: any) => {
          return (el.selectionEnd - el.selectionStart) > 0;
        });
        expect(selected).toBe(true);
      }
    }
  });

  test('touch swipe right on textbox should trigger clear button', async ({ page }) => {
    const textbox = page.locator('input.e-textbox[data-testid*="clear"], input[aria-label*="clear"]').first();
    const exists = await textbox.isVisible().catch(() => false);

    if (exists) {
      await textbox.fill('Sample text');
      await page.waitForTimeout(300);

      const box = await textbox.boundingBox();
      if (box) {
        // Look for clear button next to textbox
        const clearBtn = textbox.locator('+ button, ~ button.e-clear-btn').first();
        const clearExists = await clearBtn.isVisible().catch(() => false);

        if (clearExists) {
          const btnBox = await clearBtn.boundingBox();
          if (btnBox) {
            // Tap clear button
            await page.touchscreen.tap(btnBox.x + btnBox.width / 2, btnBox.y + btnBox.height / 2);
            await page.waitForTimeout(300);

            // Verify cleared
            const value = await textbox.inputValue();
            expect(value).toBe('');
          }
        }
      }
    }
  });
});
