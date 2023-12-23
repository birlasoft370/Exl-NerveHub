function OnCalendarDataClickNew(e) {
    window.location.href = Resources.url_Index;
}

function OnCalendarDataClickNewView(e) {
    window.location.href = Resources.url_ViewCalendarData;
}

function OnViewCalendarDataNewClick(e) {
    window.location.href = Resources.url_Index;
}
// Hide unhide or Enable disable Controls of Index page

var max = 0;
function Click_btnDefineWeek() {

    var validator = $("#mCalendarDate").val();
    var validatormWeekStartDay = $("#mWeekStartDay").val();
    if (validator == '') {
        var grid = $('#gvDays').data("kendoGrid");
        if (grid != undefined) {
            grid.destroy();
        }
        GenerateGrd([])
        $("#formCalendarDataIndex").submit();
        return;
    }
    if (validatormWeekStartDay == '') {
        var grid = $('#gvDays').data("kendoGrid");
        if (grid != undefined) {
            grid.destroy();
        }
        GenerateGrd([])
        $("#formCalendarDataIndex").submit();
        return;
    }

    $.ajax({
        url: Resources.url_Products_Read_Grd,
        data: { Parameters: $("#mCalendarDate").val() + "|" + $("#mMonth").val() + "|" + $("#mYear").val() + "|" + $("#mStartDateofMonth").val() + "|" + $("#mEndDateofMonth").val() + "|" + $("#mWeekStartDay").val() + "|" + $("#iCalID").val() + "|" + $("#mMonthYear").val() },
        type: 'Post',
        cache: false,
        success: function (data) {

            var grid = $('#gvDays').data("kendoGrid");
            if (grid != undefined) {
                grid.destroy();
            }
            var val = data;
            var baseUrl = Resources.Cultture;
            $.getScript(baseUrl, function () {
                kendo.ui.progress($("#gvDays"), false);
                GenerateGrd(data)
            });


        }
    });
};

function BindeWeek() {

    $.ajax({
        url: Resources.url_ReBindeWeek,
        data: { Parameters: $("#mCalendarDate").val() + "|" + $("#mMonth").val() + "|" + $("#mYear").val() + "|" + $("#mStartDateofMonth").val() + "|" + $("#mEndDateofMonth").val() + "|" + $("#mWeekStartDay").val() + "|" + $("#iCalID").val() + "|" + $("#mMonthYear").val() },
        type: 'Post',
        cache: false,
        success: function (data) {
            var grid = $('#gvDays').data("kendoGrid");
            if (grid != undefined) {
                grid.destroy();
            }
            var val = data;
            var baseUrl = Resources.Cultture;
            $.getScript(baseUrl, function () {
                kendo.ui.progress($("#gvDays"), false);
                GenerateGrd(data)
            });
        }
    });
};

