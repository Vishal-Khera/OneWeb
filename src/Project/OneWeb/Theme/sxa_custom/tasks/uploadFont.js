const gulp = require('gulp');
const eslint = require('gulp-eslint');
const colors = require('colors');
const vinyl = require('vinyl-file');
const { EventEmitter } = require('events');
//
const config = require(global.rootPath + '/gulp/config');
const { fileActionResolver } = require('../util/fileActionResolver');

module.exports = function uploadImg() {
	var conf = config.font;
	const emitter = new EventEmitter();
	const promises = [];
	
	gulp.src(conf.path)
		.on('data', function (_file) {
			let file = _file;
			file.event = 'change';
			if (!conf.disableSourceUploading || file.path.indexOf(conf.cssOptimiserFileName) > -1) {
				promises.push(() => 
				{
					console.log('Uploading file - ' + file.basename.yellow);
					fileActionResolver(file)
				})
			} else {
				if (process.env.debug == 'true') {
					console.log(`Uploading of ${file.basename} prevented because value disableSourceUploading:true`.yellow);
				}
			}
		})
		.on('end', async function () {
			for (const prom of promises) {
				await prom();
			}

			emitter.emit('finish');
		})
	return emitter
};