var owPagination = (function () {
    
    const initiate = function () {
        const paginations = $('.ow-pagination').not('.ow-initialized');
        if (paginations) {
            [].map.call(paginations, function (item) {
                var configId = $(item).attr("ow-config-id");
                if(configId != undefined && configId.length > 0){
                    var searchResultsContainer = $('[is-result][ow-config-id=' + configId +']');
                    if(searchResultsContainer != undefined && searchResultsContainer.length > 0){
                        $(searchResultsContainer).find(".ow-load-more").remove();
                        var parametersJson = $.parseJSON(searchResultsContainer.attr("ow-parameters"));
                        if(parametersJson != undefined){
                            var totalPages = parametersJson.TotalPageCount;
                            if(totalPages == 0){
                                $(item).find('.ow-container').pagination('destroy');
                            }else{
                                $(item).find('.ow-container').pagination({
                                    pages: totalPages,
                                    currentPage: parametersJson.IteratedPageCount,
                                    displayedPages: $(item).attr('ow-displayed-pages'),
                                    edges: $(item).attr('ow-visible-edges'),
                                    cssStyle: '',
                                    hrefTextPrefix: owCommon.getUrlWithoutParam('page')+"?page=",
                                    prevText: $(item).attr('ow-previous-text'),
                                    nextText: $(item).attr('ow-next-text'),
                                    onInit: function () {

                                    },
                                    onPageClick: function (page, evt) {
                                        if(evt != undefined){
                                            evt.preventDefault();
                                        }
                                        
                                        owCommon.insertUrlParam('page', page);
                                        parametersJson.IteratedPageCount = page -1 ;
                                        $(searchResultsContainer).attr("ow-parameters",JSON.stringify(parametersJson));
                                        owSearch.triggerPagination(searchResultsContainer); 
                                        var offsetEle = searchResultsContainer.offset().top
                                        setTimeout(function(){
                                            $("html, body").animate({ scrollTop: (offsetEle - ($('.ow-primary-header').height() + $('.ow-secondary-header').height() + 80)) });
                                        })
                                    }
                                });
                            }

                            
                        }
                    }
                }
            });
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
        };
    }
    return {
        init: initiate,
    };
})();



// $().ready(function () {
//     owPagination.init();
// });
