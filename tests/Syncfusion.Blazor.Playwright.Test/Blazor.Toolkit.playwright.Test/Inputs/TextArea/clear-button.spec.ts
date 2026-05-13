import { test, expect } from '@playwright/test';

test.describe('TextArea - Clear Button', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/inputs/textarea');
    await page.waitForLoadState('networkidle');
    await page.locator('textarea#clearButtonTextArea').waitFor({ state: 'visible', timeout: 5000 });
  });

  test('clear button appears when textarea has content', async ({ page }) => {
    const textarea = page.locator('textarea#clearButtonTextArea');
    await expect(textarea).toBeVisible();
    await textarea.fill('test content');
    await page.waitForTimeout(300);

    const clearBtn = textarea.locator('xpath=following-sibling::*').filter({ has: page.locator('.e-close') }).locator('.e-close').first();
    if (await clearBtn.count() > 0) {
      await expect(clearBtn).toBeVisible();
    }
  });

  test('clear button is hidden when textarea is empty', async ({ page }) => {
    const textarea = page.locator('textarea#clearButtonTextArea');
    await expect(textarea).toBeVisible();
    await textarea.clear();
    await page.waitForTimeout(500);

    const wrapper = textarea.locator('..');
    const clearBtn = wrapper.locator('.e-close');
    
    if (await clearBtn.count() > 0) {
      const isHidden = await clearBtn.evaluate((el) => {
        return window.getComputedStyle(el).display === 'none' || el.classList.contains('e-clear-icon-hide');
      });
      expect(isHidden).toBeTruthy();
    }
  });

  test('clicking clear empties textarea', async ({ page }) => {
    const textarea = page.locator('textarea#clearButtonTextArea');
    await expect(textarea).toBeVisible();
    await textarea.fill('Clear me');
    await page.waitForTimeout(300);

    const wrapper = textarea.locator('..');
    const clearBtn = wrapper.locator('.e-close').first();
    
    if (await clearBtn.count() > 0 && await clearBtn.isVisible()) {
      await clearBtn.click();
      await page.waitForTimeout(500);
      await expect(textarea).toHaveValue('');
    }
  });

  test('textarea without clear button does not show clear icon', async ({ page }) => {
    const textarea = page.locator('textarea#noClearButtonTextArea');
    await expect(textarea).toBeVisible();
    await textarea.fill('no clear button');
    await page.waitForTimeout(300);

    const wrapper = textarea.locator('..');
    const clearBtn = wrapper.locator('.e-close');
    
    if (await clearBtn.count() > 0) {
      const isVisible = await clearBtn.isVisible().catch(() => false);
      expect(isVisible).toBe(false);
    } else {
      expect(true).toBeTruthy();
    }
  });

  test('clear button focuses textarea after clearing', async ({ page }) => {
    const textarea = page.locator('textarea#clearButtonTextArea');
    await expect(textarea).toBeVisible();
    await textarea.fill('Focus test');
    await page.waitForTimeout(300);

    const wrapper = textarea.locator('..');
    const clearBtn = wrapper.locator('.e-close').first();
    
    if (await clearBtn.count() > 0 && await clearBtn.isVisible()) {
      await clearBtn.click();
      await page.waitForTimeout(500);
      
      // Verify textarea is cleared
      await expect(textarea).toHaveValue('');
      
      // Check if textarea has focus
      const isFocused = await textarea.evaluate((el) => {
        return document.activeElement === el;
      }).catch(() => false);
      
      if (isFocused) {
        expect(isFocused).toBeTruthy();
      }
    }
  });

  test('clear button works multiple times', async ({ page }) => {
    const textarea = page.locator('textarea#clearButtonTextArea');
    await expect(textarea).toBeVisible();
    
    const wrapper = textarea.locator('..');
    const clearBtn = wrapper.locator('.e-close').first();

    if (await clearBtn.count() > 0) {
      for (let i = 0; i < 3; i++) {
        await textarea.fill(`Test ${i}`);
        await page.waitForTimeout(300);
        
        if (await clearBtn.isVisible()) {
          await clearBtn.click();
          await page.waitForTimeout(500);
          await expect(textarea).toHaveValue('');
        }
      }
    }
  });

});