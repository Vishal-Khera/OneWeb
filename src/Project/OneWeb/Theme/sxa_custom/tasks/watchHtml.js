const gulp = require('gulp');
const colors = require('colors');
const vinyl = require('vinyl-file');
const fs = require('fs');
const path = require('path');
//
const config = require(global.rootPath + '/gulp/config');
const { fileActionResolver } = require('../util/fileActionResolver');
//
module.exports = function watchHtml() {
    try {
        var conf = config.html;
        if (conf.path) {
            setTimeout(function () {
                console.log('Watching HTML files started...'.green);
            }, 0);
            watch = gulp.watch(conf.path, { queue: true, ignorePermissionErrors: true });
            watch.on('all', function (event, path) {
                var file = {
                    path: path
                };
                if (event !== 'unlink') {
                    file = vinyl.readSync(path);
                }

                file.event = event;
                fileActionResolver(file);
            })
        } else {
            setTimeout(function () {
                console.log('Watching HTML available only in creative exchange package...'.yellow);
            }, 0);
        }

    } catch (e) {
        console.log(e)
    }
}