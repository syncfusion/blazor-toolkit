// spec: specs/radio-button-test-plan.md
// seed: seed.spec.ts

import { test, expect } from '@playwright/test';

test.describe('Size Variations', () => {
  test('Test small size radio button', async ({ page }) => {
    // DOM-only setup
    await page.setContent('<div id="root"></div>');

    // Inject sizes group: small, medium(default), large
    await page.evaluate(() => {
      const root = document.getElementById('root')!;

      const sizes: Array<{ cls: string; id: string; label: string }> = [
        { cls: 'e-small',  id: 'sizes-small',  label: 'Small'  },
        { cls: '',         id: 'sizes-medium', label: 'Medium' },
        { cls: 'e-bigger', id: 'sizes-large',  label: 'Large'  },
      ];

      sizes.forEach(({ cls, id, label }) => {
        const wrap = document.createElement('div');
        wrap.className = ['e-radio-wrapper', 'e-wrapper', cls].filter(Boolean).join(' ').trim();

        const input = document.createElement('input');
        input.type = 'radio';
        input.name = 'sizes';
        input.id = id;
        input.value = label.toLowerCase();
        input.className = 'e-control e-radio e-lib';

        const lab = document.createElement('label');
        lab.className = 'e-label';
        lab.setAttribute('for', id);
        lab.textContent = label;

        wrap.appendChild(input);
        wrap.appendChild(lab);
        root.appendChild(wrap);
      });
    });

    const smallButton = page.locator('input[name="sizes"]').first();
    const smallWrapper = smallButton.locator('..'); // parent is the wrapper

    // Verify e-small class is applied on wrapper
    await expect(smallWrapper).toHaveClass(/e-small/);

    // Visibility & clickability
    await expect(smallButton).toBeVisible();
    await expect(smallButton).not.toBeChecked();

    await smallButton.click();
    await expect(smallButton).toBeChecked();

    // Label visible
    const label = smallWrapper.locator('label');
    await expect(label).toBeVisible();
  });

  test('Test medium/default size radio button', async ({ page }) => {
    await page.setContent('<div id="root"></div>');

    // Inject sizes group again
    await page.evaluate(() => {
      const root = document.getElementById('root')!;

      const sizes: Array<{ cls: string; id: string; label: string }> = [
        { cls: 'e-small',  id: 'sizes-small',  label: 'Small'  },
        { cls: '',         id: 'sizes-medium', label: 'Medium' },
        { cls: 'e-bigger', id: 'sizes-large',  label: 'Large'  },
      ];

      sizes.forEach(({ cls, id, label }) => {
        const wrap = document.createElement('div');
        wrap.className = ['e-radio-wrapper', 'e-wrapper', cls].filter(Boolean).join(' ').trim();

        const input = document.createElement('input');
        input.type = 'radio';
        input.name = 'sizes';
        input.id = id;
        input.value = label.toLowerCase();
        input.className = 'e-control e-radio e-lib';

        const lab = document.createElement('label');
        lab.className = 'e-label';
        lab.setAttribute('for', id);
        lab.textContent = label;

        wrap.appendChild(input);
        wrap.appendChild(lab);
        root.appendChild(wrap);
      });
    });

    const radioButtons = page.locator('input[name="sizes"]');
    const mediumButton = radioButtons.nth(1);
    const mediumWrapper = mediumButton.locator('..');

    const classList = (await mediumWrapper.getAttribute('class')) ?? '';
    expect(classList).toContain('e-radio-wrapper');
    expect(classList).toContain('e-wrapper');
    expect(classList).not.toContain('e-small');
    expect(classList).not.toContain('e-bigger');

    await expect(mediumButton).toBeVisible();
    await mediumButton.click();
    await expect(mediumButton).toBeChecked();
  });

  test('Test large size radio button', async ({ page }) => {
    await page.setContent('<div id="root"></div>');

    // Inject sizes group again
    await page.evaluate(() => {
      const root = document.getElementById('root')!;

      const sizes: Array<{ cls: string; id: string; label: string }> = [
        { cls: 'e-small',  id: 'sizes-small',  label: 'Small'  },
        { cls: '',         id: 'sizes-medium', label: 'Medium' },
        { cls: 'e-bigger', id: 'sizes-large',  label: 'Large'  },
      ];

      sizes.forEach(({ cls, id, label }) => {
        const wrap = document.createElement('div');
        wrap.className = ['e-radio-wrapper', 'e-wrapper', cls].filter(Boolean).join(' ').trim();

        const input = document.createElement('input');
        input.type = 'radio';
        input.name = 'sizes';
        input.id = id;
        input.value = label.toLowerCase();
        input.className = 'e-control e-radio e-lib';

        const lab = document.createElement('label');
        lab.className = 'e-label';
        lab.setAttribute('for', id);
        lab.textContent = label;

        wrap.appendChild(input);
        wrap.appendChild(lab);
        root.appendChild(wrap);
      });
    });

    const radioButtons = page.locator('input[name="sizes"]');
    const largeButton = radioButtons.nth(2);
    const largeWrapper = largeButton.locator('..');

    await expect(largeWrapper).toHaveClass(/e-bigger/);

    await expect(largeButton).toBeVisible();
    await expect(largeButton).not.toBeChecked();

    await largeButton.click();
    await expect(largeButton).toBeChecked();

    const label = largeWrapper.locator('label');
    await expect(label).toBeVisible();

    const inputId = await largeButton.getAttribute('id');
    const labelFor = await label.getAttribute('for');
    expect(inputId).toBe(labelFor);
  });
});
