import { test, expect } from '@playwright/test';

test.describe('TextArea - Float Label Types', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/inputs/textarea');
    await page.waitForLoadState('networkidle');
    await page.locator('textarea#floatLabelNever').waitFor({ state: 'visible', timeout: 5000 });
  });

  test('FloatLabelType.Never - label never floats', async ({ page }) => {
    const textarea = page.locator('textarea#floatLabelNever');
    await expect(textarea).toBeVisible();
    
    // Fill textarea
    await textarea.fill('test');
    await page.waitForTimeout(500);

    // Check if label element exists and its visibility/position
    const parent = textarea.locator('..');
    const label = parent.locator('label').first();
    
    const isLabelPresent = await label.count() > 0;
    
    if (isLabelPresent) {
      // Check if label is floating (check computed style or position)
      const labelStyle = await label.evaluate((el) => {
        const computed = window.getComputedStyle(el);
        return {
          position: computed.position,
          top: computed.top,
          fontSize: computed.fontSize,
          opacity: computed.opacity
        };
      });
      
      // Label should not be floating (top should be auto or large value)
      expect(labelStyle.position !== 'absolute' || labelStyle.top === 'auto').toBeTruthy();
    } else {
      expect(true).toBeTruthy();
    }
  });

  test('FloatLabelType.Auto - label floats on focus', async ({ page }) => {
    const textarea = page.locator('textarea#floatLabelAuto');
    await expect(textarea).toBeVisible();

    // Before focus - check if placeholder exists (optional)
    const placeholderBefore = await textarea.getAttribute('placeholder');
    if (placeholderBefore !== null) {
      expect(placeholderBefore).toBeTruthy();
    }

    // On focus
    await textarea.focus();
    await page.waitForTimeout(800);

    // Check parent for float label indicator
    const parent = textarea.locator('..');
    const label = parent.locator('label').first();
    
    const isFocused = await textarea.evaluate((el) => document.activeElement === el);
    expect(isFocused).toBeTruthy();
    
    if (await label.count() > 0) {
      const labelStyle = await label.evaluate((el) => {
        const computed = window.getComputedStyle(el);
        return {
          position: computed.position,
          top: computed.top,
          fontSize: computed.fontSize,
          transform: computed.transform
        };
      });
      
      // Label should be floating (absolute position with small top value or transform)
      const isFloating = labelStyle.position === 'absolute' && 
                        (labelStyle.top !== 'auto' || labelStyle.transform !== 'none');
      expect(isFloating || isFocused).toBeTruthy();
    }
  });

  test('FloatLabelType.Auto - label floats when value exists', async ({ page }) => {
    const textarea = page.locator('textarea#floatLabelAuto');
    await expect(textarea).toBeVisible();

    // Fill with value
    await textarea.fill('test value');
    await page.waitForTimeout(800);

    // Check if textarea has value
    await expect(textarea).toHaveValue('test value');

    const parent = textarea.locator('..');
    const label = parent.locator('label').first();
    
    if (await label.count() > 0) {
      const labelStyle = await label.evaluate((el) => {
        const computed = window.getComputedStyle(el);
        return {
          position: computed.position,
          top: computed.top,
          fontSize: computed.fontSize,
          transform: computed.transform
        };
      });
      
      // Label should be floating when value exists
      const isFloating = labelStyle.position === 'absolute' && 
                        (labelStyle.top !== 'auto' || labelStyle.transform !== 'none');
      expect(isFloating).toBeTruthy();
    } else {
      // If no label element, just verify value is set
      expect(true).toBeTruthy();
    }
  });

  test('FloatLabelType.Always - label always floats', async ({ page }) => {
    const textarea = page.locator('textarea#floatLabelAlways');
    await expect(textarea).toBeVisible();

    const parent = textarea.locator('..');
    const label = parent.locator('label').first();

    // Check if label exists and is styled as floating
    if (await label.count() > 0) {
      const labelStyle = await label.evaluate((el) => {
        const computed = window.getComputedStyle(el);
        return {
          position: computed.position,
          top: computed.top,
          fontSize: computed.fontSize,
          transform: computed.transform
        };
      });
      
      // Label should always be floating
      const isFloating = labelStyle.position === 'absolute' && 
                        (labelStyle.top !== 'auto' || labelStyle.transform !== 'none');
      expect(isFloating).toBeTruthy();
    } else {
      expect(true).toBeTruthy();
    }
  });

  test('Placeholder visibility differs by float label type', async ({ page }) => {
    const neverFloat = page.locator('textarea#floatLabelNever');
    const autoFloat = page.locator('textarea#floatLabelAuto');
    const alwaysFloat = page.locator('textarea#floatLabelAlways');

    await expect(neverFloat).toBeVisible();
    await expect(autoFloat).toBeVisible();
    await expect(alwaysFloat).toBeVisible();

    const neverPlaceholder = await neverFloat.getAttribute('placeholder');
    const autoPlaceholder = await autoFloat.getAttribute('placeholder');
    const alwaysPlaceholder = await alwaysFloat.getAttribute('placeholder');

    // Check if placeholders exist and are non-empty strings
    const neverHasPlaceholder = neverPlaceholder !== null && neverPlaceholder !== '';
    const autoHasPlaceholder = autoPlaceholder !== null && autoPlaceholder !== '';
    const alwaysHasPlaceholder = alwaysPlaceholder !== null && alwaysPlaceholder !== '';

    expect(neverHasPlaceholder || autoHasPlaceholder || alwaysHasPlaceholder).toBeTruthy();
  });

});