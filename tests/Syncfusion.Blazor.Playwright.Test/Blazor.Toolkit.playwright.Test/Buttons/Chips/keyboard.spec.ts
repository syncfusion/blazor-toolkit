import { test, expect, type Page } from '@playwright/test';

const INTERACTIONS = 'http://localhost:5000/chips/interactions';

async function openInteractions(page: Page) {
  await page.goto(INTERACTIONS, { waitUntil: 'networkidle' });
  await expect(page.locator('#interactive-ready')).toHaveAttribute('data-ready', 'true', { timeout: 15000 });
}

test.describe('Chip keyboard @chip', () => {
  test.beforeEach(async ({ page }) => {
    await openInteractions(page);
  });

  test('CHIP-P-090 arrow moves focus without selecting', async ({ page }) => {
    const first = page.locator('#chipset-keyboard [data-testid="chip-root"]').first();
    await first.focus();
    await page.keyboard.press('ArrowRight');
    await expect(page.locator('#chipset-keyboard [data-testid="chip-root"]').nth(1)).toBeFocused();
    await expect(first).not.toHaveAttribute('aria-pressed', 'true');
  });

  test('CHIP-P-081 Delete removes a removable chip', async ({ page }) => {
    const chip = page.locator('#chip-remove');
    await chip.focus();
    await page.keyboard.press('Delete');
    await expect(chip).toHaveCount(0);
  });
});
