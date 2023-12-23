


/*  ---  Index  Page----- */
function DoTheCheck(ele) {

    var state = $(ele).is(':checked');
    if (ele.id == "chkAllApprove_" + ele.name) {

        $('.chkApprove_' + ele.name).prop('checked', state);
    }
    if (ele.id == "chkAllDelete_" + ele.name) {
        $('.chkDelete_' + ele.name).prop('checked', state);
    }
    if (ele.id == "chkAllModify_" + ele.name) {
        $('.chkModify_' + ele.name).prop('checked', state);
    }

    if (ele.id == "chkAllAdd_" + ele.name) {
        $('.chkAdd_' + ele.name).prop('checked', state);
    }

    if (ele.id == "chkAllView_" + ele.name) {
        $('.chkView_' + ele.name).prop('checked', state);
    }
}

$.validator.defaults.ignore = "";

function openRequestStatusWinPopupNew(e) {
    e.preventDefault();
    var RequestStatusWindow1 = $('#RoleRequestStatusWindowPopup').data('kendoWindow');
    RequestStatusWindow1.center().open();
    getStatusList(e);
}

function openRoleApprovalWinPopupNew() {
    var ApprovalStatusWindow1 = $('#RoleApprovalStatusWindowPopup').data('kendoWindow');
    //ApprovalStatusWindow1.refresh({
    //    url: ResourceLayout.partialURL + "RolesApprovalView"
    //}); 
    ApprovalStatusWindow1.refresh();
    ApprovalStatusWindow1.center().open();

}

//$('#createbtn').click(function () {
//    var createWindow = $('#RequestWindow').data('kendoWindow');
//    createWindow.center().open();
//});

//$('#plstRoleReject1').click(function () {
//    alert();

//});

//Open Popup Role Approval view.
function openRoleApprovalWinPopup() {
    $("#RoleApprovalWindow").kendoWindow({
        title: ResourceRoles.display_Request_Status_List,
        modal: true,
        height: "580px",
        width: "900px",
        content: ResourceRoles.urlPath_RolesApprovalView
    }).data("kendoWindow").center().open();

    //var direction = "/AppConfiguration/Roles/RolesApprovalView";
    //var wnd = $("#RoleApprovalWindow").data("kendoWindow");
    //wnd.refresh(direction);
    //wnd.center();
    //wnd.open();
}


//Open Popup Role Request Status View.
function openRequestStatusWinPopup() {
    //alert();
    //$("#RoleRequestWindow123").kendoWindow({
    //    open: function () {
    //        $.ajax({
    //            url: ResourceRoles.urlPath_RolesRequestView,
    //            method: 'GET',
    //            success: function (result) {
    //                $('#RoleRequestWindow123').html(result);
    //            }
    //        });
    //    }
    //});
    $("#RoleRequestWindow123").kendoWindow({
        actions: ["Maximize", "Close"],
        pinned: true,
        visible: true,
        resizable: true,
        title: ResourceRoles.display_Request_Approval_List,
        modal: true,
        height: "580px",
        width: "900px",
        content: ResourceRoles.urlPath_RolesRequestView
    })
    $("#RoleRequestWindow123").data("kendoWindow").center().open();      //  .data("kendoWindow").center().open();

    //var direction = "/AppConfiguration/Roles/RolesRequestView";
    //var wnd = $("#RoleRequestWindow").data("kendoWindow");
    //wnd.refresh(direction);
    //wnd.center();
    //wnd.open();
}


function OnClickRole() {
    window.location.href = ResourceRoles.urlPath_Index;// '@Url.Action("Index", "Roles", new { id = 0 })'
}
function OnClickRoleSearch() {
    window.location.href = ResourceRoles.urlPath_RolesSearchView;// '@Url.Action("RolesSearchView", "Roles")'
}
//function onDataBound(e) {
//    var grid = $("#gvRole").data("kendoGrid");

