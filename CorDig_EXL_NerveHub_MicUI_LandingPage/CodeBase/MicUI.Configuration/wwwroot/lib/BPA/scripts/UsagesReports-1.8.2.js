


var newitemtext = "ALL";
var selectedResolutionCode = [];
var newitemtext = "ALL";
var ChartData;
var GrdColumeList = [];
var lstSWMDQAALLData;
var lstSWMALLDATA;
var lstDQAALLDATA;
/* Returns client id */
 



/* Returns CampaignId */
 

/* Returns ProcessId */
 
function GetBatchParams() {

    return {
        iCampaignId: $("#iCampaignId").val(),
        FromDate: $("#FromDate").val(),
        ToDate: $("#ToDate").val()
    };
}

$("#FromDate,#ToDate").on("blur", function () {
    if ($("#iCampaignId").val() > 0 && $("#FromDate").val() != "" && $("#ToDate").val() != "") {
        BindBatchCode();
    }
});

$("#iCampaignId").on("change", function () {
     
    if ($("#iCampaignId").val() > 0 && $("#FromDate").val() != "" && $("#ToDate").val() != "" && $("#IsBackOffice").is(":checked") == false) {
        BindBatchCode();
    }
    var Cid = $("#iCampaignId").val();
    GenerateMultiSelectAll(Cid);
    $("#_DataCount").hide();
    $("#divGraphics").hide();
    $("#spnTotalRecords").text("0");
});

function BindBatchCode() {
    $("#BatchCode").data("kendoDropDownList").dataSource.read();
}

$("#AsOnCurrentDate").on("change", function () {
    if (this.checked) {
        $("#spnChoosedDate").hide();
    }
    else {
        $("#spnChoosedDate").show();
    }
});

 

function GetReportDataParam() {    
    var ret = {
        FromDate: $("#FromDate").val(),
        ToDate: $("#ToDate").val()
    };
    return ret;
}
$("#btnGenerateReport").on("click", function (event) {
    
    //checkSession();
    kendo.ui.progress($('#frmBatchStatusChart'), true);
    
   
    var validator = $("#frmBatchStatusChart").kendoValidator().data("kendoValidator");
    event.preventDefault();
    if (validator.validate()) {
        $.ajax({
            type: 'POST',
            url: Resources.url_ChartGetBatchStatusReportData,
            data: GetReportDataParam(),
            dataType: 'json',
            success: function (result) {  
                debugger;
                if (result == 'Session_Expire') {
                    kendo.ui.progress($('#frmBatchStatusChart'), false);
                    $("#_DataCount").hide();
                    $("#divGraphics").hide();
                    $("#spnTotalRecords").text("0");
                    $("#spnNotRecords").text("Records Not Found !");
                    jAlert("Session has been expired !");
                }
                else
                    if (result.lstSDALLDATA.length > 0) {
                        debugger;
                        
                        lstSWMALLDATA = null;
                        lstDQAALLDATA = null;
                        lstSWMALLDATA = result.lastSWMData
                        lstDQAALLDATA = result.lstDQAData;
                        grdshow(result.lstSDALLDATA);
                        grdshowSWM(result.lastSWMData);
                        grdshowDQA(result.lstDQAData);
                        kendo.ui.progress($('#frmBatchStatusChart'), false);
                        ChartData = result.lstSDALLDATA;
                         CreateBatchStatusChart(result);
                         CreateOutcomeBasedStatusChart(result);
                       
                        var tabToActivate = $("#TabstripChartsData-tab-1");
                        $("#TabstripChartsData").kendoTabStrip().data("kendoTabStrip").activateTab(tabToActivate);

                    }
                    else {
                        kendo.ui.progress($('#frmBatchStatusChart'), false);
                        $("#_DataCount").show();
                        $("#divGraphics").hide();
                        $("#spnTotalRecords").text("0");
                        $("#spnNotRecords").text("Records Not Found !");
                        // jAlert("Records Not Found !");
                    }
            }, error: function (xhr, status, thrownError) {
                kendo.ui.progress($('#frmBatchStatusChart'), false);
                if (xhr.responseText.indexOf("Session Expired") > 0) {
                    jAlert("Session has been expired !"); 
                } else {
                    //Other Exceptions/Errors
                    // $("#spnMessage").html(thrownError);
                }
            }
        }); 
        $("#divGraphics").show();
    }
    else {
        kendo.ui.progress($('#frmBatchStatusChart'), false);
        if ($("#lstResolutionCode_taglist li").length == 0) {
            $("#lstResolutionCode").next().show();
            return false;
        }
        else {
            $("#lstResolutionCode").next().hide();
        }
        return false;
    }
});

