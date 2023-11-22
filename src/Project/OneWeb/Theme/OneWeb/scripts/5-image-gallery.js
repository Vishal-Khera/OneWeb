

function countchildren(){
    let count = $('.lightbox-wrapper .lightbox-gallery').children().length
    if(count > 1){
        $('.ekko-lightbox-nav-overlay').addClass('d-block');
    }
}

$(document).on('click', '[data-toggle="lightbox"]', function(event) {
    event.preventDefault();
    $(this).ekkoLightbox({alwaysShowClose: true}).queue(function(target){
        countchildren();
        target();
    },100);
    
});