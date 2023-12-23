/*
     * FileName: Process-1.8.2.js
     * ClassName: Process-1.8.2
     * Purpose: This file contain all client side scripting for Power User View
     * Description:  
     * Created By: meharban ali
     * Created Date: 22 June 2015
     * Modified By: 
     * Modified Date:
     * Modified Purpose:
     * Modified Description: 
 */
/* Make cursor poiter */
$(".k-select").css("cursor", "pointer");

/* Ope Bussiness justification popup*/
function OpenBusinessJustifications() {
    try {
        var accessWindow = $("#ERPPopup").kendoWindow({
            actions: ["Maximize", "Close"],
            draggable: true,
            modal: true,
            pinned: true,
            visible: true,
            resizable: true,
            title: "Process Window",
            height: "500px",
            width: "800px"
        });
        $("#ERPPopup").data("kendoWindow").center().open();
    }
    catch (err) {
        alert(err.message);
    }
    $("#ErpPopupGrid").find(".chkbxErpPopupGrid").attr("checked", false);
    $("#ErpPopupGrid").find(".checkbox").attr("checked", false);
    var addedProcess = [];
    $("#dprocess table > tbody > tr").each(function () {
        var Bel = $(this).find(".DelteRowProcess");
        if ($(Bel).attr('ierpprocessid')) {
            addedProcess.push($(Bel).attr('ierpprocessid'));
        }
    });


    $("#ProcessERPMapping table > tbody > tr").each(function () {
        var Cel = $(this).find("[type=checkbox]");
        let match = "U";
        addedProcess.forEach(function (element) {
            if ($(Cel).val() === element) {
                match = "C";
            }
        });

        if (match == "U") {

            $(Cel).prop("checked", false);
        }
        if (match == "C") {

            $(Cel).prop("checked", true);
        }

    });
    $('.k-scrollbar').prepend('<div style="width:1px; height:70309px"></div >');
    // document.getElementsByClassName("k-scrollbar").innerHTML = '<div style="width:1px; height:70309px"></div >';
}

/* Return Selected Client Value */
function GetClientSelectedValue() {
    return {
        iClientID: $("#ClientName").val()
    };
}


/* Return Selected Client Value */
function OnSelectClient(e) {
    $("#ddl").data("kendoDropDownList").dataSource.read();
}

