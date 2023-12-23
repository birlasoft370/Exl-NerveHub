/*
 Programm Name : Shift Master-1.8.2
 Created by    : Nabin Kumar
 Create Date   : 12/04/2014
 Modifued Date :
 Modified By   : 
 Description   : To manage Shift Master

*/

// Function For Navigation Through Pages
$.validator.defaults.ignore = "";
function OnClickShiftMaster() {
    window.location.href = Resources.url_Index;
}

function OnClickShiftMasterSearch() {
    window.location.href = Resources.url_ShiftSearchView;
}


// Function To Submit Form
function GoShiftMaster() {

    var form = $('#formShiftMaster');
    form.data('validator').settings.ignore = '';
    $("#formShiftMaster").submit();
}

// Function To Edit Shift Master
function editShift(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    $.ajax({
        type: "POST",
        url: Resources.url_EditingCustom_Edit,
        data: { sShiftID: dataItem.ShiftID },
        dataType: 'json',
        success: function (result) {
            if (result == "1") {
                window.location.href = Resources.url_Index;
            }
        }
    });
}

// Function To delete Shift Master

function deleteShift(e) { // Function for Deleting Shift 

    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var index = $(e.currentTarget).closest("tr")[0].rowIndex;
    var token = $("#formShiftMaster input").val();
    jConfirm(Confirm_Delete, 'Confirmation', function (r) {
        if (r) {

            $.ajax({
                type: "Post",
                url: Resources.url_EditingCustom_Destroy,
                data: {
                    __RequestVerificationToken: token,
                    sShiftID: JSON.stringify(dataItem.ShiftID)
                },
                dataType: 'json',
                success: function (result) {
                    if (result == OK) {
                        $("#searchGrid  table > tbody tr:eq(" + index + ")").remove();
                        jAlert(Shift_Deleted);
                    }
                    else {
                        jAlert(result);
                    }
                }
            });
        }
        else {
            return false;
        }
    });
}

function startChange() {

    var startTime = this.value(),
        endPicker = $("#ShiftEndTime").data("kendoTimePicker");
    if (startTime) {
        startTime = new Date(startTime);
        startTime.setMinutes(startTime.getMinutes() + 540);
        endPicker.min(startTime);
        startTime.setHours(startTime.getHours() + 286);
        startTime.setMinutes(startTime.getMinutes() + 540);
        endPicker.max(startTime);
    }
}


function resetHoursDropdownLists() {
    $("#ShiftStartHrTime").data("kendoDropDownList").select(0);
    $("#ShiftStartMinTime").data("kendoDropDownList").select(0);
    $("#ShiftEndHrTime").data("kendoDropDownList").select(0);
    $("#ShiftEndMinTime").data("kendoDropDownList").select(0);
}

///////////// Code t get Hourse fo tow given times 
function diff(start, end) {

    start = start.split(":");
    end = end.split(":");
    var startDate = new Date(0, 0, 0, start[0], start[1], 0);
    var endDate = new Date(0, 0, 0, end[0], end[1], 0);
    var diff = endDate.getTime() - startDate.getTime();
    var hours = Math.floor(diff / 1000 / 60 / 60);
    diff -= hours * 1000 * 60 * 60;
    var minutes = Math.floor(diff / 1000 / 60);
    // If using time pickers with 24 hours format, add the below line get exact hours
    if (hours < 0)
        hours = hours + 24;
    return (hours <= 9 ? "0" : "") + hours + ":" + (minutes <= 9 ? "0" : "") + minutes;
}