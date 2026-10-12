import { test, expect, Page } from '@playwright/test';

const assets = '/_content/Syncfusion.Blazor.Toolkit/scripts/';
const font = { size: '16px', headerTextSize: '16px', fontStyle: 'normal', fontWeight: 'normal', fontFamily: 'Arial' };

async function ready(page: Page) {
  await page.goto('http://localhost:5000/chart-tooltip-playwright');
  await expect(page.locator('.e-chart').first()).toBeVisible();
  await page.evaluate(async (root) => {
    await import(root + 'base.js');
    await import(root + 'svgbase.js');
  }, assets);
}

for (const path of ['svg', 'chart', 'font-keys'] as const) {
  test(`${path}: markup-looking text cannot create elements, handlers or requests`, async ({ page }) => {
    const requests: string[] = [];
    await page.route('**/r04-probe-*', async route => {
      requests.push(route.request().url());
      await route.abort();
    });
    await ready(page);
    const result = await page.evaluate(async ({ path, assets, font }) => {
      const chart = await import(assets + 'chart.js');
      const browser = window as typeof window & { svgbase: { measureText: Function }; r04Executed?: boolean };
      browser.r04Executed = false;
      // Local sentinel only: no external resource or user data.
      const text = 'مرحبا<br/><img src="/r04-probe-image" onerror="window.r04Executed=true">';
      const ids = ['chartmeasuretext', 'sfchartmeasuretext'];
      ids.forEach(id => document.getElementById(id)?.remove());
      let size;
      if (path === 'svg') size = browser.svgbase.measureText(text, font, font, false);
      else if (path === 'chart') size = chart.measureText(text, 'normal', 'normal', 'Arial');
      else {
        const key = text + '_normal_normal_Arial';
        const values = JSON.parse(chart.getCharSizeByFontKeys([key]));
        size = values[key];
      }
      const node = document.getElementById(path === 'svg' ? ids[0] : ids[1])!;
      await new Promise<void>(resolve => requestAnimationFrame(() => requestAnimationFrame(() => resolve())));
      return {
        elements: Array.from(node.querySelectorAll('*')).map(element => ({ tag: element.tagName, attributes: element.attributes.length })),
        text: node.textContent,
        executed: browser.r04Executed,
        dimensions: Object.values(size) as number[]
      };
    }, { path, assets, font });
    expect(result.elements).toEqual([{ tag: 'BR', attributes: 0 }]);
    expect(result.text).toContain('<img src=');
    expect(result.executed).toBe(false);
    expect(requests).toEqual([]);
    for (const dimension of result.dimensions) expect(Number.isFinite(dimension) && dimension > 0).toBe(true);
  });
}

const cases = [
  'plain Latin text', 'A < B > C & D', '&amp; &nbsp; &#60; &lt;br&gt;',
  'العَرَبِيَّة שלום 中文 e\u0301 😀', 'first<br>second', 'first<br/>second',
  '<br>first<br><br/>last<br/>', '', ' ', 'two  spaces', 'raw\nnewline',
  '<b>Bold</b><br><strong>Strong</strong>', '<br onclick="x">not formatting', '<BR>literal'
];
for (const path of ['svg', 'chart'] as const) {
  test(`${path}: text, Unicode, formatting and geometry match safe DOM construction`, async ({ page }) => {
    await ready(page);
    const results = await page.evaluate(async ({ path, assets, font, cases }) => {
      const chart = await import(assets + 'chart.js');
      const browser = window as typeof window & { svgbase: { measureText: Function } };
      const results = [];
      for (const text of cases) {
        for (const weight of ['normal', 'bold']) {
          for (const direction of ['ltr', 'rtl']) {
            document.body.dir = direction;
            const id = path === 'svg' ? 'chartmeasuretext' : 'sfchartmeasuretext';
            document.getElementById(id)?.remove();
            const style = { ...font, fontWeight: weight, fontStyle: 'italic' };
            const measured = path === 'svg'
              ? browser.svgbase.measureText(text, style, style, false)
              : chart.measureText(text, weight, 'italic', 'Arial');
            const actual = document.getElementById(id)!;
            const reference = document.createElement('text');
            reference.style.cssText = actual.style.cssText;
            // Independent construction of exact supported tokens; all other input stays text.
            let parent: Node = reference;
            const stack: Node[] = [reference];
            const tokens = (path === 'chart' && text === ' ' ? '\u00a0' : text).split(/(<br>|<br\/>|<b>|<\/b>|<strong>|<\/strong>)/g);
            for (const token of tokens) {
              if (token === '<br>' || token === '<br/>') parent.appendChild(document.createElement('br'));
              else if (token === '<b>' || token === '<strong>') {
                const bold = document.createElement(token === '<b>' ? 'b' : 'strong');
                parent.appendChild(bold); stack.push(bold); parent = bold;
              } else if (token === '</b>' || token === '</strong>') {
                if (stack.length > 1) stack.pop(); parent = stack[stack.length - 1];
              } else parent.appendChild(document.createTextNode(token));
            }
            document.body.appendChild(reference);
            results.push({ path, text, weight, direction,
              actual: [measured.width ?? measured.Width, measured.height ?? measured.Height],
              expected: [reference.clientWidth, reference.clientHeight],
              unsafe: Array.from(actual.querySelectorAll('*')).some(element => !['BR', 'B', 'STRONG'].includes(element.tagName) || element.attributes.length > 0)
            });
            reference.remove();
          }
        }
      }
      document.body.removeAttribute('dir');
      return results;
    }, { path, assets, font, cases });
    for (const result of results) {
      expect(result.unsafe, JSON.stringify(result)).toBe(false);
      expect(result.actual, JSON.stringify(result)).toEqual(result.expected);
    }
  });
}

