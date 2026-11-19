import { test, expect, Page } from '@playwright/test';

// L5 visual regression (TEST-MATRIX.md section 9). Baselines live in ./baselines/<project>/ and change only in a dedicated PR.
// Screenshots are pixel output of one OS and browser build, so only Chromium is compared.
const BASE = 'http://localhost:5000';
const SHOT = { maxDiffPixelRatio: 0.005, animations: 'disabled' as const };

async function show(page: Page, route: string, size: { width: number; height: number }) {
  await page.setViewportSize(size);
  await page.goto(BASE + route);
  await expect(page.locator('#sidebar-ready')).toHaveAttribute('data-ready', 'true', { timeout: 15000 });
  await page.waitForLoadState('networkidle');
  await page.waitForTimeout(300);
}

const DESKTOP = { width: 1280, height: 720 };
const MOBILE = { width: 375, height: 667 };

test.describe('Sidebar - visual', () => {
  test.beforeEach(({ browserName }) => {
    test.skip(browserName !== 'chromium', 'baselines are recorded for Chromium only');
  });

  test('P-VIS-001 overlay-closed', async ({ page }) => {
    await show(page, '/sidebar/overlay', DESKTOP);
    await expect(page).toHaveScreenshot('overlay-closed.png', SHOT);
  });

  test('P-VIS-002 overlay-open', async ({ page }) => {
    await show(page, '/sidebar/overlay-open', DESKTOP);
    await expect(page).toHaveScreenshot('overlay-open.png', SHOT);
  });

  test('P-VIS-003 push-open', async ({ page }) => {
    await show(page, '/sidebar/push-open', DESKTOP);
    await expect(page).toHaveScreenshot('push-open.png', SHOT);
  });

  test('P-VIS-004 shrink-open', async ({ page }) => {
    await show(page, '/sidebar/shrink-open', DESKTOP);
    await expect(page).toHaveScreenshot('shrink-open.png', SHOT);
  });

  test('P-VIS-005 mini', async ({ page }) => {
    await show(page, '/sidebar/mini', DESKTOP);
    await expect(page).toHaveScreenshot('mini.png', SHOT);
  });

  test('P-VIS-006 rtl-overlay-open', async ({ page }) => {
    await show(page, '/sidebar/rtl-open', DESKTOP);
    await expect(page).toHaveScreenshot('rtl-overlay-open.png', SHOT);
  });

  test('P-VIS-007 mobile-overlay-open', async ({ page }) => {
    await show(page, '/sidebar/overlay-open', MOBILE);
    await expect(page).toHaveScreenshot('mobile-overlay-open.png', SHOT);
  });

  test('P-VIS-008 hover-expanded', async ({ page }) => {
    await show(page, '/sidebar/hover', DESKTOP);
    await page.locator('div.e-sidebar').first().hover();
    await expect(page.locator('div.e-sidebar').first()).toHaveClass(/e-sidebar-hover/);
    await expect(page).toHaveScreenshot('hover-expanded.png', SHOT);
  });

  test('P-VIS-009 end-open', async ({ page }) => {
    await show(page, '/sidebar/lab', DESKTOP);
    await page.locator('#opt-position-right').click();
    await page.locator('#opt-open').click();
    await expect(page.locator('#lab-sidebar')).toHaveClass(/e-sidebar-open/);
    await expect(page).toHaveScreenshot('end-open.png', SHOT);
  });

  test('P-VIS-010 rtl-right', async ({ page }) => {
    await show(page, '/sidebar/lab', DESKTOP);
    await page.locator('#opt-rtl').click();
    await page.locator('#opt-position-right').click();
    await page.locator('#opt-open').click();
    await expect(page.locator('#lab-sidebar')).toHaveClass(/e-sidebar-open/);
    await expect(page).toHaveScreenshot('rtl-right.png', SHOT);
  });

  test('P-VIS-011 responsive-desktop', async ({ page }) => {
    await show(page, '/sidebar/responsive', DESKTOP);
    await expect(page.locator('div.e-sidebar').first()).toHaveClass(/e-sidebar-open/);
    await expect(page).toHaveScreenshot('responsive-desktop.png', SHOT);
  });

  test('P-VIS-012 responsive-mobile', async ({ page }) => {
    await show(page, '/sidebar/responsive', MOBILE);
    await expect(page.locator('div.e-sidebar').first()).not.toHaveClass(/e-sidebar-open/);
    await expect(page).toHaveScreenshot('responsive-mobile.png', SHOT);
  });

  test('P-VIS-013 fixed-open', async ({ page }) => {
    await show(page, '/sidebar/fixed', DESKTOP);
    await page.locator('#open-fixed').click();
    await expect(page.locator('#fixed-sidebar')).toHaveClass(/e-sidebar-open/);
    await expect(page).toHaveScreenshot('fixed-open.png', SHOT);
  });

  test('P-VIS-014 inflow-open', async ({ page }) => {
    await show(page, '/sidebar/fixed', DESKTOP);
    await page.locator('#open-contained').click();
    await expect(page.locator('#contained-sidebar')).toHaveClass(/e-sidebar-open/);
    await expect(page).toHaveScreenshot('inflow-open.png', SHOT);
  });

  test('P-VIS-015 custom-width', async ({ page }) => {
    await show(page, '/sidebar/lab', DESKTOP);
    await page.locator('#opt-width').click();
    await page.locator('#opt-open').click();
    await expect(page.locator('#lab-sidebar')).toHaveClass(/e-sidebar-open/);
    await expect(page).toHaveScreenshot('custom-width.png', SHOT);
  });

  test('P-VIS-016 focus-visible', async ({ page }) => {
    await show(page, '/sidebar/overlay-open', DESKTOP);
    await page.getByRole('link', { name: 'First' }).focus();
    await expect(page).toHaveScreenshot('focus-visible.png', SHOT);
  });

  test('P-VIS-017 reduced-motion', async ({ page }) => {
    await page.emulateMedia({ reducedMotion: 'reduce' });
    await show(page, '/sidebar/overlay-open', DESKTOP);
    await expect(page).toHaveScreenshot('reduced-motion.png', SHOT);
  });

  test('P-VIS-018 forced-colors', async ({ page }) => {
    await page.emulateMedia({ forcedColors: 'active' });
    await show(page, '/sidebar/overlay-open', DESKTOP);
    await expect(page).toHaveScreenshot('forced-colors.png', SHOT);
  });

  test('P-VIS-019 mini-open', async ({ page }) => {
    await show(page, '/sidebar/mini', DESKTOP);
    await page.getByTestId('sidebar-toggle').click();
    await expect(page.locator('div.e-sidebar').first()).toHaveClass(/e-sidebar-open/);
    await expect(page).toHaveScreenshot('mini-open.png', SHOT);
  });

  test('P-VIS-020 dual-start', async ({ page }) => {
    await show(page, '/sidebar/dual', DESKTOP);
    await expect(page.locator('#dual-start')).toHaveClass(/e-sidebar-open/);
    await expect(page).toHaveScreenshot('dual-start.png', SHOT);
  });

  test('P-VIS-021 dual-both', async ({ page }) => {
    await show(page, '/sidebar/dual', DESKTOP);
    await expect(page.locator('#dual-end')).toHaveClass(/e-sidebar-open/);
    await expect(page).toHaveScreenshot('dual-both.png', SHOT);
  });

  test('P-VIS-022 animation-off', async ({ page }) => {
    await show(page, '/sidebar/lab', DESKTOP);
    await page.locator('#opt-animation').click();
    await page.locator('#opt-open').click();
    await expect(page.locator('#lab-sidebar')).toHaveClass(/e-sidebar-open/);
    await expect(page).toHaveScreenshot('animation-off.png', SHOT);
  });

  test('P-VIS-023 slots', async ({ page }) => {
    await show(page, '/sidebar/lab', DESKTOP);
    await page.locator('#opt-slots').click();
    await page.locator('#opt-open').click();
    await expect(page.locator('[data-testid="slot-header"]')).toBeVisible();
    await expect(page).toHaveScreenshot('slots.png', SHOT);
  });

  test('P-VIS-024 scrolled-body', async ({ page }) => {
    await show(page, '/sidebar/lab', DESKTOP);
    await page.locator('#opt-open').click();
    await expect(page.locator('#lab-sidebar')).toHaveClass(/e-sidebar-open/);
    await page.locator('.e-sidebar-body').evaluate((node) => { node.scrollTop = 80; });
    await expect(page).toHaveScreenshot('scrolled-body.png', SHOT);
  });
});
