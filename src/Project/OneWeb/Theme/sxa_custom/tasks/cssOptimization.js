const cssMinificator = require('../util/cssMinificator');

module.exports = function cssOptimization(cb){
    return cssMinificator(cb)
}