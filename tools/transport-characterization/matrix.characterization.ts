import { test, expect, Page } from '@playwright/test';
import { Buffer } from 'node:buffer';
import { createHash } from 'node:crypto';

type Attempt = { path: string; name: string; chunk: number | null; size: number; digest: string; headers: Record<string, string>; form: string[] };
const cleanups = new WeakMap<Page, () => Promise<void>>();
test.afterEach(async ({ page }) => { await cleanups.get(page)?.(); });

async function setup(page: Page, scenario: string, held = false, fail = false) {
  const attempts: Attempt[] = [];
  const releases = new Map<string, () => void>();
  const settled: string[] = [];
  const errors: string[] = [];
  let failed = false;
  let draining = false;
  page.on('pageerror', e => errors.push(e.message));
  await page.addInitScript(() => {
    (window as any).r06bPending = 0;
    const send = XMLHttpRequest.prototype.send;
    XMLHttpRequest.prototype.send = function (...args: Parameters<XMLHttpRequest['send']>) {
      (window as any).r06bPending++;
      this.addEventListener('loadend', () => (window as any).r06bPending--, { once: true });
      return send.apply(this, args);
    };
  });
  await page.route('**/*', async route => {
    const req = route.request(); const url = new URL(req.url());
    if (url.origin !== 'http://localhost:5000') { errors.push('nonlocal request'); await route.abort(); return; }
    if (!url.pathname.startsWith('/r06-upload/')) { await route.continue(); return; }
    const body = req.postDataBuffer()!;
    const text = body.toString('latin1');
    const removing = url.pathname.includes('/remove-');
    const name = removing ? text.match(/name="ProbeA"\r\n\r\n([AB]\.txt)/)?.[1] || '' : text.match(/filename="([^"]+)"/)?.[1] || '';
    if (!['A.txt', 'B.txt'].includes(name)) throw new Error('Unexpected non-dummy filename');
    const c = text.match(/name="chunk-index"\r\n\r\n(\d+)/);
    const chunk = c ? Number(c[1]) : null;
    const start = text.indexOf('\r\n\r\n', text.indexOf('filename=')) + 4;
    const end = text.indexOf('\r\n--', start);
    const bytes = removing ? Buffer.alloc(0) : body.subarray(start, end);
    const all = await req.allHeaders();
    const headerEntries = Object.entries(all).filter(([k]) => k.startsWith('x-r06-'));
    const markers = Array.from(text.matchAll(/name="r06-marker"\r\n\r\n([^\r\n]*)/g), m => m[1]);
    // Fail value-free rather than silently treating a redacted non-dummy value as absent.
    if (headerEntries.some(([, v]) => !v.startsWith('DUMMY_')) || markers.some(v => !v.startsWith('DUMMY_'))) errors.push('Non-dummy probe header or form marker');
    const headers = Object.fromEntries(headerEntries.filter(([, v]) => v.startsWith('DUMMY_')));
    const form = markers.filter(v => v.startsWith('DUMMY_'));
    const attempt = { path: url.pathname, name, chunk, size: bytes.length, digest: createHash('sha256').update(bytes).digest('hex'), headers, form };
    attempts.push(attempt);
    const key = name + ':' + (removing ? 'remove' : chunk);
    if (!draining && ((held && chunk === 0) || (scenario === 'overlap-remove' && (removing || (name === 'A.txt' && chunk === 0))))) await new Promise<void>(r => releases.set(key, r));
    const shouldFail = (scenario === 'retry-exhaust' && chunk === 1) || (fail && !failed && (chunk === null || chunk === 1));
    if (shouldFail) failed = true;
    await route.fulfill({ status: shouldFail ? 500 : 200, body: 'DUMMY_RESPONSE' });
    settled.push(key);
  });
  cleanups.set(page, async () => {
    try { if (!page.isClosed() && await page.locator('#release').count()) await page.locator('#release').click({ timeout: 2000 }); }
    finally {
      draining = true; releases.forEach(r => r());
      if (!page.isClosed()) {
        try {
          await expect.poll(() => page.evaluate(() => (window as any).r06bPending)).toBe(0);
          expect(errors).toEqual([]);
        } finally {
          await test.info().attach('dummy-matrix-final', { body: Buffer.from(JSON.stringify({ attempts, settled, errors, ledger: await page.locator('#matrix-ledger').innerText() })), contentType: 'application/json' });
        }
      }
    }
  });
  await page.goto(`http://localhost:5000/inputs/uploader/transport-matrix/${scenario}`);
  await expect.poll(() => state(page, 'ProbeA')).not.toBeNull();
  const ledger = page.locator('#matrix-ledger');
  return { attempts, ledger, settled,
    async select(id = 'ProbeA', names = ['A.txt'], size = 4) {
      const data = size === 3072 ? Buffer.concat([Buffer.alloc(1024, 65), Buffer.alloc(1024, 66), Buffer.alloc(1024, 67)]) : Buffer.alloc(size, 65);
      await page.locator('#' + id).setInputFiles(names.map(name => ({ name, mimeType: 'text/plain', buffer: data })));
      await expect.poll(async () => (await state(page, id))?.files.length).toBe(names.length);
    },
    release(key: string) { const r = releases.get(key); if (!r) throw new Error('Missing held request ' + key); r(); releases.delete(key); },
    async success(name = 'A.txt', id = 'ProbeA') { await expect(ledger).toContainText(id + ':success:' + name); },
    async finish() { expect(errors).toEqual([]); await test.info().attach('dummy-matrix', { body: Buffer.from(JSON.stringify({ attempts, ledger: await ledger.innerText() })), contentType: 'application/json' }); }
  };
}

