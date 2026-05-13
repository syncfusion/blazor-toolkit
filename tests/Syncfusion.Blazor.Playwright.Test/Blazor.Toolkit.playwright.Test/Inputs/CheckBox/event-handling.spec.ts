import { test, expect } from '@playwright/test';

test.describe('Checkbox – Event Handling', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/checkbox/event-handling');
    await page.waitForLoadState('networkidle');
  });

  test('ValueChange event increments counter', async ({ page }) => {
    const checkbox = page.locator('input[type="checkbox"]').first();
    const counter = page.locator('p:has-text("Change Count") strong');

    await expect(counter).toHaveText('0');

    await checkbox.click();

    await expect(counter).toHaveText('1');
  });

  test('Multiple checkboxes fire independent events', async ({ page }) => {
    const logContainer = page.locator(
      'div[style*="overflow-y"]'
    );

    const initialLogText = await logContainer.innerText();

    const checkboxes = page.locator('input[type="checkbox"]');
    await checkboxes.nth(1).click();
    await checkboxes.nth(2).click();

    await expect(logContainer).not.toHaveText(initialLogText);
  });

});