// ESLint flat config used by `npm run lint:security`.
import js from '@eslint/js';
import tseslint from 'typescript-eslint';
import security from 'eslint-plugin-security';
import globals from 'globals';

export default tseslint.config(
  // 1) Ignore everything that is not first-party product source
  {
    ignores: [
      '**/node_modules/**',
      '**/bin/**',
      '**/obj/**',
      '**/coverage/**',
      '**/TestResults/**',
      '**/playwright-report/**',
      '**/test-results/**',

      // Generated / built assets
      'src/wwwroot/styles/**',
      'src/wwwroot/scripts/**',
      '**/*.min.js',

      // Vendor & third-party
      '**/lib/**',
      '**/bootstrap/**',
      '**/wwwroot/lib/**',

      // Samples (demo site, not product source)
      'samples/**',

      // Playwright & unit-test projects (not security surface of the library)
      'tests/**',

      // Build tooling that intentionally uses CommonJS
      'gulpfile.js',
      'gulpfile.*.js'
    ]
  },

  // 2) Base recommended rules
  js.configs.recommended,

  // 3) TypeScript recommended (only for real TS files that remain)
  ...tseslint.configs.recommended,

  // 4) Security rules for remaining first-party JS/TS
  {
    files: ['**/*.{js,mjs,cjs,ts,mts,cts}'],
    languageOptions: {
      ecmaVersion: 2022,
      sourceType: 'module',
      globals: {
        ...globals.node,
        ...globals.browser,
        Blazor: 'readonly',
        hljs: 'readonly',
        define: 'readonly'
      }
    },
    plugins: {
      security
    },
    rules: {
      // Security plugin
      ...security.configs.recommended.rules,

      // Keep real injection risks as errors
      'no-eval': 'error',
      'no-implied-eval': 'error',
      'no-new-func': 'error',

      // Soften noisy rules that produce mostly false positives in this repo
      'security/detect-object-injection': 'off',
      'security/detect-non-literal-regexp': 'warn',
      'security/detect-unsafe-regex': 'warn',

      // Turn off style rules that are not security-related for this job
      '@typescript-eslint/no-unused-vars': 'off',
      '@typescript-eslint/no-explicit-any': 'off',
      '@typescript-eslint/no-require-imports': 'off',
      '@typescript-eslint/no-this-alias': 'off',
      '@typescript-eslint/no-unused-expressions': 'off',
      'no-undef': 'off',
      'no-empty': 'off',
      'no-useless-escape': 'off',
      'prefer-const': 'off',
      'no-prototype-builtins': 'off',
      'no-self-assign': 'off'
    }
  }
);