async function state(page: Page, id: string) {
  return page.evaluate(id => {
    const instance = (window as any).sfBlazorToolkit?.base?.getCompInstance(id);
    return instance ? { headers: instance.httpClientHeaders, url: instance.asyncSettings.saveUrl,
      files: instance.filesData.map((f: any) => ({ name: f.name, code: f.statusCode })), credentials: instance.withCredentials } : null;
  }, id);
}

for (const action of ['mutate', 'replace']) {
  test(`@observed config ${action} only stays stale until parent sync`, async ({ page }) => {
    const h = await setup(page, 'config');
    await page.locator('#' + action).click();
    await expect(h.ledger).toContainText('client-updated');
    expect(JSON.stringify((await state(page, 'ProbeA'))!.headers)).toContain('DUMMY_INITIAL');
    await h.select(); await page.locator('#upload-0').click(); await h.success();
    expect(h.attempts[0].headers['x-r06-default']).toBe('DUMMY_INITIAL');
    await page.locator('#sync').click();
    await expect.poll(async () => JSON.stringify((await state(page, 'ProbeA'))!.headers)).toContain(action === 'mutate' ? 'DUMMY_MUTATED' : 'DUMMY_REPLACED');
    await h.finish();
  });
}
for (const child of ['settings', 'buttons']) {
  test(`@observed child-only ${child} update uses missing module export`, async ({ page }) => {
    const h = await setup(page, 'config');
    await page.locator('#' + child).click();
    await expect(h.ledger).toContainText('child-missing-export:' + child);
    expect((await state(page, 'ProbeA'))!.url).toBe('/r06-upload/a');
    await h.finish();
  });
}

for (const reverse of [false, true]) {
  test(`@observed one instance interleaved chunks share latest headers reverse=${reverse}`, async ({ page }) => {
    const h = await setup(page, 'concurrent', true);
    await h.select('ProbeA', ['A.txt', 'B.txt'], 3072);
    const first = reverse ? 'B.txt' : 'A.txt', second = reverse ? 'A.txt' : 'B.txt';
    await page.locator(reverse ? '#upload-1' : '#upload-0').click();
    await expect.poll(() => h.attempts.length).toBe(1);
    await page.locator(reverse ? '#upload-0' : '#upload-1').click();
    await expect.poll(() => h.attempts.length).toBe(2);
    expect(h.attempts[0].headers['x-r06-explicit']).toBe('DUMMY_ProbeA_' + first);
    h.release(first + ':0'); await h.success(first);
    expect(h.attempts.find(x => x.name === first && x.chunk === 1)!.headers['x-r06-explicit']).toBe('DUMMY_ProbeA_' + second);
    h.release(second + ':0'); await h.success(second); await h.finish();
  });
}

test('@observed separate uploader instances retain their own chunk headers', async ({ page }) => {
  const h = await setup(page, 'instances', true);
  await h.select('ProbeA', ['A.txt'], 3072); await h.select('ProbeB', ['B.txt'], 3072);
  await page.locator('#upload-0').click(); await expect.poll(() => h.attempts.length).toBe(1);
  await page.locator('#upload-b').click(); await expect.poll(() => h.attempts.length).toBe(2);
  h.release('A.txt:0'); await h.success(); h.release('B.txt:0'); await h.success('B.txt', 'ProbeB');
  for (const a of h.attempts) expect(a.headers['x-r06-explicit']).toBe(a.name === 'A.txt' ? 'DUMMY_ProbeA_A.txt' : 'DUMMY_ProbeB_B.txt');
  await h.finish();
});

