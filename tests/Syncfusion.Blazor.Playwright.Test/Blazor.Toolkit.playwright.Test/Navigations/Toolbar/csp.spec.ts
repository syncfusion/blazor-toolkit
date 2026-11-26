import { test, expect } from '@playwright/test';
import { BASE } from './helpers';

const CSP = "default-src 'self'; script-src 'self'; style-src 'self'; img-src 'self' data:; connect-src 'self' ws: wss:; font-src 'self' data:; object-src 'none'; base-uri 'self'";

test.describe('Toolbar under a strict CSP @toolbar', () => {
  test('P-TB-047 operating the toolbar raises no CSP violation attributable to it', async ({ page }) => {
    await page.route(`${BASE}/toolbar/csp`, async (route) => {
      const response = await route.fetch();
      await route.fulfill({ response, headers: { ...response.headers(), 'content-security-policy': CSP } });
    });
    await page.addInitScript(() => {
      (window as unknown as { __csp: string[] }).__csp = [];
      document.addEventListener('securitypolicyviolation', (e) => {
        (window as unknown as { __csp: string[] }).__csp.push(`${e.violatedDirective} | ${e.sourceFile ?? ''}:${e.lineNumber} | ${e.sample ?? ''}`);
      });
    });
    const errors: string[] = [];
    page.on('console', (m) => { if (m.type() === 'error') { errors.push(m.text()); } });

    await page.goto(`${BASE}/toolbar/csp`, { waitUntil: 'commit' });
    await expect(page.locator('#interactive-ready')).toHaveAttribute('data-ready', 'true', { timeout: 15000 });
    await expect(page.locator('#toolbar-csp [data-testid="toolbar-item"]').first()).toBeVisible();
    const read = () => page.evaluate(() => (window as unknown as { __csp: string[] }).__csp);
    // Violations raised by the host page and the Blazor framework script exist before the toolbar is touched.
    const atLoad = await read();
    await page.locator('#width-narrow').click();
    const more = page.locator('#toolbar-csp [data-testid="toolbar-overflow-button"]');
    await expect(more).toHaveCount(1, { timeout: 8000 });
    await more.click();
    const popup = page.locator('#toolbar-csp [data-testid="toolbar-overflow-popup"]');
    await expect(popup).toBeVisible();
    const placement = await popup.evaluate((element) => {
      const bounds = element.getBoundingClientRect();
      const anchor = document.querySelector('#toolbar-csp [data-testid="toolbar-overflow-button"]');
      if (!anchor) {
        throw new Error('The overflow button was not rendered.');
      }
      const anchorBounds = anchor.getBoundingClientRect();
      return {
        position: getComputedStyle(element).position,
        left: bounds.left,
        top: bounds.top,
        right: bounds.right,
        bottom: bounds.bottom,
        anchorRight: anchorBounds.right,
        anchorBottom: anchorBounds.bottom,
        viewportWidth: window.innerWidth,
        viewportHeight: window.innerHeight
      };
    });
    expect(placement.position).toBe('fixed');
    expect(placement.left).toBeGreaterThanOrEqual(0);
    expect(placement.top).toBeGreaterThanOrEqual(0);
    expect(placement.right).toBeLessThanOrEqual(placement.viewportWidth);
    expect(placement.bottom).toBeLessThanOrEqual(placement.viewportHeight);
    expect(Math.abs(placement.right - placement.anchorRight)).toBeLessThanOrEqual(1);
    expect(Math.abs(placement.top - placement.anchorBottom)).toBeLessThanOrEqual(1);
    await page.keyboard.press('Escape');
    await page.locator('#toolbar-csp #c-one button').click();
    await expect(page.locator('#last-click')).toHaveText('Last click: c-one');

    const afterUse = await read();
    const added = afterUse.slice(atLoad.length);
    const fromToolbarScript = afterUse.filter((v) => v.includes('toolbar.js'));
    expect(fromToolbarScript, `toolbar.js violations: ${fromToolbarScript.join(' || ')}`).toEqual([]);
    expect(added, `violations raised while operating the toolbar: ${added.join(' || ')}; console: ${errors.join(' | ')}`).toEqual([]);
  });
});
