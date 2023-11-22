if ($('.asset-slider-for').length > 0) {
  $('.asset-slider-for').slick({
    slidesToShow: 1,
    slidesToScroll: 1,
    arrows: false,
    fade: true,
    asNavFor: '.asset-grid-thumbnail'
  });
}

if ($('.asset-grid-thumbnail').length > 0) {
  $('.asset-grid-thumbnail').slick({
    slidesToShow: 3,
    slidesToScroll: 1,
    asNavFor: '.asset-slider-for',
    dots: true,
    centerMode: false,
    focusOnSelect: true
  });
}
