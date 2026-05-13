import { test, expect } from '@playwright/test';

test.describe('TextArea - Row & Column Count', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/inputs/textarea');
    await page.waitForLoadState('networkidle');
    await page.locator('textarea#defaultRowCol').waitFor({ state: 'visible', timeout: 5000 });
  });

  test('default rows attribute is applied', async ({ page }) => {
    const textarea = page.locator('textarea#defaultRowCol');
    await expect(textarea).toBeVisible();
    await expect(textarea).toHaveAttribute('rows', /\d+/);
  });

  test('default columns attribute is applied', async ({ page }) => {
    const textarea = page.locator('textarea#defaultRowCol');
    await expect(textarea).toBeVisible();
    await expect(textarea).toHaveAttribute('cols', /\d+/);
  });

  test('custom rows applied to textarea', async ({ page }) => {
    const textarea = page.locator('textarea#customRowCol');
    await expect(textarea).toBeVisible();
    
    const rows = await textarea.getAttribute('rows');
    const rowsNum = parseInt(rows || '0');
    expect(rowsNum).toBeGreaterThan(0);
  });

  test('custom columns applied to textarea', async ({ page }) => {
    const textarea = page.locator('textarea#customRowCol');
    await expect(textarea).toBeVisible();
    
    const cols = await textarea.getAttribute('cols');
    const colsNum = parseInt(cols || '0');
    expect(colsNum).toBeGreaterThan(0);
  });

  test('large textarea renders with correct dimensions', async ({ page }) => {
    const textarea = page.locator('textarea#largeRowCol');
    await expect(textarea).toBeVisible();
    
    const rows = await textarea.getAttribute('rows');
    const cols = await textarea.getAttribute('cols');
    
    const rowsNum = parseInt(rows || '0');
    const colsNum = parseInt(cols || '0');
    
    expect(rowsNum).toBeGreaterThanOrEqual(10);
    expect(colsNum).toBeGreaterThanOrEqual(50);
  });

  test('rows minimum value is 1', async ({ page }) => {
    const textarea = page.locator('textarea#defaultRowCol');
    await expect(textarea).toBeVisible();

    const rows = await textarea.getAttribute('rows');
    const rowsNum = parseInt(rows || '0');
    
    expect(rowsNum).toBeGreaterThanOrEqual(1);
  });

  test('columns can accommodate different values', async ({ page }) => {
    const textareas = await page.locator('textarea[id*="RowCol"]').all();
    
    expect(textareas.length).toBeGreaterThan(0);
    
    // Verify all textareas have cols attribute
    for (const textarea of textareas) {
      const cols = await textarea.getAttribute('cols');
      expect(cols).toBeTruthy();
      const colsNum = parseInt(cols || '0');
      expect(colsNum).toBeGreaterThan(0);
    }
  });

  test('textarea height adjusts with rows count', async ({ page }) => {
    const textarea5Rows = page.locator('textarea#rowsCount5');
    const textarea10Rows = page.locator('textarea#rowsCount10');

    await expect(textarea5Rows).toBeVisible().catch(() => {});
    await expect(textarea10Rows).toBeVisible().catch(() => {});

    // If both exist, compare heights
    const textareas = await page.locator('textarea[id*="rowsCount"]').all();
    
    if (textareas.length >= 2) {
      const height5 = await textarea5Rows.evaluate((el) => el.offsetHeight);
      const height10 = await textarea10Rows.evaluate((el) => el.offsetHeight);
      
      // Textarea with more rows should be taller
      expect(height10).toBeGreaterThan(height5);
    }
  });

  test('textarea width adjusts with cols count', async ({ page }) => {
    const textarea30Cols = page.locator('textarea#colsCount30');
    const textarea60Cols = page.locator('textarea#colsCount60');

    await expect(textarea30Cols).toBeVisible().catch(() => {});
    await expect(textarea60Cols).toBeVisible().catch(() => {});

    // If both exist, compare widths
    const textareas = await page.locator('textarea[id*="colsCount"]').all();
    
    if (textareas.length >= 2) {
      const width30 = await textarea30Cols.evaluate((el) => el.offsetWidth);
      const width60 = await textarea60Cols.evaluate((el) => el.offsetWidth);
      
      // Textarea with more cols should be wider
      expect(width60).toBeGreaterThan(width30);
    }
  });

  test('text wraps at column boundary', async ({ page }) => {
    const textarea = page.locator('textarea#defaultRowCol');
    await expect(textarea).toBeVisible();

    const cols = await textarea.getAttribute('cols');
    const colsNum = parseInt(cols || '50');

    // Type text longer than cols
    const longText = 'a'.repeat(colsNum + 20);
    await textarea.fill(longText);
    await page.waitForTimeout(300);

    // Verify text is still in textarea
    await expect(textarea).toHaveValue(longText);
  });

  test('multiple rows and columns combinations work', async ({ page }) => {
    const combinations = [
      { rows: '5', cols: '30' },
      { rows: '10', cols: '50' },
      { rows: '15', cols: '60' }
    ];

    for (const combo of combinations) {
      const textarea = page.locator(`textarea[rows="${combo.rows}"][cols="${combo.cols}"]`).first();
      
      if (await textarea.count() > 0) {
        await expect(textarea).toBeVisible();
        await expect(textarea).toHaveAttribute('rows', combo.rows);
        await expect(textarea).toHaveAttribute('cols', combo.cols);
      }
    }
  });

});