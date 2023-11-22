// polyfill
window.requestAnimationFrame = (function () {
  return window.requestAnimationFrame ||
    window.webkitRequestAnimationFrame ||
    window.mozRequestAnimationFrame ||
    function (callback) {
      window.setTimeout(callback, 1000 / 60);
    };
})();

function marqueeCom(){
  var marqueeList = $('.marquee')
  if (marqueeList) {
    marqueeList.each(function () {
      var itemVal = '';
      var ItemDesktop = $(this).find('.marquee-content').attr('data-slides-to-show') != '' ? parseFloat($(this).find('.marquee-content').attr('data-slides-to-show')) : 12;
      var ItemIpad = $(this).find('.marquee-content').attr('data-slides-to-show-tab') != '' ? parseFloat($(this).find('.marquee-content').attr('data-slides-to-show-tab')) : 6;
      var ItemMobile = $(this).find('.marquee-content').attr('data-slides-to-show-mob') != '' ? parseFloat($(this).find('.marquee-content').attr('data-slides-to-show-mob')) : 2;
      if ($(window).width() > 1200) {
        itemVal = ItemDesktop
      }
      else if ($(window).width() >= 768 && $(window).width() <= 1199) {
        itemVal = ItemIpad
      }
      else {
        itemVal = ItemMobile
      }
      $(this).find('.marquee-content li').css('width', ($(this).find('.marquee-content').width() / itemVal))
    })
  }
}



$(document).ready(function () {
  marqueeCom();

  (function currencySlide() {
    var marqueeList = $('.marquee')
    if (marqueeList) {
      marqueeList.each(function () {
        var speed = parseFloat($(this).find('.marquee-content').attr('data-autoplayspeed'));
        var currencyPairWidth = $(this).find('.marquee-content li:first-child').outerWidth();
        $(this).find(".marquee-content").animate({ marginLeft: -currencyPairWidth }, speed, 'linear', function () {
          $(this).css({ marginLeft: 0 }).find("li:last").after($(this).find("li:first"));
        });

      })
      requestAnimationFrame(currencySlide);
    }
  })();
})

$(window).on('resize', function(){
  marqueeCom();
})


