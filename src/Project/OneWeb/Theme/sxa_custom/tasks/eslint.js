const gulp = require('gulp');
const eslint = require('gulp-eslint');
//
const config = require(global.rootPath + '/gulp/config');
//
module.exports = function lint() {
    var conf = config.js;
    // ESLint ignores files with "node_modules" paths.
    // So, it's best to have gulp ignore the directory as well.
    // Also, Be sure to return the stream from the task;
    // Otherwise, the task may end before the stream has finished.
    return gulp.src(['scripts/*.js', '!node_modules/**'])
        // eslint() attaches the lint output to the "eslint" property
        // of the file object so it can be used by other modules.
        .pipe(eslint())
        // eslint.format() outputs the lint results to the console.
        // Alternatively use eslint.formatEach() (see Docs).
        .pipe(eslint.formatEach())
        // .pipe(eslint.results(results => {
        //     console.log(results)
        // }));
};