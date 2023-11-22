

if($('.tag-wrapper').length){
    let tagVal = $('.tag-wrapper');
    if(tagVal){
        tagVal.each(function(){
            var targetVal = parseInt($(this).attr('data-tag'))
            var itemLength = $(this).find('.tag')
            if(itemLength.length > targetVal){
                itemLength.each(function(ele, index){
                   if(($(this).index() + 1) > targetVal){
                    $(this).hide();
                   }
                })
                itemLength.parent().append('<a href="javascript:void(0)" class="seeMore">...</a>')
            }
        })
    }
}

$(document).on('click', '.seeMore', function(){
    var anchorlist = $(this).parent().find('.tag')
    anchorlist.each(function(){
        $(this).show();
    })
    $(this).remove();
})