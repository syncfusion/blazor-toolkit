import { test, expect } from '@playwright/test';

// Localization and Hijri calendar checks
test.describe('Calendar - Localization', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/calendar-test');
    await page.waitForSelector('#calendar-test');
  });

  test('hijri calendar instance is present and visible', async ({ page }) => {
    const hijri = page.locator('#calendar-hijri');
    await expect(hijri).toBeVisible();
    // Ensure the hijri calendar renders some visible text (month/year header or day cells)
    const text = await hijri.innerText();
    expect(text.trim().length).toBeGreaterThan(0);
  });

  test('supports multiple locales via parameterized checks (visual smoke)', async ({ page }) => {
    const locales = ['en-US', 'fr-FR'];
    for (const loc of locales) {
      // This is a smoke check — pages should render for each locale when provided by the app
      // (If app supports dynamic locale switching, add more specific assertions.)
      await page.evaluate((l) => { window['testLocale'] = l; }, loc);
      await page.reload();
      await page.waitForSelector('#calendar-test');
      await expect(page.locator('#calendar-test')).toBeVisible();
    }
  });
});
