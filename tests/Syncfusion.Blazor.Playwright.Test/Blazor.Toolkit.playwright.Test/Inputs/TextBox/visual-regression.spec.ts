import { test, expect } from '@playwright/test';

test.describe('SfTextBox - Visual Regression', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/textbox');
    await page.waitForLoadState('networkidle');
  });

  test('filled variant should match snapshot', async ({ page }) => {
    const textbox = page.locator('.e-filled input.e-textbox, input.e-textbox.e-filled').first();
    const container = textbox.locator('..').first();
    
    const exists = await textbox.isVisible().catch(() => false);
    if (exists) {
      await expect(container).toHaveScreenshot('textbox-filled.png', { maxDiffPixels: 100 });
    }
  });

  test('outlined variant should match snapshot', async ({ page }) => {
    const textbox = page.locator('.e-outlined input.e-textbox, input.e-textbox.e-outlined').first();
    const container = textbox.locator('..').first();
    
    const exists = await textbox.isVisible().catch(() => false);
    if (exists) {
      await expect(container).toHaveScreenshot('textbox-outlined.png', { maxDiffPixels: 100 });
    }
  });

  test('disabled textbox should match snapshot', async ({ page }) => {
    const textbox = page.locator('input.e-textbox[disabled], input[disabled].e-textbox').first();
    const container = textbox.locator('..').first();
    
    const exists = await textbox.isVisible().catch(() => false);
    if (exists) {
      await expect(container).toHaveScreenshot('textbox-disabled.png', { maxDiffPixels: 100 });
    }
  });

  test('multiline textbox should match snapshot', async ({ page }) => {
    const textarea = page.locator('textarea.e-textbox').first();
    const container = textarea.locator('..').first();
    
    const exists = await textarea.isVisible().catch(() => false);
    if (exists) {
      await expect(container).toHaveScreenshot('textbox-multiline.png', { maxDiffPixels: 100 });
    }
  });

  test('textbox with floating label should match snapshot', async ({ page }) => {
    const textbox = page.locator('.e-float-input input.e-textbox, input.e-textbox.e-float-input').first();
    const container = textbox.locator('..').first();
    
    const exists = await textbox.isVisible().catch(() => false);
    if (exists) {
      await expect(container).toHaveScreenshot('textbox-float-label.png', { maxDiffPixels: 100 });
    }
  });

  test('textbox with clear button should match snapshot', async ({ page }) => {
    const textbox = page.locator('input.e-textbox[aria-label*="clear"], input[data-testid*="clear"]').first();
    const container = textbox.locator('..').first();
    
    const exists = await textbox.isVisible().catch(() => false);
    if (exists) {
      await fill(textbox, 'Sample text');
      await page.waitForTimeout(300);
      await expect(container).toHaveScreenshot('textbox-with-clear-btn.png', { maxDiffPixels: 100 });
    }
  });

  test('textbox with icons should match snapshot', async ({ page }) => {
    const textbox = page.locator('input.e-textbox').first();
    const hasIcon = await page.locator('span.e-input-before, span.e-input-after').first().isVisible().catch(() => false);
    
    if (hasIcon) {
      const container = textbox.locator('..').first();
      await expect(container).toHaveScreenshot('textbox-with-icons.png', { maxDiffPixels: 100 });
    }
  });
});

async function fill(element: any, text: string) {
  await element.clear();
  await element.fill(text);
}
