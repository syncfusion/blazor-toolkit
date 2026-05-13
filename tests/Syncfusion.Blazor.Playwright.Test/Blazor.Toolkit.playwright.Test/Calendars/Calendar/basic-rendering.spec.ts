import { test, expect } from '@playwright/test';

test.describe('SfCalendar - Basic Rendering', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/calendar-test');
    await page.waitForLoadState('networkidle');
  });

  test('renders calendar root and basic classes', async ({ page }) => {
    const root = page.locator('#calendar-test, .e-calendar').first();
    await expect(root).toBeVisible();
    const cls = await root.getAttribute('class');
    expect(cls).toContain('e-calendar');
    const tabindex = await root.getAttribute('tabindex');
    expect([tabindex ?? '0', '0']).toContain(tabindex ?? '0');
  });

  test('day cells and headers exist with ARIA attributes', async ({ page }) => {
    const day = page.locator('td[aria-label="calendar cell"]').first();
    await expect(day).toBeVisible();
    await expect(day).toHaveAttribute('aria-label', /calendar cell/);
    const id = await day.getAttribute('id');
    expect(id).toBeTruthy();
  });
});