function grdshow(DataAllOneTime) {
    debugger;
    $("#GridViewReportData").kendoGrid({
        dataSource: {
            data: DataAllOneTime,
            schema: {
                model: {
                    fields: {
                        ReportName: { type: "string", editable: false },
                        TotalUser: { type: "string", editable: false },
                        ActiveUser: { type: "string", editable: false }, //CheckID
                        ActiveUserPer: { type: "string", editable: false },
                       


                    }
                }
            },
            pageSize: 20
        },
        toolbar: ["excel"],
        dataBound: function () {
            detailExportPromises = [];
        },
        excelExport: function (e) {
            // prevent saving the file because we will update the workbook
            e.preventDefault();

            var workbook = e.workbook;

            // Export all detail grids
            $("#GridViewReportData [data-role=grid]").each(function () {
                $(this).data("kendoGrid").saveAsExcel();
            });

            // wait for all detail grids to finish exporting
            $.when.apply(null, detailExportPromises).then(function () {
                // get the export results
                var detailExports = $.makeArray(arguments);

                /*
                   need to recalculate the master row indexes of each export
                   everytime you click expand in the master grid, the master row indexes are recalculated by Kendo
                   e.g. you expand row 4 (which will display a grid with 6 rows), then you expand row 1 (which will display a grid with 12 rows)
                   when you do this, the index of "row 4" is updated with 4 + 12. So the new index of "row 4" is actually 16
                */

                // first we need to create a copy of the original values, otherwise every subsequent export will have the already updated rows and it will try to update again
                for (var i = 0; i < detailExports.length; i++)
                    detailExports[i].newMasterRowIndex = detailExports[i].masterRowIndex;

                for (var i = 1; i < detailExports.length; i++) {
                    if (!detailExports[i].isDetailGrid2) {
                        for (var j = 0; j < i; j++) {
                            if (detailExports[i].newMasterRowIndex <= detailExports[j].newMasterRowIndex) {
                                detailExports[j].newMasterRowIndex = detailExports[j].newMasterRowIndex + detailExports[i].sheet.rows.length - 1; // -1 to discount header
                                if (detailExports[i].sheet.rows[detailExports[i].sheet.rows.length - 1].type == "footer")
                                    detailExports[j].newMasterRowIndex = detailExports[j].newMasterRowIndex - 1
                            }
                        }
                    }
                }

                // sort by masterRowIndex
                detailExports.sort(function (a, b) {
                    return a.newMasterRowIndex - b.newMasterRowIndex;
                });

                // merge the detail export sheet rows with the master sheet rows
                var rowCount = 0;
                for (var i = 0; i < detailExports.length; i++) {
                    // we need to recalculate the masterRowIndex everytime because the detailGrid2 should be inserted "in the middle" of the detailGrid
                    // and not simply after it as you would normally do when you have only two grids
                    var masterRowIndex = rowCount + detailExports[i].newMasterRowIndex + 1; // +1 to compensate for the header row

                    var sheet = detailExports[i].sheet;

                    if (sheet.rows[sheet.rows.length - 1].type == "footer")
                        sheet.rows.splice(-1, 1); // don't export the footer

                    if (detailExports[i].isDetailGrid2) {
                        // prepend two empty cells to each row
                        for (var ci = 0; ci < sheet.rows.length; ci++) {
                            if (sheet.rows[ci].cells[0].value) {
                                sheet.rows[ci].cells.unshift({});
                                sheet.rows[ci].cells.unshift({});
                            }
                            for (var cellIndex = 2; cellIndex < sheet.rows[ci].cells.length; cellIndex++) {
                                var colTitle = sheet.rows[ci].cells[cellIndex].value;
                                sheet.rows[ci].cells[cellIndex].background = "#6b6c6e";
                            }
                        }
                        rowCount = rowCount + sheet.rows.length;
                    }
                    else {

                        // prepend an empty cell to each row
                        for (var ci = 0; ci < sheet.rows.length; ci++) {
                            if (sheet.rows[ci].cells[0].value) {
                                sheet.rows[ci].cells.unshift({});
                            }
                            for (var cellIndex = 1; cellIndex < sheet.rows[ci].cells.length; cellIndex++) {
                                var colTitle = sheet.rows[ci].cells[cellIndex].value;
                                if (ci==0) {
                                    sheet.rows[ci].cells[cellIndex].background = "#6b6c6e";
                                }
                            }
                        }
                        rowCount = rowCount + 1;
                    }
                    // insert the detail sheet rows in the correct place
                    [].splice.apply(workbook.sheets[0].rows, [masterRowIndex + 1, 0].concat(sheet.rows));
                }

                // update the indexes of the rows so they are exported correctly
                for (var i = 0; i < workbook.sheets[0].rows.length; i++) {
                    workbook.sheets[0].rows[i].index = i;
                }

                // save the workbook
                kendo.saveAs({
                    dataURI: new kendo.ooxml.Workbook(workbook).toDataURL(),
                    fileName: "MonthlyUsageReport.xlsx"
                })
            });
        },
        excel: {
            fileName: "MonthlyUsageReport.xlsx",
            allPages: true
        },
        detailInit: detailInit,
        height: 450,
        scrollable: true,
        pageable: true,
          columns: [
            {
                  field: "ReportName",
                headerAttributes: {
                    style: "font-size: 12px;font-weight: bold; font-family: Poppins, sans-serif;color:#ffffff;background-color: #6c757d;"
                },
                  title: "Report Name",
                width: 50,
                filterable: false,

            },
           
            {
                field: "TotalUser",
                title: "Total User",
                headerAttributes: {
                    style: "font-size: 12px;font-weight: bold; font-family: Poppins, sans-serif;color:#ffffff;background-color: #6c757d"
                },
                width: 170,
                filterable: false,
 
            },
            {
                field: "ActiveUser",
                width: 170,
                title: "Active User",
                headerAttributes: {
                    style: "font-size: 12px;font-weight: bold; font-family: Poppins, sans-serif;color:#ffffff;background-color: #6c757d"
                },
                filterable: false,
                
            },




            {
                field: "ActiveUserPer",
                encoded: false,
                title: "Active User (%)",
                headerAttributes: {
                    style: "font-size: 12px;font-weight: bold; font-family: Poppins, sans-serif;color:#ffffff;background-color: #fd7e14"
                },
                width: 140,
                filterable: false,
            
            },
        ]
     


    });

}
function detailInit(e) {
    debugger;
    var values = e.data.ReportName;
    if (values == 'SWM') { lstSWMDQAALLData = lstSWMALLDATA; }
    else if (values == 'DQA') { lstSWMDQAALLData = lstDQAALLDATA; }
    var deferred = $.Deferred();

    // get the index of the master row
    var masterRowIndex = e.masterRow.index(".k-master-row");

    // add the deferred to the list of promises
    detailExportPromises.push(deferred);
    $("<div/>").appendTo(e.detailCell).kendoGrid({
        dataSource: {
            data: lstSWMDQAALLData,
            serverPaging: true,
            serverSorting: true,
            serverFiltering: true,
            pageSize: 10,
            filter: { field: "ReportName", operator: "eq", value: e.data.ReportName }
        },
        scrollable: false,
        sortable: true,
        pageable: true,
        columns: [
            {
                field: "ClientName", title: "Client Name", width: "210px", headerAttributes: {
                    style: "font-size: 12px;font-weight: bold; font-family: Poppins, sans-serif;color:#ffffff;background-color: #6c757d;"
                } },
            {
                field: "ProcessName", title: "Process Name", headerAttributes: {
                    style: "font-size: 12px;font-weight: bold; font-family: Poppins, sans-serif;color:#ffffff;background-color: #6c757d;"
                }, width: "210px" },
            {
                field: "TotalCount", title: "Total Count", headerAttributes: {
                    style: "font-size: 12px;font-weight: bold; font-family: Poppins, sans-serif;color:#ffffff;background-color: #6c757d;"
                }, width: "100px" },
            {
                field: "ActiveUserCount", title: "ACTIVE USER", headerAttributes: {
                    style: "font-size: 12px;font-weight: bold; font-family: Poppins, sans-serif;color:#ffffff;background-color: #6c757d;"
                }, width: "100px" }
        ],
        excelExport: function (e) {
            // prevent saving the file
            e.preventDefault();

            // resolve the deferred
            deferred.resolve({
                masterRowIndex: masterRowIndex,
                sheet: e.workbook.sheets[0]
            });
        },
    });
}
function grdshowSWM(DataAllOneTime) {
    debugger;
    $("#GridSWMViewReportData").kendoGrid({
        dataSource: {
            data: DataAllOneTime,
            schema: {
                model: {
                    fields: {
                        ClientName: { type: "string", editable: false },
                        TotalCount: { type: "string", editable: false },
                        ActiveUserCount: { type: "string", editable: false }, //CheckID
                        //ActiveUserPer: { type: "string", editable: false },



                    }
                }
            },
            pageSize: 20
        },
        toolbar: ["excel"],
        excel: {
            fileName: "MonthlyUsageReport.xlsx",
            allPages: true
        },
        height: 450,
        scrollable: true,
        pageable: true,
        columns: [
            {
                field: "ClientName",
                headerAttributes: {
                    style: "font-size: 12px;font-weight: bold; font-family: Poppins, sans-serif;color:#ffffff;background-color: #6c757d;"
                },
                title: "Client Name",
                width: 170,
                filterable: false,

            },

            {
                field: "TotalCount",
                title: "Total User",
                headerAttributes: {
                    style: "font-size: 12px;font-weight: bold; font-family: Poppins, sans-serif;color:#ffffff;background-color: #6c757d"
                },
                width: 90,
                filterable: false,

            },
            {
                field: "ActiveUserCount",
                width: 90,
                title: "Active User",
                headerAttributes: {
                    style: "font-size: 12px;font-weight: bold; font-family: Poppins, sans-serif;color:#ffffff;background-color: #6c757d"
                },
                filterable: false,

            },




            //{
            //    field: "ActiveUserPer",
            //    encoded: false,
            //    title: "Active User (%)",
            //    headerAttributes: {
            //        style: "font-size: 12px;font-weight: bold; font-family: Poppins, sans-serif;color:#ffffff;background-color: #fd7e14"
            //    },
            //    width: 140,
            //    filterable: false,

            //},
        ]



    });

}
function grdshowDQA(DataAllOneTime) {
    debugger;
    $("#GridDQAViewReportData").kendoGrid({
        dataSource: {
            data: DataAllOneTime,
            schema: {
                model: {
                    fields: {
                        ClientName: { type: "string", editable: false },
                        TotalCount: { type: "string", editable: false },
                        ActiveUserCount: { type: "string", editable: false }, //CheckID
                        //ActiveUserPer: { type: "string", editable: false },



                    }
                }
            },
            pageSize: 20
        },
        toolbar: ["excel"],
        excel: {
            fileName: "MonthlyUsageReport.xlsx",
            allPages: true
        },
        height: 450,
        scrollable: true,
        pageable: true,
        columns: [
            {
                field: "ClientName",
                headerAttributes: {
                    style: "font-size: 12px;font-weight: bold; font-family: Poppins, sans-serif;color:#ffffff;background-color: #6c757d;"
                },
                title: "Client Name",
                width: 170,
                filterable: false,

            },

            {
                field: "TotalCount",
                title: "Total User",
                headerAttributes: {
                    style: "font-size: 12px;font-weight: bold; font-family: Poppins, sans-serif;color:#ffffff;background-color: #6c757d"
                },
                width: 90,
                filterable: false,

            },
            {
                field: "ActiveUserCount",
                width: 90,
                title: "Active User",
                headerAttributes: {
                    style: "font-size: 12px;font-weight: bold; font-family: Poppins, sans-serif;color:#ffffff;background-color: #6c757d"
                },
                filterable: false,

            },




            //{
            //    field: "ActiveUserPer",
            //    encoded: false,
            //    title: "Active User (%)",
            //    headerAttributes: {
            //        style: "font-size: 12px;font-weight: bold; font-family: Poppins, sans-serif;color:#ffffff;background-color: #fd7e14"
            //    },
            //    width: 140,
            //    filterable: false,

            //},
        ]



    });

}
function generateColumns(response) {
    var ParametercumulativeValue = [];


    for (var i = 0; i < response.length; i++) {


        ParametercumulativeValue.push({ field: response[i].Cid, title: response[i].CName })

    }

    return ParametercumulativeValue;
}
$('#expand').click(function (e) {
    var grid = $("#GridViewReportData").data("kendoGrid");
    $(".k-master-row").each(function (index) {
        grid.expandRow(this);
    });
    e.preventDefault();
})

