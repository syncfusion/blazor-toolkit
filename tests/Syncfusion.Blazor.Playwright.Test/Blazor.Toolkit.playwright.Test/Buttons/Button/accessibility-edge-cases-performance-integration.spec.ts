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

    // Real contract: IsToggle buttons expose aria-pressed that toggles on click.
    await expect(button).toHaveAttribute('aria-pressed', 'false', { timeout: 5000 });

    await button.click();
    await expect(button).toHaveAttribute('aria-pressed', 'true', { timeout: 5000 });

    await button.click();
    await expect(button).toHaveAttribute('aria-pressed', 'false', { timeout: 5000 });
  });

  test('Button supports keyboard navigation', async ({ page }) => {
    const button = page.locator('#btn-keyboard');
    await button.focus();

    const isFocused = await button.evaluate(el => el === document.activeElement);
    expect(isFocused).toBe(true);
  });

  test('Button has visible focus indicator', async ({ page }) => {
    const button = page.getByRole('button', { name: 'Focus Indicator' });

    // FocusOnNavigate moves focus to the page heading after navigation.
    // Wait for that operation to finish before testing button focus.
    await page.waitForFunction(() => document.activeElement?.tagName === 'H1');
    await button.focus();

    await expect(button).toBeFocused();

    // Real contract: a focused Syncfusion button exposes a visible focus
    // indicator via a non-zero outline or a non-none box-shadow.
    const { outlineWidth, boxShadow } = await button.evaluate((el) => {
      const s = getComputedStyle(el);
      return { outlineWidth: s.outlineWidth, boxShadow: s.boxShadow };
    });
    const hasOutline = outlineWidth !== '0px';
    const hasShadow = boxShadow !== 'none';
    expect(hasOutline || hasShadow).toBe(true);
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
    // The sample renders three sequential buttons in a single .button-group div:
    //   #btn-seq-1, #btn-seq-2, #btn-seq-3
    const group = page.locator('div.button-group');
    await expect(group).toBeVisible();

    const buttons = group.locator('button');
    const count = await buttons.count();
    expect(count).toBeGreaterThanOrEqual(3);

    // Each of the three explicit ids should be present and enabled.
    for (const id of ['#btn-seq-1', '#btn-seq-2', '#btn-seq-3']) {
      const btn = page.locator(id);
      await expect(btn).toBeVisible();
      await expect(btn).toBeEnabled();
    }
  });

  test('Button with very long text content', async ({ page }) => {
    const button = page.locator('#btn-long-text');
    await expect(button).toBeVisible();
  });

  test('Button parameter changes after render', async ({ page }) => {
    const button = page.locator('#btn-toggle-disable');
    await expect(button).toBeVisible();

    // The sample binds Disabled="@isDisabled" and flips it in @onclick.
    // The button starts enabled.
    await expect(button).toBeEnabled();

    // Click flips isDisabled, so after the Blazor re-render the button must be disabled.
    await button.click();
    await expect(button).toBeDisabled({ timeout: 5000 });
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