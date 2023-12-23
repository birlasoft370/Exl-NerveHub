

$(".k-select").css("cursor", "pointer");


$.validator.defaults.ignore = "";

function OpenBusinessJustifications() {
    try {
        var accessWindow = $("#ERPPopup").kendoWindow({
            actions: ["Custom", "Maximize", "Minimize", "Close"],
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

}

function GetClientSelectedValue() {
    return {

        iClientID: $("#ClientId").val()

    };
}

function OnSelectClient(e) {

    $("#ddl").data("kendoDropDownList").dataSource.read();

}

function OnClickAdd() {

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
    col.push({ field: "", title: "", template: "<input type='button' value='DELETE' class='k-grid-delete' iLoc='#=oERPProcess.oLocation.iLocationID#' iERPProcessID='#=oERPProcess.iERPProcessID#' iProcessGroupID='#=iProcessGroupID#' >", width: 50 });

    var col1 = [];

    col1.push({ field: 'oLocation.iLocationID', hidden: true });
    col1.push({ field: 'iProcessFTEID', hidden: true });
    col1.push({ field: 'oLocation.sLocationName', title: 'Location', attributes: { "disabled": "disabled" }, editable: true, width: 100 });
    col1.push({ field: 'iFTE', title: 'FTE', width: 100, attributes: { "cName": "iFTE", 'iLoc': '#=oLocation.iLocationID#', "iFTE": "#=iProcessFTEID#", "class": "clsCell" } });
    col1.push({ field: 'iQCACount', title: 'QCACount', width: 100, attributes: { "cName": "QCACount", 'iLoc': '#=oLocation.iLocationID#', "iFTE": "#=iProcessFTEID#", "class": "clsCell" } });
    col1.push({ field: 'dtEffectiveStartDate', template: "#=kendo.toString(dtEffectiveStartDate,'MM-dd-yyyy')#", title: 'Eff. Start Date', editable: true, width: 100, attributes: { "cName": "dtEffectiveStartDate", 'iLoc': '#=oLocation.iLocationID#', "iFTE": "#=iProcessFTEID#", "class": "clsCell" } });
    col1.push({ field: 'dtEffectiveEndDate', template: "#=kendo.toString(dtEffectiveEndDate,'MM-dd-yyyy')#", title: 'Eff. End Date', editable: true, width: 100, attributes: { "cName": "dtEffectiveEndDate", 'iLoc': '#=oLocation.iLocationID#', "iFTE": "#=iProcessFTEID#", "class": "clsCell" } });

    $.ajax({
        url: '/Process/fillErpGroupGridRead',
        type: 'post',
        data: { iERPProcessIDList: ErpCheck },
        cache: false,
        success: function (result) {


            $("#ERPPopup").data("kendoWindow").close();
            if (result.lProcessGroup != null) {

                var processList = [];
                for (var i = 0; i < result.lProcessGroup.length; i++) {

                    processList.push(result.lProcessGroup[i]);

                }
                var processgrid = $("#GridErpGroup").getKendoGrid();

                processgrid.destroy();
                $("#GridErpGroup").empty() // clear the old HTML
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
                $("#GridProcessEtE").empty() // clear the old HTML
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
    });

}

$(".chkbx").on("click", function () {

    if ($(this).is(":checked")) {
        $(".checkbox").prop("checked", "checked");
    }
    else {
        $(".checkbox").prop("checked", false);
    }
});

function OnBtnSearchClick(e) {
    var grid = $("#ErpPopupGrid").data("kendoGrid")
    grid.dataSource.read()

}

function GridFillterValue() {
    return {
        iLocation: $('#Location').val(),
        ErpProcessName: $('#ERPProcess').val()
    };
}

function deleteProcess(e) {
    //$("#ERPPopup").data("kendoWindow").close(); 
}

$(document).ready(function () {
    $("#tabstrip").kendoTabStrip({
        animation: {
            open: {
                effects: "fadeIn"
            }
        }
    });

});

function OnClickNew() {
    window.location.href = "/AppConfiguration/Process/Index"
}

function OnClickView() {

}

function GetParameter() {

    return { iERPProcessIDList: ErpCheck }
}

$(".k-grid-delete").on("click", function () {

    var iProcessGroupID = $(this).attr("iProcessGroupID");
    var iERPProcessID = $(this).attr("iERPProcessID");
    var iLocationID = $(this).attr("iLocationID");
    var remvEl = this;

    jConfirm("Do you real want to remove selected ERP", "Confirmation", function (r) {

        if (r) {
            $.ajax({
                url: "/Process/DeleteProcessGroup?iProcessGroupID=" + iProcessGroupID + "&iERPProcessID=" + iERPProcessID + "&iLocationID=" + iLocationID,
                type: 'GET',
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function (result) {

                    if (result.isGroupDeleted) {
                        $(remvEl).parent().parent().remove();
                    }
                    if (result.isLocationDelete) {

                        var count = 0;
                        var item = $("#GridProcessEtE").data("kendoGrid").dataSource.data();

                        for (var i = 0 ; i < item.length; i++) {

                            if (item[i].iLocationID == iLocationID || item[i].oLocation.iLocationID == iLocationID) {

                                item.splice(i, 1);
                            }
                        }

                        $("#GridProcessEtE").data("kendoGrid").dataSource.data(item);
                        $("#GridProcessEtE").data("kendoGrid").refresh();
                    }

                    return false;
                },
                error: function (err) {

                }
            });
        }
    })


});

function OnClickView() {
    window.location.href = "/AppConfiguration/Process/SearchView"
}

function OnSuccess(ret) {

    if (ret == 1) {

        jAlert("Process Saved Successfully!", 'Alert', function (ret) {

            window.location.href = "/AppConfiguration/Process/SearchView";

        });
        return false;
    }
    else if (ret == -1) {
        jAlert("No Calender Information exists for this period! You need to define Calender First!");
        return false;
    }
    else if (ret == -2) {
        jAlert("You don't have permission to create Process with Process Type Organization!");
        return false;
    }
    else if (ret == -3) {
        jAlert("Process Name already exists!");
        return false;
    }
    else {
        jAlert("Process Not Created Successfully!, Error is " + ref);
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



function OnSaveButtonClick(event) {

    var validator = $("#form1").kendoValidator().data("kendoValidator");
    event.preventDefault();
    if (validator.validate()) {
        $("#form1").submit();
        return true;
    }
    else {
        return false;
    }
}

//$(".k-select").on("click", function () {
//    try {
//        var el = $(this).prev();
//        var datepicker = $(el).data("kendoDatePicker");
//        datepicker.open();
//    }
//    catch (err) {
//    }
//});
