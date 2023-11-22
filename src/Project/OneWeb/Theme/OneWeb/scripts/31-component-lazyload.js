var owLazyLoad = (function () {
    var bindLazyLoadTriggers = function () {

        var imageElements = document.querySelectorAll('.ow-lazy');
        var videos = [].slice.call($('.video-container video.ow-lazy'));

        if (!('IntersectionObserver' in window)) {
            Array.from(images).forEach(image => lazyLoadImage(image));
        } else {
            // It is supported, load the images
            let options = {
                rootMargin: '0px',
                threshold: 0
            };

            observer = new IntersectionObserver(entries => {
                entries.forEach(entry => {
                    if (entry.intersectionRatio > 0) {
                        lazyLoadImage(entry.target);
                        observer.unobserve(entry.target);
                    }
                });
            }, options);

            imageElements.forEach(image => {
                observer.observe(image);
            });

            var videoObserver = new IntersectionObserver(function(entries, observer) {
            entries.forEach(function(video) {
                if (video.isIntersecting) {
                // video.poster = video.dataset.poster;
                for (var source in video.target.children) {
                    var videoSource = video.target.children[source];
                    if (typeof videoSource.tagName === "string" && videoSource.tagName === "SOURCE") {
                    videoSource.src = videoSource.dataset.src;
                    }
                }
        
                video.target.load();
                video.target.classList.remove("ow-lazy");
                videoObserver.unobserve(video.target);
                }
            });
            });
        
            videos.forEach(function(video) {
            videoObserver.observe(video);
            });
        }

        $(".slider-wrapper, .image-slider, .slider-image").on("afterChange", function (){
            var currentSlideLazyElem = $(this).find('.slick-current .ow-lazy');
            if(currentSlideLazyElem != undefined && currentSlideLazyElem.length > 0){
                lazyLoadImage(currentSlideLazyElem.get(0));
            }
        });

        $(".slider-wrapper, .image-slider, .slider-image").on("beforeChange", function (){
            var currentSlideLazyElem = $(this).find('.slick-current').next().find('.ow-lazy');
            if(currentSlideLazyElem != undefined && currentSlideLazyElem.length > 0){
                lazyLoadImage(currentSlideLazyElem.get(0));
            }
        });
    };

    var lazyLoadImage = function (elem) {
        var dataSrcText = $(elem).attr('data-src');
        if (dataSrcText != undefined) {
            var dataSrc = encodeURI(dataSrcText);
            if (elem.tagName == 'IMG') {
                elem.setAttribute('src', dataSrc);
            }
            else if (elem.tagName == 'IFRAME') {
                if($(elem).hasClass('ow-lazy')){
                    elem.setAttribute('src', dataSrc);
                }
                
            }
            else {
                elem.style.backgroundImage = "url(" + dataSrc + ")"
                if(elem.classList.contains('slide')){
                    $(elem).parents('.slick-slide').next().find('.slide').css('background-image', 'url(' + dataSrc + ')')
                }
                if (elem.classList.contains('text-white')) {
                    var bgimg = $(elem).attr('style');
                    $(elem).append('<div class="stretched-bg" style="' + bgimg.replace("\"", "\'") + '"> </div>');
                    var offsetleft = $(elem).offset().left
                    var cardwidth = $(elem).width();
                    var bgwidth = offsetleft + cardwidth + 60
                    $(elem).find('.stretched-bg').css('width', bgwidth);
                }
            }
        }
        elem.classList.remove("ow-lazy");
        elem.classList.add("ow-lazyloaded");
    }

    return {
        bindLazyLoadTriggers: bindLazyLoadTriggers,
    };

})();

$().ready(function () {
    owLazyLoad.bindLazyLoadTriggers();
});

$(window).on('resize', function(){
    owLazyLoad.bindLazyLoadTriggers();
})