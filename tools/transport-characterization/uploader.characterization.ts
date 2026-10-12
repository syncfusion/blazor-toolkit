import { test, expect, Page } from '@playwright/test';
import { Buffer } from 'node:buffer';

type RequestRecord = { path: string; headers: Record<string, string>; chunk: number | null };
type Browser = Window & { r06Pending: number; r06Consumed: string[]; sfBlazorToolkit: { base: { getCompInstance(id: string): {
  httpClientHeaders: unknown; cssClass: string; filesData: { statusCode: string }[];
} } } };
const cleanups = new WeakMap<Page, () => Promise<void>>();
test.afterEach(async ({ page }) => { await cleanups.get(page)?.(); });

async function open(page: Page, scenario: string, hold: string | null = null) {
  const requests: RequestRecord[] = [];
  const errors: string[] = [];
  const external: string[] = [];
  const failures: number[] = [];
  let releaseResponse!: () => void;
  const responseGate = new Promise<void>(resolve => { releaseResponse = resolve; });
  page.on('pageerror', error => errors.push(error.message));
  await page.addInitScript(() => {
    const browser = window as unknown as Browser;
    browser.r06Pending = 0;
    browser.r06Consumed = [];
    const send = XMLHttpRequest.prototype.send;
    XMLHttpRequest.prototype.send = function (...args: Parameters<XMLHttpRequest['send']>) {
      browser.r06Pending++;
      this.addEventListener('loadend', () => browser.r06Pending--, { once: true });
      return send.apply(this, args);
    };
  });
  cleanups.set(page, async () => {
    try {
      if (!page.isClosed()) {
        const button = page.locator('#r06-release');
        if (await button.count()) await button.click({ timeout: 2000 });
      }
    } finally {
      releaseResponse();
      if (!page.isClosed()) {
        await expect.poll(() => page.evaluate(() => (window as unknown as Browser).r06Pending)).toBe(0);
        expect(errors).toEqual([]);
      }
    }
  });
  await page.route('**/*', async route => {
    const request = route.request();
    const url = new URL(request.url());
    if (url.origin !== 'http://localhost:5000') { external.push(url.origin); await route.abort(); return; }
    if (url.pathname.startsWith('/r06-upload/')) {
      const match = request.postDataBuffer()?.toString().match(/name="chunk-index"\r\n\r\n(\d+)/);
      const chunk = match ? Number(match[1]) : null;
      // Retain only known fixture dummy headers; never cookies or browser/session headers.
      const actual = await request.allHeaders();
      const headers = Object.fromEntries(Object.entries(actual).filter(([key, value]) =>
        (key.startsWith('x-r06-') || key === 'authorization') && /^(Bearer )?DUMMY_/.test(value)));
      requests.push({ path: url.pathname, headers, chunk });
      if (url.pathname === hold || (scenario === 'chunk-late' && chunk === 2)) await responseGate;
      const fail = scenario === 'chunk-retry' && chunk === 1 && !failures.includes(1);
      if (fail) failures.push(1);
      await route.fulfill({ status: fail ? 500 : 200, contentType: 'text/plain', body: 'local dummy response' });
      return;
    }
    await route.continue();
  });
  await page.goto(`http://localhost:5000/inputs/uploader/transport/${scenario}`);
  await expect.poll(() => page.evaluate(() => !!(window as unknown as Browser).sfBlazorToolkit?.base?.getCompInstance('R06Uploader'))).toBe(true);
  // Transparent observation only: original callbacks execute synchronously and unchanged.
  await page.evaluate(() => {
    const browser = window as unknown as Browser;
    const instance = browser.sfBlazorToolkit.base.getCompInstance('R06Uploader') as unknown as Record<string, (...args: unknown[]) => unknown>;
    for (const method of ['beforeUploadCallback', 'beforeRemoveCallback', 'uploadCallback', 'removeCallback']) {
      const original = instance[method];
      instance[method] = function (...args: unknown[]) {
        try { return original.apply(this, args); }
        finally { browser.r06Consumed.push(method); }
      };
    }
  });
  const ledger = page.locator('#r06-ledger');
  return { requests, ledger, releaseResponse,
    async select(size = 4) {
      await page.locator('#R06Uploader').setInputFiles({ name: 'dummy.txt', mimeType: 'text/plain', buffer: Buffer.alloc(size, 65) });
      await expect(page.locator('.e-upload-file-list')).toHaveCount(1);
    },
    async complete() {
      await expect(ledger).toContainText('success:upload');
      await expect(page.locator('#r06-handler-sends')).toHaveText('0');
    },
    async finish() {
      releaseResponse();
      await expect.poll(() => page.evaluate(() => (window as unknown as Browser).r06Pending)).toBe(0);
      expect(external).toEqual([]);
      expect(errors).toEqual([]);
      await test.info().attach('dummy-transport-observation', { body: Buffer.from(JSON.stringify({ requests, ledger: await ledger.innerText() }, null, 2)), contentType: 'application/json' });
    }
  };
}

