

$(document).ready(function(){
  let detactmobile = ""
  if(/Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent)){
    detactmobile =  "Mobile"
    $(".language").addClass('without-after-element') 
  }
  else{
    detactmobile =  "Desktop"
    
  }



  $('.js-language').on('click', function () {
    $(this).parents('.language').toggleClass('language-active')
  })

$(document).on('click', function (event) {
    var clickover = $(event.target);
    if (!clickover.hasClass('js-language')) {
        $('.js-language').parents('.language').removeClass('language-active')
    }
})

  document.querySelectorAll('.select-box').forEach(setupSelector);
  function setupSelector(selector) {
    selector.addEventListener('change', e => {
      console.log('changed', e.target.value)  
    })
    selector.addEventListener('mousedown', e => {
      if(detactmobile == "Desktop") {// override look for non mobile
        e.preventDefault();
        
        const select = selector.children[0];
        const dropDown = document.createElement('ul');
        dropDown.className = "selector-options";
        dropDown.id = "list";
  
        [...select.children].forEach(option => {
          
          
          const dropDownOption = document.createElement('li');
          dropDownOption.textContent = option.textContent;
  
          dropDownOption.addEventListener('mousedown', (e) => {
            
            e.stopPropagation();
            select.value = option.value;
            selector.value = option.value;
            select.dispatchEvent(new Event('change'));
            selector.dispatchEvent(new Event('change'));
            dropDown.remove();
            $(document).unbind('keydown', disableArrowKeys);
          });
          
      

          if(!option.selected){
            dropDown.appendChild(dropDownOption);
          }
          
             
        });
       if(selector.childElementCount == 1){
        selector.appendChild(dropDown);
        let getparent = '.selector-options'
        arrowkey(select)
        $(document).keydown(disableArrowKeys);
        
       }  
        // handle click out
        document.addEventListener('click', (e) => {
          if(!selector.contains(e.target)) {
            dropDown.remove();
            $(document).unbind('keydown', disableArrowKeys);
            return true
          }
        });
      }
    });
  }})

function press_enter_key(evnt){


}
  
function arrowkey(key){
  var ul = document.getElementById('list');
  var liSelected;
  var index = -1;
  document.addEventListener('keydown', function(event) {
    var len = ul.getElementsByTagName('li').length - 1;

    if (event.which === 13) {
        [...key.children].forEach(option => {
          var values = "";
         
          if(liSelected.textContent.trim().toUpperCase() == option.value.toUpperCase()){
            key.value = option.value
            key.dispatchEvent(new Event('change'));
          }
        }); 
    }
    
    else if (event.which === 40) {
      index++;
      //down 
      if (liSelected) {
        removeClass(liSelected, 'selected');
        next = ul.getElementsByTagName('li')[index];
        if (typeof next !== undefined && index <= len) {
  
          liSelected = next;
        } else {
          index = 0;
          liSelected = ul.getElementsByTagName('li')[0];
        }
        addClass(liSelected, 'selected');
      
      } else {
        index = 0;
  
        liSelected = ul.getElementsByTagName('li')[0];
        addClass(liSelected, 'selected');
      }
    }
     else if (event.which === 38) {
  
      //up
      if (liSelected) {
        removeClass(liSelected, 'selected');
        index--;
   
        next = ul.getElementsByTagName('li')[index];
        if (typeof next !== undefined && index >= 0) {
          liSelected = next;
        } else {
          index = len;
          liSelected = ul.getElementsByTagName('li')[len];
        }
        addClass(liSelected, 'selected');
      } else {
        index = 0;
        liSelected = ul.getElementsByTagName('li')[len];
        addClass(liSelected, 'selected');
      }
    }
  }, 
  false);
  
  function removeClass(el, className) {
    if (el.classList) {
      el.classList.remove(className);
    } else {
      el.className = el.className.replace(new RegExp('(^|\\b)' + className.split(' ').join('|') + '(\\b|$)', 'gi'), ' ');
    }
  };
  
  function addClass(el, className) {
    if (el.classList) {
      el.classList.add(className);
    } else {
      el.className += ' ' + className;
    }
  };
}

var ar = new Array(37, 38, 39, 40);
var disableArrowKeys = function(e) {
    if ($.inArray(e.keyCode, ar)>=0) {
        e.preventDefault();
    }
}

  // language js
$('.js-language').on('change', function () {
  var getval = $(this).val();
  window.location.href = getval
})
