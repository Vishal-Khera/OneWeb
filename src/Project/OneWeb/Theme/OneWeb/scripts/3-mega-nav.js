var megaNav = (function () {
    var init = function () {
        _megaMenu();
        _megamenuhoverEnabled()

    },

        _megaMenu = function () {
            let listitem = $('.mega-nav > li > a');
            let megaList = $('.mega-nav .has-child > a')
            let mobileMenu = $(".handburger");
            let backtoprev = $(".back-to-menu");
            let mobilesearch = $('.mobile-search')
            let _innerHeight = 0;

            if (listitem) {

                [].map.call(listitem, function (openitem) {
                    $(openitem).on('click focus', function (e) {
                        debounce(function () {
                            $('.mega-nav-level2').css("min-height", 2)
                            if ($(openitem).parent().hasClass('has-menu')) {
                                if ($(openitem).parent().hasClass('menu-open')) {
                                    $(openitem).parent().removeClass('menu-open');
                                    $('body').removeClass('menu-overflow');
                                    $('nav .has-child').removeClass('list-open')
                                }
                                else {
                                    $(openitem).parent().addClass('menu-open').siblings().removeClass('menu-open');
                                    $('body').addClass('menu-overflow');
                                }
                            }
                            else {
                                $('.has-menu').removeClass('menu-open');
                                $('body').removeClass('menu-overflow');
                                $('nav .has-child').removeClass('list-open')

                            }

                        });

                    })
                })
            }
            if (megaList) {
                [].map.call(megaList, function (subChild) {
                    $(subChild).on('click focus', function () {
                        
                        debounce(function () {

                            $(subChild).parent().addClass('list-open').siblings().removeClass('list-open');
                            $(subChild).parent().find('.has-child').removeClass('list-open');
                            if ($(window).width() > 991) {
                                _innerHeight = 0
                                $(subChild).parent().find('>ul > li').each(function () {
                                    let event_height = $(this).outerHeight()
                                    _innerHeight = _innerHeight + event_height
                                })

                                let targetelement = $('.mega-nav-level2')
                                let currentheight = $(targetelement).height()
                                let TotalHeight = $(targetelement).outerHeight()
                                let FinalHeight = TotalHeight - currentheight
                                if (currentheight => _innerHeight) {
                                    $(targetelement).css('min-height', _innerHeight + FinalHeight)
                                }
                                else {
                                    _innerHeight = 0
                                    $(targetelement).css('min-height', currentheight)
                                }

                            }
                        
                        });
                    });
                });

            }
            if (mobileMenu) {
                [].map.call(mobileMenu, function (menuOpen) {
                    $(menuOpen).on('click', function () {
                        $('body').toggleClass('open-menu');
                        if ($('.mega-strip').length = 1) {
                            $('.mega-strip').toggleClass('d-none')
                        }

                    })
                })
            }

            if (mobilesearch) {
                [].map.call(mobilesearch, function (menuOpen) {
                    $(menuOpen).on('click', function () {
                        $('body').toggleClass('open-menu');
                        $('.global-search input').focus();
                        setTimeout(function () {
                            $('.global-search input').focus();
                        }, 100)
                    })
                })
            }

            if (backtoprev) {
                [].map.call(backtoprev, function (backcta) {
                    $(backcta).on('click', function () {
                        $(backcta).closest('.has-child.list-open').removeClass('list-open');
                    })
                })
            }


            $(document).on('click focus', function (event) {
                var clickover = $(event.target);
                if (!clickover.parents('.mega-nav').length && !clickover.parents('.mega-nav-level1').length) {
                    $('.mega-nav .has-menu').removeClass('menu-open')
                    $('.mega-nav .has-child').removeClass('list-open')
                    $('body').removeClass('menu-overflow')
                }

            })



            var debounceTimeout;
            function debounce(callback) {
                clearTimeout(debounceTimeout);
                debounceTimeout = setTimeout(callback, 400);
            }
        };
    $('a, input, div').on('focus', function (event) {
        let _target = $(event.target).parents("nav").length
        if (!_target == 1) {
            $('.mega-nav .has-menu').removeClass('menu-open')
            $('.mega-nav .has-child').removeClass('list-open')
            $('body').removeClass('menu-overflow')
        }
    })


    _megamenuhoverEnabled = function () {
        let detectmobile = "";
        let menuhoverclick = $('.main-navigation')
        let listitem = $('.hoverEnabled  li.has-dropdown > a');
        let submenu = $('.hoverEnabled  .dropdown-menu');

        if (listitem) {
            [].map.call(listitem, function (openitem) {
                $(listitem).hover(function (e) {
                    let megastrip = $('.mega-strip-fixed').outerHeight()
                    if ($('body').hasClass('sticky')) {
                        megastrip = ""
                    }
                    $(this).stop(true, true).queue(function (target) {
                        // $('header').addClass('position-fixed')
                        $('header').css('top', megastrip)
                        // stickybody(header)
                        
                        target()
                    });
                },
                    function () {
                        $(this).stop(true, true).queue(function (target) {
                            $('header').removeAttr("style");
                            // $('header').removeClass('position-fixed')
                            // disablstickybody(header)
                            target()
                        });
                    }
                );
            })
        }

        if (submenu) {
            [].map.call(submenu, function (openitem) {
                $(submenu).hover(function (e) {
                    let megastrip = $('.mega-strip-fixed').outerHeight()
                    if ($('body').hasClass('sticky')) {
                        megastrip = ""
                    }
                    $(this).stop(true, true).queue(function (target) {

                        // $('header').addClass('position-fixed')
                        $('header').css('top', megastrip)
                        // stickybody(header)
                        target()
                    });
                },
                    function () {
                        $(this).stop(true, true).queue(function (target) {
                            $('header').removeAttr("style");
                            // $('header').removeClass('position-fixed')
                            // disablstickybody(header)
                            target()
                        });
                    }
                );
            })
        }

        if (/Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent)) {
            detectmobile = "Mobile"
            if ($(menuhoverclick).hasClass("hoverEnabled")) {
                $(menuhoverclick).removeClass('hoverEnabled')
            }
            $('.has-dropdown').on('show.bs.dropdown', function (event) {

                $(this).find('.dropdown-menu').slideDown()
            })
            $('.has-dropdown').on('hide.bs.dropdown', function (event) {
                $(this).find('.dropdown-menu').slideUp()
            })
        }
        else {
            detectmobile = "Desktop"
            if ($(menuhoverclick).hasClass("hoverEnabled")) {
                $(menuhoverclick).find('.dropdown-toggle').attr('data-toggle', 'hover')
            }
            $('.has-dropdown').on('show.bs.dropdown', function (event) {
                let megastrip = $('.mega-strip-fixed').outerHeight()
                // stickybody(header)
                
                if ($('body').hasClass('sticky')) {
                    megastrip = ""
                }
                // $('header').css('top', megastrip)
                // $('body').addClass('menu-overflow');
               
                // if($('.ow-strip-fixed').position().top < 10){
                //     var stickHeight = $('.ow-strip-fixed').height();
                //     $('header').css('top', stickHeight)
                // }
            })
            $('.has-dropdown').on('hide.bs.dropdown', function (event) {
                // disablstickybody(header)
                $('body').removeClass('menu-overflow')
               
            })

        }
    }

    

    // function stickybody(key) {
    //     let intsoffset = $(key).outerHeight()
    //         $('body main').css('padding-top', intsoffset + 5)
    // }
    // function disablstickybody(key) {
    //     let intsoffset = $(key).outerHeight()
    //         $('body main').removeAttr('style')
    // }


    return {
        init: init
    };
})();

$().ready(function () {
    megaNav.init();
});