function GenerateGrd(BindDataList) {
    debugger;
    if ($("#iCalID").val() != "0") {

        $("#gvDays").kendoGrid({
            dataSource: {
                transport: {
                    read: function (e) {
                        if (BindDataList != null)
                            e.success(BindDataList);
                    },
                    update: function (e) {
                        var dataItem = e.data, i, j;
                        e.success(dataItem);
                    },
                    destroy: function (e) {
                        e.success(e.data);
                    },
                    create: function (e) {
                        var item = e.data;
                        e.success(item);

                    }
                },
                Edit: onEdit,
                batch: true,

                schema: {
                    model: {
                        id: "miWeek",
                        fields: {
                            miWeek: { editable: false }, /*the id field is not editable */
                            DisplayStartDate: { type: "date" },
                            DisplayEndDate: { type: "date" }


                        }
                    }
                }
            },
            columns: [
                { width: 100, field: "miWeek", title: Resources.week },
                { width: 100, field: "DisplayStartDate", title: Resources.StartDate, format: "{0:" + Resources.cult + "}", editor: dateEditor, },
                { width: 100, field: "DisplayEndDate", title: Resources.EndDate, format: "{0:" + Resources.cult + "}", editor: dateEditor, },

                { command: "destroy", title: "&nbsp;", width: 150 }
            ],
            toolbar: ["create"],
            edit: function (e) {

                onEdit(e);

            },
            dataBound: function (e) {
                if ($("#iCalID").val() != "0") {
                    $(".k-grid-add").remove();

                }
            }
        });
    }
    else {
        var CalName = $("#mCalendarDate").val();

        if (CalName != "") {
            $("#gvDays").kendoGrid({
                dataSource: {
                    transport: {
                        read: function (e) {
                            if (BindDataList != null)
                                e.success(BindDataList);
                        },
                        update: function (e) {
                            var dataItem = e.data, i, j;
                            e.success(dataItem);
                        },
                        destroy: function (e) {
                            e.success(e.data);
                        },
                        create: function (e) {
                            var item = e.data;
                            e.success(item);

                        }
                    },
                    Edit: onEdit,
                    batch: true,

                    schema: {
                        model: {
                            id: "miWeek",
                            fields: {
                                miWeek: { editable: false }, /*the id field is not editable */
                                DisplayStartDate: { type: "date" },
                                DisplayEndDate: { type: "date" }
                            }
                        }
                    }
                },
                columns: [
                    { width: 100, field: "miWeek", title: Resources.week, template: "<span class='row-number'></span>" },
                    { width: 100, field: "DisplayStartDate", title: Resources.StartDate, format: "{0:" + Resources.cult + "}", editor: dateEditor, },
                    { width: 100, field: "DisplayEndDate", title: Resources.EndDate, format: "{0:" + Resources.cult + "}", editor: dateEditor, },

                    { command: "destroy", title: "&nbsp;", width: 150 }
                ],

                editable: {
                    mode: "incell",
                    createAt: "bottom"
                }

                ,
                toolbar: ["create"],
                edit: function (e) {

                    onEdit(e);

                },
                dataBound: function (e) {

                    var rows = this.items();
                    var i = 0;
                    var OldData = 0;
                    $(rows).each(function () {

                        if (e.sender._data[i].miWeek == "") {

                            var iWeek = e.sender._data[i - 1].miWeek + 1;
                            e.sender._data[i].miWeek = iWeek;
                        }

                        var index = e.sender._data[i].miWeek;
                        if (index == '') {

                            index = OldData + 1;
                            OldData = index;
                        }
                        else {
                            OldData = e.sender._data[i].miWeek;
                        }
                        var rowLabel = $(this).find(".row-number");
                        $(rowLabel).html(index);
                        i++;
                    });

                    if ($("#iCalID").val() != "0") {
                        $(".k-grid-add").remove();
                    }
                }
            });
        }
    }
}

function dateEditor(container, options) {
    $("<input data-bind='value:" + options.field + "' />")
        .appendTo(container)
        .kendoDatePicker({
            format: "{0:" + Resources.cult + "}",
            min: new Date(2000, 0, 1)

        });
}

function startChange() {
    var endPicker = $("#mdtEndDate").data("kendoDatePicker"),
        startDate = this.value();
    if (startDate) {
        startDate = new Date(startDate);
        startDate.setDate(startDate.getDate());
        endPicker.min(startDate);
    }
}

function endChange() {
    var startPicker = $("#mdtStartDate").data("kendoDatePicker"),
        endDate = this.value();

    if (endDate) {
        endDate = new Date(endDate);
        endDate.setDate(endDate.getDate());
        startPicker.max(endDate);
    }
}
function startChangeMonth() {
    var endPicker = $("#mEndDateofMonth").data("kendoDatePicker"),
        startDate = this.value();
    if (startDate) {
        startDate = new Date(startDate);
        startDate.setDate(startDate.getDate());
        $("#mEndDateofMonth").val($("#mStartDateofMonth").val());
        endPicker.min(startDate);
    }
}

