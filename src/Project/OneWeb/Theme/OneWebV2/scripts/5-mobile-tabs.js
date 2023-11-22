$(document).ready(function(){
    $(".mobile-tabs ul li").click(function(){
        $(this).addClass("active").siblings().removeClass("active");
        $(".article-left-section").hide();
        $(".article-right-section").hide();
        $($(this).data("value")).fadeIn("500");
    })
})



function imagePosition(){
    if($('.image-wrapper-article').length){
        var imageHeight = $('.image-wrapper-article').innerHeight();
        $('.article-tabs').css('padding-bottom', (imageHeight/2 - 50) )
        
        if($(window).width() > 991){
            $('.article-left-section').css('margin-top', (imageHeight/2 + 50) );
            $('.mobile-tabs').removeAttr();
        }
        else{
            $('.mobile-tabs').css('margin-top', (imageHeight/2 + 50) );
            $('.article-left-section').removeAttr();
        }
    }
}

$(document).ready(function(){
    imagePosition();
})

$(window).on('resize', function(){
    imagePosition();
})

// if($('.sticky-top').length){
//     var headerHeight = $('.ow-primary-header').height();
//     var elemetOffset = $('.sticky-top').offset().top;
//     var elemetwidth = $('.sticky-top').width() + 30;
//     var elementOffset = $('.sticky-top').prev().offset().left;
//     var elementPlace = $('.sticky-top').prev().width()
//     $(window).on('scroll', function(){
//         if($(window).width() > 991){
//             if($(window).scrollTop() > (elemetOffset - headerHeight - 80)){
//                 $('.sticky-top > .row').css({'position':'fixed', 'width': elemetwidth, 'left': (elementOffset + elementPlace + 47), top: (headerHeight + 15)})
//             }
//             else if($(window).scrollTop() < (elemetOffset - headerHeight - 80) ){
//                 $('.sticky-top > .row').removeAttr('style')
//             }
//         }
        
//     })
// }

