import { defineConfig, devices } from '@playwright/test';

// Explicit opt-in investigation: current observations must not become CI contracts.
export default defineConfig({
  testDir: '.',
  testMatch: '**/*.characterization.ts',
  fullyParallel: false,
  workers: 1,
  retries: 0,
  timeout: 30000,
  reporter: [['line']],
  use: { ...devices['Desktop Chrome'], headless: true, serviceWorkers: 'block', trace: 'off' },
  projects: [
    { name: 'observations', grep: /@observed/ },
    { name: 'proposed-policy', grep: /@policy/ }
  ],
  webServer: {
    command: 'dotnet run --project tests/Syncfusion.Blazor.Playwright.Test/Blazor.Toolkit.Playwright.Samples/Blazor.Toolkit.Playwright.Samples.csproj -c Release --urls http://localhost:5000',
    cwd: '../..',
    url: 'http://localhost:5000',
    reuseExistingServer: false,
    env: { R06_CHARACTERIZATION: '1' },
    timeout: 300000
  }
});
