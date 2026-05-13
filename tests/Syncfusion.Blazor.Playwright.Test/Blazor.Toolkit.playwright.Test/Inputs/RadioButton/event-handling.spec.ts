// Event Handling Testing
// Tests REAL SfRadioButton components from the Blazor sample app

import { test, expect } from '@playwright/test';

test.describe('Event Handling', () => {
  test.beforeEach(async ({ page }) => {
    // Navigate to the event handling sample page
    await page.goto('http://localhost:5000/radio-button/event-handling');
    await page.waitForLoadState('networkidle');
  });

  test('Test change events are triggered', async ({ page }) => {
    const radioButtons = page.locator('input[name="event-group"]');
    const firstRadio = radioButtons.nth(0);
    const secondRadio = radioButtons.nth(1);

    // Click first radio and verify change
    await firstRadio.click();
    await expect(firstRadio).toBeChecked();

    // Click second radio and verify change
    await secondRadio.click();
    await expect(secondRadio).toBeChecked();
    await expect(firstRadio).not.toBeChecked();
  });

  test('Test click events on radio buttons', async ({ page }) => {
    const clickableRadios = page.locator('input[name="click-group"]');
    const firstRadio = clickableRadios.first();

    // Click the radio
    await firstRadio.click();
    await expect(firstRadio).toBeChecked();

    // Click again should keep it checked (can't uncheck radio, only select another)
    await firstRadio.click();
    await expect(firstRadio).toBeChecked();
  });

  test('Test selection state tracking', async ({ page }) => {
    const radioGroup = page.locator('input[name="event-group"]');
    
    // Track as we click through each radio
    for (let i = 0; i < await radioGroup.count(); i++) {
      const currentRadio = radioGroup.nth(i);
      await currentRadio.click();

      // Verify it's checked
      await expect(currentRadio).toBeChecked();

      // Verify others are not checked
      for (let j = 0; j < await radioGroup.count(); j++) {
        if (i !== j) {
          const otherRadio = radioGroup.nth(j);
          await expect(otherRadio).not.toBeChecked();
        }
      }
    }
  });

  test('Test label click triggers radio selection', async ({ page }) => {
    const labels = page.locator('.e-radio-wrapper label');
    const inputs = page.locator('input[name="event-group"]');

    if (await labels.count() > 0) {
      const firstLabel = labels.first();
      await firstLabel.click();

      // First radio should be checked
      const firstInput = inputs.first();
      await expect(firstInput).toBeChecked();
    }
  });
});
