import { test, expect, type Page } from '@playwright/test';

const SCENARIOS = 'http://localhost:5000/chips/scenarios';

async function openScenarios(page: Page) {
  await page.goto(SCENARIOS, { waitUntil: 'commit' });
  await expect(page.locator('#scenarios-ready')).toHaveAttribute('data-ready', 'true', { timeout: 15000 });
}

test.describe('Chip layout @chip', () => {
  test.beforeEach(async ({ page }) => {
    await openScenarios(page);
  });

  test('CHIP-P-001 wrap chip set renders bound chips', async ({ page }) => {
    await expect(page.locator('#chipset-wrap [data-testid="chip-root"]')).toHaveCount(5);
  });

  test('CHIP-P-030 scroll chip set keeps a single row', async ({ page }) => {
    const box = await page.locator('#chipset-scroll').boundingBox();
    expect(box?.height ?? 0).toBeLessThan(80);
  });

  test('CHIP-P-050 320px wrap does not overflow the stage', async ({ page }) => {
    await page.setViewportSize({ width: 320, height: 800 });
    const stage = await page.locator('#scn-narrow').boundingBox();
    const chip = page.locator('#chipset-narrow [data-testid="chip-root"]').last();
    const chipBox = await chip.boundingBox();
    expect((chipBox?.x ?? 0) + (chipBox?.width ?? 0)).toBeLessThanOrEqual((stage?.x ?? 0) + (stage?.width ?? 0) + 1);
  });

  test('CHIP-P-080 removable chip exposes a remove button', async ({ page }) => {
    await expect(page.locator('#chip-removable .e-chip-remove')).toHaveCount(1);
  });
});
