var fs = require("fs");

var configUpdated = require("./setThemePath");

var rootPath = (function(){
    return __dirname.split("node_modules")[0]
})();

var restoreSourcesFolder = function() {
    if (!fs.existsSync(rootPath +"sources")) {
        try {
            fs.mkdirSync(rootPath +"sources");
        } catch (err) {
            if (err.code !== "EEXIST") console.log("ERROR: " + err);
        }
    }
};

restoreSourcesFolder();
//Updating path at serverConfig
configUpdated.updateConfig();
