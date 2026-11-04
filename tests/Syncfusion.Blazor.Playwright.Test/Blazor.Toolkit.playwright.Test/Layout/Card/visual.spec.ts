// L5 visual baselines and remaining L3 appearance checks for SfCard.
// Drives the hosted page at /card/visual. Screenshot names match TEST-MATRIX.md section 7.

import { test, expect, type Page } from '@playwright/test';

const URL = 'http://localhost:5000/card/visual';

const baselines = [
  'card-basic',
  'card-media-actions',
  'card-outlined',
  'card-horizontal',
  'card-loading',
  'card-rtl',
  'card-flat',
  'card-mobile',
] as const;

async function openVisual(page: Page) {
  await page.goto(URL, { waitUntil: 'commit' });
  await expect(page.locator('#visual-ready')).toHaveAttribute('data-ready', 'true', { timeout: 15000 });
}

test.describe('Card visual baselines @card', () => {
  test.beforeEach(async ({ page }) => {
    await openVisual(page);
  });

  for (const name of baselines) {
    test(`L5 ${name} matches its baseline`, async ({ page }) => {
      const section = page.locator(`#${name}`);
      await expect(section).toBeVisible();
      await section.scrollIntoViewIfNeeded();
      await expect(section).toHaveScreenshot(`${name}.png`, {
        animations: 'disabled',
        caret: 'hide',
        maxDiffPixelRatio: 0.01,
        mask: [section.locator('img')],
      });
    });
  }

  test('P-CARD-001 product card renders header, content and footer in order', async ({ page }) => {
    const card = page.locator('#card-basic-root');
    await expect(card).toHaveClass(/e-card/);
    await expect(card).not.toHaveClass(/e-card-outlined|e-card-flat|e-card-elevated/);

    const regions = card.locator(':scope > [data-testid]');
    await expect(regions).toHaveCount(3);
    await expect(regions.nth(0)).toHaveAttribute('data-testid', 'card-header');
    await expect(regions.nth(1)).toHaveAttribute('data-testid', 'card-content');
    await expect(regions.nth(2)).toHaveAttribute('data-testid', 'card-footer');
    await expect(card.locator('[data-testid="card-title"]')).toHaveText('Trail Runner');
    await expect(card.locator('[data-testid="card-subtitle"]')).toHaveText('Outdoor');
  });

  test('outlined square card has a visible border and no radius', async ({ page }) => {
    const card = page.locator('#card-outlined-root');
    await expect(card).toHaveClass(/e-card-outlined/);
    await expect(card).toHaveClass(/e-card-square/);
    const box = await card.evaluate((el) => {
      const style = getComputedStyle(el);
      return { radius: style.borderRadius, width: style.borderTopWidth };
    });
    expect(box.radius).toBe('0px');
    expect(box.width).not.toBe('0px');
  });

  test('flat card draws neither a shadow nor a visible border', async ({ page }) => {
    const style = await page.locator('#card-flat-root').evaluate((el) => {
      const computed = getComputedStyle(el);
      return { shadow: computed.boxShadow, border: computed.borderTopColor };
    });
    expect(style.shadow).toBe('none');
  });

  test('filled and elevated variants are distinguishable', async ({ page }) => {
    const filled = page.locator('#card-filled-root');
    const elevated = page.locator('#card-elevated-root');
    await expect(filled).not.toHaveClass(/e-card-elevated/);
    await expect(elevated).toHaveClass(/e-card-elevated/);
    await expect(elevated).toHaveAttribute('data-elevation', '5');
    const elevatedShadow = await elevated.evaluate((el) => getComputedStyle(el).boxShadow);
    expect(elevatedShadow).not.toBe('none');
  });

  test('hoverable card lifts on hover', async ({ page }) => {
    const card = page.locator('#card-hoverable-root');
    const before = await card.evaluate((el) => getComputedStyle(el).transform);
    await card.hover();
    const after = await card.evaluate((el) => getComputedStyle(el).transform);
    expect(after).not.toBe(before);
    expect(after).not.toBe('none');
  });

  test('media and actions card exposes alt text and a separator', async ({ page }) => {
    const card = page.locator('#card-media-actions-root');
    await expect(card.locator('img')).toHaveAttribute('alt', 'Canada map');
    await expect(card.locator('[data-testid="card-separator"]')).toBeVisible();
    await expect(card.locator('[data-testid="card-actions"]')).toBeVisible();
    const image = await card.locator('img').boundingBox();
    expect(image).not.toBeNull();
    expect(image!.height).toBeGreaterThan(0);
  });

  test('loading baseline is busy and covers its actions', async ({ page }) => {
    const card = page.locator('#card-loading-root');
    await expect(card).toHaveClass(/e-card-loading/);
    await expect(card).toHaveAttribute('aria-busy', 'true');
    await expect(card.locator('[data-testid="card-actions"]')).toHaveAttribute('inert', '');
  });
});

test.describe('Card visual mobile @card', () => {
  test.use({ viewport: { width: 375, height: 700 } });

  test('P-CARD-008 mobile baseline stays inside the viewport', async ({ page }) => {
    await openVisual(page);
    const card = page.locator('#card-mobile-root');
    await expect(card).toBeVisible();
    const box = await card.boundingBox();
    expect(box).not.toBeNull();
    expect(box!.x).toBeGreaterThanOrEqual(0);
    expect(box!.x + box!.width).toBeLessThanOrEqual(375);
    const cardOverflow = await card.evaluate((el) => el.scrollWidth - el.clientWidth);
    expect(cardOverflow).toBeLessThanOrEqual(1);
  });
});
