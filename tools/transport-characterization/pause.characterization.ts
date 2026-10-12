import { test, expect } from '@playwright/test';
import { Buffer } from 'node:buffer';

test('@observed bounded live loopback pause resume retransmits the settled first chunk', async ({ page, request }) => {
  const errors: string[] = [];
  page.on('pageerror', e => errors.push(e.message));
  await page.route('**/*', async route => {
    const url = new URL(route.request().url());
    if (url.origin !== 'http://localhost:5000' || url.pathname.startsWith('/r06-upload/')) { errors.push('unexpected request'); await route.abort(); }
    else await route.continue();
  });
  const liveState = async () => { const response = await request.get('http://localhost:5000/r06-live/state'); expect(response.status()).toBe(200); return response.json(); };
  const release = async () => { const response = await request.post('http://localhost:5000/r06-live/release', { headers: { 'X-R06-Control': 'DUMMY_CONTROL' } }); expect(response.status()).toBe(200); };
  try {
    expect(await liveState()).toEqual({ chunks: [], active: 0, completed: 0, timedOut: false });
    await page.goto('http://localhost:5000/inputs/uploader/transport-matrix/pause');
    await expect.poll(() => page.evaluate(() => !!(window as any).sfBlazorToolkit?.base?.getCompInstance('ProbeA'))).toBe(true);
    const code = () => page.evaluate(() => (window as any).sfBlazorToolkit.base.getCompInstance('ProbeA').filesData[0]?.statusCode);
    const data = Buffer.concat([Buffer.alloc(1024, 65), Buffer.alloc(1024, 66), Buffer.alloc(1024, 67)]);
    await page.locator('#ProbeA').setInputFiles({ name: 'A.txt', mimeType: 'text/plain', buffer: data });
    await expect.poll(code).toBe('1');
    await page.locator('#upload-0').click();
    await expect.poll(async () => (await liveState()).chunks).toEqual([0]);
    await expect.poll(code).toBe('3'); // Actual progress, never assigned by the test.
    await page.locator('#pause').click();
    await expect(page.locator('#matrix-ledger')).toContainText('ProbeA:paused:0');
    await expect.poll(code).toBe('4');
    await release();
    await expect.poll(async () => (await liveState()).completed).toBe(1);
    await expect(page.locator('#matrix-ledger')).toContainText('ProbeA:chunk:A.txt:0');
    await expect.poll(code).toBe('4');
    expect((await liveState()).chunks).toEqual([0]);
    await page.locator('#resume').click();
    await expect(page.locator('#matrix-ledger')).toContainText('ProbeA:resumed:0');
    await expect(page.locator('#matrix-ledger')).toContainText('ProbeA:success:A.txt');
    const observed = await liveState();
    expect(observed).toEqual({ chunks: [0, 0, 1, 2], active: 0, completed: 4, timedOut: false });
    expect(errors).toEqual([]);
    await test.info().attach('dummy-loopback-state', { body: Buffer.from(JSON.stringify({ observed, ledger: await page.locator('#matrix-ledger').innerText() })), contentType: 'application/json' });
  } finally { await release(); await expect.poll(async () => (await liveState()).active).toBe(0); }
});
