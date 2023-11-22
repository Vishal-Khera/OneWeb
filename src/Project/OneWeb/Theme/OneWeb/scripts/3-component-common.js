var owCommon = (function () {

    var _insertUrlParameter = function (key, value) {
        if (history.pushState) {
            let searchParams = new URLSearchParams(window.location.search);
            searchParams.set(key, value);
            let newurl = window.location.protocol + "//" + window.location.host + window.location.pathname + '?' + searchParams.toString();
            window.history.pushState({ path: newurl }, '', newurl);
        }
    }


    var _removeUrlParameter = function (paramKey) {
        const url = window.location.href
        console.log("url", url)
        var r = new URL(url)
        r.searchParams.delete(paramKey)
        const newUrl = r.href
        console.log("r.href", newUrl)
        window.history.pushState({ path: newUrl }, '', newUrl)
    }

    var _getUrlWithoutParam = function(paramKey) {
        const url = window.location.href
        console.log("url", url)
        var r = new URL(url)
        r.searchParams.delete(paramKey)
        const newUrl = r.href
        return newUrl;
        }

    var _getQueryStringParams = function (key) {
        var vars = [],
            hash;
        var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
        for (var i = 0; i < hashes.length; i++) {
            hash = hashes[i].split('=');
            vars.push(hash[0]);
            vars[hash[0]] = hash[1];
        }
        return vars[key];
    };

    var _isValidJson = function (str) {
        try {
            if (typeof str == 'object') {
                return true;
            }
            JSON.parse(str);
        } catch (e) {
            return false;
        }
        return true;
    };

    var _isJson = function (str) {
        if (typeof str == 'object') {
            return true;
        }
    };
    var _getKeyword = function () {
        var keyWord = _getQueryStringParams('keyword');
        if (keyWord != undefined && keyWord.length > 0) {
            return decodeURIComponent(keyWord.replace('+', ' '));
        }
        return '';
    };

    var _getFilters = function () {
        var filterArray = [];
        try {
            var filterString = _getQueryStringParams('filters');
            if (filterString != undefined) {
                var filters = filterString.replace('+', ' ').split(';');
                filters.forEach(filter => {
                    var filterKey = filter.split(':')[0];
                    var filterValues = filter.split(':')[1].split('|');
                    var filterObject = {
                        Key: filterKey,
                        Values: filterValues,
                    };
                    filterArray.push(filterObject);
                });
            }
        } catch (error) {
            console.log('error parsing filters from query string' + error);
        }
        return filterArray;
    };

    const registerHelpers = function () {
        Handlebars.registerHelper('equals', function (v1, v2) {
            return v1 == v2;
        });

        Handlebars.registerHelper('notEquals', function (v1, v2) {
            return v1 != v2;
        });

        Handlebars.registerHelper('isdefined', function (value) {
            return value != undefined && value != 'null' && value != '';
        });

        Handlebars.registerHelper('length', function (value) {
            return value != undefined && value.length > 0;
        });

        Handlebars.registerHelper('isNotEmpty', function (value) {
            return value != undefined && !jQuery.isEmptyObject(value);
        });

        Handlebars.registerHelper('wrapSpan', function (v1, v2) {
            if(v1 != undefined && v2 != undefined){                
                return v1.replace(new RegExp(v2, "ig") ,'<span>' + v2 + '</span>');
            }
            return v1;
        });

        Handlebars.registerHelper('gt', function (v1, v2) {
            return v1 != undefined && v2 != undefined && parseInt(v1) > parseInt(v2);
        });

        Handlebars.registerHelper('sanitize', function (value) {
            return $("<div/>").html(value).text();;
        });

        Handlebars.registerHelper('lt', function (v1, v2) {
            return v1 != undefined && v2 != undefined && parseInt(v1) < parseInt(v2);
        });

        Handlebars.registerHelper('normalize', function (value) {
            if (value != undefined) {
                return value.replace(' ', '');
            }
        });

        Handlebars.registerHelper('jsonBlock', function (param, options) {
            try {
                if (options.data) {
                    Handlebars.createFrame(options.data);
                }
                if (owCommon.isJson(param)) {
                    return options.fn(this, { data: param });
                } else {
                    return options.fn(this, { data: JSON.parse(param) });
                }
            } catch (error) {
                console.log(error);
            }
        });
    };

    return {
        getQueryStringParams: _getQueryStringParams,
        insertUrlParam: _insertUrlParameter,
        removeUrlParam: _removeUrlParameter,
        getUrlWithoutParam:_getUrlWithoutParam,
        isJson: _isJson,
        isValidJson: _isValidJson,
        getKeyword: _getKeyword,
        getFilters: _getFilters,
        registerHelpers: registerHelpers,
    };
})();

$().ready(function () {
    owCommon.registerHelpers();
});