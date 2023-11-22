$(document).ready(function () {
	if($(window).width() < 767){
		$('.category-navigation ul > li:not(:first-child)').wrapAll('<div class="nav-links"></div>');
		$(".nav-links").addClass('link-visible');
		if($(".nav-links li").length > 3){
			let listCount = $('.category-navigation .first > div').attr('count');
			let linkText = $('.category-navigation .first > div').attr('displaylink');
			listCount = parseInt(listCount)
			let indexOfElement = listCount - 1 ;
			
			if($(".nav-links li").length > indexOfElement){
				$(".nav-links li:gt("+ indexOfElement +")").hide();
				$(".nav-links").append('<li class="item"><a class="nav-expand-link" href="javascript:void(0)">'+linkText+'</a></li>')
				
			}
			$('.nav-expand-link').click(function(){
				$(".nav-links li").slideDown("slow")
				$(this).hide()
			})
		}
	}
});