$('#collapse').click(function (e) {
    var grid = $("#GridViewReportData").data("kendoGrid");
    $(".k-master-row").each(function (index) {
        grid.collapseRow(this);
    });
    e.preventDefault();
})

function getRandomColor() {
    var letters = '0123456789ABCDEF'.split('');
    var color = '#';
    for (var i = 0; i < 6; i++) {
        color += letters[Math.floor(Math.random() * 16)];
    }
    return color;
}

function onDataBoundReportData() {

    setTimeout(function () {
        var grid = $("#GridViewReportData").data("kendoGrid");
        $("#ddlFilteredBy").data("kendoDropDownList").dataSource.data(grid.columns);
        var dropdownlist = $("#ddlFilteredBy").data("kendoDropDownList");
    }, 600);
    //setTimeout(function () {
    //    for (var i = 0 ; i < $("#GridViewReportData table tbody tr td").length; i++) {
    //        $("#GridViewReportData .k-grid-header-wrap").find("colgroup col").eq(i).width(150);
    //        $("#GridViewReportData .k-grid-content").find("colgroup col").eq(i).width(150);
    //    }
    //}, 200);     
}

function CreateBatchStatusChart(griddata) {

    var DQATotalUser = 0;
    var DQAActiveUser = 0;

   
    for (var i = 0; i < griddata.lstSDALLDATA.length; i++) {
        if (griddata.lstSDALLDATA[i].ReportName == 'DQA') {
            DQATotalUser = griddata.lstSDALLDATA[i].TotalUser;
            DQAActiveUser = griddata.lstSDALLDATA[i].ActiveUser;
        }
         
    }

    var data = [
        {
            "source": "Total User",
            "percentage": DQATotalUser,
            color: getRandomColor()
        },
        {
            "source": "Active User",
            "percentage": DQAActiveUser,
            color: getRandomColor()
        }
    ];

    $("#divBatchStatusChart").kendoChart({
        title: {
            text: "DQA"
        },
        legend: {
            position: "top",
            labels: {
                template: "#= text # (#= value #)"
            }
        },
        dataSource: {
            data: data
        },
      
        series: [{

            type: "pie",
            field: "percentage",
            categoryField: "source",
            explodeField: "explode"
        }],
        
        tooltip: {
            visible: true,
            template: "${ category } (${ value }) - ${ value }"
        }
    });

}

