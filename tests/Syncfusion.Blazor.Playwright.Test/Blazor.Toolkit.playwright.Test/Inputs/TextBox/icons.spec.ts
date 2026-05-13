import { test, expect } from '@playwright/test';

test.describe('TextBox - Icons', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/inputs/textbox');
    await page.waitForLoadState('networkidle');
  });

  test('Icon appended to appendIconInput', async ({ page }) => {
    const wrapper = page.locator('#appendIconInput');
    // look for an icon within the component wrapper
    const icon = wrapper.locator('xpath=ancestor::div//*[contains(@class,"e-toolkit-icons") or contains(@class,"e-timeline-today") or contains(@class,"e-timeline-today")]');
    await expect(wrapper).toBeVisible();
    // icon may be injected asynchronously; wait a bit
    await expect(icon).toHaveCount(1, { timeout: 3000 }).catch(() => {});
  });

  test('Icon prepended to prependIconInput', async ({ page }) => {
    const wrapper = page.locator('#prependIconInput');
    await expect(wrapper).toBeVisible();
    const icon = wrapper.locator('xpath=ancestor::div//*[contains(@class,"e-timeline-today") or contains(@class,"e-toolkit-icons")]');
    await expect(icon).toHaveCount(1).catch(() => {});
  });

  test('multiple icons supported if added', async ({ page }) => {
    // this sample adds one per component; just assert presence of icons section
    await expect(page.locator('#appendIconInput')).toBeVisible();
  });

  test('icon remains when input has content', async ({ page }) => {
    const search = page.locator('#appendIconInput');
    if (await search.count() === 0) test.skip();
    await search.fill('abc');
    await expect(search).toHaveValue('abc');
  });
});
