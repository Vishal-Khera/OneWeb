$(document).ready(function(){
    getMaxHeight();
});

function getMaxHeight(){
    $('.ow-disable-carousal-desktop').each(function(){ 
      $(this).find('.ow-business-content').removeAttr('style');
        var highestBox = 0;
        if( $(this).find('.ow-expand-box')){
          $(this).parents('.ow-disable-carousal-desktop').addClass('expandable-box-wrapper')
        }
        $('.ow-expand-box .ow-business-content', this).each(function(){
          if($(this).innerHeight() > highestBox) {
            highestBox = $(this).removeAttr('style').innerHeight(); 
          }
        });  
        $('.ow-expand-box .ow-business-content',this).css('min-height', highestBox);
      }); 
}

$(window).resize(function(){
  setTimeout(function(){
    getMaxHeight();
  },500)
  
})

$(document).on('click', '.ow-expand-box .button-wrapper a', function(e){
  e.preventDefault();
  if($(this).parents('.ow-expand-box').hasClass('expanded')){
    if($(window).width() > 1200){
      $(this).parents('.ow-card').find('.ow-expand-box').removeClass('expanded');
    }
    else{
      $(this).parents('.slick-slide').find('.ow-expand-box').removeClass('expanded');
    }
  }
  else{
    if($(window).width() > 1200){
      $(this).parents('.ow-card').find('.ow-expand-box').addClass('expanded');
      $(this).parents('.ow-card').siblings().find('.ow-expand-box').removeClass('expanded');
    }
    else{
      $(this).parents('.slick-slide').find('.ow-expand-box').addClass('expanded');
      $(this).parents('.slick-slide').siblings().find('.ow-expand-box').removeClass('expanded');
    }

  }
})