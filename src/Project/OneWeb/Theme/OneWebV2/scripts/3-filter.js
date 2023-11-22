var filterFacet = (function () {
    var init = function () {
        _filterExpand();
    },
    
    _filterExpand = function () {
        let mobileFilter = $('.filter-tab:not(.filter-title)');
        if(mobileFilter){
            [].map.call(mobileFilter, function(item){
                $(item).on('click', function (e) {
                    var windowWidth = $(window).width();
                    if(windowWidth < 991){
                        $(this).parent().find('.filter-wrapper-container').addClass('mobile-filter');
                        $('body').append('<div class="menu-shadow"> </div>')
                        $('body').addClass('overflow-hidden')
                    }

                });
            })
        }
        
       
    }
    return {
        init: init
    };
})();

$().ready(function () {
    filterFacet.init();
    if($('.filter-wrapper-container').length){
        $('.filter-wrapper-container .filters-card').eq(0).find('.toggle-filter').click();
    }
    filterint();
});

$(document).on('click','.close-filter', function(){
    $(this).parents('.filter-wrapper-container').removeClass('mobile-filter')
    $('.menu-shadow').remove();
    $('body').removeClass('overflow-hidden')
});

$(document).on('click', function(event){
    var clickover = $(event.target);
    if(clickover.hasClass('menu-shadow')){
        $('.filter-wrapper-container').removeClass('mobile-filter'); 
        $('.menu-shadow').remove();
        $('body').removeClass('overflow-hidden')
    }
})

// function filterint(){
//     $('.filters-card').each(function(){
//         var wrapItems = parseInt($(this).attr('data-list'));
//         var listItems = $(this).find('ul li');
//         if(listItems.length > wrapItems){   
//             $(this).find('ul > li:lt(' + (wrapItems) +')').wrapAll('<div class="list-default"></div>');
//             $(this).find('.filters-card__items ul > li').wrapAll('<div class="list-expanable"></div>')
//             $(this).find('.show-filter').show();
//         }else{
//             $(this).find('.show-filter').hide();
//         }
        
//     })
// }

$(document).on('click', '.toggle-filter', function(){
    $(this).toggleClass('active').parents('.filters-card').find('.filters-card__items').slideToggle();
})

$(document).on('click', '.show-filter, .less-filter', function (e) {
    $(this).parents('.filters-card__items').toggleClass('expand-filter');
    $(this).parents('.filters-card__items').find('.list-expanable').toggle();
});

$(document).on('click', '.list-view', function (e) {
    $(this).addClass('active').siblings().removeClass('active');
    $('.search-container__result').removeClass('grid-view-enable')
});

$(document).on('click', '.grid-view', function (e) {
    $(this).addClass('active').siblings().removeClass('active');
    $('.search-container__result').addClass('grid-view-enable')
});