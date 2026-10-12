import { test, expect } from '@playwright/test';
import { Buffer } from 'node:buffer';

// Dedicated configuration and fresh host per held chunk; not default observation discovery.
test('later/final pause and repeated controls preserve their observed outcomes', async ({ page, request }) => {
  const chunk = Number(process.env.R06_HOLD_CHUNK);
  expect([1, 2]).toContain(chunk);
  const errors: string[] = [];
  page.on('pageerror', e => errors.push(e.message));
  await page.addInitScript(() => { (window as any).r06Controls = []; });
  await page.route('**/*', async route => {
    const u = new URL(route.request().url());
    if (u.origin !== 'http://localhost:5000' || u.pathname.startsWith('/r06-upload/')) { errors.push('unexpected request'); await route.abort(); }
    else await route.continue();
  });
  const live = async () => { const r = await request.get('http://localhost:5000/r06-live/state'); expect(r.status()).toBe(200); return r.json(); };
  const release = async () => { const r = await request.post('http://localhost:5000/r06-live/release', { headers: { 'X-R06-Control': 'DUMMY_CONTROL' } }); expect(r.status()).toBe(200); };
  try {
    expect(await live()).toEqual({ chunks: [], active: 0, completed: 0, timedOut: false });
    await page.goto('http://localhost:5000/inputs/uploader/transport-matrix/pause');
    await expect.poll(() => page.evaluate(() => !!(window as any).sfBlazorToolkit?.base?.getCompInstance('ProbeA'))).toBe(true);
    await page.evaluate(() => {
      const x = (window as any).sfBlazorToolkit.base.getCompInstance('ProbeA');
      for (const name of ['pause', 'resume']) {
        const original = x[name];
        x[name] = function (...args: unknown[]) { try { return original.apply(this, args); } finally { (window as any).r06Controls.push(name); } };
      }
    });
    const code = () => page.evaluate(() => (window as any).sfBlazorToolkit.base.getCompInstance('ProbeA').filesData[0]?.statusCode);
    const data = Buffer.concat([Buffer.alloc(1024, 65), Buffer.alloc(1024, 66), Buffer.alloc(1024, 67)]);
    await page.locator('#ProbeA').setInputFiles({ name: 'A.txt', mimeType: 'text/plain', buffer: data });
    await expect.poll(code).toBe('1'); await page.locator('#upload-0').click();
    await expect.poll(async () => (await live()).chunks).toEqual(chunk === 1 ? [0, 1] : [0, 1, 2]);
    // Natural progress registered metadata for the target held chunk, not the previous request.
    await expect.poll(() => page.evaluate(() => {
      const x = (window as any).sfBlazorToolkit.base.getCompInstance('ProbeA');
      return x.uploadMetaData.at(-1)?.chunkIndex;
    })).toBe(chunk);
    await expect.poll(code).toBe('3');
    await page.locator('#pause').click(); await expect.poll(code).toBe('4');
    const ledger = page.locator('#matrix-ledger');
    await expect(ledger).toContainText('ProbeA:paused:' + chunk);
    await page.locator('#pause').click();
    await expect.poll(() => page.evaluate(() => (window as any).r06Controls.filter((x: string) => x === 'pause').length)).toBe(2);
    expect((await ledger.innerText()).split('\n').filter(x => x.startsWith('ProbeA:paused:'))).toHaveLength(1);
    if (process.env.R06_RESUME_PENDING === '1') {
      await page.locator('#resume').click();
      await expect(ledger).toContainText('ProbeA:resumed:' + chunk);
      if (chunk === 2) {
        // This probe retains any final-chunk resume error; do not patch/catch it in the component.
        await release(); await expect.poll(async () => (await live()).active).toBe(0);
        expect(await live()).toEqual({ chunks: [0, 1, 2], active: 0, completed: 3, timedOut: false });
        await expect(ledger).toContainText('ProbeA:success:A.txt');
        expect(errors).toEqual([]);
        return;
      }
      // Original response remains held; a valid nonfinal resume starts another actual request.
      await expect(ledger).toContainText('ProbeA:success:A.txt');
      expect((await live()).active).toBe(1);
      await release(); await expect.poll(async () => (await live()).active).toBe(0);
      await expect.poll(async () => (await ledger.innerText()).split('\n').filter(x => x.startsWith('ProbeA:success:')).length).toBe(2);
      expect((await live()).chunks).toEqual([0, 1, 1, 2]);
      expect(errors).toEqual([]);
      await test.info().attach('dummy-outstanding-resume', { body: Buffer.from(JSON.stringify({ result: await live(), lines: (await ledger.innerText()).split('\n') })), contentType: 'application/json' });
      return;
    }
    await release(); await expect.poll(async () => (await live()).active).toBe(0);
    await expect(ledger).toContainText('ProbeA:chunk:A.txt:' + chunk);
    await expect.poll(code).toBe(chunk === 1 ? '4' : '2');
    await page.locator('#resume').click();
    await expect(ledger).toContainText('ProbeA:success:A.txt');
    await expect.poll(code).toBe('2');
    await page.locator('#resume').click();
    await expect.poll(() => page.evaluate(() => (window as any).r06Controls.filter((x: string) => x === 'resume').length)).toBe(2);
    const result = await live();
    expect(result).toEqual({ chunks: chunk === 1 ? [0, 1, 1, 2] : [0, 1, 2], active: 0, completed: chunk === 1 ? 4 : 3, timedOut: false });
    const lines = (await ledger.innerText()).split('\n');
    expect(lines.filter(x => x.startsWith('ProbeA:resumed:'))).toHaveLength(chunk === 1 ? 1 : 0);
    expect(lines.filter(x => x.startsWith('ProbeA:success:'))).toHaveLength(1);
    expect(errors).toEqual([]);
    await test.info().attach('dummy-pause-variant', { body: Buffer.from(JSON.stringify({ result, lines })), contentType: 'application/json' });
  } finally {
    await release();
    await expect.poll(async () => (await live()).active).toBe(0);
    await test.info().attach('dummy-pause-final-state', { body: Buffer.from(JSON.stringify({ chunk, pendingResume: process.env.R06_RESUME_PENDING === '1', result: await live(), errors, ledger: await page.locator('#matrix-ledger').innerText() })), contentType: 'application/json' });
  }
});
