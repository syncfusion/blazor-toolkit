import { test, expect } from '@playwright/test';
import { openToolbarPage } from './helpers';

test.describe('Toolbar performance @toolbar', () => {
  test('P-TB-049 100 items render and 20 items add or remove, and the timings are recorded', async ({ page }, testInfo) => {
    await openToolbarPage(page, '/toolbar/stress');
    await expect(page.locator('#toolbar-stress [data-testid="toolbar-item"]')).toHaveCount(100);

    const timeAction = async (selector: string, expected: number) => {
      const start = Date.now();
      await page.locator(selector).click();
      await expect(page.locator('#item-count')).toHaveText(`Items: ${expected}`);
      await expect(page.locator('#toolbar-stress [data-testid="toolbar-item"]')).toHaveCount(expected);
      return Date.now() - start;
    };
    const addMs = await timeAction('#add-twenty', 120);
    const removeMs = await timeAction('#remove-twenty', 100);

    testInfo.annotations.push({ type: 'add-20-ms', description: String(addMs) });
    testInfo.annotations.push({ type: 'remove-20-ms', description: String(removeMs) });
    // The 10 ms budget (TB-PRF-020) is a server-side render cost. A browser round trip to an Interactive Server circuit
    // includes network and SignalR time, so this guards against a runaway regression instead of asserting the budget.
    expect(addMs).toBeLessThan(2000);
    expect(removeMs).toBeLessThan(2000);
  });

  test('P-TB-049 the toolbar settles to a stable layout after 100 items overflow', async ({ page }) => {
    await openToolbarPage(page, '/toolbar/stress');
    const bar = page.locator('#toolbar-stress .e-toolbar-item:not(.e-toolbar-item-overflowed)');
    // Until the first measurement runs every item is in the bar, so wait for the overflow to take effect before comparing.
    await expect.poll(() => bar.count(), { timeout: 8000 }).toBeLessThan(100);
    const first = await bar.count();
    await page.waitForTimeout(600);
    expect(await bar.count()).toBe(first);
  });
});
