$(document).ready(function () {
    $("#Approvalgrid").hide();/* To hide grid when page load */
    $("#spnNoRecord").show(); /* To show the message no record found  */
    $(".btnAction").hide();   /* To hide Action button like Approve Reject Cancel */
});

/* To set the check box value if any records mark for Approve/Reject/Cancel */
$(function () {
    $('#Approvalgrid').on('click', '.chkbox', function () {
        var checked = $(this).is(':checked');
        var grid = $('#Approvalgrid').data().kendoGrid;
        var dataItem = grid.dataItem($(this).closest('tr'));
        dataItem.IsChecked = checked;
        dataItem.dirty = true;
    })
})

function onRowBoundCampaignSearch(e) {

    $(".k-grid-Edit").find("span").addClass("k-icon k-edit");
    $(".k-grid-Delete").find("span").addClass("k-icon k-delete");

};

/* To open Request view in pop up */
function OpenDetail(level, id) {
    debugger;
    var token = $("#formCampaignApproval input[name=__RequestVerificationToken]").val();
    $.ajax({
        url: ResourceCampaignApprovalView.urlPath_Details,
        data: { __RequestVerificationToken: token, level: level, iApprovalId: id },
        type: "POST",
        dataType: "html",
        success: function (data) {
            $("#ApprovalDetail").html(data);
        },
        error: function () {
            jAlert("An error occured");
        }
    });
    var accessWindow = $("#ApprovalDetail").kendoWindow({
        actions: ["Close"],
        draggable: true,
        height: "600px",
        modal: true,
        resizable: false,
        title: "Campaign Creation Request Details",
        width: "1000px",
        visible: false,
    }).data("kendoWindow").center().open();

}

/* This genric method  which is used for Approve/Rejected/Canceled
   obj parameter denotes which button was clicked and id dendotes iApprovalId  */
function Approval(obj, id) {

    var msg = '';
    $.LoadingOverlay("show");
    $.ajax(
        {
            type: 'POST',
            dataType: 'JSON',
            url: ResourceCampaignApprovalView.urlPath_Approval,
            data: { iApprovalId: id, action: obj.value },
            success:
                function (result) {
                    $.LoadingOverlay("hide");
                    if (result != null) {
                        if (result == ResourceCampaignApprovalView.display_Cancel) {
                            msg = ResourceCampaignApprovalView.diplay_CancelSuccessfully
                        } else if (result == ResourceCampaignApprovalView.display_Reject) {
                            msg = ResourceCampaignApprovalView.diplay_RejectSuccessfully
                        } else if (result == ResourceCampaignApprovalView.display_Approve) {
                            msg = ResourceCampaignApprovalView.diplay_ApproveSuccessfully
                        }
                        jAlert(msg, 'Alert', function (r) {
                            if (r) {
                                $("#Approvalgrid").data("kendoGrid").dataSource.read();
                            }
                        });
                    }
                    else {
                        jAlert(ResourceCampaignApprovalView.display_Error);
                    }
                },
            error:
                function (response) {
                    $.LoadingOverlay("hide");
                    alert("Error: " + response);
                }
        });

    /*
$.ajax({
    type: "POST",
    dataType: 'JSON'
    , url: ResourceCampaignApprovalView.urlPath_Approval
    , data: JSON.stringify({ iApprovalId: id1, action: obj1.value })
    //, contentType: "application/json"
    , success: function (result) {
        if (result != null) {
            if (result == ResourceCampaignApprovalView.display_Cancel) {
                msg = ResourceCampaignApprovalView.diplay_CancelSuccessfully
            } else if (result == ResourceCampaignApprovalView.display_Reject) {
                msg = ResourceCampaignApprovalView.diplay_RejectSuccessfully
            } else if (result == ResourceCampaignApprovalView.display_Approve) {
                msg = ResourceCampaignApprovalView.diplay_ApproveSuccessfully
            }
            jAlert(msg, 'Alert', function (r) {
                if (r) {
                    $("#Approvalgrid").data("kendoGrid").dataSource.read();
                }
            });
        }
        else {
            jAlert(ResourceCampaignApprovalView.display_Error);
        }
    }

});*/
}