test('character APIs retain JSON casing, numeric measurements and single-space width', async ({ page }) => {
  await ready(page);
  const result = await page.evaluate(async assets => {
    const chart = await import(assets + 'chart.js');
    const key = 'العربية_normal_normal_Arial';
    const keyed = JSON.parse(chart.getCharSizeByFontKeys([key]));
    const widths = JSON.parse(await chart.getCharCollectionSize(['normal_normal_Arial']));
    return { keys: Object.keys(keyed), size: keyed[key], widths, space: chart.measureText(' ', 'normal', 'normal', 'Arial') };
  }, assets);
  expect(result.keys).toEqual(['العربية_normal_normal_Arial']);
  expect(Object.keys(result.size).sort()).toEqual(['X', 'Y']);
  expect(result.size.X).toBeGreaterThan(0);
  expect(result.size.Y).toBeGreaterThan(0);
  expect(result.widths).toHaveLength(558);
  expect(result.widths.every((value: unknown) => typeof value === 'string' && Number.isFinite(Number(value)))).toBe(true);
  expect(result.space.Width).toBeGreaterThan(0);
});

test('formatter has deterministic malformed-token semantics and clears reused nodes', async ({ page }) => {
  await ready(page);
  const results = await page.evaluate(async assets => {
    const { setMeasurementText } = await import(assets + 'measurement-text.js');
    const node = document.createElement('text');
    const cases = [
      ['<b>a<strong>b</strong><br>c</b>', '<b>a<strong>b</strong><br>c</b>'],
      ['</b>x', '&lt;/b&gt;x'],
      ['<b><strong>x</b>y</strong>', '<b><strong>x&lt;/b&gt;y</strong></b>'],
      ['<b>unclosed', '<b>unclosed</b>'],
      ['<b class="x">literal</b>', '&lt;b class="x"&gt;literal&lt;/b&gt;'],
      ['<BR><script>literal</script>', '&lt;BR&gt;&lt;script&gt;literal&lt;/script&gt;'],
      ['plain &amp;', 'plain &amp;amp;'], ['', ''], [null, ''], [undefined, ''], [42, '42']
    ];
    return cases.map(([value, expected]) => {
      setMeasurementText(node, value);
      return { value, actual: node.innerHTML, expected };
    });
  }, assets);
  for (const result of results) expect(result.actual, String(result.value)).toBe(result.expected);
});

test('SVG measurement preserves reused-node max-bold sizing and theme/header fonts', async ({ page }) => {
  await ready(page);
  const result = await page.evaluate(({ font }) => {
    const svg = (window as typeof window & { svgbase: { measureText: Function } }).svgbase;
    document.getElementById('chartmeasuretext')?.remove();
    const text = 'Wide مرحبا e\u0301';
    const theme = { size: '19px', headerTextSize: '27px', fontStyle: 'italic', fontWeight: 'normal', fontFamily: 'Arial' };
    const reference = (weight: string, size: string) => {
      const node = document.createElement('text');
      Object.assign(node.style, { position: 'fixed', visibility: 'hidden', whiteSpace: 'nowrap', lineHeight: 'normal',
        fontSize: size, fontFamily: 'Arial', fontStyle: 'italic', fontWeight: weight });
      node.textContent = text;
      document.body.appendChild(node);
      const dimensions = [node.clientWidth, node.clientHeight]; node.remove(); return dimensions;
    };
    const dimensions = (size: { width: number; height: number }) => [size.width, size.height];
    const normal = dimensions(svg.measureText(text, {}, theme, false));
    const node = document.getElementById('chartmeasuretext')!;
    const bold = dimensions(svg.measureText(text, { fontWeight: 'bold' }, theme, false));
    const normalAgain = dimensions(svg.measureText(text, {}, theme, false));
    const header = dimensions(svg.measureText(text, {}, theme, true));
    const normalReference = reference('normal', '19px');
    const boldReference = reference('bold', '19px');
    svg.measureText('<b>formatted</b>', font, font, false);
    svg.measureText('plain', font, font, false);
    const clearedFormatting = node.childElementCount === 0 && node.textContent === 'plain';
    svg.measureText(null, font, font, false);
    return { normal, bold, normalAgain, header, normalReference, headerReference: reference('normal', '27px'),
      boldReference: normalReference.map((value, index) => Math.max(value, boldReference[index])),
      sameNode: node === document.getElementById('chartmeasuretext'), clearedFormatting, empty: node.textContent === '' && node.childElementCount === 0 };
  }, { font });
  expect(result.normal).toEqual(result.normalReference);
  expect(result.normalAgain).toEqual(result.normalReference);
  expect(result.bold).toEqual(result.boldReference);
  expect(result.header).toEqual(result.headerReference);
  expect(result.sameNode && result.clearedFormatting && result.empty).toBe(true);
});

