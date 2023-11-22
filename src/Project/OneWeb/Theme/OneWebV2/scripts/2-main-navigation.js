$(document).ready(function(){
	$('.hamburger-mobile').click(function(){
		$(this).toggleClass('open');
        $('body').toggleClass('open-navigation');
		$('.engineering-header .dropdown-menu .sub-item').removeClass('active-menu');
	});

	if($('.main-navigation').length){
		var menuItem = $('.main-navigation .has-dropdown');
		menuItem.each(function(){
			if(!$(this).find('.dropdown-menu').length){
				$(this).removeClass('has-dropdown').removeClass('dropdown');
			}
			removeHyperlinkMobile(menuItem)
			
		})
	}

	if($(window).width() > 992){
		if(!$('.main-navigation.hoverEnabled').length){
			var menuList = $('.main-navigation .nav-item');
			menuList.each(function(){
				if($(this).hasClass('dropdown')){
					$(this).find('.nav-link').attr('href', 'javascript:void(0)');
				}
			})
		}
	}

	if(!$('.engineering-header').length){
		$('.ow-primary-header .nav-link.dropdown-toggle').removeAttr('data-toggle');
	}
});


$(document).on('click', function (event) {
	var clickover = $(event.target);
	if (clickover.hasClass('navigation-wrapper')) {
		$('body').removeClass('open-navigation');
		$('.hamburger-mobile').removeClass('open');
		$('.engineering-header .dropdown-menu .sub-item').removeClass('active-menu');	
	}
});

$(document).on('mouseover mouseout', '.hoverEnabled .has-dropdown .nav-link, .hoverEnabled .dropdown-menu', function(){
	$('.ow-primary-header').toggleClass('menu-active')
	$(this).parents('.nav-item').toggleClass('active');
});


$(window).on('resize', function(){
	removeHyperlinkMobile($('.main-navigation .has-dropdown'))
})

function removeHyperlinkMobile(ele) {
	if($(window).width() <= 991){
		ele.find('.dropdown-menu h4 a').attr('href', 'javascript:void(0)');
	}
}


$(document).on('click', '.engineering-header .dropdown-menu h4 a, .engineering-header .dropdown-menu h3 a', function(){
	$('.sub-item').removeClass('active-menu');
	$(this).parents('.sub-item').addClass('active-menu');

})

$(document).on('click', '.back-to-previous', function(){
	$(this).parents('.sub-item').removeClass('active-menu');

})

$(document).on('click', '.navigation-wrapper .has-dropdown .nav-link', function(){
	if(!$('.main-navigation').hasClass('hoverEnabled')){
		if($(this).parents('.has-dropdown').hasClass('show')){
			$('.ow-primary-header').removeClass('menu-active')
		}
		else{
			$('.ow-primary-header').addClass('menu-active')
		}
	}
})

$(document).on('click', '.global-search-header a', function(){
	$('.header-search').show();
	$('.search-overlay').remove();
	$('<div class="search-overlay"> </div>').insertBefore('.header-search');
	$('body').addClass('overflow-hidden')

})

$(document).on('click', '.close-search', function(){
	$('.header-search').hide();
	$('.search-overlay').remove();
	$('body').removeClass('overflow-hidden')
})
