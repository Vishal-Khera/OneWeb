$(window).scroll(function () {
    var scrollPos = $(window).scrollTop();
    if (scrollPos > 0) {
        $('body').addClass('sticky')
    } else {
        $('body').removeClass('sticky')
    }
})


// language js
$('.js-language').on('change', function () {
    var getval = $(this).val();
    window.location.href = getval
})

$(document).ready(function () {
    var date = new Date().getFullYear();
    $('.footer-year').text(date + ' ');

    if($('.js-language .IsActive').length){
        $('.js-language .IsActive').prop('selected', true)
    }

    XA.init();
})


$(window).scroll(function () {
    var scrollPos = $(window).scrollTop();
    if (scrollPos > 0) {
        $('body').addClass('sticky')
    } else {
        $('body').removeClass('sticky')
    }
})
$(document).ready(function () {
    stickyStrip()
    var date = new Date().getFullYear();
    $('.footer-year').text(date + ' ');

    $('body').attr('ontouchstart','')

    if($('#g-recaptcha-response').length){
        let textarea = document.getElementById("g-recaptcha-response");
        textarea.setAttribute("aria-hidden", "true");
        textarea.setAttribute("aria-label", "do not use");
        textarea.setAttribute("aria-readonly", "true");
    }

    DisableCookiepopup()

    if($("[data-mob-src]").length > 0){
        var dataimage = $("[data-mob-src]")
        dataimage.each(function(){
            var dataSrc = $(this).attr('data-src');
            $(this).attr('data-desktop-src', dataSrc);
            if($(this).attr('data-mob-src') == ''){
                $(this).attr('data-mob-src', $(this).attr('data-desktop-src'));
            }

            if($(this).hasClass('ow-lazyloaded')){
                if($(window).width() >= 768){
                    var datasource = $(this).attr('data-desktop-src');
                    $(this).css('background-image', 'url(' + datasource + ')')
                }
                else{
                    var datasource = $(this).attr('data-mob-src');
                    $(this).css('background-image', 'url(' + datasource + ')')
                }
            }
            else{
                if($(window).width() >= 768){
                    $(this).attr('data-src', $(this).attr('data-desktop-src'));
                }
                else{
                    $(this).attr('data-src', $(this).attr('data-mob-src'));
                }
            }
        })
    }
})

$(window).on('resize', function(){
    if($("[data-mob-src]").length > 0){
        var datamob = $("[data-mob-src]")
        datamob.each(function(){
            if($(this).hasClass('ow-lazyloaded')){
                if($(window).width() >= 768){
                    var datasource = $(this).attr('data-desktop-src');
                    $(this).css('background-image', 'url(' + datasource + ')')
                }
                else{
                    var datasource = $(this).attr('data-mob-src');
                    $(this).css('background-image', 'url(' + datasource + ')')
                }
            }
            else{
                if($(window).width() >= 768){
                    $(this).attr('data-src', $(this).attr('data-desktop-src'));
                }
                else{
                    $(this).attr('data-src', $(this).attr('data-mob-src'));
                }
            }
        })

        
    }
})

//close srtrip \
$('.stripclosebtn').click(function(event){
    $(this).parents('.ow-header-strip').remove()
})


function DisableCookiepopup(){
    var url = window.location.pathname;
    var filename = url.substring(url.lastIndexOf('/')+1);
    let result = filename.substring(0, 6);
    if(result == "cookie"){
        $('#onetrust-consent-sdk').addClass('d-none')
    }
    else{
        // alert(result)
    }
}

$(window).on('load', function(){
    // if($('#thankyou-message').length){
    //     setTimeout(function(){
    //         $(document).scrollTop($('#thankyou-message').offset().top - 100)
    //     },500)    
    // }
    setTimeout(function(){
        var url = window.location.href
        var index = url.indexOf("#");
        if (index !== -1)
        {
            var hash = url.substring(index + 1).replaceAll('#', '');
            $(document).scrollTop($('#' + hash).offset().top - 100)
        }
    },500)
    
})


function stickyStrip(){
    var headerHeight = $('header').height();
    $('.ow-strip-fixed').each(function(index){
        if($(this).position().top < 10 || ($('header').prev().hasClass('ow-strip-fixed') || $('#wrapper').prev().hasClass('ow-strip-fixed'))){
            var stripHeight = $(this).height();
            $('header').css('top', stripHeight )
        }

        else if($(this).position().top > headerHeight && $('main').find('.ow-strip-fixed').length){
            $(this).css('top', headerHeight - 37)
        }
    })
}

function bgResize(elem){
    if (elem.hasClass('text-white')) {
        var bgimg = $(elem).attr('style');
        $(elem).append('<div class="stretched-bg" style="' + bgimg.replace("\"", "\'") + '"> </div>');
        var offsetleft = $(elem).offset().left
        var cardwidth = $(elem).width();
        var bgwidth = offsetleft + cardwidth + 60
        $(elem).find('.stretched-bg').css('width', bgwidth);
    }
}

var resizerCont;
window.onresize = function() {
    clearTimeout(resizerCont);
    resizerCont = setTimeout(function() {
        bgResize($('.ow-card-background-image'));
        slider.init();
    }, 100);
};