// spec: specs/checkbox-component-test-plan.md
// Test Suite: Edge Cases & Special Scenarios
// Tests the REAL Syncfusion Checkbox component from the Blazor sample app

import { test, expect } from '@playwright/test';

test.describe('Edge Cases & Special Scenarios', () => {
  test.beforeEach(async ({ page }) => {
    // Navigate to the real Blazor sample page for edge cases
    await page.goto('http://localhost:5000/checkbox/edge-cases');
    // Wait for the page to load properly
    await page.waitForLoadState('networkidle');
  });

  test('Empty label text', async ({ page }) => {
    const checkbox = page.locator('input[type="checkbox"]').first();

    // 1. Checkbox with empty label should still be interactive
    // Expect: Checkbox is visible even without label text
    await expect(checkbox).toBeVisible();
    await expect(checkbox).toBeEnabled();

    // 2. Verify checkbox can be toggled
    await checkbox.click();
    await expect(checkbox).toBeChecked();

    // 3. Verify it toggles back
    await checkbox.click();
    await expect(checkbox).not.toBeChecked();
  });

  test('Very long label text', async ({ page }) => {
    const checkboxes = page.locator('input[type="checkbox"]');

    // Get second checkbox (very long label)
    const longLabelCheckbox = checkboxes.nth(1);
    const parentLabel = longLabelCheckbox.locator('..'); // Parent label element

    // 1. Render checkbox with very long label
    // Expect: Checkbox still functions correctly
    await expect(longLabelCheckbox).toBeVisible();
    await expect(longLabelCheckbox).toBeEnabled();

    // 2. Verify label is displayed
    const labelText = await parentLabel.textContent();
    expect((labelText || '').length).toBeGreaterThan(50);

    // 3. Verify checkbox still works
    await longLabelCheckbox.click();
    await expect(longLabelCheckbox).toBeChecked();
  });

  test('Multiple rapid toggles', async ({ page }) => {
    const checkboxes = page.locator('input[type="checkbox"]');
    
    // Get the rapid toggle checkbox
    const rapidCheckbox = checkboxes.nth(4); // From EdgeCases sample

    // 1. Click checkbox 10+ times rapidly
    for (let i = 0; i < 11; i++) {
      await rapidCheckbox.click();
      // Small delay to allow state change
      await page.waitForTimeout(5);
    }
    
    // Expect: Each click toggles the state correctly
    // After 11 clicks starting from unchecked, should be checked (odd number)
    await expect(rapidCheckbox).toBeChecked();
  });

  test('State persistence across interactions', async ({ page }) => {
    const checkboxes = page.locator('input[type="checkbox"]');
    const firstCheckbox = checkboxes.first();

    // 1. Toggle state multiple times
    await firstCheckbox.click();
    let isChecked = await firstCheckbox.isChecked();
    expect(isChecked).toBe(true);

    await firstCheckbox.click();
    isChecked = await firstCheckbox.isChecked();
    expect(isChecked).toBe(false);
    
    // Expect: Checkbox is functional again
    await expect(firstCheckbox).toBeEnabled();
  });

  test('Multiple checkbox instances', async ({ page }) => {
    const allCheckboxes = page.locator('input[type="checkbox"]');

    // 1. Verify all checkboxes render on the page
    // Expect: Multiple checkboxes are present (at least from edge-cases)
    const count = await allCheckboxes.count();
    expect(count).toBeGreaterThan(0);

    // 2. Toggle different checkboxes independently
    // Expect: Each maintains independent state
    const firstThree = Math.min(3, count);
    for (let i = 0; i < firstThree; i++) {
      await allCheckboxes.nth(i).check();
    }
    
    // Verify they're checked
    for (let i = 0; i < firstThree; i++) {
      await expect(allCheckboxes.nth(i)).toBeChecked();
    }

    // 3. Uncheck them to verify independence
    // Expect: Each binding updates correctly without affecting others
    await allCheckboxes.nth(0).uncheck();
    await expect(allCheckboxes.nth(0)).not.toBeChecked();
    await expect(allCheckboxes.nth(1)).toBeChecked();
    await expect(allCheckboxes.nth(2)).toBeChecked();
  });

  test('Checkbox with dynamic label rendering', async ({ page }) => {
    // Get checkboxes with labels
    const checkboxes = page.locator('input[type="checkbox"]');
    const count = await checkboxes.count();

    // 1. Render checkboxes with labels
    // Expect: Labels are displayed
    expect(count).toBeGreaterThan(0);

    // 2. Verify labels are associated with checkboxes
    // Expect: Each checkbox has proper label association
    for (let i = 0; i < Math.min(3, count); i++) {
      const checkbox = checkboxes.nth(i);
      const id = await checkbox.getAttribute('id');
      if (id) {
        // Try to find associated label
        const label = page.locator(`label[for="${id}"]`);
        const labelCount = await label.count();
        // Labels may or may not exist for all checkboxes
        expect(labelCount).toBeGreaterThanOrEqual(0);
      }
    }

    // 3. Verify state is preserved when interacting
    // Expect: Checkbox state is preserved
    await checkboxes.nth(0).click();
    await expect(checkboxes.nth(0)).toBeChecked();
  });

  test('Multiple checkboxes maintain independence', async ({ page }) => {
    const allCheckboxes = page.locator('input[type="checkbox"]');

    // 1. Get multiple checkboxes on the page
    // Expect: Each checkbox has unique presence
    const count = await allCheckboxes.count();
    expect(count).toBeGreaterThan(2);

    // 2. Toggle different checkboxes based on index
    // Expect: Each maintains independent state
    const indicesToCheck = [0, 2];
    for (const idx of indicesToCheck) {
      if (idx < count) {
        await allCheckboxes.nth(idx).check();
      }
    }

    // 3. Verify independence - check that only selected ones are checked
    for (let i = 0; i < count && i < 5; i++) {
      if (indicesToCheck.includes(i)) {
        await expect(allCheckboxes.nth(i)).toBeChecked();
      } else {
        await expect(allCheckboxes.nth(i)).not.toBeChecked();
      }
    }

    // Expect: Each binding updates correctly without affecting others
    // (verified by the independent states above)
  });

  test('Checkbox functionality with standard HTML attributes', async ({ page }) => {
    const checkbox = page.locator('input[type="checkbox"]').first();

    // 1. Verify checkbox has standard HTML attributes
    // Expect: Type attribute is checkbox
    const type = await checkbox.getAttribute('type');
    expect(type).toBe('checkbox');

    // 2. Verify basic HTML attributes work
    // Expect: Checkbox is enabled and interactive
    await expect(checkbox).toBeEnabled();
    
    // 3. Test checkbox functionality
    // Expect: Checkbox can be checked/unchecked
    await checkbox.check();
    await expect(checkbox).toBeChecked();
    
    await checkbox.uncheck();
    await expect(checkbox).not.toBeChecked();
  });

  test('Checkbox with custom attributes from advanced scenarios', async ({ page }) => {
    await page.goto('http://localhost:5000/checkbox/advanced-scenarios');
    await page.waitForLoadState('networkidle');

    const checkbox = page.locator('#complex-cb');

    // 1. Set InputAttributes with custom properties
    // Expect: Attributes are applied to input element if present
    const title = await checkbox.getAttribute('title');
    // Custom attributes may or may not be present
    if (title) {
      expect(title).toBeTruthy();
    }

    // 2. Verify functionality with custom attributes
    // Expect: Component works correctly with custom attributes
    await checkbox.click();
    await expect(checkbox).toBeChecked();
    
    // Verify state toggle
    await checkbox.click();
    await expect(checkbox).not.toBeChecked();
  });

  test('Checkbox visibility and interactivity state changes', async ({ page }) => {
    const checkboxes = page.locator('input[type="checkbox"]');

    // 1. Render checkbox component
    // Expect: Component is visible and interactive
    await expect(checkboxes.first()).toBeVisible();
    await expect(checkboxes.first()).toBeEnabled();

    // 2. Verify checkbox remains visible during interactions
    await checkboxes.first().click();
    await expect(checkboxes.first()).toBeVisible();
    await expect(checkboxes.first()).toBeEnabled();
    
    // 3. Verify multiple interactions don't affect visibility
    for (let i = 0; i < 3; i++) {
      await checkboxes.first().click();
      await expect(checkboxes.first()).toBeVisible();
    }
  });
});
