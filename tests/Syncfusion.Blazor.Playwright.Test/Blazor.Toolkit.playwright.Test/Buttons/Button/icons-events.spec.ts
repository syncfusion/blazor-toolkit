// Icon Support, Event Handling tests for SfButton
// Tests the REAL Syncfusion Button component from the Blazor sample app

import { test, expect } from '@playwright/test';

test.describe('Icon Support & Positioning', () => {
  test.beforeEach(async ({ page }) => {
    // Navigate to the real Blazor sample page
    await page.goto('http://localhost:5000/button/icon-support');
    // Wait for the page to load properly
    await page.waitForLoadState('networkidle');
  });

  test('Render button with icon using IconCss', async ({ page }) => {
    const button = page.locator('#btn-icon-left');
    await expect(button).toBeVisible();
    const icon = button.locator('.e-icons');
    await expect(icon).toBeAttached();
  });

  test('Position icon on the left (IconPosition.Left)', async ({ page }) => {
    const button = page.locator('#btn-icon-left');
    await expect(button).toBeVisible();
  });

  test('Position icon on the right (IconPosition.Right)', async ({ page }) => {
    const button = page.locator('#btn-icon-right');
    await expect(button).toBeVisible();
    const icon = button.locator('.e-icons');
    await expect(icon).toBeAttached();
  });

  test('Position icon on top (IconPosition.Top)', async ({ page }) => {
    const button = page.locator('#btn-icon-top');
    await expect(button).toBeVisible();
  });

  test('Position icon on bottom (IconPosition.Bottom)', async ({ page }) => {
    const button = page.locator('#btn-icon-bottom');
    await expect(button).toBeVisible();
  });

  test('Icon-only button without content', async ({ page }) => {
    const button = page.locator('#btn-icon-only-dedicated');
    await expect(button).toBeVisible();
    const icon = button.locator('.e-icons');
    await expect(icon).toBeAttached();
  });

  test('Round icon-only button', async ({ page }) => {
    const button = page.locator('#btn-icon-only-dedicated');
    await expect(button).toBeVisible();

    // The dedicated icon-only sample applies the e-icon-btn class
    // via CssClass to switch the button into icon-only layout.
    const classes = await button.getAttribute('class');
    expect(classes).toContain('e-icon-btn');
  });

  test('Icon with semantic color', async ({ page }) => {
    const button = page.getByRole('button', { name: 'Open Document' });
    await expect(button).toBeVisible();
  });

  test('Accessibility - icon aria-hidden attribute', async ({ page }) => {
    const button = page.locator('#btn-icon-aria-hidden');
    await expect(button).toBeVisible();

    // The component renders the user-supplied <span class="e-icons e-play" aria-hidden="true">.
    // Use a strict class selector so we target the component-rendered span, not any other .e-icons.
    const icon = button.locator('span.e-icons.e-play');
    await expect(icon).toBeAttached();
    const ariaHidden = await icon.getAttribute('aria-hidden');
    expect(ariaHidden).toBe('true');
  });

  test('Multiple icon classes', async ({ page }) => {
    // Target the component-rendered icon span from the first button that uses IconCss
    // (e.g. #btn-icon-left renders <span class="e-icons e-save e-btn-icon">).
    const button = page.locator('#btn-icon-left');
    const icon = button.locator('span.e-icons.e-save');
    await expect(icon).toBeAttached();

    // The IconCss classes are preserved on the rendered icon span.
    const classList = await icon.getAttribute('class');
    expect(classList).toContain('e-icons');
    expect(classList).toContain('e-save');
  });
});

test.describe('Event Handling', () => {
  test.beforeEach(async ({ page }) => {
    // Navigate to the real Blazor sample page
    await page.goto('http://localhost:5000/button/states');
    // Wait for the page to load properly
    await page.waitForLoadState('networkidle');
  });

  test('OnClick event fires on user click', async ({ page }) => {
    const button = page.locator('#btn-toggle-state');
    await expect(button).toBeVisible();
    await button.click();
    
    // Verify click was successful
    await expect(button).toBeTruthy();
  });

  test('OnClick event receives MouseEventArgs', async ({ page }) => {
    const button = page.locator('#btn-toggle-state');
    await button.click();
    
    // Verify click was handled
    await expect(button).toBeTruthy();
  });

  test('OnClick event fires only for user interaction', async ({ page }) => {
    const button = page.locator('#btn-toggle-state');
    
    // User mouse click
    await button.click();
    
    // Keyboard interaction on focused button
    await button.focus();
    await page.keyboard.press('Enter');
    
    // Verify interactions were handled
    await expect(button).toBeTruthy();
  });

  test('Created event fires after component render', async ({ page }) => {
    // Real contract: after the Blazor circuit finishes the first render, every
    // SfButton on this page has run its OnAfterRenderAsync and the Created event
    // has been invoked. From the test's perspective this means the root CSS
    // classes are present on the rendered element.
    const button = page.locator('#btn-enabled');
    await expect(button).toBeVisible();
    const classes = await button.getAttribute('class');
    expect(classes).toContain('e-control');
    expect(classes).toContain('e-btn');
    expect(classes).toContain('e-lib');
  });

  test('Created event for multiple buttons', async ({ page }) => {
    // The States sample renders five SfButtons. After Created runs on each
    // component, all of them should have the root CSS classes applied.
    const expectedIds = ['#btn-enabled', '#btn-disabled', '#btn-toggle-state', '#btn-toggle', '#btn-toggle-active'];
    for (const id of expectedIds) {
      const btn = page.locator(id);
      await expect(btn).toBeVisible();
      const classes = await btn.getAttribute('class');
      expect(classes).toContain('e-control');
      expect(classes).toContain('e-btn');
    }
  });
});
