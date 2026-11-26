import { test, expect } from '@playwright/test';
import { openToolbarPage, expectFocusedItem } from './helpers';

const TB = '#toolbar-template';

test.describe('Toolbar template item @toolbar', () => {
  test('P-TB-035 a template item renders its content and keeps typed input', async ({ page }) => {
    await openToolbarPage(page, '/toolbar/template');
    await expect(page.locator(`${TB} [data-testid="toolbar-template-item"]`)).toHaveCount(1);
    await page.locator('#search-box').fill('toolbar');
    await expect(page.locator('#query-value')).toHaveText('Query: toolbar');
  });

  test('P-TB-035 a button inside the template runs its own handler without raising an item click', async ({ page }) => {
    await openToolbarPage(page, '/toolbar/template');
    await page.locator('#search-go').click();
    await expect(page.locator('#search-count')).toHaveText('Searches: 1');
    await expect(page.locator('#last-click')).toHaveText('Last click: none');
  });

  test('P-TB-035 arrow keys inside the input do not move toolbar focus', async ({ page }) => {
    await openToolbarPage(page, '/toolbar/template');
    await page.locator('#search-box').fill('abc');
    await page.locator('#search-box').focus();
    await page.keyboard.press('ArrowLeft');
    await expect(page.locator('#search-box')).toBeFocused();
    await page.locator(`${TB} #t-first button`).focus();
    await page.keyboard.press('ArrowRight');
    await expectFocusedItem(page, 't-last');
  });
});