//    var gridData = grid.dataSource.view();
//    for (var i = 0; i < gridData.length; i++) {
//        var currentUid = gridData[i].uid;
//        var currenRow = grid.table.find("tr[data-uid='" + currentUid + "']");
//        var chbox = $(currenRow).find("td[role]");
//        for (var j = 0; j < chbox.length; j++) {
//            if (chbox[j].children.length > 0) {
//                var chName = chbox[j].children[0].name;
//                if (chName == "chkView") {
//                    if (gridData[i].SelectedView == null) {
//                        chbox[j].children[0].style.display = "none"
//                    }
//                }
//                else if (chName == "chkAdd") {
//                    if (gridData[i].SelectedAdd == null) {
//                        chbox[j].children[0].style.display = "none"
//                    }
//                }
//                else if (chName == "chkModify") {
//                    if (gridData[i].SelectedModify == null) {
//                        chbox[j].children[0].style.display = "none"
//                    }
//                }
//                else if (chName == "chkDelete") {
//                    if (gridData[i].SelectedDelete == null) {
//                        chbox[j].children[0].style.display = "none"
//                    }
//                }
//                else if (chName == "chkApprove") {
//                    if (gridData[i].SelectedApprove == null) {
//                        chbox[j].children[0].style.display = "none"
//                    }
//                }
//            }
//        }
//    }
//}
//function DoTheCheck(aspCheckBoxID, oSrc) {
//    re = new RegExp('' + aspCheckBoxID + '')  //generated control	name starts with a colon
//    for (i = 0; i < document.forms[0].elements.length; i++) {
//        elm = document.forms[0].elements[i]
//        if (elm.type == 'checkbox') {
//            if (re.test(elm.id)) {
//                if (oSrc.checked) {
//                    elm.checked = true;
//                }
//                else {
//                    elm.checked = false;
//                }
//            }
//        }
//    }
//}
function btnApprove_Click(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    $.LoadingOverlay("show");
    $.ajax({
        type: "POST"
        , url: ResourceRoles.urlPath_RoleApprove
        , data: { RequestId: dataItem.RequestId }//JSON.stringify()
        //, contentType: "application/json"
        , dataType: 'json'
        , success: function (result) {
            $.LoadingOverlay("hide");
            if (result == "OK") {
                jAlert(ResourceRoles.display_Request_Approved_Successfully, 'Alert', function (r) {
                    if (r) {
                        $("#gridRoleApproval").data("kendoGrid").dataSource.read();
                    }
                });
            }
            else {
                jAlert(result, "Alert");
            }
        }, error: function (response) {
            $.LoadingOverlay("hide");
            alert("Error: " + response);
        }
    });
}
function btnRejected_Click(e) {
    e.preventDefault();
    var token = $("#FromRoleIndex input[name=__RequestVerificationToken]").val();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    $.LoadingOverlay("show");
    $.ajax({
        type: "POST"
        , url: ResourceRoles.urlPath_RoleReject
        , data: { __RequestVerificationToken: token, RequestId: dataItem.RequestId }
        , datatype: "json"
        , success: function (result) {
            $.LoadingOverlay("hide");
            if (result == "OK") {
                jAlert(ResourceRoles.display_Request_Rejected_Successfully, 'Alert', function (r) {
                    if (r) {
                        $("#gridRoleApproval").data("kendoGrid").dataSource.read();
                    }
                });
            }
            else {
                jAlert(result, "Alert");
            }
        }, error: function (response) {
            $.LoadingOverlay("hide");
            alert("Error: " + response);
        }
    });
}
function startChange() {
    var endPicker = $("#EndDate").data("kendoDatePicker"),
        startDate = this.value();
    if (startDate) {
        startDate = new Date(startDate);
        startDate.setDate(startDate.getDate());
        endPicker.min(startDate);
    }
}
function endChange() {
    var startPicker = $("#StartDate").data("kendoDatePicker"),
        endDate = this.value();
    if (endDate) {
        endDate = new Date(endDate);
        endDate.setDate(endDate.getDate());
        startPicker.max(endDate);
    }
}
function getDate(e) {
    return {
        startDate: $("#StartDate").data("kendoDatePicker")._oldText,
        endDate: $("#EndDate").data("kendoDatePicker")._oldText
    };
}
function getStatusList(e) {
    e.preventDefault();
    $("#gridRoleRequest").data("kendoGrid").dataSource.read();
}
function btnCancelRequest(id) {
    // var token = $("#FromRoleIndex input").val(); c
    var token = $("#FromRoleIndex input[name=__RequestVerificationToken]").val();
    $.LoadingOverlay("show");
    $.ajax({
        type: "POST"
        , url: ResourceRoles.urlPath_CancelRequest
        //  , data: JSON.stringify({ __RequestVerificationToken: token, RequestId: dataItem.RequestId })
        , data: { __RequestVerificationToken: token, RequestId: id }
        // , contentType: "application/json"
        , success: function (result) {
            $.LoadingOverlay("hide");
            if (result == "OK") {
                jAlert(ResourceRoles.display_Request_cancelled_successfully, 'Alert', function (r) {
                    if (r) {
                        $("#gridRoleRequest").data("kendoGrid").dataSource.read();
                    }
                });
            }
            else {
                jAlert(result, "Alert");
            }
        }, error: function (response) {
            $.LoadingOverlay("hide");
            alert("Error: " + response);
        }
    });
}


