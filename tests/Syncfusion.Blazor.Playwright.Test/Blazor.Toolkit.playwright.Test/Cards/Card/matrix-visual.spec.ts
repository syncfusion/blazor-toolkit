// Visual regression mapped to the V-CARD ids in .github/specs/Cards/TEST-MATRIX.md.
// Drives /card/visual. Baselines already stored by visual.spec.ts are reused where the scenario matches:
//   V-CARD-001 card-basic, V-CARD-003 card-outlined, V-CARD-005 card-flat, V-CARD-012 card-loading,
//   V-CARD-013 card-horizontal, V-CARD-017 card-mobile, V-CARD-022 card-rtl.
// The Card has no Disabled parameter, so A-CARD-011 is captured as the disabled-looking loading presentation.

import { test, expect, type Page } from '@playwright/test';

const URL = 'http://localhost:5000/card/visual';

async function openVisual(page: Page) {
  await page.goto(URL, { waitUntil: 'commit' });
  await expect(page.locator('#visual-ready')).toHaveAttribute('data-ready', 'true', { timeout: 15000 });
}


// The baseline section is fit-content, so a 100% wide card would not follow the viewport. Let it fill the viewport.
async function fillViewport(page: Page) {
  await page.evaluate(() => {
    const section = document.getElementById('card-mobile') as HTMLElement;
    section.style.width = 'calc(100vw - 64px)';
  });
}

const shot = { animations: 'disabled', caret: 'hide', maxDiffPixelRatio: 0.01 } as const;

const sections: Array<[string, string]> = [
  ['V-CARD-006', 'v-header-only'],
  ['V-CARD-007', 'v-body-only'],
  ['V-CARD-008', 'v-footer-only'],
  ['V-CARD-009', 'v-media-only'],
  ['V-CARD-010', 'v-actions-only'],
  ['V-CARD-011', 'v-clickable'],
  ['V-CARD-014', 'v-vertical'],
  ['V-CARD-015', 'v-long-content'],
  ['V-CARD-016', 'v-overflow'],
  ['V-CARD-020', 'v-dark'],
  ['V-CARD-021', 'v-high-contrast'],
  ['V-CARD-026', 'v-nested'],
  ['V-CARD-027', 'v-multiple'],
  ['V-CARD-028', 'v-square'],
  ['V-CARD-029', 'v-hoverable'],
  ['V-CARD-030', 'v-padding-off'],
  ['V-CARD-031', 'v-object-fit'],
  ['V-CARD-032', 'v-separator'],
];

test.describe('Card visual matrix @card', () => {
  test.beforeEach(async ({ page }) => {
    await openVisual(page);
  });

  for (const [id, name] of sections) {
    test(`${id} ${name} matches its baseline`, async ({ page }) => {
      const section = page.locator(`#${name}`);
      await section.scrollIntoViewIfNeeded();
      const imageIsSubject = name === 'v-object-fit';
      await expect(section).toHaveScreenshot(`${name}.png`, imageIsSubject ? shot : { ...shot, mask: [section.locator('img')] });
    });
  }

  test('V-CARD-002 elevated draws a shadow', async ({ page }) => {
    const section = page.locator('#card-variants');
    await section.scrollIntoViewIfNeeded();
    await expect(section).toHaveScreenshot('card-variants.png', { ...shot, mask: [section.locator('img')] });
  });

  test('V-CARD-004 filled variant baseline', async ({ page }) => {
    const card = page.locator('#card-filled-root');
    await card.scrollIntoViewIfNeeded();
    await expect(card).toHaveScreenshot('card-filled.png', shot);
  });

  test('A-CARD-011 disabled presentation (loading) baseline', async ({ page }) => {
    const card = page.locator('#card-loading-root');
    await card.scrollIntoViewIfNeeded();
    await expect(card).toHaveScreenshot('card-disabled-presentation.png', shot);
  });

  test('V-CARD-018 tablet baseline', async ({ page }) => {
    await page.setViewportSize({ width: 820, height: 1100 });
    await fillViewport(page);
    const card = page.locator('#card-mobile-root');
    await card.scrollIntoViewIfNeeded();
    await expect(card).toHaveScreenshot('card-tablet.png', shot);
  });

  test('V-CARD-019 desktop baseline', async ({ page }) => {
    await page.setViewportSize({ width: 1280, height: 800 });
    await fillViewport(page);
    const card = page.locator('#card-mobile-root');
    await card.scrollIntoViewIfNeeded();
    await expect(card).toHaveScreenshot('card-desktop.png', shot);
  });

  test('V-CARD-023 hover baseline', async ({ page }) => {
    const card = page.locator('#v-hoverable-root');
    await card.scrollIntoViewIfNeeded();
    await card.hover();
    await expect(card).toHaveScreenshot('card-hover.png', shot);
  });

  test('V-CARD-024 focus-visible baseline', async ({ page }) => {
    const card = page.locator('#v-clickable-root');
    await card.scrollIntoViewIfNeeded();
    await page.keyboard.press('Tab');
    await card.focus();
    await page.keyboard.press('Shift+Tab');
    await page.keyboard.press('Tab');
    await expect(card).toBeFocused();
    await expect(card).toHaveScreenshot('card-focus-visible.png', shot);
  });

  test('V-CARD-025 pressed baseline', async ({ page }) => {
    const card = page.locator('#v-clickable-root');
    await card.scrollIntoViewIfNeeded();
    const box = (await card.boundingBox())!;
    await page.mouse.move(box.x + box.width / 2, box.y + box.height / 2);
    await page.mouse.down();
    await expect(card).toHaveScreenshot('card-pressed.png', shot);
    await page.mouse.up();
  });
});
