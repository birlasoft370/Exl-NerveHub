/*
     * FileName: ReportsConfig-1.8.2.js
     * ClassName: ReportsConfig-1.8.2
     * Purpose: This file contain all Reports scripting for Reports Config 
     * Description:  Create Js file Page Script send to JS File
     * Created By: Deepak Kumar Maurya
     * Created Date: 14 June 2016
     * Modified By: 
     * Modified Date:
     * Modified Purpose:
     * Modified Description: 
 */
/*  Rpt_AccessReport Page  Crated By Nabin*/

var wnd;
function filterProcess() {

    return {
        iClientID: $("#ClientName").val()
    };
}
function OnSelect(e) {

    var multiselect = $("#ProcessName").data("kendoMultiSelect");
    //multiselect.dataSource.filter({});
    multiselect.dataSource.filter(filterProcess());

}
function checkAllRpt_AccessReport(ele) {
    var state = $(ele).is(':checked');
    $('.chkRpt_AccessReport').prop('checked', state);
}



//$("#exportACL").on("click", function (e) {

//    var grid = $("#divRptAccessControl").data("kendoGrid");
//    grid.saveAsExcel();
//});

//MultiSelect - A user extension of KendoUI DropDownList widget.
function OnClickGenerateAccessControl(e) {
    var dllClient = $("#ClientName").data("kendoMultiSelect");
    // var token = $("#Rpt_AccessReport input").val();
    var token = $("#Rpt_AccessReport input[name=__RequestVerificationToken]").val();
    if (GetFormValidate($('form'), "btnGenerateAccessControlReport")) {
        kendo.ui.progress($('#Main_Div'), true);
        $.LoadingOverlay("show");
        $.ajax({
            type: 'POST',
            url: ResourceRptAccessReport.urlPath_GetAccessControlReport,
            dataType: 'json',
            data: {
                __RequestVerificationToken: token,
                ClientID: $("#ClientName").data("kendoMultiSelect").value(),
                ProcessID: $("#ProcessName").data("kendoMultiSelect").value(),
                RoleID: $("#RoleName").data("kendoMultiSelect").value()
            },
            success: function (result) {
                $.LoadingOverlay("hide");
                if (result != 0) {
                    var columnslist = [];
                    if (result.length > 0) {
                        columnslist.push({ field: "ClientID", title: ResourceRptAccessReport.display_ClientID, editable: false, hidden: true });
                        columnslist.push({ field: "LastUpdatedAt", title: ResourceRptAccessReport.display_LastUpdatedAt, editable: false, hidden: true });
                        columnslist.push({ field: "LastUpdatedBy", title: ResourceRptAccessReport.display_LastUpdatedBy, editable: false, hidden: true });
                        columnslist.push({ field: "ClientName", title: ResourceRptAccessReport.display_ClientName, editable: false, width: 140 });
                        columnslist.push({ field: "ProcessID", title: ResourceRptAccessReport.display_ProcessID, editable: false, hidden: true });
                        columnslist.push({ field: "ProcessName", title: ResourceRptAccessReport.display_ProcessName, editable: false, width: 140 });
                        columnslist.push({ field: "CampaignID", title: ResourceRptAccessReport.display_CampaignID, editable: false, hidden: true });
                        columnslist.push({ field: "CampaignName", title: ResourceRptAccessReport.display_CampaignName, editable: false, hidden: true });
                        columnslist.push({ field: "TeamID", title: ResourceRptAccessReport.display_TeamID, editable: false, hidden: true });
                        columnslist.push({ field: "TeamName", title: ResourceRptAccessReport.display_TeamName, editable: false, width: 140 });
                        columnslist.push({ field: "UserID", title: ResourceRptAccessReport.display_UserID, editable: false, hidden: true });
                        columnslist.push({ field: "UserName", title: ResourceRptAccessReport.display_UserName, editable: false, width: 140 });
                        columnslist.push({ field: "EmpID", title: ResourceRptAccessReport.display_EmployeeID, editable: false, width: 140 });
                        columnslist.push({ field: "RoleName", title: ResourceRptAccessReport.display_RoleName, editable: false, width: 140 });
                        columnslist.push({ field: "LoginName", title: ResourceRptAccessReport.display_LoginName, editable: false, width: 140 });
                        columnslist.push({ field: "Expr1", title: ResourceRptAccessReport.display_Expr1, editable: false, hidden: true });
                        columnslist.push({ field: "RoleID", title: ResourceRptAccessReport.display_RoleID, editable: false, hidden: true });
                        if (result.length > 0) {
                            $("#divRptAccessControl").css("width", "98%");
                            $("#divRptAccessControl").css("height", "1000");
                            $('#divRptAccessControl').html("");

                            var dataSource = new kendo.data.DataSource({
                                data: result,
                                autoSync: true,
                                pageSize: 10,
                                schema: {
                                    model: {

                                    }
                                }
                            });

                            $("#divRptAccessControl").kendoGrid({
                                dataSource: dataSource,
                                height: 450,
                                sortable: true,
                                reorderable: true,
                                autoBind: true,
                                columns: columnslist,
                                pageable: {
                                    refresh: true,
                                    pageSizes: [5, 10, 20, 50, 100, 200, 300, "all"],
                                    buttonCount: 5
                                },
                                toolbar: ["excel", "search"],
                                excel: {
                                    fileName: "Export.xlsx",
                                    // proxyURL: ResourceRptAccessReport.urlPath_Excel_Export_Save,
                                    filterable: true,
                                    allPages: true

                                },
                            })
                        }
                    }
                    kendo.ui.progress($('#Main_Div'), false);
                } else {
                    kendo.ui.progress($('#Main_Div'), false);
                    $("#divRptAccessControl").css("width", "0");
                    $("#divRptAccessControl").css("height", "0");
                    $('#divRptAccessControl').html("");
                    kendo.ui.progress($('#divRptAccessControl'), false);
                    jAlert(ResourceRptAccessReport.display_RecordNotFound);
                }


            }
            , error: function (error) {
                $.LoadingOverlay("hide");
            }
        });
    }
    else {
        $('form').submit();
    }


}

