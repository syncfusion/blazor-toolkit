import { test, expect } from '@playwright/test';
import { Buffer } from 'node:buffer';

for (const action of ['mutate', 'replace']) {
  test(`@observed ordinary Razor parent ${action} requires tracked synchronization and preserves selection`, async ({ page }) => {
    const errors: string[] = [];
    page.on('pageerror', e => errors.push(e.message));
    const sent: string[] = [];
    await page.route('**/*', async route => {
      const req = route.request(), url = new URL(req.url());
      if (url.origin !== 'http://localhost:5000') { errors.push('external request'); await route.abort(); return; }
      if (url.pathname.startsWith('/r06-upload/')) {
        const value = await req.headerValue('x-r06-default');
        if (!value?.startsWith('DUMMY_')) throw new Error('Unexpected non-dummy header');
        sent.push(value);
        await route.fulfill({ status: 200, body: 'DUMMY_RESPONSE' });
      } else await route.continue();
    });
    await page.goto('http://localhost:5000/inputs/uploader/transport-parent');
    const state = () => page.evaluate(() => {
      const x = (window as any).sfBlazorToolkit?.base?.getCompInstance('ParentProbe');
      return x ? { headers: x.httpClientHeaders, files: x.filesData.map((f: any) => ({ id: f.id, name: f.name, raw: f.rawFile instanceof File })) } : null;
    });
    await expect.poll(state).not.toBeNull();
    await page.locator('#ParentProbe').setInputFiles({ name: 'A.txt', mimeType: 'text/plain', buffer: Buffer.from('DUMMY') });
    await expect.poll(async () => (await state())!.files.length).toBe(1);
    const files = (await state())!.files;
    await page.locator('#parent-' + action).click();
    await expect(page.locator('#parent-generation')).toHaveText('1');
    expect((await state())!.files).toEqual(files);
    expect(JSON.stringify((await state())!.headers)).toContain('DUMMY_INITIAL');
    await page.locator('#parent-sync').click();
    const expected = action === 'mutate' ? 'DUMMY_MUTATED' : 'DUMMY_REPLACED';
    await expect.poll(async () => JSON.stringify((await state())!.headers)).toContain(expected);
    expect((await state())!.files).toEqual(files);
    // Use the component's public module export, not mutated internal state.
    await page.evaluate(async () => { const path = '/_content/Syncfusion.Blazor.Toolkit/scripts/uploader.js'; (await import(path)).upload('ParentProbe', null, false); });
    await expect.poll(() => sent.length).toBe(1);
    expect(sent).toEqual([expected]);
    await expect(page.locator('.e-file-status')).toHaveText('File uploaded successfully');
    await expect.poll(() => page.evaluate(() => (window as any).sfBlazorToolkit.base.getCompInstance('ParentProbe').filesData[0].statusCode)).toBe('2');
    expect(errors).toEqual([]);
  });
}
