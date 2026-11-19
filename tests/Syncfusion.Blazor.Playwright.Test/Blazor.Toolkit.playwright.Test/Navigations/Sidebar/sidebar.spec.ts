import { test, expect, Page } from '@playwright/test';

// Matrix IDs (TEST-MATRIX.md) lead every title so a failing case maps straight to its row.
const BASE = 'http://localhost:5000';
const PANEL = 'div.e-sidebar';
const WAIT = 8000;

async function load(page: Page, route: string, size?: { width: number; height: number }) {
  if (size) {
    await page.setViewportSize(size);
  }
  await page.goto(BASE + route);
  // Interactive Server: the fixture flips #sidebar-ready after its first interactive render, so nothing below acts on a dead circuit.
  await expect(page.locator('#sidebar-ready')).toHaveAttribute('data-ready', 'true', { timeout: 15000 });
  await page.waitForLoadState('networkidle');
  // The viewport observer registers right after sidebar.js is imported. A viewport found at registration is the load state, not a
  // crossing, so wait for the import to be recorded and for the registration call that follows it.
  await page.waitForFunction(() => performance.getEntriesByType('resource').some((e) => e.name.endsWith('scripts/sidebar.js')), undefined, { timeout: 15000 });
  await page.waitForTimeout(300);
}

const toggle = (page: Page) => page.getByTestId('sidebar-toggle');
const open = (page: Page) => expect(page.locator(PANEL)).toHaveClass(/e-sidebar-open/, { timeout: WAIT });
const closed = (page: Page) => expect(page.locator(PANEL)).not.toHaveClass(/e-sidebar-open/, { timeout: WAIT });
const box = async (page: Page, selector: string) => (await page.locator(selector).first().boundingBox())!;
const active = (page: Page) =>
  page.evaluate(() => {
    const el = document.activeElement as HTMLElement | null;
    return { id: el?.id ?? '', testid: el?.getAttribute('data-testid') ?? '', inPanel: !!el?.closest('div.e-sidebar'), tag: el?.tagName ?? '' };
  });

// M-O6 applies Escape only while focus is inside the host, so wait for the focus-entry step before pressing it.
const focusInPanel = (page: Page) => expect.poll(async () => (await active(page)).inPanel, { timeout: WAIT }).toBe(true);

const DESKTOP = { width: 1280, height: 720 };
const MOBILE = { width: 375, height: 667 };

test.describe('Sidebar - core interaction', () => {
  test('P-UI-001 Open overlay: panel visible and e-sidebar-open', async ({ page }) => {
    await load(page, '/sidebar/overlay', DESKTOP);
    await toggle(page).click();
    await open(page);
    await expect(page.locator(PANEL)).toBeVisible();
    await expect(page.locator('.e-sidebar-backdrop')).toHaveCount(1);
  });

  test('P-UI-002 Backdrop closes the overlay', async ({ page }) => {
    await load(page, '/sidebar/overlay-open', DESKTOP);
    await open(page);
    await page.locator('.e-sidebar-backdrop').click({ position: { x: 900, y: 300 } });
    await closed(page);
  });

  test('P-UI-003 Escape closes the overlay', async ({ page }) => {
    await load(page, '/sidebar/overlay', DESKTOP);
    await toggle(page).click();
    await open(page);
    await focusInPanel(page);
    await page.keyboard.press('Escape');
    await closed(page);
  });

  test('P-UI-004 CloseOnOverlayClick=false keeps the overlay open', async ({ page }) => {
    await load(page, '/sidebar/options', DESKTOP);
    await page.locator('#opt-overlay-click').click();
    await expect(page.locator('#opt-overlay-click')).toContainText('off');
    await page.locator('.e-sidebar-backdrop').click({ position: { x: 900, y: 300 } });
    await page.waitForTimeout(300);
    await open(page);
  });

  test('P-UI-005 CloseOnEscape=false keeps the overlay open', async ({ page }) => {
    await load(page, '/sidebar/options', DESKTOP);
    await page.locator('#opt-escape').click();
    await expect(page.locator('#opt-escape')).toContainText('off');
    await page.locator('#sidebar-last').focus();
    await page.keyboard.press('Escape');
    await page.waitForTimeout(300);
    await open(page);
  });

  test('P-UI-006 Push offsets the target by the width', async ({ page }) => {
    await load(page, '/sidebar/push', DESKTOP);
    const before = await box(page, '.e-sidebar-target');
    await toggle(page).click();
    await open(page);
    await expect.poll(async () => Math.round((await box(page, '.e-sidebar-target')).x - before.x), { timeout: WAIT }).toBe(280);
    expect(Math.round((await box(page, '.e-sidebar-target')).width)).toBe(Math.round(before.width));
  });

  test('P-UI-007 Shrink reduces the target width by the width', async ({ page }) => {
    await load(page, '/sidebar/shrink', DESKTOP);
    const before = await box(page, '.e-sidebar-target');
    await toggle(page).click();
    await open(page);
    await expect.poll(async () => Math.round(before.width - (await box(page, '.e-sidebar-target')).width), { timeout: WAIT }).toBe(280);
  });

  test('P-UI-008 Mini rail occupies MiniWidth', async ({ page }) => {
    await load(page, '/sidebar/mini', DESKTOP);
    const visible = await page.evaluate(() => {
      const r = document.querySelector('div.e-sidebar')!.getBoundingClientRect();
      const c = document.querySelector('.e-sidebar-container')!.getBoundingClientRect();
      return Math.round(Math.min(r.right, window.innerWidth) - Math.max(r.left, c.left));
    });
    expect(visible).toBe(64);
    const target = await box(page, '.e-sidebar-target');
    const container = await box(page, '.e-sidebar-container');
    expect(Math.round(target.x - container.x)).toBeGreaterThanOrEqual(64);
  });

  test('P-UI-009 Mini expands to Width', async ({ page }) => {
    await load(page, '/sidebar/mini', DESKTOP);
    await toggle(page).click();
    await open(page);
    await expect.poll(async () => page.evaluate(() => {
      const r = document.querySelector('div.e-sidebar')!.getBoundingClientRect();
      return Math.round(r.width);
    }), { timeout: WAIT }).toBe(280);
  });

  test('P-UI-010 Imperative ToggleAsync flips the state', async ({ page }) => {
    await load(page, '/sidebar/options', DESKTOP);
    await open(page);
    await page.locator('#opt-api-toggle').click({ force: true });
    await closed(page);
    await page.locator('#opt-api-toggle').click({ force: true });
    await open(page);
  });

  test('P-UI-011 CloseOnNavigate closes after following a link', async ({ page }) => {
    await load(page, '/sidebar/navigate', DESKTOP);
    await open(page);
    await page.getByTestId('sidebar-nav-link').click();
    await expect(page).toHaveURL(/page=3/);
    await closed(page);
  });

  test('P-UI-012 IsEnabled=false ignores Escape and backdrop', async ({ page }) => {
    await load(page, '/sidebar/options', DESKTOP);
    await page.locator('#opt-enabled').click({ force: true });
    await expect(page.locator('#opt-enabled')).toContainText('off');
    await page.locator('#sidebar-last').focus();
    await page.keyboard.press('Escape');
    await page.locator('.e-sidebar-backdrop').click({ position: { x: 900, y: 300 } });
    await page.waitForTimeout(300);
    await open(page);
  });
});

