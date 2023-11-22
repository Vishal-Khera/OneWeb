var owSearch = (function () {
    var loadmore = false;
    var clearText = false;
    var PaginationType = {
        None: 0,
        LazyLoad: 1,
        Pagination: 2,
    };

    function triggerNormalSearch(searchBoxElem) {
        var searchObject = {
            Wrapper: getResultWrapper(searchBoxElem),
            PageType: PaginationType.None,
        };
        if (searchObject.Wrapper != undefined && searchObject.Wrapper.length > 0) {
            startSearch(searchObject, false);
        } else {
            var autoSuggestContext = getAutoSuggestContext(searchBoxElem, false);
            if (clearText == false) {
                window.location.href = autoSuggestContext.SearchPage + "?keyword=" + autoSuggestContext.Keyword;
            }
            clearText = false;

        }
    }

    function ssResetFilters(configurationId) {
        $('[ow-config-id=' + configurationId + '][is-filter]').each(function () {
            var filterGroup = $(this).find('.filter-facet-wrapper').not(".ow-template .filter-facet-wrapper").parents('.ow-search-template').not('[is-advanced-filter]');
            if (filterGroup != undefined && filterGroup.length > 0) {
                if ($(this).attr('is-alphabet-filter') != undefined) {
                    var activeFilter = $(this).find('.isActive').removeClass('isActive');
                } else if ($(this).attr('is-link-filter') != undefined) {
                    var activeFilter = $(this).find('.isActive');
                    if (activeFilter.length > 0) {
                        $(this).find('.isActive').removeClass('isActive');
                    }
                }
                else {
                    $.map(filterGroup.find('input:checked'), function (filter) {
                        $(filter).prop('checked', '');
                    });

                    $.map(filterGroup.find('a.isActive'), function (filter) {
                        if ($(filter).attr('value') != undefined && $(filter).attr('value').length > 0) {
                            $(filter).removeClass('isActive');
                        }
                    });
                }
            }

            var filterGroup = $(this).find('.filter-facet-wrapper').not(".ow-template .filter-facet-wrapper").parents('.ow-search-template[is-advanced-filter]')
            if (filterGroup != undefined && filterGroup.length > 0) {
                var primaryFilterObject = {
                    Key: filterGroup.find('.filter-facet-head').attr('ow-key'),
                    Values: []
                };

                $.map(filterGroup.find('input:checked').not('.sub-item'), function (filter) {
                    $(filter).prop('checked', '');
                });

                $.map(filterGroup.find('input.sub-item:checked'), function (filter) {
                    $(filter).prop('checked', '');
                });
            }
        });
    }

    function clearAll() {
        owCommon.removeUrlParam('filters');
        var parentDiv = $('.clearAll-filter').parents('.component-content').find('[ow-config-id]')[0];
        ssResetFilters($(parentDiv).attr('ow-config-id'));
        var searchObject = {
            Wrapper: getResultWrapper(parentDiv),
            PageType: PaginationType.None,
        };
        if (searchObject.Wrapper != undefined && searchObject.Wrapper.length > 0) {
            startSearch(searchObject, false);
        }
    }

    function triggerAutoSuggestSearch(inputElem, searchBox) {
        var searchObject = {
            PageType: PaginationType.None,
            Endpoint: '/oneweb/search/getautosuggestresults',
            Keyword: inputElem.val(),
            SourceId: $(searchBox).attr('ow-as-config-id'),
            Container: $(searchBox).find('.ow-container'),
            Language: $(searchBox).attr('ow-language'),
            PageType: PaginationType.None
        }
        startSearch(searchObject, true);
    }

    function setPlaceholderText(elem, searchBox) {
        $(elem).attr('placeholder-bak', $(elem).attr('placeholder'));
        $(elem).attr('placeholder', searchBox.attr('ow-empty-text-message'));
        $(elem).addClass('empty-message');
    }

    function resetPlaceholderText(elem) {
        $(elem).removeClass('empty-message');
        $(elem).attr('placeholder', $(elem).attr('placeholder-bak'));
    }

    var bindOneTimeTriggers = function () {
        $(document).on('click', '.ow-search-box .view-all', function (e) {
            var autoSuggestContext = getAutoSuggestContext($(this), true);
            window.location.href = autoSuggestContext.SearchPage + "?keyword=" + autoSuggestContext.Keyword + "&filters=" + autoSuggestContext.GroupId + ":" + autoSuggestContext.Id;
        });

        $(document).on('click', function (e) {
            if (!$(e.target).hasClass('search-box-input')) {
                $('.ow-search-box').find('.ow-container').html('');
                $('.ow-search-box').find('.popular-keywords').addClass('d-none');
            }
        });
        $(document).on('blur', '.ow-search-box[ow-config-id] input', function (event) {
            resetPlaceholderText($(this));
        });

        $(document).on('click', '.ow-search-box[ow-config-id] input', function (event) {
            var searchBox = $(this).parents('.ow-search-box');
            $(this).parents('.ow-search-box').find('.ow-container').html('');
            var autoSuggestTrigger = parseInt($(searchBox).attr('ow-as-count'));
            if (autoSuggestTrigger == undefined) {
                autoSuggestTrigger = 3;
            }
            if ($(this).val().length >= autoSuggestTrigger) {
                triggerAutoSuggestSearch($(this), searchBox);
                searchBox.find('.popular-keywords').addClass('d-none');
            } else {
                searchBox.find('.popular-keywords').removeClass('d-none');
            }
        });


        //load more
        $(document).on('click keyup', '.ow-load-more', function () {
            loadmore = true;
            var searchObject = {
                Wrapper: getResultWrapper(this),
                PageType: PaginationType.LazyLoad
            };
            startSearch(searchObject, false);
            loadmore = false;
        });

        $(document).on('click', '.ow-search-box[ow-config-id] .icon-search', function () {
            resetPlaceholderText($(this))
            $(this).removeClass('empty-message');
            var searchBox = $(this).parents('.ow-search-box').find('input');
            if (searchBox.val().length > 0 || $(this).parents('.ow-search-box').attr('ow-results-by-default') == 'True') {
                triggerNormalSearch(searchBox);
            } else if ($(this).val().length == 0) {
                setPlaceholderText(searchBox, $(this).parents('.ow-search-box'));
                triggerNormalSearch(searchBox);
            }
        });

        $(document).on('keyup', '.ow-search-box[ow-config-id] input', function (event) {
            if (!(event.keyCode === 38 || event.keyCode === 40)) {
                resetPlaceholderText($(this))
                var searchBox = $(this).parents('.ow-search-box');
                $(searchBox).find('.ow-container').html('');
                if (event.keyCode === 13) {
                    if ($(this).val().length > 0 || searchBox.attr('ow-results-by-default') == 'True') {
                        triggerNormalSearch($(this));
                    } else if ($(this).val().length == 0) {
                        setPlaceholderText($(this), searchBox)
                    }
                } else {
                    if ($(searchBox).attr('ow-as-config-id') != undefined) {
                        var autoSuggestTrigger = parseInt($(searchBox).attr('ow-as-count'));
                        if (autoSuggestTrigger == undefined) {
                            autoSuggestTrigger = 3;
                        }
                        if ($(this).val().length >= autoSuggestTrigger) {
                            triggerAutoSuggestSearch($(this), searchBox);
                        }
                        if ($(this).val().length > 0) {
                            $(this).parents('.ow-search-box').find('.clear-text').addClass('d-block');
                        }
                        if ($(this).val().length == 0) {
                            $(this).parents('.ow-search-box').find('.clear-text').removeClass('d-block');
                        }

                    }
                }
            }
        });

        $(document).on('click', '.filter-facet-wrapper .uncheck-all', function (event) {
            var parentContainer = $(this).parents('[ow-config-id]');
            var checkedBoxes = $(parentContainer).find('.filter-item input:checked');
            if (checkedBoxes.length == 0) {
                $(this).addClass('disabled');
            } else {
                $(this).removeClass('disabled');
            }
            $(parentContainer).find('.filter-item input').prop('checked', '');
            if ($(this).parents('.tab-wrapper') != undefined) {
                $(this).parents('.tab-wrapper').find('a').removeClass('isActive');
                $(this).addClass('isActive');
            } else {
                $(parentContainer).find('.isActive').removeClass('isActive');
            }
            var searchObject = {
                Wrapper: getResultWrapper(this),
                PageType: PaginationType.None
            }
            startSearch(searchObject, false);
        });

        $(document).on('click', '.filter-facet-wrapper .filter-item input, .filter-facet-wrapper .alphabetic-facet a, .filter-facet-wrapper .filter-item a, .tab-wrapper a', function (e) {
            if ($(this).parents('alphabetic-facet') != undefined) {
                e.preventDefault();
                if ($(this).hasClass('disabled')) {
                    e.stopPropagation();
                    return;
                }
                $(this).addClass('isActive');
            }
            if ($(this).parents('.tab-wrapper') != undefined) {
                $(this).parents('.tab-wrapper').find('a').removeClass('isActive');
                $(this).addClass('isActive');
            }

            var searchObject = {
                Wrapper: getResultWrapper(this),
                PageType: PaginationType.None
            }
            startSearch(searchObject, false);
        });

        $(document).on('click', '.filter-search-wrapper a.fa-times-circle', function (e) {
            e.preventDefault();
            e.stopPropagation();
            var searchObject = {
                Wrapper: getResultWrapper($(this)),
                PageType: PaginationType.None,
            }
            $(this).parents('label').remove();
            startSearch(searchObject, false);
        });

        function callEvent(key) {
            $(key).parents('.ow-search-box').find('input').val('');
            $(key).parents('.ow-search-box').find('input').focus();
            $(key).parents('.ow-search-box').find('.global-search').addClass('focusIn');
            $(key).removeClass('d-block');
            if ($(window).width() > 992) {
                var eleWidth = $(key).parents('.ow-search-box').innerWidth();
                var extendedWith = $(key).parents('.mobile-menu').next().width();
                if ($(window).width() > 1200) {
                    $(key).parents('.global-search').width(eleWidth + extendedWith + 50);
                }
                else {
                    $(key).parents('.global-search').width(eleWidth + extendedWith + 30);
                }
            }
        }

        $(document).on('click', '.clear-text', function (e) {
            clearText = true;
            callEvent(this)

        });

        $(document).on('click', '.clearAll-filter', function (e) {
            clearAll();
        });

        $(document).on('click', '.suggestion-label, .taxonomy', function (e) {
            if ($(this).hasClass('attached')) {
                return;
            }
            e.preventDefault();
            if ($('.filter-search-wrapper').find('label[tag-id=' + $(this).attr('tag-id') + ']').length == 0) {
                $('.filter-search-wrapper').append('<label class="suggestion-label tag" tag-id=' + $(this).attr('tag-id') + '>' + $(this).html() + '<a href="javascript:void(0)" class="far fa-times-circle" aria-hidden="true"></a></label>');
                var searchObject = {
                    Wrapper: getResultWrapper(this),
                    PageType: PaginationType.None
                }
                startSearch(searchObject, false);
            }
        });
    }

    const triggerPagination = function (searchWrapper) {
        loadmore = true;
        var searchObject = {
            Wrapper: getResultWrapper(searchWrapper),
            PageType: PaginationType.Pagination,
        };
        startSearch(searchObject, false);
        loadmore = false;
    }

    const renderResults = function (inputObj) {
        if (inputObj != undefined) {
            $(inputObj.Wrapper).attr('ow-parameters', JSON.stringify(inputObj.Data.Parameters));
            var configId = $(inputObj.Wrapper).attr('ow-config-id');
            $('.ow-search-box[ow-config-id=' + configId + '] input').not('.global-header input').val(inputObj.Keyword);
            var searchContext = getContext(inputObj.Wrapper, PaginationType.None, true);
            var rrAppendUrlParameters = function (searchObj) {
                var currentUrl = window.location.href;
                var url = new URL(currentUrl);
                if (searchObj.Keyword != undefined && searchObj.Keyword.length > 0) {
                    url.searchParams.set("keyword", encodeURIComponent(searchObj.Keyword));
                } else {
                    url.searchParams.delete("keyword");
                }
                if (searchObj.Filters != undefined && searchObj.Filters.length > 0) {
                    var filterList = [];
                    searchObj.Filters.forEach(filter => {
                        filterList.push(filter.Key + ":" + filter.Values.join('|'))
                    })
                    url.searchParams.set("filters", filterList.join(';'));
                } else {
                    url.searchParams.delete("filters");
                }
                window.history.pushState('search', searchObj.Keyword, decodeURIComponent(url.href));
            }
            var rrRenderFilters = function (data) {
                var allFacets = data.Facets;
                var configId =  data.Parameters.CurrentItemId;
                data.Facets.forEach(facet => {
                    var facetElem = $('[ow-config-id='+configId+'][ow-source-id=' + facet.Identifier + ']');
                    if (facetElem != undefined && facetElem.length > 0) {
                        if (facetElem.attr('is-alphabet-filter') != undefined) {
                            var allElems = facetElem.find('.alphabetic-facet a');
                            allElems.each(function () {
                                $(this).parent().addClass('disabled');
                                $(this).parent().removeClass('active');
                                $(this).attr('tabIndex', '-1');
                            });

                            facet.Values.forEach(facetValue => {
                                var facetValueElem = facetElem.find('.alphabetic-facet a[value="' + facetValue.Name.toUpperCase().split('')[0] + '"]');
                                if (facetValueElem != undefined) {
                                    facetValueElem.parent().removeClass('disabled');
                                    facetValueElem.attr('tabIndex', '0');
                                }
                            });
                        } else {
                            if (facetElem.attr('is-static-filter') == undefined) {
                                //handling case of advanced filters
                                var attr = $(facetElem).attr('is-advanced-filter');
                                if (typeof attr !== 'undefined' && attr !== false) {
                                    $.ajax({
                                        contentType: 'application/json; charset=utf-8',
                                        url: $(facetElem).attr('ow-mapping-api'),
                                        cache: false,
                                        async: false,
                                        dataType: 'json',
                                        type: "POST",
                                        success: function (response) {
                                            if (response != undefined) {
                                                if (owCommon.isValidJson(response)) {
                                                    var primaryFacet = facetElem.attr('ow-primary-facet');
                                                    var secondaryFacet = facetElem.attr('ow-secondary-facet');
                                                    var model = {};
                                                    model.Values = [];
                                                    model.PrimaryFacet = allFacets.find((o) => { return o["Key"] === primaryFacet });;
                                                    model.SecondaryFacet = allFacets.find((o) => { return o["Key"] === secondaryFacet });
                                                    function GetSecondaryFacetsFromMapping(mapping) {
                                                        var viableMappings = mapping.SecondaryFilter.map(function (item) { return (item.Title); });
                                                        return model.SecondaryFacet.Values.filter(x => viableMappings.indexOf(x.Name) != -1)
                                                    }
                                                    $.each(response.Data, function (key, value) {
                                                        //var currentFacet = facetElem.find("#" + value.PrimaryFilter.Title.replace(new RegExp(" ", "g"), '\\ '));                                                                                                        
                                                        var facetModel = {}
                                                        var facetValues = model.PrimaryFacet.Values.filter(x => x.Name == value.PrimaryFilter.Title)[0];
                                                        if (facetValues != undefined && facetValues.Count > 0) {
                                                            facetModel.Primary = facetValues
                                                            facetModel.Secondary = GetSecondaryFacetsFromMapping(value, model.SecondaryFacet);
                                                            model.Values.push(facetModel);
                                                        }
                                                    });

                                                    var hbObject = {
                                                        SourceId: facetElem.attr('ow-source-id'),
                                                    }
                                                    owHandlebar.populate(hbObject, model);
                                                }
                                            }
                                        },
                                        error: function (xhr) {
                                            console.log(xhr);
                                        }
                                    });
                                } else {
                                    var hbObject = {
                                        SourceId: facetElem.attr('ow-source-id'),
                                    }
                                    facet.Values = facet.Values.sort((a, b) => (a.Name > b.Name) ? 1 : ((b.Name > a.Name) ? -1 : 0))
                                    owHandlebar.populate(hbObject, facet);
                                }

                            }
                        }
                    }
                });

                var filters = owCommon.getFilters();
                filters.forEach(filter => {
                    var filterElem = $('[ow-config-id='+configId+'] [ow-key=' + filter.Key + ']');
                    if (filterElem == undefined || filterElem.length == 0) {
                        filterElem = $('[ow-config-id='+configId+'] [ow-secondary-key=' + filter.Key + ']');
                    }
                    if (filterElem != undefined) {
                        filter.Values.forEach(filterValue => {
                            var filterValueElem = filterElem.parent().find('[id="' + filterValue + '"]');
                            if (filterValueElem != undefined && filterValueElem.length > 0) {
                                filterValueElem.prop('checked', 'checked');
                            } else {
                                filterValueElem = filterElem.parent().find('[value="' + filterValue + '"]');
                                if (filterValueElem != undefined) {
                                    filterValueElem.parent().removeClass('disabled');
                                    filterValueElem.parent().addClass('active');
                                    filterValueElem.attr('tabIndex', '0');

                                }
                            }
                        });
                    }
                });
            }
            var rrUpdateStatistics = function (searchObj) {
                if (searchObj.Parameters.SearchTerm != undefined && searchObj.Parameters.SearchTerm.length > 0) {
                    searchObj.SearchBox.find('.filter-facets .search-value').removeClass('d-none');
                    searchObj.SearchBox.find('.filter-facets .result-text').removeClass('d-none');
                } else {
                    searchObj.SearchBox.find('.filter-facets .search-value').addClass('d-none');
                    searchObj.SearchBox.find('.filter-facets .result-text').addClass('d-none');
                }
                var searchValue = searchObj.SearchBox.find('span.search-value');
                if (searchObj.Keyword != undefined && searchObj.Keyword.length > 0) {
                    if ($(searchValue).hasClass('wrap-double-quotes')) {
                        searchValue.text("\"" + searchObj.Keyword + "\"");
                    } else {
                        searchValue.text(searchObj.Keyword);
                    }
                }

                searchObj.SearchBox.find('span.search-current-count').text(searchObj.Parameters.IteratedResultCount);
                searchObj.SearchBox.find('span.search-total-count').text(searchObj.Parameters.TotalResultCount);
            }
            var rrUpdateVisibility = function (searchObj) {
                if (searchObj.Parameters.RemainingPageCount == 0) {
                    $(searchObj.LoadMore).addClass('d-none');
                } else {
                    $(searchObj.LoadMore).removeClass('d-none');
                }

                if ($('.ow-search-box .filter-search-wrapper .search-box-input').length) {
                    if (searchObj != undefined && searchObj.Parameters != undefined && searchObj.Parameters.CurrentResultCount > 0) {
                        $('[ow-config-id=' + searchObj.ConfigurationId + '][is-result]').removeClass('d-none');
                    } else {
                        $('[ow-config-id=' + searchObj.ConfigurationId + '][is-result]').addClass('d-none');
                    }
                }

                if (searchObj.Keyword != undefined && searchObj.Keyword.length > 0) {
                    $('.result-text').not('.no-keyword').removeClass('d-none');
                    $('.result-text.no-keyword').addClass('d-none')
                } else {
                    $('.result-text').not('.no-keyword').addClass('d-none');
                    $('.result-text.no-keyword').removeClass('d-none')
                }
            }
            var rrSetPostSearchFilters = function (searchObj) {
                var enabledFilters = $(searchObj.Wrapper).attr('ow-filters')
                if (enabledFilters != undefined && enabledFilters.length > 0) {
                    enabledFilters = JSON.parse(enabledFilters);
                    if (enabledFilters != undefined) {
                        enabledFilters.forEach(filterGroup => {
                            if (filterGroup.Key != 'tags_sm') {
                                var currentFilter = $('[ow-config-id=' + searchObj.ConfigurationId + '][is-filter]').find('.filter-facet-wrapper .filter-facet-head[ow-key=' + filterGroup.Key + ']');
                                $.map(filterGroup.Values, function (filter) {
                                    $(currentFilter).parents('.filter-facet-wrapper').find('[id="' + filter + '"]').prop('checked', true);
                                });

                                //advanced filters
                                var advancedFilter = $('[ow-config-id=' + searchObj.ConfigurationId + '][is-advanced-filter]').find('.filter-facet-wrapper .filter-facet-head[ow-secondary-key=' + filterGroup.Key + ']');
                                $.map(filterGroup.Values, function (filter) {
                                    var secondaryFilter = $(advancedFilter).parents('.filter-facet-wrapper').find('[id="' + filter + '"]');
                                    secondaryFilter.prop('checked', true);
                                    var primaryFilter = secondaryFilter.parents('.filter-item').find('input').eq(0);
                                    primaryFilter.prop('checked', true);
                                });
                            }
                        });
                    }
                }

                var secondaryFilters = $('.filter-facet-items-secondary');

                if (secondaryFilters.length > 0) {
                    secondaryFilters.each(function () {
                        var primaryFilter = $(this).parents('.filter-item').find('input').eq(0);
                        if (primaryFilter.prop('checked')) {
                            $(this).removeClass('d-none');
                        } else {
                            $(this).addClass('d-none');
                        }
                    });
                }
            }


            rrAppendUrlParameters(inputObj);
            rrRenderFilters(inputObj.Data);
            rrUpdateStatistics(searchContext);
            rrUpdateVisibility(searchContext);
            rrSetPostSearchFilters(searchContext);
            if (loadmore == false) {
                if ($('.ow-search-box .filter-search-wrapper .search-box-input').length && $('.ow-search-box .filter-search-wrapper .search-box-input').val() != '') {
                    $([document.documentElement, document.body]).animate({
                        scrollTop: $('.ow-search-box[ow-config-id="' + searchContext.ConfigurationId + '"]').offset().top - $('#header').height() - 50
                    }, 500);
                }
                loadmore = false;
            }

            owLazyLoad.bindLazyLoadTriggers();
            owPagination.init();
            filterint();
            slider.init();
        }
    };

    var startSearch = function (sObject, isAutoSuggest = false) {
        if (sObject != undefined) {
            if (isAutoSuggest) {
                ssAutoSuggestSearch(sObject);
            } else {
                isNormalSearch(sObject);
            }
        }

        function isNormalSearch(searchObject) {
            if (searchObject != undefined) {
                var hbObject = getContext(searchObject.Wrapper, searchObject.PageType, false);
                hbObject.Filters = ssFetchFilters(hbObject.ConfigurationId),
                    ssSavePreSearchFilters(searchObject.Wrapper, hbObject);
                owHandlebar.fetch(hbObject);
            }
        }
        function ssAutoSuggestSearch(searchObject) {
            $.ajax({
                contentType: 'application/json; charset=utf-8',
                url: searchObject.Endpoint,
                data: JSON.stringify({ 'parameters': searchObject }),
                cache: false,
                dataType: 'json',
                type: "POST",
                success: function (response) {
                    if (response != undefined && response.Groups != undefined && response.Groups.length > 0) {
                        response.Groups.map(function (group) {
                            if (group.Results != undefined && group.Results.length > 0) {
                                var parentDiv = $(searchObject.Container).parents('[ow-config-id]');
                                var autoSuggestTemplate = parentDiv.find('.ow-template');
                                if (autoSuggestTemplate != undefined && autoSuggestTemplate.length > 0) {
                                    var hbTemplate = Handlebars.compile(autoSuggestTemplate.html());
                                    content = hbTemplate(response);
                                    var autoSuggestContainer = parentDiv.find('.ow-container');
                                    if (autoSuggestContainer != undefined && autoSuggestContainer.length > 0) {
                                        autoSuggestContainer.html(content);
                                    }
                                }
                            }
                        })
                    }
                },
                error: function (xhr) {
                    console.log(xhr);
                },
            });
        }
        function ssSavePreSearchFilters(wrapper, searchObj) {
            $(wrapper).attr('ow-filters', JSON.stringify(searchObj.Filters));
        }

        function ssFetchFilters(configurationId) {
            var activeFilters = []
            $('[ow-config-id=' + configurationId + '][is-filter]').each(function () {
                var filterGroup = $(this).find('.filter-facet-wrapper').not(".ow-template .filter-facet-wrapper").parents('.ow-search-template').not('[is-advanced-filter]');
                if (filterGroup != undefined && filterGroup.length > 0) {
                    var filterObject = {
                        Key: filterGroup.find('.filter-facet-head').attr('ow-key'),
                        Values: []
                    };
                    if ($(this).attr('is-alphabet-filter') != undefined) {
                        var activeFilter = $(this).find('.isActive');
                        if (activeFilter.length > 0) {
                            filterObject.Values.push(activeFilter.attr('value').toLowerCase() + "*");
                        }
                    } else if ($(this).attr('is-link-filter') != undefined) {
                        var activeFilter = $(this).find('.isActive').not('.disabled');
                        if (activeFilter.length > 0) {
                            filterObject.Values.push(activeFilter.attr('id'));
                            filterGroup.find('.show-filter').not('.uncheck-all').addClass('d-none');
                        } else {
                            filterGroup.find('.show-filter').not('.uncheck-all').removeClass('d-none');
                            filterGroup.find('.filters-card__items').removeClass('expand-filter');
                        }
                    }
                    else {
                        $.map(filterGroup.find('input:checked'), function (filter) {
                            filterObject.Values.push($(filter).attr('id'));
                        });

                        $.map(filterGroup.find('a.isActive').not('.disabled'), function (filter) {
                            if ($(filter).attr('value') != undefined && $(filter).attr('value').length > 0) {
                                filterObject.Values.push($(filter).attr('value'));
                                $(filter).removeClass('isActive');
                            } else {
                                filterObject.Values.push($(filter).attr('id'));
                                $(filter).removeClass('isActive');
                            }
                        });
                    }

                    if (filterObject.Values.length > 0) {
                        activeFilters.push(filterObject);
                    }
                }

                var filterGroup = $(this).find('.filter-facet-wrapper').not(".ow-template .filter-facet-wrapper").parents('.ow-search-template[is-advanced-filter]')
                if (filterGroup != undefined && filterGroup.length > 0) {
                    var primaryFilterObject = {
                        Key: filterGroup.find('.filter-facet-head').attr('ow-key'),
                        Values: []
                    };

                    $.map(filterGroup.find('input:checked').not('.sub-item'), function (filter) {
                        primaryFilterObject.Values.push($(filter).attr('id'));
                    });

                    if (primaryFilterObject.Values.length > 0) {
                        activeFilters.push(primaryFilterObject);
                    }

                    var secondaryFilterObject = {
                        Key: filterGroup.find('.filter-facet-head').attr('ow-secondary-key'),
                        Values: []
                    };

                    $.map(filterGroup.find('input.sub-item:checked'), function (filter) {
                        secondaryFilterObject.Values.push($(filter).attr('id'));
                    });

                    if (secondaryFilterObject.Values.length > 0) {
                        activeFilters.push(secondaryFilterObject);
                    }
                }
            });

            var activeTagElems = $('.filter-search-wrapper').find('.suggestion-label');
            if (activeTagElems != undefined && activeTagElems.length > 0) {
                var tagObject = {
                    Key: 'ow_taglist_sm',
                    Values: []
                };

                $.map(activeTagElems, function (tagElem) {
                    tagObject.Values.push($(tagElem).attr('tag-id'));
                });

                if (tagObject.Values.length > 0) {
                    activeFilters.push(tagObject);
                }
            }

            return activeFilters;
        }

    }

    function getResultWrapper(elem) {
        if (elem != undefined) {
            var parentDiv = $(elem).closest('[ow-config-id]');
            if (parentDiv == undefined || parentDiv.length == 0) {
                parentDiv = $(elem).parents('[ow-config-id]');
            }
            if (parentDiv != undefined) {
                var configurationId = parentDiv.attr('ow-config-id');
                if (configurationId != undefined) {
                    return $('[ow-config-id=' + configurationId + '][is-result]');
                }
            }
        }
    }
    var getAutoSuggestContext = function (elem, isViewAll) {
        if (isViewAll) {
            var asGroupContainer = $(elem).parents('[ow-as-group-identifier]');
            var searchBox = $(elem).parents('.ow-search-box');
            if (asGroupContainer != undefined && asGroupContainer.length > 0) {
                var groupId = asGroupContainer.attr('ow-as-group-identifier');
                var id = asGroupContainer.attr('ow-as-identifier');
                var searchPageUrl = searchBox.attr('ow-as-result-page');
                var keyword = searchBox.find('input').val();
                return context = {
                    SearchPage: searchPageUrl,
                    Keyword: keyword,
                    Id: id,
                    GroupId: groupId,
                }
            }
        } else {
            var parentElem = elem.parents('.ow-search-box');
            var searchPageUrl = parentElem.attr('ow-as-result-page');
            var keyword = parentElem.find('input').val();
            return context = {
                SearchPage: searchPageUrl,
                Keyword: keyword,
            }
        }
    };

    var getContext = function (elem, paginationType, includeWrapper = false) {
        var resultWrapper = getResultWrapper(elem);
        var configurationId = resultWrapper.attr('ow-config-id');
        var searchBox = $('.ow-search-box[ow-config-id=' + configurationId + ']');
        var searchInput = searchBox.find('input').val();
        if (paginationType == PaginationType.Pagination) {
            parameterJson = resultWrapper.attr('ow-parameters');
            searchInput = JSON.parse(parameterJson).SearchTerm;
        }
        var contextObject = {
            Endpoint: resultWrapper.attr('ow-endpoint'),
            SourceId: resultWrapper.attr('ow-source-id'),
            ConfigurationId: configurationId,
            Keyword: searchInput,
            PageSize: resultWrapper.attr('ow-page-size'),
            PagesToSkip: 0,
            PageType: paginationType,
            Append: paginationType === PaginationType.LazyLoad,
            Language: searchBox.attr('ow-language'),
        };
        if ($(resultWrapper).attr('ow-parameters') != undefined && paginationType != PaginationType.None) {
            var parameters = JSON.parse($(resultWrapper).attr('ow-parameters'));
            contextObject.PagesToSkip = parameters.IteratedPageCount;
        }
        if (paginationType == PaginationType.None) {
            contextObject.PagesToSkip = 0;
        }

        if (includeWrapper) {
            var resultContainer = resultWrapper.find('.ow-container');
            var resultTemplate = resultWrapper.find('.ow-template');
            var loadMoreButton = resultWrapper.find('.ow-load-more');
            if (resultWrapper.attr('ow-parameters') != undefined) {
                contextObject.Parameters = JSON.parse($(resultWrapper).attr('ow-parameters'));
            }
            contextObject.Wrapper = resultWrapper;
            contextObject.Container = resultContainer;
            contextObject.Template = resultTemplate;
            contextObject.LoadMore = loadMoreButton;
            contextObject.SearchBox = searchBox;
        }
        if (searchBox.find('input').length > 0 && searchBox.find('input').val() != '') {
            $('.ow-search-box[ow-config-id=' + configurationId + '] .clear-text').addClass('d-block');
        }

        return contextObject;
    }

    return {
        renderResults: renderResults,
        startSearch: startSearch,
        bindOneTimeTriggers: bindOneTimeTriggers,
        triggerPagination: triggerPagination
    };
})();

$(document).ready(function () {
    owSearch.bindOneTimeTriggers();
})


function filterint() {
    if ($('.filters-card').length > 0) {
        $('.filters-card').each(function () {
            var wrapItems = parseInt($(this).attr('data-list'));
            var listItems = $(this).find('ul li');
            if (listItems.length > wrapItems) {
                $(this).find('ul > li:lt(' + (wrapItems) + ')').wrapAll('<div class="list-default"></div>');
                $(this).find('.filters-card__items ul > li').wrapAll('<div class="list-expanable"></div>')
                $(this).find('.show-filter').show();
            } else {
                $(this).find('.show-filter').hide();
            }

        })
    }

}