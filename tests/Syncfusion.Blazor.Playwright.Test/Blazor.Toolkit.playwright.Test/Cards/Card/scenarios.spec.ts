// Playwright coverage mapped to the P-CARD ids in .github/specs/Cards/TEST-MATRIX.md.
// Drives /card/scenarios (Scenarios.razor). The Card has no Disabled parameter, so V-CARD-002, 015 and 046 assert
// the closest real behaviour: a non-clickable card is skipped by Tab and ignores keyboard input.

import { test, expect, type Page } from '@playwright/test';

const SCENARIOS = 'http://localhost:5000/card/scenarios';
const INTERACTIONS = 'http://localhost:5000/card/interactions';

async function openScenarios(page: Page, url = SCENARIOS) {
  await page.goto(url, { waitUntil: 'commit' });
  await expect(page.locator('#scenarios-ready')).toHaveAttribute('data-ready', 'true', { timeout: 15000 });
}

async function openInteractions(page: Page, url = INTERACTIONS) {
  await page.goto(url, { waitUntil: 'commit' });
  await expect(page.locator('#interactive-ready')).toHaveAttribute('data-ready', 'true', { timeout: 15000 });
}

test.describe('Card core interactions @card', () => {
  test.beforeEach(async ({ page }) => {
    await openScenarios(page);
  });

  test('V-CARD-001 clickable card activation', async ({ page }) => {
    await page.locator('#pointer-card').click();
    await expect(page.locator('#pointer-count')).toHaveText('Pointer clicks: 1');
  });

  test('V-CARD-002 a non-clickable card ignores clicks and keys (no Disabled parameter exists)', async ({ page }) => {
    const card = page.locator('#static-card');
    await card.click();
    await page.keyboard.press('Enter');
    await expect(card).not.toHaveAttribute('role', /.+/);
    await expect(card).not.toHaveAttribute('tabindex', /.+/);
  });

  test('V-CARD-003 loading card interaction suppression', async ({ page }) => {
    await openInteractions(page);
    const card = page.locator('#card-loading');
    await expect(card).toHaveAttribute('aria-busy', 'true');
    await card.click({ force: true });
    await expect(page.locator('#loading-click-count')).toHaveText('Loading card clicks: 0');
  });

  test('V-CARD-004 multiple card interaction', async ({ page }) => {
    await page.locator('#collection-card-1').click();
    await page.locator('#collection-card-3').click();
    await page.locator('#collection-card-3').click();
    await expect(page.locator('#collection-counts')).toHaveText('1,0,2');
  });

  test('V-CARD-005 nested interactive content', async ({ page }) => {
    await page.locator('#nested-input').fill('hello');
    await expect(page.locator('#nested-input')).toHaveValue('hello');
    await page.locator('#nested-button').click();
    await expect(page.locator('#nested-counts')).toHaveText('outer:0 inner:1');
  });

  test('P-CARD-008 actions isolation: clicking an action button does not activate the card', async ({ page }) => {
    await page.locator('#nested-button').click();
    await page.locator('#nested-button').focus();
    await page.keyboard.press('Enter');
    await expect(page.locator('#nested-counts')).toHaveText('outer:0 inner:2');
  });

  test('V-CARD-007 mouse interaction', async ({ page }) => {
    await page.locator('#pointer-card').hover();
    await page.mouse.down();
    await page.mouse.up();
    await expect(page.locator('#pointer-count')).toHaveText('Pointer clicks: 1');
  });

  test('V-CARD-008 pointer interaction', async ({ page }) => {
    await page.locator('#pointer-card').scrollIntoViewIfNeeded();
    const box = (await page.locator('#pointer-card').boundingBox())!;
    const x = box.x + box.width / 2;
    const y = box.y + box.height / 2;
    await page.mouse.move(x, y);
    await page.mouse.click(x, y, { delay: 50 });
    await expect(page.locator('#pointer-count')).toHaveText('Pointer clicks: 1');
  });
});

test.describe('Card touch @card', () => {
  test.use({ hasTouch: true });

  test('V-CARD-006 touch interaction', async ({ page }) => {
    await openScenarios(page);
    await page.locator('#pointer-card').tap();
    await expect(page.locator('#pointer-count')).toHaveText('Pointer clicks: 1');
  });
});

