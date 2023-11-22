const config = require(global.rootPath + '/gulp/config');
const path = require('path');
const request = require('request');
const fs = require('fs');
const colors = require('colors');

module.exports = function (file) {
    let conf = config.serverOptions,
        name = path.basename(file.path),
        dirName = path.dirname(file.path),
        relativePath = path.relative(global.rootPath, dirName),
        prom = new Promise((resolve, reject) => {
            setTimeout(function () {
                resolve();
            }, 600);
            let url = [conf.server,
            conf.updateTemplatePath,
                '?user=',
            config.user.login,
                '&password=',
            config.user.password,
                '&path=',
                name
            ].join('');
            var formData = {
                file: fs.createReadStream(relativePath + '/' + name),
            };
            var a = request.post({
                url: url,
                formData: formData,
                agentOptions: {
                    rejectUnauthorized: false
                }
            }, function (err, httpResponse, body) {
                resolve();
                if (err) {
                    return console.log(('upload failed:' + err).red);
                }
                if (httpResponse.statusCode !== 200) {
                    console.log(('Status code:' + httpResponse.statusCode).red);
                    console.log(('Answer:' + httpResponse.body).red);
                    return
                } else {
                    return console.log(('Uploading of ' + name + ' was successful!').green);
                }


            });

        });
    return prom;
}