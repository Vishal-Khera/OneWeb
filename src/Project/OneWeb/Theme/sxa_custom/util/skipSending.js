const path = require('path');
const config = require(global.rootPath + '/gulp/config');
const colors = require('colors');

var testPath = function(element, filePath) {
    var res = false;
    if (typeof element == 'object') {
        res = element.test(filePath);
    } else {
        res = filePath.indexOf(path.normalize(element)) > -1;
    }
    return res;
}

module.exports = function(filePath) {
    let excludedPath = config.excludedPath.some(function(element, index, array) {
        return testPath(element, filePath)
    });
    let nameValidation = config.serverNameValidation.some(function(element, index, array) {
        let pathNoExtension = path.basename(filePath).replace(/\.[^/.]+$/, ""),
            nameValidation = !testPath(element, pathNoExtension);
        if (nameValidation) {
            console.log('Incorrect File Name.'.blue);
        }
        return nameValidation;
    });
    return !!(excludedPath || nameValidation);
}