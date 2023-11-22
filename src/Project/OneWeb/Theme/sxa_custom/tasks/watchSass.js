const gulp = require('gulp');
const sass = require('gulp-sass');
const sourcemaps = require('gulp-sourcemaps');
const rename = require('gulp-rename');
const autoprefixer = require('gulp-autoprefixer');
const path = require('path');
const bulkSass = require('gulp-sass-bulk-import');
const gulpif = require('gulp-if');
const colors = require('colors');
const vinyl = require('vinyl-file');
//
const destPath = require('../util/destPath');
const config = require(global.rootPath + '/gulp/config');
const stylesBuilder = require('../util/sassStyleBuilder');
const componentsBuilder = require('../util/sassComponentsBuilder');
const updateFromDependency = require('../util/sassUpdateFromDependency');
const removeTargetCss = require('../util/removeTargetCss');
//
module.exports.watchStyles = function watchStyles() {
    let conf = config.sass.styles,
        watch = gulp.watch(conf.sassPath);
    setTimeout(function () {
        console.log('Watching SASS styles files started...'.green);
    }, 0);
    watch.on('all', function (event, path) {
        var file = {
            path: path
        };
        if (event !== 'unlink') {
            file = vinyl.readSync(path);
        }

        file.event = event;
        if (!conf.disableSourceUploading) {
            stylesBuilder(file);
        } else {
            if (process.env.debug == 'true') {
                console.log(`Uploading ${file.basename} prevented because value disableSourceUploading:true`.yellow);
            }

        }
    })

};
module.exports.watchBase = function watchBase() {
    let conf = config.sass.core,
        watch = gulp.watch(conf.sassPath)
    setTimeout(function () {
        console.log('Watching SASS base files started...'.green);
    }, 0);
    watch.on('all', function (event, path) {
        var file = {
            path: path
        };
        if (event !== 'unlink') {
            file = vinyl.readSync(path);
        }

        file.event = event;
        if (!conf.disableSourceUploading) {
            stylesBuilder(file);
            componentsBuilder(file);
        } else {
            if (process.env.debug == 'true') {
                console.log(`Uploading ${file.basename} prevented because value disableSourceUploading:true`.yellow);
            }
        }
    })
};

const watchComponents = module.exports.watchComponents = function watchComponentsTask() {
    var conf = config.sass.components,
        watch = gulp.watch(conf.sassPath, { base: path.resolve('sass/'), queue: true })
    setTimeout(function () {
        console.log('Watching SASS components files started...'.green);
    }, 0);
    gulp.watch(conf.sassPath, { depth: 1 })
    watch.on('all', function (event, path) {
        var file = {
            path: path
        };
        if (event !== 'unlink') {
            file = vinyl.readSync(path);
        }

        file.event = event;
        if (event === 'unlink') {
            if (!conf.disableSourceUploading) {
                removeTargetCss(path);
            } else {
                if (process.env.debug == 'true') {
                    console.log(`Uploading ${file.basename} prevented because value disableSourceUploading:true`.yellow);
                }
            }
        } else {
            gulp.src(path)
                .pipe(gulpif(function () {
                    return file.event == 'change' || file.event == 'add';
                }, bulkSass()))
                .pipe(gulpif(function () {
                    return config.sassSourceMap;
                }, sourcemaps.init()))
                .pipe(sass({ outputStyle: 'expanded' })
                    .on('error', function (error) {
                        gulp.series(watchComponents);
                        console.log('watchSass should be restarted'.red)
                        sass.logError.call(this, error);

                    })
                )
                .pipe(autoprefixer(config.autoprefixer))
                .pipe(rename({ dirname: '' }))
                .pipe(gulpif(function () {
                    return config.sassSourceMap;
                }, sourcemaps.write()))
                .pipe(gulp.dest(function (file) { return destPath({ file: file, event: event }) })
                );
        }

    })
};

module.exports.watchDependency = function watchDependency() {
    let conf = config.stylesConfig,
        watch = gulp.watch(['sass/styles/**/*.scss', 'sass/variants/**/*.scss'], { queue: true, base: path.resolve('sass/') })
    setTimeout(function () {
        console.log('Watching SASS dependency files started...'.green);
    }, 0);
    watch.on('all', function (event, path) {
        var file = {
            path: path
        };
        if (event !== 'unlink') {
            file = vinyl.readSync(path);
            file.event = event;
            let dirName = file.dirname.match('[a-z\-]*$')[0],
                fileName = conf[dirName];
            if (fileName && !conf.disableSourceUploading) {
                updateFromDependency(file, fileName);
            } else {
                if (conf.disableSourceUploading) {
                    if (process.env.debug == 'true') {
                        console.log(`Uploading ${file.basename} prevented because value disableSourceUploading:tru`.yellow);
                    }
                } else {
                    console.log('Uploading prevented because value of fileName is not defined'.yellow);
                }

            }
        }
    })
};
