
const fs = require('fs');
const path = require('path');
const onlyScripts = require('./util/scriptFilter');
const tasks = fs.readdirSync(path.resolve(__dirname, './tasks/')).filter(onlyScripts);
const configUtils = require(path.resolve(__dirname,'./util/setThemePath'));
const login = require(path.resolve(__dirname, './tasks/login'));
module.exports = {
	
	configUtils: configUtils,
	getTask: function (fileName) {
		return require(path.join(__dirname, 'tasks', fileName))
	},
	gulpTaskInit: function () {
		tasks.forEach((task) => {
			require(path.join(__dirname, 'tasks', task));
		});
	},
	login: login,
	setUserData: function(user){
		if (user && user.login && user.password){
			global.user = user;
		}
	}
}