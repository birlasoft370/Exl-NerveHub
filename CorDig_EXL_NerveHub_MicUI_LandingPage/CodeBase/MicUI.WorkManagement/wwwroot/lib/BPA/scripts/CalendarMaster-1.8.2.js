/*
     * FileName: CalendarMaster-1.8.2.js
     * ClassName: CalendarMaster-1.8.2
     * Purpose: This file contains all the scripts for the CalendarMaster Form
     * Description:  
     * Created By: NABIN KUMAR
     * Created Date: 03/DEC/ 2015
     * Modified By: 
     * Modified Date:
     * Modified Purpose:
     * Modified Description: 
 */
// Form  Navigation functions

function OnCalendarMasterClickNew() {
    window.location.href = Resources.url_Index;
}

function OnCalendarMasterClickNewView() {
    window.location.href = Resources.url_ShowCalendarMaster;
}

function OnClickNew() {
    window.location.href = Resources.url_Index;
}


// Edit calendar function
function editCalendar(e) {

    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var value = dataItem.iCalendarID;
  
    $.ajax({
        type: "POST",
        url: Resources.url_SetCalendarID,   
        data: {Calids: value},
        dataType: 'json',
        success: function (result) {
            if (result == "1") {
                window.location.href = Resources.url_Index;
            }
        }
    });
   
}


// Function to delete  calendar
function deleteCalendar(e) {

    var index = $(e.currentTarget).closest("tr")[0].rowIndex;
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var token = $('#formCalendarMaster input').val();

    jConfirm(Resources.display_Comfirm_Delete, Resources.display_Alert, function (r) {
        if (r) {
            e.preventDefault();
            $.ajax({
                type: "DELETE",
                url: Resources.url_Delete,
                data: { __RequestVerificationToken: token, CALID: JSON.stringify(dataItem.iCalendarID) },
                dataType: 'json',
                success: function (result) {

                    if (result == display_OK) {
                        $("#searchGrid  table > tbody tr:eq(" + index + ")").remove();
                        jAlert(Resources.display_deleted, Resources.display_Alert);
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
    });
}

// Function to Submit Form
function GoCalendarMaster() {
    var grid = $("#searchGrid").data("kendoGrid");
    grid.dataSource.read();
}



function GetCalenderName() {

    return {
        CalenderName: $("#CalendarSearchName").val()
    }
}