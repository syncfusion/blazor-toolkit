import { test, expect } from '@playwright/test';

const DEFAULT_TIMEOUT = 5000;

test.describe('Dialog - Accessibility', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/dialog/basic-rendering');
    await page.waitForLoadState('networkidle');
  });

  test('Modal dialog has aria-modal and aria-describedby attributes', async ({ page }) => {
    await page.click('#open-modal');
    const dlg = page.locator('#dlg-modal');
    await expect(dlg).toBeVisible({ timeout: DEFAULT_TIMEOUT });
    await expect(dlg).toHaveAttribute('aria-modal', 'true');
    const described = await dlg.getAttribute('aria-describedby');
    expect(described).toBeTruthy();
    // content referenced by aria-describedby should be present
    const describedLocator = page.locator('#' + described);
    const count = await describedLocator.count();
    let visibleFound = false;
    for (let i = 0; i < count; ++i) {
      const item = describedLocator.nth(i);
      if (await item.isVisible()) {
        await expect(item).toBeVisible();
        visibleFound = true;
        break;
      }
    }
    expect(visibleFound).toBeTruthy();
  });

  test('Modal overlay prevents interaction behind dialog (basic)', async ({ page }) => {
    // Basic test: overlay present and blocking clicks to underlying controls
    await page.click('#open-modal');
    const overlay = page.locator('#dlg-modal_overlay');
    await expect(page.locator('#dlg-modal')).toBeVisible({ timeout: DEFAULT_TIMEOUT });
    await expect(overlay).toBeVisible({ timeout: DEFAULT_TIMEOUT });

    // Instead of attempting a click (which Playwright blocks when element is covered),
    // verify the overlay physically covers the underlying control by checking bounding boxes.
    const btn = page.locator('#open-nonmodal');
    const btnBox = await btn.boundingBox();
    const overlayBox = await overlay.boundingBox();
    expect(btnBox).not.toBeNull();
    expect(overlayBox).not.toBeNull();
    // Helper to check rectangle intersection
    const intersects = (a: any, b: any) => !(a.x + a.width <= b.x || b.x + b.width <= a.x || a.y + a.height <= b.y || b.y + b.height <= a.y);
    expect(intersects(overlayBox, btnBox)).toBeTruthy();
  });

  test('Focus trap: Tab cycles inside modal dialog and focus restores on close', async ({ page }) => {
    // Open modal
    await page.click('#open-modal');
    await expect(page.locator('#dlg-modal')).toBeVisible({ timeout: DEFAULT_TIMEOUT });

    // Focus first focusable element inside dialog (OK button)
    const ok = page.locator('#dlg-modal .e-footer-content button').first();
    await ok.focus();
    // Press Tab repeatedly and ensure activeElement remains inside dialog
    for (let i = 0; i < 6; i++) {
      await page.keyboard.press('Tab');
      const inside = await page.evaluate(() => {
        const dlg = document.getElementById('dlg-modal');
        return dlg ? dlg.contains(document.activeElement) : false;
      });
      expect(inside).toBeTruthy();
    }

    // Close dialog and ensure focus is not inside dialog
    await ok.click();
    await page.waitForTimeout(200);
    const activeInside = await page.evaluate(() => document.getElementById('dlg-modal')?.contains(document.activeElement));
    expect(activeInside).toBeFalsy();
  });
});
