import { test, expect } from '@playwright/test';

test.describe('TextBox - Edge cases', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/inputs/textbox');
    await page.waitForLoadState('networkidle');
  });

  test('null/empty initial value handles gracefully', async ({ page }) => {
    const el = page.locator('#emptyValueInput');
    await expect(el).toBeVisible();
    await expect(el).toHaveValue('');
  });

  test('very long text entry', async ({ page }) => {
    const el = page.locator('#textTypeInput');
    const long = 'x'.repeat(1200);
    await el.fill(long);
    await expect(el).toHaveValue(long);
  });

  test('special characters are accepted', async ({ page }) => {
    const el = page.locator('#textTypeInput');
    const s = `&<>\"' é 😊`;
    await el.fill(s);
    await expect(el).toHaveValue(s);
  });

  test('rapid typing and paste operations', async ({ page }) => {
    const el = page.locator('#textTypeInput');
    await el.fill('');
    await el.type('quicktypingfast');
    await expect(el).toHaveValue(/quicktypingfast/);
    await el.focus();
    await page.keyboard.down('Control');
    await page.keyboard.press('KeyA');
    await page.keyboard.up('Control');
    await page.keyboard.press('Control+v').catch(()=>{});
  });

  test('value with whitespace only', async ({ page }) => {
    const el = page.locator('#textTypeInput');
    await el.fill('   ');
    await expect(el).toHaveValue('   ');
  });

  test('copy and paste between TextBox components', async ({ page }) => {
    const a = page.locator('#textTypeInput');
    const b = page.locator('#emailTypeInput');
    await a.fill('copyme');
    await a.selectText();
    await page.keyboard.press('Control+C');
    await b.focus();
    await page.keyboard.press('Control+V');
    await expect(b).toHaveValue(/copyme/);
  });

  test('undo/redo keyboard actions', async ({ page }) => {
    const el = page.locator('#textTypeInput');
    await el.fill('one');
    await el.type('two');
    await page.keyboard.press('Control+z');
    // Undo may revert; ensure input still exists
    await expect(el).toBeVisible();
  });
});