function CreateOutcomeBasedStatusChart(griddata) {

 


    var SWMTotalUser = 0;
    var SWMActiveUser = 0;


    for (var i = 0; i < griddata.lstSDALLDATA.length; i++) {
        if (griddata.lstSDALLDATA[i].ReportName == 'SWM') {
            SWMTotalUser = griddata.lstSDALLDATA[i].TotalUser;
            SWMActiveUser = griddata.lstSDALLDATA[i].ActiveUser;
        }

    }

    var data = [
        {
            "source": "Total User",
            "percentage": SWMTotalUser,
            color: getRandomColor()
        },
        {
            "source": "Active User",
            "percentage": SWMActiveUser,
            color: getRandomColor()
        }
    ];

    $("#divOutcomeBasedStatusChart").kendoChart({
        title: {
            text: "SWM"
        },
        legend: {
            position: "top",

            labels: {
                template: "#= text # (#= value #)"

            }
        },
        dataSource: {
            data: data
        },
        series: [{
            type: "pie",
            field: "percentage",
            categoryField: "source",
            explodeField: "explode"
        }],
        tooltip: {
            visible: true,
            template: "${ category } (${ value }) - ${ value }"
        }
    });

}
function printFieldNames() {
    var e = viewFields.getEnumerator();
    while (e.moveNext()) {
        var fieldName = e.get_current();
        console.log(fieldName);
    }
}
function CreateOutcomeBasedBarChart(filterBy, cln) {

    
    var seriesChartData = [];
    var arrTerminationName = [];
    var textTitle = Resources.display_BSR_OutcomeStatusBasedon + filterBy;
    var grid = $("#GridViewReportData").data("kendoGrid");
    var unAttempted = ChartData.length;
    var xAxisData = [];
    var results = ChartData;



    var columns1 = $("#GridViewReportData").data("kendoGrid").columns;
    for (var _i = 0; _i < results.length; _i++) {

        var celVal;
        var termVal;
        var columnsIn = results[_i];
        cln = cln == -1 ? 0 : cln;
        var col1 = columns1[cln];
        var columnsNames1 = col1.field;
        for (var key in columnsIn) {
            if (columnsNames1 == key) {
                var units = results[_i][columnsNames1];
                var ConvertDate = ConvertDateFormate(units, columnsNames1);
                if (ConvertDate != 0) {
                    celVal = ConvertDate;
                }
                else {
                    celVal = results[_i][columnsNames1];
                }
            }
        }

        var idx = $.inArray(celVal, xAxisData);
        if (idx == -1 && celVal != "") {
            xAxisData.push(celVal);
        }
    }

    for (var j = 0; j < ChartData.length; j++) {

        var idx = $.inArray(ChartData[j].ResolutionName == ("" || null) ? "Un Attempted" : ChartData[j].ResolutionName, arrTerminationName);
        if (idx == -1) {
            if (ChartData[j].ResolutionName == ("" || null)) {

                arrTerminationName.push("Un Attempted");
            }
            else {
                arrTerminationName.push(ChartData[j].ResolutionName);
            }
        }
    }

    dataChart_ = [];
    for (var j = 0; j < arrTerminationName.length; j++) {


        var arrAttrCount = [];

        var columns = $("#GridViewReportData").data("kendoGrid").columns;
        for (var k = 0; k < xAxisData.length; k++) {

            var attrCount = 0;
            var unAttempted = 0;

            for (var i = 0; i < results.length; i++) {
             //   debugger;
                var celVal;
                var termVal;
                var columnsIn = results[i];
                var col1 = columns[cln];
                var columnsNames1 = col1.field;
                var col2 = columns[8];
                var columnsNames2 = col2.field;
                for (var key in columnsIn) {
                    if (columnsNames1 == key) {
                        var units = results[i][columnsNames1];
                        var ConvertDate = ConvertDateFormate(units, columnsNames1);
                        if (ConvertDate != 0) {
                            celVal = ConvertDate;
                        }
                        else {
                            celVal = results[i][columnsNames1];
                        }
                    }
                    if (columnsNames2 == key) {
                        termVal = results[i][columnsNames2];
                    }
                }
                var xVal = xAxisData[k];
                var xTermVal = arrTerminationName[j];

              if(termVal == xTermVal && celVal == xVal) {
               //if (celVal == xVal) {
                  //  debugger;
                    attrCount++;
              }
                else if (xTermVal == "Un Attempted" && (termVal == "" || termVal == null) && celVal == xVal) {

                    unAttempted++;
                    attrCount = unAttempted;

                }
              //else if (xTermVal == "Un Attempted") {

              //    unAttempted++;
              //    attrCount = unAttempted;

              //}
            }
          //  debugger;
            arrAttrCount.push(attrCount);
            dataChart_.push({
                category: xVal,
                val: attrCount
            });



        }
       // debugger;
        seriesChartData.push({ name: arrTerminationName[j], data: arrAttrCount, color: getRandomColor() });
    }
    if (dataChart_.length <= 10) {
        MIN_SIZE = dataChart_.length;
        MAX_SIZE = dataChart_.length;
        viewSize = MIN_SIZE;
    }
    else {
        MIN_SIZE = 10;
        MAX_SIZE = 20;
        viewSize = MIN_SIZE;
    }
    CreateOutComeStatusBarCharBasedOn(seriesChartData, xAxisData, textTitle);

}
function ConvertDateFormate(strDate, columnsNames) {
    var units = strDate;
    var Resultval;
    if (units != null && ($.type(units) == 'date' || ($.type(units) == 'string' && units.indexOf('/Date(') == 0))) {

        var dd = kendo.parseDate(units);
        var tOff = dd.getTimezoneOffset();
        dd.setMinutes(dd.getMinutes() + (tOff));
        var currentTimeZone = $("#hdnCurrentOffset").val();
        var TZ = currentTimeZone.split("/");
        if (TZ != null && TZ != undefined)
            tzS = TZ[1];
        else tzS = "330";
        var tOffset = parseInt(tzS, 10);
        dd.setMinutes(dd.getMinutes() + (tOffset));
        var FormatPart = CultureDateFormat.split(' ');
        var ColumnFormate = CultureDateFormat;
        if (FormatPart != null && FormatPart != undefined && (columnwithOnlyDate != null || columnwithOnlyTime != null)) {
            if (columnwithOnlyDate.indexOf(columnsNames) >= 0) {
                ColumnFormate = FormatPart[0];
            }
            else if (columnwithOnlyTime.indexOf(columnsNames) >= 0 && FormatPart.length > 1) {
                ColumnFormate = "";
                for (var i = 1; i < FormatPart.length; i++) {
                    ColumnFormate += FormatPart[i] + " ";
                }
            }
        }
        Resultval = kendo.toString(dd, ColumnFormate);
    }
    else {
        Resultval = strDate;
    }
    return Resultval;
}
function CreateOutComeStatusBarCharBasedOn(seriesChartData, categoryAxisData, textTitle) {
    
    $("#divOutcomeChartBasedOn").kendoChart({
        renderAs: "canvas",
        title: {
            text: textTitle
        },
        legend: {
            position: "bottom"
        },
        seriesDefaults: {
            type: "column",
            stack: true
        },
        series: seriesChartData,
        valueAxis: {
            line: {
                visible: false
            }
        },
        categoryAxis: {
            categories: categoryAxisData,
            majorGridLines: {
                visible: false
            },
            labels: {
                rotation: "auto"
            }
        },
        tooltip: {
            visible: true,
            position: "top",
            template: "#= series.name #: #= value #"
        },
        transitions: false,
        drag: onDrag,
        dragEnd: onDragEnd,
        zoom: onZoom
    });
}
//function CreateOutComeStatusBarCharBasedOn(seriesChartData, categoryAxisData, textTitle) {
//    $("#divOutcomeChartBasedOn").kendoChart({
//        renderAs: "canvas",
//        title: {
//            text: textTitle
//        },
//        dataSource: {
//            data: seriesChartData,
//            pageSize: viewSize,
//            page: 0,
//            sort: { field: "val", dir: "desc" }
//        },
//        categoryAxis: {
//            // field: "category",
//            categories:categoryAxisData,
//            labels: { rotation: 90, padding: { right: 0 } },
//            majorGridLines: {
//                visible: true
//            }
//        },
//        valueAxis: {
//            name: "numeric",
//            line: {
//                visible: true
//            },
//            minorGridLines: {
//                visible: true
//            }
//        },
//        series: [{
//            type: "column",
//            field: "val"
//        }],
//        tooltip: {
//            visible: true,
//            position: "top",
//            template: "#= series.category #: #= value #"
//        },
//        transitions: false,
//        drag: onDrag,
//        dragEnd: onDragEnd,
//        zoom: onZoom
//    });
//}

