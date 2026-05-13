import { test, expect } from '@playwright/test';

const DEFAULT_TIMEOUT = 2000;

test.describe('Tooltip - markup rendering', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/tooltip-all-samples');
    await page.waitForLoadState('networkidle');
  });

  test('tooltip markup and markupstring render correctly', async ({ page }) => {
    // Verify markup-rendered tooltip for #targetMarkup shows expected bold text
    await page.hover('#targetMarkup');
    await expect(page.locator('.e-tip-content', { hasText: 'Environmentally friendly' })).toBeVisible({ timeout: DEFAULT_TIMEOUT });
  });
});