function OnClickRefresh(e) {
    var multiselectClient = $("#ClientName").data("kendoMultiSelect");
    var multiselectProcess = $("#ProcessName").data("kendoMultiSelect");
    var multiselectRole = $("#RoleName").data("kendoMultiSelect");
    multiselectClient.dataSource.filter({}); //clear applied filter before setting value
    multiselectClient.value("");
    multiselectProcess.dataSource.filter({}); //clear applied filter before setting value
    multiselectProcess.value("");
    multiselectRole.dataSource.filter({}); //clear applied filter before setting value
    multiselectRole.value("");
    //Clears Grid
    $("#divRptAccessControl").css("width", "0");
    $("#divRptAccessControl").css("height", "0");
    $('#divRptAccessControl').html("");

    var validator = $('form').kendoValidator().data("kendoValidator");
    //hide the validation messages when hide button is clicked
    validator.hideMessages();

}
function OnClickERPGenReport(e) {
    OnSelect_ERPRoleJobMapping('n');
}


/* Rpt_ERPRoleJobMappingReport Page  Crated By Nabin*/
function OnSelect_ERPRoleJobMapping(e) {

    var Job = $('#Job').val();
    $.LoadingOverlay("show");
    $.ajax({
        type: 'POST',
        url: ResourceRptERPRoleJobMappingReport.urlPath_GetERPJobRoleMappingReport, // /AppConfiguration/Reports/GetERPJobRoleMappingReport",
        dataType: 'json',
        data: {
            ERPJob: Job

        },
        success: function (result) {
            $.LoadingOverlay("hide");
            var columnslist = [];

            if (result.length > 0) {


                columnslist.push({ field: "iRoleJobID", title: ResourceRptERPRoleJobMappingReport.display_RoleJobID, editable: false, width: 140 });
                columnslist.push({ field: "oRole", title: "Role", editable: false, width: 140 });
                columnslist.push({ field: "oJob", title: "Job", editable: false, width: 140 });
                columnslist.push({ field: "RoleJob", title: "Role Job", editable: false, width: 140 });
                columnslist.push({ field: "iMappedOn", title: "Mapped On", editable: false, width: 140 });
                columnslist.push({ field: "bDefaultRole", title: "Default Role", editable: false, width: 140 });
                columnslist.push({ field: "bDisabled", title: "Disabled", editable: false, width: 140 });
                columnslist.push({ field: "bFreshTransaction", title: "Fresh Transaction", editable: false, width: 140 });
                columnslist.push({ field: "iCreatedBy", title: "CreatedBy", editable: false, width: 140 });
                columnslist.push({ field: "dCreateDate", title: "Create Date", editable: false, width: 140 });
                columnslist.push({ field: "iModifiedBy", title: "Modified By", editable: false, width: 140 });
                columnslist.push({ field: "dModifyDate", title: "Modify Date", editable: false, width: 140 });

                if (result.length > 0) {
                    $("#divRptERPRoleJobMapping").css("width", "1200");
                    $("#divRptERPRoleJobMapping").css("height", "300");
                    $('#divRptERPRoleJobMapping').html("");

                    var dataSource = new kendo.data.DataSource({
                        data: result,
                        autoSync: true,
                        pageSize: 10,
                        schema: {
                            model: {

                            }
                        }
                    });

                    $("#divRptERPRoleJobMapping").kendoGrid({
                        dataSource: dataSource,
                        height: 450,
                        sortable: true,
                        pageable: {
                            refresh: true,
                            pageSizes: [5, 10, 20, 50, 100, 200, 300, "all"],
                            buttonCount: 5
                        },

                        reorderable: true,
                        autoBind: true,
                        columns: columnslist,
                        toolbar: ["excel", "search"],
                        excel: {
                            fileName: "Export.xlsx",
                            // proxyURL: "/AppConfiguration/Reports/Excel_Export_Save",
                            filterable: true,
                            allPages: true
                        },


                    })
                }

            }
            else {

                $("#divRptERPRoleJobMapping").css("width", "0");
                $("#divRptERPRoleJobMapping").css("height", "0");
                $('#divRptERPRoleJobMapping').html("");
                jAlert(ResourceRptERPRoleJobMappingReport.display_RecordNotFound);
            }
        }
    });
}
function checkAllRpt_AccessReport_ERPRoleJobMapping(ele) {
    var state = $(ele).is(':checked');
    $('.chkRpt_AccessReport').prop('checked', state);
}
//MultiSelect - A user extension of KendoUI DropDownList widget.