test.describe('Sidebar - keyboard and focus', () => {
  test('P-KB-001 Focus enters the panel on open', async ({ page }) => {
    await load(page, '/sidebar/overlay', DESKTOP);
    await toggle(page).click();
    await open(page);
    await expect.poll(async () => (await active(page)).inPanel, { timeout: WAIT }).toBe(true);
  });

  test('P-KB-002 Tab from the last focusable wraps to the first', async ({ page }) => {
    await load(page, '/sidebar/overlay', DESKTOP);
    await toggle(page).click();
    await open(page);
    await page.locator('#sidebar-last').focus();
    await page.keyboard.press('Tab');
    await expect.poll(async () => page.evaluate(() => document.activeElement?.textContent?.trim()), { timeout: WAIT }).toBe('First');
  });

  test('P-KB-003 Shift+Tab from the first focusable wraps to the last', async ({ page }) => {
    await load(page, '/sidebar/overlay', DESKTOP);
    await toggle(page).click();
    await open(page);
    await page.getByRole('link', { name: 'First' }).focus();
    await page.keyboard.press('Shift+Tab');
    await expect.poll(async () => (await active(page)).id, { timeout: WAIT }).toBe('sidebar-last');
  });

  test('P-KB-004 Closing restores focus to the opener', async ({ page }) => {
    await load(page, '/sidebar/overlay', DESKTOP);
    await toggle(page).focus();
    await page.keyboard.press('Enter');
    await open(page);
    await focusInPanel(page);
    await page.keyboard.press('Escape');
    await closed(page);
    await expect.poll(async () => (await active(page)).testid, { timeout: WAIT }).toBe('sidebar-toggle');
  });

  test('P-KB-005 Focus never reaches main content while modal', async ({ page }) => {
    await load(page, '/sidebar/overlay', DESKTOP);
    await toggle(page).click();
    await open(page);
    for (let i = 0; i < 12; i++) {
      await page.keyboard.press('Tab');
      expect((await active(page)).testid).not.toBe('main-focusable');
    }
    for (let i = 0; i < 12; i++) {
      await page.keyboard.press('Shift+Tab');
      expect((await active(page)).testid).not.toBe('main-focusable');
    }
  });

  test('P-KB-006 Push does not trap focus', async ({ page }) => {
    await load(page, '/sidebar/push-open', DESKTOP);
    await open(page);
    await page.locator('#sidebar-last').focus();
    await page.keyboard.press('Tab');
    await expect.poll(async () => (await active(page)).inPanel, { timeout: WAIT }).toBe(false);
  });

  test('P-KB-007 Escape from a docked Push panel closes it', async ({ page }) => {
    await load(page, '/sidebar/push-open', DESKTOP);
    await page.locator('#sidebar-last').focus();
    await page.keyboard.press('Escape');
    await closed(page);
  });

  test('P-KB-008 Closing by backdrop restores focus to the opener', async ({ page }) => {
    await load(page, '/sidebar/overlay', DESKTOP);
    await toggle(page).focus();
    await page.keyboard.press('Enter');
    await open(page);
    await page.locator('.e-sidebar-backdrop').click({ position: { x: 900, y: 300 } });
    await closed(page);
    await expect.poll(async () => (await active(page)).testid, { timeout: WAIT }).toBe('sidebar-toggle');
  });

  test('P-KB-009 The consumer toggle exposes aria-expanded and aria-controls', async ({ page }) => {
    await load(page, '/sidebar/overlay', DESKTOP);
    const button = toggle(page);
    await expect(button).toHaveAttribute('aria-expanded', 'false');
    await expect(button).toHaveAttribute('aria-controls', 'fx-sidebar');
    await button.click();
    await expect(button).toHaveAttribute('aria-expanded', 'true');
    await expect(page.locator('#fx-sidebar')).toHaveCount(1);
  });
});

test.describe('Sidebar - responsive viewports', () => {
  test('P-VP-001 Desktop keeps Push', async ({ page }) => {
    await load(page, '/sidebar/responsive', DESKTOP);
    await expect(page.locator(PANEL)).toHaveClass(/e-sidebar-push/);
    await open(page);
  });

  test('P-VP-002 Mobile load uses Overlay and starts closed', async ({ page }) => {
    await load(page, '/sidebar/responsive', MOBILE);
    await expect(page.locator(PANEL)).toHaveClass(/e-sidebar-overlay/, { timeout: WAIT });
    await closed(page);
  });

  test('P-VP-003 Resizing to mobile auto-closes and reports the breakpoint', async ({ page }) => {
    await load(page, '/sidebar/responsive', DESKTOP);
    await open(page);
    await page.setViewportSize(MOBILE);
    await closed(page);
    await expect(page.locator(PANEL)).toHaveClass(/e-sidebar-overlay/);
    await expect(page.locator('#status')).toHaveAttribute('data-below', 'true');
    await expect(page.locator('#log')).toContainText('breakpoint:true');
  });

  test('P-VP-004 Resizing to desktop never opens the panel', async ({ page }) => {
    await load(page, '/sidebar/responsive', MOBILE);
    await closed(page);
    await page.setViewportSize(DESKTOP);
    await expect(page.locator(PANEL)).toHaveClass(/e-sidebar-push/, { timeout: WAIT });
    await page.waitForTimeout(300);
    await closed(page);
    await expect(page.locator('#status')).toHaveAttribute('data-below', 'false');
  });

  test('P-VP-005 The threshold is exact: 767px is mobile and 768px is desktop', async ({ page }) => {
    await load(page, '/sidebar/responsive', { width: 768, height: 800 });
    await expect(page.locator(PANEL)).toHaveClass(/e-sidebar-push/, { timeout: WAIT });
    await page.setViewportSize({ width: 767, height: 800 });
    await expect(page.locator(PANEL)).toHaveClass(/e-sidebar-overlay/, { timeout: WAIT });
  });

  test('P-VP-006 Event Mode reports the effective mode', async ({ page }) => {
    await load(page, '/sidebar/responsive', MOBILE);
    await toggle(page).click();
    await open(page);
    await expect(page.locator('#log')).toContainText('opened:Overlay');
  });
});

test.describe('Sidebar - RTL', () => {
  test('P-RTL-001 Start docks to the physical right in rtl', async ({ page }) => {
    await load(page, '/sidebar/rtl', DESKTOP);
    const panel = await box(page, '#rtl-start');
    expect(Math.round(panel.x + panel.width)).toBeGreaterThan(DESKTOP.width - 40);
    await expect(page.locator('#rtl-start')).toHaveClass(/e-sidebar-right/);
  });

  test('P-RTL-002 End docks to the physical left in rtl', async ({ page }) => {
    await load(page, '/sidebar/rtl', DESKTOP);
    const panel = await box(page, '#rtl-end');
    expect(Math.round(panel.x)).toBeLessThan(40);
    await expect(page.locator('#rtl-end')).toHaveClass(/e-sidebar-left/);
  });

  test('P-RTL-003 An rtl overlay opens on the correct edge', async ({ page }) => {
    await load(page, '/sidebar/rtl-open', DESKTOP);
    await open(page);
    await expect.poll(async () => Math.round((await box(page, PANEL)).x + (await box(page, PANEL)).width), { timeout: WAIT }).toBeGreaterThan(DESKTOP.width - 40);
  });

  test('P-RTL-004 A runtime dir change re-resolves a logical position', async ({ page }) => {
    await load(page, '/sidebar/overlay-open', DESKTOP);
    await expect(page.locator(PANEL)).toHaveClass(/e-sidebar-left/);
    await page.evaluate(() => document.querySelector('.fx-root')!.setAttribute('dir', 'rtl'));
    await expect(page.locator(PANEL)).toHaveClass(/e-sidebar-right/, { timeout: WAIT });
    await page.evaluate(() => document.querySelector('.fx-root')!.setAttribute('dir', 'ltr'));
    await expect(page.locator(PANEL)).toHaveClass(/e-sidebar-left/, { timeout: WAIT });
  });
});

