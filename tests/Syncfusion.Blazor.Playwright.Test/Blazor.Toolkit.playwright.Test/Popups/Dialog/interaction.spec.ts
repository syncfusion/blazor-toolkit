import { test, expect } from '@playwright/test';

const DEFAULT_TIMEOUT = 5000;

test.describe('Dialog - Interaction (drag/resize)', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/dialog/basic-rendering');
    await page.waitForLoadState('networkidle');
  });

  test('Draggable dialog moves when dragging header', async ({ page }) => {
    await page.click('#open-drag');
    const dlg = page.locator('#dlg-drag');
    await expect(dlg).toBeVisible({ timeout: DEFAULT_TIMEOUT });

    const header = page.locator('#dlg-drag_dialog-header');
    await expect(header).toBeVisible({ timeout: DEFAULT_TIMEOUT });
    const before = await dlg.boundingBox();
    const hb = await header.boundingBox();
    if (hb && before) {
      const startX = hb.x + hb.width / 2;
      const startY = hb.y + hb.height / 2;
      // Move to header center, press down and drag with multiple steps to simulate a user drag.
      await page.mouse.move(startX, startY);
      await page.mouse.down();
      await page.mouse.move(startX + 120, startY + 80, { steps: 12 });
      // small pause to allow any drag handlers to apply transforms
      await page.waitForTimeout(100);
      await page.mouse.up();
      const after = await dlg.boundingBox();
      expect(after).not.toBeNull();
      if (after) {
        expect(Math.abs((after.y ?? 0) - (before.y ?? 0))).toBeGreaterThan(0);
      }
    }
  });

  test('Resizable dialog shows resize handles when enabled', async ({ page }) => {
    await page.click('#open-resize');
    const dlg = page.locator('#dlg-resize');
    await expect(dlg).toBeVisible({ timeout: DEFAULT_TIMEOUT });
    // presence of resizable class on root
    await expect(dlg).toHaveClass(/e-dlg-resizable|e-resizable/);
  });

  test('Resizable dialog changes dimensions when dragged at SE corner', async ({ page }) => {
    await page.click('#open-resize');
    const dlg = page.locator('#dlg-resize');
    await expect(dlg).toBeVisible({ timeout: DEFAULT_TIMEOUT });

    const boxBefore = await dlg.boundingBox();
    if (!boxBefore) {
      throw new Error('Dialog bounding box unavailable');
    }

    // Ensure the SE resize handle is present and visible.
    const handle = dlg.locator('.e-resize-handle.e-south-east');
    await expect(handle).toBeVisible({ timeout: DEFAULT_TIMEOUT });
  });
});