function OnClickERPRoleJobMappingReport(e) {


    $.ajax({
        type: 'POST',
        url: "/AppConfiguration/Reports/GetERPJobRoleMappingReport",
        dataType: 'json',
        data: {
            ERPJob: $('#ClientName').val()
        },
        success: function (result) {

            var columnslist = [];

            if (result.length > 0) {

                for (var prop in result[0]) {
                    if (result[0].hasOwnProperty(prop)) {
                        columnslist.push({ field: prop, editable: false, width: 140 });
                    }
                }


                if (result.length > 0) {
                    $("#divRptERPRoleJobMapping").css("width", "1200");
                    $("#divRptERPRoleJobMapping").css("height", "300");
                    $('#divRptERPRoleJobMapping').html("");

                    var dataSource = new kendo.data.DataSource({
                        pageSize: 10,
                        data: result,
                        autoSync: true,
                        schema: {
                            model: {
                                id: "AdjustID"
                            }
                        }
                    });

                    $("#divRptERPRoleJobMapping").kendoGrid({
                        dataSource: dataSource,
                        height: 300,
                        sortable: true,
                        pageable: {
                            refresh: true,
                            pageSizes: [5, 10, 20, 50, 100, 200, 300, "all"],
                            buttonCount: 5
                        },

                        reorderable: true,
                        autoBind: true,
                        columns: columnslist,
                        toolbar: ["excel", "search"],
                        excel: {
                            fileName: "Export.xlsx",
                            filterable: true,
                            allPages: true
                        },
                        // START/



                        // END/


                    })
                }

            }
            else {

                $("#divRptERPRoleJobMapping").css("width", "0");
                $("#divRptERPRoleJobMapping").css("height", "0");
                $('#divRptERPRoleJobMapping').html("");
                jAlert(display_RecordNotFound);
            }
        }





    });

}

