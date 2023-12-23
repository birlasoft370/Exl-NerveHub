function OnClickSubmit() {

}
function OnClickReset() {

}
function editRow(e) {
    var EMSLanguageConfigViewModel = {};
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var LanguageConfigID = dataItem.LanguageConfigID;
    var token = $("#form1 input[name=__RequestVerificationToken]").val();
    $.LoadingOverlay("show");
    $.ajax({
        type: 'Post'
        , url: ResourceLayout.partialURL + "SetConfigurationID"
        , data: { __RequestVerificationToken: token, objLangID: LanguageConfigID }
        , dataType: "json"
        , success: function (result) {
            $.LoadingOverlay("hide");
            if (result == "1") {
                var SearchViewPopUpWindow2 = $('#SearchViewPopUp').data('kendoWindow');
                SearchViewPopUpWindow2.refresh();
                SearchViewPopUpWindow2.center().open();

                //$('#SearchViewPopUp').data("kendoWindow").title("Language Configuration")
                //    .refresh({ url: ResourceLayout.partialURL + "SearchViewPopUp" }).center().open();
            }
        },
        error: function (err) {
            $.LoadingOverlay("hide");
        }
    });
}

function GetData(e) {
    return { StoreID: $("#WorkObjName").val() }
}

function OnClickClose(e) {

    var EMSLanguageConfigViewModel = {};
    var dataItem = [];
    var ClientName = $('#ClientName').val();
    var ProcessName = $('#ProcessName').val();
    var CampaignName = $('#CampaignName').val();
    var LanguageConfigID = $('#LanguageConfigID').val();
    var LanguageConfigName = $('#LanguageConfigName').val();
    var AzureKey = $('#AzureKey').val();
    var CategoryID = $('#CategoryID').val();
    var ProviderID = $('#ProviderID').val();
    var AppId = $('#AppId').val();
    var EndPointAddress = $('#EndPointAddress').val();
    var SystranKey = $('#SystranKey').val();
    var FilePath = $('#FilePath').val();
    var Target = $('#Target').val();
    var Source = $('#Source').val();
    var ApiKey = $('#ApiKey').val();
    var ApiUrl = $('#ApiUrl').val();
    var BatchId = $('#BatchId').val();
    var Callback = $('#Callback').val();
    var RequestId = $('#RequestId').val();
    var Format = $('#Format').val();
    var Profile = $('#Profile').val();
    var ProfileID = $('#ProfileID').val();
    var WithDictionary = $('#WithDictionary').val();
    var WithCorpus = $('#WithCorpus').val();
    var Options = $('#Options').val();
    var Encoding = $('#Encoding').val();
    var IncomingMail = $('#IncomingMail').val();
    var InputData = $('#InputData').val();
    var BackTranslation = false;
    var Async = false;
    var WithAnnotations = false;
    var WithSource = false;
    var IgnoreHidden = false;
    var IncomingMail = false;
    var Disabled = false;

    if ($('#BackTranslation').is(':checked')) {
        BackTranslation = true;
    }
    else {
        BackTranslation = false;
    }
    if ($('#Async').is(':checked')) {
        Async = true;
    }
    else {
        Async = false;
    }
    if ($('#WithAnnotations').is(':checked')) {
        WithAnnotations = true;
    }
    else {
        WithAnnotations = false;
    }
    if ($('#WithSource').is(':checked')) {
        WithSource = true;
    }
    else {
        WithSource = false;
    }
    if ($('#IgnoreHidden').is(':checked')) {
        IgnoreHidden = true;
    }
    else {
        IgnoreHidden = false;
    }

    if ($('#Disabled').is(':checked')) {
        Disabled = true;
    }
    else {
        Disabled = false;
    }
    if ($('#IncomingMail').is(':checked')) {
        IncomingMail = true;
    }
    else {
        IncomingMail = false;
    }

    if ($("#LanguageConfigName").val() == "") {
        jAlert('Please enter Language Config. Name', "Alert", function (r) {
            $('#LanguageConfigName').focus();
        })
        return false;
    }

    if ($("#Target").val() == "") {
        jAlert('Please select Target Language', "Alert", function (r) {
            $('#Target').focus();
        })
        return false;
    }

    EMSLanguageConfigViewModel.CampaignName = CampaignName;
    EMSLanguageConfigViewModel.ClientName = ClientName;
    EMSLanguageConfigViewModel.ProcessName = ProcessName;
    EMSLanguageConfigViewModel.ApiKey = ApiKey;
    EMSLanguageConfigViewModel.ApiUrl = ApiUrl;
    EMSLanguageConfigViewModel.LanguageConfigName = LanguageConfigName;
    EMSLanguageConfigViewModel.LanguageConfigID = LanguageConfigID;
    EMSLanguageConfigViewModel.Target = Target;
    EMSLanguageConfigViewModel.Source = Source;
    EMSLanguageConfigViewModel.Async = Async;
    EMSLanguageConfigViewModel.AzureKey = AzureKey;
    EMSLanguageConfigViewModel.BackTranslation = BackTranslation;
    EMSLanguageConfigViewModel.BatchId = BatchId;
    EMSLanguageConfigViewModel.Callback = Callback;
    EMSLanguageConfigViewModel.AppId = AppId;
    EMSLanguageConfigViewModel.FilePath = FilePath;
    EMSLanguageConfigViewModel.Disabled = Disabled;
    EMSLanguageConfigViewModel.IncomingMail = IncomingMail;
    EMSLanguageConfigViewModel.Format = Format;
    EMSLanguageConfigViewModel.Profile = Profile;
    EMSLanguageConfigViewModel.ProfileID = ProfileID;
    EMSLanguageConfigViewModel.RequestId = RequestId;
    EMSLanguageConfigViewModel.WithAnnotations = WithAnnotations;
    EMSLanguageConfigViewModel.WithCorpus = WithCorpus;
    EMSLanguageConfigViewModel.WithDictionary = WithDictionary;
    EMSLanguageConfigViewModel.WithSource = WithSource;
    var token = $("#form1 input[name=__RequestVerificationToken]").val();

    $.LoadingOverlay("show");
    $.ajax({
        type: 'Post'
        , url: ResourceLayout.partialURL + "InsertUpdateLanguageConfiguration"
        , data: ({ __RequestVerificationToken: token, objEMSLanguageConfigViewModel: EMSLanguageConfigViewModel })
        , dataType: 'json'
        , success: function (result) {
            $.LoadingOverlay("hide");
            var valcheck = result.split(',')[1];
            if (valcheck == 'OK') {
                jAlert(result.split(',')[0]);
                $('#gridLanguageConfig').data('kendoGrid').dataSource.read();
                $("#SearchViewPopUp").closest(".k-window-content").data("kendoWindow").close();
            }
            else {

                jAlert(result);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $.LoadingOverlay("hide");
            jAlert(display_SomethingWrongHappenedContactAdmin);
            $("#load").toggle();
        }
    });


};

function gvAddrow_Click() {

    if ($("#ClientName").val() == "") {
        $("#ClientName").focus();
        return false;
    }
    if ($("#ProcessName").val() == "") {
        $("#ProcessName").focus();
        return false;
    }
    if ($("#CampaignName").val() == "") {
        $("#CampaignName").focus();
        return false;
    }


    var SearchViewPopUpWindow1 = $('#SearchViewPopUp').data('kendoWindow');
    SearchViewPopUpWindow1.refresh();
    SearchViewPopUpWindow1.center().open();

    //$('#SearchViewPopUp').data("kendoWindow").title("Language Config")
    //    .refresh({ url: ResourceLayout.partialURL + "SearchViewPopUp" }).center().open();

}

function Binding_WorkApproval() {
    //if ($("#Approvalgrid table tbody tr").length > 0) {
    //    var showApp = false;
    //    var showRej = false;
    //    var showCan = false;
    //    $("#spnNoRecord").hide();
    //    $(".btnAction").show();
    //    $("#Approvalgrid").show();
    //    $("#Approvalgrid table tbody tr").each(function () { if ($(this).find('td:eq(11)').html().length > 0) { showCan = true; } });
    //    $("#Approvalgrid table tbody tr").each(function () { if ($(this).find('td:eq(10)').html().length > 0) { showRej = true; } });
    //    $("#Approvalgrid table tbody tr").each(function () { if ($(this).find('td:eq(9)').html().length > 0) { showApp = true; } });
    //    if (showCan) { $("#BtnCancel").show(); } else { $("#BtnCancel").hide(); }
    //    if (showRej) { $("#BtnReject").show(); } else { $("#BtnReject").hide(); }
    //    if (showApp) { $("#BtnApprove").show(); } else { $("#BtnApprove").hide(); }
    //}
    //else {
    //    $("#spnNoRecord").show();
    //    $("#Approvalgrid").hide();
    //    $(".btnAction").hide();
    //}
}

$(function () {

    $('#Approvalgrid').on('click', '.chkbox', function () {
        var checked = $(this).is(':checked');
        var grid = $('#Approvalgrid').data().kendoGrid;
        var dataItem = grid.dataItem($(this).closest('tr'));
        dataItem.IsChecked = checked;
        dataItem.dirty = true;
    })
})

$("#CampaignNameSearch").on("change", function () {
    $("#Approvalgrid").data("kendoGrid").dataSource.read();
    $("#Approvalgrid").data("kendoGrid").refresh();
});

$(document).on("click", "#chkSelectAll", function (e) {
    if ($(this).is(":checked")) {
        $(".chkbox").prop("checked", "checked");
        var grid = $('#Approvalgrid').data().kendoGrid;
        if (grid != undefined) {
            if (grid._data != undefined) {
                if (grid._data.length > 0) {
                    for (var i = 0; i < grid._data.length; i++) {
                        grid._data[i].IsChecked = true;
                        grid._data[i].dirty = true;
                    }
                }
            }
        }
    }
    else {
        $(".chkbox").prop("checked", false);
        var grid = $('#Approvalgrid').data().kendoGrid;
        if (grid != undefined) {
            if (grid._data != undefined) {
                if (grid._data.length > 0) {
                    for (var i = 0; i < grid._data.length; i++) {
                        grid._data[i].IsChecked = false;
                        grid._data[i].dirty = true;
                    }
                }
            }
        }
    }
});

$("#ProfileName").on("change", function () {
    $("#Approvalgrid").data("kendoGrid").dataSource.read();
    $("#Approvalgrid").data("kendoGrid").refresh();
});

function GetFilter() {
    return {

        CampaignName: $("#CampaignNameSearch").val(),
        LanguageConfigID: $("#ProfileName").val()
    };
}


function Work_Approval(obj, id) {
    kendo.ui.progress($('#SearchViewForm'), true);
    var msg = '';
    var grid = $("#Approvalgrid").data("kendoGrid"),
        parameterMap = grid.dataSource.transport.parameterMap;
    var currentData = grid.dataSource.data();
    var updatedRecords = [];
    var CheckVal = 0;
    var token = $("#SearchViewForm input[name=__RequestVerificationToken]").val();
    // var ds = grid.dataSource.view();
    for (var i = 0; i < currentData.length; i++) {
        /// var row = grid.table.find("tr[data-uid='" + ds[i].uid + "']");
        // var checkbox = $(row).find(".k-checkbox k-valid");
        //if (checkbox.is(":checked")) {
        //    idsToSend.push(ds[i].ProductID);
        //}
        if (currentData[i].LngID == id) {
            if (currentData[i].dirty) {
                updatedRecords.push(currentData[i].toJSON());
            }
            if (currentData[i].IsChecked == true) {
                CheckVal = CheckVal + 1;
            }
            break;
        }
    }
    if (CheckVal > 0) {

        $.LoadingOverlay("show");
        $.ajax({
            type: "POST"
            , url: ResourceLayout.partialURL + "Approval"
            , data: { __RequestVerificationToken: token, updatedWorkApproval: updatedRecords, action: obj.value }
            , dataType: 'json'
            , success: function (result) {
                $.LoadingOverlay("hide");
                if (result.Value.Successed == true) {
                    jAlert(result.msg);
                }
                else if (result.Value.Successed == false) {
                    kendo.ui.progress($('#SearchViewForm'), false);
                    if (result.Value.strAction == display_Cancel) {
                        msg = cancel_workApporval
                    }
                    else if (result.Value.strAction == display_Reject) {
                        msg = reject_workApproval
                    }
                    else if (result.Value.strAction == display_Approve) {
                        msg = approve_workApproval
                    }
                    else {
                        msg = result.Value.msg;
                    }

                    jAlert(msg, 'Alert', function (r) {
                        if (r) {
                            $("#Approvalgrid").data("kendoGrid").dataSource.read();
                        }
                    });
                }
                else {
                    kendo.ui.progress($('#SearchViewForm'), false);
                    jAlert(result.Data.msg);
                }

            },
            error: function (err) {
                $.LoadingOverlay("hide");
                kendo.ui.progress($('#SearchViewForm'), false);

            }

        });
    }
    else {
        kendo.ui.progress($('#SearchViewForm'), false);
        jAlert("Please select checkbox Option !");
    }
}

function Save_multiWorkApproval(e) {

    var msg = '';
    var grid = $("#Approvalgrid").data("kendoGrid"),
        parameterMap = grid.dataSource.transport.parameterMap;
    var currentData = grid.dataSource.data();
    var updatedRecords = [];
    var CheckVal = 0;
    var token = $("#SearchViewForm input[name=__RequestVerificationToken]").val();
    for (var i = 0; i < currentData.length; i++) {
        if (currentData[i].dirty) {
            updatedRecords.push(currentData[i].toJSON());
        }
        if (currentData[i].IsChecked == true) {
            CheckVal = CheckVal + 1;
        }
    }
    if (CheckVal > 0) {
        var modelData = e.value;
        var data = {};
        $.extend(data, parameterMap({ updated: updatedRecords }), parameterMap({ modelData: modelData }));
        $.LoadingOverlay("show");
        $.ajax({
            url: ResourceLayout.partialURL + "ApprovalAction",
            data: { __RequestVerificationToken: token, updated: updatedRecords, modelData: modelData },
            type: "POST",
            error: function (err) {

                //Handle the server errors using the approach from the previous example
            },
            success: function (result) {
                $.LoadingOverlay("hide");
                if (result != null) {
                    if (result == display_Cancel) { msg = cancel_workApporval } else if (result == display_Reject) { msg = reject_workApproval } else if (result == display_Approve) { msg = approve_workApproval } else { msg = result.data }
                    jAlert(msg, 'Alert', function (r) {
                        if (r) {
                            $("#Approvalgrid").data("kendoGrid").dataSource.read();
                        }
                    });
                }
                else {
                    jAlert(result);
                }
            },
            error: function (err) {
                $.LoadingOverlay("hide");            
            }
        })
    }
    else {
        jAlert("Please select at least one checkbox Option !");
    }
}



function gvConfiguration_Click() {

}