async function options(page: Page) {
  return page.evaluate(() => (window as unknown as Browser).sfBlazorToolkit.base.getCompInstance('R06Uploader').httpClientHeaders);
}

test('@observed default headers reach initialization, parent update and browser upload', async ({ page }) => {
  const h = await open(page, 'defaults');
  expect(JSON.stringify(await options(page))).toContain('DUMMY_BACKEND_INITIAL');
  expect(JSON.stringify(await options(page))).toContain('DUMMY_PRIVILEGED');
  expect(h.requests).toHaveLength(0);
  await page.locator('#r06-update').click();
  await expect.poll(async () => JSON.stringify(await options(page))).toContain('DUMMY_BACKEND_UPDATED');
  await h.select();
  await page.locator('#r06-upload').click();
  await h.complete();
  expect(h.requests).toHaveLength(1);
  expect(h.requests[0].headers['x-r06-backend']).toBe('DUMMY_BACKEND_UPDATED');
  expect(h.requests[0].headers.authorization).toBe('Bearer DUMMY_PRIVILEGED');
  expect(h.requests[0].headers['x-r06-explicit']).toBe('DUMMY_BEFORE_UPLOAD');
  await h.finish();
});

for (const stage of ['before-upload', 'before-remove']) {
  for (const cancel of [false, true]) {
    test(`@observed ${stage} awaits decision; cancel=${cancel}`, async ({ page }) => {
      const h = await open(page, stage + (cancel ? '-cancel' : ''));
      await h.select();
      await page.locator('#r06-upload').click();
      if (stage === 'before-remove') { await h.complete(); await page.locator('#r06-remove').click(); }
      await expect(h.ledger).toContainText(stage + ':entered');
      await page.locator('#r06-barrier').click();
      await expect(h.ledger).toContainText('barrier');
      const path = stage === 'before-upload' ? '/r06-upload/save' : '/r06-upload/remove';
      expect(h.requests.filter(r => r.path === path)).toHaveLength(0);
      await page.locator('#r06-release').click();
      await expect(h.ledger).toContainText(stage + ':released');
      if (cancel) {
        const method = stage === 'before-upload' ? 'beforeUploadCallback' : 'beforeRemoveCallback';
        await expect.poll(() => page.evaluate(method => (window as unknown as Browser).r06Consumed.includes(method), method)).toBe(true);
        expect(h.requests.filter(r => r.path === path)).toHaveLength(0);
      } else {
        await expect.poll(() => h.requests.filter(r => r.path === path).length).toBe(1);
        expect(h.requests.find(r => r.path === path)!.headers['x-r06-explicit']).toBe(stage === 'before-upload' ? 'DUMMY_BEFORE_UPLOAD' : 'DUMMY_BEFORE_REMOVE');
      }
      await h.finish();
    });
  }
}

