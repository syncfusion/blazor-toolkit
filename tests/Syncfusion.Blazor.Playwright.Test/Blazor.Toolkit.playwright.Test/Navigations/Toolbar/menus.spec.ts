// P-TB-015, P-TB-051, P-TB-052, P-TB-054. Nested menus open, move focus, and honor click cancel.

import { test, expect } from '@playwright/test';
import { expectFocusedItem, openToolbarPage } from './helpers';

test.describe('Toolbar menus @toolbar', () => {
  test.beforeEach(async ({ page }) => {
    await openToolbarPage(page, '/toolbar/menus');
  });

  test('P-TB-015 split primary does not open the menu and the chevron does', async ({ page }) => {
    const primary = page.locator('#m-paste [data-testid="toolbar-split-primary"]');
    const chevron = page.locator('#m-paste [data-testid="toolbar-split-toggle"]');
    await primary.click();
    await expect(page.locator('[data-testid="toolbar-nested-popup"]')).toHaveCount(0);
    await expect(page.locator('#last-click')).toHaveText('Last click: m-paste');

    await chevron.click();
    await expect(page.locator('#m-paste [data-testid="toolbar-nested-popup"]')).toBeVisible();
    // The chevron button owns aria-expanded; the split wrapper is a plain span with no ARIA.
    await expect(chevron).toHaveAttribute('aria-expanded', 'true');
  });

  test('P-TB-051 and P-TB-054 arrow keys move inside the menu and Escape returns to the owner', async ({ page }) => {
    await page.locator('#m-file [data-testid="toolbar-menubutton"]').click();
    const menu = page.locator('#m-file [data-testid="toolbar-nested-popup"]');
    await expect(menu).toBeVisible();
    await expectFocusedItem(page, 'm-new');

    await page.keyboard.press('ArrowDown');
    await expectFocusedItem(page, 'm-open');
    await page.keyboard.press('ArrowDown');
    await expectFocusedItem(page, 'm-open');
    await page.keyboard.press('ArrowUp');
    await expectFocusedItem(page, 'm-new');
    await page.keyboard.press('ArrowUp');
    await expectFocusedItem(page, 'm-new');

    await page.keyboard.press('Escape');
    await expect(menu).toHaveCount(0);
    await expectFocusedItem(page, 'm-file');
  });

  test('P-TB-052 a cancelled click leaves the menu closed', async ({ page }) => {
    await page.locator('#m-insert [data-testid="toolbar-dropdown"]').click();
    await expect(page.locator('#m-insert [data-testid="toolbar-nested-popup"]')).toHaveCount(0);
    await expect(page.locator('#was-cancelled')).toHaveText('Cancelled: yes');
  });
});