/* On Click Add Process */
function OnClickAddProcessMaster() {
    var ErpCheck = [];
    $("#ProcessERPMapping table > tbody > tr").each(function () {
        var el = $(this).find("[type=checkbox]");
        if ($(el).is(":checked")) {
            ErpCheck.push(el.val());
        }
    });
    var col = [];
    col.push({ field: 'iProcessGroupID', hidden: true, });
    col.push({ field: 'oERPProcess.iERPCode', title: 'ERP Code', width: 100 });
    col.push({ field: 'oERPProcess.sName', title: 'ERP Name', encoded: false, width: 250 });
    col.push({ field: "", title: "", template: "<input type='button' value='DELETE' class='k-primary DelteRowProcess' iLoc='#=oERPProcess.oLocation.iLocationID#' iERPProcessID='#=oERPProcess.iERPProcessID#' iProcessGroupID='#=iProcessGroupID#' >", width: 50 });
    //var col1 = [];
    //col1.push({ field: 'oLocation.iLocationID', hidden: true });
    //col1.push({ field: 'iProcessFTEID', hidden: true });
    //col1.push({ field: 'oLocation.sLocationName', title: 'Location', attributes: { "disabled": "disabled" }, editable: true, width: 100 });
    //col1.push({ field: 'iFTE', title: 'FTE', width: 100, attributes: { "cName": "iFTE", 'iLoc': '#=oLocation.iLocationID#', "iFTE": "#=iProcessFTEID#", "class": "clsCell" } });
    //col1.push({ field: 'iQCACount', title: 'QCACount', width: 100, attributes: { "cName": "QCACount", 'iLoc': '#=oLocation.iLocationID#', "iFTE": "#=iProcessFTEID#", "class": "clsCell" } });
    //col1.push({ field: 'dtEffectiveStartDate', template: "#=kendo.toString(dtEffectiveStartDate,'MM-dd-yyyy')#", title: 'Eff. Start Date', editable: true, width: 100, attributes: { "cName": "dtEffectiveStartDate", 'iLoc': '#=oLocation.iLocationID#', "iFTE": "#=iProcessFTEID#", "class": "clsCell" } });
    //col1.push({ field: 'dtEffectiveEndDate', template: "#=kendo.toString(dtEffectiveEndDate,'MM-dd-yyyy')#", title: 'Eff. End Date', editable: true, width: 100, attributes: { "cName": "dtEffectiveEndDate", 'iLoc': '#=oLocation.iLocationID#', "iFTE": "#=iProcessFTEID#", "class": "clsCell" } });
    $.ajax({
        url: ResourceProcess.urlPath_fillErpGroupGridRead,
        type: 'POST',
        data: { iERPProcessIDList: ErpCheck },
        cache: false,
        datatype: "json",
        success: function (result) {
            $("#ERPPopup").data("kendoWindow").close();
            if (result.lProcessGroup != null) {
                var processList = [];
                for (var i = 0; i < result.lProcessGroup.length; i++) {
                    processList.push(result.lProcessGroup[i]);
                }
                var processgrid = $("#GridErpGroup").getKendoGrid();
                processgrid.destroy();
                $("#GridErpGroup").empty()
                    .kendoGrid({
                        dataSource: { data: processList },
                        groupable: false,
                        sortable: false,
                        scrollable: true,
                        pageable: false,
                        columns: col
                    });
                //  var locationList = new Array();
                // var arrlocationList = [];
                //$.each(result.lProcessFTE, function (index, value) {
                //    value.dtEffectiveStartDate = (new Date().getMonth() + 1) + "/" + new Date().getDate() + "/" + new Date().getYear();
                //    value.dtEffectiveEndDate = (new Date().getMonth() + 1) + "/" + new Date().getDate() + "/" + new Date().getYear();
                //});
                //var locationGrid = $("#GridProcessEtE").getKendoGrid();
                //$("#GridProcessEtE").empty()
                //    .kendoGrid({
                //        dataSource: {
                //            data: result.lProcessFTE
                //        },
                //        groupable: false,
                //        sortable: false,
                //        scrollable: true,
                //        pageable: false,
                //        editable: true,
                //        schema: col1,
                //        columns: col1
                //    });
            }
        },
        error: function (err) {
            console.warn(err);
        }
    });
}

/* On check box click inside ErpPopupGrid */
$(".chkbxErpPopupGrid").on("click", function () {
    if ($(this).is(":checked")) {
        $(".checkbox").prop("checked", "checked");
    }
    else {
        $(".checkbox").prop("checked", false);
    }
});

/* On Click of the search button */
function OnBtnSearchClickProcessMaster(e) {
    var grid = $("#ErpPopupGrid").data("kendoGrid")
    grid.dataSource.read();
}

/* Get filter Values*/
function GridFillterValueProcessMaster() {
    return {
        iLocation: $('#Location').val(),
        ErpProcessName: $('#ERPProcess').val()
    };
}



/* tabstrip fade effect*/
$(document).ready(function () {
    $("#tabstrip").kendoTabStrip({
        animation: {
            open: {
                effects: "fadeIn"
            }
        }
    });
});

/* On Click New Process */
function OnClickNewProcessMaster() {
    window.location.href = ResourceProcess.urlPath_Index;
}


function OnClickViewProcessMaster() {
    window.location.href = ResourceProcess.urlPath_SearchView
}