test.describe('Card keyboard navigation @card', () => {
  test.beforeEach(async ({ page }) => {
    await openScenarios(page);
  });

  test('V-CARD-009 Tab enters card', async ({ page }) => {
    await page.locator('#order-before').focus();
    await page.keyboard.press('Tab');
    await expect(page.locator('#order-card')).toBeFocused();
  });

  test('V-CARD-010 Shift+Tab exits card', async ({ page }) => {
    await page.locator('#order-after').focus();
    await page.keyboard.press('Shift+Tab');
    await expect(page.locator('#order-card')).toBeFocused();
    await page.keyboard.press('Shift+Tab');
    await expect(page.locator('#order-before')).toBeFocused();
  });

  test('V-CARD-011 Enter activation', async ({ page }) => {
    await page.locator('#pointer-card').focus();
    await page.keyboard.press('Enter');
    await expect(page.locator('#pointer-count')).toHaveText('Pointer clicks: 1');
  });

  test('A-CARD-011 Space activation', async ({ page }) => {
    await page.locator('#pointer-card').focus();
    await page.keyboard.press('Space');
    await expect(page.locator('#pointer-count')).toHaveText('Pointer clicks: 1');
  });

  test('V-CARD-012 focus-visible indicator', async ({ page }) => {
    await page.locator('#order-before').focus();
    await page.keyboard.press('Tab');
    const card = page.locator('#order-card');
    await expect(card).toBeFocused();
    expect(await card.evaluate((el) => getComputedStyle(el).outlineWidth)).not.toBe('0px');
  });

  test('V-CARD-013 keyboard navigation order', async ({ page }) => {
    await page.locator('#order-before').focus();
    await page.keyboard.press('Tab');
    await expect(page.locator('#order-card')).toBeFocused();
    await page.keyboard.press('Tab');
    await expect(page.locator('#order-after')).toBeFocused();
  });

  test('V-CARD-014 a non-clickable card is skipped by Tab', async ({ page }) => {
    await page.locator('#static-card').scrollIntoViewIfNeeded();
    const focusable = await page.locator('#static-card').evaluate((el) => el.tabIndex);
    expect(focusable).toBe(-1);
  });

  test('V-CARD-015 multiple cards keyboard navigation', async ({ page }) => {
    await page.locator('#multi-a').focus();
    await page.keyboard.press('Tab');
    await expect(page.locator('#multi-b')).toBeFocused();
    await page.keyboard.press('Enter');
    await expect(page.locator('#multi-counts')).toHaveText('A:0 B:1');
    await page.keyboard.press('Shift+Tab');
    await expect(page.locator('#multi-a')).toBeFocused();
  });

  test('P-CARD-016 Enter activates on keydown', async ({ page }) => {
    await page.locator('#pointer-card').focus();
    await page.keyboard.down('Enter');
    await expect(page.locator('#pointer-count')).toHaveText('Pointer clicks: 1');
    await page.keyboard.up('Enter');
    await expect(page.locator('#pointer-count')).toHaveText('Pointer clicks: 1');
  });

  test('P-CARD-017 Space activates on keyup', async ({ page }) => {
    await page.locator('#pointer-card').focus();
    await page.keyboard.down(' ');
    await expect(page.locator('#pointer-count')).toHaveText('Pointer clicks: 0');
    await page.keyboard.up(' ');
    await expect(page.locator('#pointer-count')).toHaveText('Pointer clicks: 1');
  });

  test('P-CARD-018 actions buttons remain independently focusable', async ({ page }) => {
    await page.locator('#actions-first').focus();
    await expect(page.locator('#actions-first')).toBeFocused();
    await page.keyboard.press('Tab');
    await expect(page.locator('#actions-second')).toBeFocused();
  });
});

const viewports: Array<[string, string, { width: number; height: number }]> = [
  ['V-CARD-016', 'mobile portrait', { width: 375, height: 700 }],
  ['V-CARD-017', 'mobile landscape', { width: 700, height: 375 }],
  ['V-CARD-018', 'tablet', { width: 820, height: 1100 }],
  ['V-CARD-019', 'desktop', { width: 1280, height: 800 }],
  ['V-CARD-020', 'large desktop', { width: 1920, height: 1080 }],
];

