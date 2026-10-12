import { test, expect, Page } from '@playwright/test';
import { Buffer } from 'node:buffer';

const probe = (part: string) => `<img src=r05-probe-${part} onerror="self['r05Executed']=true">`;
const name = 'report' + probe('name');
const extension = 'txt' + probe('type');
const status = 'Ready ' + probe('status') + ' &amp;';
const metadataSelector = '.e-file-name, .e-file-type, .e-file-status, .template-name, .template-type, .template-status';

type UploaderWindow = Window & {
  r05Executed: boolean;
  r05Unexpected: string[];
  r05Constructions: number;
  sfBlazorToolkit: { base: { getCompInstance: (id: string) => any }; SanitizeHtmlHelper: any };
};

async function open(page: Page, rendering: string, sanitizer: string, source: string) {
  const requests: string[] = [];
  const errors: string[] = [];
  page.on('pageerror', error => errors.push(error.message));
  await page.route('**/*', async route => {
    const url = new URL(route.request().url());
    if (url.pathname.includes('r05-probe-') || url.origin !== 'http://localhost:5000' || url.pathname.startsWith('/r05-upload/')) {
      requests.push(url.href);
      await route.abort();
    } else await route.continue();
  });
  await page.addInitScript(selector => {
    const browser = window as unknown as UploaderWindow;
    browser.r05Executed = false;
    browser.r05Unexpected = [];
    browser.r05Constructions = 0;
    customElements.define('r05-marker', class extends HTMLElement {
      constructor() { super(); browser.r05Constructions++; }
    });
    new MutationObserver(records => {
      for (const record of records) {
        const target = record.target instanceof Element ? record.target.closest(selector) : null;
        for (const node of Array.from(record.addedNodes)) {
          if (!(node instanceof Element)) continue;
          if (target) browser.r05Unexpected.push(node.outerHTML);
          if (node.matches(selector)) node.querySelectorAll('*').forEach(child => browser.r05Unexpected.push(child.outerHTML));
          node.querySelectorAll(selector).forEach(root => root.querySelectorAll('*').forEach(child => browser.r05Unexpected.push(child.outerHTML)));
        }
      }
    }).observe(document, { subtree: true, childList: true });
  }, metadataSelector);
  await page.goto(`http://localhost:5000/inputs/uploader/metadata/${rendering}/${sanitizer}/${source}`);
  await expect.poll(() => page.evaluate(() => {
    const instance = (window as unknown as UploaderWindow).sfBlazorToolkit?.base?.getCompInstance('MetadataUploader');
    return instance ? { ajax: !instance.isSaveUrlNotConfigured, template: !!instance.isValidTemplate, sanitizer: instance.enableHtmlSanitizer } : null;
  })).toEqual({ ajax: rendering.includes('ajax'), template: rendering.startsWith('template'), sanitizer: sanitizer === 'on' });
  return async () => {
    await page.evaluate(() => new Promise<void>(resolve => requestAnimationFrame(() => requestAnimationFrame(() => resolve()))));
    const result = await page.evaluate(() => ({ executed: (window as unknown as UploaderWindow).r05Executed,
      unexpected: (window as unknown as UploaderWindow).r05Unexpected, constructions: (window as unknown as UploaderWindow).r05Constructions }));
    expect(result).toEqual({ executed: false, unexpected: [], constructions: 0 });
    expect(requests).toEqual([]);
    expect(errors).toEqual([]);
  };
}

async function assertText(page: Page, index: number, expectedName: string, expectedType: string) {
  const row = page.locator('#metadata-fixture .e-upload-file-list').nth(index);
  await expect(row.locator('.e-file-name')).toHaveText(expectedName);
  await expect(row.locator('.e-file-type')).toHaveText('.' + expectedType);
  await expect(row.locator('.e-file-name *, .e-file-type *, .e-file-status *')).toHaveCount(0);
}

