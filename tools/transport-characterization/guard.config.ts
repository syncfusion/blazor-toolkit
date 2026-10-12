import { defineConfig } from '@playwright/test';
import base from './playwright.config';

// Run separately: these tests consume the one-host upload budget, or start a disabled host.
const disabled = process.env.R06_VERIFY_DISABLED === '1';
const server = base.webServer;
if (!server || Array.isArray(server)) throw new Error('Expected one characterization server');
export default defineConfig({
  ...base,
  testMatch: '**/endpoint-guards.characterization.ts',
  projects: [{ name: 'endpoint-guards', grep: disabled ? /@disabled/ : /@guard/ }],
  webServer: { ...server, env: { R06_CHARACTERIZATION: disabled ? '0' : '1' } }
});