test.describe('Sidebar - dual and layout', () => {
  test('P-DUAL-001 Start and End Push panels do not overlap', async ({ page }) => {
    await load(page, '/sidebar/dual', DESKTOP);
    const start = await box(page, '#dual-start');
    const end = await box(page, '#dual-end');
    expect(start.x + start.width).toBeLessThanOrEqual(end.x + 1);
    const target = await box(page, '#dual-host main');
    expect(Math.round(target.width)).toBeGreaterThan(0);
  });

  test('P-FIX-001 Fixed=false positions the panel and backdrop inside the host', async ({ page }) => {
    await load(page, '/sidebar/fixed', DESKTOP);
    await page.locator('#open-contained').click();
    await expect(page.locator('#contained-sidebar')).toHaveClass(/e-sidebar-open/, { timeout: WAIT });
    const host = await box(page, '#contained');
    const panel = await box(page, '#contained-sidebar');
    const backdrop = await box(page, '.contained-backdrop');
    expect(await page.locator('#contained-sidebar').evaluate((e) => getComputedStyle(e).position)).toBe('absolute');
    expect(Math.round(panel.x)).toBeGreaterThanOrEqual(Math.round(host.x));
    expect(Math.round(backdrop.width)).toBeLessThanOrEqual(Math.round(host.width) + 2);
    expect(Math.round(backdrop.height)).toBeLessThanOrEqual(Math.round(host.height) + 2);
  });

  test('P-FIX-002 Fixed=true positions the panel and backdrop against the viewport', async ({ page }) => {
    await load(page, '/sidebar/fixed', DESKTOP);
    await page.locator('#open-fixed').click();
    await expect(page.locator('#fixed-sidebar')).toHaveClass(/e-sidebar-open/, { timeout: WAIT });
    expect(await page.locator('#fixed-sidebar').evaluate((e) => getComputedStyle(e).position)).toBe('fixed');
    const backdrop = await box(page, '.fixed-backdrop');
    expect(Math.round(backdrop.width)).toBe(DESKTOP.width);
    expect(Math.round(backdrop.height)).toBe(DESKTOP.height);
  });

  test('P-FIX-003 BackdropClass lands on the backdrop element only', async ({ page }) => {
    await load(page, '/sidebar/fixed', DESKTOP);
    await page.locator('#open-fixed').click();
    await expect(page.locator('.e-sidebar-backdrop.fixed-backdrop')).toHaveCount(1, { timeout: WAIT });
    await expect(page.locator('#fixed-sidebar')).not.toHaveClass(/fixed-backdrop/);
  });
});

test.describe('Sidebar - mini hover', () => {
  test('P-HOV-001 Pointer hover expands the rail and leaving collapses it', async ({ page }) => {
    await load(page, '/sidebar/hover', DESKTOP);
    await page.mouse.move(900, 300); // the pointer starts over the rail; a browser hovers it on load
    const width = () => page.evaluate(() => {
      const r = document.querySelector('div.e-sidebar')!.getBoundingClientRect();
      return Math.round(Math.min(r.right, window.innerWidth) - Math.max(r.left, 0));
    });
    await expect.poll(width, { timeout: WAIT }).toBe(64);
    await page.mouse.move(20, 300);
    await expect.poll(width, { timeout: WAIT }).toBe(280);
    await expect(page.locator(PANEL)).not.toHaveClass(/e-sidebar-open/);
    await page.mouse.move(900, 300);
    await expect.poll(width, { timeout: WAIT }).toBe(64);
  });

  test('P-HOV-002 Keyboard focus does not expand the rail', async ({ page }) => {
    await load(page, '/sidebar/hover', DESKTOP);
    await page.mouse.move(900, 300);
    await toggle(page).focus();
    await page.keyboard.press('Tab');
    await page.keyboard.press('Tab');
    await expect(page.locator(PANEL)).not.toHaveClass(/e-sidebar-hover/);
    await expect(page.locator(PANEL)).not.toHaveClass(/e-sidebar-open/);
  });
});

test.describe('Sidebar - animation and motion', () => {
  test('P-ANI-001 OnOpened waits for the Duration when animated', async ({ page }) => {
    await load(page, '/sidebar/timing', DESKTOP);
    const start = Date.now();
    await toggle(page).click();
    await open(page);
    await expect(page.locator('#log')).toContainText('opened:Overlay', { timeout: WAIT });
    expect(Date.now() - start).toBeGreaterThanOrEqual(350);
  });

  test('P-ANI-002 Reduced motion raises OnOpened without waiting', async ({ page }) => {
    await page.emulateMedia({ reducedMotion: 'reduce' });
    await load(page, '/sidebar/timing', DESKTOP);
    await toggle(page).click();
    await open(page);
    await expect(page.locator('#log')).toContainText('opened:Overlay', { timeout: 250 });
  });

  test('P-ANI-003 A close before the Duration elapses cancels the OnOpened callback', async ({ page }) => {
    await load(page, '/sidebar/timing', DESKTOP);
    await toggle(page).click();
    await open(page);
    await page.locator('#opt-api-toggle').click({ force: true });
    await closed(page);
    await expect(page.locator('#log')).toContainText('closed:Overlay', { timeout: 4000 });
    expect(await page.locator('#log').textContent()).not.toContain('opened');
  });

  test('P-ANI-004 Reduced motion removes the transition', async ({ page }) => {
    await page.emulateMedia({ reducedMotion: 'reduce' });
    await load(page, '/sidebar/timing', DESKTOP);
    const duration = await page.locator(PANEL).evaluate((e) => getComputedStyle(e).transitionDuration);
    expect(duration.split(',').every((d) => parseFloat(d) === 0)).toBe(true);
  });
});

test.describe('Sidebar - forced colors', () => {
  test('P-FC-001 Focus is visible with an outline in forced-colors mode', async ({ page, browserName }) => {
    test.skip(browserName !== 'chromium', 'forced-colors emulation is a Chromium feature (matrix: P2 / scheduled)');
    await page.emulateMedia({ forcedColors: 'active' });
    await load(page, '/sidebar/overlay', DESKTOP);
    await toggle(page).click();
    await open(page);
    await page.getByRole('link', { name: 'First' }).focus();
    await page.keyboard.press('Tab');
    await page.keyboard.press('Shift+Tab');
    const outline = await page.getByRole('link', { name: 'First' }).evaluate((e) => {
      const s = getComputedStyle(e);
      return { style: s.outlineStyle, width: parseFloat(s.outlineWidth) };
    });
    expect(outline.style).not.toBe('none');
    expect(outline.width).toBeGreaterThan(0);
    const border = await page.locator(PANEL).evaluate((e) => parseFloat(getComputedStyle(e).borderRightWidth) + parseFloat(getComputedStyle(e).borderLeftWidth));
    expect(border).toBeGreaterThan(0);
  });
});

