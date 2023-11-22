function stream_video(key){
        if ($(key).parents('.ow-video-player').hasClass("ow-video-play")) {
        var videobind = $(key).parents('.ow-video-player').find('video source').attr('data-src')
        $(key).parents('.ow-video-player').find('video source').attr('src', videobind)
        $(key).parents('.ow-video-player').addClass('video-played');
        $(key).parents('.ow-video-player').find("video").get(0).load();
        $(key).parents('.ow-video-player').find("video").get(0).play();
    }
    else {
        var srcattr = $(key).parents('.ow-video-player').find('iframe').attr('data-src')
        $(key).parents('.ow-video-player').addClass('video-played');
        $(key).parents('.ow-video-player').find('iframe').attr('src', srcattr)
    }
}

$(document).on('click', '.ow-video-player a', function (e) {
    if (e.keyCode === 13) {
        stream_video(this)
        return false
    }
    stream_video(this)
});



function pause_media(key){
    var btn = $(key);   
    btn.toggleClass("paused");
    if((btn).hasClass("paused")){
        $(key).parents('.media-wrapper').find("video").get(0).pause();
    }
    else{
        $(key).parents('.media-wrapper').find("video").get(0).play();
    }
    return false;
}

$(document).on('click', '.play-control', function (e) {
    if (e.keyCode === 13) {
        pause_media(this)
        return false
    }
    pause_media(this)
});