// npm install gulp --save-deps && npm install gulp-sass --save-deps && && npm install sass --save-deps
'use strict'

// Mods
const {src, dest, watch} = require('gulp');
const sass = require('gulp-sass')(require('sass'));
const minify = require('gulp-minify');
const javascriptObfuscator = require('gulp-javascript-obfuscator');

// Paths
const paths = {
    scss: 'wwwroot/scss/',
    css: 'wwwroot/css/',
    js: 'wwwroot/js/',
};

// Files to watch
const scssFiles = ("login.scss,sidebar.scss,uniqly-forms.scss").split(",");

// Compile SCSS to CSS
async function compileSass() {
    for (let i = 0; i < scssFiles.length; i++) {
        src(paths.scss + scssFiles[i])
            .pipe(sass().on('error', sass.logError))
            .pipe(dest('wwwroot//css'));
    }
}

function watchSass() {
    watch('app/scss/app.scss', compileSass);
}

// Minify JS
async function minifyJS(fromObfuscated = false) {
    let path = paths.js;
    if (fromObfuscated) path += 'obfuscated/';
    return src(path, {allowEmpty: true})
        .pipe(minify({noSource: true}))
        .pipe(dest(paths.js + 'min/'));
}

// Obfuscate JS
async function obfuscateJS() {
    return src(paths.js + '/*.js', {allowEmpty: true})
        .pipe(javascriptObfuscator())
        .pipe(dest(paths.js + 'obfuscated/'));
}

// Complete run of all tasks
async function run(done) {
    console.log("Compiling SCSS to CSS...")
    compileSass();
    console.log("Obfuscating JS files...")
    obfuscateJS();
    console.log("Minifying JS files...")
    minifyJS(true);
    done();
}

// Call functions
exports.compileSass = compileSass // One-time compile
exports.watchSass = watchSass // Will automatically compile SCSS to CSS
exports.minifyJS = minifyJS // Minify all JS files
exports.obfuscateJS = obfuscateJS // Obfuscate all JS files
exports.default = run // Run all tasks