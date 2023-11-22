const gulp = require("gulp");
const sass = require('gulp-sass');
const sourcemaps = require('gulp-sourcemaps');
const autoprefixer = require('gulp-autoprefixer');
const bulkSass = require('gulp-sass-bulk-import');
const gulpif = require('gulp-if');
//
const destPath = require('../util/destPath');
const config = require(global.rootPath + '/gulp/config');

module.exports = function (origFile) {
    var conf = config.sass.components;
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
        .pipe(gulpif(function () {
            return config.sassSourceMap;
        }, sourcemaps.write()))
        .pipe(gulp.dest(function (file) { return destPath({ origFile: origFile, file: file, event: file.event }) }))
    stream.on('end', function () {
        console.log('Sass build done'.grey)
    });
    return stream;
}