
//Created By:Santosh
//Create on :2020-07-16
//Descriptions: ERP Job & Role Mapping Master


function OnSelectRoleApproveUser(e) {

    //var dataItem = this.dataItem(e.item);
    //if (dataItem.Value == 0) {
    //    $("#Approver").kendoDropDownList({
    //        dataTextField: "ApproverName",
    //        dataValueField: "UserId",
    //        dataSource: null
    //    });
    //}
    //else {
    //    $.ajax({
    //        url: '/ERPJobRoleMap/JsonGetUserRoleApproverList',
    //        type: 'GET',
    //        data: { iRoleId: dataItem.Value },
    //        cache: false,
    //        success: function (dataSource) {
    //            $("#Approver").kendoDropDownList({
    //                dataTextField: "ApproverName",
    //                dataValueField: "UserId",
    //                dataSource: dataSource
    //            });
    //        }
    //    });
    //}
}

function onClick(e) {
    $.ajax({
        type: "POST"
        , url: Resources.urlPath_Index
        , contentType: "application/json"
        , success: function (result) {
        }
    });
}


function btnERPJobRoleMapSaveClick() {
    //debugger;

    //alert('tst');
    //alert('test');
    //window.location.href = Resources.urlPath_Index;

    if (GetFormValidate($('form'), "btnERPJobRoleMapSave")) {

        var modelData = {
            iRoleID: $("#sRoleName").val(), iJobCode: $("#iJobCode").val(), sJobDesc: $("#iJobCode").data("kendoDropDownList").text(),
            bDefaultRole: $("#bDefaultRole").val(), bDisable: $("#bDisable").val(), iApprover: $("#iApprover").val(),
            sRoleName: $("#sRoleName").data("kendoDropDownList").text(), iMappedOn: $("#iMappedOn").val(),


        }


        //var data = {};
        var token = $("#ERPJobRoleMap input").val();
        //$.extend(data);
        // alert(token);
        $.ajax({
            url: Resources.url_SaveERPJobRoleMapping,
            data: { __RequestVerificationToken: token, modelData: modelData },
            type: "POST",
            dataType: 'json',

            success: function (result) {
                if (result.success == false) {
                    jAlert(result.responseText);
                    window.location.href = Resources.urlPath_Index;
                }
                else {

                    jAlert(Resources.ERPJobRoleMappingSaveMsg, 'Alert', function (r) {


                        window.location.href = Resources.urlPath_Index;
                    });
                }
            }
            , error: function (jqXHR, textStatus, errorThrown) {
                // var json = $.parseJSON(jqXHR.responseText);
                // alert(json.message);

                jAlert(errorThrown);
            }
        })
    }
    else {

        $('form').submit();
    }


}

function ERPJobRoleMappingSearch() {

    window.location.href = Resources.urlPath_Search;

}








//$(document).ready(function () {


//    var iJobCode = $("#iJobCode").val();// $("#iJobCode").data("kendoDropDownList");



//});

function onOpenJobCode() {

    // alert('test');

}
function onOpenRolelist() {

    // alert('test');

}
function onOpenUserRoleApprover() {

    // alert('test');

}

function filterApprover() { return { iRoleID: $("#sRoleName").val() }; }


//
function ReloadPage() {

    window.location.href = Resources.urlPath_Index;
};

function onClickRequest() {

    window.location.href = Resources.urlPath_Request;
    //$("#GrdERPJobRoleMap").data("kendoGrid").dataSource.read();
    // $("#GrdERPJobRoleMap").data("kendoGrid").refresh();
};

function onClickRequestSearch() {

    $("#GrdERPJobRoleMap").data("kendoGrid").dataSource.read();
    $("#GrdERPJobRoleMap").data("kendoGrid").refresh();

};

function ERPJobRoleMapRequestFilterGrid() {
    return { FromDate: $("#FromDate").val(), ToDate: $("#ToDate").val() };

};

function startChange() {
    var endPicker = $("#ToDate").data("kendoDatePicker"),
        startDate = this.value();
    if (startDate) {
        startDate = new Date(startDate);
        startDate.setDate(startDate.getDate());
        $("#ToDate").val($("#FromDate").val());
        endPicker.min(startDate);
    }
};
function endChange() {
    var startPicker = $("#FromDate").data("kendoDatePicker"),
        endDate = this.value();
    if (endDate) {
        endDate = new Date(endDate);
        endDate.setDate(endDate.getDate());
        //startPicker.max(endDate);
    }
};

//function onClickBack() {

//    alert('test');

//    urlPath_Index: ResourceERPJobRoleMappingSearch.urlPath_Index;
//};

function onClickApprove() {

    window.location.href = Resources.urlPath_Approve;
};


function onRowBoundGrdERPJobRoleMapApprove(e) {

    $(".k-grid-Reject").find("span").addClass("k-icon k-Reject");
    $(".k-grid-Approve").find("span").addClass("k-icon k-Approve");
}

//function onRowBoundGrdERPJobRoleMapSearch(e) {