for (const mode of ['null', 'empty']) {
  test(`@observed ${mode} header result and true to false credential state`, async ({ page }) => {
    const h = await setup(page, 'reset'); await h.select('ProbeA', ['A.txt', 'B.txt']);
    await page.locator('#true').click(); await page.locator('#upload-0').click(); await h.success();
    await page.locator('#false').click(); await page.locator('#' + mode).click(); await page.locator('#upload-1').click(); await h.success('B.txt');
    expect(h.attempts.map(a => a.name)).toEqual(['A.txt', 'B.txt']);
    await expect(h.ledger).toContainText('decision:A.txt:file:True');
    await expect(h.ledger).toContainText('decision:B.txt:' + mode + ':False');
    expect(h.attempts[1].headers['x-r06-explicit']).toBe(mode === 'null' ? 'DUMMY_ProbeA_A.txt' : undefined);
    await expect(h.ledger).toContainText('success:B.txt:True'); // XHR flag only, no cookie-policy claim.
    await h.finish();
  });
}

for (const scenario of ['endpoint-pending', 'endpoint-chunks']) {
  test(`@observed ${scenario} child failure then explicit parent resync retargets work`, async ({ page }) => {
    const h = await setup(page, scenario, scenario === 'endpoint-chunks');
    await h.select('ProbeA', ['A.txt'], scenario === 'endpoint-chunks' ? 3072 : 4);
    if (scenario === 'endpoint-pending') await page.locator('#hold').click();
    await page.locator('#upload-0').click();
    if (scenario === 'endpoint-chunks') await expect.poll(() => h.attempts.length).toBe(1);
    else await expect(h.ledger).toContainText('before:A.txt');
    await page.locator('#settings').click(); await expect(h.ledger).toContainText('child-missing-export:settings');
    await page.locator('#sync').click(); await expect.poll(async () => (await state(page, 'ProbeA'))!.url).toBe('/r06-upload/b');
    if (scenario === 'endpoint-chunks') h.release('A.txt:0'); else await page.locator('#release').click();
    try { await h.success(); }
    finally { await h.finish(); }
    expect(h.attempts.map(a => a.path)).toEqual(scenario === 'endpoint-chunks' ? ['/r06-upload/a', '/r06-upload/b', '/r06-upload/b'] : ['/r06-upload/b']);
  });
}

for (const removalMode of ['file', 'null', 'empty']) {
test(`@observed completed-file removal ${removalMode} changes headers and form data of another active upload`, async ({ page }) => {
  const h = await setup(page, 'overlap-remove');
  await h.select('ProbeA', ['A.txt', 'B.txt'], 3072);
  await page.locator('#upload-0').click(); await expect.poll(() => h.attempts.length).toBe(1);
  await page.locator('#upload-1').click(); await h.success('B.txt');
  if (removalMode !== 'file') await page.locator('#' + removalMode).click();
  await page.locator('#remove-1').click();
  await expect(h.ledger).toContainText('remove-decision:B.txt:' + removalMode);
  await expect.poll(() => h.attempts.some(a => a.path.includes('/remove-'))).toBe(true);
  h.release('A.txt:0'); await h.success();
  const removal = h.attempts.find(a => a.path.includes('/remove-'))!;
  expect(removal.headers['x-r06-explicit']).toBe(removalMode === 'file' ? 'DUMMY_REMOVE_B.txt' : undefined);
  expect(removal.form).toEqual(['DUMMY_REMOVE_B.txt']);
  const later = h.attempts.filter(a => a.name === 'A.txt' && a.chunk !== 0);
  expect(later.map(a => a.chunk)).toEqual([1, 2]);
  for (const a of later) {
    expect(a.headers['x-r06-explicit']).toBe(removalMode === 'file' ? 'DUMMY_REMOVE_B.txt' : undefined);
    expect(a.form).toEqual(['DUMMY_REMOVE_B.txt']);
  }
  h.release('B.txt:remove');
  await expect(h.ledger).toContainText('operation:remove:B.txt');
  await h.finish();
});
}