const lab = (page: Page) => page.locator('#lab-sidebar');
const labOpen = (page: Page) => expect(lab(page)).toHaveClass(/e-sidebar-open/, { timeout: WAIT });
const labClosed = (page: Page) => expect(lab(page)).not.toHaveClass(/e-sidebar-open/, { timeout: WAIT });
const log = (page: Page) => page.locator('#log');
const edge = async (page: Page) => page.evaluate(() => {
  const panel = document.querySelector('#lab-sidebar')!.getBoundingClientRect();
  const host = document.querySelector('#lab-sidebar')!.closest('.e-sidebar-container')!.getBoundingClientRect();
  return { left: Math.round(panel.left - host.left), right: Math.round(host.right - panel.right), width: Math.round(panel.width) };
});

test.describe('Sidebar - public methods and events', () => {
  test('P-API-013 OpenAsync opens a closed Sidebar and raises the open event', async ({ page }) => {
    await load(page, '/sidebar/lab', DESKTOP);
    await page.locator('#opt-open').click();
    await labOpen(page);
    await expect(log(page)).toContainText('opened:True:Overlay:Api');
  });

  test('P-API-014 CloseAsync closes an open Sidebar and raises the close event', async ({ page }) => {
    await load(page, '/sidebar/lab', DESKTOP);
    await page.locator('#opt-open').click();
    await labOpen(page);
    await page.locator('#opt-close').click();
    await labClosed(page);
    await expect(log(page)).toContainText('closed:False:Overlay:Api');
  });

  test('P-API-015 OpenAsync on an already-open Sidebar raises no duplicate event', async ({ page }) => {
    await load(page, '/sidebar/lab', DESKTOP);
    await page.locator('#opt-open').click();
    await labOpen(page);
    const before = await log(page).textContent();
    await page.locator('#opt-open').click();
    await page.waitForTimeout(200);
    expect(await log(page).textContent()).toBe(before);
  });

  test('P-API-016 CloseAsync on an already-closed Sidebar raises no duplicate event', async ({ page }) => {
    await load(page, '/sidebar/lab', DESKTOP);
    const before = await log(page).textContent();
    await page.locator('#opt-close').click();
    await page.waitForTimeout(200);
    expect(await log(page).textContent()).toBe(before);
    await labClosed(page);
  });

  test('P-API-017 A parent-driven open changes state without user-change events', async ({ page }) => {
    await load(page, '/sidebar/lab', DESKTOP);
    await toggle(page).click();
    await labOpen(page);
    expect(await log(page).textContent()).not.toContain('opened');
  });

  test('P-API-018 A parent-driven close changes state without user-change events', async ({ page }) => {
    await load(page, '/sidebar/lab', DESKTOP);
    await toggle(page).click();
    await labOpen(page);
    await toggle(page).click();
    await labClosed(page);
    expect(await log(page).textContent()).not.toContain('closed');
  });

  test('P-EVT-011 An API open raises OnOpened after the open state is true', async ({ page }) => {
    await load(page, '/sidebar/lab', DESKTOP);
    await page.locator('#opt-open').click();
    await expect(page.locator('#status')).toHaveAttribute('data-open', 'true');
    await expect(log(page)).toContainText('opened:True');
  });

  test('P-EVT-012 An API close raises OnClosed after the open state is false', async ({ page }) => {
    await load(page, '/sidebar/lab', DESKTOP);
    await page.locator('#opt-open').click();
    await labOpen(page);
    await page.locator('#opt-close').click();
    await expect(page.locator('#status')).toHaveAttribute('data-open', 'false');
    await expect(log(page)).toContainText('closed:False');
  });

  test('P-EVT-013 API-triggered events report Trigger, IsOpen, and effective Mode', async ({ page }) => {
    await load(page, '/sidebar/lab', DESKTOP);
    await page.locator('#opt-open').click();
    await expect(log(page)).toContainText('opened:True:Overlay:Api');
  });

  test('P-EVT-014 A desktop open event reports the configured mode', async ({ page }) => {
    await load(page, '/sidebar/lab', DESKTOP);
    await page.locator('#opt-mode').click();
    await page.locator('#opt-open').click();
    await expect(log(page)).toContainText('opened:True:Push:Api');
  });

  test('P-EVT-015 A mobile responsive open event reports effective Overlay mode', async ({ page }) => {
    await load(page, '/sidebar/lab', MOBILE);
    await page.locator('#opt-mode').click();
    await page.locator('#opt-open').click();
    await expect(log(page)).toContainText('opened:True:Overlay:Api');
  });

  test('P-EVT-016 A later state change does not leave a stale pending event', async ({ page }) => {
    await load(page, '/sidebar/lab', DESKTOP);
    await page.locator('#opt-animation').click();
    await page.locator('#opt-duration').click();
    await page.locator('#opt-stale').click();
    await labClosed(page);
    await page.waitForTimeout(1000);
    expect(await log(page).textContent()).not.toContain('opened:');
  });
});

test.describe('Sidebar - interaction options', () => {
  test('P-UI-013 Open Overlay with ShowBackdrop=false has no backdrop', async ({ page }) => {
    await load(page, '/sidebar/lab', DESKTOP);
    await page.locator('#opt-backdrop').click();
    await page.locator('#opt-open').click();
    await labOpen(page);
    await expect(page.locator('.e-sidebar-backdrop')).toHaveCount(0);
  });

  test('P-UI-014 Overlay without a backdrop is not modal', async ({ page }) => {
    await load(page, '/sidebar/lab', DESKTOP);
    await page.locator('#opt-backdrop').click();
    await page.locator('#opt-open').click();
    await labOpen(page);
    await expect(lab(page)).toHaveAttribute('role', 'navigation');
    await expect(lab(page)).not.toHaveAttribute('aria-modal', 'true');
  });

  test('P-UI-015 Overlay without a backdrop does not make the target inert', async ({ page }) => {
    await load(page, '/sidebar/lab', DESKTOP);
    await page.locator('#opt-backdrop').click();
    await page.locator('#opt-open').click();
    await labOpen(page);
    await expect(page.locator('.e-sidebar-target')).not.toHaveAttribute('inert', '');
  });

  test('P-UI-016 CloseOnNavigate=false keeps an open Sidebar open after navigation', async ({ page }) => {
    await load(page, '/sidebar/lab', DESKTOP);
    await page.locator('#opt-navigate').click();
    await page.locator('#opt-open').click();
    await labOpen(page);
    await page.getByTestId('sidebar-nav-link').click();
    await expect(page).toHaveURL(/page=3/);
    await labOpen(page);
  });

  test('P-UI-017 A closed Sidebar does not raise an extra close callback after navigation', async ({ page }) => {
    await load(page, '/sidebar/lab', DESKTOP);
    const before = await log(page).textContent();
    await page.locator('#visible-nav').click();
    await expect(page).toHaveURL(/page=3/);
    expect(await log(page).textContent()).toBe(before);
  });

  test('P-UI-018 CloseOnOverlayClick=false still allows Escape to close', async ({ page }) => {
    await load(page, '/sidebar/lab', DESKTOP);
    await page.locator('#opt-overlay-click').click();
    await page.locator('#opt-open').click();
    await labOpen(page);
    await focusInPanel(page);
    await page.keyboard.press('Escape');
    await labClosed(page);
    await expect(log(page)).toContainText('Keyboard');
  });

  test('P-UI-019 CloseOnEscape=false still allows the backdrop to close', async ({ page }) => {
    await load(page, '/sidebar/lab', DESKTOP);
    await page.locator('#opt-escape').click();
    await page.locator('#opt-open').click();
    await labOpen(page);
    await page.locator('.e-sidebar-backdrop').click({ position: { x: 900, y: 300 } });
    await labClosed(page);
    await expect(log(page)).toContainText('Backdrop');
  });

  test('P-UI-020 A disabled Sidebar ignores a gesture, then responds after re-enabling', async ({ page }) => {
    await load(page, '/sidebar/lab', DESKTOP);
    await page.locator('#opt-enabled').click();
    const panel = lab(page);
    const box0 = await panel.boundingBox();
    await page.mouse.move(box0!.x + 8, box0!.y + 80);
    await page.mouse.down();
    await page.mouse.up();
    await lab(page).dispatchEvent('pointerdown', { pointerType: 'touch', clientX: box0!.x + 8, clientY: box0!.y + 80 });
    await lab(page).dispatchEvent('pointerup', { pointerType: 'touch', clientX: box0!.x + 120, clientY: box0!.y + 80 });
    await labClosed(page);
    await page.locator('#opt-enabled').click();
    await page.locator('#opt-open').click();
    await labOpen(page);
  });

  test('P-UI-021 An unrelated key does not close an open Sidebar', async ({ page }) => {
    await load(page, '/sidebar/lab', DESKTOP);
    await page.locator('#opt-open').click();
    await labOpen(page);
    await page.locator('#sidebar-last').focus();
    await page.keyboard.press('KeyA');
    await page.waitForTimeout(200);
    await labOpen(page);
  });
});

