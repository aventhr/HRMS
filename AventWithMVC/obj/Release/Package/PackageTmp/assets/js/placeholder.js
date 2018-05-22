
$(document).ready(function(){
	
	$('input[type="text"], input[type="password"], textarea').each(function() {
		$(this).val( $(this).attr('placeholder') );
    });
	
	$('p').each(function () {
    var $this = $(this);
    $.ajax({
        url: $this.data('src'),
        dataType: "text",
        success: function (data) {
           $this.html(data);
        }
    });
		$('#text').load("~/assets/text.txt");
});
});