// sub menu link active 
$(document).on('click','.dropdwon-arrow', function () {
	if($(this).parent().hasClass('éxpand')){
		$(this).parents('.leftmenu').find('ul').removeClass('show').slideUp();
		$(this).parents('.leftmenu').find('.dropdwon-arrow').parent().removeClass('expand')
	}
	else{
		$(this).closest('li').siblings().find('ul').removeClass('show').slideUp();
		$(this).closest('li').siblings().find('.dropdwon-arrow').parent().removeClass('expand')
		$(this).parent().toggleClass('expand').next().addClass('show').slideToggle();
	}
})

// left menu open close button
$('.left-Menu-btn').click(function (e) {
	$('.leftnav-menu').toggleClass('MenuOpen')
})