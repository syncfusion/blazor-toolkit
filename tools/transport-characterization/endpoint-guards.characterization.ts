import { test, expect } from '@playwright/test';

test('@guard local endpoint rejects bad origin, oversized data and exhausted malformed-attempt budget', async ({ request }) => {
  const save = 'http://localhost:5000/r06-live/save';
  expect((await request.post(save, { data: 'dummy' })).status()).toBe(403);
  expect((await request.post(save, { headers: { Origin: 'http://localhost:5000' }, data: 'A'.repeat(8193) })).status()).toBe(413);
  const malformed = 'filename="A.txt"\r\n\r\nAAAA\r\n--dummy\r\nname="chunk-index"\r\n\r\n';
  for (let i = 0; i < 16; i++) expect((await request.post(save, { headers: { Origin: 'http://localhost:5000' }, data: malformed })).status()).toBe(400);
  expect((await request.post(save, { headers: { Origin: 'http://localhost:5000' }, data: malformed })).status()).toBe(429);
  expect((await request.post('http://localhost:5000/r06-live/release')).status()).toBe(404);
  const state = await request.get('http://localhost:5000/r06-live/state');
  expect(await state.json()).toEqual({ chunks: [], active: 0, completed: 0, timedOut: false });
});

test('@disabled normal host does not register endpoint or render matrix uploader', async ({ request, page }) => {
  // A missing POST is re-executed by this host's Razor error page and becomes 400;
  // the GET state route gives an unambiguous missing-route control without bypassing antiforgery.
  expect((await request.get('http://localhost:5000/r06-live/state')).status()).toBe(404);
  expect((await request.post('http://localhost:5000/r06-live/save', { data: 'dummy' })).status()).toBe(400);
  await page.goto('http://localhost:5000/inputs/uploader/transport-matrix/config');
  await expect(page.getByText('Characterization disabled.', { exact: true })).toBeVisible();
  await expect(page.locator('#ProbeA')).toHaveCount(0);
});
