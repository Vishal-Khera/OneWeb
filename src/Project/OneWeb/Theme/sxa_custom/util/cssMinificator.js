const gulp = require('gulp');
const cleanCSS = require('gulp-clean-css');
const path = require('path');
const gulpSourcemaps = require('gulp-sourcemaps');
const gulpif = require('gulp-if');
const gulpConcat = require('gulp-concat');
//
const config = require(path.resolve(process.cwd(), './gulp/config'));

module.exports = function (cb) {
    let conf = config.css;
    if (!conf.enableMinification) {
        if (process.env.debug == 'true') {
            console.log('CSS minification is disabled by enableMinification flag'.red)
        }
        return cb();
    }
    let streamSource = conf.minificationPath.concat(['!' + conf.cssOptimiserFilePath + conf.cssOptimiserFileName])
    let stream = gulp.src(streamSource)
        .pipe(gulpif(function () {
            return conf.cssSourceMap;
        }, gulpSourcemaps.init()))
        .pipe(cleanCSS(config.minifyOptions.css))
        .pipe(gulpConcat(conf.cssOptimiserFileName))
        .pipe(gulpif(function () {
            return conf.cssSourceMap;
        }, gulpSourcemaps.write('./')))
        .pipe(gulp.dest('styles'));
    stream.on('end', function () {
        console.log('Css minification done'.grey)
    });
    return stream

}