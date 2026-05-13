import { test, expect } from '@playwright/test';

test.describe('TextBox - Basic rendering and attributes', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/inputs/textbox');
    await page.waitForLoadState('networkidle');
  });

  test('renders input types and placeholders', async ({ page }) => {
    const text = page.locator('#textTypeInput');
    await expect(text).toBeVisible();
    await expect(text).toHaveAttribute('placeholder', 'Enter text');

    const email = page.locator('#emailTypeInput');
    await expect(email).toBeVisible();
    await expect(email).toHaveAttribute('placeholder', 'Enter email');
  });

  test('disabled and readonly inputs expose attributes', async ({ page }) => {
    const disabled = page.locator('#disabledInput');
    await expect(disabled).toBeVisible();
    // prefer Playwright's control state matcher
    await expect(disabled).toBeDisabled();

    const readonly = page.locator('#readonlyInput');
    await expect(readonly).toBeVisible();
    // readonly may be reflected on the DOM property rather than attribute
    const isReadOnly = await readonly.evaluate((el: any) => el.readOnly === true || el.hasAttribute('readonly'));
    expect(isReadOnly).toBeTruthy();
  });
});
