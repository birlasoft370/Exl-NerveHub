
// Function to Navigation between pages
function OnCampTermMappingClickView(e) {
    window.location.href = Resources.url_ShowCampTermMapping;
}

function OnCampTermMappingNewClick(e) {
    window.location.href = Resources.url_Index;
}

// Function check all check boxes

function DoTheCheck(ele) {

    var state = $(ele).is(':checked');
    var grid = $('#gvGridTermCodeMap').data('kendoGrid');
    if (ele.id == "chkAllDisabled") {
        $('.chkDisabled').prop('checked', state);
    }
    if (ele.id == "chkAllIsEnd") {
        $('.chkIsEnd').prop('checked', state);
    }
    if (ele.id == "chkAllIsProductive") {
        $('.chkIsProductive').prop('checked', state);
    }
    if (ele.id == "chkSelectAll") {
        $('.chkFormolsCampTermMap').prop('checked', state);
    }

}


//Function to provide data

function Datainfo() {
    return {
        iClientID: $("#iClientID").val()
        , iProcessID: $("#iProcessID").val()
        , mcampainName: $("#mcampainName").val()

    };
}

//Function to Edit
function editTermCode(e) {

    //  e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var value = dataItem.iCampaignID
    $.ajax({
        type: 'Post',
        url: Resources.url_SetCampTermID,
        data: JSON.stringify({ iCampaignID: dataItem.iCampaignID }),
        contentType: "application/json",
        success: function (result) {
            if (result == "1") {
                window.location.href = Resources.url_Index;
            }
        }
    });
};

//Function to Delete
function deleteTermCode(e) {

    // e.preventDefault();
    var index = $(e.currentTarget).closest("tr")[0].rowIndex;
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var token = $("#formCampTermMapping input").val();

    jConfirm(Resources.Confirm_Delete, Resources.display_Confirmation, function (r) {
        if (r) {
            $.ajax({
                type: "POST",
                url: Resources.url_DeleteCampaignTermCode,
                data: {
                    __RequestVerificationToken: token,
                    id: JSON.stringify(dataItem.iCampaignID)
                },
                dataType: 'json',
                success: function (result) {

                    if (result == Resources.OK) {
                        $("#searchGrid  table > tbody tr:eq(" + index + ")").remove();
                        jAlert(Resources.display_Delete_Message, Resources.display_Alert);
                    }
                    else {
                        jAlert(result, Resources.display_Alert);
                    }
                }
            });
        }
        else {
            return false;
        }

    })
};


function onRequestEnd(e) {

    // if (e.confirm == "delete") {

    if (e.type == "delete") {
        $("#BECampaignList").data("kendoGrid").dataSource.read();

    }

};

// Function to Submit Search View
function GoCampTermMapping(e) {
    $("#formCampTermMapping").submit();
};

// Filter Functions for Cascade Dropdown Lists
function filterProcess(e) {
    return {
        iClientID: $("#iClientID").val()

    };
}


function filterSubProcess(e) {
    return {
        miProcessID: $("#iProcessID").val()

    };
}


/* OnBoundClient*/
function OnBoundClient(e) {
    if ($("#miCampaignID").val() != 0) {

        var dropdownlistiClientID = $("#iClientID").data("kendoDropDownList");
        dropdownlistiClientID.readonly();
    }
};


/* OnBoundProcess*/
function OnBoundProcess(e) {
    if ($("#miCampaignID").val() != 0) {

        var dropdownlistProcessID = $("#iProcessID").data("kendoDropDownList");
        dropdownlistProcessID.readonly();
    }
};

/* OnBoundCampaign*/
function OnBoundCampaign(e) {
    if ($("#miCampaignID").val() != 0) {

        var dropdownlistCampaignID = $("#sCampaignName").data("kendoDropDownList");
        dropdownlistCampaignID.readonly();
    }
};

