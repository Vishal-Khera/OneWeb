module.exports = function login() {
    const config = require(global.rootPath + '/gulp/config'),
        inquirer = require('inquirer'),
        loginPromise = new Promise((resolve, reject) => {
            //if already entered credentials
            if (typeof global.user!=='undefined') {
                config.user = global.user
                return resolve(global.user);
            }
            if (config.user.login.length) {
                return resolve(config.user);
            }
            inquirer.prompt(config.loginQuestions).then(answer => {
                if (!(answer.login.length && answer.password.length)) {
                    reject('loginError');
                    process.exit();
                } else {
                    resolve(answer)
                    config.user = answer
                }
            });
        });
    return loginPromise;
}