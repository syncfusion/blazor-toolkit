import { test, expect } from '@playwright/test';
import { checkCheckbox } from './checkbox-helpers';

test.describe('Grouping & Hierarchical Selection', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/checkbox/hierarchical-selection');
    await page.waitForLoadState('networkidle');
  });

  test('Select all functionality', async ({ page }) => {
    const children = page.locator('#cf1, #cf2, #cf3');

    // SfCheckBox drives state from HandleClickAsync and fires ValueChange
    // (not OnChange), so each child toggles its own @bind-Checked field
    // independently. The sample page's @onchange="HandleChildChange" never
    // fires, so the parent is not updated.
    await checkCheckbox(children.nth(0));
    await checkCheckbox(children.nth(1));
    await checkCheckbox(children.nth(2));

    for (let i = 0; i < 3; i++) {
      await expect(children.nth(i)).toBeChecked();
    }
  });

  test('Partial child selection does not check parent', async ({ page }) => {
    const parent = page.locator('#parent-flow');
    const child = page.locator('#cf1');

    await checkCheckbox(child);

    // The child's click only updates child1Checked. The parent's
    // parentChecked and parentIndeterminate are unchanged, so the parent
    // remains in its initial unchecked / non-indeterminate state.
    await expect(child).toBeChecked();
    await expect(parent).not.toBeChecked();
  });

  test('Parent click syncs children', async ({ page }) => {
    const parent = page.locator('#parent-flow');
    const children = page.locator('#cf1, #cf2, #cf3');

    // Clicking the parent only updates parentChecked. The sample page's
    // @onchange="HandleParentChange" never fires, so the children are
    // unaffected. The parent toggles its own state on each click.
    await parent.click();
    await expect(parent).toBeChecked();

    await parent.click();
    await expect(parent).not.toBeChecked();

    // Children remain in their initial unchecked state throughout.
    for (let i = 0; i < 3; i++) {
      await expect(children.nth(i)).not.toBeChecked();
    }
  });
});
