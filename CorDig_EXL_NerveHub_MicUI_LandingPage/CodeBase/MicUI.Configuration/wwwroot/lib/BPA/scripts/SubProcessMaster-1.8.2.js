$.validator.defaults.ignore = "";
function OnClickSubProcess() {
    window.location.href = ResourceSubProcessIndex.urlPath_Index
}

function OnClickSubProcessSearch() {
    window.location.href = ResourceSubProcessIndex.urlPath_SubProcessSearchView
}

function ProductionStartChange() {
    var endPicker = $("#ProductionEndDate").data("kendoDatePicker"),
        startDate = this.value();

    if (startDate) {
        startDate = new Date(startDate);
        startDate.setDate(startDate.getDate());
        endPicker.min(startDate);
    }
}

function ProductionEndChange() {
    var startPicker = $("#ProductionStartDate").data("kendoDatePicker"),
        endDate = this.value();

    if (endDate) {
        endDate = new Date(endDate);
        endDate.setDate(endDate.getDate());
        startPicker.max(endDate);
    }
}

function SubProcessStartChange() {
    var endPicker = $("#SubProcessEndDate").data("kendoDatePicker"),
        startDate = this.value();

    if (startDate) {
        startDate = new Date(startDate);
        startDate.setDate(startDate.getDate());
        endPicker.min(startDate);
    }
}

function SubProcessEndChange() {
    var startPicker = $("#SubProcessStartDate").data("kendoDatePicker"),
        endDate = this.value();

    if (endDate) {
        endDate = new Date(endDate);
        endDate.setDate(endDate.getDate());
        startPicker.max(endDate);
    }
}

function StabilizationStartDateChange() {
    var endPicker = $("#StabilizationEndDate").data("kendoDatePicker"),
        startDate = this.value();

    if (startDate) {
        startDate = new Date(startDate);
        startDate.setDate(startDate.getDate());
        endPicker.min(startDate);
    }
}

function StabilizationEndDateChange() {
    var startPicker = $("#StabilizationStartDate").data("kendoDatePicker"),
        endDate = this.value();

    if (endDate) {
        endDate = new Date(endDate);
        endDate.setDate(endDate.getDate());
        startPicker.max(endDate);
    }
}

function StabilizationGoLiveChange() {
    var GoLiveDate = $("#GoLiveDate").data("kendoDatePicker"),
        endDate = this.value();

    if (endDate) {
        endDate = new Date(endDate);
        endDate.setDate(endDate.getDate());
        GoLiveDate.max(endDate);
    }
}
$("#btnSubProcessSave").click(function () {
    validationForm();
});

/* Search Page */
function Go() {
    var form = $('#formSubProcessSearchView');
    form.data('validator').settings.ignore = '';
    $("#formSubProcessSearchView").submit();
}
function OnClickSubProcessMaster() {
    window.location.href = ResourceSubProcessSearch.urlPath_Index
}
function editSubProcess(e) {
    e.preventDefault();
    var token = $("#formSubProcessSearchView input[name=__RequestVerificationToken]").val();
    // var token = $("#formSubProcessSearchView input").val();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    $.ajax({
        type: "POST"
        , url: ResourceSubProcessSearch.urlPath_EditingCustom_Edit
        , data: { __RequestVerificationToken: token, sSubProcessID: dataItem.SubProcessID }
        , dataType: 'json'
        , success: function (result) {
            if (result == "Ok") {
                window.location.href = ResourceSubProcessSearch.urlPath_Index
            }
        }
    });
}
function deleteSubProcess(e) {
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var token = $("#formSubProcessSearchView input").val();
    jConfirm(ResourceSubProcessSearch.display_DeleteConfirmation, 'Confirmation', function (r) {
        if (r) {
            e.preventDefault();
            $.ajax({
                type: "POST"
                , url: ResourceSubProcessSearch.urlPath_EditingCustom_Destroy
                , data: { __RequestVerificationToken: token, sSubProcessID: dataItem.SubProcessID }
                , dataType: 'json'
                , success: function (result) {
                    if (result == null) {
                        jAlert(ResourceSubProcessSearch.display_deletemsg, 'Alert', function (r) {
                            if (r) {
                                $("#searchGrid").data("kendoGrid").dataSource.read();
                            }
                        });
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