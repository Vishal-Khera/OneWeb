const fs = require('fs');
//
module.exports = function (filePath) {
    var targetPath = filePath.replace('sass', 'styles').replace('.scss', '.css');
    if (fs.existsSync(targetPath)) {
        fs.unlinkSync(targetPath)
    }
};