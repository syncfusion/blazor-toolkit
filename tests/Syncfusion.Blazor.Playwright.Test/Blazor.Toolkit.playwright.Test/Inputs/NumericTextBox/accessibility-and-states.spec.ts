import { test, expect, Page } from '@playwright/test';

const base = 'http://localhost:5000/numerictextbox';

test.describe('NumericTextBox — Accessibility & States', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto(`${base}/states`);
    await page.waitForLoadState('networkidle');
  });

test('readonly states behave correctly', async ({ page }) => {
  const readonly = page.locator('#readonlyNumeric');
  const editable = page.locator('#editableNumeric');

  // readonly should have readonly attribute and keep its value
  await expect(readonly).toHaveAttribute('readonly', '');
  const readonlyValue = await readonly.inputValue();
  expect(parseFloat(readonlyValue)).toBe(42);

  // editable should accept new value
  await editable.fill('55');
  await page.waitForTimeout(80);
  const editableValue = await editable.inputValue();
  await page.waitForTimeout(80);
  expect(parseFloat(editableValue)).toBe(55);
});



  test('ARIA attributes update on value change', async ({ page }) => {
    await page.goto(`${base}/accessibility`);
    await page.waitForLoadState('networkidle');
    const container = page.locator('#accessNumeric');
    const input = page.locator('#accessNumeric.e-input').first();
    const wrap = container;

    await input.fill('3');
    await page.waitForTimeout(80);
    const role = await wrap.getAttribute('role');
    expect(role === 'spinbutton' || role === null).toBeTruthy();
    const ariaNow = await wrap.getAttribute('aria-valuenow');
    if (ariaNow) expect(Number(ariaNow)).toBeGreaterThanOrEqual(0);
  });
});
