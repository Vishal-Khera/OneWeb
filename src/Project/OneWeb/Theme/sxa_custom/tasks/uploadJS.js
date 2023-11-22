const gulp = require('gulp');
const path = require('path');
const eslint = require('gulp-eslint');
const tap = require('gulp-tap');
const colors = require('colors');
const vinyl = require('vinyl-file');
//
const config = require(global.rootPath + '/gulp/config');
const { fileActionResolver } = require('../util/fileActionResolver');

module.exports = function uploadJs() {
	var conf = config.js;
	const promises = [];
	const OPTIMIZED_FILE_PATH = conf.jsOptimiserFilePath + conf.jsOptimiserFileName;

	if (!conf.enableMinification && conf.disableSourceUploading) {
		if (process.env.debug == 'true') {
			console.log(
				`Uploading and minification are disabled(enableMinification, disableSourceUploading flags)`.red
			)
		}
		return
	}

	const pushFile = file => {
		file.event = 'change';		
		promises.push(() => 
		{
			console.log('Uploading file - ' + file.basename.yellow);
			fileActionResolver(file)
		});
	}
	
	return gulp.src([conf.path])
		.pipe(tap((file) => {
			if (file.path.indexOf(conf.jsOptimiserFileName) === -1) {
				return;
			}

			pushFile(file);
		}))
		.pipe(eslint({
			ignorePattern: OPTIMIZED_FILE_PATH
		}))
		.pipe(eslint.format())
		.pipe(eslint.results(results => {
			results.forEach(result => {
				let file = vinyl.readSync(result.filePath);
				if ((result.errorCount == 0 || conf.esLintUploadOnError) && (!conf.disableSourceUploading || result.filePath.indexOf(conf.jsOptimiserFileName) > -1)) {			
					pushFile(file);
				} else {
					if (conf.disableSourceUploading) {
						if (process.env.debug == 'true') {
							console.log(('Uploading of ' + path.basename(result.filePath) + ' prevented because value disableSourceUploading:true').yellow);
						}
					} else {
						if (process.env.debug == 'true') {
							console.log(('Please fix errors in  ' + path.basename(result.filePath) + '  before uploading or set esLintUploadOnError:true').yellow);
						}
					}
				}
			});
		}))
		.on('end', async () => {
			for (const prom of promises) {
				await prom();
			}
		})
};