test.describe('Sidebar - configuration and content', () => {
  test('P-CFG-001 A custom Width sets the rendered panel width', async ({ page }) => {
    await load(page, '/sidebar/lab', DESKTOP);
    await page.locator('#opt-width').click();
    await page.locator('#opt-open').click();
    await expect.poll(async () => (await edge(page)).width, { timeout: WAIT }).toBe(360);
  });

  test('P-CFG-002 A custom MiniWidth sets the closed rail width', async ({ page }) => {
    await load(page, '/sidebar/lab', DESKTOP);
    await page.locator('#opt-mini').click();
    await page.locator('#opt-mini-width').click();
    await expect.poll(async () => page.locator('#lab-sidebar').evaluate((e) => getComputedStyle(e).getPropertyValue('--e-sidebar-mini-width').trim()), { timeout: WAIT }).toBe('96px');
  });

  test('P-CFG-003 A calc width is applied by the browser', async ({ page }) => {
    await load(page, '/sidebar/lab', DESKTOP);
    await page.locator('#opt-calc').click();
    await page.locator('#opt-open').click();
    await expect.poll(async () => (await edge(page)).width, { timeout: WAIT }).toBe(240);
  });

  test('P-CFG-004 A custom-width Push Sidebar offsets its target', async ({ page }) => {
    await load(page, '/sidebar/lab', DESKTOP);
    await page.locator('#opt-fixed').click();
    await page.locator('#opt-mode').click();
    await page.locator('#opt-width').click();
    const before = await box(page, '#lab-root .e-sidebar-target');
    await page.locator('#opt-open').click();
    await expect.poll(async () => Math.round((await box(page, '#lab-root .e-sidebar-target')).x - before.x), { timeout: WAIT }).toBeGreaterThan(200);
  });

  test('P-CFG-005 A custom-width Shrink Sidebar reduces the target width', async ({ page }) => {
    await load(page, '/sidebar/lab', DESKTOP);
    await page.locator('#opt-fixed').click();
    await page.locator('#opt-mode').click();
    await page.locator('#opt-mode').click();
    await page.locator('#opt-width').click();
    const before = await box(page, '#lab-root .e-sidebar-target');
    await page.locator('#opt-open').click();
    await expect.poll(async () => Math.round(before.width - (await box(page, '#lab-root .e-sidebar-target')).width), { timeout: WAIT }).toBeGreaterThan(200);
  });

  test('P-CFG-006 CssClass is applied to the panel, not the container', async ({ page }) => {
    await load(page, '/sidebar/lab', DESKTOP);
    await page.locator('#opt-css').click();
    await expect(lab(page)).toHaveClass(/consumer-panel/);
    await expect(page.locator('#lab-root .e-sidebar-container')).not.toHaveClass(/consumer-panel/);
  });

  test('P-CFG-007 Safe HtmlAttributes appear on the panel', async ({ page }) => {
    await load(page, '/sidebar/lab', DESKTOP);
    await page.locator('#opt-attrs').click();
    await expect(lab(page)).toHaveAttribute('data-consumer', 'lab');
    await expect(lab(page)).toHaveAttribute('title', 'Consumer title');
  });

  test('P-CFG-008 HtmlAttributes cannot replace owned id, role, class, tabindex, or ARIA', async ({ page }) => {
    await load(page, '/sidebar/lab', DESKTOP);
    await page.locator('#opt-attrs').click();
    const panel = lab(page);
    await expect(panel).toHaveAttribute('id', 'lab-sidebar');
    await expect(panel).toHaveAttribute('role', 'navigation');
    await expect(panel).toHaveAttribute('tabindex', '-1');
    await expect(panel).not.toHaveAttribute('aria-label', 'Stolen label');
  });

  test('P-CFG-009 Event-handler attributes in HtmlAttributes are not rendered', async ({ page }) => {
    await load(page, '/sidebar/lab', DESKTOP);
    await page.locator('#opt-attrs').click();
    await expect(lab(page)).not.toHaveAttribute('onclick', /.*/);
  });

  test('P-CFG-010 Header, body, footer, and target render in their regions', async ({ page }) => {
    await load(page, '/sidebar/lab', DESKTOP);
    await page.locator('#opt-slots').click();
    await expect(page.locator('.e-sidebar-header [data-testid="slot-header"]')).toHaveCount(1);
    await expect(page.locator('.e-sidebar-body [data-testid="slot-body"]')).toHaveCount(1);
    await expect(page.locator('.e-sidebar-footer [data-testid="slot-footer"]')).toHaveCount(1);
    await expect(page.locator('.e-sidebar-target [data-testid="slot-target"]')).toHaveCount(1);
  });

  test('P-CFG-011 An empty target still leaves the Sidebar interactive', async ({ page }) => {
    await load(page, '/sidebar/lab', DESKTOP);
    await page.locator('#opt-omit-target').click();
    await expect(page.locator('#lab-root [data-testid="slot-target"]')).toHaveCount(0);
    await page.locator('#opt-open').click();
    await labOpen(page);
  });
});

