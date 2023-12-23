function Approval_DateValidation() {

    var fromdt = new Date($("#FromDate").val());
    var todt = new Date($("#ToDate").val());
    if (fromdt > todt) {
        jAlert("From date must be smaller than To date.");
        return false;
    }
    else if (!chkDate(fromdt, todt))
        return false;
    else { Bind_ProcessApproval(); return true; }

}

/*To Validate from date and to date*/
function chkDate(fromDt, toDt) {

    var diffMSec = toDt - fromDt;
    var DiffMonth = diffMSec / (1000 * 60 * 60 * 24 * 30);
    if (DiffMonth > 1.055535726080247) {
        jAlert("Date Difference must not be older than one month.");
        return false;
    }
    else
        return true;
}

function Bind_ProcessApproval() {

    var grid = $("#Approvalgrid").data("kendoGrid")
    grid.dataSource._destroyed = [];
    grid.dataSource.read();
    if (grid.dataSource.data.length > 0) {
        $("#spnNoRecord").hide();
        $("#Approvalgrid").show();
        $(".btnAction").show();
    }
}

function Binding_ProcessApproval() {

    if ($("#Approvalgrid table tbody tr").length > 0) {
        var showApp = false;
        var showRej = false;
        var showCan = false;
        $("#spnNoRecord").hide();
        $(".btnAction").show();
        $("#Approvalgrid").show();
        $("#Approvalgrid table tbody tr").each(function () { if ($(this).find('td:eq(14)').html().length > 0) { showCan = true; } });
        $("#Approvalgrid table tbody tr").each(function () { if ($(this).find('td:eq(13)').html().length > 0) { showRej = true; } });
        $("#Approvalgrid table tbody tr").each(function () { if ($(this).find('td:eq(12)').html().length > 0) { showApp = true; } });
        if (showCan) { $("#BtnCancel").show(); } else { $("#BtnCancel").hide(); }
        if (showRej) { $("#BtnReject").show(); } else { $("#BtnReject").hide(); }
        if (showApp) { $("#BtnApprove").show(); } else { $("#BtnApprove").hide(); }
    }
    else {
        $("#spnNoRecord").show();
        $("#Approvalgrid").hide();
        $(".btnAction").hide();
    }
}

/*To apply filter to read Campaign approval records*/
function GetFilter() {

    return { dFrom: $("#FromDate").val(), dTo: $("#ToDate").val() };
}

function Process_Approval(obj, id) {
    debugger;
    var msg = '';
    var token = $("#formProcesskApproval input").val();
    $.ajax({
        type: "POST"
        , url: ResourceLayout.partiaArea + "ProcessOwner/ApproveCancelReject"
        , data: { __RequestVerificationToken: token, iRequestId: id, _action: obj.value }
        , dataType: 'json'
        , success: function (result) {
            if (result != null) {
                if (obj.value == "Cancel") {
                    msg = Resource.displayCancelledsuccessfully
                }
                else if (obj.value == "Reject") {
                    msg = Resource.displayRejectedsuccessfully
                }
                else if (obj.value == "Approve") {
                    msg = Resource.displayApprovedsuccessfully
                }

                jAlert(msg, 'Alert', function (r) {
                    if (r) {
                        $("#Approvalgrid").data("kendoGrid").dataSource.read();
                    }
                });
            }
            else {
                jAlert(result);
            }
        }

    });
}