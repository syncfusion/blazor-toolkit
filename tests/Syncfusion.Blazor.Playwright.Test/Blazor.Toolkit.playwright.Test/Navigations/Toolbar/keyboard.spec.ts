// Toolbar keyboard tests: single tab stop, roving tabindex, arrows, Home and End, orientation and RTL.

import { test, expect } from '@playwright/test';
import { openToolbarPage, expectFocusedItem } from './helpers';

const TB = '#toolbar-keys';


test.describe('Toolbar keyboard @toolbar', () => {
  test('P-TB-003 Tab enters the toolbar once and Shift+Tab leaves it', async ({ page }) => {
    await openToolbarPage(page, '/toolbar/keyboard');
    await page.locator('#before-toolbar').focus();
    await page.keyboard.press('Tab');
    await expectFocusedItem(page, 'k-one');
    await page.keyboard.press('Tab');
    await expect(page.locator('#after-toolbar')).toBeFocused();
    await page.keyboard.press('Shift+Tab');
    await expectFocusedItem(page, 'k-one');
    await page.keyboard.press('Shift+Tab');
    await expect(page.locator('#before-toolbar')).toBeFocused();
  });

  test('P-TB-004 ArrowRight and ArrowLeft move between enabled items without wrapping', async ({ page }) => {
    await openToolbarPage(page, '/toolbar/keyboard');
    await page.locator(`${TB} #k-one button`).focus();
    await page.keyboard.press('ArrowRight');
    await expectFocusedItem(page, 'k-three');
    await page.keyboard.press('ArrowRight');
    await expectFocusedItem(page, 'k-four');
    await page.keyboard.press('ArrowRight');
    await expectFocusedItem(page, 'k-toggle');
    await page.keyboard.press('ArrowRight');
    await expectFocusedItem(page, 'k-toggle');
    await page.keyboard.press('ArrowLeft');
    await expectFocusedItem(page, 'k-four');
  });

  test('P-TB-010 focus skips disabled, hidden, separator and spacer elements', async ({ page }) => {
    await openToolbarPage(page, '/toolbar/keyboard');
    await page.locator(`${TB} #k-one button`).focus();
    const order = ['k-three', 'k-four', 'k-toggle'];
    for (const id of order) {
      await page.keyboard.press('ArrowRight');
      await expectFocusedItem(page, id);
    }
    // Only the enabled, visible items were visited: k-two (disabled) and k-hidden (not rendered) never held focus.
  });

  test('P-TB-006 Home and End jump to the first and last enabled item', async ({ page }) => {
    await openToolbarPage(page, '/toolbar/keyboard');
    await page.locator(`${TB} #k-four button`).focus();
    await page.keyboard.press('Home');
    await expectFocusedItem(page, 'k-one');
    await page.keyboard.press('End');
    await expectFocusedItem(page, 'k-toggle');
  });

  test('P-TB-005 a vertical toolbar uses ArrowDown and ArrowUp', async ({ page }) => {
    await openToolbarPage(page, '/toolbar/keyboard');
    await page.locator('#toolbar-keys-v #kv-one button').focus();
    await page.keyboard.press('ArrowDown');
    await expectFocusedItem(page, 'kv-two');
    await page.keyboard.press('ArrowRight');
    await expectFocusedItem(page, 'kv-two');
    await page.keyboard.press('ArrowUp');
    await expectFocusedItem(page, 'kv-one');
  });

  test('P-TB-007 Enter and Space activate the focused item', async ({ page }) => {
    await openToolbarPage(page, '/toolbar/keyboard');
    await page.locator(`${TB} #k-three button`).focus();
    await page.keyboard.press('Enter');
    await expect(page.locator('#last-click')).toHaveText('Last click: k-three');
    await page.locator(`${TB} #k-four button`).focus();
    await page.keyboard.press('Space');
    await expect(page.locator('#last-click')).toHaveText('Last click: k-four');
  });

  test('A11Y-050 focus returns to the last focused item after leaving the toolbar', async ({ page }) => {
    await openToolbarPage(page, '/toolbar/keyboard');
    await page.locator(`${TB} #k-one button`).focus();
    await page.keyboard.press('ArrowRight');
    await expectFocusedItem(page, 'k-three');
    await page.keyboard.press('ArrowRight');
    await expectFocusedItem(page, 'k-four');
    await page.keyboard.press('Tab');
    await expect(page.locator('#after-toolbar')).toBeFocused();
    await page.keyboard.press('Shift+Tab');
    await expectFocusedItem(page, 'k-four');
  });

  test('P-TB-029 in RTL ArrowLeft moves forward and ArrowRight backward', async ({ page }) => {
    await openToolbarPage(page, '/toolbar/keyboard?rtl=true');
    await page.locator(`${TB} #k-one button`).focus();
    await page.keyboard.press('ArrowLeft');
    await expectFocusedItem(page, 'k-three');
    await page.keyboard.press('ArrowRight');
    await expectFocusedItem(page, 'k-one');
  });
});
