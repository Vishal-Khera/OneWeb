const gulp = require("gulp");
const sass = require('gulp-sass');
const sourcemaps = require('gulp-sourcemaps');
const rename = require('gulp-rename');
const concat = require('gulp-concat');
const autoprefixer = require('gulp-autoprefixer');
const bulkSass = require('gulp-sass-bulk-import');
const gulpif = require('gulp-if');
//
const config = require(global.rootPath + '/gulp/config');
const destPath = require('./destPath');

module.exports = function (origFile) {
    let conf = config.sass.styles;
    let stream = gulp.src(conf.sassPath)
        .pipe(gulpif(function (file) {
            return typeof origFile == 'undefined' || origFile.event == 'change' || origFile.event == 'add';
        }, bulkSass()))
        .pipe(gulpif(function () {
            return config.sassSourceMap;
        }, sourcemaps.init()))
        .pipe(sass({ outputStyle: 'expanded' }).on('error', function (error) {
            sass.logError.call(this, error);
        }))
        .pipe(autoprefixer(config.autoprefixer))
        .pipe(rename({ dirname: '' }))
        .pipe(concat(conf.concatName))
        .pipe(gulpif(function () {
            return config.sassSourceMap;
        }, sourcemaps.write('./')))
        .pipe(gulp.dest(function (file) { return destPath({ origFile: origFile, file: file, event: file.event }) }));
    stream.on('end', function () {
        console.log('Sass style build done'.grey)
    });
    return stream;
};