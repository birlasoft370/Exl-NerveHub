function OnProcessOffsClickNew(e) {
    window.location.href = Resources.url_Index;
}
function OnProcessOffsView(e) {
    window.location.href = Resources.url_ShowProcessOffs;
}
function OnShowProcessOffsClickNew(e) {
    window.location.href = Resources.url_Index;
}


function ProcessOffsDoTheCheck(aspCheckBoxID, oSrc) {

    re = new RegExp('' + aspCheckBoxID + '')
    for (i = 0; i < document.forms[0].elements.length; i++) {

        elm = document.forms[0].elements[i]
        if (elm.type == 'checkbox') {
            if (re.test(elm.id)) {
                if (oSrc.checked) {
                    elm.checked = true;
                }
                else {
                    elm.checked = false;
                }
            }
        }
    }
}

function filterProcess() {
    return {
        iClientID: $("#mClientID").val()
    };
}

$(function (e) {
    $("#list-grid").kendoGrid({
        sotrable: true, height: 450,
        columns: []
    })
});

function OnProcessOffsbtnGenerateClick(e) {

    if (GetFormValidate($('form'), "btnGenerate")) {
        $("#SubmitMode").val('Generate');
    }
}

$("#ProcessOffsbtnSave").on("click", function () {

    var flg = false;
    $(".chkRow").each(function (i) {
        if ($(this).is(":checked")) {
            flg = true;
            return false;
        }
    });

    if (!flg) {

        jAlert(Resources.msg_PleaseSelectDay, Resources.display_Alert);
        return false;
    }
});


function Param() {
    var processid = $("#mProcessIDSearch").val();
    var month = $("#mMonth").val();
    var year = $("#mYear").val();
}

function edit(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var value = dataItem.iProcessId + "|" + dataItem.sName;
    $.ajax({
        type: 'POST',
        url: Resources.url_SetProcessOffsID,
        data: { Proccessid: value },
        dataType: 'json',
        success: function (result) {
            if (result == "1") {
                window.location.href = Resources.url_Index;
            }
        }
    });

    //e.preventDefault();
    //var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    //var value = dataItem.iProcessId + "|" + dataItem.sName;
    //$.ajax({
    //    type: 'Post',
    //    url: Resources.url_SetProcessOffsID,
    //    data: { Proccessid: value },
    //    dataType: 'json',
    //    success: function (result) {
    //        if (result == "1") {
    //            window.location.href = Resources.url_Index;
    //        }
    //    }
    //});
};

function _delete(e) {
    var index = $(e.currentTarget).closest("tr")[0].rowIndex - 1;
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var token = $('#formShowProcessOffs input').val();
    jConfirm(Resources.msg_ProcessDeletedConfirm, Resources.display_Confirmation, function (r) {
        if (r) {
            e.preventDefault();
            $.ajax({
                type: "DELETE",
                url: Resources.url_Delete,
                data: {
                    __RequestVerificationToken: token,
                    ProcessId: JSON.stringify(dataItem.iProcessId + "|" + dataItem.sName)
                },
                dataType: 'json',
                success: function (result) {

                    if (result == "OK") {

                        $("#BEProcessOffGrid  table > tbody tr:eq(" + index + ")").remove();
                        jAlert(Resources.msg_ProcessDeleted, Resources.display_Alert);
                    }
                }
            });
        }
        else {
            return false;
        }
    });
}