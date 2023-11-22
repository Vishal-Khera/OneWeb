const gulp = require("gulp");
const colors = require("colors");
const babel = require("gulp-babel");
const config = require(global.rootPath + '/gulp/config');
const { fileActionResolver } = require("../util/fileActionResolver");
const vinyl = require('vinyl-file');

module.exports = function watchEs() {
    setTimeout(function () {
        console.log("Watching ES files started...".green);
    }, 0);
    var conf = config.es,
        watch = gulp.watch(conf.path, { queue: true });
    watch.on('all', function (event, path) {
        var file = {
            path: path
        };
        file.event = event;
        if (event == 'unlink') {
            return fileActionResolver(file);
        } else {
            file = vinyl.readSync(path);
            file.event = event;
            if (!conf.disableSourceUploading) {
                fileActionResolver(file);
            } else {
                if (process.env.debug == 'true') {
                    console.log(`Uploading ${file.basename} prevented because value disableSourceUploading:true`.yellow);
                }
            }
            var stream = gulp
                .src(file.path)
                .pipe(babel())
                .pipe(gulp.dest(function () {
                    console.log(`Transpiled sources/${file.basename} to scripts/${file.basename}`.yellow)
                    return "scripts";
                }));

            return stream;
        }
    });
}