/*To Validate from date and to date*/
function chkDate(fromDt, toDt) {
    var diffMSec = toDt - fromDt;
    var DiffMonth = diffMSec / (1000 * 60 * 60 * 24 * 30);
    if (DiffMonth > 1.055535726080247) {
        jAlert("Date Difference must not be older than one month.");
        return false;
    }
    else
        return true;
}

/*To Validate from date and to date*/
function chkDateValidation() {

    var fromdt = new Date($("#FromDate").val());
    var todt = new Date($("#ToDate").val());
    if (fromdt > todt) {
        jAlert("From date must be smaller than To date.");
        return false;
    }
    else if (!chkDate(fromdt, todt))
        return false;
    else { BindApproval(); return true; }

}

/*This is databind event of grid used to check that any visible Approve/Canceled/Rejected */
function Binding_Grid() {
    if ($("#Approvalgrid table tbody tr").length > 0) {
        var showApp = false;
        var showRej = false;
        var showCan = false;
        $("#spnNoRecord").hide();
        $(".btnAction").show();
        $("#Approvalgrid").show();
        $("#Approvalgrid table tbody tr").each(function () { if ($(this).find('td:eq(16)').html().length > 0) { showCan = true; } });
        $("#Approvalgrid table tbody tr").each(function () { if ($(this).find('td:eq(15)').html().length > 0) { showRej = true; } });
        $("#Approvalgrid table tbody tr").each(function () { if ($(this).find('td:eq(14)').html().length > 0) { showApp = true; } });
        if (showCan) { $("#BtnCancel").show(); } else { $("#BtnCancel").hide(); }
        if (showRej) { $("#BtnReject").show(); } else { $("#BtnReject").hide(); }
        if (showApp) { $("#BtnApprove").show(); } else { $("#BtnApprove").hide(); }
    }
    else {
        $("#spnNoRecord").show();
        $("#Approvalgrid").hide();
        $(".btnAction").hide();
    }
}

/*Select all check box for approval/reject/cancel*/
$("#chkSelectAll").on('click', function (e) {
    if ($(this).is(":checked")) {

        //for (var i = 0; i < $("#Approvalgrid table tbody tr").length; i++) {


        //    var row = $("#Approvalgrid table tbody tr")[i];

        //}

        //$("#Approvalgrid table tbody tr")

        var grid = $('#Approvalgrid').data().kendoGrid;
        $(".chkbox").each(function () {

            $(this).prop("checked", "checked");
            var dataItem = grid.dataItem($(this).closest('tr'));
            if (dataItem != null) {
                dataItem.set('IsChecked', true);
            }
        })

    }
    else {
        $(".chkbox").prop("checked", false);
        var grid = $('#Approvalgrid').data().kendoGrid;
        $(".chkbox").each(function () {
            var dataItem = grid.dataItem($(this).closest('tr'));
            if (dataItem != null) {
                dataItem.set('IsChecked', false);
            }
        })
    }
});