$("#btnExportToExcel").click(function (e) {
    var grid = $("#GridViewReportData").data("kendoGrid");
    grid.saveAsExcel();
    e.preventDefault();
});

function ClickExportChart() {

    // Convert the DOM element to a drawing using kendo.drawing.drawDOM
    kendo.drawing.drawDOM($("#TabstripChartsData-1"))
        .then(function (group) {
            // Render the result as a PNG image
            return kendo.drawing.exportImage(group);
        })
        .done(function (data) {
            // Save the image file
            kendo.saveAs({
                dataURI: data,
                fileName: "HR-Dashboard.png"
                //proxyURL: "@Url.Action('ExportChartToImage', 'rptBatchStatus')"
            });
        });
}

$("#btnClearFilter").on("click", function (event) {
    window.location.href = Resources.url_Index;
});
function onCampaignChange() {
    
   // $("#iCampaignId").val(0);
    //BindBatchCode();
    //GenerateMultiSelectAll(0);
    //$("#_DataCount").hide();
    //$("#divGraphics").hide();
    //$("#spnTotalRecords").text("0");
}
function onProcessChange() {
    
    $("#iCampaignId").val(0);
    BindBatchCode();
    GenerateMultiSelectAll(0);
    $("#_DataCount").hide();
    $("#divGraphics").hide();
    $("#spnTotalRecords").text("0");
}

