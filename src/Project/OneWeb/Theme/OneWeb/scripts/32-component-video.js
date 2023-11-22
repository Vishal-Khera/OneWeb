$(document).on('click', '.video-modal', function (e) {
    e.preventDefault();
    $('#videoModal .modal-content').html($.ajax({ type: "GET", url: $(this).attr('href'), async: false }).responseText);
    $('#videoModal').modal('show');
    owLazyLoad.bindLazyLoadTriggers();
});

$(document).ready(function () {
    $('#videoModal').on('hidden.bs.modal', function () {
        $('#videoModal .modal-content').html("");
        $('#videoModal').modal('hide');
    });
});

function play_video(key){
        if ($(key).parents('.iframe-wrapper').hasClass("videoplay-btn")) {
        var videobind = $(key).parents('.iframe-wrapper').find('video source').attr('data-src')
        $(key).parents('.iframe-wrapper').find('video source').attr('src', videobind)
		$('.iframe-wrapper').removeClass('video-played');
        $(key).parents('.iframe-wrapper').addClass('video-played');
        $(key).parents('.iframe-wrapper').find("video").get(0).load();
        $(key).parents('.iframe-wrapper').find("video").get(0).play();
    }
    else {
        var srcattr = $(key).parents('.iframe-wrapper').find('iframe').attr('data-src')
        $(key).parents('.iframe-wrapper').addClass('video-played');
        $(key).parents('.iframe-wrapper').find('iframe').attr('src', srcattr)
    }
}
$(document).on('click', '.play-video', function (e) {
    if (e.keyCode === 13) {
        play_video(this)
        return false
    }
    play_video(this)
    

});