/* RolesSearchView Page */

function Go() {
    $("#formRolesSearchView").submit();
}
function OnClickRoles() {
    window.location.href = ResourceSearch.urlPath_Index + "?id=" + 0;// '@Url.Action("Index", "Roles", new { id = 0 })'
}
function editRole(e) {
    e.preventDefault();

    var token = $("#formRolesSearchView input").val();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    $.ajax({
        type: "POST"
        , url: ResourceSearch.urlPath_EditingCustom_Edit
        , data: { sRoleID: dataItem.RoleId }
        , dataType: 'json'
        , success: function (result) {

            if (result == 1)
                window.location.href = ResourceSearch.urlPath_Index; //"@Url.Action("Index", "Roles")"

        }
    });
}
function deleteRole(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var ddlSearchApprover = $("#SearchApprover").val();
    if (ddlSearchApprover == '') {
        jAlert("Please Select Approver..");
        $("#SearchApprover").focus();
    }
    else {
        jConfirm(ResourceSearch.display_Delete_Confirmation, 'Confirmation', function (r) {
            if (r) {

                var token = $("#formRolesSearchView input").val();

                $.ajax({
                    type: "POST"
                    , url: ResourceSearch.urlPath_EditingCustom_Edit
                    , data: { __RequestVerificationToken: token, sRoleID: dataItem.RoleId }
                    , dataType: 'json'
                    , success: function (result) {

                        if (result == null) {

                            $.ajax({
                                type: "DELETE"
                                , url: ResourceSearch.urlPath_EditingCustom_Destroy
                                , data: { __RequestVerificationToken: token, sRoleID: dataItem.RoleId, iApprover: $("#SearchApprover").val() }
                                , dataType: 'json'
                                , success: function (result) {
                                    if (result == null) {
                                        jAlert('Role Delete Successfully.', 'Alert', function (r) {
                                            if (r) {
                                                $("#searchGrid").data("kendoGrid").dataSource.read();
                                            }
                                        });
                                    }
                                    else {
                                        jAlert(result);
                                    }
                                }
                                , error: function (jqXHR, textStatus, errorThrown) {

                                    jAlert(errorThrown);
                                }
                            });
                        }
                    }
                });

            }
            else {
                return false;
            }
        });
    }
}

