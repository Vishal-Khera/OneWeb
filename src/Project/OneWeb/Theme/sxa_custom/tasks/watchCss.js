const gulp = require('gulp');
const colors = require('colors');
const vinyl = require('vinyl-file');
//
const config = require(global.rootPath + '/gulp/config');
const { fileActionResolver } = require('../util/fileActionResolver');
const cssMinificator = require('../util/cssMinificator');

module.exports = function watchCss() {
    setTimeout(function () {
        console.log('Watching CSS files started...'.green);
    }, 0);
    let conf = config.css,
        indervalId,
        watch = gulp.watch(conf.path, { queue: true });
    watch.on('all', function (event, path) {
        var file = {
            path: path
        };
        if (event !== 'unlink') {
            file = vinyl.readSync(path);
        }

        file.event = event;
        if (!conf.disableSourceUploading || file.path.indexOf(conf.cssOptimiserFileName) > -1) {
            fileActionResolver(file);
        } else {
            if (process.env.debug == 'true') {
                console.log(`Uploading ${file.basename} prevented because value disableSourceUploading:true`.yellow);
            }
        }
        if (conf.enableMinification && file.path.indexOf(conf.cssOptimiserFileName) == -1) {
            indervalId && clearTimeout(indervalId);
            indervalId = setTimeout(function () {
                indervalId = clearTimeout(indervalId);
                cssMinificator();
            }, 400)
        }
    })
}
