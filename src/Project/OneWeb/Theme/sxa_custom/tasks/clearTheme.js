const requestClearTheme = require('../util/requestClearTheme');

module.exports = function clearThemeTasks(cb){
    return requestClearTheme(cb);
}
