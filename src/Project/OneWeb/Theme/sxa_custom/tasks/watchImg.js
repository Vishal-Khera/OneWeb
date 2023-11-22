const gulp = require('gulp');
const colors = require('colors');
const vinyl = require('vinyl-file');
//
const config = require(global.rootPath + '/gulp/config');
const { fileActionResolver } = require('../util/fileActionResolver');
//
module.exports = function watchImg() {
    setTimeout(function () {
        console.log('Watching Image files started...'.green);
    }, 0);
    var conf = config.img,
        watch = gulp.watch(conf.path, { queue: true });
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
}