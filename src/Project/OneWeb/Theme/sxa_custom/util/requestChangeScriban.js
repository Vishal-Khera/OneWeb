const config = require(global.rootPath + '/gulp/config');
const path = require('path');
const request = require('request');
const fs = require('fs');

function scribanFileFilter(name) {
    return /(\.(scriban)$)/i.test(name);
};

var getScribanFiles = function (dir) {
    var results = [],
        list = fs.readdirSync(dir);
    list.forEach(function (fileInst) {
        let file = dir + '/' + fileInst,
            stat = fs.statSync(file);
        if (stat && stat.isDirectory()) {
            results = results.concat(getScribanFiles(file));
        } else {
            if (scribanFileFilter(file)) {
                results.push(file);
            }
        }
    });
    return results;
}

function getPayload(file) {
    let dirName = file.path.replace(/\\/g, "/"),
        variantFolderPaths = dirName.match(/.*-.scriban.*(?=\/)/g),
        streams = [],
        variantRootPath = variantFolderPaths[0];
    getScribanFiles(variantRootPath).forEach((scribanFile) => {
        var content = fs.readFileSync(scribanFile, 'utf8'),
            contentBuffer = new Buffer(content),
            fileObj = {
                path: scribanFile,
                content: contentBuffer.toString('base64')
            };
        streams.push(fileObj);
    });
    return streams;
}

function getMetadata() {
    var conf = config.scriban;
    if (fs.existsSync(conf.metadataFilePath)) {
        let rawData = fs.readFileSync(conf.metadataFilePath);
        return JSON.parse(rawData);
    }
    return { "siteId": "" }
}

module.exports = function (file) {
    var conf = config.serverOptions,
        prom = new Promise((resolve, reject) => {
            setTimeout(function () {
                resolve();
            }, 600);

            let url = [conf.server,
            conf.updateScribanPath,
                '?user=',
            config.user.login,
                '&password=',
            config.user.password,
                '&path=',
            file.path
            ].join('');
            var formData = {
                streams: JSON.stringify(getPayload(file)),
                metadata: JSON.stringify(getMetadata())
            };
            var a = request.post({
                url: url,
                formData: formData,
                agentOptions: {
                    rejectUnauthorized: false
                }
            }, function (err, httpResponse, body) {
                try {
                    var response = JSON.parse(body);
                    if (!response.result) {
                        console.log(('Error: ' + response.Reason).red);
                    } else {
                        if (process.env.debug == 'true') {
                            console.log('Scriban import was successful!'.green);
                        }
                    }
                    if (err) {
                        if (process.env.debug == 'true') {
                            return console.error(('Scriban import failed:').red, err.red);
                        } else {
                            return
                        }
                    }
                } catch (e) {
                    if (process.env.debug == 'true') {
                        console.log(('Server Error').red)
                        console.log(('Status code:' + httpResponse.statusCode).red);
                        console.log(('Answer:' + httpResponse.body).red);
                    }

                }
            });

        });
    return prom;
}