/*To apply approval/reject/cancel for multiple row at a time*/
function Save(e) {

    var msg = '';
    var grid = $("#Approvalgrid").data("kendoGrid"),
        parameterMap = grid.dataSource.transport.parameterMap;
    var currentData = grid.dataSource.data();
    var updatedRecords = [];
    for (var i = 0; i < currentData.length; i++) {
        if (currentData[i].dirty) {
            updatedRecords.push(currentData[i].toJSON());
        }
    }

    var modelData = e.value;
    var data = {};
    $.extend(data, parameterMap({ updated: updatedRecords }), parameterMap({ modelData: modelData }));
    $.LoadingOverlay("show");
    $.ajax({
        url: ResourceCampaignApprovalView.urlPath_ApprovalAction,
        data: data,
        type: "POST",
        error: function () {
            $.LoadingOverlay("hide");
            /*Handle the server errors using the approach from the previous example*/
        },
        success: function (result) {
            $.LoadingOverlay("hide");
            if (result != null) {
                if (result == ResourceCampaignApprovalView.display_Cancel) { msg = ResourceCampaignApprovalView.diplay_CancelSuccessfully } else if (result == ResourceCampaignApprovalView.display_Reject) { msg = ResourceCampaignApprovalView.diplay_RejectSuccessfully } else if (result == ResourceCampaignApprovalView.display_Approve) { msg = ResourceCampaignApprovalView.diplay_ApproveSuccessfully }
                jAlert(msg, 'Alert', function (r) {
                    if (r) {
                        $("#Approvalgrid").data("kendoGrid").dataSource.read();
                    }
                });
            }
            else {
                jAlert(display_Error);
            }
        }
    })
}

/*To apply filter to read Campaign approval records*/
function GetFilter() {
    return { dFrom: $("#FromDate").val(), dTo: $("#ToDate").val() };
}

/*To read campaign approval grid*/
function BindApproval() {
    var grid = $("#Approvalgrid").data("kendoGrid")
    grid.dataSource._destroyed = [];
    grid.dataSource.read();
    if (grid.dataSource.data.length > 0) {
        $("#spnNoRecord").hide();
        $("#Approvalgrid").show();
        $(".btnAction").show();
    }
}

/*This is used for Request view when user want save change request*/
function AddClick() {
    SaveBussnessData();
}

function SaveBussnessData() {

    if ($("#ChangeRequest").val() == '' && $("#IsLevel").val() == 1) {
        jAlert(ResourceCampaignApprovalView.display_commentmsg);
        $("#ChangeRequest").focus();
        return false;
    }

    var token = $("#formCampaignApproval input[name=__RequestVerificationToken]").val();
    //var token = $("#formCampaignApproval input").val();
    debugger;
    var BECampaignInfo = {};
    var postUrl;
    BECampaignInfo.ApprovalId = $("#iApproverId").val();
    BECampaignInfo.Location = $("#Location").val();
    BECampaignInfo.ShiftWindow = $("#ShiftWindow").val();
    BECampaignInfo.Purposeofcreationofcampaign = $("#Purposeofcreationofwork").val();
    BECampaignInfo.BusinessJustifications = $("#BusinessJustifications").val();
    BECampaignInfo.Q1 = $("#Q1").val();
    BECampaignInfo.Q2 = $("#Q2").val();
    BECampaignInfo.Q3 = $("#Q3").val();
    BECampaignInfo.Y1 = $("#Y1").val();
    BECampaignInfo.Y2 = $("#Y2").val();
    BECampaignInfo.Y3 = $("#Y3").val();
    BECampaignInfo.KeyBenefits = $("#KeyBenefits").val();
    postUrl = ResourceLayout.partialURL + "SaveRequestDetail";

    //   BEWorkObjectApprover = JSON.stringify({ 'objCampaignInfo': BECampaignInfo, UserLevel: $("#IsLevel").val(), ChangeReq: $("#ChangeRequest").val() });
    BEWorkObjectApprover = { _RequestVerificationToken: token, 'objCampaignInfo': BECampaignInfo, UserLevel: $("#IsLevel").val(), ChangeReq: $("#ChangeRequest").val() };
    $.LoadingOverlay("show");

    $.ajax({
        url: postUrl,
        type: 'POST',
        dataType: 'json',
        data: BEWorkObjectApprover,
        //contentType: 'application/json; charset=utf-8',
        success: function (result) {
            $.LoadingOverlay("hide");
            if (result != null) {
                $("#btnAdd").attr("disabled", "disabled");
                jAlert(ResourceCampaignApprovalView.diplay_submitmsg);
            }

        },
        error: function (err) {
            $.LoadingOverlay("hide");
        }
    });
}
/*Campaign Search View*/
function Go() {

    var form = $('#formCampaignSearchView');
    form.data('validator').settings.ignore = '';
    $("#formCampaignSearchView").submit();
}