$("#btnSaveCampTermMapping").on("click", function () {

    var validator = $("#formCampTermMapping").kendoValidator().data("kendoValidator");
    // event.preventDefault();
    var el = "";
    if (validator.validate()) {

        var flg = false;
        $(".chkFormolsCampTermMap").each(function (item) {
            if ($(this).is(":checked")) { flg = $(this).is(":checked"); } else { flg = false; }
        });

        if (!flg) {
            jAlert(Resources.requiredTermCode, "Alert");
            return false;
        }
        else {
            $("#formCampTermMapping").submit();
        }
    }
})

function OnChangeCampaignID(e) {

    // e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    $.ajax({
        type: 'Post',
        url: Resources.url_GetCampTermMappingData,
        data: JSON.stringify({
            CampaignID: $("#sCampaignName").val()
        }),
        contentType: "application/json",
        success: function (result) {

            if (result.miCampaignID != 0) {


                $("#Disable").prop("checked", result.bDisabled);
                $("#miCampaignID").val(result.miCampaignID);
                $("#gvGridTermCodeMap").data("kendoGrid").dataSource.read();
                //  $('#gvGridTermCodeMap').data('kendoGrid').dataSource.data(result.GridTermCodeMap)
                var GridTermCodeMap = $("#gvGridTermCodeMap").data("kendoGrid").dataSource.view();
                $(".chkFormolsCampTermMap").not(this).prop("checked", false);
                $(".chkIsProductive").not(this).prop("checked", false);
                $(".chkIsEnd").not(this).prop("checked", false);
                $(".chkDisabled").not(this).prop("checked", false);
                var count = 0;
                $("#gvGridTermCodeMap table tbody tr").each(function (e) {
                    if (result.GridTermCodeMap != null) {
                        for (var j = 0; j < result.GridTermCodeMap.length; j++) {
                            if (GridTermCodeMap[count].TerminationID == result.GridTermCodeMap[j].TerminationID && result.GridTermCodeMap[j].Selected == "1") {
                                $(this).find(".chkFormolsCampTermMap").prop('checked', 'checked');
                            }
                            if (GridTermCodeMap[count].TerminationID == result.GridTermCodeMap[j].TerminationID && result.GridTermCodeMap[j].IsProductive == "1") {
                                $(this).find(".chkIsProductive").prop('checked', 'checked');
                            }
                            if (GridTermCodeMap[count].TerminationID == result.GridTermCodeMap[j].TerminationID && result.GridTermCodeMap[j].IsEnd == "1") {
                                $(this).find(".chkIsEnd").prop('checked', 'checked');
                            }
                            if (GridTermCodeMap[count].TerminationID == result.GridTermCodeMap[j].TerminationID && result.GridTermCodeMap[j].Disabled == "1") {
                                $(this).find(".chkDisabled").prop('checked', 'checked');
                            }
                        }
                        count++;
                    }
                });
            }
        }
    });
};
$(document.body).on('click', ".clsRemoveSelectedUsers", function (e) {
    var uid = $(this).parent().parent().attr("data-uid")
    var gridViewSelectedUsersData = $("#gridViewSelectedUsers").data().kendoGrid.dataSource.data();

    for (var i = 0; i < gridViewSelectedUsersData.length; i++) {
        if (gridViewSelectedUsersData[i].uid == uid) {
            gridViewSelectedUsersData.remove(gridViewSelectedUsersData[i]);
        }
    }
    $(this).parent().parent().remove();

});
function OnClickAddProcessMaster() {

    var gridData = $("#gridViewSelectedUsers").data("kendoGrid").dataSource.data();
    if (gridData.length > 0) {
        for (var i = 0; i < gridData.length; i++) {
            if (lstTerminationName == '') { lstTerminationName = gridData[i].iTerminationCodeID + '/' + gridData[i].sTermCodeName; }
            else { lstTerminationName = lstTerminationName + ',' + gridData[i].iTerminationCodeID + '/' + gridData[i].sTermCodeName; }

            if (lstTerminationID == '') { lstTerminationID = gridData[i].iTerminationCodeID } else { lstTerminationID = lstTerminationID + ',' + gridData[i].iTerminationCodeID }

        }
        $("#sltTerminationNameID").val(lstTerminationName);
        $("#sltTerminationID").val(lstTerminationID);
        $("#gvGridTermCodeMap").data("kendoGrid").dataSource.read();
        $("#ERPPopup").data("kendoWindow").close();
    }
    else {
        jAlert('Please Select Termination Code  !');
    }
}
function OpenBusinessJustifications() {

    //if(  $("#sltBreakID").val() !='')
    //{ $("#sltBreakNameID").val($("#sltBreakID").val()); } else { $("#sltBreakNameID").val(0); }
    if (GetFormValidate($('form'), "BreakListLink")) {
        try {
            var accessWindow = $("#ERPPopup").kendoWindow({
                actions: ["Maximize", "Close"],
                draggable: true,
                modal: true,
                pinned: true,
                visible: true,
                resizable: true,
                title: "Termination Code Window",
                height: "600px",
                width: "900px"
            });
            $("#ERPPopup").data("kendoWindow").center().open();
        }
        catch (err) {
            alert(err.message);
        }
        // $("#ErpPopupGrid").data("kendoGrid").dataSource.read();
        $("#ErpPopupGrid").find(".chkbxErpPopupGrid").attr("checked", false);
        $("#ErpPopupGrid").find(".checkbox").attr("checked", false);
        $("#sTerminationName").val('');
        $("#gridViewSelectedUsers").data('kendoGrid').dataSource.data([]);
        $("#ErpPopupGrid").data('kendoGrid').dataSource.data([]);
    }
    else {
        $("form").submit();
    }


}
function GetBreakNameDetials() {
    return {
        TerminationName: $("#sTerminationName").val(),
        iCampID: $("#miCampaignID").val() == "0" ? ($("#sCampaignName").val() == "" ? "0" : $("#sCampaignName").val()) : $("#miCampaignID").val(),
        sltTerminationID: $("#sltTerminationID").val()

    }
}
function OnBtnSearchClickBreakMaster() {
    if ($("#sTerminationName").val() != '') {
        var grid = $("#ErpPopupGrid").data("kendoGrid")
        grid.dataSource.read();
    }
    else {
        jAlert('Please fill search text !');
    }

}
function GetCampkMapID() {
    return {
        CampaignID: $("#miCampaignID").val() == "0" ? ($("#sCampaignName").val() == "" ? "0" : $("#sCampaignName").val()) : $("#miCampaignID").val(),
        sltTerminationNameID: $("#sltTerminationNameID").val()
    }



}
function gridViewSerachedUsers_OnRowSelect() {

    var flg1 = false;
    var flg2 = false;
    var gdlen1 = $("#gvGridTermCodeMap").data('kendoGrid').dataSource.data().length
    var gdlen2 = $("#gridViewSelectedUsers").data('kendoGrid').dataSource.data().length

    var gridViewSerachedUser = $("#ErpPopupGrid").data("kendoGrid");
    var selectedItem = gridViewSerachedUser.dataItem(gridViewSerachedUser.select());

    var gridViewUsers = $("#gvGridTermCodeMap").data("kendoGrid");
    var gridViewSelectedUsers = $("#gridViewSelectedUsers").data("kendoGrid");

    if (gdlen1 == 0 && gdlen2 == 0) {
        gridViewSelectedUsers.dataSource.add(selectedItem);
    }
    else {
        var gridViewUsersData = gridViewUsers.dataSource.data();
        var gridViewSelectedUsersData = gridViewSelectedUsers.dataSource.data();

        for (var i = 0; i < gridViewUsersData.length; i++) {
            if (gridViewUsersData[i].TerminationID == selectedItem.iTerminationCodeID) {
                flg1 = true;
            }
        }

        for (var i = 0; i < gridViewSelectedUsersData.length; i++) {
            if (gridViewSelectedUsersData[i].iTerminationCodeID == selectedItem.iTerminationCodeID) {
                flg2 = true;
            }
        }

        if (flg1 || flg2) {
            jAlert("User is already selected or exists", Resources.display_Alert)
        }
        else {
            gridViewSelectedUsers.dataSource.add(selectedItem);
        }
    }
}

function OnChangeClientID() {

    $("#miCampaignID").val(0);
    $('#gvGridTermCodeMap').data('kendoGrid').dataSource.data([]);
}