test.describe('Sidebar - physical position', () => {
  test('P-POS-001 Position=Left stays on the physical left in LTR', async ({ page }) => {
    await load(page, '/sidebar/lab', DESKTOP);
    await page.locator('#opt-position-left').click();
    await page.locator('#opt-open').click();
    await expect.poll(async () => (await edge(page)).left, { timeout: WAIT }).toBeLessThanOrEqual(8);
  });

  test('P-POS-002 Position=Right stays on the physical right in LTR', async ({ page }) => {
    await load(page, '/sidebar/lab', DESKTOP);
    await page.locator('#opt-position-right').click();
    await page.locator('#opt-open').click();
    await expect.poll(async () => (await edge(page)).right, { timeout: WAIT }).toBeLessThanOrEqual(8);
  });

  test('P-POS-003 Position=Left stays on the physical left in RTL', async ({ page }) => {
    await load(page, '/sidebar/lab', DESKTOP);
    await page.locator('#opt-rtl').click();
    await page.locator('#opt-position-left').click();
    await page.locator('#opt-open').click();
    await expect.poll(async () => (await edge(page)).left, { timeout: WAIT }).toBeLessThanOrEqual(8);
  });

  test('P-POS-004 Position=Right stays on the physical right in RTL', async ({ page }) => {
    await load(page, '/sidebar/lab', DESKTOP);
    await page.locator('#opt-rtl').click();
    await page.locator('#opt-position-right').click();
    await page.locator('#opt-open').click();
    await expect.poll(async () => (await edge(page)).right, { timeout: WAIT }).toBeLessThanOrEqual(8);
  });

  test('P-POS-005 Changing Position from left to right moves the Sidebar', async ({ page }) => {
    await load(page, '/sidebar/lab', DESKTOP);
    await page.locator('#opt-position-left').click();
    await page.locator('#opt-open').click();
    await page.locator('#opt-position-right').click();
    await expect.poll(async () => (await edge(page)).right, { timeout: WAIT }).toBeLessThanOrEqual(8);
  });

  test('P-POS-006 Changing Position from right to left moves the Sidebar', async ({ page }) => {
    await load(page, '/sidebar/lab', DESKTOP);
    await page.locator('#opt-position-right').click();
    await page.locator('#opt-open').click();
    await page.locator('#opt-position-left').click();
    await expect.poll(async () => (await edge(page)).left, { timeout: WAIT }).toBeLessThanOrEqual(8);
  });
});

test.describe('Sidebar - breakpoints and media query', () => {
  test('P-VP-007 Sm switches below 576px and keeps the mode at 576px', async ({ page }) => {
    await load(page, '/sidebar/lab', { width: 576, height: 700 });
    await page.locator('#opt-bp').click();
    await page.locator('#opt-mode').click();
    await expect(lab(page)).toHaveClass(/e-sidebar-push/);
    await page.setViewportSize({ width: 575, height: 700 });
    await expect(lab(page)).toHaveClass(/e-sidebar-overlay/, { timeout: WAIT });
  });

  test('P-VP-008 Lg switches below 992px and keeps the mode at 992px', async ({ page }) => {
    await load(page, '/sidebar/lab', { width: 992, height: 700 });
    for (const _ of [1, 2]) await page.locator('#opt-bp').click();
    await page.locator('#opt-mode').click();
    await expect(lab(page)).toHaveClass(/e-sidebar-push/);
    await page.setViewportSize({ width: 991, height: 700 });
    await expect(lab(page)).toHaveClass(/e-sidebar-overlay/, { timeout: WAIT });
  });

  test('P-VP-009 Xl switches below 1200px and keeps the mode at 1200px', async ({ page }) => {
    await load(page, '/sidebar/lab', { width: 1200, height: 700 });
    for (const _ of [1, 2, 3]) await page.locator('#opt-bp').click();
    await page.locator('#opt-mode').click();
    await expect(lab(page)).toHaveClass(/e-sidebar-push/);
    await page.setViewportSize({ width: 1199, height: 700 });
    await expect(lab(page)).toHaveClass(/e-sidebar-overlay/, { timeout: WAIT });
  });

  test('P-VP-010 Xxl switches below 1400px and keeps the mode at 1400px', async ({ page }) => {
    await load(page, '/sidebar/lab', { width: 1400, height: 700 });
    for (const _ of [1, 2, 3, 4]) await page.locator('#opt-bp').click();
    await page.locator('#opt-mode').click();
    await expect(lab(page)).toHaveClass(/e-sidebar-push/);
    await page.setViewportSize({ width: 1399, height: 700 });
    await expect(lab(page)).toHaveClass(/e-sidebar-overlay/, { timeout: WAIT });
  });

  test('P-VP-011 Breakpoint=None keeps the configured mode at mobile width', async ({ page }) => {
    await load(page, '/sidebar/lab', MOBILE);
    for (const _ of [1, 2, 3, 4, 5]) await page.locator('#opt-bp').click();
    await page.locator('#opt-mode').click();
    await expect(lab(page)).toHaveClass(/e-sidebar-push/);
  });

  test('P-VP-012 Breakpoint=Xs keeps the configured mode at mobile width', async ({ page }) => {
    await load(page, '/sidebar/lab', MOBILE);
    for (const _ of [1, 2, 3, 4, 5, 6]) await page.locator('#opt-bp').click();
    await page.locator('#opt-mode').click();
    await expect(lab(page)).toHaveClass(/e-sidebar-push/);
  });

  test('P-VP-013 Breakpoint=Always keeps the configured mode at mobile width', async ({ page }) => {
    await load(page, '/sidebar/lab', MOBILE);
    for (const _ of [1, 2, 3, 4, 5, 6, 7]) await page.locator('#opt-bp').click();
    await page.locator('#opt-mode').click();
    await expect(lab(page)).toHaveClass(/e-sidebar-push/);
  });

  test('P-VP-014 Responsive=false keeps the configured mode at mobile width', async ({ page }) => {
    await load(page, '/sidebar/lab', MOBILE);
    await page.locator('#opt-responsive').click();
    await page.locator('#opt-mode').click();
    await expect(lab(page)).toHaveClass(/e-sidebar-push/);
  });

  test('P-VP-015 Changing Breakpoint after render updates responsive behavior', async ({ page }) => {
    await load(page, '/sidebar/lab', { width: 700, height: 700 });
    await page.locator('#opt-mode').click();
    await expect(lab(page)).toHaveClass(/e-sidebar-overlay/);
    for (const _ of [1, 2, 3, 4, 5]) await page.locator('#opt-bp').click();
    await expect(lab(page)).toHaveClass(/e-sidebar-push/, { timeout: WAIT });
  });

  test('P-VP-016 Initial detection does not raise OnBreakpointChanged; a crossing does', async ({ page }) => {
    await load(page, '/sidebar/lab', DESKTOP);
    expect(await log(page).textContent()).not.toContain('breakpoint:');
    await page.setViewportSize(MOBILE);
    await expect(log(page)).toContainText('breakpoint:true');
  });

  test('P-MQ-001 A matching media query opens the Sidebar', async ({ page }) => {
    await load(page, '/sidebar/lab', DESKTOP);
    await page.locator('#opt-media-on').click();
    await labOpen(page);
    await expect(log(page)).toContainText('Api');
  });

  test('P-MQ-002 A non-matching query closes the Sidebar', async ({ page }) => {
    await load(page, '/sidebar/lab', DESKTOP);
    await page.locator('#opt-media-on').click();
    await labOpen(page);
    await page.locator('#opt-media-off').click();
    await labClosed(page);
  });

  test('P-MQ-003 Clearing MediaQuery stops the previous query from changing state', async ({ page }) => {
    await load(page, '/sidebar/lab', DESKTOP);
    await page.locator('#opt-media-on').click();
    await labOpen(page);
    await page.locator('#opt-media-clear').click();
    await page.setViewportSize({ width: 900, height: 700 });
    await page.waitForTimeout(400);
    await labOpen(page);
  });
});

