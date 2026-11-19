import { test, expect, type Page } from '@playwright/test';

const URL = 'http://localhost:5000/chips/visual';

const shots = [
  ['CHIP-V-001', 'chip-filter-selected', 'chip-filter-selected.png'],
  ['CHIP-V-002', 'chip-choice-selected', 'chip-choice-selected.png'],
  ['CHIP-V-003', 'chip-filter-clear', 'chip-filter-clear.png'],
  ['CHIP-V-004', 'chip-remove-filled', 'chip-remove-filled.png'],
  ['CHIP-V-005', 'chip-remove-soft', 'chip-remove-soft.png'],
  ['CHIP-V-006', 'chip-remove-outlined', 'chip-remove-outlined.png'],
  ['CHIP-V-007', 'chip-filter-selected-remove', 'chip-filter-selected-remove.png'],
  ['CHIP-V-008', 'chip-filter-rtl', 'chip-filter-rtl.png'],
] as const;

async function openVisual(page: Page) {
  await page.goto(URL, { waitUntil: 'commit' });
  await expect(page.locator('#visual-ready')).toHaveAttribute('data-ready', 'true', { timeout: 15000 });
}

test.describe('Chip visual baselines @chip', () => {
  test.beforeEach(async ({ page }) => {
    await openVisual(page);
  });

  for (const [id, sectionId, png] of shots) {
    test(`${id} ${sectionId} matches ${png}`, async ({ page }) => {
      const section = page.locator(`#${sectionId}`);
      await expect(section.locator('[data-testid="chip-root"]')).toBeVisible();
      await expect(section).toBeVisible();
      await expect(section).toHaveScreenshot(png, {
        animations: 'disabled',
        caret: 'hide',
        maxDiffPixelRatio: 0.01,
      });
    });
  }
});