$(document).on("click", "#btnRoleSave", function (event) {
    var editid = $("#RoleId").val();
    var CheckFlagExp = false;
    var CheckedCondetion = false;
    var cnt = 0;
    var tempevent = event.srcElement == null || undefined ? event.target : event.srcElement; //added by indresh 17/08/2017 for cross browser

    if (GetFormValidate($('form'), (tempevent.name || tempevent.name))) {


        var RoleViewModel = {};
        RoleViewModel.RoleId = $("#RoleId").val();
        RoleViewModel.RoleName = $("#RoleName").val();
        RoleViewModel.Description = $("#Description").val();
        RoleViewModel.Disable = $("#Disable").is(":checked");
        RoleViewModel.IsClientUserRole = $("#IsClientUserRole").is(":checked");
        RoleViewModel.RoleLevel = $("#RoleLevel").val();
        var GetFromActionCollection = [];
        var dataItem = $("#gvRole").data("kendoGrid").dataSource;

        var DataItemLng = dataItem._data.length;

        for (var i = 0; i < dataItem._data.length; i++) {
            var ChiledGrd = '#gvRole_' + $("#gvRole").data("kendoGrid").dataSource._data[i].ModuleName + ' table tbody tr';
            var tModuleName = $("#gvRole").data("kendoGrid").dataSource._data[i].ModuleName;
            for (var j = 0; j < $(ChiledGrd).length; j++) {
                CheckFlagExp = true;
                CheckedCondetion = true;
                var GetFromAction = {};

                GetFromAction.SelectedView = $($(ChiledGrd)[j]).find(".chkView_" + tModuleName).is(":checked") == true ? 1 : 0;
                GetFromAction.SelectedAdd = $($(ChiledGrd)[j]).find(".chkAdd_" + tModuleName).is(":checked") == true ? 1 : 0;
                GetFromAction.SelectedModify = $($(ChiledGrd)[j]).find(".chkModify_" + tModuleName).is(":checked") == true ? 1 : 0;
                GetFromAction.SelectedDelete = $($(ChiledGrd)[j]).find(".chkDelete_" + tModuleName).is(":checked") == true ? 1 : 0;
                GetFromAction.SelectedApprove = $($(ChiledGrd)[j]).find(".chkApprove_" + tModuleName).is(":checked") == true ? 1 : 0;
                GetFromAction.FormId = $($(ChiledGrd)[j]).find("td:eq(0)").text();
                GetFromActionCollection.push(GetFromAction);
            }


            if (CheckFlagExp == true) {
                if (!(validateModuleName(GetFromActionCollection))) {
                    cnt = cnt + 1;
                }
                CheckFlagExp = false;
            }
            else {
                if (editid <= 0) {
                    if (!(validateModuleName(GetFromActionCollection))) {
                        cnt = cnt + 1;
                    }
                }
            }
        }
        if (DataItemLng == cnt) {
            jAlert("Please assign atleast one module.");
            return false;
        }
        RoleViewModel.lstFromActionMap = GetFromActionCollection;
        //BERoleViewModelObject = JSON.stringify({__RequestVerificationToken: token, 'JsonAllowAction': RoleViewModel });
        var token = $("#FromRoleIndex input[name=__RequestVerificationToken]").val();
        //var token = $("#FromRoleIndex input").val();
        $.LoadingOverlay("show");
        $.ajax({
            type: "POST",
            dataType: 'json'
            , url: ResourceRoles.urlPath_Index
            , data: { __RequestVerificationToken: token, 'JsonAllowActionData': RoleViewModel, 'iApprover': $("#Approver").val() }
            // , data: JSON.stringify({ 'JsonAllowActionData': RoleViewModel }),
            // contentType: 'application/json; charset=utf-8'
            , success: function (result) {
                $.LoadingOverlay("hide");
                if (result.success == true) {
                    if (parseInt(result.strMsg) > 0) {

                        jAlert(ResourceRoles.display_Request_Saved_successfully, 'Alert', function () {
                            window.location.href = ResourceRoles.urlPath_Index;
                        });

                    }
                    else {
                        jAlert(result.strMsg, "Alert")
                    }
                }
                else {
                    jAlert(result.message, "Alert")
                }
            }
            , error: function (err) {
                $.LoadingOverlay("hide");
            }
        });
    }
    else {
        $("form").submit();
    }
});


// Developed By Indresh
// validate module name 
function validateModuleName(GetFromActionCollection) {

    var count = 0;
    for (var i = 0; i < GetFromActionCollection.length; i++) {
        if (GetFromActionCollection[i].SelectedView == 1 || GetFromActionCollection[i].SelectedAdd == 1 || GetFromActionCollection[i].SelectedModify == 1 || GetFromActionCollection[i].SelectedDelete == 1 || GetFromActionCollection[i].SelectedApprove == 1) {
            count++;
        }
    }

    if (count > 0) {
        return true;
    }
    else {
        return false;
    }

}