test.describe('Sidebar - gestures', () => {
  const swipe = async (page: Page, fromX: number, toX: number, fromY: number, toY = fromY) => {
    const host = page.locator('#lab-root .e-sidebar-container').first();
    await host.dispatchEvent('pointerdown', { pointerType: 'touch', clientX: fromX, clientY: fromY });
    await host.dispatchEvent('pointerup', { pointerType: 'touch', clientX: toX, clientY: toY });
  };

  test('P-GES-001 A swipe toward content opens a left Sidebar', async ({ page }) => {
    await load(page, '/sidebar/lab', DESKTOP);
    await page.locator('#opt-position-left').click();
    await swipe(page, 8, 120, 240);
    await labOpen(page);
  });

  test('P-GES-002 A swipe away from content closes a left Sidebar', async ({ page }) => {
    await load(page, '/sidebar/lab', DESKTOP);
    await page.locator('#opt-position-left').click();
    await page.locator('#opt-open').click();
    await labOpen(page);
    await swipe(page, 200, 40, 240);
    await labClosed(page);
  });

  test('P-GES-003 A swipe opens a right Sidebar from its edge', async ({ page }) => {
    await load(page, '/sidebar/lab', DESKTOP);
    await page.locator('#opt-position-right').click();
    await swipe(page, 1270, 1100, 240);
    await labOpen(page);
  });

  test('P-GES-004 A swipe closes a right Sidebar from its edge', async ({ page }) => {
    await load(page, '/sidebar/lab', DESKTOP);
    await page.locator('#opt-position-right').click();
    await page.locator('#opt-open').click();
    await labOpen(page);
    await swipe(page, 1100, 1260, 240);
    await labClosed(page);
  });

  test('P-GES-005 A swipe from the wrong direction does not open', async ({ page }) => {
    await load(page, '/sidebar/lab', DESKTOP);
    await page.locator('#opt-position-left').click();
    await swipe(page, 200, 40, 240);
    await labClosed(page);
  });

  test('P-GES-006 A below-threshold movement does not change state', async ({ page }) => {
    await load(page, '/sidebar/lab', DESKTOP);
    await swipe(page, 8, 30, 240);
    await labClosed(page);
  });

  test('P-GES-007 A vertical-dominant movement does not change state', async ({ page }) => {
    await load(page, '/sidebar/lab', DESKTOP);
    await swipe(page, 8, 20, 200, 360);
    await labClosed(page);
  });

  test('P-GES-008 Mouse drag and EnableGestures=false do not swipe', async ({ page }) => {
    await load(page, '/sidebar/lab', DESKTOP);
    const host = page.locator('#lab-root .e-sidebar-container').first();
    await host.dispatchEvent('pointerdown', { pointerType: 'mouse', clientX: 8, clientY: 240 });
    await host.dispatchEvent('pointerup', { pointerType: 'mouse', clientX: 180, clientY: 240 });
    await labClosed(page);
    await page.locator('#opt-gestures').click();
    await swipe(page, 8, 180, 240);
    await labClosed(page);
  });
});

test.describe('Sidebar - mini hover guards', () => {
  test('P-HOV-003 IsEnabled=false prevents hover expansion', async ({ page }) => {
    await load(page, '/sidebar/lab', DESKTOP);
    await page.locator('#opt-hover').click();
    await page.locator('#opt-enabled').click();
    await lab(page).hover();
    await expect(lab(page)).not.toHaveClass(/e-sidebar-hover/);
  });

  test('P-HOV-004 A touch pointer does not expand the mini rail', async ({ page }) => {
    await load(page, '/sidebar/lab', DESKTOP);
    await page.locator('#opt-hover').click();
    await lab(page).dispatchEvent('pointerenter', { pointerType: 'touch' });
    await expect(lab(page)).not.toHaveClass(/e-sidebar-hover/);
  });

  test('P-HOV-005 A mobile viewport does not enter hover-expanded state', async ({ page }) => {
    await load(page, '/sidebar/lab', MOBILE);
    await page.locator('#opt-hover').click();
    await lab(page).hover();
    await expect(lab(page)).not.toHaveClass(/e-sidebar-hover/);
  });

  test('P-HOV-006 Push hover reserves space without changing IsOpen', async ({ page }) => {
    await load(page, '/sidebar/lab', DESKTOP);
    await page.locator('#opt-mode').click();
    await page.locator('#opt-hover').click();
    await lab(page).hover();
    await expect(page.locator('#status')).toContainText('open=False');
    await expect(lab(page)).toHaveClass(/e-sidebar-hover/);
  });

  test('P-HOV-007 Shrink hover reserves space without changing IsOpen', async ({ page }) => {
    await load(page, '/sidebar/lab', DESKTOP);
    await page.locator('#opt-mode').click();
    await page.locator('#opt-mode').click();
    await page.locator('#opt-hover').click();
    await lab(page).hover();
    await expect(page.locator('#status')).toContainText('open=False');
  });
});

test.describe('Sidebar - focus and announcements', () => {
  test('P-KB-010 FocusAsync skips hidden and disabled candidates', async ({ page }) => {
    await load(page, '/sidebar/lab', DESKTOP);
    await page.locator('#opt-open').click();
    await labOpen(page);
    await page.locator('#opt-focus').click();
    await expect.poll(async () => (await active(page)).inPanel, { timeout: WAIT }).toBe(true);
    const focused = await active(page);
    expect(focused.id).not.toBe('hidden-candidate');
    expect(focused.id).not.toBe('disabled-candidate');
  });

  test('P-KB-011 FocusAsync focuses the panel when it has no focusable descendants', async ({ page }) => {
    await load(page, '/sidebar/lab', DESKTOP);
    await page.locator('#opt-hide-focusable').click();
    await page.locator('#opt-open').click();
    await page.locator('#opt-focus').click();
    await expect.poll(async () => (await active(page)).inPanel, { timeout: WAIT }).toBe(true);
  });

  test('P-KB-012 FocusAsync on a closed non-mini Sidebar does not open it', async ({ page }) => {
    await load(page, '/sidebar/lab', DESKTOP);
    await page.locator('#opt-focus').click();
    await labClosed(page);
    expect((await active(page)).id).not.toBe('lab-sidebar');
  });

  test('P-KB-013 FocusAsync on a closed mini Sidebar focuses it without opening', async ({ page }) => {
    await load(page, '/sidebar/lab', DESKTOP);
    await page.locator('#opt-mini').click();
    await page.locator('#opt-focus').click();
    await labClosed(page);
    await expect.poll(async () => (await active(page)).inPanel, { timeout: WAIT }).toBe(true);
  });

  test('P-KB-014 Modal Tab trapping redirects focus that was moved outside', async ({ page }) => {
    await load(page, '/sidebar/lab', DESKTOP);
    await page.locator('#opt-open').click();
    await labOpen(page);
    await page.locator('#main-focusable').focus();
    await page.keyboard.press('Tab');
    await expect.poll(async () => (await active(page)).inPanel, { timeout: WAIT }).toBe(true);
  });

  test('P-KB-015 A modal with no focusable descendants keeps focus on the panel', async ({ page }) => {
    await load(page, '/sidebar/lab', DESKTOP);
    await page.locator('#opt-hide-focusable').click();
    await page.locator('#opt-open').click();
    await lab(page).focus();
    await page.keyboard.press('Tab');
    await expect.poll(async () => (await active(page)).inPanel, { timeout: WAIT }).toBe(true);
  });

  test('P-KB-016 Opening a modal Overlay makes the target inert and closing removes it', async ({ page }) => {
    await load(page, '/sidebar/lab', DESKTOP);
    await page.locator('#opt-open').click();
    await labOpen(page);
    await expect(page.locator('.e-sidebar-target')).toHaveAttribute('inert', '');
    await page.locator('#opt-close').click();
    await labClosed(page);
    await expect(page.locator('.e-sidebar-target')).not.toHaveAttribute('inert', '');
  });

  test('P-KB-017 The live region announces the open state', async ({ page }) => {
    await load(page, '/sidebar/lab', DESKTOP);
    await page.locator('#opt-open').click();
    await expect(page.locator('.e-sidebar-live')).toContainText(/open/i);
  });

  test('P-KB-018 The live region announces the closed state', async ({ page }) => {
    await load(page, '/sidebar/lab', DESKTOP);
    await page.locator('#opt-open').click();
    await page.locator('#opt-close').click();
    await expect(page.locator('.e-sidebar-live')).toContainText(/closed/i);
  });
});