function OnClickERPRoleJobMappingRefresh(e) {


    $("#Job").data("kendoDropDownList").select(0);

    ////Clears Grid
    $("#divRptERPRoleJobMapping").css("width", "0");
    $("#divRptERPRoleJobMapping").css("height", "0");
    $('#divRptERPRoleJobMapping').html("");
    RefreshValidation();
}



/* Rpt_ERPRoleJobMappingReport Page Crated By Nabin*/
function OnClickRoleFormAccessReport(e) {

    var token = $("#Rpt_RoleFormAccessReport input").val();
    if (GetFormValidate($('form'), "btnGenerateAccessControlReport")) {
        $.ajax({
            type: 'POST',
            url: ResourceRptRoleFormAccessRepot.urlPath_GetRoleFormAccessReport,
            dataType: 'json',
            data: {
                __RequestVerificationToken: token,
                Role: $('#RoleName').data("kendoMultiSelect").value()
            },
            success: function (result) {

                var columnslist = [];

                if (result != 0 && result.length > 0) {

                    columnslist.push({ field: "RoleName", title: "Role Name", editable: false, width: 140 });
                    columnslist.push({ field: "FormName", title: "Form Name", editable: false, width: 140 });
                    columnslist.push({ field: "View", title: "View", editable: false, width: 140 });
                    columnslist.push({ field: "Add", title: "Add", editable: false, width: 140 });
                    columnslist.push({ field: "Modify", title: "Modify", editable: false, width: 140 });
                    columnslist.push({ field: "Delete", title: "Delete", editable: false, width: 140 });
                    $("#divRoleFormAccess").css("width", "1200");
                    $("#divRoleFormAccess").css("height", "300");
                    $('#divRoleFormAccess').html("");

                    var dataSource = new kendo.data.DataSource({
                        pageSize: 10,
                        data: result,
                        autoSync: true,
                        schema: {
                            model: {
                                id: "AdjustID"
                            }
                        }
                    });

                    $("#divRoleFormAccess").kendoGrid({
                        dataSource: dataSource,
                        height: 300,
                        sortable: true,
                        pageable: {
                            refresh: true,
                            pageSizes: [5, 10, 20, 50, 100, 200, 300, "all"],
                            buttonCount: 5
                        },

                        reorderable: true,
                        autoBind: true,
                        columns: columnslist,
                        toolbar: ["excel", "search"],
                        excel: {
                            fileName: "Export.xlsx",
                            // proxyURL: ResourceRptRoleFormAccessRepot.urlPath_Excel_Export_Save,
                            filterable: true,
                            allPages: true
                        },


                    })
                }

                else {

                    $("#divRoleFormAccess").css("width", "0");
                    $("#divRoleFormAccess").css("height", "0");
                    $('#divRoleFormAccess').html("");
                    jAlert(ResourceRptRoleFormAccessRepot.display_RecordNotFound);
                }
            }





        });
    }
    else {
        $('form').submit();
    }
}

function OnClickRoleFormAccessRefresh(e) {

    //ClientID: $('#ClientName').val(),
    //ProcessID: $('#ProcessName').val(),
    //RoleID: $('#RoleName').val()
    //Client
    //var ddlJob = $("#Job").data("DropDownList");
    // $("#Job").data("kendoDropDownList").select(0);
    //var multiselectProcess = $("#ProcessName").data("kendoMultiSelect");
    var multiselectRole = $("#RoleName").data("kendoMultiSelect");

    //multiselectClient.dataSource.filter({}); //clear applied filter before setting value
    //multiselectClient.value("");

    //multiselectProcess.dataSource.filter({}); //clear applied filter before setting value
    //multiselectProcess.value("");

    multiselectRole.dataSource.filter({}); //clear applied filter before setting value
    multiselectRole.value("");


    ////Clears Grid
    $("#divRoleFormAccess").css("width", "0");
    $("#divRoleFormAccess").css("height", "0");
    $('#divRoleFormAccess').html("");
    RefreshValidation();
}


