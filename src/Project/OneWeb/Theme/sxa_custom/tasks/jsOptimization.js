const jsMinificator = require('../util/jsMinification');

module.exports = function jsOptimization(cb){
    return jsMinificator(cb);
}
