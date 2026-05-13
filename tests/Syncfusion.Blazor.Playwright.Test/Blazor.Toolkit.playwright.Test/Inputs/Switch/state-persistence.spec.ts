// State Persistence Test for SfSwitch Component
// Tests EnablePersistence feature and localStorage integration

import { test, expect } from '@playwright/test';

test.describe('Switch - State Persistence', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/switch/state-persistence');
    await page.waitForLoadState('networkidle');
    
    // Clear all localStorage before each test
    await page.evaluate(() => {
      localStorage.clear();
    });
  });

  test('Persistent switch saves state to localStorage on toggle', async ({ page }) => {
    const persistInput = page.locator('#switch-persist-enabled');
    const wrapper = page.locator('.e-switch-wrapper', { has: persistInput });
    
    // Initial state - unchecked
    await expect(persistInput).not.toBeChecked();
    
    // Toggle the switch
    await wrapper.click();
    
    // Verify state changed
    await expect(persistInput).toBeChecked();
    
    // Verify localStorage was updated (Syncfusion uses specific key pattern)
    const storageValue = await page.evaluate(() => {
      const keys = Object.keys(localStorage);
      return keys.filter(k => k.includes('switch-persist-enabled'));
    });
    
    expect(storageValue.length).toBeGreaterThan(0);
  });

  test('Persistent switch restores state on reload', async ({ page }) => {
    const persistInput = page.locator('#switch-persist-enabled');
    const wrapper = page.locator('.e-switch-wrapper', { has: persistInput });
    
    // Toggle the switch to ON
    await wrapper.click();
    await expect(persistInput).toBeChecked();
    
    // Reload the page
    await page.reload();
    await page.waitForLoadState('networkidle');
    
    // Verify state is restored
    const reloadedInput = page.locator('#switch-persist-enabled');
    await expect(reloadedInput).toBeChecked();
  });

  test('Non-persistent switch does not save to localStorage', async ({ page }) => {
    const nonPersistInput = page.locator('#switch-persist-disabled');
    const wrapper = page.locator('.e-switch-wrapper', { has: nonPersistInput });
    
    // Toggle the switch
    await wrapper.click();
    await expect(nonPersistInput).toBeChecked();
    
    // Check that it doesn't store to localStorage (or uses different pattern)
    const storageValue = await page.evaluate(() => {
      const keys = Object.keys(localStorage);
      return keys.filter(k => k.includes('switch-persist-disabled'));
    });
    
    // Non-persistent should not have storage entry
    expect(storageValue.length).toBe(0);
  });

  test('Non-persistent switch loses state on reload', async ({ page }) => {
    const nonPersistInput = page.locator('#switch-persist-disabled');
    const wrapper = page.locator('.e-switch-wrapper', { has: nonPersistInput });
    
    // Toggle the switch to ON
    await wrapper.click();
    await expect(nonPersistInput).toBeChecked();
    
    // Reload the page
    await page.reload();
    await page.waitForLoadState('networkidle');
    
    // State should be lost (back to default OFF)
    const reloadedInput = page.locator('#switch-persist-disabled');
    await expect(reloadedInput).not.toBeChecked();
  });

  test('Multiple persistent switches maintain independent state', async ({ page }) => {
    const persist1Input = page.locator('#switch-persist-1');
    const persist2Input = page.locator('#switch-persist-2');
    
    const wrapper1 = page.locator('.e-switch-wrapper', { has: persist1Input });
    const wrapper2 = page.locator('.e-switch-wrapper', { has: persist2Input });
    
    // Toggle only the first switch
    await wrapper1.click();
    await expect(persist1Input).toBeChecked();
    await expect(persist2Input).not.toBeChecked();
    
    // Reload
    await page.reload();
    await page.waitForLoadState('networkidle');
    
    // Verify individual states are restored
    const reloadedPersist1 = page.locator('#switch-persist-1');
    const reloadedPersist2 = page.locator('#switch-persist-2');
    
    await expect(reloadedPersist1).toBeChecked();
    await expect(reloadedPersist2).not.toBeChecked();
  });

 test('Persistent switch shows correct initial display value', async ({ page }) => {
  const valueDisplay = page.locator('#persist-value');

  // Initial state
  await expect(valueDisplay).toHaveText(/false/i);

  const persistInput = page.locator('#switch-persist-enabled');
  const wrapper = page.locator('.e-switch-wrapper', { has: persistInput });

  // Toggle
  await wrapper.click();

  // Display reflects bound value
  await expect(valueDisplay).toHaveText(/true/i);
});

  test('Toggling persistent switch multiple times maintains final state', async ({ page }) => {
    const persistInput = page.locator('#switch-persist-enabled');
    const wrapper = page.locator('.e-switch-wrapper', { has: persistInput });
    
    // Toggle multiple times: OFF -> ON -> OFF -> ON
    await wrapper.click(); // ON
    await wrapper.click(); // OFF
    await wrapper.click(); // ON
    
    await expect(persistInput).toBeChecked();
    
    // Reload and verify final ON state is preserved
    await page.reload();
    await page.waitForLoadState('networkidle');
    
    const reloadedInput = page.locator('#switch-persist-enabled');
    await expect(reloadedInput).toBeChecked();
  });
});
