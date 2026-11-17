import { test, expect } from '@playwright/test';

test('CHIP-P-110 static and interactive chip roots stay native', async ({ page }) => {
  await page.goto('http://localhost:5000/chips/scenarios', { waitUntil: 'commit' });
  await expect(page.locator('#chipset-wrap [data-testid="chip-root"]').first()).toBeVisible();
  await expect(page.locator('#chipset-wrap')).not.toHaveAttribute('role', 'listbox');

  await page.goto('http://localhost:5000/chips/interactions', { waitUntil: 'networkidle' });
  await expect(page.locator('#interactive-ready')).toHaveAttribute('data-ready', 'true');
  await expect(page.locator('#chipset-keyboard')).not.toHaveAttribute('role', 'listbox');
});
