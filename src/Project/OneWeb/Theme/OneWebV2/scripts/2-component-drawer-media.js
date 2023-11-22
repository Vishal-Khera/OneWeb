var sliderDrawer = (function () {
  var init = function () {
    _drawerSlider();
  },
    _drawerSlider = function () {
      let drawerSlide = $('.drawer-slider');
      if (drawerSlide) {
        [].map.call(drawerSlide, function (item) {
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
            arrows: arrow,
            slidesToShow: slidetoshow,
            slidesToScroll: 1,
            centerMode: false,
            pauseOnFocus: false,
            pauseOnHover: false,
            lazyLoad: 'ondemand',
            centerPadding: '20px',
            swipe: false,
            initialSlide: 1,
            responsive: [
              {
                breakpoint: 1025,
                settings: {
                  slidesToShow: tabslidetoshow,
                  slidesToScroll: 1,
                  dots: false,
                }
              },
              {
                breakpoint: 992,
                settings: {
                  slidesToShow: tabslidetoshow,
                  slidesToScroll: 1,
                  dots: true,
                  swipe: true
                }
              },
              {
                breakpoint: 767,
                settings: {
                  slidesToShow: mobileslidetoshow,
                  slidesToScroll: 1,
                  dots: true,
                  swipe: true
                }
              }
            ]
          });
          $(item).on("beforeChange", function (event, slick) {
            slick = $(slick.$slider);
            playPauseVideo(slick, "pause");
          });
          $(item).on("afterChange", function (event, slick) {
            slick = $(slick.$slider);
            playPauseVideo(slick, "play");
          });
          if($(item).find('.slick-active').find('video').length > 0){
            $(item).find('.slick-active').find('video').get(0).play();
          }

          if($(item).find('.slick-slide').length < 2){
            $(item).find('.slick-slide').find('.drawer-card__description').slideDown();
          }
        })
      }
    }
  return {
    init: init
  };
})();
$().ready(function () {
  sliderDrawer.init();
  $('[data-slide]').click(function (e) {
    if ($(window).width() > 991) {
      e.preventDefault();
      var slideno = $(this).data('slide');
      $('.drawer-slider').slick('slickGoTo', slideno - 1);

    }
  });
  $(".drawer-slider").on("afterChange", function () {
    if ($(window).width() > 991) {
      $(this).find('.slick-current').siblings().find('.drawer-card__description').slideUp();
      $(this).find('.slick-current').find('.drawer-card__description').slideDown();
    }
  });

  setTimeout(function(){
    $('.drawer-title').hover(function(){
      $(this).parents('.drawer-card__content').toggleClass('hovered');
    });
  })
  
});
function playPauseVideo(slick, control) {
  var currentSlide, slideType, startTime, player, video;
  currentSlide = slick.find(".slick-current");
  startTime = currentSlide.data("video-start");
  if (currentSlide.find('video').length > 0) {
    video = currentSlide.find('video').get(0);
    if (video != null) {
      if (control === "play") {
        video.play();
        
      } else {
        video.pause();
      }
    }
  }
}


