import { test, expect } from '@playwright/test';

test.describe('TextBox - Input types', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/inputs/textbox');
    await page.waitForLoadState('networkidle');
  });

  test('email, password, tel, url and search input types are rendered', async ({ page }) => {
    await expect(page.locator('#emailTypeInput')).toBeVisible();
    await expect(page.locator('#emailTypeInput')).toHaveAttribute('type', 'email');

    await expect(page.locator('#passwordTypeInput')).toBeVisible();
    await expect(page.locator('#passwordTypeInput')).toHaveAttribute('type', 'password');

    await expect(page.locator('#telTypeInput')).toBeVisible();
    await expect(page.locator('#telTypeInput')).toHaveAttribute('type', 'tel');

    await expect(page.locator('#urlTypeInput')).toBeVisible();
    await expect(page.locator('#urlTypeInput')).toHaveAttribute('type', 'url');

    await expect(page.locator('#searchTypeInput')).toBeVisible();
    await expect(page.locator('#searchTypeInput')).toHaveAttribute('type', 'search');
  });
});
