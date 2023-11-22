$('.ow-navigation-bar a, .ow-redirect-navigation a').click(function(){
    $('html, body').animate({
    scrollTop: $( $(this).attr('href') ).offset().top - ($("header").height() + 30 )
    }, 500);
    return false;
});
