// Value Binding Testing
// Tests REAL SfRadioButton components from the Blazor sample app

import { test, expect } from '@playwright/test';

test.describe('Value Binding', () => {
  test.beforeEach(async ({ page }) => {
    // Navigate to the value binding sample page
    await page.goto('http://localhost:5000/radio-button/value-binding');
    await page.waitForLoadState('networkidle');
  });

  test('Test string value binding', async ({ page }) => {
    // Test the string binding section
    const stringRadios = page.locator('input[name="string-group"]');
    
    // Check first radio button
    const firstRadio = stringRadios.first();
    const firstValue = await firstRadio.getAttribute('value');
    expect(firstValue).toBeTruthy();

    // Click and verify value
    await firstRadio.click();
    await expect(firstRadio).toBeChecked();
    
    // Click second radio
    const secondRadio = stringRadios.nth(1);
    const secondValue = await secondRadio.getAttribute('value');
    await secondRadio.click();
    await expect(secondRadio).toBeChecked();
    await expect(firstRadio).not.toBeChecked();
  });

  test('Test two-way binding updates value', async ({ page }) => {
    // Get the binding display text
    const bindingText = page.locator('p').filter({ hasText: 'Bound Value:' }).first();
    
    // Get radio buttons
    const bindRadios = page.locator('input[name="bind-group"]');
    const optionInput = page.locator('input[type="text"]').first();
    
    // Select first radio
    const firstRadio = bindRadios.first();
    const firstValue = await firstRadio.getAttribute('value');
    await firstRadio.click();

    // Verify display updates
    const displayText = await bindingText.textContent();
    expect(displayText).toContain(firstValue);
  });

 test('Test value binding with input field', async ({ page }) => {
  const inputField = page.locator('input[type="text"]').first();
  const customValueButton = page.locator('button', {
    hasText: 'Set to custom-value'
  });

  await inputField.fill('test-input-value');

  await customValueButton.click();

  await expect(inputField).toHaveValue('custom-value');
});

  test('Test value binding reflects selection', async ({ page }) => {
    // Find radio buttons in the string-group
    const radios = page.locator('input[name="string-group"]');
    const valA = radios.nth(0);
    const valB = radios.nth(1);
    const valC = radios.nth(2);

    // Verify values
    await expect(valA).toHaveAttribute('value', 'val-a');
    await expect(valB).toHaveAttribute('value', 'val-b');
    await expect(valC).toHaveAttribute('value', 'val-c');

    // Click each and verify
    await valA.click();
    await expect(valA).toBeChecked();

    const display = page.locator('p').filter({ hasText: 'String Binding' });
    await expect(display).toContainText('val-a');

    await valB.click();
    await expect(valB).toBeChecked();
    await expect(valA).not.toBeChecked();

    await valC.click();
    await expect(valC).toBeChecked();
    await expect(valB).not.toBeChecked();
  });

  // -------------------------------------------------------------
  // 2) BOOLEAN VALUE BINDING
  // -------------------------------------------------------------
  test('Test boolean value binding', async ({ page }) => {
    await page.setContent('<div id="root"></div>');

    await page.evaluate(() => {
      const root = document.getElementById('root')!;
      const container = document.createElement('div');
      container.id = 'bool-binding-test';

      const makeRadio = (id: string, text: string, value: string) => {
        const wrap = document.createElement('div');
        wrap.className = 'e-radio-wrapper e-wrapper';

        const input = document.createElement('input');
        input.type = 'radio';
        input.name = 'bool-test';
        input.id = id;
        input.value = value;
        input.className = 'e-control e-radio e-lib';

        const label = document.createElement('label');
        label.className = 'e-label';
        label.setAttribute('for', id);
        label.textContent = text;

        wrap.appendChild(input);
        wrap.appendChild(label);
        return wrap;
      };

      const trueWrap = makeRadio('bool-true', 'True', 'true');
      const falseWrap = makeRadio('bool-false', 'False', 'false');

      const disp = document.createElement('div');
      disp.id = 'bool-display';
      disp.textContent = 'false';

      container.appendChild(trueWrap);
      container.appendChild(falseWrap);
      container.appendChild(disp);
      root.appendChild(container);

      trueWrap.querySelector('input')!.addEventListener('change', () => disp.textContent = 'true');
      falseWrap.querySelector('input')!.addEventListener('change', () => disp.textContent = 'false');
    });

    const trueBtn = page.locator('#bool-true');
    const falseBtn = page.locator('#bool-false');
    const display = page.locator('#bool-display');

    await trueBtn.click();
    await expect(trueBtn).toBeChecked();
    await expect(falseBtn).not.toBeChecked();
    await expect(display).toHaveText('true');

    await falseBtn.click();
    await expect(falseBtn).toBeChecked();
    await expect(trueBtn).not.toBeChecked();
    await expect(display).toHaveText('false');
  });

  // -------------------------------------------------------------
  // 3) NULLABLE BOOLEAN
  // -------------------------------------------------------------
  test('Test nullable boolean binding', async ({ page }) => {
    await page.setContent('<div id="root"></div>');

    await page.evaluate(() => {
      const root = document.getElementById('root')!;
      const container = document.createElement('div');

      const opts = [
        { id: 'nullable-null',  value: 'null',  label: 'Null' },
        { id: 'nullable-true',  value: 'true',  label: 'True' },
        { id: 'nullable-false', value: 'false', label: 'False' }
      ];

      opts.forEach(o => {
        const wrap = document.createElement('div');
        wrap.className = 'e-radio-wrapper e-wrapper';

        const input = document.createElement('input');
        input.type = 'radio';
        input.name = 'nullable-test';
        input.id = o.id;
        input.value = o.value;
        input.className = 'e-control e-radio e-lib';

        const label = document.createElement('label');
        label.className = 'e-label';
        label.setAttribute('for', o.id);
        label.textContent = o.label;

        wrap.appendChild(input);
        wrap.appendChild(label);
        container.appendChild(wrap);
      });

      const disp = document.createElement('div');
      disp.id = 'nullable-display';
      container.appendChild(disp);

      root.appendChild(container);

      document.querySelectorAll('input[name="nullable-test"]').forEach(el => {
        el.addEventListener('change', () => disp.textContent = (el as HTMLInputElement).value);
      });
    });

    const nullBtn = page.locator('input[name="nullable-test"][value="null"]');
    const trueBtn = page.locator('input[name="nullable-test"][value="true"]');
    const falseBtn = page.locator('input[name="nullable-test"][value="false"]');
    const display = page.locator('#nullable-display');

    await nullBtn.click();
    await expect(nullBtn).toBeChecked();
    await expect(display).toHaveText('null');

    await trueBtn.click();
    await expect(trueBtn).toBeChecked();
    await expect(nullBtn).not.toBeChecked();
    await expect(display).toHaveText('true');

    await falseBtn.click();
    await expect(falseBtn).toBeChecked();
    await expect(trueBtn).not.toBeChecked();
    await expect(display).toHaveText('false');
  });

  // -------------------------------------------------------------
  // 4) INTEGER VALUE BINDING
  // -------------------------------------------------------------
  test('Test integer value binding', async ({ page }) => {
    await page.setContent('<div id="root"></div>');

    await page.evaluate(() => {
      const root = document.getElementById('root')!;
      const container = document.createElement('div');

      for (let i = 0; i <= 2; i++) {
        const wrap = document.createElement('div');
        wrap.className = 'e-radio-wrapper e-wrapper';

        const input = document.createElement('input');
        input.type = 'radio';
        input.name = 'int-test';
        input.id = `int-${i}`;
        input.value = String(i);
        input.className = 'e-control e-radio e-lib';

        const label = document.createElement('label');
        label.className = 'e-label';
        label.setAttribute('for', `int-${i}`);
        label.textContent = `Option ${i}`;

        wrap.appendChild(input);
        wrap.appendChild(label);
        container.appendChild(wrap);
      }

      const disp = document.createElement('div');
      disp.id = 'int-display';
      container.appendChild(disp);

      root.appendChild(container);

      document.querySelectorAll('input[name="int-test"]').forEach(el => {
        el.addEventListener('change', () => disp.textContent = (el as HTMLInputElement).value);
      });
    });

    const option0 = page.locator('input[name="int-test"][value="0"]');
    const option1 = page.locator('input[name="int-test"][value="1"]');
    const option2 = page.locator('input[name="int-test"][value="2"]');
    const display = page.locator('#int-display');

    await option0.click();
    await expect(option0).toBeChecked();
    await expect(display).toHaveText('0');

    await option1.click();
    await expect(option1).toBeChecked();
    await expect(option0).not.toBeChecked();
    await expect(display).toHaveText('1');

    await option2.click();
    await expect(option2).toBeChecked();
    await expect(option1).not.toBeChecked();
    await expect(display).toHaveText('2');
  });

  // -------------------------------------------------------------
  // 5) CHECKED STATE SYNC
  // -------------------------------------------------------------
  test('Test checked state synchronization', async ({ page }) => {
    await page.setContent('<div id="root"></div>');

    // Inject pay group again
    await page.evaluate(() => {
      const root = document.getElementById('root')!;
      ['Card', 'Net'].forEach((v) => {
        const wrap = document.createElement('div');
        wrap.className = 'e-radio-wrapper e-wrapper';

        const input = document.createElement('input');
        input.type = 'radio';
        input.name = 'pay';
        input.id = `pay-${v}`;
        input.value = v;
        input.className = 'e-control e-radio e-lib';

        const label = document.createElement('label');
        label.className = 'e-label';
        label.setAttribute('for', input.id);
        label.textContent = v;

        wrap.appendChild(input);
        wrap.appendChild(label);
        root.appendChild(wrap);
      });
    });

    const buttons = page.locator('input[name="pay"]');
    const button1 = buttons.nth(0);
    const button2 = buttons.nth(1);

    await button1.click();
    await expect(button1).toBeChecked();
    await expect(button2).not.toBeChecked();

    const value1 = await button1.getAttribute('value');
    const value2 = await button2.getAttribute('value');
    expect(value1).not.toBe(value2);

    await button2.click();
    await expect(button1).not.toBeChecked();
    await expect(button2).toBeChecked();

    expect(await button1.isChecked()).toBe(false);
    expect(await button2.isChecked()).toBe(true);
  });
});
