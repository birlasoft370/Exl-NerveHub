function OpenSmallModalPopup(html) {
    var strHtml = "";
    strHtml = strHtml + '<div class="header1">'
    + '<div style="float:left;">'
    + '<span id="spnHeaderTitle"></span>'
    + '</div>'
    + '<div style="float:right;">'
    + '<div class="close1" style="color:white;cursor:pointer;">x</div>'
    + '</div>'
    + '</div>'
    + '<div id="divPopupContainer">'
    + html
    + '</div>'
    + '</div>'
    $("#divPopup").html(strHtml);
    $("#divOverlay").show();
    $("#divPopup").show();

    $("#divPopup").draggable({ handle: $(".header1") });
    $(".header1").css({ cursor: 'move' });
}

$(".alphaonly").on("keypress", function (e) {
    var key;
    key = e.which ? e.which : e.keyCode;
    if ((key >= 65 && key <= 91) || (key >= 97 && key < 123) || key == 8 || key == 9 || (key >= 37 && key <= 40) || key == 46 || /*key == 35 || key == 36 ||*/key == 116) {
        return true;
    }
    else {
        return false;
    }
});

function AllowTabOnly() {
   
    var keyCode = this.event.keyCode || this.event.keyCode;
    if (keyCode == 9) {

        return true;
    }
    else {
        return false;
    }
}

$("#parentOfTextbox").on('keydown', '#textbox', function (e) {

});
/* To open a given url(View) */
function MoveToUrl(url)
{ window.location.href = $.trim(url); }

/*Read the gridView */
function SearchGridView() {
    var grid = $("#gridView" + controller).data("kendoGrid")
    grid.dataSource._destroyed = [];
    grid.dataSource.read();
}

/* to select all checkbox of grid */
$("#chkSelectAll").on('click', function (e) {    
    if ($(this).is(":checked")) {
        $(".chkbox").prop("checked", "checked");
    }
    else {
        $(".chkbox").prop("checked", false);
    }
});
$("#chkCateSelectAll").on('click', function (e) {    
    if ($(this).is(":checked")) {
        $(".chk").prop("checked", "checked");
    }
    else {
        $(".chk").prop('checked', false);
    }
});

function ErroPopup(error, body) {

    var erropopup = "<div id='popup_container' class='errorSummary' style='position: fixed; z-index: 99999; padding: 0px; margin: 0px; min-width: 460px; max-width: 460px; top: 195.5px; left: 401.5px;'" +
       "class='ui-draggable'>" +
    "<div id='divPopup_title'>" +
    "<h1 id='popup_title' class='ui-draggable-handle' style='cursor: move;'>error Summary</h1></div><div id='popup_content' class='confirm'><div style='width: 365px; color: rgb(255, 87, 95);' id='popup_message'>" +
   error +
    "</div><div id='popup_panel'><span><button tabindex='0' aria-disabled='false' role='button' data-role='button' class='k-primary k-button k-button-icontext claOkay'  id='popup_ok' type='button'><span class='k-icon k-i-tick'></span>&nbsp;Ok&nbsp;</button></span></div></div></div>"
    + "<div id='popup_overlay' ></div>"
    $("body").append(erropopup);
    $("#popup_overlay").css({
        position: 'fixed',
        zIndex: 99998,
        top: '0px',
        left: '0px',
        width: '100%',
        height: '100%',
        background: 'gray',
        opacity: '0.6',
        overflow: 'auto'
    });
    $("#popup_container").draggable({ handle: $("#popup_title") });
}

function ClientSideValidatePage(formId) {
    var html = "";

    $(formId).find(".rfv").each(function () {
        if (($(this).is("input") || $(this).is("select") || $(this).is("textarea") || $(this).is("file")) && $(this).attr("validationmessage") != undefined)
            html += "<li>" + $(this).attr("validationmessage"); + "</li>";
    });
    if (html == "") {
        return true;
    }
    else {
        ErroPopup(html);
        return false;
    }
}

$(".claOkay").on("click", function () {
    $(".errorSummary").remove();
    $("#popup_overlay").remove();
})