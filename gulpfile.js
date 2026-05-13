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