//    $(".k-grid-Edit").find("span").addClass("k-icon k-Edit");

//};

//function onClickSearch() {
//    alert('test');
//    window.Location.href = ResourceERPJobRoleMappingSearch.urlPath_ERPJobRoleMapSearchRead;
//}
//function onClickSearch() {
//    //$("#GrdERPJobRoleMapSearch").data("kendoGrid").dataSource.read();
//    //$("#GrdERPJobRoleMapSearch").data("kendoGrid").refresh();

//    if (GetFormValidate($('form'), "btnERPJobRoleMapSave")) {
//       // var token = $("#ERPJobRoleMapSearch input").val();
//        $.ajax(
//            {
//                //RoleJob: $('#SearchJobRoleMapping').val(),
//                type: "POST"
//                , url: ResourceERPJobRoleMappingSearch.urlPath_ERPJobRoleMapSearchRead
//                , data: JSON.stringify({ SearchJobRoleMapping: SearchJobRoleMapping })
//                , contentType: "application/json"
//                , success: function (result) {

//                    $("#GrdERPJobRoleMapSearch").data("kendoGrid").dataSource.read();

//                }
//            });
//    }

//}

//function GridFillterValue() {
//    return {
//        RoleJob: $('#SearchJobRoleMapping').val()

//    };
//}

function EditSearch(e) {


}

//onRowBoundGrdERPJobRoleMapSearch
function onRowBoundGrdERPJobRoleMap(e) {

    $(".k-grid-Cancel").find("span").addClass("k-icon k-Cancel");
}
function cancelrequest(e) {
    jConfirm(ResourceERPJobRoleMappingSearch.display_CancelConfirmation, 'Confirmation', function (r) {
        if (r) {
            e.preventDefault();
            var currentDataItem = $("#GrdERPJobRoleMap").data("kendoGrid").dataItem($(e.currentTarget).closest("tr"));
            // alert('test');
            $.ajax(
                {
                    type: "DELETE"
                    , url: ResourceERPJobRoleMappingSearch.urlPath_DeleteERPJobRoleMappedRequest
                    , data: JSON.stringify({ RequestId: currentDataItem.RequestId })
                    , contentType: "application/json"
                    , success: function (result) {
                        jAlert(ResourceERPJobRoleMappingSearch.display_CancelMessage, "Alert", function () {
                            $("#GrdERPJobRoleMap").data("kendoGrid").dataSource.read();
                        });
                    }
                });
        }
        else {
            return false;
        }
    });

}

function rejectrequest(e) {
    jConfirm(ResourceERPJobRoleMappingSearch.display_RejectConfirmation, 'Confirmation', function (r) {
        if (r) {
            e.preventDefault();
            var currentDataItem = $("#GrdERPJobRoleMapApprove").data("kendoGrid").dataItem($(e.currentTarget).closest("tr"));
            // alert('test');
            $.ajax(
                {
                    type: "DELETE"
                    , url: ResourceERPJobRoleMappingSearch.urlPath_RejectERPJobRoleMappedRequest
                    , data: JSON.stringify({ RequestId: currentDataItem.RequestId })
                    , contentType: "application/json"
                    , success: function (result) {
                        jAlert(ResourceERPJobRoleMappingSearch.display_RejectMessage, "Alert", function () {
                            $("#GrdERPJobRoleMapApprove").data("kendoGrid").dataSource.read();
                        });
                    }
                });
        }
        else {
            return false;
        }
    });

}

function approverequest(e) {
    jConfirm(ResourceERPJobRoleMappingSearch.display_ApproveConfirmation, 'Confirmation', function (r) {
        if (r) {
            e.preventDefault();



            var currentDataItem = $("#GrdERPJobRoleMapApprove").data("kendoGrid").dataItem($(e.currentTarget).closest("tr"));
            // alert('test');
            $.ajax(
                {
                    type: "DELETE"
                    , url: ResourceERPJobRoleMappingSearch.urlPath_ApproveERPJobRoleMappedRequest
                    , data: JSON.stringify({ RequestId: currentDataItem.RequestId })
                    , contentType: "application/json"
                    , success: function (result) {

                        if (result.success == false) {
                            jAlert(result.responseText);
                            // window.location.href = Resources.urlPath_Index;
                        }
                        else {
                            jAlert(ResourceERPJobRoleMappingSearch.display_ApproveMessage, 'Alert', function (r) {
                                //window.location.href = Resources.urlPath_Index;
                                $("#GrdERPJobRoleMapApprove").data("kendoGrid").dataSource.read();
                            });
                        }

                    }
                });
        }
        else {
            return false;
        }
    });

};


function editFilter(e) {

    e.preventDefault();

    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    // var token = $("#frmWorkFilterSearch input").val();


    $.ajax({
        type: "POST",
        url: Resources.url_SetEditableId,
        data: { tempFilterId: dataItem.RoleJobID },
        dataType: "json",
        success: function () {

            window.location.href = Resources.url_Index;

        }
        , error: function (jqXHR, textStatus, errorThrown) {

            jAlert(errorThrown);
        }
    });



}