//$("#iprocessId").on("change", function () {
    
//});
function onClientChange() {
    
    $("#iCampaignId").val(0);
    BindBatchCode();
    GenerateMultiSelectAll(0);
    $("#_DataCount").hide();
    $("#divGraphics").hide();
    $("#spnTotalRecords").text("0");
}
//$("#iClientID").on("change", function () {
//    debugger;
//    $("#iCampaignId").val(0);
//    BindBatchCode();
//    GenerateMultiSelectAll(0);
//});



function startChange() {

    var endPicker = $("#ToDate").data("kendoDatePicker"),
        startDate = this.value();
    if (startDate) {
        startDate = new Date(startDate);
        startDate.setDate(startDate.getDate());
        endPicker.min(startDate);
    }
}

function endChange() {
    var startPicker = $("#FromDate").data("kendoDatePicker"),
        endDate = this.value();
    if (endDate) {
        endDate = new Date(endDate);
        endDate.setDate(endDate.getDate());
        startPicker.max(endDate);
    }
}



function GenerateMultiSelectAll(Cid) {

    var checkInputs = function (elements, FSA) {
        //  debugger;
        if (FSA == "SA") {
            SelectAllClick();
        }
        else if (FSA == "N") {
            DSelectAllClick();
        }
        else if (FSA == "NTSA") {

            DSelectAllClick();
            SelectAllClick();

        }
        //
        elements.each(function () {
            var element = $(this);
            var input = element.children("input");
            //debugger;
            if (FSA == "SA" || FSA == "NTSA") {
                input.prop("checked", true);
            }
            else {
                input.prop("checked", element.hasClass("k-state-selected"));
            }
        });
    };
    // var multi = $("#lstResolutionCode").data("kendoMultiSelect");
    var multiSelect = $("#lstResolutionCode").data("kendoMultiSelect");
    if (multiSelect != undefined) {
        var items = multiSelect.ul.find("li").find("input").attr("checked", false);
        FSA = "N";
        checkInputs(items, FSA);
        multiSelect.destroy();
        $("#MultiSelect").find(".k-multiselect-wrap").remove();
        $("#MultiSelect").prepend($("#lstResolutionCode")).find(".k-widget").remove();
        //multiSelect.destroy();
        $("#lstResolutionCode").empty();
        newitemtext = "ALL";
    }
    $('.k-multiselect-wrap').remove();
    $("#lstResolutionCode").empty().kendoMultiSelect({

        dataTextField: "sTermCodeName",
        dataValueField: "iTerminationCodeID",
        dataSource: {
            transport: {
                read: {
                    url: Resources.url_GetResolutionCodeList + "?CampaignId=" + Cid,
                    dataType: "json",
                }
            }
        },
        itemTemplate: " <input type='checkbox'/> #:data.sTermCodeName#",
        autoClose: false,
        dataBound: function () {
            //
            var items = this.ul.find("li");
            setTimeout(function () {
                checkInputs(items);
            });
        },
        change: function (e) {
            //
            var items;
            var FSA = 'N';
            var values = this.value();
            if (jQuery.inArray('All', this.value()) != -1) {
                if (values.length > 1) {
                    //FSA = "NTSA";
                    if (values.length == e.sender.listView._view.length) {
                        this.ul.find("li").find("input").attr("checked", false);
                        items = this.ul.find("li");
                    }
                    else {
                        this.ul.find("li").find("input").attr("checked", "checked");
                        items = this.ul.find("li");
                        FSA = "NTSA";
                    }
                }
                else
                    if (this._isSelect) {
                        this.ul.find("li").find("input").attr("checked", "checked");
                        items = this.ul.find("li");
                        FSA = "SA";
                    }
                    else {
                        this.ul.find("li").find("input").attr("checked", false);
                        items = this.ul.find("li");

                    }
            }
            else {
                items = this.ul.find("li");
                FSA = "SSV";
            }

            checkInputs(items, FSA);
        },
        open: function (e) {
            // //
            if ((newitemtext || this._prev) && newitemtext != this._prev) {
                newitemtext = this._prev;

                var dataitems = this.dataSource.data();

                var isfound = false;
                for (var i = 0; i < dataitems.length; i++) {
                    var dataItem = dataitems[i];

                    if (dataItem.Value != dataItem.Text) {
                        dataItem.Text = "Add new tag: " + newitemtext;
                        // this.dataSource.insert(0, { Value: newitemtext, Text: newitemtext });
                        //this.refresh();
                        isfound = true;
                    }
                }
                if (!isfound) {
                    //  this.dataSource.add({ Text: "Add new tag: " + newitemtext, Value: newitemtext });
                    this.dataSource.insert(0, { sTermCodeName: "Select All", iTerminationCodeID: "All" });
                    //isfound = true;
                    this.refresh();

                }
                //this.search();
                //this.open();
            }
        }
    });

}

