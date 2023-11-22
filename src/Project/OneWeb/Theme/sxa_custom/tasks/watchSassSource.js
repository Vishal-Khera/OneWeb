const gulp = require('gulp');
const vinyl = require('vinyl-file');
//
const config = require(global.rootPath + '/gulp/config');
const { fileActionResolver } = require('../util/fileActionResolver');

module.exports = function watchSassSource() {
    var conf = config.sass,
        watch = gulp.watch(conf.root, { queue: true });
    setTimeout(function () {
        console.log('Watching SASS source files started...'.green);
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
            fileActionResolver(file);
        } else {
            if (process.env.debug == 'true') {
                console.log('Uploading prevented because value disableSourceUploading:true'.yellow);
            }
        }
    })
};