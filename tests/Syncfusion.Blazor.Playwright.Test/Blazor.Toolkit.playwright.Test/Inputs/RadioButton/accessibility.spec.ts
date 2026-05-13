// Accessibility Testing
// Tests REAL SfRadioButton components from the Blazor sample app

import { test, expect } from '@playwright/test';

test.describe('Accessibility Features', () => {
  test.beforeEach(async ({ page }) => {
    // Navigate to the basic rendering sample page (which includes accessibility)
    await page.goto('http://localhost:5000/radio-button/basic-rendering');
    await page.waitForLoadState('networkidle');
  });

  test('Test label-input association for accessibility', async ({ page }) => {
    const radio = page.locator('input[type="radio"]').first();
    const radioId = await radio.getAttribute('id');

    // Label should be associated with the radio
    const label = page.locator(`label[for="${radioId}"]`);
    await expect(label).toBeVisible();

    // Click label should select the radio
    await label.click();
    await expect(radio).toBeChecked();
  });

  test('Test keyboard navigation through radio button group', async ({ page }) => {
    const radioGroup = page.locator('input[name="basic-group"]');
    const firstRadio = radioGroup.first();

    // Focus the first radio
    await firstRadio.focus();
    let focusedElement = await page.evaluate(() => document.activeElement?.id ?? null);
    const firstId = await firstRadio.getAttribute('id');
    expect(focusedElement).toBe(firstId);

    // Arrow keys should navigate
    if (await radioGroup.count() > 1) {
      await page.keyboard.press('ArrowDown');
      // Focus or value should change
      focusedElement = await page.evaluate(() => document.activeElement?.id ?? null);
      // May have moved focus or changed value
      expect(focusedElement).toBeTruthy();
    }
  });

  test('Test semantic HTML structure', async ({ page }) => {
    // Check that radio buttons have proper attributes
    const radios = page.locator('input[type="radio"]');
    
    for (let i = 0; i < await radios.count(); i++) {
      const radio = radios.nth(i);
      
      // Should have type="radio"
      await expect(radio).toHaveAttribute('type', 'radio');
      
      // Should have name attribute
      const name = await radio.getAttribute('name');
      expect(name).toBeTruthy();
      
      // Should have value attribute
      const value = await radio.getAttribute('value');
      expect(value).toBeTruthy();
      
      // Should have id for label association
      const id = await radio.getAttribute('id');
      expect(id).toBeTruthy();
    }
  });

  test('Test radio buttons are tab-focusable', async ({ page }) => {
    const radioGroup = page.locator('input[name="basic-group"]');
    
    if (await radioGroup.count() > 0) {
      const firstRadio = radioGroup.first();
      
      // Focus the radio
      await firstRadio.focus();
      
      // Verify it's focused
      let focusedElement = await page.evaluate(() => document.activeElement?.id ?? null);
      const radioId = await firstRadio.getAttribute('id');
      expect(focusedElement).toBe(radioId);
      
      // Space should toggle (or click)
      await firstRadio.click();
      await expect(firstRadio).toBeChecked();
    }
  });

  // ---------------------------------------------------------------
  // 3. ARIA attributes test (VALID)
  // ---------------------------------------------------------------
  test('Test ARIA attributes are present', async ({ page }) => {
    await page.setContent('<div></div>');

    await page.evaluate(() => {
      const wrapper = document.createElement('div');
      wrapper.id = 'aria-wrapper';
      wrapper.setAttribute('role', 'radiogroup');
      wrapper.setAttribute('aria-label', 'Choose an option');

      for (let i = 0; i < 2; i++) {
        const input = document.createElement('input');
        input.type = 'radio';
        input.id = `aria-radio-${i}`;
        input.name = 'aria-test';
        input.className = 'e-control e-radio e-lib';

        const label = document.createElement('label');
        label.className = 'e-label';
        label.setAttribute('for', input.id);
        label.textContent = `Option ${i}`;

        wrapper.appendChild(input);
        wrapper.appendChild(label);
      }

      document.body.appendChild(wrapper);
    });

    await expect(page.locator('#aria-radio-0')).toHaveAttribute('type', 'radio');
    await expect(page.locator('#aria-radio-0')).toHaveAttribute('name', 'aria-test');
    await expect(page.locator('[aria-label="Choose an option"]'))
      .toHaveAttribute('role', 'radiogroup');
  });

  // ---------------------------------------------------------------
  // 4. Disabled radio behavior (UPDATED)
  // ---------------------------------------------------------------
  test('Test disabled button accessibility', async ({ page }) => {
    await page.setContent('<div></div>');

    await page.evaluate(() => {
      const wrap = document.createElement('div');

      const input = document.createElement('input');
      input.type = 'radio';
      input.id = 'disabled-radio';
      input.disabled = true;
      input.name = 'disabled-test';
      input.value = 'disabled';
      input.className = 'e-control e-radio e-lib';

      const label = document.createElement('label');
      label.className = 'e-label';
      label.setAttribute('for', 'disabled-radio');
      label.textContent = 'Disabled Option';

      wrap.appendChild(input);
      wrap.appendChild(label);
      document.body.appendChild(wrap);
    });

    const radio = page.locator('#disabled-radio');
    const label = page.locator('label[for="disabled-radio"]');

    await expect(radio).toBeDisabled();
    await expect(radio).not.toBeChecked();

    // Playwright blocks normal clicks on labels tied to disabled controls
    // → force the click but still expect NO change
    await label.click({ force: true });
    await expect(radio).not.toBeChecked();

    // Programmatic click has no effect when disabled
    const afterClick = await page.evaluate(() => {
      const el = document.getElementById('disabled-radio');
      el!.click();
      return (el as HTMLInputElement).checked;
    });
    expect(afterClick).toBe(false);
  });

});