for (const rendering of ['ajax', 'razor']) {
  for (const sanitizer of ['on', 'off']) {
    test(`${rendering}/${sanitizer}: preload metadata is literal text`, async ({ page }) => {
      const safe = await open(page, rendering, sanitizer, 'preload');
      await expect(page.locator('#metadata-fixture .e-upload-file-list')).toHaveCount(2);
      await assertText(page, 0, name, 'txt');
      await assertText(page, 1, 'report &amp; العربية é', extension);
      await safe();
    });

    test(`${rendering}/${sanitizer}: selected filenames retain text and callback identity`, async ({ page }) => {
      const safe = await open(page, rendering, sanitizer, 'selection');
      const names = [name + '.txt', 'report.' + extension, 'plain &amp; العربية é.txt'];
      await page.locator('#MetadataUploader').setInputFiles(names.map(name => ({ name, mimeType: 'text/plain', buffer: Buffer.from('tiny') })));
      await expect(page.locator('#metadata-fixture .e-upload-file-list')).toHaveCount(3);
      await assertText(page, 0, name, 'txt');
      await assertText(page, 1, 'report', extension);
      await assertText(page, 2, 'plain &amp; العربية é', 'txt');
      await expect(page.locator('#metadata-selected')).toHaveText(names[0]);
      const selected = JSON.parse(await page.locator('#metadata-selection').innerText());
      expect(selected.map((file: { Name: string }) => file.Name)).toEqual(names);
      expect(selected.map((file: { StatusCode: string }) => file.StatusCode)).toEqual(sanitizer === 'on' ? ['0', '0', '0'] : ['1', '1', '1']);
      // The current resource set returns its missing invalid-name key; Razor replaces
      // non-ready statuses with its existing invalid-type message. Neither is changed here.
      expect(selected[0].Status).toBe(sanitizer === 'on' ? 'InvalidFileName' : 'Ready to upload');
      const renderedStatus = sanitizer === 'off' ? 'Ready to upload' : rendering === 'ajax' ? 'InvalidFileName' : 'File type is not allowed';
      await expect(page.locator('#metadata-fixture .e-file-status').first()).toHaveText(renderedStatus);
      const statusClass = sanitizer === 'on' ? /e-validation-fails/ : /e-file-status/;
      await expect(page.locator('#metadata-fixture .e-file-status').first()).toHaveClass(statusClass);
      await safe();
    });

    test(`${rendering}/${sanitizer}: benign names and existing size validation remain usable`, async ({ page }) => {
      const safe = await open(page, rendering, sanitizer, 'selection');
      const names = ['report.txt', 'README', 'report & العربية é.txt', 'oversize.txt'];
      await page.locator('#MetadataUploader').setInputFiles(names.map((name, index) => ({ name, mimeType: 'text/plain', buffer: Buffer.alloc(index === 3 ? 1025 : 4) })));
      await expect(page.locator('#metadata-fixture .e-upload-file-list')).toHaveCount(4);
      await assertText(page, 0, 'report', 'txt');
      await assertText(page, 1, 'README', '');
      await assertText(page, 2, 'report & العربية é', 'txt');
      const selected = JSON.parse(await page.locator('#metadata-selection').innerText());
      expect(selected.map((file: { Name: string }) => file.Name)).toEqual(names);
      expect(selected.map((file: { StatusCode: string }) => file.StatusCode)).toEqual(['1', '1', '1', '0']);
      await expect(page.locator('#metadata-fixture .e-file-status').first()).toHaveText('Ready to upload');
      await expect(page.locator('#metadata-fixture .e-file-status').last()).toHaveText('File size is too large');
      await safe();
    });

    test(`${rendering}/${sanitizer}: selected metadata cannot activate custom elements or nested resources`, async ({ page }) => {
      const safe = await open(page, rendering, sanitizer, 'selection');
      const names = ['<r05-marker></r05-marker>.txt', '<b><img src=r05-probe-nested srcset="r05-probe-srcset 1x"></b>.txt', '<script>sentinel</script>.txt'];
      await page.locator('#MetadataUploader').setInputFiles(names.map(name => ({ name, mimeType: 'text/plain', buffer: Buffer.from('tiny') })));
      await expect(page.locator('#metadata-fixture .e-upload-file-list')).toHaveCount(3);
      for (let index = 0; index < names.length; index++) await assertText(page, index, names[index].slice(0, -4), 'txt');
      const selected = JSON.parse(await page.locator('#metadata-selection').innerText());
      expect(selected.map((file: { Name: string }) => file.Name)).toEqual(names);
      expect(selected.map((file: { StatusCode: string }) => file.StatusCode)).toEqual(sanitizer === 'on' ? ['1', '0', '0'] : ['1', '1', '1']);
      await safe();
    });

    for (const source of ['api', 'modified']) {
      test(`${rendering}/${sanitizer}: ${source} metadata and status stay text`, async ({ page }) => {
        const safe = await open(page, rendering, sanitizer, source);
        if (source === 'api') await page.locator('#metadata-api').click();
        else await page.locator('#MetadataUploader').setInputFiles({ name: 'plain.txt', mimeType: 'text/plain', buffer: Buffer.from('tiny') });
        await expect(page.locator('#metadata-fixture .e-upload-file-list')).toHaveCount(1);
        await assertText(page, 0, name, extension);
        await expect(page.locator('#metadata-fixture .e-file-status')).toHaveText(status);
        await safe();
      });
    }
  }
}

