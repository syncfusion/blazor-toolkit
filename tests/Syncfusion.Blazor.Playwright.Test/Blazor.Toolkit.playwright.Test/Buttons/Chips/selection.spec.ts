import { test, expect, type Locator, type Page } from '@playwright/test';

const STRUCTURE = 'http://localhost:5000/chips/structure';

async function openStructure(page: Page) {
  await page.goto(STRUCTURE, { waitUntil: 'networkidle' });
  await expect(page.locator('#structure-ready')).toHaveAttribute('data-ready', 'true', { timeout: 15000 });
}

type Box = { x: number; y: number; width: number; height: number };

async function boxOf(locator: Locator): Promise<Box> {
  const box = await locator.boundingBox();
  expect(box).not.toBeNull();
  return box!;
}

test.describe('Chip selection structure @chip', () => {
  test.beforeEach(async ({ page }) => {
    await openStructure(page);
  });

  test('CHIP-P-201 selected Filter and Choice show a visible leading check', async ({ page }) => {
    for (const id of ['#chip-filter-selected', '#chip-choice-selected']) {
      const chip = page.locator(`${id} [data-testid="chip-root"]`);
      await expect(chip).toHaveClass(/e-chip-selected/);
      await expect(chip).toHaveAttribute('aria-pressed', 'true');
      const checks = chip.locator('.e-chip-leading .e-toolkit-icons.e-check');
      await expect(checks).toHaveCount(1);
      const box = await boxOf(checks);
      expect(box.width).toBeGreaterThan(0);
      expect(box.height).toBeGreaterThan(0);
    }
  });

  test('CHIP-P-202 clear Filter and selected Action or Input have no check', async ({ page }) => {
    for (const id of ['#chip-filter-clear', '#chip-action-selected', '#chip-input-selected']) {
      await expect(page.locator(`${id} .e-toolkit-icons.e-check`)).toHaveCount(0);
    }
  });

  test('CHIP-P-203 author leading content suppresses the automatic check', async ({ page }) => {
    const icon = page.locator('#chip-filter-icon');
    await expect(icon.locator('.e-chip-leading .e-icons.e-leaf')).toHaveCount(1);
    await expect(icon.locator('.e-toolkit-icons.e-check')).toHaveCount(0);
    const template = page.locator('#chip-filter-template');
    await expect(template.locator('[data-testid="author-leading"]')).toHaveCount(1);
    await expect(template.locator('.e-toolkit-icons.e-check')).toHaveCount(0);
  });

  test('CHIP-P-204 remove button stays inside the chip on filled, soft, and outlined', async ({ page }) => {
    for (const id of ['#chip-remove-filled', '#chip-remove-soft', '#chip-remove-outlined']) {
      const chip = page.locator(`${id} [data-testid="chip-root"]`);
      const remove = chip.locator('.e-chip-remove');
      await expect(remove).toHaveCount(1);
      expect(await remove.evaluate((el) => el.tagName)).toBe('BUTTON');
      await expect(remove).toHaveAttribute('type', 'button');
      const chipBox = await boxOf(chip);
      const removeBox = await boxOf(remove);
      expect(removeBox.height).toBeLessThanOrEqual(chipBox.height + 0.5);
      expect(removeBox.width).toBeGreaterThan(0);
      expect(removeBox.width).toBeLessThanOrEqual(chipBox.height + 0.5);
      expect(removeBox.x).toBeGreaterThanOrEqual(chipBox.x - 0.5);
      expect(removeBox.y).toBeGreaterThanOrEqual(chipBox.y - 0.5);
      expect(removeBox.x + removeBox.width).toBeLessThanOrEqual(chipBox.x + chipBox.width + 0.5);
      expect(removeBox.y + removeBox.height).toBeLessThanOrEqual(chipBox.y + chipBox.height + 0.5);
    }
  });

  test('CHIP-P-205 soft and outlined selected filters keep the check and content text color', async ({ page }) => {
    const filled = page.locator('#chip-filter-selected [data-testid="chip-root"]');
    const filledColor = await filled.evaluate((el) => getComputedStyle(el).color);
    for (const id of ['#chip-filter-soft', '#chip-filter-outlined']) {
      const chip = page.locator(`${id} [data-testid="chip-root"]`);
      await expect(chip.locator('.e-chip-leading .e-toolkit-icons.e-check')).toHaveCount(1);
      const color = await chip.evaluate((el) => getComputedStyle(el).color);
      expect(color).not.toBe(filledColor);
    }
  });

  test('CHIP-P-206 RTL places the check at inline-start and remove at inline-end', async ({ page }) => {
    const root = page.locator('#chip-filter-rtl [data-testid="chip-root"]');
    const check = await boxOf(root.locator('.e-toolkit-icons.e-check'));
    const text = await boxOf(root.locator('.e-chip-text'));
    const remove = await boxOf(root.locator('.e-chip-remove'));
    const checkCenter = check.x + check.width / 2;
    const textCenter = text.x + text.width / 2;
    const removeCenter = remove.x + remove.width / 2;
    expect(checkCenter).toBeGreaterThan(textCenter);
    expect(removeCenter).toBeLessThan(textCenter);
  });

  test('CHIP-P-207 visible selected chip in a collapse set has the check', async ({ page }) => {
    const selected = page.locator('#chipset-collapse [data-testid="chip-root"].e-chip-selected');
    await expect(selected).toHaveCount(1);
    await expect(selected.locator('.e-chip-leading .e-toolkit-icons.e-check')).toHaveCount(1);
  });
});
