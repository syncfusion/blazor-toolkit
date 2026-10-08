import { test, expect } from '@playwright/test';

const pageUrl = 'http://localhost:5000/numerictextbox/accessibility';

test.describe('NumericTextBox — keyboard clear button (#82)', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto(pageUrl);
    await page.waitForLoadState('networkidle');
    await expect(page.locator('#accessNumeric')).toBeVisible();
  });

  test('Tab from the input lands on the clear button, not the document body', async ({ page }) => {
    const input = page.locator('#accessNumeric');
    await input.focus();

    const clearButton = page.locator('button.e-close');
    await expect(clearButton).toHaveAttribute('type', 'button');
    await expect(clearButton).not.toHaveAttribute('tabindex', '-1');

    await page.keyboard.press('Tab');
    await expect(clearButton).toBeVisible();

    const active = await page.evaluate(() => ({
      tag: document.activeElement?.tagName ?? '',
      className: document.activeElement?.className ?? '',
    }));
    expect(active.tag).not.toBe('BODY');
    expect(active.className).toContain('e-close');
    await expect(clearButton).toBeFocused();
  });

  test('Space on the focused clear button clears the nullable value', async ({ page }) => {
    const input = page.locator('#accessNumeric');
    await input.focus();
    await expect(input).toHaveValue(/5/);

    await page.keyboard.press('Tab');
    await expect(page.locator('button.e-close')).toBeFocused();

    await page.keyboard.press('Space');

    await expect(page.locator('#access-value')).toContainText('(empty)');
    await expect(input).toHaveValue('');
  });

  test('Tab continues to the spin buttons after the clear button', async ({ page }) => {
    await page.locator('#accessNumeric').focus();
    await page.keyboard.press('Tab');
    await expect(page.locator('button.e-close')).toBeFocused();

    await page.keyboard.press('Tab');
    await expect(page.locator('button.e-chevron-down')).toBeFocused();

    await page.keyboard.press('Tab');
    await expect(page.locator('button.e-chevron-up')).toBeFocused();

    const activeTag = await page.evaluate(() => document.activeElement?.tagName ?? '');
    expect(activeTag).not.toBe('BODY');
  });
});
