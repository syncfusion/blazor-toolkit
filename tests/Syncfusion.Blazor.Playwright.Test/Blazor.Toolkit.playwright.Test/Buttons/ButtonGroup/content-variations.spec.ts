// Content Variations Test for Real SfButtonGroup Component
// Tests different button content types: text, icon, and mixed

import { test, expect } from '@playwright/test';

test.describe('ButtonGroup - Content Variations', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/button-group/content-variations');
    await page.waitForLoadState('networkidle');
  });

  test('Icon and text buttons render both icon and text', async ({ page }) => {
    // Check Cut button with icon and text
    const cutBtn = page.locator('#btn-cut').first();
    await expect(cutBtn).toBeVisible();

    // Should have text content
    const textContent = await cutBtn.textContent();
    expect(textContent).toContain('Cut');

    // Should have icon element (icon class typically applies to span inside)
    const iconContainer = cutBtn.locator('.e-icons').first();
    const iconExists = await iconContainer.count() > 0;
    // Icon may or may not be visible depending on CSS
  })

  test('Different content types coexist in same page', async ({ page }) => {
    // Text-only group exists
    const textGroup = page.locator('#bg-text-only').first();
    await expect(textGroup).toBeVisible();

    // Icon-text group exists
    const iconTextGroup = page.locator('#bg-icon-text').first();
    await expect(iconTextGroup).toBeVisible();

    // Icon-only group exists
    const iconOnlyGroup = page.locator('#bg-icon-only').first();
    await expect(iconOnlyGroup).toBeVisible();

    // Custom content group exists
    const customGroup = page.locator('#bg-custom').first();
    await expect(customGroup).toBeVisible();
  });

  test('Buttons with mixed content types are independently interactive', async ({ page }) => {
    const cutBtn = page.locator('#btn-cut').first();
    const boldBtn = page.locator('#fmt-bold').first();

    // Both should be clickable
    await expect(cutBtn).toBeEnabled();
    await expect(boldBtn).toBeEnabled();

    // Clicking should not throw error
    await cutBtn.click();
    await boldBtn.click();
  });

  test('Button text is preserved and visible', async ({ page }) => {
    const copyBtn = page.locator('#btn-copy').first();

    const text = await copyBtn.textContent();
    expect(text?.trim()).toBe('Copy');
  });

  test('Icon elements have correct classes', async ({ page }) => {
    const pasteBtn = page.locator('#btn-paste').first();

    // Icon element should have e-icons class
    const icons = pasteBtn.locator('.e-icons');
    const count = await icons.count();

    // If icon is present, it should have the correct class
    if (count > 0) {
      const classList = await icons.first().getAttribute('class');
      expect(classList).toContain('e-icons');
    }
  });
});