type MeasurementWindow = Window & { r04State: { unsafe: string[]; measurements: string[]; svgMeasurements: string[] } };

test('category layout, refresh, tooltip and crosshair never create transient data markup', async ({ page }) => {
  const errors: string[] = [];
  const requests: string[] = [];
  page.on('pageerror', error => errors.push(error.message));
  await page.route('**/r04-probe-*', async route => { requests.push(route.request().url()); await route.abort(); });
  await page.addInitScript(() => {
    const state = { unsafe: [] as string[], measurements: [] as string[], svgMeasurements: [] as string[] };
    (window as unknown as MeasurementWindow).r04State = state;
    const selector = '#sfchartmeasuretext, #chartmeasuretext';
    const inspect = (element: Element) => {
      if (!['BR', 'B', 'STRONG'].includes(element.tagName) || element.attributes.length > 0) state.unsafe.push(element.outerHTML);
    };
    new MutationObserver(records => {
      for (const record of records) {
        const target = record.target instanceof Element ? record.target.closest(selector) : null;
        if (target) {
          const measurements = target.id === 'sfchartmeasuretext' ? state.measurements : state.svgMeasurements;
          measurements.push(Array.from(record.addedNodes).map(node => node.textContent || '').join(''));
        }
        for (const node of Array.from(record.addedNodes)) {
          if (!(node instanceof Element)) continue;
          if (target || node.matches(selector)) {
            if (target) inspect(node);
            node.querySelectorAll('*').forEach(inspect);
          }
          node.querySelectorAll(selector).forEach(root => root.querySelectorAll('*').forEach(inspect));
        }
      }
    }).observe(document, { childList: true, subtree: true });
  });
  await page.goto('http://localhost:5000/chart/text-measurement');
  await page.waitForLoadState('networkidle');
  const point = page.locator('#MeasurementChart_Series_0_Point_0');
  await expect(point).toBeVisible();
  await expect.poll(async () => {
    const bounds = await page.locator('#MeasurementChart_svg').boundingBox();
    return bounds && [bounds.width, bounds.height];
  }).toEqual([800, 400]);
  for (const label of ['مرحبا0', 'العربية0']) {
    await expect.poll(() => page.evaluate(label => (window as unknown as MeasurementWindow).r04State.measurements.some(text => text.includes(label)), label)).toBe(true);
  }
  const hoverReadyPoint = async () => {
    await expect(async () => {
      await page.mouse.move(0, 0);
      await point.hover();
      expect(errors, 'Chart integration must not report browser exceptions').toEqual([]);
      await expect(page.locator('#MeasurementChart_tooltip')).toBeVisible({ timeout: 500 });
    }).toPass({ timeout: 10000 });
  };
  await hoverReadyPoint();
  await expect(page.locator('#MeasurementChart_tooltip_text')).toContainText('series marker');
  await expect(page.locator('#MeasurementChart_tooltip_text')).toContainText('tooltip marker');
  // The internal axes collection order is not a public contract; only X has a tooltip.
  const crosshair = page.locator('[id^="MeasurementChart_axis_tooltip_text_"]');
  await expect(crosshair).toHaveCount(1);
  await expect(crosshair).toHaveText('مرحبا0<br/><i>category marker</i>');
  await page.locator('#measurement-refresh').click();
  await expect(page.locator('#measurement-generation')).toHaveText('1');
  for (const label of ['مرحبا1', 'العربية1']) {
    await expect.poll(() => page.evaluate(label => (window as unknown as MeasurementWindow).r04State.measurements.some(text => text.includes(label)), label)).toBe(true);
  }
  await hoverReadyPoint();
  await expect(page.locator('#MeasurementChart_tooltip_text')).toContainText('مرحبا1');
  await expect(crosshair).toHaveText('مرحبا1<br/><i>category marker</i>');
  const bounds = await crosshair.boundingBox();
  expect(bounds && Number.isFinite(bounds.width) && bounds.width > 0 && bounds.height > 0).toBe(true);
  const state = await page.evaluate(() => (window as unknown as MeasurementWindow).r04State);
  expect(state.unsafe).toEqual([]);
  expect(state.svgMeasurements.some(text => text.includes('مرحبا1') && text.includes('<i>category marker</i>'))).toBe(true);
  expect(requests).toEqual([]);
  expect(errors).toEqual([]);
});
