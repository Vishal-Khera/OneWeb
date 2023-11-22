var owSliderCard = (function () {
    
    _cardDestroy = function () {
        let slideCard = $('.slider-widget');
        if(slideCard){
            [].map.call(slideCard, function(item){
                if($(item).hasClass('slick-initialized')){
                    $(item).slick("unslick");
                }
            })
        };
    }

    _cardSlider = function () {
        let slideCard = $('.slider-widget');
        if(slideCard){
            [].map.call(slideCard, function(item){
                var viewportWidth = window.innerWidth;
                if($(item).hasClass('disable-slider-desktop') && viewportWidth >= 1200){
                  if($(item).hasClass('slick-initialized')){
                    $(item).slick("unslick");
                  }
                }
                else{
                    var sliderRowMob = $(item).attr('data-row-mobile') != '' ? parseFloat($(item).attr('data-row-mobile')) : 1;
                    var sliderRowTab = $(item).attr('data-row-tab') != '' ? parseFloat($(item).attr('data-row-tab')) : 1;
                    var sliderRowDesktop = $(item).attr('data-row-desktop') != '' ? parseFloat($(item).attr('data-row-desktop')) : 1;
                    var slidetoshow = $(item).attr('data-slides-to-show') != '' ? parseFloat($(item).attr('data-slides-to-show')) : 1;
                    var mobileslidetoshow = $(item).attr('data-slides-to-show-mob') != '' ? parseFloat($(item).attr('data-slides-to-show-mob')) : 1;
                    var tabslidetoshow = $(item).attr('data-slides-to-show-tab') != '' ? parseFloat($(item).attr('data-slides-to-show-tab')) : 1;
                    var arrow = $(item).attr('data-arrow') != '' ? (/true/i).test($(item).attr('data-arrow')) : false;
                    var dots = $(item).attr('data-dots') != '' ? (/true/i).test($(item).attr('data-dots')) : false;
                    var autoplaySpeed = $(item).attr('data-autoplaySpeed') != '' ? parseInt($(item).attr('data-autoplaySpeed')) : 100000;
                    var autoplay = $(item).attr('data-autoplay') != '' ? (/true/i).test($(item).attr('data-autoplay')) : true;
                    var infiniteval = $(item).attr('data-infinite') != '' ? (/true/i).test($(item).attr('data-infinite')) : true;
                    $(item).not('.slick-initialized').slick({
                        dots: dots,
                        autoplay: autoplay,
                        autoplaySpeed: autoplaySpeed,
                        infinite: infiniteval,
                        speed: 500,
                        cssEase: 'linear',
                        arrows :arrow,
                        slidesToShow: slidetoshow,
                        slidesToScroll: 1,
                        centerMode: false,
                        pauseOnFocus: false, 
                        pauseOnHover: false,
                        responsive: [
                            {
                                breakpoint: 1025,
                                settings: {
                                slidesToShow: tabslidetoshow,
                                slidesToScroll: 1,
                                rows:sliderRowDesktop
                                }
                            },
                            {
                                breakpoint: 992,
                                settings: {
                                slidesToShow: tabslidetoshow,
                                slidesToScroll: 1,
                                rows:sliderRowTab
                                }
                            },
                            {
                                breakpoint: 767,
                                settings: {
                                slidesToShow: mobileslidetoshow,
                                slidesToScroll: 1,
                                rows:sliderRowMob
                                }
                            }
                        ]
                    });
                    
                    $('.slider-widget .slick-dots li button').on('click', function(e){
                        e.stopPropagation();
                    });
                }
            })
        }
    }
    return {
        init: _cardSlider,
        destory: _cardDestroy
    };
})();

$().ready(function () {
    owSliderCard.init();
    //owSliderCard.destory();
});
