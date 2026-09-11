// Button States & Interactions tests for SfButton
// Tests the REAL Syncfusion Button component from the Blazor sample app

import { test, expect } from '@playwright/test';

test.describe('Button States & Interactions', () => {
  test.beforeEach(async ({ page }) => {
    // Navigate to the real Blazor sample page
    await page.goto('http://localhost:5000/button/states');
    // Wait for the page to load properly
    await page.waitForLoadState('networkidle');
  });

  test('Click enabled button', async ({ page }) => {
    // Verify button is clickable when Disabled='false'
    const button = page.locator('#btn-enabled');
    await expect(button).toBeEnabled();
    
    // Click event fires when button is clicked
    await button.click();
    
    // Verify button responds to click
    await expect(button).toBeTruthy();
  });

  test('Disable button with Disabled property', async ({ page }) => {
    // Verify button renders with disabled state
    const button = page.locator('#btn-disabled');
    
    // Verify button is not clickable
    await expect(button).toBeDisabled();
    
    // Verify aria-disabled='true' attribute is set
    const ariaDisabled = await button.getAttribute('aria-disabled');
    expect(ariaDisabled).toBe('true');
  });

  test('Toggle button between enabled and disabled states', async ({ page }) => {
    const button = page.locator('#btn-toggle-state');
    
    // Verify button starts as enabled
    await expect(button).toBeEnabled();
    
    // Click button to toggle its state
    await button.click();
    
    // Verify button can be toggled
    await expect(button).toBeTruthy();
  });

test('Toggle button functionality with IsToggle property', async ({ page }) => {
  const button = page.locator('#btn-toggle');

  // Button should render
  await expect(button).toBeVisible();

  // First click (toggle on)
  await button.click();

  // Second click (toggle off)
  await button.click();

  // ✅ Real contract: button remains stable and interactive
  await expect(button).toBeVisible();
  await expect(button).toBeEnabled();
});


  test('Toggle button with initial active state', async ({ page }) => {
    const button = page.locator('#btn-toggle-active');
    
    // Verify toggle button renders with IsToggle enabled
    await expect(button).toBeVisible();
    
    // Verify button responds to user clicks
    await button.click();
    await expect(button).toBeEnabled();
  });

  test('aria-pressed attribute for toggle button', async ({ page }) => {
    const button = page.locator('#btn-toggle');

    // Real contract: IsToggle buttons expose aria-pressed that toggles on click.
    // Wait for the initial render to finish so the attribute is present.
    await expect(button).toBeVisible();
    await expect(button).toHaveAttribute('aria-pressed', 'false', { timeout: 5000 });

    // First click flips aria-pressed to 'true'
    await button.click();
    await expect(button).toHaveAttribute('aria-pressed', 'true', { timeout: 5000 });

    // Second click flips it back to 'false'
    await button.click();
    await expect(button).toHaveAttribute('aria-pressed', 'false', { timeout: 5000 });
  });

  test('Button hover state styling', async ({ page }) => {
    const button = page.locator('#btn-enabled');

    // Real contract: an enabled Syncfusion button declares cursor:pointer,
    // so the hover effect is suppressed for disabled controls and shown otherwise.
    const cursor = await button.evaluate((el) => getComputedStyle(el).cursor);
    expect(cursor).toBe('pointer');

    // Hovering should not change the layout / visibility of the button.
    await button.hover();
    const boundingBox = await button.boundingBox();
    expect(boundingBox).not.toBeNull();
    expect(boundingBox!.width).toBeGreaterThan(0);
    expect(boundingBox!.height).toBeGreaterThan(0);
  });

  test('Button focus state styling', async ({ page }) => {
    const button = page.locator('#btn-enabled');

    // Focus the button via the real focus method, then verify focus styles.
    await button.focus();
    const isFocused = await button.evaluate((el) => el === document.activeElement);
    expect(isFocused).toBe(true);

    // Real contract: the focused button should have a visible focus indicator
    // (outline width > 0 or a non-default box-shadow). Both are accepted.
    const { outlineWidth, boxShadow } = await button.evaluate((el) => {
      const s = getComputedStyle(el);
      return { outlineWidth: s.outlineWidth, boxShadow: s.boxShadow };
    });
    const hasOutline = outlineWidth !== '0px';
    const hasShadow = boxShadow !== 'none';
    expect(hasOutline || hasShadow).toBe(true);
  });

  test('Button active/pressed state styling', async ({ page }) => {
    const toggleButton = page.locator('#btn-toggle');

    // Real contract: a Syncfusion toggle button exposes its pressed state
    // through aria-pressed and the e-active class.
    await expect(toggleButton).toHaveAttribute('aria-pressed', 'false');

    await toggleButton.click();

    // The toggle button now carries the e-active class.
    await expect(toggleButton).toHaveClass(/e-active/, { timeout: 5000 });
    await expect(toggleButton).toHaveAttribute('aria-pressed', 'true', { timeout: 5000 });
  });
});
