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
//
// Emits two combined-scss outputs:
//   - fluent.scss       : :root{} light tokens + structural rules
//   - highcontrast.scss : :root{} HC tokens (via @use) + structural rules
gulp.task('combined-scss', function (done) {
    var componentFiles = glob.sync(`./src/wwwroot/styles/*.scss`);
    shelljs.mkdir('-p', './src/wwwroot/styles/combined-scss/');

    // Build fluent.scss (unchanged from original behavior)
    var getFluentScss = '';
    for (var themeOrder of componentThemeOrder) {
        var paths = componentFiles.filter((value) => { return value.indexOf('styles/' + themeOrder) !== -1; });
        if (paths.length) {
            getFluentScss += stripBom(fs.readFileSync(paths[0], 'utf8'));
        }
    }
    getFluentScss = removeCustomUse(getFluentScss);
    fs.writeFileSync('./src/wwwroot/styles/combined-scss/fluent.scss', reorderUseRules(getFluentScss), 'utf8');
.
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
