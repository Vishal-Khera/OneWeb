var businessCard = (function () {
    var init = function () {
        _businessToggle();
        _gridView();
        _listView();
        _filterExpand();
    },
    
    _businessToggle = function () {
        $(document).on('click', '.business-detail-toggle', function (e) {
            $(this).parents('.ow-business').toggleClass('expend');
            $(this).parents('.ow-business').find('.business-details').slideToggle();
        });
    }
    
    _gridView = function () {
        let cardView = $('.card-view');
        if(cardView){
            [].map.call(cardView, function(item){
                $(item).on('click', function(e){
                    $('.filter-result').addClass('gridView');
                    $('.list-grid-view .card-view').addClass('active')
                    $('.list-grid-view .list-view').removeClass('active')
                })
            })
        }
    }
    _listView = function () {
        let listView = $('.list-view');
        if(listView){
            [].map.call(listView, function(item){
                $(item).on('click', function(e){
                    $('.filter-result').removeClass('gridView')
                    $('.list-grid-view .card-view').removeClass('active')
                    $('.list-grid-view .list-view').addClass('active')
                })
            })
        }
    }

    _filterExpand = function () {
        let filter = $('.filter-mobile');
        if(filter){
            [].map.call(filter, function(item){
                $(item).on('click', function(e){
                    $(item).toggleClass('active');
                    $('.filter-wrapper').slideToggle();
                })
            })
        }
    }
    return {
        init: init
    };
})();

$().ready(function () {
    businessCard.init();
});