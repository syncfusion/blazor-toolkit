// P-TB-024, P-TB-025. Radio groups keep one selection; check groups allow several.

import { test, expect } from '@playwright/test';
import { openToolbarPage } from './helpers';

test.describe('Toolbar groups @toolbar', () => {
  test.beforeEach(async ({ page }) => {
    await openToolbarPage(page, '/toolbar/groups');
  });

  test('P-TB-024 a radio group keeps exactly one pressed item', async ({ page }) => {
    await expect(page.locator('#g-left button')).toHaveAttribute('aria-pressed', 'true');
    await page.locator('#g-center button').click();
    await expect(page.locator('#g-center button')).toHaveAttribute('aria-pressed', 'true');
    await expect(page.locator('#g-left button')).toHaveAttribute('aria-pressed', 'false');
    await expect(page.locator('#g-right button')).toHaveAttribute('aria-pressed', 'false');
  });

  test('P-TB-025 a check group allows several pressed items', async ({ page }) => {
    await page.locator('#g-bold button').click();
    await page.locator('#g-italic button').click();
    await expect(page.locator('#g-bold button')).toHaveAttribute('aria-pressed', 'true');
    await expect(page.locator('#g-italic button')).toHaveAttribute('aria-pressed', 'true');
    await expect(page.locator('#g-underline button')).toHaveAttribute('aria-pressed', 'false');
  });
});
