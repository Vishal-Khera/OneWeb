//Used in post init
let path = require('path');
let fs = require('fs');
var rootPath = (function () {
    return __dirname.split("node_modules")[0]
})();
var getConf = () => {
    let fileContent = fs.readFileSync(path.join(global.rootPath, 'gulp/serverConfig.json'))
    return JSON.parse(fileContent)
}
let setConfig = (content) => {
    fs.writeFileSync(path.join(global.rootPath, 'gulp/serverConfig.json'), JSON.stringify(content));
}
module.exports = {
    "getConf": getConf,
    "updateConfig": function () {
        let mediaLibPath = rootPath.split('-' + path.sep + 'media'),
            isCreativeExchange = mediaLibPath.length > 1,
            themePath,
            projPath;
        if (isCreativeExchange) {
            try {
                let config = getConf(),
                    pathSep = path.sep == "\\" ? path.sep + path.sep : path.sep,
                    pathReg = new RegExp(pathSep + "[\\w\\d\\. ]*$", "i");
                themePath = mediaLibPath[1].match(pathReg)[0];
                projPath = mediaLibPath[1].replace(pathReg, '');
                config.serverOptions.projectPath = projPath;
                config.serverOptions.themePath = themePath;
                setConfig(config);
            } catch (e) {
                console.log(e);
            }

        }
    }
}