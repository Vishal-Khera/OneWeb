var slider = (function () {
    var init = function () {
        _slickSlider();
        _slickArrow();
        
    },
    
    _slickSlider = function () {
        let slide = $('.slider-wrapper, .image-slider .slider-image, .related-products-slider');
        if(slide){
            [].map.call(slide, function(item){
                var viewportWidth = window.innerWidth;
                if($(item).hasClass('ow-disable-carousal-desktop') && viewportWidth >= 1200){
                  if($(item).hasClass('slick-initialized')){
                    $(item).slick("unslick");
                  }
                  
                }
                else{
                  if($(item).attr('data-slides-to-show') != undefined){
                    var slidetoshow = $(item).attr('data-slides-to-show') != '' ? parseFloat($(item).attr('data-slides-to-show').replace(',','.')) : 1;
                    var mobileslidetoshow = $(item).attr('data-slides-to-show-mob') != '' ? parseFloat($(item).attr('data-slides-to-show-mob').replace(',','.')) : 1;
                    var tabslidetoshow = $(item).attr('data-slides-to-show-tab') != '' ? parseFloat($(item).attr('data-slides-to-show-tab').replace(',','.')) : 1;  
                  }
                  else{
                    var slidetoshow = $(item).attr('data-slides-to-show') != '' ? parseFloat($(item).attr('data-slides-to-show')) : 1;
                    var mobileslidetoshow = $(item).attr('data-slides-to-show-mob') != '' ? parseFloat($(item).attr('data-slides-to-show-mob')) : 1;
                    var tabslidetoshow = $(item).attr('data-slides-to-show-tab') != '' ? parseFloat($(item).attr('data-slides-to-show-tab')) : 1;
                  }
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
                              slidesToScroll: 1
                            }
                          },
                          {
                            breakpoint: 992,
                            settings: {
                              slidesToShow: tabslidetoshow,
                              slidesToScroll: 1
                            }
                          },
                          {
                            breakpoint: 767,
                            settings: {
                              slidesToShow: mobileslidetoshow,
                              slidesToScroll: 1
                            }
                          }
                        ]
                    });
                }
            })
        }
    },
    _slickArrow = function () {
      let slideArrow = $('.slider-wrapper .slick-arrow');
      if(slideArrow){
          [].map.call(slideArrow, function(item){
            var slideHeight = $(item).parents('.slider-wrapper').find('.slick-active .slide').innerHeight();
            $(item).parents('.slider-wrapper').find('.slick-arrow').css('top', (slideHeight/2))
          })
      }
  }
    return {
        init: init
    };
})();

$().ready(function () {
    slider.init();

    if($('.slider-content a').length){
      var anchor =  $('.slider-content a')
      anchor.each(function(){
        if($(this).attr('tabindex') == '-1'){
          $(this).attr('tabindex', '0')
        }
      })
    }
});

// var resizer;
// window.onresize = function() {
//     clearTimeout(resizer);
//     resizer = setTimeout(function() {
//       slider.init();
//     }, 100);
// };