test('@observed clear retains request state until reselection and does not reset credentials', async ({ page }) => {
  const h = await setup(page, 'reset'); await h.select();
  await page.locator('#true').click(); await page.locator('#upload-0').click(); await h.success();
  await page.locator('#clear').click();
  await expect.poll(async () => (await state(page, 'ProbeA'))!.files.length).toBe(0);
  await expect(page.locator('#ProbeA')).toHaveValue('');
  const retained = await page.evaluate(() => {
    const x = (window as any).sfBlazorToolkit.base.getCompInstance('ProbeA');
    return { headers: x.currentRequestHeader, form: x.customFormDatas, credentials: x.withCredentials };
  });
  expect(retained).toEqual({ headers: [{ 'X-R06-Explicit': 'DUMMY_ProbeA_A.txt' }], form: [{ 'r06-marker': 'DUMMY_ProbeA_A.txt' }], credentials: true });
  await page.locator('#false').click(); await page.locator('#empty').click();
  await h.select('ProbeA', ['B.txt']); await page.locator('#upload-0').click(); await h.success('B.txt');
  expect(h.attempts).toHaveLength(2);
  expect(h.attempts[1].headers).toEqual({});
  await expect(h.ledger).toContainText('decision:B.txt:empty:False');
  await expect(h.ledger).toContainText('success:B.txt:True');
  await h.finish();
});

test('@observed retry-stage advances bytes without advancing the failed chunk index', async ({ page }) => {
  const h = await setup(page, 'retry-chunks', false, true);
  await h.select('ProbeA', ['A.txt'], 3072);
  await page.locator('#upload-0').click();
  await expect(h.ledger).toContainText('failure:A.txt');
  await expect.poll(async () => (await state(page, 'ProbeA'))!.files[0].code).toBe('0');
  await page.locator('#retry-stage').click(); await h.success();
  const digest = (byte: number, size = 1024) => createHash('sha256').update(Buffer.alloc(size, byte)).digest('hex');
  expect(h.attempts.map(a => [a.chunk, a.size, a.digest])).toEqual([
    [0, 1024, digest(65)], [1, 1024, digest(66)], [1, 1024, digest(67)], [2, 0, digest(0, 0)]
  ]);
  expect((await h.ledger.innerText()).split('\n').filter(x => x === 'ProbeA:before:A.txt')).toHaveLength(1);
  await h.finish();
});

test('@observed configured retry budget exhausts after two retries of the same bytes', async ({ page }) => {
  const h = await setup(page, 'retry-exhaust');
  await h.select('ProbeA', ['A.txt'], 3072); await page.locator('#upload-0').click();
  await expect(h.ledger).toContainText('failure:A.txt');
  await expect.poll(async () => (await state(page, 'ProbeA'))!.files[0].code).toBe('0');
  expect(h.attempts.map(a => a.chunk)).toEqual([0, 1, 1, 1]);
  for (const a of h.attempts.slice(1)) {
    expect(a.size).toBe(1024);
    expect(a.digest).toBe(createHash('sha256').update(Buffer.alloc(1024, 66)).digest('hex'));
  }
  const lines = (await h.ledger.innerText()).split('\n');
  expect(lines.filter(x => x === 'ProbeA:chunk-failed:1')).toHaveLength(3);
  expect(lines.filter(x => x === 'ProbeA:failure:A.txt')).toHaveLength(1);
  expect(lines.filter(x => x.startsWith('ProbeA:success:'))).toHaveLength(0);
  await h.finish();
});

for (const scenario of ['retry-ordinary', 'retry-chunks']) {
  test(`@observed ${scenario} manual restart callback and byte sequence`, async ({ page }) => {
    const h = await setup(page, scenario, false, true);
    await h.select('ProbeA', ['A.txt'], scenario === 'retry-chunks' ? 3072 : 4);
    await page.locator('#upload-0').click(); await expect(h.ledger).toContainText('failure:A.txt');
    await page.locator('#retry').click(); await h.success();
    expect(h.attempts.map(a => a.chunk)).toEqual(scenario === 'retry-chunks' ? [0, 1, 0, 1, 2] : [null, null]);
    expect((await h.ledger.innerText()).split('\n').filter(x => x === 'ProbeA:before:A.txt')).toHaveLength(scenario === 'retry-chunks' ? 1 : 2);
    if (scenario === 'retry-chunks') expect(h.attempts[0].digest).toBe(h.attempts[2].digest);
    await h.finish();
  });
}
