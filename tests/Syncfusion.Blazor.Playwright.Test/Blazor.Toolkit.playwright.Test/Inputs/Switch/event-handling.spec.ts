// Event Handling Test for Real SfSwitch Component
// Tests ValueChange event callback

import { test, expect } from '@playwright/test';

test.describe('Switch - Event Handling', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/switch/event-handling');
    await page.waitForLoadState('networkidle');
  });

  test('ValueChange event fires when switch is toggled', async ({ page }) => {
    const switchInput = page.locator('#switch-event');
    const switchWrapper = page.locator('.e-switch-wrapper', { has: switchInput });
    const lastChangeDisplay = page.locator('p:has-text("Last Change")').first();

    // Get initial last change value
    let lastChangeText = await lastChangeDisplay.textContent();
    expect(lastChangeText).toContain('None');

    // Toggle the switch by clicking wrapper
    await switchWrapper.click();
    await page.waitForTimeout(100);

    // Last change should have updated with a timestamp
    lastChangeText = await lastChangeDisplay.textContent();
    expect(lastChangeText).not.toContain('None');
    expect(lastChangeText).toMatch(/\d{2}:\d{2}:\d{2}/);
  });

  test('Event log updates on value changes', async ({ page }) => {
    const switchInput = page.locator('#switch-event');
    const switchWrapper = page.locator('.e-switch-wrapper', { has: switchInput });
    const eventLog = page.locator('#event-log');

    // Initial log should be empty
    let logItems = await eventLog.locator('li').count();
    expect(logItems).toBe(0);

    // Toggle switch
    await switchWrapper.click();
    await page.waitForTimeout(100);

    // Event should be logged
    logItems = await eventLog.locator('li').count();
    expect(logItems).toBeGreaterThan(0);

    // Log entry should contain the new value
    const firstLogEntry = await eventLog.locator('li').first().textContent();
    expect(firstLogEntry).toContain('True');
  });

  test('Multiple value changes are logged', async ({ page }) => {
    const switchInput = page.locator('#switch-event');
    const switchWrapper = page.locator('.e-switch-wrapper', { has: switchInput });
    const eventLog = page.locator('#event-log');

    // Perform multiple toggles
    for (let i = 0; i < 3; i++) {
      await switchWrapper.click();
      await page.waitForTimeout(50);
    }

    // All events should be logged
    const logItems = await eventLog.locator('li').count();
    expect(logItems).toBe(3);
  });

  test('Clear log button removes all log entries', async ({ page }) => {
    const switchInput = page.locator('#switch-event');
    const switchWrapper = page.locator('.e-switch-wrapper', { has: switchInput });
    const eventLog = page.locator('#event-log');
    const clearButton = page.locator('button:has-text("Clear Log")');

    // Create some log entries
    await switchWrapper.click();
    await page.waitForTimeout(50);
    await switchWrapper.click();
    await page.waitForTimeout(50);

    let logItems = await eventLog.locator('li').count();
    expect(logItems).toBeGreaterThan(0);

    // Clear the log
    await clearButton.click();
    await page.waitForTimeout(50);

    // Log should be empty now
    logItems = await eventLog.locator('li').count();
    expect(logItems).toBe(0);
  });

  test('Event log displays changes in chronological order', async ({ page }) => {
    const switchInput = page.locator('#switch-event');
    const switchWrapper = page.locator('.e-switch-wrapper', { has: switchInput });
    const eventLog = page.locator('#event-log');

    // Perform toggles: OFF -> ON -> OFF
    await switchWrapper.click();
    await page.waitForTimeout(50);
    await switchWrapper.click();
    await page.waitForTimeout(50);

    // Get log entries
    const entries = await eventLog.locator('li').allTextContents();

    // Most recent change should be first (OFF), oldest should be last (ON)
    expect(entries[0]).toContain('False');
    expect(entries[1]).toContain('True');
  });

  test('Switch value display updates on toggle', async ({ page }) => {
    const switchInput = page.locator('#switch-event');
    const switchWrapper = page.locator('.e-switch-wrapper', { has: switchInput });
    const valueDisplay = page.locator('p:has-text("Switch Value")').first();

    // Initial value
    let displayText = await valueDisplay.textContent();
    expect(displayText).toContain('False');

    // Toggle ON
    await switchWrapper.click();
    await page.waitForTimeout(100);

    displayText = await valueDisplay.textContent();
    expect(displayText).toContain('True');

    // Toggle back OFF
    await switchWrapper.click();
    await page.waitForTimeout(100);

    displayText = await valueDisplay.textContent();
    expect(displayText).toContain('False');
  });
});
