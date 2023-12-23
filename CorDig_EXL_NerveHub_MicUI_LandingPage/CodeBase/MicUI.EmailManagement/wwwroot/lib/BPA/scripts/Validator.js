
$(document).ready(function () {
    $("#divValidate").append("<div class='@(Html.ViewData.ModelState.IsValid ? "+"'validation-summary-valid' : 'validation-summary-errors')' data-valmsg-summary='true'><ul></ul></div>");
   
});

$(document).ready(function () {
    // If there is an error element already (server side error), show it.
    showValidationSummaryDialog();

    // When the form validates, and there is an error element, show it
    $('form').bind('invalid-form.validate', function (error, element) {
        showValidationSummaryDialog();
    })
});

function showValidationSummaryDialog() {    
    //$('.validation-summary-errors').dialog({
    //    width: 400,
    //    height: 200,
    //    title: 'Error on Page',
    //    //stack: true,
    //    open: function () {  $(this).css("display", "block"); },
    //    close: function () {           
    //        $(this).dialog('destroy').prependTo($('form'));
    //        $(this).css("display", "none");
    //    }
    //});
}


//function onErrors(event, validator) { // 'this' is the form element
//    var container = $(this).find("[data-valmsg-summary=true]"), list = container.find("ul");
//    if (list && list.length && validator.errorList.length) {
//        list.empty();
//        container.addClass("validation-summary-errors").removeClass("validation-summary-valid");
//        $.each(validator.errorList, function () { $("<li />").html(this.message).appendTo(list); });
//    }
//}
