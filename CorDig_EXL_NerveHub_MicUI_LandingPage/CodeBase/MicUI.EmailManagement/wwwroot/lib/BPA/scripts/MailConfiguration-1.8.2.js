
var checkedNodeValues = []

function GetData(e) {
    return { StoreID: $("#WorkObjName").val() }
}

function OnClickClose(e) {

    var MailConfigurationViewModel = {};
    var dataItem = [];
    var success = validateEmail($("#EmailID").val()); // Calls method that validate EMail ID
    var StoreID = $("#WorkObjName").val();      // StoreID added on  31/Aug/2016 by Nabin Kumar
    var EmailID = $('#EmailID').val();
    var UserID = $('#UserID').val();
    var Password = $('#Password').val();
    var MailServerTypeID = $('#MailServerTypeID').val();
    // var ServiceType = $('#ServiceType').val();
    var ServiceType = false;
    var ScheduleInterval = $('#ScheduleInterval').val();
    var AutoReplyTemplate = $('#AutoReplyTemplate').val();
    var AutoDiscoveryPath = $('#AutoDiscoveryPath').val();
    //var LotusServerPath = $('#LotusServerPath').val();
    var NFSFilePath = $('#NFSFilePath').val();
    var WebServerURL = $('#WebServerURL').val();
    var LotusServerPath = $('#LotusServerPath').val();
    var LotusDomainName = $('#LotusDomainName').val();
    var LotusDomainPrefix = $('#LotusDomainPrefix').val();

    var token = $("#formSearchView input[name=__RequestVerificationToken]").val();
    var ClientName = $('#ClientName').val();
    var ProcessName = $('#ProcessName').val();
    var CampaignName = $('#CampaignName').val();
    var MailConfigID = $('#MailConfigID').val();
    var FolderType = $('#FolderType').val();
    var UseServiceCredentialToPull = false;
    var UseUserCredentialToSend = false;
    var MailBoxName = $('#MailBoxName').val();
    var AutoReplyEnableDisable = false;
    var IsReadMail = false;
    var Disable = false;
    var WebEnabled = false;
    var bOutofOfficeEnabled = false;
    var sOutofOffice = $('#sOutofOffice').val();

    var bOutLookEnabled = false;
    var bTranslationEnabled = false;

    var bImpersonation = false;
    var sImpersonationIDType = $('#sImpersonationIDType').val();
    var sImpersonationID = $('#sImpersonationID').val();

    var GClinetID = $('#GClinetID').val();
    var TenentID = $('#TenentID').val();
    var Scope = $('#Scope').val();
    var RedirectUrl = $('#RedirectUrl').val();
    var Instance = $('#Instance').val();
    var IsForSWMIntegration = false;
    var IsSWMEMSIntegration = false;
    // Grid Change To Capture Grid Checked Data
    var grid = $('#grdSubFolder').data("kendoGrid");
    if (grid != undefined) {
        var gridRows = grid.tbody.find("tr");

        var i = 0
        gridRows.each(function (e) {
            var rowItem = grid.dataItem($(this));
            rowItem.Ingestion = $("#grdSubFolder tbody").find("tr").eq(i).find("td").eq(3).find("input").is(":checked");
            rowItem.Search = $("#grdSubFolder tbody").find("tr").eq(i).find("td").eq(4).find("input").is(":checked");
            rowItem.MoveFolder = $("#grdSubFolder tbody").find("tr").eq(i).find("td").eq(5).find("input").is(":checked");
            rowItem.Disable = $("#grdSubFolder tbody").find("tr").eq(i).find("td").eq(6).find("input").is(":checked");
            dataItem.push(rowItem);
            i++;
        });
    }
    // End Of Grid Change To Capture Grid Checked Data

    if ($('#Disable').is(':checked')) {
        Disable = true;
    }
    else {
        Disable = false;
    }

    if ($('#ServiceType').is(':checked')) {
        ServiceType = true;
    }
    else {
        ServiceType = false;
    }
    if ($('#bImpersonation').is(':checked')) {
        bImpersonation = true;
    }
    else {
        bImpersonation = false;
    }

    if ($('#UseServiceCredentialToPull').is(':checked')) {
        UseServiceCredentialToPull = true;
    }
    else {
        UseServiceCredentialToPull = false;
    }
    if ($('#UseUserCredentialToSend').is(':checked')) {
        UseUserCredentialToSend = true;
    }
    else {
        UseUserCredentialToSend = false;
    }
    if ($('#AutoReplyEnableDisable').is(':checked')) {
        AutoReplyEnableDisable = true;
    }
    else {
        AutoReplyEnableDisable = false;
    }
    if ($('#IsReadMail').is(':checked')) {
        IsReadMail = true;
    }
    else {
        IsReadMail = false;
    }

    if ($('#WebEnabled').is(':checked')) {
        WebEnabled = true;
    }
    else {
        WebEnabled = false;
    }

    if ($('#bOutofOfficeEnabled').is(':checked')) {
        bOutofOfficeEnabled = true;
    }
    else {
        bOutofOfficeEnabled = false;
    }
    if ($('#bOutLookEnabled').is(':checked')) {
        bOutLookEnabled = true;
    }
    else {
        bOutLookEnabled = false;
    }
    if ($('#bTranslationEnabled').is(':checked')) {
        bTranslationEnabled = true;
    }
    else {
        bTranslationEnabled = false;
    }

    if ($("#MailServerTypeID").val() == "") {
        jAlert(display_PleaseSelectExchangeServerType, "Alert", function (r) {
            $('#MailServerTypeID').focus();
        })
        return false;
    }

    if ($('#MailBoxName').val() == "") {
        jAlert(display_PleaseEnterMailBoxName, "Alert", function (r) {
            $('#MailBoxName').focus();
        })
        return false;
    }
    if (!success && UseServiceCredentialToPull == false) {
        jAlert(display_PleaseEnterEmailID, "Alert", function (r) {
            $('#EmailID').focus();
        })
        return false;
    }
    if ($("#EmailID").val() == "" && UseServiceCredentialToPull == false) {
        jAlert(display_PleaseEnterEmailID, "Alert", function (r) {
            $('#EmailID').focus();
        })
        return false;
    }
    if ($("#UserID").val() == "" && UseServiceCredentialToPull == false) {
        jAlert(display_PleaseEnterUserID, "Alert", function (r) {
            $('#UserID').focus();
        })

        return false;
    }
    //if ($("#Password").val() == "" && UseServiceCredentialToPull == false) {
    //  jAlert(display_PleaseEnterPassword, "Alert", function (r) {
    // $('#Password').focus();
    //  })

    //  return false;
    // }
    if ($("#Password").val() == "" && $("#AutoDiscoveryPath").val() == "") {
        jAlert(display_PleaseEnterPassword, "Alert", function (r) {
            $('#Password').focus();
        })

        return false;
    }


    if ($('#ScheduleInterval').val() == "") {
        jAlert(display_PleaseEnterScheduleInterval, "Alert", function (r) {
            $('#ScheduleInterval').focus();
        })
        return false;
    }


    if ($('#WebServerURL').val() == "") {
        jAlert(display_PleaseEnterWebServerURL, "Alert", function (r) {
            $('#WebServerURL').focus();
        })

        return false;
    }
    if ($("#MailServerTypeID").val() == "0") {
        if ($('#LotusServerPath').val() == "") {
            jAlert(display_PleaseEnterLotusServerPath, display_Alert, function (r) {
                $('#LotusServerPath').focus();
            })
            return false;
        }

        if ($('#NFSFilePath').val() == "") {
            jAlert(display_PleaseEnterNFSFilePath, display_Alert, function (r) {
                $('#NFSFilePath').focus();
            })
            return false;
        }

    }
    // Added By ManishDwivedi
    if ($('#IsForSWMIntegration').is(':checked')) {
        IsForSWMIntegration = true;
    }
    else {
        IsForSWMIntegration = false;
    }
    if ($('#IsSWMEMSIntegration').is(':checked')) {
        IsSWMEMSIntegration = true;
    }
    else {
        IsSWMEMSIntegration = false;
    }

    if ($("#MailServerTypeID").val() == "5") {
        if ($('#GClinetID').val() == "") {
            jAlert(display_PleaseEnterTheClinetid, display_Alert, function (r) {
                $('#GClinetID').focus();
            })
            return false;
        }
        if ($('#TenentID').val() == "") {
            jAlert(display_PleaseEnterTheTenentID, display_Alert, function (r) {
                $('#TenentID').focus();
            })
            return false;
        }
        if ($('#Scope').val() == "") {
            jAlert(display_PleaseEnterTheScope, display_Alert, function (r) {
                $('#Scope').focus();
            })
            return false;
        }
        if ($('#RedirectUrl').val() == "") {
            jAlert(display_PleaseEnterTheRedirectUrl, display_Alert, function (r) {
                $('#RedirectUrl').focus();
            })
            return false;
        }
        if ($('#Instance').val() == "") {
            jAlert(display_PleaseEnterTheInstance, display_Alert, function (r) {
                $('#Instance').focus();
            })
            return false;
        }
    }
    //end
    MailConfigurationViewModel.MailConfigID = MailConfigID;
    MailConfigurationViewModel.StoreID = StoreID;
    MailConfigurationViewModel.CampaignID = CampaignName;
    MailConfigurationViewModel.MailBoxName = MailBoxName
    MailConfigurationViewModel.EmailID = EmailID;
    MailConfigurationViewModel.UserID = UserID;
    MailConfigurationViewModel.Password = Password;
    MailConfigurationViewModel.MailServerTypeID = MailServerTypeID;
    MailConfigurationViewModel.ServiceType = ServiceType;
    MailConfigurationViewModel.ScheduleInterval = ScheduleInterval;
    MailConfigurationViewModel.AutoReplyTemplate = AutoReplyTemplate;
    MailConfigurationViewModel.AutoReplyEnableDisable = AutoReplyEnableDisable;
    MailConfigurationViewModel.AutoDiscoveryPath = AutoDiscoveryPath;
    MailConfigurationViewModel.LotusServerPath = LotusServerPath;
    MailConfigurationViewModel.LotusDomainName = LotusDomainName;
    MailConfigurationViewModel.LotusDomainPrefix = LotusDomainPrefix;
    MailConfigurationViewModel.NFSFilePath = NFSFilePath;
    MailConfigurationViewModel.WebEnabled = WebEnabled;
    MailConfigurationViewModel.bOutofOfficeEnabled = bOutofOfficeEnabled;
    MailConfigurationViewModel.bOutLookEnabled = bOutLookEnabled;
    MailConfigurationViewModel.bTranslationEnabled = bTranslationEnabled;

    MailConfigurationViewModel.sOutofOffice = sOutofOffice;
    MailConfigurationViewModel.WebServerURL = WebServerURL;
    MailConfigurationViewModel.IsReadMail = IsReadMail;
    MailConfigurationViewModel.UseServiceCredentialToPull = UseServiceCredentialToPull;
    MailConfigurationViewModel.bImpersonation = bImpersonation;
    MailConfigurationViewModel.sImpersonationID = sImpersonationID;
    MailConfigurationViewModel.sImpersonationIDType = sImpersonationIDType;
    MailConfigurationViewModel.UseUserCredentialToSend = UseUserCredentialToSend;
    MailConfigurationViewModel.FolderType = FolderType;
    MailConfigurationViewModel.Disable = Disable;
    // addesd by manishdwivedi
    MailConfigurationViewModel.GClinetID = GClinetID;
    MailConfigurationViewModel.TenentID = TenentID;
    MailConfigurationViewModel.Scope = Scope;
    MailConfigurationViewModel.RedirectUrl = RedirectUrl;
    MailConfigurationViewModel.IsForSWMIntegration = IsForSWMIntegration;
    MailConfigurationViewModel.IsSWMEMSIntegration = IsSWMEMSIntegration;

    MailConfigurationViewModel.Instance = Instance;
    //end

    var JsonData = [];
    if (dataItem != undefined) {
        for (var i = 0; i < dataItem.length; i++) {
            var GridList = {
                MailFolderDetailID: dataItem[i].MailFolderDetailID,
                Id: dataItem[i].Id,
                Name: dataItem[i].Name,
                Search: dataItem[i].Search,
                MoveFolder: dataItem[i].MoveFolder,
                Ingestion: dataItem[i].Ingestion,
                Disable: dataItem[i].Disable,
                MailFolderPath: dataItem[i].MailFolderPath
            }
            JsonData.push(GridList);
        }
    }

    var token = $("#form1 input[name=__RequestVerificationToken]").val();
    $.LoadingOverlay("show");
    $.ajax({
        type: 'Post'
        , url: ResourceLayout.partialURL + "InsertUpdateMailConfiguration"
        , data: ({ __RequestVerificationToken: token, objMailConfigurationViewModel: MailConfigurationViewModel, JData: JsonData })
        , dataType: 'json'
        , success: function (result) {
            $.LoadingOverlay("hide");
            var valcheck = result.split(',')[1];
            if (valcheck == 'OK') {
                jAlert(result.split(',')[0]);
                $('#gridMailConfig').data('kendoGrid').dataSource.read();
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


//add by alankar
$('#bScheduletosameuser').prop('disabled', true);
if ($('#bSendmailquiqueidentified').is(':checked')) {
    $('#bScheduletosameuser').prop('disabled', false);
    $('#bDuringUpload').prop('disabled', true);
    $('#bNeedTicket').prop('disabled', true);
    $('#bAssignLast').prop('disabled', true);
    $('#bFreshRequired').prop('disabled', true);
}

//$('#bNeedTicket').prop('disabled', true);
if ($('#bNeedTicket').is(':checked')) {
    $('#bScheduletosameuser').prop('disabled', true);
    $('#bSendmailquiqueidentified').prop('disabled', true);
    $('#bDuringUpload').prop('disabled', true);
    $('#bFreshRequired').prop('disabled', false);
    $('#bAssignLast').prop('disabled', false);
}
//$('#bDuringUpload').prop('disabled', true);
if ($('#bDuringUpload').is(':checked')) {
    $('#bScheduletosameuser').prop('disabled', true);
    $('#bSendmailquiqueidentified').prop('disabled', true);
    $('#bNeedTicket').prop('disabled', true);
    $('#bFreshRequired').prop('disabled', false);
    $('#bAssignLast').prop('disabled', false);
}

$('#bSendmailquiqueidentified').click(function () {
    if (!$(this).is(':checked')) {
        //alert("Test 2?");
        //   alert("Are you sure?");
        $('#bScheduletosameuser').prop('disabled', true);
        $('#bScheduletosameuser').prop('checked', false);

        $('#bDuringUpload').prop('disabled', false);
        $('#bNeedTicket').prop('disabled', false);
        $('#bAssignLast').prop('disabled', false);
        $('#bFreshRequired').prop('disabled', false);
    }
    else {
        $('#bDuringUpload').prop('disabled', true);
        $('#bDuringUpload').prop('checked', false);

        $('#bNeedTicket').prop('disabled', true);
        $('#bNeedTicket').prop('checked', false);

        $('#bFreshRequired').prop('disabled', true);
        $('#bFreshRequired').prop('checked', false);

        $('#bAssignLast').prop('disabled', true);
        $('#bAssignLast').prop('checked', false);
        // alert("Test 3?");
        //alert("Sure one?");
        $('#bScheduletosameuser').prop('disabled', false);

    }
});

$('#bDuringUpload').click(function () {
    if (!$(this).is(':checked')) {
        $('#bSendmailquiqueidentified').prop('disabled', false);
        $('#bNeedTicket').prop('disabled', false);
    }
    else {

        // $('#bScheduletosameuser').prop('disabled', false);

        $('#bScheduletosameuser').prop('disabled', true);
        $('#bScheduletosameuser').prop('checked', false);

        $('#bNeedTicket').prop('disabled', true);
        $('#bNeedTicket').prop('checked', false);

        $('#bSendmailquiqueidentified').prop('disabled', true);
        $('#bSendmailquiqueidentified').prop('checked', false);
    }
});

$('#bNeedTicket').click(function () {
    if (!$(this).is(':checked')) {
        $('#bSendmailquiqueidentified').prop('disabled', false);
        $('#bDuringUpload').prop('disabled', false);
    }
    else {

        // $('#bScheduletosameuser').prop('disabled', false);
        $('#bScheduletosameuser').prop('disabled', true);
        $('#bScheduletosameuser').prop('checked', false);

        $('#bDuringUpload').prop('disabled', true);
        $('#bDuringUpload').prop('checked', false);

        $('#bSendmailquiqueidentified').prop('disabled', true);
        $('#bSendmailquiqueidentified').prop('checked', false);
    }
});

function gvConfiguration_Click(e) {

    if ($("#ClientName").val() == "") {
        //  jAlert(display_PleaseSelectClient)
        $("#ClientName").focus();
        return false;
    }
    if ($("#ProcessName").val() == "") {
        //  jAlert(display_PleaseSelectProcess)
        $("#ProcessName").focus();
        return false;
    }
    if ($("#CampaignName").val() == "") {
        //jAlert(display_PleaseSelectCampaign)
        $("#CampaignName").focus();
        return false;
    }

    var MailConfigurationViewModel = {};
    var CampaignName = $('#CampaignName').val();
    MailConfigurationViewModel.CampaignID = CampaignName;
    MailConfigurationViewModel.MailConfigID = 1;
    var token = $("#form1 input[name=__RequestVerificationToken]").val();
    $.ajax({
        type: 'Post'
        , url: ResourceLayout.partialURL + "SetConfigurationID"
        , data: { __RequestVerificationToken: token, objMailConfigurationViewModel: MailConfigurationViewModel }
        , dataType: "json"
        , success: function (result) {

            if (result == "1") {

                var AdvanceConfigurationPopUp1 = $('#AdvanceConfigurationPopUp').data('kendoWindow');
                AdvanceConfigurationPopUp1.refresh();
                AdvanceConfigurationPopUp1.center().open();

                //$('#AdvanceConfigurationPopUp').data("kendoWindow").title(display_EMSAdvancedConfiguration).refresh({
                //    url: ResourceLayout.partialURL + "AdvanceConfigurationPopUp"
                //}).center().open();
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {

            jAlert(display_SomethingWrongHappenedContactAdmin, display_Alert);
            $("#load").toggle();
        }
    });
}

function OnClickSaveAdvnaceConfig(e) {

    var MailConfigurationViewModel = {};
    var SameUser = false;
    var Sendmail = false;
    var InlineEdit = false;
    var eNeedFile = false;
    var eNeedPrnt = false;
    var ReadMailBody = false;
    var CFX = false;
    var bOutLookMailEnabled = false;
    var DuringUpload = false;
    var FreshRequired = false;
    var NeedTicket = false;
    var AssignLast = false;

    if ($('#bAssignLast').is(':checked')) {
        AssignLast = true;
    }
    if ($('#bNeedTicket').is(':checked')) {
        NeedTicket = true;
    }
    if ($('#bFreshRequired').is(':checked')) {
        FreshRequired = true;
    }

    if ($('#bScheduletosameuser').is(':checked')) {
        SameUser = true;
    }
    if ($('#bSendmailquiqueidentified').is(':checked')) {
        Sendmail = true;
    }

    if ($('#bInlineEditing').is(':checked')) {
        InlineEdit = true;
    }

    if ($('#bNeedeFile').is(':checked')) {
        eNeedFile = true;
    }
    if ($('#bNeedPrint').is(':checked')) {
        eNeedPrnt = true;
    }
    if ($('#bReadMailBody').is(':checked')) {
        ReadMailBody = true;
    }
    if ($('#bCFX').is(':checked')) {
        CFX = true;
    }
    if ($('#bOutLookMailEnabled').is(':checked')) {
        bOutLookMailEnabled = true;
    }

    if ($('#bDuringUpload').is(':checked')) {
        DuringUpload = true;
    }
    var Sensitivity = false;
    if ($('#bSensitivity').is(':checked')) {
        Sensitivity = true;
    }

    var SubmitDisplay = false;
    if ($('#bSubmitDisplay').is(':checked')) {
        SubmitDisplay = true;
    }

    var CampaignName = $('#CampaignName').val();
    MailConfigurationViewModel.CampaignID = CampaignName;
    MailConfigurationViewModel.bScheduletosameuser = SameUser;
    MailConfigurationViewModel.bSendmailquiqueidentified = Sendmail;
    MailConfigurationViewModel.bInlineEditing = InlineEdit;

    MailConfigurationViewModel.AssignType = $("#AssignType").val();
    MailConfigurationViewModel.bAssignLast = AssignLast;
    MailConfigurationViewModel.bNeedeFile = eNeedFile;
    MailConfigurationViewModel.bNeedPrint = eNeedPrnt;
    MailConfigurationViewModel.bReadMailBody = ReadMailBody;
    MailConfigurationViewModel.bCFX = CFX;
    MailConfigurationViewModel.bOutLookMailEnabled = bOutLookMailEnabled;
    MailConfigurationViewModel.bDuringUpload = DuringUpload;

    MailConfigurationViewModel.bNeedTicket = NeedTicket
    MailConfigurationViewModel.sUploadBy = $("#sUploadBy").val();
    MailConfigurationViewModel.bSensitivity = Sensitivity;

    MailConfigurationViewModel.bSubmitDisplay = SubmitDisplay;

    MailConfigurationViewModel.sEfilePath = $("#sEfilePath").val();
    MailConfigurationViewModel.sEfilePath = $("#sEfilePath").val();
    MailConfigurationViewModel.sSubmitDisplayMsg = $("#sSubmitDisplayMsg").val();
    MailConfigurationViewModel.iNeedTicketLenth = $("#iNeedTicketLenth").val();
    MailConfigurationViewModel.iUploadBy = $("#iUploadBy").val();
    MailConfigurationViewModel.bFreshRequired = FreshRequired;
    MailConfigurationViewModel.sTicketName = $("#sTicketName").val();


    MailConfigurationViewModel.BatchFrequencyType = $("#BatchFrequencyType").val();
    MailConfigurationViewModel.iTimeZoneID = $("#iTimeZoneID").val();
    if (MailConfigurationViewModel.BatchFrequencyType == "" || MailConfigurationViewModel.BatchFrequencyType == undefined) {
        jAlert(msg_SelectBatchFrequency, display_Alert);
        return false;
    }
    var token = $("#form1 input[name=__RequestVerificationToken]").val();
    $.LoadingOverlay("show");
    $.ajax({
        type: 'Post'
        , url: ResourceLayout.partialURL + "InsertUpdateAdvancedConfiguration"
        , data: { __RequestVerificationToken: token, objMailConfigurationViewModel: MailConfigurationViewModel }
        , dataType: 'json'
        , success: function (result) {
            $.LoadingOverlay("hide");
            jAlert(result, display_Alert);
            $("#AdvanceConfigurationPopUp").closest(".k-window-content").data("kendoWindow").close();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $.LoadingOverlay("hide");
            jAlert(display_SomethingWrongHappenedContactAdmin, display_Alert);
            $("#load").toggle();
        }
    });
};

//end alankar
function gvAddrow_Click(e) {
    if ($("#ClientName").val() == "") {
        //jAlert(display_PleaseSelectClient)
        $("#ClientName").focus();
        return false;
    }
    if ($("#ProcessName").val() == "") {
        //jAlert(display_PleaseSelectProcess)
        $("#ProcessName").focus();
        return false;
    }
    if ($("#CampaignName").val() == "") {
        //jAlert(display_PleaseSelectCampaign)
        $("#CampaignName").focus();
        return false;
    }

    e.preventDefault();
    var SearchViewPopUpWindow1 = $('#SearchViewPopUp').data('kendoWindow');
    SearchViewPopUpWindow1.refresh();
    SearchViewPopUpWindow1.center().open();

    //$('#SearchViewPopUp').data("kendoWindow").title(display_EMSMailConfiguration)
    //    .refresh({ url: ResourceLayout.partialURL + "SearchViewPopUp" }).center().open();
    $('#grdSubFolder').html("");
}


function editRow(e) {
    debugger;
    var MailConfigurationViewModel = {};
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var MailConfigID = dataItem.iMailConfigID;
    var sEmailID = dataItem.sEmailID;
    var sUserID = dataItem.sUserID;
    var sPassword = dataItem.sPassword;
    var bUseServiceCredentialToPull = dataItem.bUseServiceCredentialToPull;
    var bUseUserCredentialToSend = dataItem.bUseUserCredentialToSend;
    var iMailServerTypeID = dataItem.iMailServerTypeID;
    var sAutoDiscoveryPath = dataItem.sAutoDiscoveryPath;
    var sLotusServerPath = dataItem.sLotusServerPath;
    var sLotusDomainName = dataItem.sLotusDomainName;
    var sLotusDomainPrefix = dataItem.sLotusDomainPrefix;
    var sNFSFilePath = dataItem.sNFSFilePath;
    var bWebEnabled = dataItem.bWebEnabled;
    var bEMSWebServerHosting = dataItem.bEMSWebServerHosting;
    var EMSWebServerURL = dataItem.EMSWebServerURL;
    var isPasswordExpire = dataItem.isPasswordExpire;
    var AutoReply = dataItem.AutoReply;
    var MailTemplateID = dataItem.MailTemplateID;
    var PoolingValue = dataItem.PoolingValue;
    var IsReadMail = dataItem.IsReadMail;
    var iFolderType = dataItem.iFolderType;
    var sMailBoxName = dataItem.sMailBoxName;
    var iScheduleInterval = dataItem.iScheduleInterval;
    var bDisabled = dataItem.bDisabled;
    var CampaignName = dataItem.iCampaignID;
    var bOutofOfficeEnabled = dataItem.bOutofOfficeEnabled;
    var bOutLookEnabled = dataItem.bOutLookEnabled;
    var bTranslationEnabled = dataItem.bTranslationEnabled;

    var sOutofOffice = dataItem.sOutofOffice;
    var bImpersonation = dataItem.bImpersonation;
    var sImpersonationIDType = dataItem.sImpersonationIDType;
    var sImpersonationID = dataItem.sImpersonationID;


    MailConfigurationViewModel.MailConfigID = MailConfigID;
    var token = $("#form1 input[name=__RequestVerificationToken]").val();
    $.LoadingOverlay("show");
    $.ajax({
        type: 'Post'
        , url: ResourceLayout.partialURL + "SetConfigurationID"
        , data: { __RequestVerificationToken: token, objMailConfigurationViewModel: MailConfigurationViewModel }
        , dataType: "json"
        , success: function (result) {
            $.LoadingOverlay("hide");
            if (result == "1") {
                //$('#SearchViewPopUp').data("kendoWindow").title(display_EMSMailConfiguration)
                //    .refresh({ url: ResourceLayout.partialURL + "SearchViewPopUp" }).center().open();
                var SearchViewPopUpWindow1 = $('#SearchViewPopUp').data('kendoWindow');
                SearchViewPopUpWindow1.refresh();
                SearchViewPopUpWindow1.center().open();
            }
        },
        error: function (err) {
            $.LoadingOverlay("hide");
        }
    });
}

$(document).ready(function (e) {
    debugger;
    if ($("#MailConfigID").val() != "0" && $("#MailConfigID").val() != undefined) {
        var pathname = window.location.pathname; // Returns path only
        var url = window.location.href;
        //var WebUrlPath = url.split('/')[0] + "//" + url.split('/')[2] + "/" + "WebUrlPath";

        //$('#WebServerURL').val(WebUrlPath);
        $("#treeview").html('');
        var data = [
            { Text: "Primary", Value: "1" },
            { Text: "Shared Mailbox", Value: "2" },
            { Text: "Public Folder", Value: "3" }
        ];
        var data3 = [
            { Text: "Select", Value: "0" },
            { Text: "PrincipalName", Value: "1" },
            { Text: "SID", Value: "2" },
            { Text: "SmtpAddress", Value: "3" }
        ];
        var data2 = [
            { Text: "Primary", Value: "1" }

        ];

        var MailConfigurationViewModel = {};
        var dataItem = [];
        var columnslist = [];
        columnslist.push({ field: "Id", hidden: true, width: 30 });
        columnslist.push({ field: "MailFolderPath", hidden: true, width: 30 });
        columnslist.push({ field: "Name", width: 230, title: display_Name });
        columnslist.push({ field: "Ingestion", title: display_Ingestion, width: 85, template: '<input type="checkbox" class="chkIngestion"  #= Ingestion ? checked="checked" : "" #  />' });
        columnslist.push({ field: "Search", title: display_Search, width: 80, template: '<input type="checkbox" class="chkSearch" #= Search ? checked="checked" : "" #  />' });
        columnslist.push({ field: "MoveFolder", title: display_MoveFolder, width: 100, template: '<input type="checkbox" class="chkMoveFolder" #= MoveFolder ? checked="checked" : "" # />' });
        columnslist.push({ field: "Disable", title: display_Disable, width: 80, template: '<input type="checkbox" class="chkDisable"   #= Disable ? checked="checked" : "" # />' });

        if ($("#MailServerTypeID").val() == 0) {
            $('#MainTable .Lotus').show();
            $('#MainTable .Outlook').hide();
        }
        else if ($("#MailServerTypeID").val() != 0) {
            $('#MainTable .Lotus').hide();
            $('#MainTable .Outlook').show();
        }
        if ($("#MailServerTypeID").val() == "") {
            $('#MainTable .Lotus').hide();
            $('#MainTable .Outlook').hide();
        }
        var ServiceType = false;
        var ClientName = $('#ClientName').val();
        var ProcessName = $('#ProcessName').val();
        var CampaignName = $('#CampaignName').val();
        var MailConfigID = $('#MailConfigID').val();
        var FolderType = $('#FolderType').val();
        var UseServiceCredentialToPull = false;
        var UseUserCredentialToSend = false;
        var MailBoxName = $('#MailBoxName').val();
        var AutoReplyEnableDisable = true;
        var sOutofOffice = $('#sOutofOffice').val();
        var IsReadMail = true;
        var bOutofOfficeEnabled = false;
        if ($('#bOutofOfficeEnabled').is(':checked')) {
            bOutofOfficeEnabled = true;
        }
        else {
            bOutofOfficeEnabled = false;
        }

        var bOutLookEnabled = false;
        if ($('#bOutLookEnabled').is(':checked')) {
            bOutLookEnabled = true;
        }
        else {
            bOutLookEnabled = false;
        }
        if ($('#bTranslationEnabled').is(':checked')) {
            bTranslationEnabled = true;
        }
        else {
            bTranslationEnabled = false;
        }

        var bImpersonation = false;
        var sImpersonationIDType = $('#sImpersonationIDType').val();
        var sImpersonationID = $('#sImpersonationID').val();
        if ($('#bImpersonation').is(':checked')) {
            bImpersonation = true;
            $('#MainTable .ImpersonationID').show();
            $('#MainTable .ImpersonationIDType').show();
        }
        else {
            bImpersonation = false;
            $('#MainTable .ImpersonationID').hide();
            $('#MainTable .ImpersonationIDType').hide();
        }
        var Disable = false;
        if ($('#Disable').is(':checked')) {
            Disable = true;
        }
        else {
            Disable = false;
        }
        if ($('#UseServiceCredentialToPull').is(':checked')) {
            UseServiceCredentialToPull = true;
        }
        else {
            UseServiceCredentialToPull = false;
        }
        if ($('#UseUserCredentialToSend').is(':checked')) {
            UseUserCredentialToSend = true;
        }
        else {
            UseUserCredentialToSend = false;
        }
        if ($('#AutoReplyEnableDisable').is(':checked')) {
            AutoReplyEnableDisable = true;
        }
        else {
            AutoReplyEnableDisable = false;
        }
        if ($('#IsReadMail').is(':checked')) {
            IsReadMail = true;
        }
        else {
            IsReadMail = false;
        }
        if ($('#ServiceType').is(':checked')) {
            ServiceType = true;
        }
        else {
            ServiceType = false;
        }
        var EmailID = $('#EmailID').val();
        var UserID = $('#UserID').val();
        var Password = $('#Password').val();
        var MailServerTypeID = $('#MailServerTypeID').val();
        var ScheduleInterval = $('#ScheduleInterval').val();
        var AutoReplyTemplate = $('#AutoReplyTemplate').val();
        var AutoDiscoveryPath = $('#AutoDiscoveryPath').val();
        //var LotusServerPath = $('#LotusServerPath').val();
        var NFSFilePath = $('#NFSFilePath').val();
        var WebEnabled = $('#WebEnabled').val();
        var WebServerURL = $('#WebServerURL').val();
        var LotusServerPath = $('#LotusServerPath').val();
        var LotusDomainName = $('#LotusDomainName').val();
        var LotusDomainPrefix = $('#LotusDomainPrefix').val();
        //var bOutofOfficeEnabled = $('#bOutofOfficeEnabled').val();
        var sOutofOffice = $('#sOutofOffice').val();
        //var bImpersonation = $('#bImpersonation').val();
        var sImpersonationIDType = $('#sImpersonationIDType').val();
        var sImpersonationID = $('#sImpersonationID').val();
        debugger;
        var GClinetID = $('#GClinetID').val();
        var TenentID = $('#TenentID').val();
        var Scope = $('#Scope').val();
        var RedirectUrl = $('#RedirectUrl').val();
        var Instance = $('#Instance').val();
        var IsForSWMIntegration = false;

        if ($('#IsForSWMIntegration').is(':checked')) {
            IsForSWMIntegration = true;
        }
        else {
            IsForSWMIntegration = false;
        }
        var IsSWMEMSIntegration = false;
        if ($('#IsSWMEMSIntegration').is(':checked')) {
            IsSWMEMSIntegration = true;
        }
        else {
            IsSWMEMSIntegration = false;
        }
        // jAlert($("#Instance").val(), display_Alert);
        if ($("#MailServerTypeID").val() == "0") {
            $('#MainTable .Lotus').show();
            $('#MainTable .Outlook').hide();
            $("#FolderType").kendoDropDownList({
                dataTextField: "Text",
                dataValueField: "Value",
                dataSource: data2,
                index: 0,
            });
        }
        else if ($("#MailServerTypeID").val() == "1" || $("#MailServerTypeID").val() == "2" || $("#MailServerTypeID").val() == "3") {
            $('#MainTable .Lotus').hide();
            $('#MainTable .Outlook').show();

            $("#FolderType").kendoDropDownList({
                dataTextField: "Text",
                dataValueField: "Value",
                dataSource: data,
                index: 0,
            });
        }
        if ($("#MailServerTypeID").val() == "5") {

            $('#MainTable .div_forgraph').show();
            $('#MainTable .SCID').show();
            $('#MainTable .auto').hide();
            // $('#AutoDiscoveryPath').val(Instance);
        }
        else {
            $('#MainTable .auto').show();
            $('#MainTable .SCID').hide();
            $('#MainTable .div_forgraph').hide();
            $('#AutoDiscoveryPath').val(AutoDiscoveryPath);

        }

        $("#sImpersonationIDType").kendoDropDownList({
            dataTextField: "Text",
            dataValueField: "Value",
            dataSource: data3,
            index: 0,
        });

        $('#MailBoxName').prop("disabled", true);
        if ($('#UseServiceCredentialToPull').is(':checked')) {
            $('#UserID').val('');
            $('#UserID').prop("disabled", true);
            $('#Password').val('');
            $('#Password').prop("disabled", true);
            $('#EmailID').val('');
            $('#EmailID').prop("disabled", true);
            $('#grdSubFolder').html("");
            if ($("#treeview").html() != "") {
                $("#treeview").data("kendoTreeView").dataSource.data([]);
            }
        }
        else {
            $('#UserID').prop("disabled", false);
            $('#Password').prop("disabled", false);
            $('#EmailID').prop("disabled", false);
        }
        MailConfigurationViewModel.MailConfigID = MailConfigID;
        MailConfigurationViewModel.CampaignID = CampaignName;
        MailConfigurationViewModel.MailBoxName = MailBoxName
        MailConfigurationViewModel.EmailID = EmailID;
        MailConfigurationViewModel.UserID = UserID;
        MailConfigurationViewModel.Password = Password;
        MailConfigurationViewModel.MailServerTypeID = MailServerTypeID;
        MailConfigurationViewModel.ServiceType = ServiceType;
        MailConfigurationViewModel.ScheduleInterval = ScheduleInterval;
        MailConfigurationViewModel.AutoReplyTemplate = AutoReplyTemplate;
        MailConfigurationViewModel.AutoReplyEnableDisable = AutoReplyEnableDisable;
        MailConfigurationViewModel.AutoDiscoveryPath = AutoDiscoveryPath;
        MailConfigurationViewModel.LotusServerPath = LotusServerPath;
        MailConfigurationViewModel.LotusDomainName = LotusDomainName;
        MailConfigurationViewModel.LotusDomainPrefix = LotusDomainPrefix;
        MailConfigurationViewModel.NFSFilePath = NFSFilePath;
        MailConfigurationViewModel.WebEnabled = WebEnabled;
        MailConfigurationViewModel.WebServerURL = WebServerURL;
        MailConfigurationViewModel.IsReadMail = IsReadMail;
        MailConfigurationViewModel.UseServiceCredentialToPull = UseServiceCredentialToPull;
        MailConfigurationViewModel.UseUserCredentialToSend = UseUserCredentialToSend;
        MailConfigurationViewModel.FolderType = FolderType;
        MailConfigurationViewModel.bOutofOfficeEnabled = bOutofOfficeEnabled;
        MailConfigurationViewModel.bOutLookEnabled = bOutLookEnabled;
        MailConfigurationViewModel.bTranslationEnabled = bTranslationEnabled;

        MailConfigurationViewModel.sOutofOffice = sOutofOffice;
        MailConfigurationViewModel.bImpersonation = bImpersonation;
        MailConfigurationViewModel.sImpersonationID = sImpersonationID;
        MailConfigurationViewModel.sImpersonationIDType = sImpersonationIDType;

        debugger;
        MailConfigurationViewModel.TenentID = TenentID;

        MailConfigurationViewModel.GClinetID = GClinetID;
        MailConfigurationViewModel.Scope = Scope;
        MailConfigurationViewModel.RedirectUrl = RedirectUrl;
        MailConfigurationViewModel.Instance = Instance;


        var dropdownFolderType = $("#FolderType").data("kendoDropDownList"),
            setValue = function (e) {
                if (e.type != "keypress" || kendo.keys.ENTER == e.keyCode)
                    dropdownlist.Value(FolderType);
            }
        MailConfigurationViewModel.Disable = Disable;


        var token = $("#formSearchView input[name=__RequestVerificationToken]").val();
        $("#load").show();
        $.ajax({
            type: 'POST'
            , url: ResourceLayout.partialURL + "TestConnection"
            , data: ({ __RequestVerificationToken: token, objMailConfigurationViewModel: MailConfigurationViewModel })     //, data: JSON.stringify({ UserID: UserIDVal, Password: PasswordVal, EmailID: EmailIDVal, MailServerTypeID: MailServerTypeIDVal, AutoDiscoveryPath: AutoDiscoveryPathVal, FolderType: FolderTypeVal, LotusPath: LotusPathVal,NFSFILEPath:NFSFILEPathVal })
            , dataType: 'json'
            , success: function (result) {

                if (result.Data != undefined) {
                    if (result.erorVal == "ERROR") {
                        jAlert(result.ErrorMessage, display_Alert);
                        $("#load").toggle();
                        //return;
                    }
                    else {

                        $("#treeview").kendoTreeView({
                            animation: false,
                            checkboxes: true,
                            dataSource: result.lst,
                            check: onCheck,
                            expand: onExpand

                        });
                        var treeView = $("#treeview").data("kendoTreeView");
                        var FolderList = result.Data;
                        //if (FolderType != 3) {
                        for (var i = 0; i < FolderList.length; i++) {
                            for (var j = 0; j < treeView.dataSource.view().length; j++) {
                                var dataSource = treeView.dataSource;
                                var dataItem = dataSource.get(FolderList[i].Id);
                                if (dataItem != undefined) {
                                    var node = treeView.dataSource.getByUid(dataItem.uid);
                                    if (node) {
                                        node.set("checked", true);
                                    }
                                }
                            }
                        }
                    }
                    //}
                    //To Bind Grid

                    if (result.Data.length > 0) {

                        $("#grdSubFolder").css("width", "600");
                        $("#grdSubFolder").css("height", "300");
                        $('#grdSubFolder').html("");
                        var dataSource = new kendo.data.DataSource({
                            //pageSize: 20,
                            data: result.Data,
                            autoSync: true,
                            schema: {
                                model: {
                                    Id: "Id",
                                    fields: {
                                        Id: { editable: false },
                                        MailFolderPath: { editable: false },
                                        Name: { editable: false },
                                        Ingestion: { type: "boolean" },
                                        Search: { type: "boolean" },
                                        MoveFolder: { type: "boolean" },
                                        Disable: { type: "boolean" },
                                    }
                                }
                            }
                        });

                        $("#grdSubFolder").kendoGrid({
                            columns: columnslist,
                            dataSource: dataSource,
                            height: 300,
                            editable: true,
                            filterable: true,
                            //selectable: "multiple row",
                            autoBind: true,
                        });
                    }
                    else {
                        $('#grdSubFolder').html("");
                    }
                }
                $("#load").hide();
                $(".k-in").css("border-left-width: 10px;");
            },
            error: function (err) {

                jAlert(display_SomethingWrongHappenedContactAdmin, display_Alert);
                $("#load").toggle();
            }
        });
    }

});

function OnChangeMailServerTypeID() {

    var data = [
        { Text: "Primary", Value: "1" },
        { Text: "Shared Mailbox", Value: "2" },
        { Text: "Public Folder", Value: "3" }
    ];
    var data2 = [
        { Text: "Primary", Value: "1" }

    ];
    $('#AutoDiscoveryPath').val('');
    if ($("#MailServerTypeID").val() == "0") {
        $('#MainTable .Lotus').show();
        $('#MainTable .Outlook').hide();
        $("#FolderType").kendoDropDownList({
            dataTextField: "Text",
            dataValueField: "Value",
            dataSource: data2,
            index: 0,
        });
    }
    else if ($("#MailServerTypeID").val() == "4") {
        $('#AutoDiscoveryPath').val(display_office365Exchange);
        $('#MainTable .Lotus').hide();
        $('#MainTable .Outlook').show();

        $("#FolderType").kendoDropDownList({
            dataTextField: "Text",
            dataValueField: "Value",
            dataSource: data,
            index: 0,
        });
    }
    else if ($("#MailServerTypeID").val() == "1" || $("#MailServerTypeID").val() == "2" || $("#MailServerTypeID").val() == "3") {
        $('#MainTable .Lotus').hide();
        $('#MainTable .Outlook').show();

        $("#FolderType").kendoDropDownList({
            dataTextField: "Text",
            dataValueField: "Value",
            dataSource: data,
            index: 0,
        });
    }
    if ($("#MailServerTypeID").val() == "") {
        $("#FolderType").kendoDropDownList({
            dataTextField: "Text",
            dataValueField: "Value",
            dataSource: data,
            index: 0,
        });
        $('#MainTable .Lotus').hide();
        $('#MainTable .Outlook').hide();
    }
    //jAlert($("#MailServerTypeID").val(), display_Alert);
    if ($("#MailServerTypeID").val() == "5") {
        //alert('show');
        $('#MainTable .div_forgraph').show();
        $('#MainTable .SCID').show();
        $('#MainTable .auto').hide();
        //$('#AutoDiscoveryPath').val($('#Instance').val());
    } else {
        //alert('hide');
        $('#MainTable .SCID').hide();
        $('#MainTable .auto').show();
        $('#MainTable .div_forgraph').hide();

    }
}
function Call() {
    $("#treeViewSearchInput").css({ "background": "whitesmoke" }); // whitesmoke
}
$('#UseServiceCredentialToPull').click(function (e) {
    if ($('#UseServiceCredentialToPull').is(':checked')) {
        $('#UserID').val('');
        $('#UserID').prop("disabled", true);
        $('#Password').val('');
        $('#Password').prop("disabled", true);
        //$('#EmailID').val('');
        // $('#EmailID').prop("disabled", true);
        $('#grdSubFolder').html("");
        if ($("#treeview").html() != "") {
            $("#treeview").data("kendoTreeView").dataSource.data([]);
        }
    }
    else {
        $('#UserID').prop("disabled", false);
        $('#Password').prop("disabled", false);
        $('#EmailID').prop("disabled", false);
    }


});
$('#UseUserCredentialToSend').click(function (e) {
    if ($('#UseServiceCredentialToPull').is(':checked')) {
    }
});

$('#bImpersonation').click(function (e) {

    if ($('#bImpersonation').is(':checked')) {
        $('#MainTable .ImpersonationID').show();
        $('#MainTable .ImpersonationIDType').show();
    }
    else {
        $('#MainTable .ImpersonationID').hide();
        $('#MainTable .ImpersonationIDType').hide();
        $("#sImpersonationID").val(' ');
    }

});


$('#AutoReplyEnableDisable').click(function (e) {
    $("#AutoReplyTemplate").data("kendoDropDownList").dataSource.read();
});

//        //function that gathers IDs of checked nodes
function checkedNodeIds(nodes, checkedNodes, checkedNodesValues, checkNodePath) {

    for (var i = 0; i < nodes.length; i++) {
        if (nodes[i].checked) {
            checkedNodes.push(nodes[i].text);
            checkedNodesValues.push(nodes[i].id);
            checkNodePath.push(nodes[i].parentId);
        }
        if (nodes[i].hasChildren) {
            checkedNodeIds(nodes[i].children.view(), checkedNodes, checkedNodesValues, checkNodePath);
        }
    }
}



//     show checked node IDs on datasource change
function onCheck(e) {
    var columnslist = [];
    debugger;
    var gr = $('#grdSubFolder').data('kendoGrid');
    if ($("#treeview").data('kendoTreeView').dataItem(e.node).checked) {
        if (gr != undefined) {
            var datasource = gr.dataSource;
            var newRecord = {
                Id: $("#treeview").data('kendoTreeView').dataItem(e.node).id,
                MailFolderPath: $("#treeview").data('kendoTreeView').dataItem(e.node).parentId,
                Name: $("#treeview").data('kendoTreeView').dataItem(e.node).text,
                Ingestion: false,
                Search: false,
                MoveFolder: false,
                Disable: false,
            };
            datasource.insert((gr.dataSource.length + 1), newRecord);
        }
        else {
            $("#grdSubFolder").css("width", "600");
            $("#grdSubFolder").css("height", "300");
            $('#grdSubFolder').html("");
            var newRecord = {
                Id: $("#treeview").data('kendoTreeView').dataItem(e.node).id,
                MailFolderPath: $("#treeview").data('kendoTreeView').dataItem(e.node).parentId,
                Name: $("#treeview").data('kendoTreeView').dataItem(e.node).text,
                Ingestion: false,
                Search: false,
                MoveFolder: false,
                Disable: false,

            };
            var dataSource = new kendo.data.DataSource({
                pageSize: 20,
                data: newRecord,
                autoSync: true,
                schema: {
                    model: {
                        Id: "Id",
                        fields: {
                            Id: { editable: false },
                            MailFolderPath: { editable: false },
                            Name: { editable: false },
                            Ingestion: { type: "boolean" },
                            Search: { type: "boolean" },
                            MoveFolder: { type: "boolean" },
                            Disable: { type: "boolean" },
                        }
                    }
                }
            });
            columnslist.push({ field: "Id", hidden: true, width: 30 });
            columnslist.push({ field: "MailFolderPath", hidden: true, width: 30 });
            columnslist.push({ field: "Name", width: 230, title: display_Name });
            columnslist.push({ field: "Ingestion", title: display_Ingestion, width: 85, template: '<input type="checkbox" class="chkIngestion"  #=Ingestion ? checked="checked" : "" #  />' });
            columnslist.push({ field: "Search", title: display_Search, width: 80, template: '<input type="checkbox" class="chkSearch" #=Search ? checked="checked" : "" #  />' });
            columnslist.push({ field: "MoveFolder", title: display_MoveFolder, width: 100, template: '<input type="checkbox" class="chkMoveFolder" #=MoveFolder ? checked="checked" : "" # />' });
            columnslist.push({ field: "Disable", title: display_Disable, width: 80, template: '<input type="checkbox" class="chkDisable"   #=Disable ? checked="checked" : "" # />' });
            $("#grdSubFolder").kendoGrid({
                columns: columnslist,
                dataSource: dataSource,
                height: 300,
                editable: true,
                filterable: true,
                //selectable: "multiple row",
                autoBind: true,
            });
            $("#grdSubFolder").kendoTooltip({
                filter: "td:nth-child(3)", //this filter selects the second column's cells
                position: "right",
                content: function (e) {
                    var dataItem = $("#grdSubFolder").data("kendoGrid").dataItem(e.target.closest("tr"));
                    var content = dataItem.MailFolderPath;
                    return content;
                }
            }).data("kendoTooltip");
        }
    }
    else {
        var datasource = gr.dataSource;

        for (var i = 0; i < datasource.data().length; i++) {
            if ($("#treeview").data('kendoTreeView').dataItem(e.node).id == datasource.data()[i].Id) {
                datasource.remove(datasource.data()[i]);
                break;
            }
        }

        if (datasource.data().length == 0) {
            $("#grdSubFolder").css("width", "400");
            $("#grdSubFolder").css("height", "300");
            $('#grdSubFolder').data('kendoGrid').destroy();
            $('#grdSubFolder').html("");
        }
    }


}

var objModel = [];
function onExpand(e) {

    var boolTree = false;
    if (objModel.length != 0) {
        for (var i = 0; i < objModel.length; i++) {
            if ($("#treeview").data('kendoTreeView').dataItem(e.node).id == objModel[i].id) {
                boolTree = true;
                break;
            }
            else {

                boolTree = false;
            }
        }
    }

    if (!boolTree) {
        objModel.push({ id: $("#treeview").data('kendoTreeView').dataItem(e.node).id });
        var treeview = $("#treeview").data("kendoTreeView");
        treeview.select(e.node);

        var selectedNode = treeview.select();

        var childNodes = $(".k-item", e.node);
        treeview.remove(childNodes);
        //setTimeout(function () {
        //   
        //    $("#FirstColumn").find(".k-state-selected").parent().next().find(".k-item").eq().remove();
        //}, 100);



        var UserIDVal = $("#UserID").val();
        var PasswordVal = $("#Password").val();
        var EmailIDVal = $("#EmailID").val();
        var MailServerTypeIDVal = $("#MailServerTypeID").val();
        var AutoDiscoveryPathVal = $("#AutoDiscoveryPath").val();


        var FolderTypeVal = $("#FolderType").val();
        var LotusPathVal = $("#LotusServerPath").val();
        var LotusDomainName = $('#LotusDomainName').val();
        var LotusDomainPrefix = $('#LotusDomainPrefix').val();
        var NFSFILEPathVal = $("#NFSFilePath").val();
        var Items = [];
        var success = validateEmail($("#EmailID").val());
        var MailConfigurationViewModel = {};
        var dataItem = [];
        var ClientName = $('#ClientName').val();
        var ProcessName = $('#ProcessName').val();
        var CampaignName = $('#CampaignName').val();
        var MailConfigID = $('#MailConfigID').val();
        var FolderType = $('#FolderType').val();
        var UseServiceCredentialToPull = false;
        var UseUserCredentialToSend = false;
        var MailBoxName = $('#MailBoxName').val();
        var sOutofOffice = $('#sOutofOffice').val();
        var AutoReplyEnableDisable = true;
        var IsReadMail = true;
        var Disable = false;
        var ServiceType = false;
        if ($('#ServiceType').is(':checked')) {
            ServiceType = true;
        }
        else {
            ServiceType = false;
        }
        var bImpersonation = false;
        var sImpersonationIDType = $('#sImpersonationIDType').val();
        var sImpersonationID = $('#sImpersonationID').val();
        if ($('#bImpersonation').is(':checked')) {
            bImpersonation = true;
        }
        else {
            bImpersonation = false;
        }
        var bOutofOfficeEnabled = false;
        if ($('#bOutofOfficeEnabled').is(':checked')) {
            bOutofOfficeEnabled = true;
        }
        else {
            bOutofOfficeEnabled = false;
        }

        var bOutLookEnabled = false;
        if ($('#bOutLookEnabled').is(':checked')) {
            bOutLookEnabled = true;
        }
        else {
            bOutLookEnabled = false;
        }
        var bTranslationEnabled = false;
        if ($('#bTranslationEnabled').is(':checked')) {
            bTranslationEnabled = true;
        }
        else {
            bTranslationEnabled = false;
        }

        if ($('#Disable').is(':checked')) {
            Disable = true;
        }
        else {
            Disable = false;
        }
        if ($('#UseServiceCredentialToPull').is(':checked')) {
            UseServiceCredentialToPull = true;
        }
        else {
            UseServiceCredentialToPull = false;
        }
        if ($('#UseUserCredentialToSend').is(':checked')) {
            UseUserCredentialToSend = true;
        }
        else {
            UseUserCredentialToSend = false;
        }
        if ($('#AutoReplyEnableDisable').is(':checked')) {
            AutoReplyEnableDisable = true;
        }
        else {
            AutoReplyEnableDisable = false;
        }
        if ($('#IsReadMail').is(':checked')) {
            IsReadMail = true;
        }
        else {
            IsReadMail = false;
        }
        var EmailID = $('#EmailID').val();
        var UserID = $('#UserID').val();
        var Password = $('#Password').val();
        var MailServerTypeID = $('#MailServerTypeID').val();
        var ServiceType = $('#ServiceType').val();
        var ScheduleInterval = $('#ScheduleInterval').val();
        var AutoReplyTemplate = $('#AutoReplyTemplate').val();
        var AutoDiscoveryPath = $('#AutoDiscoveryPath').val();
        var LotusServerPath = $('#LotusServerPath').val();
        var LotusDomainName = $('#LotusDomainName').val();
        var LotusDomainPrefix = $('#LotusDomainPrefix').val();
        var NFSFilePath = $('#NFSFilePath').val();
        var WebEnabled = $('#WebEnabled').val();
        var WebServerURL = $('#WebServerURL').val();
        // var LotusServerPath = $('#LotusServerPath').val();

        if ($("#MailServerTypeID").val() == "") {
            jAlert(display_PleaseSelectExchangeServerType, display_Alert, function (r) {
                $('#MailServerTypeID').focus();
            })
            return false;
        }



        if ($("#FolderType").val() == "") {
            jAlert(display_PleaseSelectFolderType, display_Alert, function (r) {
                $('#FolderType').focus();
            })

            return false;
        }

        if ($('#MailBoxName').val() == "") {
            jAlert(display_PleaseEnterMailBoxName, display_Alert, function (r) {
                $('#MailBoxName').focus();
            })
            return false;
        }
        if (!success && UseServiceCredentialToPull == false) {
            jAlert(display_PleaseEnterEmailID, display_Alert, function (r) {
                $('#EmailID').focus();
            })
            return false;
        }
        if ($("#EmailID").val() == "" && UseServiceCredentialToPull == false) {
            jAlert(display_PleaseEnterEmailID, display_Alert, function (r) {
                $('#EmailID').focus();
            })
            return false;
        }
        if ($("#UserID").val() == "" && UseServiceCredentialToPull == false) {
            jAlert(display_PleaseEnterUserID, display_Alert, function (r) {
                $('#UserID').focus();
            })

            return false;
        }
        if ($("#Password").val() == "" && UseServiceCredentialToPull == false) {
            jAlert(display_PleaseEnterPassword, display_Alert, function (r) {
                $('#Password').focus();
            })

            return false;
        }


        if ($('#ScheduleInterval').val() == "") {
            jAlert(display_PleaseEnterScheduleInterval, display_Alert, function (r) {
                $('#ScheduleInterval').focus();
            })
            return false;
        }
        if (MailServerTypeIDVal != 0) {
            if (AutoDiscoveryPathVal == '') {
                jAlert(required_AutoDiscoveryPath, display_Alert, function (r) {
                    $('#AutoDiscoveryPath').focus();
                })
                return false;
            }
        }

        if ($("#MailServerTypeID").val() == display_DominoServer) {
            if ($('#LotusServerPath').val() == "") {
                jAlert(display_PleaseEnterLotusServerPath, display_Alert, function (r) {
                    $('#LotusServerPath').focus();
                })
                return false;
            }

            if ($('#NFSFilePath').val() == "") {
                jAlert(display_PleaseEnterNFSFilePath, display_Alert, function (r) {
                    $('#NFSFilePath').focus();
                })
                return false;
            }

        }
        var GClinetID = $('#GClinetID').val();
        var TenentID = $('#TenentID').val();
        var Scope = $('#Scope').val();
        var RedirectUrl = $('#RedirectUrl').val();
        var Instance = $('#Instance').val();
        var IsForSWMIntegration = false;

        if ($('#IsForSWMIntegration').is(':checked')) {
            IsForSWMIntegration = true;
        }
        else {
            IsForSWMIntegration = false;
        }

        var IsSWMEMSIntegration = false;

        if ($('#IsSWMEMSIntegration').is(':checked')) {
            IsSWMEMSIntegration = true;
        }
        else {
            IsSWMEMSIntegration = false;
        }

        MailConfigurationViewModel.IsForSWMIntegration = IsForSWMIntegration;
        MailConfigurationViewModel.IsSWMEMSIntegration = IsSWMEMSIntegration;
        MailConfigurationViewModel.Instance = Instance;
        MailConfigurationViewModel.RedirectUrl = RedirectUrl
        MailConfigurationViewModel.Scope = Scope;
        MailConfigurationViewModel.TenentID = TenentID;
        MailConfigurationViewModel.GClinetID = GClinetID;

        MailConfigurationViewModel.MailConfigID = MailConfigID;
        MailConfigurationViewModel.CampaignID = CampaignName;
        MailConfigurationViewModel.MailBoxName = MailBoxName
        MailConfigurationViewModel.EmailID = EmailID;
        MailConfigurationViewModel.UserID = UserID;
        MailConfigurationViewModel.Password = Password;
        MailConfigurationViewModel.MailServerTypeID = MailServerTypeID;
        MailConfigurationViewModel.ServiceType = ServiceType;
        MailConfigurationViewModel.ScheduleInterval = ScheduleInterval;
        MailConfigurationViewModel.AutoReplyTemplate = AutoReplyTemplate;
        MailConfigurationViewModel.AutoReplyEnableDisable = AutoReplyEnableDisable;
        MailConfigurationViewModel.AutoDiscoveryPath = AutoDiscoveryPath;
        MailConfigurationViewModel.LotusServerPath = LotusServerPath;
        MailConfigurationViewModel.LotusDomainName = LotusDomainName;
        MailConfigurationViewModel.LotusDomainPrefix = LotusDomainPrefix;
        MailConfigurationViewModel.NFSFilePath = NFSFilePath;
        MailConfigurationViewModel.WebEnabled = WebEnabled;
        MailConfigurationViewModel.bOutofOfficeEnabled = bOutofOfficeEnabled;
        MailConfigurationViewModel.bOutLookEnabled = bOutLookEnabled;
        MailConfigurationViewModel.bTranslationEnabled = bTranslationEnabled;

        MailConfigurationViewModel.sOutofOffice = sOutofOffice;
        MailConfigurationViewModel.WebServerURL = WebServerURL;
        MailConfigurationViewModel.IsReadMail = IsReadMail;
        MailConfigurationViewModel.UseServiceCredentialToPull = UseServiceCredentialToPull;
        MailConfigurationViewModel.UseUserCredentialToSend = UseUserCredentialToSend;
        MailConfigurationViewModel.bImpersonation = bImpersonation;
        MailConfigurationViewModel.sImpersonationID = sImpersonationID;
        MailConfigurationViewModel.sImpersonationIDType = sImpersonationIDType;
        MailConfigurationViewModel.FolderType = FolderType;
        MailConfigurationViewModel.Disable = Disable;
        MailConfigurationViewModel.FolderID = $("#treeview").data('kendoTreeView').dataItem(e.node).id;
        MailConfigurationViewModel.ParentFolderName = $("#treeview").data('kendoTreeView').dataItem(e.node).parentId;
        $("#load").show();
        var token = $("#form1 input[name=__RequestVerificationToken]").val();
        $.LoadingOverlay("show");
        $.ajax({
            type: 'Post'
            , url: ResourceLayout.partialURL + "GetNodeValue"
            , data: { __RequestVerificationToken: token, objMailConfigurationViewModel: MailConfigurationViewModel }      //, data: JSON.stringify({ UserID: UserIDVal, Password: PasswordVal, EmailID: EmailIDVal, MailServerTypeID: MailServerTypeIDVal, AutoDiscoveryPath: AutoDiscoveryPathVal, FolderType: FolderTypeVal, LotusPath: LotusPathVal,NFSFILEPath:NFSFILEPathVal })
            , dataType: 'json'

            , success: function (result) {
                $.LoadingOverlay("hide");
                if (selectedNode.length == 0) {
                    $("#load").hide();
                    selectedNode = null;
                }
                for (var i = 0; i < result.lst.length; i++) {
                    treeview.append({
                        id: result.lst[i].id,
                        text: result.lst[i].text,
                        parentId: result.lst[i].parentId,
                        items: result.lst[i].items
                    }, selectedNode);
                }
                if ($("#grdSubFolder").data("kendoGrid") != undefined) {
                    if ($("#grdSubFolder").data("kendoGrid").dataSource != undefined) {

                        if ($("#grdSubFolder").data("kendoGrid").dataSource.data() != undefined) {
                            for (var i = 0; i < $("#grdSubFolder").data("kendoGrid").dataSource.data().length; i++) {
                                for (var j = 0; j < $("#treeview").data('kendoTreeView').dataItem(e.node).items.length; j++) {
                                    if ($("#treeview").data('kendoTreeView').dataItem(e.node).items[j] != undefined && $("#grdSubFolder").data("kendoGrid").dataSource.data()[i].Id == $("#treeview").data('kendoTreeView').dataItem(e.node).items[j].id) {

                                        $("#treeview").data('kendoTreeView').dataItem(e.node).items[j].set("checked", true)
                                        // $($("#treeview").data('kendoTreeView').dataItem(e.node).items[i]).prop("checked", "checked");
                                    }
                                }
                            }
                        }
                    }
                    $(".k-in").css("border-left-width: 10px;");
                }
                $("#load").hide();
            },
            error: function (err) {
                $.LoadingOverlay("hide");
                jAlert(err.ErrorMessage, display_Alert);
                $("#load").hide();
            }

        });
    }
}

function GetTemplateValue() {
    //  setTimeout(function () {
    console.log($('#AutoReplyEnableDisable').is(':checked'));
    return {
        isDisabled: $('#Disable').is(':checked'),
        isAutoReply: $('#AutoReplyEnableDisable').is(':checked')
    };
    // }, 2500)
}


$("#BtnTestConnection").unbind("click").click(function (e) {

    debugger;
    objModel = [];
    var UserIDVal = $("#UserID").val();
    var PasswordVal = $("#Password").val();
    var EmailIDVal = $("#EmailID").val();
    var MailServerTypeIDVal = $("#MailServerTypeID").val();
    var AutoDiscoveryPathVal = $("#AutoDiscoveryPath").val();


    var FolderTypeVal = $("#FolderType").val();
    var LotusPathVal = $("#LotusServerPath").val();
    var LotusDomainName = $('#LotusDomainName').val();
    var LotusDomainPrefix = $('#LotusDomainPrefix').val();
    var NFSFILEPathVal = $("#NFSFilePath").val();
    var Items = [];
    var success = validateEmail($("#EmailID").val());
    var MailConfigurationViewModel = {};
    var dataItem = [];
    var ClientName = $('#ClientName').val();
    var ProcessName = $('#ProcessName').val();
    var CampaignName = $('#CampaignName').val();
    var MailConfigID = $('#MailConfigID').val();
    var FolderType = $('#FolderType').val();
    var UseServiceCredentialToPull = false;
    var UseUserCredentialToSend = false;
    var MailBoxName = $('#MailBoxName').val();
    var AutoReplyEnableDisable = true;
    var IsReadMail = true;
    var Disable = false;
    var bImpersonation = false;
    var sImpersonationIDType = $('#sImpersonationIDType').val();
    var sImpersonationID = $('#sImpersonationID').val();
    if ($('#bImpersonation').is(':checked')) {
        bImpersonation = true;
    }
    else {
        bImpersonation = false;
    }

    var bOutofOfficeEnabled = false;
    var sOutofOffice = $('#sOutofOffice').val();

    if ($('#bOutofOfficeEnabled').is(':checked')) {
        bOutofOfficeEnabled = true;
    }
    else {
        bOutofOfficeEnabled = false;
    }
    var bOutLookEnabled = false;

    if ($('#bOutLookEnabled').is(':checked')) {
        bOutLookEnabled = true;
    }
    else {
        bOutLookEnabled = false;
    }
    var bTranslationEnabled = false;

    if ($('#bTranslationEnabled').is(':checked')) {
        bTranslationEnabled = true;
    }
    else {
        bTranslationEnabled = false;
    }

    if ($('#Disable').is(':checked')) {
        Disable = true;
    }
    else {
        Disable = false;
    }
    if ($('#UseServiceCredentialToPull').is(':checked')) {
        UseServiceCredentialToPull = true;
    }
    else {
        UseServiceCredentialToPull = false;
    }
    if ($('#UseUserCredentialToSend').is(':checked')) {
        UseUserCredentialToSend = true;
    }
    else {
        UseUserCredentialToSend = false;
    }
    if ($('#AutoReplyEnableDisable').is(':checked')) {
        AutoReplyEnableDisable = true;
    }
    else {
        AutoReplyEnableDisable = false;
    }
    if ($('#IsReadMail').is(':checked')) {
        IsReadMail = true;
    }
    else {
        IsReadMail = false;
    }
    var EmailID = $('#EmailID').val();
    var UserID = $('#UserID').val();
    var Password = $('#Password').val();
    var MailServerTypeID = $('#MailServerTypeID').val();
    var ServiceType = $('#ServiceType').val();
    var ScheduleInterval = $('#ScheduleInterval').val();
    var AutoReplyTemplate = $('#AutoReplyTemplate').val();
    var AutoDiscoveryPath = $('#AutoDiscoveryPath').val();
    var LotusServerPath = $('#LotusServerPath').val();
    var LotusDomainName = $('#LotusDomainName').val();
    var LotusDomainPrefix = $('#LotusDomainPrefix').val();
    var NFSFilePath = $('#NFSFilePath').val();
    var WebEnabled = $('#WebEnabled').val();
    var WebServerURL = $('#WebServerURL').val();
    //var LotusServerPath = $('#LotusServerPath').val();

    if ($("#MailServerTypeID").val() == "") {
        jAlert(display_PleaseSelectExchangeServerType, display_Alert, function (r) {
            $('#MailServerTypeID').focus();
        })
        return false;
    }



    if ($("#FolderType").val() == "") {
        jAlert(display_PleaseSelectFolderType, display_Alert, function (r) {
            $('#FolderType').focus();
        })

        return false;
    }

    if ($('#MailBoxName').val() == "") {
        jAlert(display_PleaseEnterMailBoxName, display_Alert, function (r) {
            $('#MailBoxName').focus();
        })
        return false;
    }
    if (!success && UseServiceCredentialToPull == false) {
        jAlert(display_PleaseEnterEmailID, display_Alert, function (r) {
            $('#EmailID').focus();
        })
        return false;
    }
    if ($("#EmailID").val() == "" && UseServiceCredentialToPull == false) {
        jAlert(display_PleaseEnterEmailID, display_Alert, function (r) {
            $('#EmailID').focus();
        })
        return false;
    }
    if ($("#UserID").val() == "" && UseServiceCredentialToPull == false) {
        jAlert(display_PleaseEnterUserID, display_Alert, function (r) {
            $('#UserID').focus();
        })

        return false;
    }
    if ($("#Password").val() == "" && UseServiceCredentialToPull == false) {
        jAlert(display_PleaseEnterPassword, display_Alert, function (r) {
            $('#Password').focus();
        })

        return false;
    }


    if ($('#ScheduleInterval').val() == "") {
        jAlert(display_PleaseEnterScheduleInterval, display_Alert, function (r) {
            $('#ScheduleInterval').focus();
        })
        return false;
    }
    if (MailServerTypeIDVal != 0) {
        if (AutoDiscoveryPathVal == '') {
            jAlert(required_AutoDiscoveryPath, display_Alert, function (r) {
                $('#AutoDiscoveryPath').focus();
            })
            return false;
        }
    }

    if ($("#MailServerTypeID").val() == "0") {
        if ($('#LotusServerPath').val() == "") {
            jAlert(display_PleaseEnterLotusServerPath, display_Alert, function (r) {
                $('#LotusServerPath').focus();
            })
            return false;
        }

        if ($('#NFSFilePath').val() == "") {
            jAlert(display_PleaseEnterNFSFilePath, display_Alert, function (r) {
                $('#NFSFilePath').focus();
            })
            return false;
        }

    }

    var GClinetID = $('#GClinetID').val();
    var TenentID = $('#TenentID').val();
    var Scope = $('#Scope').val();
    var RedirectUrl = $('#RedirectUrl').val();
    var Instance = $('#Instance').val();
    var IsForSWMIntegration = false;

    if ($('#IsForSWMIntegration').is(':checked')) {
        IsForSWMIntegration = true;
    }
    else {
        IsForSWMIntegration = false;
    }

    var IsSWMEMSIntegration = false;

    if ($('#IsSWMEMSIntegration').is(':checked')) {
        IsSWMEMSIntegration = true;
    }
    else {
        IsSWMEMSIntegration = false;
    }

    MailConfigurationViewModel.IsSWMEMSIntegration = IsSWMEMSIntegration;
    MailConfigurationViewModel.IsForSWMIntegration = IsForSWMIntegration;
    MailConfigurationViewModel.Instance = Instance;
    MailConfigurationViewModel.RedirectUrl = RedirectUrl
    MailConfigurationViewModel.Scope = Scope;
    MailConfigurationViewModel.TenentID = TenentID;
    MailConfigurationViewModel.GClinetID = GClinetID;
    MailConfigurationViewModel.MailConfigID = MailConfigID;
    MailConfigurationViewModel.CampaignID = CampaignName;
    MailConfigurationViewModel.MailBoxName = MailBoxName
    MailConfigurationViewModel.EmailID = EmailID;
    MailConfigurationViewModel.UserID = UserID;
    MailConfigurationViewModel.Password = Password;
    MailConfigurationViewModel.MailServerTypeID = MailServerTypeID;
    MailConfigurationViewModel.ServiceType = ServiceType;
    MailConfigurationViewModel.ScheduleInterval = ScheduleInterval;
    MailConfigurationViewModel.AutoReplyTemplate = AutoReplyTemplate;
    MailConfigurationViewModel.AutoReplyEnableDisable = AutoReplyEnableDisable;
    MailConfigurationViewModel.AutoDiscoveryPath = AutoDiscoveryPath;
    MailConfigurationViewModel.LotusServerPath = LotusServerPath;
    MailConfigurationViewModel.LotusDomainName = LotusDomainName;
    MailConfigurationViewModel.LotusDomainPrefix = LotusDomainPrefix;
    MailConfigurationViewModel.sOutofOffice = sOutofOffice;
    MailConfigurationViewModel.bOutofOfficeEnabled = bOutofOfficeEnabled;
    MailConfigurationViewModel.bOutLookEnabled = bOutLookEnabled;
    MailConfigurationViewModel.bTranslationEnabled = bTranslationEnabled;

    MailConfigurationViewModel.NFSFilePath = NFSFilePath;
    MailConfigurationViewModel.WebEnabled = WebEnabled;
    MailConfigurationViewModel.WebServerURL = WebServerURL;
    MailConfigurationViewModel.IsReadMail = IsReadMail;
    MailConfigurationViewModel.UseServiceCredentialToPull = UseServiceCredentialToPull;
    MailConfigurationViewModel.UseUserCredentialToSend = UseUserCredentialToSend;
    MailConfigurationViewModel.FolderType = FolderType;
    MailConfigurationViewModel.Disable = Disable;
    MailConfigurationViewModel.bImpersonation = bImpersonation;
    MailConfigurationViewModel.sImpersonationID = sImpersonationID;
    MailConfigurationViewModel.sImpersonationIDType = sImpersonationIDType;
    debugger;

    var MailConfigurationViewModelNew =
    {
        MailConfigID: MailConfigID
    }

    $("#load").show();
    var token = $("#formSearchView input[name=__RequestVerificationToken]").val();
    $.LoadingOverlay("show");
    $.ajax({
        type: 'POST'
        , url: ResourceLayout.partialURL + "TestConnection"
        , data: ({ __RequestVerificationToken: token, objMailConfigurationViewModel: MailConfigurationViewModel })     //, data: JSON.stringify({ UserID: UserIDVal, Password: PasswordVal, EmailID: EmailIDVal, MailServerTypeID: MailServerTypeIDVal, AutoDiscoveryPath: AutoDiscoveryPathVal, FolderType: FolderTypeVal, LotusPath: LotusPathVal,NFSFILEPath:NFSFILEPathVal })
        , dataType: 'json'
        , success: function (result) {
            $.LoadingOverlay("hide");
            if (result.erorVal == "ERROR") {
                jAlert(result.ErrorMessage, display_Alert);
                $("#load").hide();
            }
            else {


                //$('#grdSubFolder').html("");
                if ($("#treeview").html() != "") {
                    $("#treeview").data("kendoTreeView").dataSource.data(result.lst);

                }
                else {

                    $("#treeview").kendoTreeView({
                        animation: false,
                        checkboxes: true,
                        dataSource: result.lst,
                        check: onCheck,
                        expand: onExpand

                    });
                }

                var FolderList = result.Data;
                //if (FolderType != 3) {
                for (var i = 0; i < FolderList.length; i++) {
                    for (var j = 0; j < $("#treeview").data("kendoTreeView").dataSource.view().length; j++) {
                        var dataSource = $("#treeview").data("kendoTreeView").dataSource;
                        var dataItem = dataSource.get(FolderList[i].Id);
                        if (dataItem != undefined) {
                            var node = $("#treeview").data("kendoTreeView").dataSource.getByUid(dataItem.uid);
                            if (node) {
                                node.set("checked", true);
                            }
                        }
                    }
                }

                $(".k-in").css("border-left-width: 10px;");
            }
            $("#load").hide();
        },
        error: function (err) {
            $.LoadingOverlay("hide");
            jAlert(err.ErrorMessage, display_Alert);
            $("#load").hide();
        }
    });
}
);

function validateEmail(email) {
    var re = /^\s*[\w\-\+_]+(\.[\w\-\+_]+)*\@[\w\-\+_]+\.[\w\-\+_]+(\.[\w\-\+_]+)*\s*$/;
    if (re.test(email.replace(/^\s+|\s+$/gm, ''))) {
        return true;

    } else {
        return false;
    }
};

$('#WebServerURL').keyup(function (event) {
    $("#chkDefaultWebServerURL").prop('checked', false);
});

$("#chkDefaultWebServerURL").click(function (e) {

    var url = window.location.href;
    var WebUrlPath = url.split('/')[0] + "//" + url.split('/')[2] + "/" + "EMSService";
    $('#WebServerURL').val(WebUrlPath);

});


$("#grdSubFolder").kendoTooltip({
    filter: "td:nth-child(3)", //this filter selects the second column's cells
    position: "right",
    content: function (e) {
        var dataItem = $("#grdSubFolder").data("kendoGrid").dataItem(e.target.closest("tr"));
        var content = dataItem.MailFolderPath;
        return content;
    }
}).data("kendoTooltip");

