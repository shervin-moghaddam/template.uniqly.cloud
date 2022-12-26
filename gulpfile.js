// npm install gulp --save-deps && npm install gulp-sass --save-deps && && npm install sass --save-deps
'use strict'
const {src, dest, watch} = require('gulp');
const sass = require('gulp-sass')(require('sass'));

// Paths
var paths = {
    scss: 'wwwroot/scss/',
    css: 'wwwroot/css/',
    js: 'wwwroot/js/'
};

// Files to watch
const scssFiles = ("login.scss,sidebar.scss,uniqly-forms.scss").split(",");

// Compile SCSS to CSS
function compileSass(done) {
    for (let i = 0; i < scssFiles.length; i++) {
        src(paths.scss + scssFiles[i])
            .pipe(sass().on('error', sass.logError))
            .pipe(dest('wwwroot//css'));
        done();
    };
};

function watchSass() {
    watch('app/scss/app.scss', compileSass);
};

// Call functions
exports.compileSass = compileSass // One-time compile
exports.watchSass = watchSass // Will automatically compile SCSS to CSS