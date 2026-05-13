// Mutual Exclusivity (Grouping) Testing
// Tests REAL SfRadioButton components from the Blazor sample app

import { test, expect } from '@playwright/test';

test.describe('Mutual Exclusivity (Grouping)', () => {
  test.beforeEach(async ({ page }) => {
    // Navigate to the selection states sample page (which tests mutual exclusivity)
    await page.goto('http://localhost:5000/radio-button/selection-states');
    await page.waitForLoadState('networkidle');
  });

  test('Test radio buttons with same Name are mutually exclusive', async ({ page }) => {
    const radioButtons = page.locator('input[name="selection-group"]');
    const button1 = radioButtons.nth(0); // Selected Option
    const button2 = radioButtons.nth(1); // Unselected Option
    const button3 = radioButtons.nth(2); // Another Option

    // All share the same name
    await expect(button1).toHaveAttribute('name', 'selection-group');
    await expect(button2).toHaveAttribute('name', 'selection-group');
    await expect(button3).toHaveAttribute('name', 'selection-group');

    // Select #1
    await button1.click();
    await expect(button1).toBeChecked();
    await expect(button2).not.toBeChecked();
    await expect(button3).not.toBeChecked();

    // Select #2
    await button2.click();
    await expect(button1).not.toBeChecked();
    await expect(button2).toBeChecked();
    await expect(button3).not.toBeChecked();

    // Select #3
    await button3.click();
    await expect(button1).not.toBeChecked();
    await expect(button2).not.toBeChecked();
    await expect(button3).toBeChecked();
  });

  test('Test independent groups work correctly', async ({ page }) => {
    // Navigate to edge cases page which has multiple groups
    await page.goto('http://localhost:5000/radio-button/edge-cases');
    await page.waitForLoadState('networkidle');

    // Test Group 1
    const group1Radios = page.locator('input[name="edge-group"]');
    const group1First = group1Radios.first();
    
    // Test Group 2
    const group2Radios = page.locator('input[name="group1"]');
    const group2First = group2Radios.first();

    if (await group1First.count() > 0) {
      // Select from group 1
      await group1First.click();
      await expect(group1First).toBeChecked();

      // Group 2 should be independent
      if (await group2First.count() > 0) {
        await group2First.click();
        await expect(group2First).toBeChecked();
        
        // Group 1 selection should be unaffected by group 2
        const group1Second = group1Radios.nth(1);
        if (await group1Second.count() > 0) {
          // Group 1 first was selected before
          // It should still be checked because groups are independent
          const isMutuallyExclusive = !(await group1First.isChecked());
          expect(isMutuallyExclusive).toBeFalsy(); // Should still be checked
        }
      }
    }
  });

  test('Test only one radio in group can be selected at a time', async ({ page }) => {
    const radioGroup = page.locator('input[name="selection-group"]');
    
    // Check that all can't be checked simultaneously
    for (let i = 0; i < await radioGroup.count(); i++) {
      const radio = radioGroup.nth(i);
      await radio.click();

      // Verify only this one is checked
      const checkedCount = await radioGroup.evaluateAll(
        (inputs) => inputs.filter((inp) => (inp as HTMLInputElement).checked).length
      );
      
      expect(checkedCount).toBe(1);
    }
  });
});
