function stickeyElement() {
    let page_nav_height = "";

    if ($('.Page-navigation').length) {
        page_nav_height = $('.Page-navigation').outerHeight() + $('.ow-primary-header').outerHeight()
    }
    else {
        page_nav_height = $('.ow-primary-header').outerHeight()
    }
    $('.stickeyElement').each(function (key, target) {
        let elementPosition = $(target).find('.comparison-wrapper__card-header-row').offset()

        let headerHeight = $(target).find('.comparison-wrapper__card-header-row .card-wrapper').outerHeight();
        $(target).find('.comparison-table .card-header').each(function () {
            $(this).css('padding-top', headerHeight + "px")
        })


        var stickyOffset = elementPosition.top;

        $(window).scroll(function(){
          var sticky = $(target).find('.comparison-wrapper__card-header-row'),
              scroll = $(window).scrollTop();
              $('body').addClass('overflowX-hidden')
            
          if (scroll >= stickyOffset) sticky.addClass('element-fix-position').removeClass('space-between').css('top', (page_nav_height - 1) + 'px');
          else sticky.removeClass('element-fix-position').addClass('space-between');
          $('body').removeClass('overflowX-hidden')
        });

    })

}
$(document).ready(function () {
    stickeyElement()
})
$(window).load(function () {
    stickeyElement()
})
