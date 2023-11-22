const gulp = require("gulp");
const sass = require('gulp-sass');
const sourcemaps = require('gulp-sourcemaps');
const autoprefixer = require('gulp-autoprefixer');
const bulkSass = require('gulp-sass-bulk-import');
const gulpif = require('gulp-if');
//
const config = require(global.rootPath + '/gulp/config');
const destPath = require('./destPath');

module.exports = function (origFile, fileName) {
    return gulp.src('sass/' + fileName)
        .pipe(bulkSass())
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
        .pipe(gulp.dest(function (file) { return destPath({ origFile: origFile, file: file, event: origFile.event }) }))
};