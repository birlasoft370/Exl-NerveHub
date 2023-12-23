
window.columnwithOnlyDate = new Array();
window.columnwithOnlyTime = new Array();

function FormatDate() {
    //debugger;
    $(".k-grid").each(function () {       
        var grid = $(this).data("kendoGrid");
        
        if (grid !== null && grid != undefined)// && grid.dataSource == args.sender)
        {
            
            if (grid.dataSource != null && grid.dataSource != undefined) {
                var DataSource = grid.dataSource;
                var currentTimeZone = $("#hdnCurrentOffset").val();
                var TZ = currentTimeZone.split("/");
                if (TZ != null && TZ != undefined)
                    tzS = TZ[1];
                else tzS = "330"; 
               
                for (var i = 0; i < DataSource.total() ; i++) {
                    if (DataSource.data()[i] != undefined) {
                        for (var j = 0; j < grid.columns.length; j++) {

                            var units = DataSource.data()[i].get(grid.columns[j].field)
                            if (units != null && ($.type(units) == 'date' || ($.type(units) == 'string' && units.indexOf('/Date(') == 0))) {
                                var dd = kendo.parseDate(units)
                                var tOff = dd.getTimezoneOffset();
                                dd.setMinutes(dd.getMinutes() + (tOff));
                                //var tOffset = parseInt(tzS, 10);
                                //dd.setMinutes(dd.getMinutes() + (tOffset));
                                var FormatPart = CultureDateFormat.split(' ');
                                var ColumnFormate = CultureDateFormat;
                                if (FormatPart != null && FormatPart != undefined && (columnwithOnlyDate != null || columnwithOnlyTime != null)) {
                                    if (columnwithOnlyDate.indexOf(grid.columns[j].field) >= 0) {
                                        ColumnFormate = FormatPart[0];
                                    }
                                    else if (columnwithOnlyTime.indexOf(grid.columns[j].field) >= 0 && FormatPart.length > 1) {
                                        ColumnFormate = "";
                                        for (var i = 1; i < FormatPart.length; i++) {
                                            ColumnFormate += FormatPart[i] + " ";
                                        }
                                    }
                                }

                                //var dg = kendo.toString(dd, "MM/dd/yyyy HH:mm:ss");
                                var dg = kendo.toString(dd, ColumnFormate);
                                //alert(dg);
                                //DataSource.data()[i].set(grid.columns[j].field, dg);
                                // 
                                var ds = "DataSource.data()[" + i + "]." + grid.columns[j].field + " = dg";
                                eval(ds);
                            }
                        }
                    }
                }
                grid.refresh();
            }
            grid.bind("dataBound", function (e) {
                GridDataBound(e, this);
            })
        }
    });
}
function GridDataBound(e, grid) {
    var columns = e.sender.columns;
    var rows = e.sender.tbody.children();
    for (var j = 0; j < rows.length; j++) {
        var row = $(rows[j]);
        var dataItem = e.sender.dataItem(row);
        for (var k = 0; k < columns.length; k++) {
            var units = dataItem.get(columns[k].field);
            //if (units != null && ($.type(units) == 'date' || ($.type(units) == 'string' && units.indexOf('/Date(') == 0))) {

                //var dd = kendo.parseDate(units);
                //var tOff = dd.getTimezoneOffset();
                //dd.setMinutes(dd.getMinutes() + (tOff));
                ////var tzS = $("#hdnCurrentOffset").val();
                ////var tzS = $("#ddlTimeZone option:selected").text();
                //var currentTimeZone = $("#hdnCurrentOffset").val();
                //var TZ = currentTimeZone.split("/");
                //if (TZ != null && TZ != undefined)
                //    tzS = TZ[1];
                //else tzS = "330";
                //var tOffset = parseInt(tzS, 10);
            //dd.setMinutes(dd.getMinutes() + (tOffset));

            var isDate = false;


                var FormatPart = CultureDateFormat.split(' ');
                var ColumnFormate = CultureDateFormat;
                if (FormatPart != null && FormatPart != undefined && (columnwithOnlyDate != null || columnwithOnlyTime != null)) {
                    if (columnwithOnlyDate.indexOf(grid.columns[k].field) >= 0) {
                        isDate = true;
                        ColumnFormate = FormatPart[0];
                    }
                    else if (columnwithOnlyTime.indexOf(grid.columns[k].field) >= 0 && FormatPart.length > 1) {
                        ColumnFormate = "";
                        isDate = true;
                        for (var i = 1; i < FormatPart.length; i++) {
                            ColumnFormate += FormatPart[i]+" ";
                        }
                    }
                }
                if (isDate == true) {
                    var dd = kendo.parseDate(units);
                    var dg = kendo.toString(dd, ColumnFormate);
                    var ds = "dataItem." + (grid.columns[k].field) + "=dg";
                    // dataItem.set(grid.columns[k].field, dg);
                    eval(ds);
                }
            //}
        }
    }
    grid.unbind('dataBound');
    grid.refresh();
    grid.bind("dataBound", function (e) {
        GridDataBound(e, this);
    });
}

function registerIndexOftoArray() {
    if (!Array.prototype.indexOf) {
        Array.prototype.indexOf = function (obj, fromIndex) {
            if (fromIndex == null) {
                fromIndex = 0;
            } else if (fromIndex < 0) {
                fromIndex = Math.max(0, this.length + fromIndex);
            }
            for (var i = fromIndex, j = this.length; i < j; i++) {
                if (this[i] === obj)
                    return i;
            }
            return -1;
        };
    }
}


$(document).ready(function () {
    //FormatDate();
    registerIndexOftoArray();
    setTimeout(FormatDate, 1000);
});