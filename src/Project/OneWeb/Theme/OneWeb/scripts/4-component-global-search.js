function globalSearch(ele){
    if($(window).width() > 992){
        var eleWidth =  ele.innerWidth();
        var parentWidth = ele.parents('.global-header').innerWidth();
        if($('.search-wrap').length){
            var extendedWith = ele.parents('.search-wrap').next().width() - 45;
        }
        else{
            var extendedWith = ele.parents('.mobile-menu').next().width();
        }
        
        if(ele.parents('.global-search').hasClass('focusIn')){
            ele.parents('.global-search').width(parentWidth + 55);
        }
        else{
            if($(window).width() > 1200){
                ele.parents('.global-search').width(eleWidth + extendedWith + 55);
            }
            else{
                ele.parents('.global-search').width(eleWidth + extendedWith + 35);
            }
        }
        ele.parents('.global-search').toggleClass('focusIn'); 
    }
}

$(document).on('focus', '.global-search input', function(e){
    globalSearch($(this));
});

$(document).on('keyup', function(e){
    if(e.keyCode === 9){
        if(!$(e.target).parents('.ow-search-box').length){
            $('.global-search input').parents('.global-search').removeClass('focusIn'); 
            $('.ow-search-box').find('.ow-container').html('');
        }
    }
});

$(document).on('click', function(event){
    var clickover = $(event.target);
    if(!clickover.parents('.global-search').length){
        if($(window).width() > 992){
            $('.global-search input').parents('.global-search').removeClass('focusIn'); 
        }
    }
})


// $(document).on('keydown', function(event){
//     if(e.keyCode === 9){
//     var clickover = $(event.target);
//     if(!clickover.parents('.global-search').length){
//         if($(window).width() > 992){
//             $('.global-search input').parents('.global-search').removeClass('focusIn'); 
//         }
//     }
//     }
// })

$(document).on('keydown', '.global-search input, .filter-search-wrapper input', function(e){
	if($(this).val().length > 2 && $(this).parents('.global-search, .filter-search-wrapper').find('.result-item-wrapper a').length > 0){
		if (e.keyCode == 40) {
			e.preventDefault();
			$(this).parents('.global-search, .filter-search-wrapper').find('.result-item-wrapper').on('focus', 'a', function () {
				$this = $(this);
				$this.addClass('active');
			}).on('keydown', 'a', function (e) {
				$this = $(this);
                var listLength = $this.parents('ul').find('li').length;
				if (e.keyCode == 40) {
                    if($this.parents('li').index() > (listLength - 2)){
                        $this.parents('.result-item').next().find('a').first().focus();
                    }
                    else{
                        $this.parents('li').next().find('a').focus();
                    }
					return false;
				} else if (e.keyCode == 38) {
                    if($this.parents('li').index() < 1){
                        $this.parents('.result-item').prev().find('a').last().focus();
                    }
                    else{
                        $this.parents('li').prev().find('a').focus();
                    }
					
					return false;
				}
			}).find('a').first().focus();
		} 
	}
});