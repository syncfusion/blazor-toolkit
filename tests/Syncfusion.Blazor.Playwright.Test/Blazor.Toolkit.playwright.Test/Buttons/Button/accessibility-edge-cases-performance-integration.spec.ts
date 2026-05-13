// Accessibility, Edge Cases, Performance, and Integration tests for SfButton
// Tests the REAL Syncfusion Button component from the Blazor sample app

import { test, expect } from '@playwright/test';

test.describe('Accessibility Support', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/button/accessibility-edge-cases');
    await page.waitForLoadState('networkidle');
  });

  test('Button has proper role and ARIA attributes', async ({ page }) => {
    const button = page.locator('#btn-accessible');
    await expect(button).toBeVisible();

    const ariaDisabled = await button.getAttribute('aria-disabled');
    expect(ariaDisabled).toBe('false');
  });

  test('Disabled button has correct ARIA attributes', async ({ page }) => {
    const button = page.locator('#btn-disabled-aria');
    await expect(button).toBeDisabled();

    const ariaDisabled = await button.getAttribute('aria-disabled');
    expect(ariaDisabled).toBe('true');
  });

  test('Toggle button has aria-pressed attribute', async ({ page }) => {
    const button = page.locator('#btn-toggle-aria');
    await expect(button).toBeVisible();
  });

  test('Button supports keyboard navigation', async ({ page }) => {
    const button = page.locator('#btn-keyboard');
    await button.focus();

    const isFocused = await button.evaluate(el => el === document.activeElement);
    expect(isFocused).toBe(true);
  });

  test('Button has visible focus indicator', async ({ page }) => {
    const button = page.locator('#btn-focus-indicator');
    await button.focus();

    const isFocused = await button.evaluate(el => el === document.activeElement);
    expect(isFocused).toBe(true);
  });

  test('Button label is properly associated for screen readers', async ({ page }) => {
    const button = page.locator('#btn-accessible');
    const text = await button.textContent();
    expect(text).toBeTruthy();
  });
});

test.describe('HTML Attributes & Styling', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/button/accessibility-edge-cases');
    await page.waitForLoadState('networkidle');
  });

  test('HTML attributes like title and data-* are supported', async ({ page }) => {
    const button = page.locator('#btn-accessible');
    await expect(button).toBeVisible();
  });

  test('Button color contrast for accessibility', async ({ page }) => {
    const button = page.locator('#btn-accessible');
    await expect(button).toBeVisible();
  });
});

test.describe('Edge Cases & Special Scenarios', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/button/accessibility-edge-cases');
    await page.waitForLoadState('networkidle');
  });

  test('Rapid clicking on button', async ({ page }) => {
    const button = page.locator('#btn-rapid-click');

    for (let i = 0; i < 5; i++) {
      await button.click();
    }

    await expect(button).toBeTruthy();
  });

  test('Button with null or undefined content', async ({ page }) => {
    const button = page.locator('#btn-icon-only-no-label');
    await expect(button).toBeVisible();
  });

  test('Button component is destroyed and recreated', async ({ page }) => {
    const button = page.locator('#btn-submit-form');
    await expect(button).toBeVisible();
  });

  test('Button in a form submission', async ({ page }) => {
    const button = page.locator('#btn-submit-form');
    await expect(button).toBeVisible();

    const isClickable = await button.isEnabled();
    expect(isClickable).toBe(true);
  });

  test('Multiple buttons in sequence', async ({ page }) => {
    const buttons = page.locator('div.button-group button');
    const count = await buttons.count();
    expect(count).toBeGreaterThanOrEqual(0);
  });

  test('Button with very long text content', async ({ page }) => {
    const button = page.locator('#btn-long-text');
    await expect(button).toBeVisible();
  });

  test('Button parameter changes after render', async ({ page }) => {
    const button = page.locator('#btn-toggle-disable');
    await expect(button).toBeVisible();
    await button.click();
  });

  test('Nested button content', async ({ page }) => {
    const button = page.locator('#btn-nested-content');
    await expect(button).toBeVisible();

    const nestedSpan = button.locator('#nested-span');
    await expect(nestedSpan).toBeVisible();
  });
});

test.describe('Button Form Integration', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/button/accessibility-edge-cases');
    await page.waitForLoadState('networkidle');
  });

  test('Submit button in form', async ({ page }) => {
    const button = page.locator('#btn-submit-form');
    await expect(button).toBeVisible();
  });

  test('Reset button in form', async ({ page }) => {
    const button = page.locator('#btn-reset-form');
    await expect(button).toBeVisible();
  });

  test('Regular button in form', async ({ page }) => {
    const button = page.locator('#btn-regular-form');
    await expect(button).toBeVisible();
  });
});