function OnClickProcessOwnerRequestStatus(e) {
    debugger;
    if (GetFormValidate($('form'), "btnProcessOwnerRequestStatus")) {
        var Params = {
            StartDate: $("#StartDate").val(),
            EndDate: $("#EndDate").val(),
            Status: $("#Status").val()
        };
        $.LoadingOverlay("show");
        $.ajax({
            type: 'POST',
            url: Resource.urlPath_GetProcessOwnerApprovalReport,
            dataType: 'json',
            data: Params,
            success: function (result) {
                $.LoadingOverlay("hide");
                if (result.length < 1) {
                    jAlert(Resource.display_RecordNotFound);
                    return false;
                }

                var columnslist = [];
                columnslist.push({ field: "RequestId", title: "Request Id", editable: false, width: 140 });
                columnslist.push({ field: "ClientName", title: "Client Name", editable: false, width: 140 });
                columnslist.push({ field: "ProcessName", title: "Process Name", editable: false, width: 140 });
                columnslist.push({ field: "Creater", title: "Creater", editable: false, width: 140 });
                columnslist.push({ field: "Approver", title: "Approver", editable: false, width: 140 });
                columnslist.push({ field: "Foruser", title: "For User", editable: false, width: 140 });
                columnslist.push({ field: "TransStatus", title: "Transaction Status", editable: false, width: 140 });
                if (result.length > 0) {

                    $("#gridProcessOwnerRequestStatus").css("width", "100%");
                    $("#gridProcessOwnerRequestStatus").css("height", "300");
                    $('#gridProcessOwnerRequestStatus').html("");

                    var dataSource = new kendo.data.DataSource({
                        pageSize: 10,
                        data: result,
                        autoSync: true,
                        schema: {
                            model: {
                                id: "ReuquestId"
                            }
                        }
                    });

                    $("#gridProcessOwnerRequestStatus").kendoGrid({
                        dataSource: dataSource,
                        height: 300,
                        sortable: true,
                        pageable: {
                            refresh: true,
                            pageSizes: [5, 10, 20, 50, 100, 200, 300, "all"],
                            buttonCount: 5
                        },
                        scrollable: true,
                        reorderable: true,
                        autoBind: true,
                        columns: columnslist,
                        toolbar: ["excel", "search"],
                        excel: {
                            fileName: "Export.xlsx",
                            //proxyURL: Resource.urlPath_Excel_Export_Save,
                            filterable: true,
                            allPages: true
                        },
                    })
                }
            }
            , error: function (error) {
                $.LoadingOverlay("hide");
            }
        });
    }
    else {
        $('form').submit();
    }
}


