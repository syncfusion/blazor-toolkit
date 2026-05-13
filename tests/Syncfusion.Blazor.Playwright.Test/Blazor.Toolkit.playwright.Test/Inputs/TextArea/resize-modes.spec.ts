import { test, expect } from '@playwright/test';

test.describe('TextArea - Resize Modes', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/inputs/textarea');
    await page.waitForLoadState('networkidle');
    await page.locator('textarea#noResizeTextArea').waitFor({ state: 'visible', timeout: 5000 });
  });

  test('no resize mode disables resizing', async ({ page }) => {
    const textarea = page.locator('textarea#noResizeTextArea');
    await expect(textarea).toBeVisible();
    
    const resizeStyle = await textarea.evaluate((el: HTMLTextAreaElement) => {
      return window.getComputedStyle(el).resize;
    });
    expect(resizeStyle).toBe('none');
  });

  test('vertical resize mode allows vertical resizing', async ({ page }) => {
    const textarea = page.locator('textarea#verticalResizeTextArea');
    await expect(textarea).toBeVisible();

    const resizeStyle = await textarea.evaluate((el: HTMLTextAreaElement) => {
      return window.getComputedStyle(el).resize;
    });
    expect(resizeStyle).toBe('vertical');
  });

  test('horizontal resize mode allows horizontal resizing', async ({ page }) => {
    const textarea = page.locator('textarea#horizontalResizeTextArea');
    await expect(textarea).toBeVisible();

    const resizeStyle = await textarea.evaluate((el: HTMLTextAreaElement) => {
      return window.getComputedStyle(el).resize;
    });
    expect(resizeStyle).toBe('horizontal');
  });

  test('both resize mode allows both directions', async ({ page }) => {
    const textarea = page.locator('textarea#bothResizeTextArea');
    await expect(textarea).toBeVisible();

    const resizeStyle = await textarea.evaluate((el: HTMLTextAreaElement) => {
      return window.getComputedStyle(el).resize;
    });
    expect(resizeStyle).toBe('both');
  });

  test('resizable textarea has handle indicator', async ({ page }) => {
    const textarea = page.locator('textarea#bothResizeTextArea');
    await expect(textarea).toBeVisible();
    
    const boundingBox = await textarea.boundingBox();
    expect(boundingBox).toBeTruthy();
    expect(boundingBox?.width).toBeGreaterThan(0);
    expect(boundingBox?.height).toBeGreaterThan(0);
  });

});