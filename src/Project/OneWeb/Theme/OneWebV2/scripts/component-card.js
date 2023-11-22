var cardSize = (function () {
    var cardHeight = function () {
        __pricetag()
        __cardHeaderHeight()
        __cardRowHeight()
    }
      

function __pricetag (){
    let cardPriceActive = $('.comparison-card')
    if(cardPriceActive){
        [].map.call(cardPriceActive,function(cardpriceSet){
                let elementActive =   $(cardpriceSet).find('.price-wrapper-dots')
                let range =   $(cardpriceSet).find('.price-wrapper-dots').attr('dataprice')
                range =  parseInt(range)
                for (let i = 0; i < range; i++) {
                 elementActive.find('li:eq('+i+')').addClass('active')
               }
           
        })
    }
}

function __cardHeaderHeight (){
    let cardheaderSize = $('.card .equal-height')
    if(cardheaderSize){
        let initialHeight = cardheaderSize.innerHeight();
        [].map.call(cardheaderSize,function(cardheaderSize){
            let dynamicHeight = $(cardheaderSize).innerHeight()
            if(dynamicHeight > initialHeight){
                initialHeight =  dynamicHeight
            }
           
        })
        cardheaderSize.height(initialHeight)
    }
}


function __cardRowHeight (){
    let cardList = $('.equal-card-height')
    if(cardList){
        [].map.call(cardList,function(items, key){
            let initialHeight = 0;
            var allItems = $(items).find('.card-row-size').eq(0).find('>')
            allItems.each(function(item, ele){
                $(items).find('.card-row-'+item).each(function(){
                    if(initialHeight < $(this).outerHeight()){
                        initialHeight = $(this).outerHeight();
                    };
                })
                $(items).find('.card-row-'+item).height(initialHeight);
                initialHeight = 0 
            })
        })
    }
    
}

    return{
        cardHeight:cardHeight
    }

})();
$().ready(function () {
    cardSize.cardHeight();
});

