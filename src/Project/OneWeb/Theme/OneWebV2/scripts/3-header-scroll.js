if($('.ow-primary-header').length){
    $window = $(window);
    var scrollHeader = $('.ow-primary-header').offset().top;
    $(document).ready(function(){
        headerScroll();
        activelink_id()
        $(window).scroll(function(){
            headerScroll();
        })
    })

    $(window).resize(function(){
        headerScroll();
        $(window).scroll(function(){
            headerScroll();
        })
    })
}

function headerScroll(){
    if($('.ow-primary-header').length){
        if($window.scrollTop() >= (scrollHeader + 1)){
            let  headerheight =  $('.ow-primary-header').innerHeight()
            $('.ow-primary-header').addClass('sticky-header');
            $('.ow-primary-header').closest('.container-fluid').addClass('sticky-header-spacing');
            
            $('.Page-navigation').addClass('sticky-navigation');
            $('.Page-navigation').css('top',headerheight + "px");
            let navigationHeight = $('.Page-navigation').innerHeight();
            $('.sticky-navigation').next().css('margin-top', (headerheight + navigationHeight) +'px')
            
            if($(window).width() > 768){
                $('.ow-primary-header').parents('body').addClass('sticky-page');
                $('.Page-navigation').css('top',headerheight + "px")
            }
            else{
                $('.ow-primary-header').parents('body').removeClass('sticky-page');
            }

            if($('.dark-theme-overlay .Page-navigation').hasClass('ow-head-space')){
                $('.Page-navigation').css('top',headerheight + "px");
            }
        }
        else{
            let  headerPosition = $('header').offset()
            let  headerheight_initial =  $('header').innerHeight() + headerPosition.top - $window.scrollTop()
            $('.Page-navigation').css('top',headerheight_initial + "px")
            $('.ow-primary-header').removeClass('sticky-header');
            $('.ow-primary-header').closest('.container-fluid').removeClass('sticky-header-spacing');
            $('.Page-navigation').removeClass('sticky-navigation');
            $('.Page-navigation').next().removeAttr('style')
            $('.ow-primary-header').parents('body').removeClass('sticky-page');
            if($('.dark-theme-overlay .Page-navigation').hasClass('ow-head-space')){
                $('.Page-navigation').css('top', "auto");
            }
        }
    }
    let page_nav_height= $('.Page-navigation').outerHeight()

   
    if($('.breadcrump-overlay').length){
        
        if($('.breadcrump-overlay').parents('main')){
            $('main').addClass('overlay-breadcrump-enable')
        }
    }
    else{
        if($('.Page-navigation').length){
        
            if(!$('.ow-head-space').length){
                //$('main').css('margin-top',page_nav_height+"px")
            }
        }
    }
    
    
 
}

function activelink_id(){
    $('.ow-page-navigation .nav-item a').click(function(){
        $('.navbar-toggler').trigger('click')
        $(this).toggleClass('active-link')
        $(this).parent().siblings().find('a').removeClass('active-link')
        var elementPosition= $('.ow-page-navigation').position()
        $('html, body').animate({
            scrollTop: $( $(this).attr('href') ).offset().top - 156
        }, 500);
        return false;     
    })
}

function footerScroll(){
    var scrollPos = (window.innerHeight + window.scrollY);
    var bodyHeight = $(document).height() - 50
    if( scrollPos >= bodyHeight){
        $('footer').addClass('footer-fullWidth')
    }
    else{
        $('footer').removeClass('footer-fullWidth')
    }
}

function appendNav(){
    if($('a.careers').length){
        var contactusText = $('a.contact-us').parent().html()
        var careersText = $('a.careers').parent().html()
        $('#main_nav').find('ul').append('<li class="nav-item border-0">'+contactusText+'</li><li class="nav-item border-0">'+careersText+'</li>')
        $('#main_nav').find('ul a.contact-us').addClass('nav-link')
        $('#main_nav').find('ul a.careers').addClass('nav-link')
    }
    
}

$(window).on('scroll', function(){
    footerScroll()
});

$(document).ready(function(){
    footerScroll()
    appendNav()
})

$(window).load(function(){
    appendNav()
})