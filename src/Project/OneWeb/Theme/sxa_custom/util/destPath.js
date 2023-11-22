const path = require('path');

var getLogMessage = function (files, stylePath) {
    var message = 'Action: ';
    if (typeof files.origFile !== 'undefined') {
        message += files.origFile.event;
    } else if (typeof files.file !== 'undefined' && files.file.event) {
        message += files.file.event;
    } else if (typeof files.file !== 'undefined' && files.event) {
        message += files.event;
    }
    message += '. File: ';
    message += stylePath;
    message += '/';
    message += path.basename(files.file.path);
    return message;
}

module.exports = function (files, style = 'styles') {
    if (process.env.debug === 'true') {
        console.log(getLogMessage(files, style).yellow);
    }
    return style;
}