//reset

//$("#btnReset").click(function () {
//    alert();
//    winow.location.reload();
//});


function editCampaign(e) {

    e.preventDefault();

    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    // var token = $("#formCampaignSearchView input").val();
    var token = $('input[name=__RequestVerificationToken]').val()
    $.ajax({
        type: "POST"
        , url: ResourceCampaignSearch.urlPath_EditingCustom_Edit
        //  , data: JSON.stringify({ sCampaignID: dataItem.CampaignID })
        , data: { __RequestVerificationToken: token, sCampaignID: dataItem.CampaignID }
        // , contentType: "application/json"

        , success: function (result) {
            if (result == "Ok") {
                window.location.href = ResourceCampaignSearch.urlPath_Index
            }
        }
    });
}
function deleteCampaign(e) {
    e.preventDefault();
    var token = $("#formCampaignSearchView input").val();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var index = $(e.currentTarget).closest("tr")[0].rowIndex - 1;
    jConfirm(ResourceCampaignSearch.display_Delete_Confirmation, 'Confirmation', function (r) {
        if (r) {

            $.ajax({
                type: "POST"
                , url: ResourceCampaignIndex.urlPath_EditingCustom_Destroy
                , data: { __RequestVerificationToken: token, sCampaignID: dataItem.CampaignID }

                , datatype: "json"
                , success: function (result) {
                    $("#searchGrid  table > tbody tr:eq(" + index + ")").remove();
                    jAlert(ResourceCampaignSearch.display_CampaignDeleted);
                }
                , error: function (err) {
                    jAlert(err);
                }

            });
        }
        else {
            return false;
        }
    });
}


/*Index View*/
function OnClickCampaign() {
    window.location.href = ResourceCampaignIndex.urlPath_Index
}
function OnClickCampaignNew() {
    window.location.href = ResourceCampaignSearch.urlPath_Index
}



$.validator.defaults.ignore = "";
function Confirmation(chkbx) {
    jConfirm(ResourceCampaignIndex.display_NoField_Confirmation, 'Confirmation', function (r) {
        if (r) {
            return true;
        }
        else {
            chkbx.checked = false;
            return false;
        }
    });
}

/*To navigate on Work Object Apporvals page*/
function OnClickCampaignApporval() {
    window.location.href = ResourceCampaignIndex.urlPath_ApprovalView
}

function OnClickCampaignSearch() {

    window.location.href = ResourceCampaignIndex.urlPath_CampaignSearchView;
}

$.validator.unobtrusive.adapters.add
    ("multicheckboxvalidation", ['checkboxproperty0', 'checkboxproperty1'],
        function (options) {
            options.rules['multicheckboxvalidation'] = {
                other: options.params.other,
                Checkboxproperty0: options.params.Checkboxproperty0,
                Checkboxproperty1: options.params.Checkboxproperty1
            };
            options.messages['multicheckboxvalidation'] = options.message;
        }
    );
$.validator.addMethod("multicheckboxvalidation", function (value, element, params) {
    var retVal = false;
    if ($(element)) {
        retVal = $(element).attr("checked");
    }
    if (retVal == "checked") {
        return retVal;
    }
    if (params.checkboxproperty0) {
        if ($('#' + params.checkboxproperty0)) {
            retVal = $('#' + params.checkboxproperty0).attr("checked");
        }
    }
    if (retVal == "checked") {
        return retVal;
    }
    if (params.checkboxproperty1) {
        if ($('#' + params.checkboxproperty1)) {
            retVal = $('#' + params.checkboxproperty1).attr("checked");
        }
    }
    if (retVal == "checked") {
        return retVal;
    }
    return false;
});

function OnClickApprovalView() {
    window.location.href = ResourceCampaignApprovalView.urlPath_Index
}