function endChangeMonth() {
    var startPicker = $("#mStartDateofMonth").data("kendoDatePicker"),
        endDate = this.value();

    if (endDate) {
        endDate = new Date(endDate);
        endDate.setDate(endDate.getDate());
        // startPicker.max(endDate);
    }
}

//$('#gvAddrow').unbind("click", 

function OnCalendarDataAddNewRow(e) {

    if ($("#gvDays").data("kendoGrid").dataSource.total() > 0) {
        var gr = $('#gvDays').data('kendoGrid');
        max = $('#gvDays table tbody tr:last td:first').text();
        max = parseInt(max) + 1
        gr.addRow();
    }
    else {
        jAlert(Resources.required_DefineCalendar, Resources.display_Alert);
    }
}

//);

 function OnClickbtnSave_CalendarData(e) {   // Function to save Calendar Data
    var validator = $("form").data("kendoValidator");
    if (GetFormValidate($('form'), "btnSave_CalendarData")) {
        if ($("#gvDays").data("kendoGrid") == undefined) {
            jAlert(Resources.requiredWeekList, Resources.display_Alert);
            return;
        }
        var dataSource = $("#gvDays").data("kendoGrid").dataSource.view();
        var Result = "YES";

        var token = $("#formCalendarDataIndex input").val();
        if (dataSource.length > 0) {

            var CalendarID = 0;
            var iCalID = $("#iCalID").val();
            var mCalendarDate = $("#mCalendarDate").val();
            var mMonth = $("#mMonth").val();
            var mMonthYear = $("#mMonthYear").val();
            var mYear = $("#mYear").val();
            var mWeekStartDay = $("#mWeekStartDay").val();
            var mStartDateofMonth = $("#mStartDateofMonth").val();
            var mEndDateofMonth = $("#mEndDateofMonth").val();
            var Disable = $("#Disable").is(':checked') ? true : false;

            var ArrCalendarweek = [];

            var CalendarDataModel = {
                iCalID: iCalID,
                iCalendarID: CalendarID,
                mCalendarDate: mCalendarDate,
                mMonthYear: mMonthYear,
                mWeekStartDay: mWeekStartDay,
                mStartDateofMonth: mStartDateofMonth,
                mEndDateofMonth: mEndDateofMonth,
            };

            for (var i = 0; i < dataSource.length; i++) {

                var Calendarweek = {

                    miWeek: dataSource[i].miWeek,
                    StrDisplayStartDate: GetProperDate(dataSource[i].DisplayStartDate),
                    StrDisplayEndDate: GetProperDate(dataSource[i].DisplayEndDate)
                    //DisplayStartDate: GetProperDate(dataSource[i].DisplayStartDate),
                    //DisplayEndDate: GetProperDate(dataSource[i].DisplayEndDate)
                }

                ArrCalendarweek.push(Calendarweek);
            }

            $.ajax({
                url: Resources.url_InsertUpdateCalenderData,
                type: 'Post',
                data: { __RequestVerificationToken: token, strCalendarDataModel: CalendarDataModel, jsonCalendarGridData: ArrCalendarweek },
                cache: false,
                datatype: "JSON",
                success: function (result) {
                    if (result == 1) {
                        jAlert(Resources.display_save, Resources.display_Alert);
                        $("#iCalID").text = "0";
                        $("#gvDays").data("kendoGrid").dataSource.read();
                        window.location.href = Resources.url_Index;
                    }
                    else if (result == 5) {
                        jAlert(Resources.displayUpdate, Resources.display_Alert);
                        $("#iCalID").text = "0";
                        $("#mCalendarDate").data("kendoDropDownList").select(0);
                        $("#mWeekStartDay").data("kendoDropDownList").select(0);
                        $("#Disable").prop("checked", false);
                        $("#gvDays").data("kendoGrid").dataSource.read();
                        window.location.href = Resources.url_Index;
                    }
                    else {
                        jAlert(result, Resources.display_Alert);
                        return false;
                    }
                }
            });
        }
        else {
            jAlert(Resources.displayDefineWeek, Resources.display_Alert);
            return false;
        }
    }
    else {
        $('form').submit();
    }
}