function OnSuccessSavedProcess(ret) {

    if (ret == '1') {
        jAlert(ResourceProcess.Msg_ProcessSaved, 'Alert', function (ret) {
            window.location.href = ResourceProcess.urlPath_Index;
        });
    }
    else if (ret == '2') {
        jAlert(ResourceProcess.Msg_ProcessUpdated, 'Alert', function (ret) {
            window.location.href = ResourceProcess.urlPath_Index;
        });
        return false;
    }
    else if (ret == '-1') {
        jAlert(ResourceProcess.Msg_NoCalenderExist);
        return false;
    }
    else if (ret == '-2') {
        jAlert(ResourceProcess.Msg_PeocessCreatePermission);
        return false;
    }
    else if (ret == '-3') {
        jAlert(ResourceProcess.Msg_ProcessNameAlreadyExist);
        return false;
    }
    else if (ret == '-4') {
        jAlert("You cannot change Process Month Year. Because this process is Working on 1st Stage.!");
        return false;
    }
    else if (ret == '-5' || ret == '-8') {
        jAlert("Please chose Create New Option !");
        return false;
    }
    else if (ret == '-6' || ret == '-7') {
        jAlert("Please chose Amend Option!");
        return false;
    }
    else {
        jAlert(ret);
        return false;
    }

}

$(".clsCell").on("change", function () {
    var Params = {
        iLocationID: $(this).attr("iLoc"),
        iProcessFTEID: $(this).attr("iFTE"),
        columnName: $(this).attr("cName"),
        columnValue: $(this).val() == "" ? $(this).find('input').val() : $(this).val()
    }
    $.ajax({
        url: '/Process/UpdateProcessFTERowState',
        type: 'post',
        datatype: "json",
        data: { Params: Params },
        cache: false,
        success: function (result) {

        }
    });
});

//function OnSaveButtonClickProcessMaster(event) {
//    
//    var validator = $("#frmProcessMaster").kendoValidator().data("kendoValidator");
//    event.preventDefault();
//    if (validator.validate()) {
//        $("#frmProcessMaster").submit();
//    }
//    else {
//        return false;
//    }
//}

/* Search Page Edite */
function editProcess(e) {

    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var token = $('input[name=__RequestVerificationToken]').val();// $("#frmProcessSearchView input").val();
    $.ajax({
        type: "POST",
        url: ResourceProcessSearch.urlPath_SetEditableProcessId,// '@Url.Action("SetEditableProcessId", "Process")',
        data: {
            __RequestVerificationToken: token,
            tempProcessId: dataItem.iProcessID
        },
        dataType: "json",
        success: function (data) {
            if (true) {
                window.location.href = ResourceProcessSearch.urlPath_Index;// "@Url.Action("Index", "Process")";
            }
        }
    });
}

/* Delete Process*/
function deleteProcess(e) {
    jConfirm(ResourceProcessSearch.confirm_DeleteProcess, 'Confirmation', function (r) {
        if (r) {

            e.preventDefault();
            var index = $(e.currentTarget).closest("tr")[0].rowIndex;
            var dataItem = $("#ProcessGrid").data("kendoGrid").dataItem($(e.currentTarget).closest("tr"));
            var token = $("#frmProcessSearchView input").val();
            ;
            $.ajax({
                type: "POST",
                url: ResourceProcessSearch.urlPath_SetEditableProcessId,// '@Url.Action("SetEditableProcessId", "Process")',
                data: {
                    __RequestVerificationToken: token,
                    tempProcessId: dataItem.iProcessID
                },
                dataType: "json",
                success: function () {
                    ;
                    $.ajax({
                        type: "POST"
                        , url: ResourceProcessSearch.urlPath_DeleteProcess
                        , data: { __RequestVerificationToken: token, iProcessID: dataItem.iProcessID }
                        , dataType: "json"
                        , success: function (result) {

                            if (result == ResourceProcessSearch.display_Ok) {

                                jAlert(ResourceProcessSearch.display_ProcessDeleted, "Alert", function () {
                                    $("#ProcessGrid  table > tbody tr:eq(" + index + ")").remove();
                                });
                            }

                        }
                        , error: function (jqXHR, textStatus, errorThrown) {
                            jAlert(errorThrown);
                        }

                    });
                }
            });

        }
        else {
            return false;
        }
    });
}
function GetProcessListParam() {

    return {
        clientName: $("#ClientId").val(),
        processName: $("#ProcessName").val()
    }
}


function OnClickNew() {

    window.location.href = ResourceProcessSearch.urlPath_Index
}
//function OnClickView() {

//    window.location.href = "/AppConfiguration/Process/SearchView"
//}