test.describe('Card responsive @card', () => {
  for (const [id, name, size] of viewports) {
    test(`${id} ${name} layout has no horizontal overflow`, async ({ page }) => {
      await page.setViewportSize(size);
      await openScenarios(page);
      const overflow = await page.evaluate(() => document.documentElement.scrollWidth - document.documentElement.clientWidth);
      expect(overflow).toBeLessThanOrEqual(0);
      const card = await page.locator('#media-card').boundingBox();
      expect(card).not.toBeNull();
      expect(card!.x + card!.width).toBeLessThanOrEqual(size.width);
    });
  }

  test('V-CARD-021 horizontal card responsiveness', async ({ page }) => {
    await page.setViewportSize({ width: 375, height: 700 });
    await openInteractions(page);
    const card = await page.locator('#card-horizontal').boundingBox();
    expect(card).not.toBeNull();
    expect(card!.x + card!.width).toBeLessThanOrEqual(375);
  });

  test('V-CARD-022 media responsiveness', async ({ page }) => {
    await page.setViewportSize({ width: 375, height: 700 });
    await openScenarios(page);
    const card = await page.locator('#responsive-media-card').boundingBox();
    const image = await page.locator('#responsive-media-card img').boundingBox();
    expect(card).not.toBeNull();
    expect(image).not.toBeNull();
    expect(image!.width).toBeLessThanOrEqual(card!.width + 1);
  });

  test('V-CARD-023 overflow handling keeps the card width and clips an unbroken word', async ({ page }) => {
    await openScenarios(page);
    const card = page.locator('#overflow-card');
    expect(await card.evaluate((el) => getComputedStyle(el).overflow)).toBe('hidden');

    // The card keeps its declared width and does not push the page wider.
    const box = (await card.boundingBox())!;
    expect(Math.round(box.width)).toBe(160);
    const pageOverflow = await page.evaluate(() => document.documentElement.scrollWidth - document.documentElement.clientWidth);
    expect(pageOverflow).toBeLessThanOrEqual(0);
  });
});

test.describe('Card theme @card', () => {
  test('V-CARD-024 default theme renders the card', async ({ page }) => {
    await openScenarios(page);
    await expect(page.locator('#theme-card')).toBeVisible();
    await expect(page.locator('#scenario-stage')).toHaveAttribute('data-theme', 'light');
  });

  test('V-CARD-025 dark theme renders the card', async ({ page }) => {
    await openScenarios(page);
    await page.locator('#theme-toggle').click();
    await expect(page.locator('#scenario-stage')).toHaveAttribute('data-theme', 'dark');
    await expect(page.locator('#theme-card')).toBeVisible();
  });

  test('V-CARD-026 theme switch at runtime keeps the card mounted', async ({ page }) => {
    await openScenarios(page);
    const idBefore = await page.locator('#theme-card').getAttribute('id');
    await page.locator('#theme-toggle').click();
    await page.locator('#theme-toggle').click();
    await expect(page.locator('#scenario-stage')).toHaveAttribute('data-theme', 'light');
    expect(await page.locator('#theme-card').getAttribute('id')).toBe(idBefore);
  });

  test('V-CARD-027 consistent spacing across themes', async ({ page }) => {
    await openScenarios(page);
    const padding = () => page.locator('#theme-card [data-testid="card-content"]').evaluate((el) => getComputedStyle(el).paddingLeft);
    const light = await padding();
    await page.locator('#theme-toggle').click();
    expect(await padding()).toBe(light);
  });
});

test.describe('Card RTL @card', () => {
  test.beforeEach(async ({ page }) => {
    await openScenarios(page);
  });

  test('V-CARD-028 RTL rendering', async ({ page }) => {
    expect(await page.locator('#rtl-card').evaluate((el) => getComputedStyle(el).direction)).toBe('rtl');
  });

  test('V-CARD-029 RTL horizontal layout puts media on the right', async ({ page }) => {
    const media = await page.locator('#rtl-media').boundingBox();
    const content = await page.locator('#rtl-card [data-testid="card-content"]').boundingBox();
    expect(media!.x).toBeGreaterThan(content!.x);
  });

  test('V-CARD-030 RTL actions layout', async ({ page }) => {
    const first = await page.locator('#rtl-first').boundingBox();
    const second = await page.locator('#rtl-second').boundingBox();
    expect(first!.x).toBeGreaterThan(second!.x);
  });

  test('V-CARD-031 RTL keyboard navigation', async ({ page }) => {
    await page.locator('#rtl-clickable').focus();
    await page.keyboard.press('Enter');
    await expect(page.locator('#rtl-clickable')).toBeFocused();
  });

  test('V-CARD-032 RTL focus handling', async ({ page }) => {
    await page.locator('#rtl-clickable').focus();
    await expect(page.locator('#rtl-clickable')).toBeFocused();
  });
});