//function GetProperDate(input) {
//    var date = new Date(input);
//    return date;
//}
function GetProperDate(input) {
    //debugger;
    var culStr = [];
    var delimiter = '/';
    culStr = Resources.cult.split('/');
    if (culStr == undefined) {
        return input;
    }
    if (culStr.length > 1) delimiter = '-';
    if (culStr.length == 1) {
        culStr = Resources.cult.split('-');
        if (culStr.length > 1) delimiter = '-';
        if (culStr.length == 1) {
            culStr = Resources.cult.split('.');
            if (culStr.length > 1) delimiter = '.';
        }
    }

    var currentdate = '';
    if (culStr.length == 3) {
        for (var i = 0; i < culStr.length; i++) {
            if (culStr[i] == "d" || culStr[i] == "D" || culStr[i] == "dd" || culStr[i] == "DD") {
                currentdate += input.getDate() + ((i != 2) ? delimiter : '');
            }
            else if (culStr[i] == "m" || culStr[i] == "M" || culStr[i] == "mm" || culStr[i] == "MM" || culStr[i] == "MMM" || culStr[i] == "mmm") {
                currentdate += (input.getMonth() + 1) + ((i != 2) ? delimiter : '');
            }
            else {
                currentdate += input.getFullYear() + ((i != 2) ? delimiter : '');
            }
        }
    }
    return currentdate;
}


function onEdit(e) {

    if (e.model.isNew()) {
        e.model.set("miWeek", max);
    }
}



function SetFilter() {

    return {
        Parameters: $("#mCalendarDate").val() + "|" + $("#mMonth").val() + "|" + $("#mYear").val() + "|" + $("#mStartDateofMonth").val() + "|" + $("#mEndDateofMonth").val() + "|" + $("#mWeekStartDay").val() + "|" + $("#iCalID").val() + "|" + $("#mMonthYear").val()
    }
}



function db(e) {

    var grid = this;

    $(".templateCell").each(function () {

        eval($(this).children("script").last().html());

        var tr = $(this).closest('tr');

        var item = grid.dataItem(tr);

        kendo.bind($(this), item);

    });

}

function GoCalendarData(e) {
    var form = $('#formViewCalendarData');
    //form.data('validator').settings.ignore = '';
    $("#formViewCalendarData").submit();
}

function edit(e) {

    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var value = dataItem.sDescription
    $.ajax({
        type: 'POST',
        url: Resources.url_SetCalendarID,
        data: { CalendarID: dataItem.sDescription },
        dataType: 'json',
        success: function (result) {
            if (result == "1") {
                window.location.href = Resources.url_Index;
            }
        }
    });
}





function _delete(e) {

    var token = $("#formViewCalendarData input").val();
    var index = $(e.currentTarget).closest("tr")[0].rowIndex - 1;
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var Description = dataItem.sDescription
    jConfirm("Do You Wish To Delete Record", 'Confirmation', function (r) {
        if (r) {
            e.preventDefault();

            $.ajax({
                type: "DELETE",
                url: Resources.url_Delete,
                data: { __RequestVerificationToken: token, CalDataID: Description },
                dataType: 'json',
                success: function (result) {

                    if (result == "OK") {
                        $("#searchGrid  table > tbody tr:eq(" + index + ")").remove();
                        jAlert(Resources.msg_DataDeleted, Resources.display_Alert);
                    }
                    else if (result != "OK") {
                        jAlert(result, Resources.display_Alert);
                    }
                }
            });
        }
        else {
            return false;
        }
    });
}

$("#SBUName").removeClass('input-validation-error');


function Parameter() {
    mCalendarDate = $("#mCalendarDate").val();
    mYear = $("#mYear").val();
    mMonth = $("#mMonth").val();
}


