const gulp = require("gulp");
const spritesmith = require('gulp.spritesmith');
const config = require(global.rootPath + '/gulp/config');

module.exports = function sprites() {
    let conf = config.sprites.flags,
        spriteData = gulp.src(conf.flagsFolder).pipe(spritesmith(conf.spritesmith));

    spriteData.img.pipe(gulp.dest(conf.imgDest));
    spriteData.css.pipe(gulp.dest(conf.cssDest))
    spriteData.on('end',function(){
        console.log('Sprite build done'.grey)
    })
    return spriteData;
};