for (const stage of ['upload-start', 'remove-start']) {
  test(`@observed ${stage} dispatches while callback remains pending`, async ({ page }) => {
    const path = stage === 'upload-start' ? '/r06-upload/save' : '/r06-upload/remove';
    const h = await open(page, stage + '-cancel', path);
    try {
      await h.select();
      await page.locator('#r06-upload').click();
      if (stage === 'remove-start') { await h.complete(); await page.locator('#r06-remove').click(); }
      await expect(h.ledger).toContainText(stage + ':entered');
      await expect.poll(() => h.requests.filter(r => r.path === path).length).toBe(1);
      await expect(h.ledger).not.toContainText(stage + ':released');
      expect(h.requests.find(r => r.path === path)!.headers['x-r06-explicit']).toBe(stage === 'upload-start' ? 'DUMMY_BEFORE_UPLOAD' : 'DUMMY_BEFORE_REMOVE');
      await page.locator('#r06-release').click();
      await expect(h.ledger).toContainText(stage + ':released');
      await h.finish();
    } finally { h.releaseResponse(); }
  });
}

for (const scenario of ['chunks', 'chunk-retry', 'chunk-late']) {
  test(`@observed ${scenario} records actual attempts and callbacks`, async ({ page }) => {
    const h = await open(page, scenario);
    try {
      await h.select(3072);
      await page.locator('#r06-upload').click();
      if (scenario === 'chunk-late') {
        await expect(h.ledger).toContainText('chunk-start:2');
        await expect.poll(() => h.requests.some(r => r.chunk === 2)).toBe(true);
        await expect(h.ledger).not.toContainText('chunk-released:2');
        await page.locator('#r06-release').click();
        await expect(h.ledger).toContainText('chunk-released:2');
        h.releaseResponse();
      }
      await h.complete();
      expect(h.requests.map(r => r.chunk)).toEqual(scenario === 'chunk-retry' ? [0, 1, 1, 2] : [0, 1, 2]);
      const text = await h.ledger.innerText();
      expect(text.split('\n').filter(x => x === 'success:upload')).toHaveLength(1);
      expect(text.split('\n').filter(x => x.startsWith('chunk-success:'))).toEqual(['chunk-success:0', 'chunk-success:1', 'chunk-success:2']);
      expect(text).not.toContain('chunk-start:0');
      if (scenario === 'chunk-retry') expect(text).toContain('chunk-failure:1');
      for (const r of h.requests) expect(r.headers['x-r06-backend']).toContain('DUMMY_BACKEND_INITIAL');
      await h.finish();
    } finally { h.releaseResponse(); }
  });
}

test('@observed direct removal skips OnRemove but retains BeforeRemove', async ({ page }) => {
  const h = await open(page, 'defaults');
  await h.select(); await page.locator('#r06-upload').click(); await h.complete();
  await page.locator('#r06-direct-remove').click();
  await expect.poll(() => h.requests.filter(r => r.path.endsWith('/remove')).length).toBe(1);
  await expect(h.ledger).toContainText('before-remove:released');
  await expect(h.ledger).not.toContainText('remove-start:entered');
  await h.finish();
});

test('@policy backend defaults must not be exported', async ({ page }) => {
  const h = await open(page, 'defaults');
  try { expect(JSON.stringify(await options(page))).not.toContain('DUMMY_'); }
  finally { await h.finish(); }
});

test('@policy no upload dispatch while OnUploadStart decision is pending', async ({ page }) => {
  const h = await open(page, 'upload-start-cancel', '/r06-upload/save');
  try {
    await h.select(); await page.locator('#r06-upload').click();
    await expect(h.ledger).toContainText('upload-start:entered');
    await page.locator('#r06-barrier').click();
    await expect(h.ledger).toContainText('barrier');
    expect(h.requests).toHaveLength(0);
  } finally { await page.locator('#r06-release').click(); h.releaseResponse(); await h.finish(); }
});