function OnClickClear() {

    $("#ClientId").data("kendoDropDownList").value(-1);
    $("#ProcessGrid").data("kendoGrid").dataSource.read();
}

function onRowBoundProcessSearch(e) {

    $(".k-grid-Edit").find("span").addClass("k-icon k-edit");
    $(".k-grid-Delete").find("span").addClass("k-icon k-delete");
}


$("#btnSearchProcess").on("click", function (e) {

    e.preventDefault();
    var validator = $("#frmProcessSearchView").kendoValidator().data("kendoValidator");

    if (validator.validate()) {

        $("#ProcessGrid").data("kendoGrid").dataSource.read();
    }
    else {
        return false;
    }

});
/*  Popup */
//Grid filter Values
function GridFillterValue() {
    return {
        iLocation: $('#Location').val(),
        ErpProcessName: $('#ERPProcess').val()

    };
}

//Get CheckBox list Values and fill Main Grid("GridErpGroup").
var iERPProcessIDList = null;



//Button Click refresh ErpPopupGrid
$("#btnSearch").click(function (e) {

    $("#ErpPopupGrid").data("kendoGrid").dataSource.read();
    $("#ErpPopupGrid").data("kendoGrid").refresh();
    e.preventDefault();
});

$(function () {
    $(document).on("click", ".DelteRowProcess", function () {


        var token = $('input[name=__RequestVerificationToken]').val();//added by indresh 22-08-2017
        //alert(token);
        //var token = $("#frmProcessIndex input").val();

        var DelID = $(this).attr('iERPProcessID');
        //var DelID = $(this).attr('ierpprocessid');

        //  var token = $("#form0 input").val();


        var ErpCheck = [];

        ErpCheck.push(DelID);
        var col = [];
        col.push({ field: 'iProcessGroupID', hidden: true, });
        col.push({ field: 'oERPProcess.iERPCode', title: 'ERP Code', width: 100 });
        col.push({ field: 'oERPProcess.sName', title: 'ERP Name', encoded: false, width: 250 });
        col.push({ field: "", title: "", template: "<input type='button' value='DELETE' class='k-primary DelteRowProcess' iLoc='#=oERPProcess.oLocation.iLocationID#' iERPProcessID='#=oERPProcess.iERPProcessID#' iProcessGroupID='#=iProcessGroupID#' >", width: 50 });
        var col1 = [];
        col1.push({ field: 'oLocation.iLocationID', hidden: true });
        col1.push({ field: 'iProcessFTEID', hidden: true });
        col1.push({ field: 'oLocation.sLocationName', title: 'Location', attributes: { "disabled": "disabled" }, editable: true, width: 100 });
        col1.push({ field: 'iFTE', title: 'FTE', width: 100, attributes: { "cName": "iFTE", 'iLoc': '#=oLocation.iLocationID#', "iFTE": "#=iProcessFTEID#", "class": "clsCell" } });
        col1.push({ field: 'iQCACount', title: 'QCACount', width: 100, attributes: { "cName": "QCACount", 'iLoc': '#=oLocation.iLocationID#', "iFTE": "#=iProcessFTEID#", "class": "clsCell" } });
        col1.push({ field: 'dtEffectiveStartDate', template: "#=kendo.toString(dtEffectiveStartDate,'MM-dd-yyyy')#", title: 'Eff. Start Date', editable: true, width: 100, attributes: { "cName": "dtEffectiveStartDate", 'iLoc': '#=oLocation.iLocationID#', "iFTE": "#=iProcessFTEID#", "class": "clsCell" } });
        col1.push({ field: 'dtEffectiveEndDate', template: "#=kendo.toString(dtEffectiveEndDate,'MM-dd-yyyy')#", title: 'Eff. End Date', editable: true, width: 100, attributes: { "cName": "dtEffectiveEndDate", 'iLoc': '#=oLocation.iLocationID#', "iFTE": "#=iProcessFTEID#", "class": "clsCell" } });
        $.ajax({
            url: ResourceProcess.urlPath_DeleteErpGroupGridRead,
            type: 'post',
            data: { __RequestVerificationToken: token, iERPProcessIDList: ErpCheck },
            cache: false,
            dataType: 'json',
            success: function (result) {
                //  $(this).parent().parent().remove();
                //$("#ERPPopup").data("kendoWindow").close();
                if (result.lProcessGroup != null) {
                    var processList = [];
                    for (var i = 0; i < result.lProcessGroup.length; i++) {
                        processList.push(result.lProcessGroup[i]);
                    }
                    var processgrid = $("#GridErpGroup").getKendoGrid();
                    processgrid.destroy();
                    $("#GridErpGroup").empty()
                        .kendoGrid({
                            dataSource: { data: processList },
                            groupable: false,
                            sortable: false,
                            scrollable: true,
                            pageable: false,
                            columns: col
                        });
                    var locationList = new Array();
                    var arrlocationList = [];
                    $.each(result.lProcessFTE, function (index, value) {
                        value.dtEffectiveStartDate = (new Date().getMonth() + 1) + "/" + new Date().getDate() + "/" + new Date().getYear();
                        value.dtEffectiveEndDate = (new Date().getMonth() + 1) + "/" + new Date().getDate() + "/" + new Date().getYear();
                    });
                    var locationGrid = $("#GridProcessEtE").getKendoGrid();
                    $("#GridProcessEtE").empty()
                        .kendoGrid({
                            dataSource: {
                                data: result.lProcessFTE
                            },
                            groupable: false,
                            sortable: false,
                            scrollable: true,
                            pageable: false,
                            editable: true,
                            schema: col1,
                            columns: col1
                        });
                }


            }
            , error: function (jqXHR, textStatus, errorThrown) {

                jAlert(errorThrown);
            }
        });
        // $(this).parent().parent().remove();
    });
});

