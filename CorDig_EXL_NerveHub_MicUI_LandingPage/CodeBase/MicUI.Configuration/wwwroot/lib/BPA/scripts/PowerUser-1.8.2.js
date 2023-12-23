/*
     * FileName: PowerUser-1.8.2.js
     * ClassName: PowerUser-1.8.2
     * Purpose: This file contain all client side scripting for Power User View
     * Description:  
     * Created By: Manoj Kumar Yadav
     * Created Date: 22 June 2015
     * Modified By: 
     * Modified Date:
     * Modified Purpose:
     * Modified Description: 
 */

/* This event is being used to handle btnSavePowerUser click*/
$("#btnSavePowerUser").click(function (event) {
    var validator = $("#frmPowerUser").kendoValidator().data("kendoValidator");
    event.preventDefault();
    if (validator.validate()) {
        try {
            $.LoadingOverlay("show");
            $("#frmPowerUser").submit();
        }
        catch (err) {
            $.LoadingOverlay("hide");
            alert(err);
        }

    } else {

    }
});

/* Used to get the parameter*/
function GetParamLANID() {
    return {
        lanId: $("#txtLanId").val()
    };
}

/* Event trigger on Select Role */
function OnSelectRolePowerUser(e) {
    var dataItem = this.dataItem(e.item);
    if (dataItem.Value == 0) {
        $("#Approver").kendoDropDownList({
            dataTextField: "ApproverName",
            dataValueField: "UserId",
            dataSource: null
        });
    } else {
        $.LoadingOverlay("show");
        $.ajax({
            url: ResourcePowerUser.url_GetApprover,/*'/PowerUser/GetApprover',*/
            type: 'POST',
            data: { iRoleId: dataItem.Value },
            cache: false,
            success: function (dataSource) {
                $.LoadingOverlay("hide");
                $("#Approver").kendoDropDownList({
                    dataTextField: "ApproverName",
                    dataValueField: "UserId",
                    dataSource: dataSource
                });
            },
            error: function (e) {
                 $.LoadingOverlay("hide");
                alert(e);
            }
        });
    }
}
/*  To add the new power user*/
function OnClickNewPowerUser() {
    window.location.href = ResourcePowerUser.urlPath_Index;
}
/* To view the power user list */
function OnClickViewPowerUser() {
    window.location.href = ResourcePowerUser.urlPath_SearchView;
}


/* Click to search the power user*/
function OnClickbtnSearchLanId(event) {

    var validator = $("#frmLanUserPowerUser").kendoValidator().data("kendoValidator");

    event.preventDefault();
    if (validator.validate()) {

        try {

            $("#UserNameGridByLan").data("kendoGrid").dataSource.read();
            $("#divLanGrid").show();
        }
        catch (err) {
            alert(err);
        }

    } else {
        return false;
    }
}

/* Click to get the LAN user*/
function OnClickSearchLan() {
    $("#KendoWindowSearchLAN").data("kendoWindow").center().open();
}

/* Click t close the popup */
$("#divClose").on("click", function () {
    $("#popup_container").hide();
    $("#popup_overlay").hide();
});

/* Click to User name */
$(document).ready($("#UserNameGridByLan table tbody tr").on("click", function () {
    $("#UserName").val($(this).context.all[0].innerText);
    $("#Email").val($(this).context.all[1].innerText);
    $("#FirstName").val($(this).context.all[2].innerText);
    $("#LastName").val($(this).context.all[3].innerText);
    $("#MiddleName").val($(this).context.all[3].innerText);
    $("#Email").val($(this).context.all[5].innerText);
    $("#LanId").val($(this).context.all[6].innerText);
    if ($(this).context.all[7].innerText == "true") {
        $("#IsDisabled").prop("checked", "checked");
    }
    $("#Role").data("kendoDropDownList").select(0);
    var dropdownlist = $("#Role").data("kendoDropDownList");
    dropdownlist.trigger("select");
    $("#KendoWindowSearchLAN").data("kendoWindow").close();
}));
/* Event triggered when Power user saved*/
function OnSuccessSavePowerUser(result) {
    $.LoadingOverlay("hide");
    if (result == '1') {
        jAlert(ResourcePowerUser.display_Role_Request_Made, "Alert");
    }
    else if (result == '2') {
        jAlert(ResourcePowerUser.display_Role_Change_Request, "Alert");
    }
    else if (result == 'notallowed') {
        jAlert(ResourcePowerUser.display_Role_Not_Allowed, "Alert");
    }
    else if (result == 'pending') {
        jAlert(ResourcePowerUser.display_Role_Request_Pending, "Alert");
    }
    else {
        jAlert(result, "Alert");
        return false;
    }
    window.location.href = ResourcePowerUser.urlPath_Index;;
}

/* Search View Page */


function GetParam() {
    return {
        userName: $("#UserName").val()
    };
}

function OnClickClear(event) {
    $("#UserName").val("");
    $("#UserNameGrid").data("kendoGrid").dataSource.read();

}

function OnUserSearchClick(event) {

    $('from').submit();
    //$.ajax({
    //    type: "Get"
    //    , url: "/PowerUser/GetTheUserList"
    //    , data: JSON.stringify({ userName: $("#UserName").val() })
    //    , contentType: "application/json"
    //    , success: function (result) {
    //        
    //        $("#UserNameGrid").data("kendoGrid").dataSource = result;
    //    }
    //});

    //var validator = $("#frmSearchPowerUser").kendoValidator().data("kendoValidator");
    //event.preventDefault();

    //if (validator.validate()) {
    //    try {

    //        $("#UserNameGrid").data("kendoGrid").dataSource.read();
    //    }
    //    catch (err) {
    //        alert(err);
    //    }

    //} else {
    //    return false;
    //}
}

function editUser(e) {
    e.preventDefault();
    var form = $('#frmSearchPowerUser');

    var token = $('input[name="__RequestVerificationToken"]', form).val();

    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    $.ajax({
        type: "POST",
        async: false,
        url: ResourcePowerUser.url_SetEditableUserId,//url: '/PowerUser/SetEditableUserId',// '@Url.Action("SetEditableUserId", "PowerUser")',
        data: { tempUserId: dataItem.UserId },
        dataType: "json",
        success: function () {
            window.location.href = ResourcePowerUser.urlPath_Index;// "@Url.Action("Index", "PowerUser")";
        }
    });
}


function deleteUser(e) {
    jConfirm(ResourcePowerUser.display_UserDeleteConfirmation, 'Confirmation', function (r) {
        if (r) {
            e.preventDefault();
            var currentDataItem = $("#UserNameGrid").data("kendoGrid").dataItem($(e.currentTarget).closest("tr"));

            $.ajax({
                type: "DELETE"
                , url: "/PowerUser/DeleteUser"
                , data: JSON.stringify({ UserId: currentDataItem.UserId })
                , contentType: "application/json"
                , success: function (result) {
                    jAlert(ResourcePowerUser.display_UserDeletedMessage, "Alert", function () {
                        $("#UserNameGrid").data("kendoGrid").dataSource.read();
                    });
                }
            });
        }
        else {
            return false;
        }
    });
}

function onRowBoundPowerUserSearch() {
    $(".k-grid-Edit").find("span").addClass("k-icon k-edit");
    $(".k-grid-Delete").find("span").addClass("k-icon k-delete");
}

function OnClickNew() {
    window.location.href = ResourcePowerUser.urlPath_Index;
}
function OnClickView() {
    window.location.href = ResourcePowerUser.urlPath_SearchView;
}