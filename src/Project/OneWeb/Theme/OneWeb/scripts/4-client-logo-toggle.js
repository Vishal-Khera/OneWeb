var businessLogo = (function () {
    var init = function () {
        _businessTabToggle();
       
    },
    
    _businessTabToggle = function () {
        let tabs = $('.client-industry-name');
        if(tabs){
            [].map.call(tabs, function(item){
                var debounceTimeout;
                function debounce(callback) {
                    clearTimeout(debounceTimeout);
                    debounceTimeout = setTimeout(callback, 100);
                }
                $(item).on('click focus', function(){
                    debounce(function() {
                    if($(window).width() > 992){
                        $(item).parents('.client-industry-item').addClass('active').siblings().removeClass('active');                        
                    }
                    else{
                        $(item).next().slideToggle().parent().siblings().find('.client-industry-logos').slideUp();
                        $(item).parents('.client-industry-item').toggleClass('active').siblings().removeClass('active');
                    }
                    $('.client-industry-logos .slider-wrapper').slick('setPosition');
                })
            });
            })
        }
    }
   
    return {
        init: init
    };
})();

$().ready(function () {
    businessLogo.init();
});