test.describe('Card high contrast @card', () => {
  test.use({ forcedColors: 'active' });

  test('P-CARD-036 forced colors mode keeps a visible border', async ({ page }) => {
    await openScenarios(page);
    const width = await page.locator('#hc-card').evaluate((el) => getComputedStyle(el).borderTopWidth);
    expect(width).not.toBe('0px');
  });

  test('P-CARD-039 focus visibility in forced colors', async ({ page }) => {
    await openScenarios(page);
    await page.locator('#order-before').focus();
    await page.keyboard.press('Tab');
    const card = page.locator('#order-card');
    await expect(card).toBeFocused();
    const outline = await card.evaluate((el) => getComputedStyle(el).outlineStyle);
    expect(outline).not.toBe('none');
  });
});

test.describe('Card high contrast light @card', () => {
  test.use({ forcedColors: 'active', colorScheme: 'light' });

  test('P-CARD-037 high contrast light', async ({ page }) => {
    await openScenarios(page);
    await expect(page.locator('#hc-card')).toBeVisible();
    expect(await page.evaluate(() => matchMedia('(forced-colors: active)').matches)).toBe(true);
  });
});

test.describe('Card high contrast dark @card', () => {
  test.use({ forcedColors: 'active', colorScheme: 'dark' });

  test('P-CARD-038 high contrast dark', async ({ page }) => {
    await openScenarios(page);
    await expect(page.locator('#hc-card')).toBeVisible();
    expect(await page.evaluate(() => matchMedia('(forced-colors: active)').matches)).toBe(true);
  });
});


test.describe('Card end-to-end @card', () => {
  test.beforeEach(async ({ page }) => {
    await openScenarios(page);
  });

  test('P-CARD-041 header only page', async ({ page }) => {
    const regions = page.locator('#header-only-card > [data-testid]');
    await expect(regions).toHaveCount(1);
    await expect(regions.first()).toHaveAttribute('data-testid', 'card-header');
  });

  test('P-CARD-042 media card page', async ({ page }) => {
    await expect(page.locator('#media-card img')).toHaveAttribute('alt', 'Canada map');
    const box = await page.locator('#media-card img').boundingBox();
    expect(box!.height).toBeGreaterThan(0);
  });

  test('P-CARD-043 actions page', async ({ page }) => {
    await expect(page.locator('#actions-card [data-testid="card-actions"] button')).toHaveCount(2);
  });

  test('P-CARD-044 clickable card page', async ({ page }) => {
    const card = page.locator('#pointer-card');
    await expect(card).toHaveAttribute('role', 'button');
    await expect(card).toHaveAttribute('tabindex', '0');
  });

  test('B-CARD-046 a static card page has no button semantics (no Disabled parameter exists)', async ({ page }) => {
    await expect(page.locator('#static-card')).not.toHaveAttribute('role', /.+/);
  });

  test('P-CARD-045 loading card page', async ({ page }) => {
    await openInteractions(page);
    await expect(page.locator('#card-loading')).toHaveClass(/e-card-loading/);
    await expect(page.locator('#card-loading [data-testid="card-actions"]')).toHaveAttribute('inert', '');
  });

  test('P-CARD-046 horizontal card page', async ({ page }) => {
    await openInteractions(page);
    expect(await page.locator('#card-horizontal').evaluate((el) => getComputedStyle(el).flexDirection)).toBe('row');
  });

  test('P-CARD-047 dynamic update scenario', async ({ page }) => {
    const card = page.locator('#dynamic-card');
    await expect(card).not.toHaveClass(/e-card-outlined/);
    await page.locator('#dynamic-update').click();
    await expect(card).toHaveClass(/e-card-outlined/);
    await expect(card).toHaveClass(/e-card-horizontal/);
    await expect(card.locator('[data-testid="card-content"]')).toHaveText('Updated');
  });

  test('P-CARD-048 multiple card collection', async ({ page }) => {
    await expect(page.locator('#scn-collection [data-testid="card-root"]')).toHaveCount(3);
    const ids = await page.locator('#scn-collection [data-testid="card-root"]').evaluateAll((els) => els.map((e) => e.id));
    expect(new Set(ids).size).toBe(3);
  });

  test('P-CARD-049 nested interaction scenario', async ({ page }) => {
    await page.locator('#nested-card').getByText('Inner').click();
    await expect(page.locator('#nested-counts')).toHaveText('outer:0 inner:1');
    await page.locator('#nested-card [data-testid="card-content"]').click({ position: { x: 4, y: 4 } });
    await expect(page.locator('#nested-counts')).toHaveText('outer:1 inner:1');
  });
});
