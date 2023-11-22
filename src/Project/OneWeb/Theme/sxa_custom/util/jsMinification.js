const gulp = require('gulp');
const gulpConcat = require('gulp-concat');
const gulpSourcemaps = require('gulp-sourcemaps');
const gulpif = require('gulp-if');
const gulpUglify = require('gulp-uglify-es').default;
const rollup = require('gulp-better-rollup')
const gulpIf = require('gulp-if');
const { babel } = require('@rollup/plugin-babel');
const commonjs = require('@rollup/plugin-commonjs');
const { nodeResolve } = require('@rollup/plugin-node-resolve');
//
const config = require(global.rootPath + '/gulp/config');

const logError = err => {
    const { name: errorName, id: filename, message } = err;

    console.error(('Error: ' + errorName).yellow);
    console.error('File:', filename);
    console.error('Message:', message)
}

const logErrorTaskStatus = () => {
    if (process.env.debug == 'true') {
        console.log(`During minfication error occured. Check that es6Support:true if you use ES6+ features.`.red)
    }
}

module.exports = function (cb) {
    let conf = config.js;
    if (!conf.enableMinification) {
        if (process.env.debug == 'true') {
            console.log('JS minification is disabled by enableMinification flag'.red)
        }
        return cb();
    }
    let streamSource = conf.minificationPath.concat(['!' + conf.jsOptimiserFilePath + conf.jsOptimiserFileName])

    var stream = gulp
        .src(streamSource)
        .pipe(gulpIf(function () {
            return conf.es6Support;
        }, rollup({
            onwarn: () => {},
            plugins: [
                babel({
                    babelHelpers: 'runtime',
                    plugins: [
                        ["@babel/plugin-transform-runtime", {
                            "helpers": true,
                            "regenerator": true
                        }]
                    ]
                }),
                commonjs(),
                nodeResolve()
            ]
        }, 'iife').on('error', logError)).on('error', logErrorTaskStatus))
        .pipe(gulpIf(function () {
            return conf.jsSourceMap;
        }, gulpSourcemaps.init()))
        .pipe(gulpConcat(conf.jsOptimiserFileName))
        .pipe(gulpUglify(config.minifyOptions.js))
        .on('error', logErrorTaskStatus)
        .pipe(gulpif(function () {
            return conf.jsSourceMap;
        }, gulpSourcemaps.write('./')))
        .pipe(gulp.dest('scripts'));

    stream.on('end', function () {
        console.log('Js minification done'.grey)
    });
    return stream;
};