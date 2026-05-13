import { defineConfig, devices } from '@playwright/test';

export default defineConfig({
    testDir: './tests', 
    testMatch: '**/*.spec.ts',
    /* Run tests in files in parallel */
    fullyParallel: true,   
    retries: 1,
    workers: 4,
    reporter: [['html', { open: 'never' }]],
    /* Shared settings */
    use: {
        headless :true,
        /* Collect trace when retrying the failed test. See https://playwright.dev/docs/trace-viewer */
        trace: 'off'
    },
    /* Configure projects for major browsers */
    projects: [ 
        { 
            name: 'chromium', 
            use: { ...devices['Desktop Chrome'] }
        }
    ],
    webServer: { 
        command: 'dotnet run --project ./tests/Syncfusion.Blazor.Playwright.Test/Blazor.Toolkit.Playwright.Samples/Blazor.Toolkit.Playwright.Samples.csproj -c Release --urls http://localhost:5000', 
        url: 'http://localhost:5000', 
        reuseExistingServer: true, 
        timeout: 300 * 1000
    }
});