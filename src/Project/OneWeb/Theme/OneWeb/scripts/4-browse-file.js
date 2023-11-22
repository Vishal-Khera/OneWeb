$(window).on('load', function(){
  // upload file remove
    $(".upload-file-remove").on('click', function(){
        $(this).parents('.browse-file').find('input[type="file"]').val('');
		    $(this).hide();
        if($('.browse-file:visible').length > 1){
          $(this).parents('.browse-file').hide();
        }
    });
});

$('.browse-file').change(function(){
	if($(this).find('input[type="file"]').val() != ''){
		$(this).find('.upload-file-remove').show();
	}
})
