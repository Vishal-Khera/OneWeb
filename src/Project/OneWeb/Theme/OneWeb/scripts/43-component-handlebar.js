var owHandlebar = (function () {
    function sendHandleBarRequest(item) {
        var hbObject = {
            Endpoint: $(item).attr('ow-endpoint'),
            SourceId: $(item).attr('ow-source-id'),
            ConfigId: $(item).attr('ow-config-id'),
            Keyword: owCommon.getKeyword(),
            Filters: owCommon.getFilters(),
            Parameters: {},
            Append: false
        };

        if($('.ow-search-box').length > 0){
            hbObject.Language = $('.ow-search-box').attr('ow-language');
        }

        if (hbObject.Endpoint != undefined && hbObject.Endpoint.length > 0 && hbObject.SourceId != undefined) {
            $(item).each(function () {
                $.each(this.attributes, function () {
                    if (this.specified) {
                        if (this.name.indexOf('ow-') != -1) {
                            hbObject.Parameters[this.name] = this.value;
                        }
                    }
                });
            });
            fetch(hbObject);
            $(item).addClass('ow-initialized');
        }

        return hbObject;
    };

    const initiate = function () {
        const filterHandleBar = $('.ow-search-template[is-filter]').not('.ow-initialized');
        if (filterHandleBar) {
            [].map.call(filterHandleBar, function (item) {
                sendHandleBarRequest(item);
            });
        }

        const searchHandleBar = $('.ow-search-template[is-result]').not('.ow-initialized');
        if (searchHandleBar) {
            [].map.call(searchHandleBar, function (item) {
                sendHandleBarRequest(item);
            });
        }
    };

    var fetch = function (hbObject) {
        if(hbObject.PageType == 0){
            owCommon.removeUrlParam('page');
        }else{
            var currentPage = owCommon.getQueryStringParams('page');
            if(currentPage != undefined && currentPage.length > 0){
                hbObject.PagesToSkip = currentPage - 1;
            }
        }

        $.ajax({
            contentType: 'application/json; charset=utf-8',
            url: hbObject.Endpoint,
            data: JSON.stringify({ 'parameters': hbObject }),
            cache: false,
            async: false,
            dataType: 'json',
            type: "POST",
            success: function (response) {
                if (response != undefined) {
                    if (owCommon.isValidJson(response)) {
                        populate(hbObject, response);
                    }
                }
            },
            error: function (xhr) {
                console.log(xhr);
            }
        });
    }

    var populate = function (hbObject, handleBarData) {
        if (hbObject.SourceId != undefined) {
            var identifier = hbObject.SourceId;
            var parentDiv = $('[ow-source-id=' + identifier + ']');
            if (parentDiv == undefined) {
                console.log('parent div not found for ' + identifier);
                return;
            }
            if (handleBarData != undefined) {
                parentDiv.removeClass('d-none');
                var templateDiv = parentDiv.find('.ow-template');
                var containerDiv = parentDiv.find('.ow-container');

                if (templateDiv == undefined || containerDiv == undefined) {
                    console.log('template/container not found for ' + identifier);
                    return;
                }

                var content = '';
                var hbTemplate = '';
                try {
                    hbTemplate = Handlebars.compile(templateDiv.html());
                    content = hbTemplate(handleBarData);
                }
                catch (e) {
                    console.log('error parsing handlebar for ' + identifier);
                    console.log(e);
                    return;
                }

                if (hbObject.Append == true) {
                    containerDiv.append(content);
                } else {
                    if($(containerDiv).hasClass('slider-widget')){
                        owSliderCard.destory();
                        containerDiv.html(content);
                        owSliderCard.init();
                    }else{
                        containerDiv.html(content);
                    }
                }

                if (parentDiv.attr('is-result') != undefined) {
                    var searchObject = {
                        Wrapper: parentDiv,
                        Data: handleBarData,
                        Append: hbObject.Append,
                        Keyword: hbObject.Keyword,
                        Filters: hbObject.Filters
                    }
                    owSearch.renderResults(searchObject);
                }

                if (parentDiv.attr('is-filter') != undefined && parentDiv.attr('is-alphabet-filter') == undefined) {
                    updateFilterVisibility(parentDiv, handleBarData);
                }
            } else {
                parentDiv.addClass('d-none');
            }
        }
    };

    var updateFilterVisibility = function (parentDiv, data) {
        if (data.Values == undefined || data.Values.length == 0) {
            parentDiv.addClass('d-none');
        } else {
            parentDiv.removeClass('d-none');
        }
        var filterWrappers = parentDiv.find('.filter-facet-wrapper .filter-item');
        if (filterWrappers.length == 0) {
            parentDiv.find('.filter-facet-wrapper').addClass('d-none');
        } else {
            parentDiv.find('.filter-facet-wrapper').removeClass('d-none');
            var filterWrapperContainer = parentDiv.parents('.filter-wrapper-container');
            if(filterWrapperContainer != undefined){
                filterWrapperContainer.removeClass('d-none');
                filterWrapperContainer.find('.filter-tab').removeClass('d-none')
            }
        };
    }
    return {
        init: initiate,
        populate: populate,
        fetch: fetch,
    };
})();

$().ready(function () {
    owHandlebar.init();
});