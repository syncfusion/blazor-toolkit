import { test, expect } from '@playwright/test';
import { openToolbarPage } from './helpers';

const TB = '#toolbar-disabled';

test.describe('Toolbar disabled items @toolbar', () => {
  test('P-TB-033 every item is aria-disabled and a click raises nothing', async ({ page }) => {
    await openToolbarPage(page, '/toolbar/disabled');
    const buttons = page.locator(`${TB} .e-toolbar-item .e-btn`);
    await expect(buttons).toHaveCount(3);
    for (let i = 0; i < 3; i++) {
      await expect(buttons.nth(i)).toHaveAttribute('aria-disabled', 'true');
    }
    await buttons.first().click({ force: true });
    await buttons.nth(2).click({ force: true });
    await expect(page.locator('#last-click')).toHaveText('Last click: none');
    await expect(buttons.nth(2)).not.toHaveAttribute('aria-pressed', 'true');
  });

  test('P-TB-033 Tab passes over a toolbar whose items are all disabled', async ({ page }) => {
    await openToolbarPage(page, '/toolbar/disabled');
    await page.locator('#before-toolbar').focus();
    await page.keyboard.press('Tab');
    await expect(page.locator('#after-toolbar')).toBeFocused();
  });
});
