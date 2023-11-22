const gulp = require('gulp');
const eslint = require('gulp-eslint');
const vinyl = require('vinyl-file');
//
const config = require(global.rootPath + '/gulp/config');
const { fileActionResolver } = require('../util/fileActionResolver');
const jsMinificator = require('../util/jsMinification');


module.exports = function watchJs() {
    setTimeout(function () {
        console.log('Watching JS files started...'.green);
    }, 0);
    var conf = config.js,
        watch = gulp.watch(conf.path, { queue: true });

    watch.on('all', function (event, path) {
        var file = {
            path: path
        };

        const isOptimizedFile = file.path.indexOf(conf.jsOptimiserFileName) !== -1;

        file.event = event;
        if (conf.enableMinification && file.path.indexOf(conf.jsOptimiserFileName) == -1) {
            jsMinificator();
        }
        if (event == 'unlink') {
            return fileActionResolver(file);
        } else {
            file = vinyl.readSync(path);
            file.event = event;

            if (isOptimizedFile) {
                return fileActionResolver(file);
            }

            var stream = gulp.src(file.path)
                .pipe(eslint())
                .pipe(eslint.format())
                .pipe(eslint.results(results => {
                    if ((results.errorCount == 0 || conf.esLintUploadOnError) && (!conf.disableSourceUploading || file.path.indexOf(conf.jsOptimiserFileName) > -1)) {
                        fileActionResolver(file);
                    } else {
                        if (conf.disableSourceUploading) {
                            if (process.env.debug == 'true') {
                                console.log(`Uploading ${file.basename} prevented because value disableSourceUploading:true`.yellow);
                            }
                        } else {
                            if (process.env.debug == 'true') {
                                console.log('Please fix errors before uploading or set esLintUploadOnError:true'.yellow);
                            }
                        }
                    }
                }));
            return stream;
        }
    })
};