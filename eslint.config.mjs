// ESLint flat config used by `npm run lint:security`.
//

import js from '@eslint/js';
import tseslint from 'typescript-eslint';
import security from 'eslint-plugin-security';
import globals from 'globals';

export default [
    // Always-ignore generated/output directories.
    {
        ignores: [
            '**/node_modules/**',
            '**/bin/**',
            '**/obj/**',
            '**/playwright-report/**',
            '**/test-results/**',
            'src/wwwroot/styles/**',
            'src/wwwroot/scripts/**',
            'coverage/**',
            'TestResults/**'
        ]
    },

    // JavaScript base recommended only.
    js.configs.recommended,

    // TypeScript surface - only *.ts / *.mts / *.cts.
    {
        files: ['**/*.ts', '**/*.mts', '**/*.cts'],
        languageOptions: {
            ecmaVersion: 2022,
            sourceType: 'module',
            globals: {
                ...globals.node,
                ...globals.browser
            }
        },
        ...tseslint.configs.recommended
    },

    // JavaScript surface - only *.js / *.mjs / *.cjs.
    {
        files: ['**/*.js', '**/*.mjs', '**/*.cjs'],
        languageOptions: {
            ecmaVersion: 2022,
            sourceType: 'script',
            globals: {
                ...globals.node
            }
        }
    },

    // Security rules apply to every included JS / TS file.
    {
        files: ['**/*.js', '**/*.mjs', '**/*.cjs', '**/*.ts', '**/*.mts', '**/*.cts'],
        plugins: { security },
        rules: {
            ...security.configs.recommended.rules,
            'no-eval': 'error',
            'no-implied-eval': 'error',
            'no-new-func': 'error'
        }
    }
];