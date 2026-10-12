import { defineConfig } from '@playwright/test';
import base from './playwright.config';
const chunk = process.env.R06_HOLD_CHUNK;
if (chunk !== '1' && chunk !== '2') throw new Error('Choose held chunk 1 or 2; each run requires a fresh host');
const server = base.webServer;
if (!server || Array.isArray(server)) throw new Error('Expected one characterization server');
export default defineConfig({
  ...base,
  testMatch: '**/pause-variants.characterization.ts',
  projects: [{ name: 'pause-variant' }],
  webServer: { ...server, env: { R06_CHARACTERIZATION: '1', R06_HOLD_CHUNK: chunk } }
});
