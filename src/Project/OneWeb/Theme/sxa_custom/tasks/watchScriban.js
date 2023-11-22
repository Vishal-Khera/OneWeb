const gulp = require('gulp');
const colors = require('colors');
const vinyl = require('vinyl-file');
//
const config = require(global.rootPath + '/gulp/config');
const { fileActionResolver } = require('../util/fileActionResolver');
//

module.exports = function watchScriban() {
    setTimeout(function () {
        console.log('Watching Scriban files started...'.green);
    }, 0);
    var conf = config.scriban,
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