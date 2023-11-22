$(document).on('click', ".card-item", function () {
    if (!$(this).hasClass('active')) {
        $('.slider-wrapper').find('.card-item').removeClass('active')
        $(this).addClass('active');
    }
});

$(document).on('click', function (event) {
    if (!$(event.target).closest('.card-item.active').length) {
        $('.card-item').removeClass('active')
    }
})

//skip main content 

$(window).on('load', function () {
    // $('.skip-content').hide();
    $('.skip-content').click(function () {
        alert('run');
        $("#" + $(this).attr("href").slice(1) + "")
            .focus().effect("slow", {}, 1000);
    });
});
