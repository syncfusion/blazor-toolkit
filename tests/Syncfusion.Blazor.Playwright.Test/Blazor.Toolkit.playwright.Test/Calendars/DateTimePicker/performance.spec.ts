import { test, expect } from '@playwright/test';

test.describe('DateTimePicker - Performance & Rendering', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/datetimepicker-test');
    await page.waitForLoadState('networkidle');
  });

  test('single DateTimePicker renders without lag', async ({ page }) => {
    const input = page.locator('#wrapper-dtp-basic input');
    
    const startTime = Date.now();
    await expect(input).toBeVisible();
    const renderTime = Date.now() - startTime;
    
    // Should render quickly
    expect(renderTime).toBeLessThan(5000);
  });

  test('multiple DateTimePicker instances render efficiently', async ({ page }) => {
    const inputs = page.locator('#wrapper-dtp-many input');
    
    const startTime = Date.now();
    
    // Should have 10 instances
    const count = await inputs.count();
    expect(count).toBe(10);
    
    const renderTime = Date.now() - startTime;
    
    // Should render all 10 without significant lag
    expect(renderTime).toBeLessThan(10000);
  });

  test('many pickers all visible on page', async ({ page }) => {
    const inputs = page.locator('#wrapper-dtp-many input');
    
    const count = await inputs.count();
    expect(count).toBeGreaterThanOrEqual(10);
    
    // Sample a few to verify they are visible
    for (let i = 0; i < Math.min(count, 5); i++) {
      await expect(inputs.nth(i)).toBeVisible();
    }
  });

  test('many pickers are interactive', async ({ page }) => {
    const firstIcon = page.locator('#wrapper-dtp-many #many-1 .e-timeline-today');
    const lastIcon = page.locator('#wrapper-dtp-many #many-10 .e-timeline-today');
    
    // Open first popup
    await firstIcon.click();
    let popup = page.locator('.e-popup:visible');
    await expect(popup).toHaveCount(1, { timeout: 5000 });
    
    // Close
    await page.keyboard.press('Escape');
    await page.waitForTimeout(300);
    
    // Open last popup
    await lastIcon.click();
    popup = page.locator('.e-popup:visible');
    await expect(popup).toHaveCount(1, { timeout: 5000 });
  });

  test('repeated popup open/close does not create new instances', async ({ page }) => {
    const icon = page.locator('#wrapper-dtp-basic .e-timeline-today');
    
    const initialPopups = await page.locator('.e-popup').count();
    
    // Repeated open/close
    for (let i = 0; i < 3; i++) {
      await icon.click();
      await page.waitForTimeout(200);
      await page.keyboard.press('Escape');
      await page.waitForTimeout(200);
    }
    
    const finalPopups = await page.locator('.e-popup').count();
    
    // Should reuse same popup element
    expect(finalPopups).toBeLessThanOrEqual(initialPopups + 1);
  });

  test('calendar popup renders quickly', async ({ page }) => {
    const icon = page.locator('#wrapper-dtp-basic .e-timeline-today');
    
    const startTime = Date.now();
    
    await icon.click();
    const popup = page.locator('.e-popup:visible');
    await expect(popup).toBeVisible();
    
    const renderTime = Date.now() - startTime;
    
    // Popup should render quickly
    expect(renderTime).toBeLessThan(2000);
  });

  test('time popup list renders quickly', async ({ page }) => {
    const icon = page.locator('#wrapper-dtp-basic .e-clock');
    
    const startTime = Date.now();
    
    await icon.click();
    const popup = page.locator('.e-popup:visible');
    await expect(popup).toBeVisible();
    
    const renderTime = Date.now() - startTime;
    
    // Time list should render quickly
    expect(renderTime).toBeLessThan(2000);
  });

  test('scrolling calendar is smooth', async ({ page }) => {
    const icon = page.locator('#wrapper-dtp-basic .e-timeline-today');
    await icon.click();
    
    const popup = page.locator('.e-popup:visible');
    const calendar = popup.locator('.e-calendar');
    
    // Navigate months
    const startTime = Date.now();
    
    const next = popup.locator('.e-next');
    for (let i = 0; i < 5; i++) {
      await next.click();
      await page.waitForTimeout(100);
    }
    
    const navigationTime = Date.now() - startTime;
    
    // Navigation should be responsive
    expect(navigationTime).toBeLessThan(2000);
  });

  test('scrolling time popup is smooth', async ({ page }) => {
    const icon = page.locator('#wrapper-dtp-basic .e-clock');
    await icon.click();
    
    const popup = page.locator('.e-popup:visible');
    const content = popup.locator('.e-content');
    
    // Scroll down
    const startTime = Date.now();
    
    await content.evaluate((el) => {
      el.scrollTop = 500;
    });
    
    await page.waitForTimeout(300);
    
    const scrollTime = Date.now() - startTime;
    
    // Scrolling should be responsive
    expect(scrollTime).toBeLessThan(1000);
  });

  test('value change events are performant', async ({ page }) => {
    const input = page.locator('#wrapper-dtp-basic input');
    
    const startTime = Date.now();
    
    // Change value multiple times
    for (let i = 0; i < 5; i++) {
      await input.fill(`0${i + 1}/15/2025 1${i}:30`);
      await input.press('Tab');
      await page.waitForTimeout(100);
    }
    
    const changeTime = Date.now() - startTime;
    
    // Should handle rapid changes efficiently
    expect(changeTime).toBeLessThan(5000);
  });

  test('memory not leaked on repeated clear', async ({ page }) => {
    const input = page.locator('#wrapper-dtp-initial input');
    const clear = page.locator('#wrapper-dtp-initial .e-close');
    
    await input.focus();
    
    // Get initial memory (if available)
    const initialMemory = await page.evaluate(() => {
      if ((performance as any).memory) {
        return (performance as any).memory.usedJSHeapSize;
      }
      return 0;
    });
    
    // Repeated clear operations
    for (let i = 0; i < 5; i++) {
      if (await clear.isVisible()) {
        await clear.click();
        await page.waitForTimeout(100);
        
        // Re-enter value
        await input.fill('05/15/2025 14:30');
        await page.waitForTimeout(100);
      }
    }
    
    // Check memory again
    const finalMemory = await page.evaluate(() => {
      if ((performance as any).memory) {
        return (performance as any).memory.usedJSHeapSize;
      }
      return 0;
    });
    
    // Memory should not grow excessively
    if (initialMemory > 0 && finalMemory > 0) {
      const memoryGrowth = finalMemory - initialMemory;
      expect(memoryGrowth).toBeLessThan(50 * 1024 * 1024); // Less than 50MB growth
    }
  });

  test('input responsiveness with keyboard', async ({ page }) => {
    const input = page.locator('#wrapper-dtp-basic input');
    
    const startTime = Date.now();
    
    // Type character by character
    await input.focus();
    await input.type('05/15/2025 14:30');
    
    const typeTime = Date.now() - startTime;
    
    // Typing should be responsive
    expect(typeTime).toBeLessThan(2000);
  });

  test('popup position calculation is fast', async ({ page }) => {
    const inputs = page.locator('#wrapper-dtp-many input');
    
    // Open popup for different items
    for (let i = 0; i < 3; i++) {
      const icon = page.locator(`#wrapper-dtp-many #many-${i + 1} .e-timeline-today`);
      
      const startTime = Date.now();
      
      await icon.click();
      const popup = page.locator('.e-popup:visible');
      await expect(popup).toBeVisible();
      
      const positionTime = Date.now() - startTime;
      
      // Positioning should be quick
      expect(positionTime).toBeLessThan(1500);
      
      await page.keyboard.press('Escape');
      await page.waitForTimeout(200);
    }
  });
});
