const config = require(global.rootPath + '/gulp/config');

const request = require('request');
const colors = require('colors');

module.exports = function (cb) {
    let conf = config.serverOptions;
	var server = global.server != undefined ? global.server : conf.server;
	var login = global.user != undefined ? global.user.login : config.user.login;
	var password = global.user != undefined ? global.user.password : config.user.password;		
        url = [
            server,
            conf.clearThemePath,
            '?user=',
            login,
            '&password=',
            password,
            '&path=',
            conf.projectPath,
            conf.themePath,
            '/',
            '&database=master'
        ].join('');
    setTimeout(function () {
        request.get({
            url: url,
            agentOptions: {
                rejectUnauthorized: false
            }
        }, function (err, httpResponse, body) {
            try {
                var response = JSON.parse(body);
                if (!response.result) {
                    console.log(('Error: ' + response.Reason).red);
                } else {
                    console.log('Clearing successfull!'.green);
                }
                if (err) {
					return console.error(('Clearing failed:').red, err.red);
					return cb();
                }
				return cb();
            } catch (e) {
                console.log(('Status code:' + httpResponse.statusCode).red);
                console.log(('Answer:' + httpResponse.body).red);
				return cb();
            }
        });
    }, 5000)
}