function SelectAllClick() {
    //
    //debugger;
    var multiSelect = $("#lstResolutionCode").data("kendoMultiSelect");
    var selectedValues = "";
    var strComma = "";
    for (var i = 0; i < multiSelect.dataSource.data().length; i++) {
        var item = multiSelect.dataSource.data()[i];
        if (item.iTerminationCodeID != "All") {
            selectedValues += strComma + item.iTerminationCodeID;
            strComma = ",";
        }
        var th = this;
        // this.ul.find("li").find("input").attr("checked", "checked");
    }
    multiSelect.value(selectedValues.split(","));
}
function DSelectAllClick() {
    //
    var multiSelect = $("#lstResolutionCode").data("kendoMultiSelect");
    multiSelect.value([]);
}
function changeDllFilterBy() {
    debugger;
    var value = $("#ddlFilteredBy").val();
    var ddlFilteredByText = $("#ddlFilteredBy").data("kendoDropDownList").text();
    var index = $('#ddlFilteredBy').data('kendoDropDownList').select();
    CreateOutcomeBasedBarChart(ddlFilteredByText, index);
};
function onSelectTab(e) {
     
    //if ($(e.item).find("> .k-link").text()=='Data') {
    //    grdshow();
    //}

}
function CreateOutcomeBasedBarChart_1(filterBy, cln) {
    debugger;

    var seriesChartData = [];
    var arrTerminationName = [];
    var textTitle = Resources.display_BSR_OutcomeStatusBasedon + filterBy;
   
    var unAttempted = ChartData.length;
    var xAxisData = [];
    var results = ChartData;



    var columns1 = GrdColumeList;
    for (var _i = 0; _i < results.length; _i++) {

        var celVal;
        var termVal;
        var columnsIn = results[_i];
        cln = cln == -1 ? 0 : cln;
        var col1 = columns1[cln];
        var columnsNames1 = col1.Cid;
        for (var key in columnsIn) {
            if (columnsNames1 == key) {
                var units = results[_i][columnsNames1];
                var ConvertDate = ConvertDateFormate(units, columnsNames1);
                if (ConvertDate != 0) {
                    celVal = ConvertDate;
                }
                else {
                    celVal = results[_i][columnsNames1];
                }
            }
        }

        var idx = $.inArray(celVal, xAxisData);
        if (idx == -1 && celVal != "") {
            xAxisData.push(celVal);
        }
    }

    for (var j = 0; j < ChartData.length; j++) {

        var idx = $.inArray(ChartData[j].ResolutionName == ("" || null) ? "Un Attempted" : ChartData[j].ResolutionName, arrTerminationName);
        if (idx == -1) {
            if (ChartData[j].ResolutionName == ("" || null)) {

                arrTerminationName.push("Un Attempted");
            }
            else {
                arrTerminationName.push(ChartData[j].ResolutionName);
            }
        }
    }

    dataChart_ = [];
    for (var j = 0; j < arrTerminationName.length; j++) {


        var arrAttrCount = [];

        var columns = $("#GridViewReportData").data("kendoGrid").columns;
        for (var k = 0; k < xAxisData.length; k++) {

            var attrCount = 0;
            var unAttempted = 0;

            for (var i = 0; i < results.length; i++) {
                //   debugger;
                var celVal;
                var termVal;
                var columnsIn = results[i];
                var col1 = columns[cln];
                var columnsNames1 = col1.field;
                var col2 = columns[8];
                var columnsNames2 = col2.field;
                for (var key in columnsIn) {
                    if (columnsNames1 == key) {
                        var units = results[i][columnsNames1];
                        var ConvertDate = ConvertDateFormate(units, columnsNames1);
                        if (ConvertDate != 0) {
                            celVal = ConvertDate;
                        }
                        else {
                            celVal = results[i][columnsNames1];
                        }
                    }
                    if (columnsNames2 == key) {
                        termVal = results[i][columnsNames2];
                    }
                }
                var xVal = xAxisData[k];
                var xTermVal = arrTerminationName[j];

                if (termVal == xTermVal && celVal == xVal) {
                    //if (celVal == xVal) {
                    //  debugger;
                    attrCount++;
                }
                else if (xTermVal == "Un Attempted" && (termVal == "" || termVal == null) && celVal == xVal) {

                    unAttempted++;
                    attrCount = unAttempted;

                }
                //else if (xTermVal == "Un Attempted") {

                //    unAttempted++;
                //    attrCount = unAttempted;

                //}
            }
            //  debugger;
            arrAttrCount.push(attrCount);
            dataChart_.push({
                category: xVal,
                val: attrCount
            });



        }
        // debugger;
        seriesChartData.push({ name: arrTerminationName[j], data: arrAttrCount, color: getRandomColor() });
    }
    if (dataChart_.length <= 10) {
        MIN_SIZE = dataChart_.length;
        MAX_SIZE = dataChart_.length;
        viewSize = MIN_SIZE;
    }
    else {
        MIN_SIZE = 10;
        MAX_SIZE = 20;
        viewSize = MIN_SIZE;
    }
    CreateOutComeStatusBarCharBasedOn(seriesChartData, xAxisData, textTitle);

}