function OnClickProcessOwnerReport(e) {
    debugger
    if (GetFormValidate($('form'), "btnProcessOwnerReport")) {
        var Params = {
            ClientID: $("#ClientName").val(),
            ProcessID: $("#ProcessName").val(),
        };

        $.LoadingOverlay("show");
        $.ajax({
            type: 'POST',
            url: ResourceLayout.partialURL + "GetProcessOwnerReport",
            dataType: 'json',
            data: {
                ClientID: $("#ClientName").val(),
                ProcessID: $("#ProcessName").val(),
            },
            success: function (result) {
                $.LoadingOverlay("hide");
                if (result.length < 1) {
                    jAlert(Resource.display_RecordNotFound);
                    return false;
                }

                var columnslist = [];

                columnslist.push({ field: "ClientName", title: "Client Name", editable: false, width: 140 });
                columnslist.push({ field: "ProcessName", title: "Process Name", editable: false, width: 140 });
                columnslist.push({ field: "UserName", title: "User Name", editable: false, width: 140 });
                columnslist.push({ field: "EmpID", title: "Employee Id", editable: false, width: 140 });
                columnslist.push({ field: "ProcessOwner", title: "Process Owner", editable: false, width: 140 });

                if (result.length > 0) {

                    $("#gridProcessOwnerReport").css("width", "100%");
                    $("#gridProcessOwnerReport").css("height", "300");
                    $('#gridProcessOwnerReport').html("");

                    var dataSource = new kendo.data.DataSource({
                        pageSize: 10,
                        data: result,
                        autoSync: true,
                        schema: {
                            model: {
                                id: "EmpID"
                            }
                        }
                    });

                    $("#gridProcessOwnerReport").kendoGrid({
                        dataSource: dataSource,
                        height: 300,
                        sortable: true,
                        pageable: {
                            refresh: true,
                            pageSizes: [5, 10, 20, 50, 100, 200, 300, "all"],
                            buttonCount: 5
                        },
                        scrollable: true,
                        reorderable: true,
                        autoBind: true,
                        columns: columnslist,
                        toolbar: ["excel", "search"],
                        excel: {
                            fileName: "Export.xlsx",
                            // proxyURL: Resource.urlPath_Excel_Export_Save,
                            filterable: true,
                            allPages: true
                        },
                    })
                }
            }
            ,
            error: function (error) {
                $.LoadingOverlay("hide");
            }
        });
    }
    else {
        $('form').submit();
    }


}

$(document).on("click", "#btnProcessOwnerRequestStatusClear", function () {
    //debugger;
    var curDate = $("#CurrentDate").val();
    $("#StartDate").val(curDate);
    $("#EndDate").val(curDate);
    $("#Status").data('kendoDropDownList').value("");
    //Clears Grid
    $("#gridProcessOwnerRequestStatus").css("width", "0");
    $("#gridProcessOwnerRequestStatus").css("height", "0");
    $('#gridProcessOwnerRequestStatus').html("");
    RefreshValidation();
});


//$("#btnUserAccessRequestStatusReport").on("click", function () {
function OnbtnUserAccessRequestStatusReport() {

    //if ($("#RequestedFor").val() == "") {
    //    jAlert(display_PleaseSelectRequestedFor);
    //    return false;
    //}

    //if ($("#RequestedBy").val() == "") {
    //    jAlert(display_PleaseSelectRequestedBy);
    //    return false;
    //}
    if (GetFormValidate($('form'), "btnUserAccessRequestStatusReport")) {

        var param = {
            StartDate: $('#StartDate').val(),
            EndDate: $('#EndDate').val(),
            RequestedFor: RequestedForVal,
            RequestedBy: RequestedByVal,
            RequestedStatus: $('#Status').val()
        }
        
        $.ajax({
            async: false,
            type: 'POST',
            url: ResourceRptUserAccessRequestStatusReport.urlPath_GetUserAccessRequestStatusReport,
            dataType: 'json',
            data: param,
            success: function (result) {
                if (result != null) {
                    var columnslist = [];
                    if (result.length > 0) {
                        columnslist.push({ field: "RequestId", editable: false, hidden: false, width: 100 });
                        columnslist.push({ field: "RequestedFor", title: "Requested For", editable: false, width: 140 });
                        columnslist.push({ field: "RequestedBy", title: "Requested By", editable: false, width: 140 });
                        columnslist.push({ field: "RequestedOn", title: "Requested On", editable: false, width: 140 });
                        columnslist.push({ field: "Approver", title: "Approver", editable: false, width: 140 });
                        columnslist.push({ field: "RequestStatus", title: "Request Status", editable: false, width: 140 });
                        columnslist.push({ field: "RequestDesc", title: "Request Description", editable: false, width: 140 });

                        if (result.length > 0) {
                            $("#gridUserAccessRequestStatusReport").css("width", "100%");
                            $("#gridUserAccessRequestStatusReport").css("height", "1000");
                            $('#gridUserAccessRequestStatusReport').html("");

                            var dataSource = new kendo.data.DataSource({
                                data: result,
                                autoSync: true,
                                pageSize: 10,
                                schema: {
                                    model: {
                                        id: "RequestId"
                                    }
                                }
                            });

                            $("#gridUserAccessRequestStatusReport").kendoGrid({
                                dataSource: dataSource,
                                height: 450,
                                sortable: true,
                                reorderable: true,
                                autoBind: true,
                                columns: columnslist,
                                pageable: {
                                    refresh: true,
                                    pageSizes: [5, 10, 20, 50, 100, 200, 300, "all"],
                                    buttonCount: 5
                                },

                                toolbar: ["excel", "search"],
                                excel: {
                                    fileName: "Export.xlsx",
                                    // proxyURL: ResourceRptUserAccessRequestStatusReport.urlPath_Excel_Export_Save,
                                    filterable: true,
                                    allPages: true
                                },
                            })
                        }
                    }
                    else { jAlert(ResourceRptUserAccessRequestStatusReport.display_RecordNotFound); }

                } else {

                    $("#gridUserAccessRequestStatusReport").css("width", "0");
                    $("#gridUserAccessRequestStatusReport").css("height", "0");
                    $('#gridUserAccessRequestStatusReport').html("");
                    var validator = $('form').kendoValidator().data("kendoValidator");
                    //hide the validation messages when hide button is clicked
                    validator.hideMessages();
                    jAlert(ResourceRptUserAccessRequestStatusReport.display_RecordNotFound);
                }


            }

        });
    }
    else {
        $('form').submit();
    }
}

