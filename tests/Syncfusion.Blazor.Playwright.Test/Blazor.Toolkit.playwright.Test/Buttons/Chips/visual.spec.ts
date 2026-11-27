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

  test('CHIP-V-009 removable interactive chip keeps the remove control inside the pill', async ({ page }) => {
    const chip = page.locator('#chip-filter-selected-remove [data-testid="chip-root"]');
    const remove = page.locator('#chip-filter-selected-remove [data-testid="chip-remove"]');
    const geometry = await chip.evaluate((element) => {
      const chipStyle = getComputedStyle(element);
      const chipBounds = element.getBoundingClientRect();
      const removeElement = element.parentElement?.querySelector('[data-testid="chip-remove"]');
      const removeBounds = removeElement?.getBoundingClientRect();

      return {
        removePosition: removeElement ? getComputedStyle(removeElement).position : '',
        paddingInlineEnd: Number.parseFloat(chipStyle.paddingInlineEnd),
        chipLeft: chipBounds.left,
        chipRight: chipBounds.right,
        removeLeft: removeBounds?.left ?? Number.NaN,
        removeRight: removeBounds?.right ?? Number.NaN,
      };
    });

    expect(geometry.removePosition).toBe('absolute');
    expect(geometry.paddingInlineEnd).toBeGreaterThan(30);
    expect(geometry.removeLeft).toBeGreaterThanOrEqual(geometry.chipLeft);
    expect(geometry.removeRight).toBeLessThanOrEqual(geometry.chipRight);
  });

  test('CHIP-V-010 removable chip remove icon follows the chip foreground color', async ({ page }) => {
    for (const sectionId of [
      'chip-remove-filled',
      'chip-remove-soft',
      'chip-remove-outlined',
      'chip-filter-selected-remove',
    ]) {
      const section = page.locator(`#${sectionId}`);
      const chip = section.locator('[data-testid="chip-root"]');
      const remove = section.locator('[data-testid="chip-remove"]');

      await expect(remove).toHaveCSS('color', await chip.evaluate((element) => getComputedStyle(element).color));

      await chip.hover();
      await expect(remove).toHaveCSS('color', await chip.evaluate((element) => getComputedStyle(element).color));
    }
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
