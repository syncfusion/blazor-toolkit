import { test, expect } from '@playwright/test';

test.describe('SfTextBox - Performance', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/textbox');
    await page.waitForLoadState('networkidle');
  });

  test('multiline resizing should not cause lag', async ({ page }) => {
    const textarea = page.locator('textarea.e-textbox').first();
    const exists = await textarea.isVisible().catch(() => false);

    if (exists) {
      await textarea.fill('Line 1\nLine 2\nLine 3');
      await page.waitForTimeout(300);

      const box = await textarea.boundingBox();
      if (box) {
        const startTime = Date.now();

        // Add many lines
        const manyLines = Array(100).fill('Test line').join('\n');
        await textarea.fill(manyLines);
        await page.waitForTimeout(500);

        const endTime = Date.now();
        const resizeTime = endTime - startTime;

        // Resizing should be reasonably fast
        expect(resizeTime).toBeLessThan(2000);
      }
    }
  });
});