//$("#btnProcessOwnerReportClear").on("click", function () {
$(document).on('click', "#btnProcessOwnerReportClear", function () {
    var dropDownClientName = $("#ClientName").data("kendoDropDownList");
    var dropDownProcessName = $("#ProcessName").data("kendoDropDownList");


    dropDownClientName.select(0);
    dropDownProcessName.select(0);
    //Clears Grid
    $("#gridProcessOwnerReport").css("width", "0");
    $("#gridProcessOwnerReport").css("height", "0");
    $('#gridProcessOwnerReport').html("");
    RefreshValidation();

});


$(document).on("click", "#btnUserLogReportClear", function () {
    // window.location.href = "/Reports/Rpt_UserAccessRequestStatusReport";
    $("#RequestedBy").val('');
    $("#RequestedFor").val('');
    //var dropDownStatus = $("#Status").data("kendoDropDownList");
    //dropDownStatus.select(0);
    $("#SeverityFlag").data('kendoDropDownList').value("");
    var validator = $('form').kendoValidator().data("kendoValidator");
    var StartDate = $("#StartDate").data("kendoDatePicker");
    var EndDate = $("#EndDate").data("kendoDatePicker");
    var dt = new Date();
    StartDate.max(dt);
    StartDate.value(dt);
    StartDate.trigger("change");
    EndDate.min(dt);
    EndDate.value(dt);
    EndDate.trigger("change");
    ////Clears Grid
    var grid = $("#gridError").data("kendoGrid")
    grid.dataSource._destroyed = [];
    grid.dataSource.read();
    validator.hideMessages();
});


function RefreshValidation() {
    var validator = $('form').kendoValidator().data("kendoValidator");
    //hide the validation messages when hide button is clicked
    validator.hideMessages();
}
function startChange() {

    var endPicker = $("#EndDate").data("kendoDatePicker"),
        startDate = this.value();

    if (startDate) {
        startDate = new Date(startDate);
        startDate.setDate(startDate.getDate());
        $("#EndDate").val($("#StartDate").val());
        endPicker.min(startDate);
    }
}

function endChange() {

    var startPicker = $("#StartDate").data("kendoDatePicker"),
        endDate = this.value();

    if (endDate) {
        endDate = new Date(endDate);
        endDate.setDate(endDate.getDate());
        // startPicker.max(endDate);
    }
}


