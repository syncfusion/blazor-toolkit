var fs = global.fs = global.fs || require('fs');
var shelljs = global.shelljs = global.shelljs || require('shelljs');
var gulp = global.gulp = global.gulp || require('gulp');
const glob = require('glob');
const sass = require('gulp-sass')(require('sass'));
const rename = require('gulp-rename');

var componentThemeOrder = [
    "base",
    "animation",
    "icons",
    "input",
    "popup",
    "spinner",
    "button",
    "calendar",
    "chart",
    "checkbox",
    "numerictextbox",
    "tooltip",
    "datepicker",
    "radio-button",
    "switch",
    "datetimepicker",
    "textbox",
    "textarea",
    "uploader",
    "dialog",
    "buttongroup",
    "timepicker"
];

// To move the @use rule references to the top of the SCSS file
function reorderUseRules(definitionFile) {
    // Extract all @use rules
    const useRegex = /@use\s+['"].+['"];/g;
    var useStatements = definitionFile.match(useRegex) || [];
    // Remove duplicate @use rules
    useStatements = [...new Set(useStatements)];
    // Remove @use rules from original content
    let modifiedContent = definitionFile.replace(useRegex, '').trim();
    // Prepare final content with all the @use rules at the top
    const finalContent = useStatements.join('\n') + '\n\n' + modifiedContent;
    // Return the updated content back to write on the file
    return finalContent;
}

// Match any custom @use(dependencies) content and remove that content
function removeCustomUse(fileContent) {
    var regex = new RegExp("@(use)\\s+['\"][^'\"]+['\"][^;]*;", "g");
    var importedStyles = fileContent.match(regex) || [];
    const builtInUse = /^@use\s+['"]sass:(math|color|list|meta)['"]\s*;$/;
    importedStyles = importedStyles.filter(s => !builtInUse.test(s));
    if (importedStyles) {
        for (var importedStyle of importedStyles) {
            fileContent = fileContent.replace(importedStyle, '');
        }
    }
    return fileContent;
}

// Task to generate single SCSS files for Blazor toolkit.
gulp.task('combined-scss', function (done) {
    // Get the all components scss files' path
    var componentFiles = glob.sync(`./src/wwwroot/styles/*.scss`);
    shelljs.mkdir('-p', './src/wwwroot/styles/combined-scss/');
    var getFluentScss = '';
    // Place component styles as per styles order
    for (var themeOrder of componentThemeOrder) {
        var paths = componentFiles.filter((value) => {
            return value.indexOf('styles/' + themeOrder) !== -1;
        });
        if (paths.length) {
            getFluentScss += stripBom(fs.readFileSync(paths[0], 'utf8'));
        }
    }
    getFluentScss = removeCustomUse(getFluentScss);
    fs.writeFileSync('./src/wwwroot/styles/combined-scss/fluent.scss', reorderUseRules(getFluentScss), 'utf8');
    var hcBody = '';
    for (var hcOrder of componentThemeOrder) {
        var hcPaths = componentFiles.filter((value) => { return value.indexOf('styles/' + hcOrder) !== -1; });
        if (!hcPaths.length) continue;
        var content = stripBom(fs.readFileSync(hcPaths[0], 'utf8'));
        if (hcOrder === 'base') content = stripRootScopes(content);
        hcBody += '\n' + content;
    }
    hcBody = removeCustomUse(hcBody);
    hcBody = reorderUseRules(hcBody);

    var tokensSrc = stripBom(fs.readFileSync('./src/wwwroot/styles/highcontrast-tokens.scss', 'utf8'));
    var unlayeredMarker = '// Unscoped component overrides (component-state corrections that var() tokens';
    var markerIdx = tokensSrc.indexOf(unlayeredMarker);
    var unlayeredPostlude = '';
    if (markerIdx >= 0) {
        var forcedIdx = tokensSrc.indexOf('@media (forced-colors: active)', markerIdx);
        var endIdx = forcedIdx > markerIdx ? forcedIdx : tokensSrc.length;
        unlayeredPostlude = tokensSrc.substring(markerIdx, endIdx).trim() + '\n';
    }

    fs.writeFileSync(
        './src/wwwroot/styles/combined-scss/highcontrast.scss',
        "@use 'highcontrast-tokens';\n" + hcBody + '\n' + unlayeredPostlude,
        'utf8'
    );

    fs.copyFileSync(
        './src/wwwroot/styles/highcontrast-tokens.scss',
        './src/wwwroot/styles/combined-scss/_highcontrast-tokens.scss'
    );
    done();
});

function stripBom(content) {
    return content.replace(/^\uFEFF/, '');
}

function stripRootScopes(content) {
    var out = '', i = 0;
    while (i < content.length) {
        // Skip a preceding @layer <name> { ... } block if it's a theme
        // layer or contains a :root{} rule.
        var layerSearch = content.slice(i).search(/(^|[;}]\s*)@layer\s+[a-zA-Z][\w.]*\s*\{/);
        if (layerSearch >= 0) {
            var headerMatch = content.slice(i + layerSearch).match(/@layer\s+([a-zA-Z][\w.]*)\s*\{/);
            if (headerMatch) {
                var openBrace = i + layerSearch + headerMatch.index + headerMatch[0].length - 1;
                var end = openBrace, depth = 1;
                while (++end < content.length && depth > 0) {
                    if (content[end] === '{') depth++;
                    else if (content[end] === '}') depth--;
                }
                var body = content.slice(i + layerSearch, end);
                if (/fluent|themes/.test(headerMatch[1]) || /:root\s*\{/.test(body)) {
                    out += content.slice(i, i + layerSearch);
                    i = end;
                    continue;
                }
                out += content.slice(i, end);
                i = end;
                continue;
            }
        }
        // Process the next rule
        var brace = content.indexOf('{', i);
        if (brace === -1) { out += content.slice(i); break; }
        var selStart = Math.max(content.lastIndexOf(';', brace), content.lastIndexOf('}', brace), 0) + 1;
        while (selStart < brace && /\s/.test(content[selStart])) selStart++;
        var selector = content.slice(selStart, brace).trim();
        var depth = 1, j = brace + 1;
        while (j < content.length && depth > 0) {
            if (content[j] === '{') depth++;
            else if (content[j] === '}') depth--;
            if (depth === 0) break;
            j++;
        }
        if (/^:root\b/.test(selector) || /\.e-dark-mode\b/.test(selector)) {
            out += content.slice(i, selStart);
        } else {
            out += content.slice(i, j + 1);
        }
        i = j + 1;
    }
    return out;
}

// Compile SCSS to CSS.
gulp.task('scss-to-css', function (done) {
    function cleanup() {
        try { fs.unlinkSync('./src/wwwroot/styles/combined-scss/_highcontrast-tokens.scss'); } catch (e) { }
        console.log("SCSS to CSS compiled successfully");
        done();
    }
    return gulp.src(
        ['./src/wwwroot/styles/combined-scss/*.scss', './src/wwwroot/styles/*.scss'],
        { ignore: [
            './src/wwwroot/styles/icons.scss',
            './src/wwwroot/styles/animation.scss',
            './src/wwwroot/styles/base.scss',
            './src/wwwroot/styles/highcontrast-tokens.scss',
            './src/wwwroot/styles/combined-scss/_highcontrast-tokens.scss'
        ] }
    )
    .pipe(sass({ outputStyle: 'compressed' }).on('error', function (error) {
        fs.appendFileSync('./gulp_error.log', 'Failed scss-to-css task\n' + error.message + '\n');
        console.error('Sass Compilation Error:', error.messageFormatted);
        process.exit(1);
    }))
    .pipe(rename({ suffix: '.min' }))
    .pipe(gulp.dest('./src/wwwroot/styles'))
    .on('end', cleanup)
    .on('error', cleanup);
});

gulp.task('blazor-toolkit-themes', gulp.series('combined-scss', 'scss-to-css'));

/*
 * security-xss-scan
 *
 * Scans C#, Razor, JS, and TS source files for patterns that frequently
 * indicate unsanitised HTML / JavaScript evaluation. The job that runs this in
 * CI (see .github/workflows/ci.yml) fails fast on findings unless the file:line
 * is added to XSS_ALLOWLIST below with a short reviewer-issued justification.
 *
 * IMPORTANT: this is a *defensive heuristic*, not a complete XSS detector. It
 * complements CodeQL (semantic) for the C# / Razor surface and ESLint
 * (`eslint-plugin-security`) for the JS / TS surface.
 */
const XSS_PATTERNS = [
    { name: 'eval()', regex: /\beval\s*\(/g },
    { name: 'new Function(', regex: /new\s+Function\s*\(/g },
    { name: 'MarkupString', regex: /\bMarkupString\b/g },
    { name: 'HtmlString', regex: /\bHtmlString\b/g },
    { name: 'dangerouslySetInnerHTML-like', regex: /innerHTML\s*=/g },
    { name: 'document.write', regex: /\bdocument\.write\b/g },
    { name: 'setTimeout-string-arg', regex: /setTimeout\s*\(\s*['"`]/g },
    { name: 'setInterval-string-arg', regex: /setInterval\s*\(\s*['"`]/g }
];

const XSS_ALLOWLIST = [
    // e.g. 'src/Components/SafeMarkup/Render.cs:42'
];

const XSS_SCAN_GLOBS = [
    'src/Components/**/*.{cs,razor,js,ts,mjs,cjs}',
    'src/Base/**/*.{cs,razor,js,ts,mjs,cjs}',
    'src/Data/**/*.{cs,razor,js,ts,mjs,cjs}'
];

function isXSSAllowlisted(file, line) {
    return XSS_ALLOWLIST.some(entry => {
        const [af, al] = entry.split(':');
        if (af !== file) {
            return false;
        }
        if (!al) {
            return true;
        }
        if (al.includes('-')) {
            const [from, to] = al.split('-').map(n => parseInt(n, 10));
            return line >= from && line <= to;
        }
        return parseInt(al, 10) === line;
    });
}

gulp.task('security-xss-scan', function (done) {
    let allFiles = [];
    for (const pattern of XSS_SCAN_GLOBS) {
        allFiles = allFiles.concat(glob.sync(pattern, {
            nodir: true,
            ignore: [
                '**/bin/**',
                '**/obj/**',
                '**/node_modules/**',
                // Never scan the bundled client scripts / sample apps here.
                '**/wwwroot/**',
                '**/samples/**',
                '**/tests/**',
                // Compiled themes.
                '**/wwwroot/styles/**'
            ]
        }));
    }
    // Deduplicate (some files may match multiple globs)
    allFiles = Array.from(new Set(allFiles));

    const findings = [];
    for (const file of allFiles) {
        let content;
        try {
            content = fs.readFileSync(file, 'utf8');
        } catch {
            continue;
        }
        const lines = content.split(/\r?\n/);
        lines.forEach((line, i) => {
            const lineNumber = i + 1;
            if (isXSSAllowlisted(file, lineNumber)) {
                return;
            }
            for (const pattern of XSS_PATTERNS) {
                if (pattern.regex.test(line)) {
                    pattern.regex.lastIndex = 0;
                    findings.push({
                        file: file,
                        line: lineNumber,
                        pattern: pattern.name,
                        text: line.trim()
                    });
                }
                pattern.regex.lastIndex = 0;
            }
        });
    }

    if (findings.length) {
        const grouped = Object.create(null);
        for (const f of findings) {
            if (!Object.prototype.hasOwnProperty.call(grouped, f.pattern)) {
                grouped[f.pattern] = [];
            }
            grouped[f.pattern].push(f);
        }
        console.error('=========================================================');
        console.error(`XSS / unsafe markup scan: ${findings.length} finding(s)`);
        console.error('=========================================================');
        for (const pattern of Object.keys(grouped)) {
            console.error(`\n[${pattern}] (${grouped[pattern].length})`);
            for (const f of grouped[pattern]) {
                console.error(`  ${f.file}:${f.line}  ${f.text}`);
            }
        }
        try {
            fs.writeFileSync(
                'xss-scan-report.txt',
                findings.map(f => `${f.file}:${f.line} [${f.pattern}] ${f.text}`).join('\n'),
                'utf8'
            );
        } catch (err) {
            console.error('Could not write xss-scan-report.txt: ' + err.message);
        }
        process.exitCode = 1;
        return done(new Error(`${findings.length} XSS-related finding(s) - see xss-scan-report.txt`));
    }

    console.log('XSS / unsafe markup scan: no risky patterns found');
    done();
});

gulp.task('security', gulp.series('security-xss-scan'));