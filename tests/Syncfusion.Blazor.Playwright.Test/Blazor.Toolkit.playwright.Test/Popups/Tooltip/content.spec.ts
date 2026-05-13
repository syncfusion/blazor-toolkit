import { test, expect } from '@playwright/test';

test.describe('Tooltip content scenarios', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/tooltip-all-samples');
    await page.waitForLoadState('networkidle');
  });

  test('basic tooltip shows on hover', async ({ page }) => {
    await page.hover('#btnText');
    await expect(page.locator('.e-tip-content', { hasText: 'Lets go green & Save Earth !!' })).toBeVisible();
  });

  test('tooltip renders HTML content', async ({ page }) => {
    // title-based tooltip: hover element with title attribute inside #container
    await page.hover('#btn1');
    await expect(page.locator('.e-tip-content', { hasText: 'Go green and save energy!' })).toBeVisible();
  });

  test('custom template content is present', async ({ page }) => {
    // OpensOn Click - click the button to open the template tooltip
    await page.click('#btnTemplate');
    await expect(page.locator('.democontent h3', { hasText: 'Eastern Bluebird' })).toBeVisible();
  });

  test('interactive tooltip allows clicking internal button', async ({ page }) => {
    // Dynamic RenderFragment content - hover the target and assert rendered markup
    await page.hover('#targetDynamic');
    await expect(page.locator('.e-tip-content h3', { hasText: 'Complex Tooltip Content' })).toBeVisible();
  });
});
