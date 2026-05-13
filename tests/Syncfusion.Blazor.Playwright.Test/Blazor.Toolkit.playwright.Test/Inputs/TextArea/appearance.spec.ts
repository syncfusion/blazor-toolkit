import { test, expect } from '@playwright/test';

test.describe('TextArea - Appearance Variants', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/inputs/textarea');
    await page.waitForLoadState('networkidle');
    await page.locator('textarea#dynamicAppearanceTextArea').waitFor({ state: 'visible', timeout: 5000 });
  });

  test('outline appearance renders with correct class', async ({ page }) => {
    const textarea = page.locator('textarea#dynamicAppearanceTextArea');
    await expect(textarea).toBeVisible();
    
    // Check the parent container for outline appearance styling
    const container = textarea.locator('..');
    const borderStyle = await container.evaluate((el) => {
      const computed = window.getComputedStyle(el);
      return computed.borderStyle;
    });
    
    expect(borderStyle).not.toBe('none');
  });

  test('filled appearance renders with correct class', async ({ page }) => {
    const textarea = page.locator('textarea#dynamicAppearanceTextArea');
    const filledBtn = page.getByRole('button', { name: /filled/i }).first();

    await expect(textarea).toBeVisible();
    await expect(filledBtn).toBeVisible();
    
    await filledBtn.click();
    await page.waitForTimeout(800);

    // Wait for appearance to change and check background color
    const container = textarea.locator('..');
    const bgColor = await container.evaluate((el) => {
      const computed = window.getComputedStyle(el);
      return computed.backgroundColor;
    });
    
    expect(bgColor).not.toBe('rgba(0, 0, 0, 0)');
    expect(bgColor).not.toBe('transparent');
  });

  test('appearance can be switched dynamically to filled', async ({ page }) => {
    const filledBtn = page.getByRole('button', { name: /filled/i }).first();
    const textarea = page.locator('textarea#dynamicAppearanceTextArea');

    await expect(textarea).toBeVisible();
    await expect(filledBtn).toBeVisible();
    
    await filledBtn.click();
    await page.waitForTimeout(800);

    // Verify current appearance text shows "Filled"
    const appearanceDisplay = page.locator('div:has-text("Current Appearance:")').first();
    const displayText = await appearanceDisplay.textContent();
    
    expect(displayText).toContain('Filled');
  });

  test('appearance can be switched dynamically to outline', async ({ page }) => {
    const outlineBtn = page.getByRole('button', { name: /outline/i }).first();
    const filledBtn = page.getByRole('button', { name: /filled/i }).first();
    const textarea = page.locator('textarea#dynamicAppearanceTextArea');

    await expect(textarea).toBeVisible();
    await expect(filledBtn).toBeVisible();
    await expect(outlineBtn).toBeVisible();

    // First switch to filled
    await filledBtn.click();
    await page.waitForTimeout(800);

    // Then switch back to outline
    await outlineBtn.click();
    await page.waitForTimeout(800);

    // Verify current appearance text shows "Outline"
    const appearanceDisplay = page.locator('div:has-text("Current Appearance:")').first();
    const displayText = await appearanceDisplay.textContent();
    
    expect(displayText).toContain('Outline');
  });

  test('appearance does not affect functionality', async ({ page }) => {
    const textarea = page.locator('textarea#dynamicAppearanceTextArea');
    const filledBtn = page.getByRole('button', { name: /filled/i }).first();
    const outlineBtn = page.getByRole('button', { name: /outline/i }).first();

    await expect(textarea).toBeVisible();
    await expect(filledBtn).toBeVisible();
    await expect(outlineBtn).toBeVisible();

    // Test with outline appearance (default)
    await textarea.fill('outline test');
    await page.waitForTimeout(300);
    await expect(textarea).toHaveValue('outline test');

    // Switch to filled
    await filledBtn.click();
    await page.waitForTimeout(800);

    // Clear and test with filled appearance
    await textarea.clear();
    await textarea.fill('filled test');
    await page.waitForTimeout(300);
    await expect(textarea).toHaveValue('filled test');

    // Switch back to outline
    await outlineBtn.click();
    await page.waitForTimeout(800);

    // Clear and test with outline appearance again
    await textarea.clear();
    await textarea.fill('back to outline');
    await page.waitForTimeout(300);
    await expect(textarea).toHaveValue('back to outline');
  });

});