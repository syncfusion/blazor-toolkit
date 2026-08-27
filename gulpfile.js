var fs = global.fs = global.fs || require('fs');
var shelljs = global.shelljs = global.shelljs || require('shelljs');
var gulp = global.gulp = global.gulp || require('gulp');
const glob = require('glob');
const sass = require('gulp-sass')(require('sass'));
const cleanCSS = require('gulp-clean-css');
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
]

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
function removeCustomUse(fileContent){
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
    var getFluentScss = '';
    // Place component styles as per styles order
    for (var themeOrder of componentThemeOrder) {
        var paths = componentFiles.filter((value) => { return value.indexOf('styles/' + themeOrder) !== -1; });
        if (paths.length) {
            getFluentScss += stripBom(fs.readFileSync(paths[0], 'utf8'));
        }
    }
    getFluentScss = removeCustomUse(getFluentScss);
    shelljs.mkdir('-p', './src/wwwroot/styles/combined-scss/');
    fs.writeFileSync('./src/wwwroot/styles/combined-scss/fluent.scss', reorderUseRules(getFluentScss), 'utf8');
    done();
});

function stripBom(content) {
    return content.replace(/^\uFEFF/, '');
}

// Compile SCSS to CSS.
gulp.task('scss-to-css', function (done) {
    return gulp.src(['./src/wwwroot/styles/combined-scss/*.scss', './src/wwwroot/styles/*.scss'], { ignore: ['./src/wwwroot/styles/icons.scss','./src/wwwroot/styles/animation.scss','./src/wwwroot/styles/base.scss'] }) // Select all SCSS files in the directory for compiling to css expect base and icons scss
    .pipe(sass().on('error', function (error) {
        // Handle SCSS compilation errors
        fs.appendFileSync('./gulp_error.log', 'Failed scss-to-css task \nDetails:\n' + error.message + '\n');
        console.error('Sass Compilation Error:', error.messageFormatted);
        process.exit(1);
    }))
    // Minify and write only the .min.css files
    .pipe(cleanCSS())
    .pipe(rename({ suffix: '.min' }))
    .pipe(gulp.dest('./src/wwwroot/styles'))
    .on('end', function () {
        console.log("SCSS to CSS compiled successfully");
        done();
    });
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
        } catch (e) {
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
        const grouped = {};
        for (const f of findings) {
            grouped[f.pattern] = grouped[f.pattern] || [];
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
            fs.writeFileSync('xss-scan-report.txt',
                findings.map(f => `${f.file}:${f.line} [${f.pattern}] ${f.text}`).join('\n'),
                'utf8');
        } catch (e) {
            console.error('Could not write xss-scan-report.txt: ' + e.message);
        }
        process.exitCode = 1;
        return done(new Error(`${findings.length} XSS-related finding(s) - see xss-scan-report.txt`));
    }

    console.log('XSS / unsafe markup scan: no risky patterns found');
    done();
});

gulp.task('security', gulp.series('security-xss-scan'));

