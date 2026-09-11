import { test, expect } from '@playwright/test';
import { checkCheckbox } from './checkbox-helpers';

test.describe('Checkbox – Disabled State', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/checkbox/disabled-state');
    await page.waitForLoadState('networkidle');
  });

  test('Disabled checkbox cannot be toggled', async ({ page }) => {
    const disabledCheckbox = page.locator('input[type="checkbox"][disabled]').first();

    await expect(disabledCheckbox).not.toBeChecked();
    await disabledCheckbox.click({ force: true });
    await expect(disabledCheckbox).not.toBeChecked();
  });

  test('Dynamically disabled checkbox stops interaction', async ({ page }) => {
    const toggleBtn = page.getByRole('button', { name: 'Disable Checkbox' });
    const checkbox = page.locator('input[type="checkbox"]').nth(2);

    await checkCheckbox(checkbox);
    await expect(checkbox).toBeChecked();

    await toggleBtn.click();

    await expect(checkbox).toBeDisabled();
  });

});