import { test, expect } from '@playwright/test';
import { openToolbarPage, BASE } from './helpers';

const TB = '#toolbar-overflow';
const MORE = `${TB} [data-testid="toolbar-overflow-button"]`;
const POPUP = `${TB} [data-testid="toolbar-overflow-popup"]`;

async function openPopup(page: import('@playwright/test').Page) {
  await openToolbarPage(page, '/toolbar/overflow');
  await page.locator('#width-narrow').click();
  await expect(page.locator(MORE)).toHaveCount(1, { timeout: 8000 });
  await page.locator(MORE).click();
  await expect(page.locator(POPUP)).toBeVisible();
  await expect(page.locator(`${POPUP} [role="menuitem"]`).first()).toBeFocused();
}

test.describe('Toolbar focus restore @toolbar', () => {
  test('P-TB-011 focus returns to the More button after Escape closes the popup', async ({ page }) => {
    await openPopup(page);
    await page.keyboard.press('Escape');
    await expect(page.locator(POPUP)).toHaveCount(0);
    await expect(page.locator(MORE)).toBeFocused();
  });

  test('P-TB-011 focus returns to the More button after selecting an entry', async ({ page }) => {
    await openPopup(page);
    await page.keyboard.press('Enter');
    await expect(page.locator(POPUP)).toHaveCount(0);
    await expect(page.locator(MORE)).toBeFocused();
  });

  test('P-TB-011 clicking outside closes the popup and does not trap focus', async ({ page }) => {
    await openPopup(page);
    await page.locator('#after-toolbar').click({ position: { x: 2, y: 2 }, force: true });
    await expect(page.locator(POPUP)).toHaveCount(0);
    expect(page.url().startsWith(BASE)).toBe(true);
  });
});