function StabilizationstartChange() {

    var endPicker = $("#StabilizationEndDate").data("kendoDatePicker"),
        startDate = this.value();

    if (startDate) {
        startDate = new Date(startDate);
        startDate.setDate(startDate.getDate());
        $("#StabilizationEndDate").val($("#StabilizationStartDate").val());
        endPicker.min(startDate);
    }
}

function StabilizationendChange() {

    var startPicker = $("#StabilizationStartDate").data("kendoDatePicker"),
        endDate = this.value();

    if (endDate) {
        endDate = new Date(endDate);
        endDate.setDate(endDate.getDate());
        // startPicker.max(endDate);
    }
}
function PilotstartChange() {

    var endPicker = $("#PilotEndDate").data("kendoDatePicker"),
        startDate = this.value();

    if (startDate) {
        startDate = new Date(startDate);
        startDate.setDate(startDate.getDate());
        $("#PilotEndDate").val($("#PilotStartDate").val());
        endPicker.min(startDate);
    }
}

function PilotendChange() {

    var startPicker = $("#PilotStartDate").data("kendoDatePicker"),
        endDate = this.value();

    if (endDate) {
        endDate = new Date(endDate);
        endDate.setDate(endDate.getDate());
        //startPicker.max(endDate);
    }
}
function ProductionstartChange() {

    var endPicker = $("#ProductionEndDate").data("kendoDatePicker"),
        startDate = this.value();

    if (startDate) {
        startDate = new Date(startDate);
        startDate.setDate(startDate.getDate());
        $("#ProductionEndDate").val($("#ProductionStartDate").val());
        endPicker.min(startDate);
    }
}

function ProductionendChange() {

    var startPicker = $("#ProductionStartDate").data("kendoDatePicker"),
        endDate = this.value();

    if (endDate) {
        endDate = new Date(endDate);
        endDate.setDate(endDate.getDate());
        //startPicker.max(endDate);
    }
}
function OnClickAddData() {

    var iERPProcessID = "";
    var grid = $("#ErpPopupGrid").data("kendoGrid");
    var ErpCheck = [];
    grid.tbody.find("input:checked").closest("tr").each(function (index) {
        grid.select($(this));
        var dataItem = grid.dataItem($(this));
        dataItem.checkbox = 'true';
        ErpCheck.push(dataItem.iERPProcessID);
    });
    window.location.href = ResourceProcessViewPopup.urlPath_Index + "?iERPProcessIDList=" + ErpCheck
}