for (const sanitizer of ['on', 'off']) {
  test(`ajax/${sanitizer}: cancellation, status updates, progress and truncation stay text`, async ({ page }) => {
    const safe = await open(page, 'ajax', sanitizer, 'selection');
    const longName = 'long '.repeat(35) + '&amp; العربية tail';
    await page.locator('#MetadataUploader').setInputFiles({ name: longName + '.txt', mimeType: 'text/plain', buffer: Buffer.from('tiny') });
    await expect(page.locator('#metadata-fixture .e-upload-file-list')).toHaveCount(1);
    const result = await page.evaluate(({ status }) => {
      const instance = (window as unknown as UploaderWindow).sfBlazorToolkit.base.getCompInstance('MetadataUploader');
      const row = instance.fileList[0] as HTMLElement;
      const file = instance.filesData[0];
      const name = row.querySelector('.e-file-name') as HTMLElement;
      // Deterministic overflow independent of the host's theme width.
      Object.assign(name.style, { display: 'inline-block', maxWidth: '80px', whiteSpace: 'nowrap', overflow: 'hidden' });
      instance.truncateName(name);
      instance.createProgressBar(row);
      instance.changeProgressValue(row, '37%');
      instance.updateStatus(file, status, '1');
      const updatedStatus = row.querySelector('.e-file-status')!.textContent;
      const localize = instance.localizedTexts.bind(instance);
      instance.localizedTexts = (key: string) => key === 'fileUploadCancel' ? status : localize(key);
      try { instance.eventCancelByArgs({}, { fileData: file }, file); }
      finally { instance.localizedTexts = localize; }
      return { updatedStatus, cancelStatus: row.querySelector('.e-file-status')!.textContent,
        name: name.textContent, tail: name.dataset.tail,
        progress: row.querySelector('.e-progress-bar-text')!.textContent,
        width: (row.querySelector('.e-upload-progress-bar') as HTMLElement).style.width,
        code: file.statusCode, metadataChildren: row.querySelectorAll('.e-file-status *').length };
    }, { status });
    expect(result).toEqual({ updatedStatus: status, cancelStatus: status, name: longName,
      tail: longName.slice(-10), progress: '37%', width: '37%', code: '5', metadataChildren: 0 });
    await safe();
  });
}

test('legacy form-list helper constructs filename and extension as text', async ({ page }) => {
  const safe = await open(page, 'ajax', 'off', 'api');
  await page.evaluate(({ name, extension }) => {
    const instance = (window as unknown as UploaderWindow).sfBlazorToolkit.base.getCompInstance('MetadataUploader');
    instance.createParentList();
    instance.createFileListElement([{ name: name + '.' + extension, size: 12, type: extension,
      status: 'Ready to upload', statusCode: '1', validationMessages: { minSize: '', maxSize: '' } }], []);
  }, { name, extension });
  await expect(page.locator('#metadata-fixture .e-upload-file-list')).toHaveCount(1);
  await assertText(page, 0, name, extension);
  await safe();
});

test('optional filename validator is inert and removes banned tags without throwing', async ({ page }) => {
  const safe = await open(page, 'razor', 'on', 'selection');
  const result = await page.evaluate(async () => {
    const moduleUrl = '/_content/Syncfusion.Blazor.Toolkit/scripts/sanitize-html-helper.js';
    await import(moduleUrl);
    const helper = (window as unknown as UploaderWindow).sfBlazorToolkit.SanitizeHtmlHelper;
    return ['plain & text', '<b>text</b>', '<script>sentinel</script>file', '<img src=r05-probe-validator onerror="self[\'r05Executed\']=true">'].map(value => {
      try { return { value, output: helper.serializeValue(helper.beforeSanitize(), value), error: null }; }
      catch (error) { return { value, output: null, error: String(error) }; }
    });
  });
  expect.soft(result.map(item => item.error)).toEqual([null, null, null, null]);
  expect.soft(result.map(item => item.output)).toEqual(['plain & text', '<b>text</b>', 'file', '<img src="r05-probe-validator">']);
  await safe();
});

for (const rendering of ['template-ajax', 'template-razor']) {
  for (const sanitizer of ['on', 'off']) {
    for (const source of ['preload', 'selection']) {
      test(`${rendering}/${sanitizer}/${source}: template lifecycle keeps data as text`, async ({ page }) => {
        const safe = await open(page, rendering, sanitizer, source);
        const expectedNames = [name + '.txt', 'report &amp; العربية é.' + extension];
        if (source === 'selection') {
          await page.locator('#MetadataUploader').setInputFiles(expectedNames.map(name => ({ name, mimeType: 'text/plain', buffer: Buffer.from('tiny') })));
          await expect(page.locator('#metadata-selected')).toHaveText(expectedNames[0]);
        }
        await expect(page.locator('#metadata-fixture .e-upload-file-list')).toHaveCount(2);
        await expect(page.locator('#metadata-fixture strong.trusted-template')).toHaveCount(2);
        await expect(page.locator('.template-name')).toHaveText(expectedNames);
        await expect(page.locator('.template-type')).toHaveText(['txt', extension]);
        await safe();
      });
    }
    test(`${rendering}/${sanitizer}: trusted template structure remains separate from metadata`, async ({ page }) => {
      const safe = await open(page, rendering, sanitizer, 'api');
      await page.locator('#metadata-api').click();
      await expect(page.locator('#metadata-fixture .e-upload-file-list')).toHaveCount(1);
      await expect(page.locator('#metadata-fixture strong.trusted-template')).toHaveText('File');
      await expect(page.locator('.template-name')).toHaveText(name + '.' + extension);
      await expect(page.locator('.template-type')).toHaveText(extension);
      await expect(page.locator('.template-status')).toHaveText(status);
      await safe();
    });
  }
}