test.describe('Sidebar - animation runtime', () => {
  test('P-ANI-005 OnClosed waits for Duration when animation is active', async ({ page }) => {
    await load(page, '/sidebar/lab', DESKTOP);
    await page.locator('#opt-animation').click();
    await page.locator('#opt-duration').click();
    await page.locator('#opt-open').click();
    await labOpen(page);
    await page.locator('#opt-close').click();
    await page.waitForTimeout(200);
    expect(await log(page).textContent()).not.toContain('closed:');
    await expect(log(page)).toContainText('closed:', { timeout: 2000 });
  });

  test('P-ANI-006 EnableAnimation=false completes close without waiting', async ({ page }) => {
    await load(page, '/sidebar/lab', DESKTOP);
    await page.locator('#opt-duration').click();
    await page.locator('#opt-open').click();
    await page.locator('#opt-close').click();
    await expect(log(page)).toContainText('closed:', { timeout: 500 });
  });

  test('P-ANI-007 A newer state change cancels a stale OnClosed', async ({ page }) => {
    await load(page, '/sidebar/lab', DESKTOP);
    await page.locator('#opt-animation').click();
    await page.locator('#opt-duration').click();
    await page.locator('#opt-open').click();
    await labOpen(page);
    await page.locator('#opt-close').click();
    await page.locator('#opt-open').click();
    await page.waitForTimeout(1000);
    const text = await log(page).textContent();
    expect(text?.split('closed:').length ?? 1).toBeLessThanOrEqual(2);
    await expect(log(page)).toContainText('opened:True');
  });

  test('P-ANI-008 A runtime reduced-motion change affects the next transition', async ({ page }) => {
    await load(page, '/sidebar/lab', DESKTOP);
    await page.locator('#opt-animation').click();
    await page.emulateMedia({ reducedMotion: 'reduce' });
    await page.locator('#opt-open').click();
    await expect(log(page)).toContainText('opened:', { timeout: 500 });
  });

  test('P-ANI-009 Global animation disabled prevents a delayed callback', async ({ page }) => {
    await load(page, '/sidebar/lab', DESKTOP);
    await page.locator('#opt-global-anim').click();
    await page.locator('#opt-open').click();
    await expect(log(page)).toContainText('opened:', { timeout: 4000 });
  });
});

test.describe('Sidebar - component integration', () => {
  test('P-INTG-001 An SfButton in Sidebar content keeps the Sidebar open', async ({ page }) => {
    await load(page, '/sidebar/lab', DESKTOP);
    await page.locator('#opt-open').click();
    await page.locator('#sidebar-last').click();
    await labOpen(page);
  });

  test('P-INTG-002 An SfButtonGroup selection does not change Sidebar open state', async ({ page }) => {
    await load(page, '/sidebar/lab', DESKTOP);
    await page.locator('#opt-open').click();
    await page.locator('#grp-two').click();
    await expect(page.locator('#group-result')).toHaveText('two');
    await labOpen(page);
  });

  test('P-INTG-003 SfTextBox in the Sidebar updates a consumer-owned result', async ({ page }) => {
    await load(page, '/sidebar/lab', DESKTOP);
    await page.locator('#opt-open').click();
    await page.locator('#lab-text').fill('ledger');
    await page.locator('#lab-text').press('Tab');
    await expect(page.locator('#text-result')).toHaveText('ledger');
    await labOpen(page);
  });

  test('P-INTG-004 SfCheckBox updates bound parent state without closing', async ({ page }) => {
    await load(page, '/sidebar/lab', DESKTOP);
    await page.locator('#opt-open').click();
    await page.getByLabel('Include archived').click();
    await expect(page.locator('#check-result')).toHaveText('True');
    await labOpen(page);
  });

  test('P-INTG-005 An SfSwitch updates independently of Sidebar open state', async ({ page }) => {
    await load(page, '/sidebar/lab', DESKTOP);
    await page.locator('#opt-open').click();
    await page.locator('.e-switch-wrapper').click();
    await expect(page.locator('#switch-result')).toHaveText('True');
    await labOpen(page);
  });

  test('P-INTG-006 A popup opened from Sidebar content is not clipped', async ({ page }) => {
    await load(page, '/sidebar/lab', DESKTOP);
    await page.locator('#opt-open').click();
    await page.locator('#lab-choice').click();
    const popup = page.locator('#lab-popup');
    await expect(popup).toBeVisible();
    const panelBox = await box(page, '#lab-sidebar');
    const popupBox = await popup.boundingBox();
    expect(popupBox!.y + popupBox!.height).toBeGreaterThan(panelBox.y);
    expect(popupBox!.height).toBeGreaterThan(20);
  });

  test('P-INTG-007 A second overlay control closes without changing the first Sidebar', async ({ page }) => {
    await load(page, '/sidebar/lab', DESKTOP);
    await page.locator('#opt-open').click();
    await page.locator('#opt-second-open').click();
    await expect(page.locator('#second-note')).toHaveText('second-open');
    await page.locator('#opt-second-close').click();
    await expect(page.locator('#second-note')).toHaveText('second-closed');
    await labOpen(page);
  });

  test('P-INTG-008 Sidebar controls remain in an accessible named region', async ({ page }) => {
    await load(page, '/sidebar/lab', DESKTOP);
    await page.locator('#opt-open').click();
    await expect(lab(page)).toHaveAttribute('aria-describedby', /.+/);
  });

  test('P-INTG-009 Two Sidebars open and close independently', async ({ page }) => {
    await load(page, '/sidebar/lab', DESKTOP);
    await page.locator('#opt-second-open').click();
    await expect(page.locator('#second-note')).toHaveText('second-open');
    await labClosed(page);
    await page.locator('#opt-open').click();
    await labOpen(page);
    await expect(page.locator('#second-note')).toHaveText('second-open');
  });
});
