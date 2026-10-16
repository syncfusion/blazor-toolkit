import { test, expect } from '@playwright/test';

const pageUrl = 'http://localhost:5000/numerictextbox/accessibility';

test.describe('NumericTextBox — keyboard clear button (#82)', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto(pageUrl);
    await page.waitForLoadState('networkidle');
    await expect(page.locator('#accessNumeric')).toBeVisible({ timeout: 10000 });
  });

  test('Tab from the input lands on the clear button, not the document body', async ({ page }) => {
    const input = page.locator('#accessNumeric');
    const clearButton = page.locator('button.e-close');
    await expect(input).toBeVisible();
    await expect(input).toHaveValue(/5/);
    await input.focus();
    await expect(clearButton).toBeVisible({ timeout: 5000 });
    await expect(clearButton).toHaveAttribute('type', 'button');
    await expect(clearButton).not.toHaveAttribute('tabindex', '-1', { timeout: 5000 });
    await page.waitForFunction(() => {
      const btn = document.querySelector('button.e-close');
      return btn && btn.getAttribute('tabindex') !== '-1';
    });
    await page.keyboard.press('Tab');
    await expect(clearButton).toBeFocused({ timeout: 5000 });
    const active = await page.evaluate(() => ({
      tag: document.activeElement?.tagName ?? '',
      className: document.activeElement?.className ?? '',
    }));
    expect(active.tag).not.toBe('BODY');
    expect(active.className).toContain('e-close');
  });

  test('Space on the focused clear button clears the nullable value', async ({ page }) => {
    const input = page.locator('#accessNumeric');
    await input.focus();
    await expect(input).toHaveValue(/5/, { timeout: 10000 });

    const clearButton = page.locator('#accessNumeric').locator('..').locator('button.e-close');
    await expect(clearButton).toBeVisible({ timeout: 10000 });

    await page.keyboard.press('Tab');
    await expect(clearButton).toBeFocused({ timeout: 10000 });

    await page.keyboard.press('Space');

    await expect(page.locator('#access-value')).toContainText('(empty)', { timeout: 10000 });
    await expect(input).toHaveValue('', { timeout: 10000 });
  });

  test('Tab continues to the spin buttons after the clear button', async ({ page }) => {
    const input = page.locator('#accessNumeric');
    await input.focus();

    const clearButton = page.locator('#accessNumeric').locator('..').locator('button.e-close');
    await expect(clearButton).toBeVisible({ timeout: 10000 });

    await page.keyboard.press('Tab');
    await expect(clearButton).toBeFocused({ timeout: 10000 });

    await page.keyboard.press('Tab');
    await expect(page.locator('button.e-chevron-down')).toBeFocused({ timeout: 10000 });

    await page.keyboard.press('Tab');
    await expect(page.locator('button.e-chevron-up')).toBeFocused({ timeout: 10000 });

    const activeTag = await page.evaluate(() => document.activeElement?.tagName ?? '');
    expect(activeTag).not.toBe('BODY');
  });
});