var MessageDetails;
function click_logReports() {
    if ($('#SeverityFlag').val() != '') {
        var grid = $("#gridError").data("kendoGrid")
        grid.dataSource._destroyed = [];
        grid.dataSource.read();
    }
    else {
        jAlert('Please select Type');
    }

    //        detailsTemplate = kendo.template($("#template").html());
    //var param = {
    //    StartDate: $('#StartDate').val(),
    //    EndDate: $('#EndDate').val(),
    //    MachineName: ''
    //}


    //$.ajax({
    //    async: false,
    //    url: ResourceReport.urlPath_GetExceptionHandlerReport,
    //    data: param,
    //    type: 'Post',
    //    cache: false,
    //    success: function (data) {

    //        var grid = $('#grid').data("kendoGrid");
    //        if (grid != undefined) {
    //            grid.destroy();
    //        }
    //        GenerateGrdError(data)
    //       // Changecolor();
    //        wnd = $("#detailsError")
    //            .kendoWindow({
    //                title: "Error Message Details",
    //                modal: true,
    //                visible: false,
    //                resizable: false,
    //                width: 1000
    //            }).data("kendoWindow");

    //        detailsTemplate = kendo.template($("#template").html());



    //    }



    //});


}
function GenerateGrdError(BindDataList) {

    $("#gridError").kendoGrid({
        pageable: {
            refresh: true,
            pageSizes: [5, 10, 20, 50, 100],
            buttonCount: 5
        },
        sortable: {
            mode: "single",
            allowUnsort: false
        },
        dataSource: {

            transport: {
                read: function (e) {
                    e.success(BindDataList);
                }
            },

            pageSize: 20,
            schema: {
                model: {
                    id: "LogID",
                    fields: {
                        LogID: { editable: false }, // the id field is not editable
                        Severity: { editable: false },
                        Timestamp: { type: "string" },
                        MachineName: { type: "string" },
                        Message_cut: { type: "string" },
                        AppDomainName: { type: "string" },
                        ProcessID: { type: "string" },
                        ProcessName: { type: "string" },
                        ThreadName: { type: "string" },
                        Win32ThreadId: { type: "string" },
                        FormattedMessage: { type: "string" }


                    }
                }
            }
        },
        dataBound: onDataBound,
        columns: [

            { field: "Timestamp", title: "Timestamp", width: "140px" },
            { field: "MachineName", title: "MachineName", width: "140px" },
            { command: { text: "View Error Message", click: showErrorDetails }, title: "View Message", width: "140px" },
            { field: "Message_cut", title: "Message", width: "350px" },
            { field: "ProcessID", title: "ProcessID", width: "100px" },
            { field: "Win32ThreadId", title: "Win32ThreadId", width: "140px" },
            { field: "FormattedMessage", title: "FormattedMessage", width: "140px" }

        ],

    });
}


function showErrorDetails(e) {
    var detailsTemplate = kendo.template($("#template").html());


    e.preventDefault();

    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    $.ajax({
        async: false,
        url: ResourceReport.urlPath_GetExceptionHandlerReport_Message,
        data: { LogID: dataItem.LogID },
        type: 'Post',
        cache: false,
        success: function (data) {
            MessageDetails = data;
            //kendo.ui.progress($('#frmdErrorStatus'), false);
        }
    });
    var wnd = $("#detailsError")
        .kendoWindow({
            title: "Error Message Details",
            modal: true,
            visible: false,
            resizable: false,
            width: 1000
        }).data("kendoWindow");

    wnd.content(detailsTemplate(dataItem));
    wnd.center().open();
    //kendo.ui.progress($('#frmdErrorStatus'), true);

    //e.preventDefault();

    //var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    //$.ajax({
    //    async: false,
    //    url: ResourceReport.urlPath_GetExceptionHandlerReport_Message,
    //    data: { LogID: dataItem.LogID },
    //    type: 'Post',
    //    cache: false,
    //    success: function (data) {
    //        MessageDetails = data;
    //        kendo.ui.progress($('#frmdErrorStatus'), false);
    //    }
    //});

    //wnd.content(detailsTemplate(dataItem));
    //wnd.center().open();

}