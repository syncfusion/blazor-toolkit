import { test, expect } from '@playwright/test';

test.describe('Switch - Label Placement (SfSwitch behavior)', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/switch/label-placement');
    await page.waitForLoadState('networkidle');
  });

  test('Accessibility: Label is applied via aria attributes', async ({ page }) => {
    const switchInput = page.locator('#switch-label');

    const ariaLabel = await switchInput.getAttribute('aria-label');
    const ariaLabelledBy = await switchInput.getAttribute('aria-labelledby');

    // SfSwitch uses Label only for accessibility
    expect(ariaLabel || ariaLabelledBy).toBeTruthy();
  });

  test('OnLabel and OffLabel elements are rendered', async ({ page }) => {
    const wrapper = page.locator('.e-switch-wrapper').first();

    await expect(wrapper.locator('.e-switch-on')).toBeVisible();
    await expect(wrapper.locator('.e-switch-off')).toBeVisible();
  });

  test('OffLabel is active when switch is OFF', async ({ page }) => {
    const input = page.locator('#switch-label');
    const wrapper = page.locator('.e-switch-wrapper', { has: input });

    // Ensure OFF state
    if (await input.isChecked()) {
      await wrapper.click();
    }

    await expect(input).not.toBeChecked();
    await expect(wrapper.locator('.e-switch-off')).toBeVisible();
  });

  test('OnLabel is active when switch is ON', async ({ page }) => {
    const input = page.locator('#switch-label');
    const wrapper = page.locator('.e-switch-wrapper', { has: input });

    await wrapper.click();
    await expect(input).toBeChecked();
    await expect(wrapper.locator('.e-switch-on')).toBeVisible();
  });

  test('Clicking switch wrapper toggles state', async ({ page }) => {
    const input = page.locator('#switch-label');
    const wrapper = page.locator('.e-switch-wrapper', { has: input });

    const initial = await input.isChecked();

    await wrapper.click();

    if (initial) {
      await expect(input).not.toBeChecked();
    } else {
      await expect(input).toBeChecked();
    }
  });
});
