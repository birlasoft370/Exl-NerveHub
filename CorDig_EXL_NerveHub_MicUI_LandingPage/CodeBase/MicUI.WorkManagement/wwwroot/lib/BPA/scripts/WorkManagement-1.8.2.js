
/*
     * FileName: WorkManagement-1.8.2.js
     * ClassName: WorkManagement-1.8.2
     * Purpose: This file contains all the scripts for the WorkManagement including Object Formula
     * Description:  
     * Created By:Meharban, Nabin
     * Created Date: 12 December 2015
     * Modified By: 
     * Modified Date::
     * Modified Purpose:
     * Modified Description: 
 */
/*Check and add new blank row in a grid*/
var count = 0;
var CheckMailObject = ['MailReceivedDate', 'MailConfigID', 'MailFolder', 'MailFolderId', 'MoveMailFolderID', 'ReceivedMail', 'Importance', 'MailReplyType', 'MailReferenceID', 'conversationid', 'AttachmentsCount', 'Getmaildetails', 'MailFrom', 'MailTo', 'MailCC', 'MailSubject', 'MailTemplate', 'CategoriesVal']
var max = 0;
var isWinOpen = false;
var ClientLanguagebol = bClientLanguage;

function WorkDefinition_OnClickAddNewRow() {

    var errors = "";
    var gridData = $("#GridWorkObject").data("kendoGrid").dataSource.data();
    var IsGridFlag = $("input[name=IsGridConfiguration]:checked").val();
    var IsTABFlag = $("input[name=TABMapping]:checked").val();
    var GridObjectlabel = [];
    for (var i = 0; i < gridData.length; i++) {

        var GridControlType = gridData[i].selectControlType.sControlType

        if (IsTABFlag == "true") {
            var fgf = gridData[i].selectedRowTAB.sChoiceValue;
            var fgf1 = gridData[i].selectedRowTAB.sTABNameValue;
            if (gridData[i].selectedRowTAB.sChoiceValue == null || gridData[i].selectedRowTAB.sChoiceValue == "" || gridData[i].selectedRowTAB.sChoiceValue == "0") {
                errors += 'Select TAB Name ' + ", "
            }
        }
        if (GridControlType != "Grid Control") {

            if (IsGridFlag == "true") {
                var GridName = gridData[i].sGridControlID;
                if (GridName != null) {
                    errors += "Select Object Type Grid Control Type" + ", "
                }
                else {
                    if (GridObjectlabel.indexOf(gridData[i].sObjectLabel) > -1) {
                        errors = "Object label cannot be duplicate " + gridData[i].sObjectLabel;
                        break;
                    } else {
                        GridObjectlabel.push(gridData[i].sObjectLabel);
                    }

                    if (gridData[i].sObjectName == null || gridData[i].sObjectName == "") {
                        errors += ObjectName + ", "
                    }
                    if (gridData[i].iObjectType == null || gridData[i].iObjectType == "") {
                        errors += ObjectType + ", "
                    }
                    switch (gridData[i].selectControlType.sControlType) {

                        case 'DropDownList-MultiSelect':
                            // code block
                            break;
                        case 'DropDownList':
                            // code block
                            break;
                        case 'CheckBoxList':
                            // code block
                            break;
                        case 'RadioButtonList':
                            // code block
                            break;
                        default:
                            if (gridData[i].sDataType == null || gridData[i].sDataType == "") {
                                errors += 'Object ' + Datatype
                            }
                        // code block
                    }
                }
            }
            else {
                if (GridObjectlabel.indexOf(gridData[i].sObjectLabel) > -1) {
                    errors = "Object label cannot be duplicate " + gridData[i].sObjectLabel;
                    break;
                } else {
                    GridObjectlabel.push(gridData[i].sObjectLabel);
                }
                if (gridData[i].sObjectName == null || gridData[i].sObjectName == "") {
                    errors += ObjectName + ", "
                }
                if (gridData[i].iObjectType == null || gridData[i].iObjectType == "") {
                    errors += ObjectType + ", "
                }
                switch (gridData[i].selectControlType.sControlType) {

                    case 'DropDownList-MultiSelect':
                        // code block
                        break;
                    case 'DropDownList':
                        // code block
                        break;
                    case 'CheckBoxList':
                        // code block
                        break;
                    case 'RadioButtonList':
                        // code block
                        break;
                    default:
                        if (gridData[i].sDataType == null || gridData[i].sDataType == "") {
                            errors += 'Object ' + Datatype
                        }
                    // code block
                }
            }

        }
        else {
            //alert(gridData[i].sGridControlID )
            if (gridData[i].sGridControlID == null || gridData[i].sGridControlID == "") {
                errors += 'Object ' + Datatype
            }
            gridData[i].sObjectName = gridData[i].sGridControlID;
            gridData[i].sObjectDescription = gridData[i].sGridControlID;
            gridData[i].sObjectLabel = gridData[i].sGridControlID;
        }

    }
    if (errors != "") {
        errors = msgBlankRow + errors;
        jAlert(errors, display_Alert);
        return false;
    }
    else {
        //  debugger;
        var gr = $('#GridWorkObject').data('kendoGrid');
        max = $('#GridWorkObject table tbody tr:last td:first').text();
        max = parseInt(max) + 1;
        var datasource = gr.dataSource;
        var newRecord = {
            ISExistingRow: 'NO',
            iColSpan: '', bWorkID: '', sObjectName: '', sObjectDescription: '', sObjectLabel: '',
            selectControlType: { sControlType: display_Select, iControlTypeID: 0 },
            iLength: '0',
            selectedDataType: { Text: display_Select, Value: 0 },
            selectedValidation: { ValidationType: display_Select, ValidationId: 0 },
            selectedRow: { Text: display_Select, Value: 0 },
            selectedcolumn: { Text: display_Select, Value: 0 },
            selectedcolumnSpan: { Text: display_Select, Value: 0 },
            bVisible: false,
            bSearch: false,
            bEditable: false,
            bRequired: false,
            bDisabled: false,
            bUniqueID: false,
            bTransactionType: false,
            bLANID: false,
            bIsUpload: false,
            bIsReport: false,
            bCustomerIdentifier: false,
            iIsReportOrder: '0',
            iLengthReadonly: '0',
            bIsTranslate: false,
            bSearchableSearch: false,
            iReportsOrderSearch: '0',
            selectedRowTAB: { sTABNameValue: display_Select, sChoiceValue: 0 },
            selectedGridControlObj: { sGridChoiceValue: display_Select, iObjectGridChoiceID: 0 },
        };
        /*Inserting new row*/
        var idx = $('#GridWorkObject table tbody tr').length;
        datasource.insert(0, newRecord);

        EnableDisableField();
        $("#iColSpan").val(max);
    };
}

/* Add New Tab List ina Grid*/
function WorkDefinition_OnClickAddNewTAB() {
    //var errors = "";
    //if ($("#ClientName").val() == "") {
    //    errors += required_Client + '\n';
    //}
    //if ($("#ProcessName").val() == "") {
    //    errors += required_Process + '\n';
    //}
    //if ($("#CampaignName").val() == "") {
    //    errors += MsgCampaign + '\n';
    //}
    //if ($("#WorkDefinitionName").val() == "") {
    //    errors += required_WorkDefination + '\n';  
    //}
    //if ($("#NoOfColumns").val() == "") {
    //    errors += required_NoOfColumns + '\n';
    //}

    //if (errors != "") {
    //    errors = display_msg_following + '\n' + errors;
    //    jAlert(errors);
    //    return false;
    //}

    var objBEWorkObjectChoice = [];
    //debugger;
    $.ajax({
        url: urlPath_GetTABMasterData,
        type: 'POST',
        dataType: 'json',

        //data: { 'objchoiceData': objBEWorkObjectChoice },
        success: function (result) {

        },
        error: function (err) {

            alert(err)
        }
    });

    //GetUid = this.dataItem($(e.currentTarget).closest("tr")).uid;
    //var oChoice = (this.dataItem($(e.currentTarget).closest("tr")).oChoice);
    var accessWindow = $("#OpenPartialPopupTAB").kendoWindow({
        actions: ["Close"],
        draggable: true,
        height: "400px",
        modal: true,
        resizable: false,
        title: "Add Tab Details",
        width: "700px"
    }).data("kendoWindow").center().open();

    //if (oChoice == undefined) {
    //    $('#GridChoice').data('kendoGrid').dataSource.data(objBEWorkObjectChoice);
    //}
    //else {
    //    $('#GridChoice').data('kendoGrid').dataSource.data(oChoice);
    //}
    //$('#GridChoice').data('kendoGrid').refresh();
}

/*Div Slow loding*/
$(function () {

    setTimeout(function () {
        $('#GridBind').fadeIn('slow');
    }, 5);
});




// SpaniErrorDuration Dropdown set Visiblity true/false
function WorkDefinition_ShowHide(chkbx) {

    if (chkbx.checked) {
        document.getElementById("SpaniErrorDuration").style.display = "block"
        document.getElementById("SpaniErrorDurationInDay").style.display = "block"

    }
    else {
        document.getElementById("SpaniErrorDuration").style.display = "none"
        document.getElementById("SpaniErrorDurationInDay").style.display = "none"
    }
}
// TAB Button set Visiblity true/false
function WorkDefinition_TAB_ShowHide(chkbx) {

    var buttonObject = $("#btnAddNewWorkDefinitionTAB").data("kendoButton");
    var grdView = $('#GridWorkObject').data('kendoGrid');
    //By Using Columns Name.


    if (chkbx.checked) {
        buttonObject.enable(true);
        grdView.showColumn("selectedRowTAB");
        //document.getElementById("btnAddNewWorkDefinitionTAB").style.display = "block"
        //document.getElementById("btnAddNewWorkDefinitionTAB").style.display = "block"

    }
    else {
        buttonObject.enable(false);
        grdView.hideColumn("selectedRowTAB");
        //document.getElementById("btnAddNewWorkDefinitionTAB").style.display = "none"
        //document.getElementById("btnAddNewWorkDefinitionTAB").style.display = "none"
    }
}
// SpaniErrorDuration Dropdown set Visiblity true/false
$(document).ready(function () {

    if (document.getElementById("SpaniErrorDuration") != null) {
        if ($("input[name=ErrorSnapshot]:checked").val() == "true") {
            document.getElementById("SpaniErrorDuration").style.display = "block"
            document.getElementById("SpaniErrorDurationInDay").style.display = "block"
        } else {
            document.getElementById("SpaniErrorDuration").style.display = "none"
            document.getElementById("SpaniErrorDurationInDay").style.display = "none"
        }
    }
});

/* Compare Old or New Vlues in grid Lenght row*/
$(".clslength").on("keyup", function () {

    var el = $(this);
    var oldVal = el.parent().children().last().text();
    setTimeout(function () {
        var currentValue = el.find("input").val();
        if (parseInt(currentValue) < parseInt(oldVal)) {
            el.find("input").val(oldVal);
            el.parent().children().last().text(oldVal);
            jAlert(display_MsgOldAndNewValue, display_Alert);
        }
    }, 1000);
});


/*Get Number of row*/
function WorkDefinition_GetNoOfRows() {

    return { NoOfRow: $("#NoOfRows").val() }
};
/*Get Number of row*/
function WorkDefinition_GetNoOfRowsTAB() {

    return { NoOfRow: $("#NoOfRows").val() }
};

function WorkDefinition_GridParameter() {
    var iCampaignID = $("#CampaignName").val();
    var iClientID = $("#ClientName").val();
    var iProcessID = $("#ProcessName").val();
    var ccp = iCampaignID + "/" + iClientID + "/" + iProcessID;
    return { ClientProcesscampId: ccp }
};

/*get Number of coulmn*/
function WorkDefinition_GetNoOfCol() {


    if ($("#NoOfColumns").val() == "") {
        jAlert("Select No.of Columns !");
        return {
            NoOfCol: $("#NoOfColumns").val() == "" ? "0" : $("#NoOfColumns").val()
        }
    }
    else {
        return {
            NoOfCol: $("#NoOfColumns").val() == "" ? "0" : $("#NoOfColumns").val()
        }
    }
};
/*Get Number of column Span*/
function WorkDefinition_GetNoOfSapn() {

    if ($("#NoOfColumns").val() == "") {
        jAlert("Select No.of Columns !");
        return {
            NoOfSpan: $("#NoOfColumns").val() == "" ? "0" : $("#NoOfColumns").val()
        }
    }
    else {
        return {
            NoOfSpan: $("#NoOfColumns").val() == "" ? "0" : $("#NoOfColumns").val()
        }
    }

};

/*Check control validation in grid*/




/*Get Client Selected Value.*/
function WorkDefinition_filterProcess() {

    return {
        iClientID: $("#ClientName").val()

    };
};
/*Get Process Selected Value*/
function WorkDefinition_filterCampaignName() {

    return {
        iProcessID: $("#ProcessName").val()

    };
};

/*Set Dropdown Values In Grid*/
function WorkDefinition_onSelect(e) {
    e.preventDefault();
    debugger;
    var gridData = $("#GridWorkObject").data("kendoGrid").dataSource.data();
    var GridObjectlabel = [];
    for (var i = 0; i < gridData.length; i++) {
        DuplicateLabel = false;

        if (GridObjectlabel.indexOf(gridData[i].sObjectLabel.trim()) > -1) {
            var errors = "Object label cannot be duplicate " + gridData[i].sObjectLabel;
            DuplicateLabel = true;
            //  setValuesDublicate("");
            jAlert(errors, display_Alert);
            return;
        } else {
            GridObjectlabel.push(gridData[i].sObjectLabel.trim());
        }
    }
    // debugger;
    var item = $('#GridWorkObject').data().kendoGrid.dataItem($(this.element).closest('tr'));
    var controlname = $(e.sender.wrapper[0].innerHTML)[1].id;
    //e.sender.wrapper.context.id;
    var indexval = e.item.index();
    var $row = $(this.element).closest('tr');
    var col_index = $(this.element).closest('td').index();

    if (indexval > 0) {
        debugger;
        if (controlname == "selectControlType_selectControlType") {

            item.selectControlType = this.dataItem(e.item.index());
            item.iObjectType = this.dataItem(e.item.index()).iControlTypeID;
            $row.find('td:nth-child(' + (col_index + 1) + ')').text(item.selectControlType.sControlType);
            // alert(item.iObjectType);
            if (item.iObjectType == 16) {


            }
        }
        else if (controlname == "selectedRowTAB_selectedRowTAB") {

            item.selectedRowTAB = this.dataItem(e.item.index());
            item.iTAB_ID = this.dataItem(e.item.index()).sChoiceValue;


        }
        else if (controlname == "selectedGridControlObj_selectedGridControlObj") {
            //debugger;
            item.selectedGridControlObj = this.dataItem(e.item.index());
            item.sGridControlID = this.dataItem(e.item.index()).iObjectGridChoiceID;
            setValues(this.dataItem(e.item.index()).iObjectGridChoiceID);

        }
        else if (controlname == "selectedDataType_selectedDataType") {
            // debugger;
            item.selectedDataType = this.dataItem(e.item.index());
            item.sDataType = this.dataItem(e.item.index()).Value;
            $row.find('td:nth-child(' + (col_index + 1) + ')').text(item.selectedDataType.Value);
        }
        else if (controlname == "selectedValidation_selectedValidation") {
            item.selectedValidation = this.dataItem(e.item.index());
            item.iValidationID = this.dataItem(e.item.index()).ValidationId;
            $row.find('td:nth-child(' + (col_index + 1) + ')').text(item.selectedValidation.ValidationType);
        }
        else if (controlname == "selectedcolumnSpan_selectedcolumnSpan") {

            //item.selectedcolumnSpan = this.dataItem(e.item.index());
            //item.icolumn_Span = this.dataItem(e.item.index()).Value;

            if (this.dataItem(e.item.index()) == undefined) {

                item.selectedcolumnSpan = this.dataItem(0);
                item.icolumn_Span = this.dataItem(0).Value;
            }
            else {
                if (e.item == undefined) {
                    item.selectedcolumnSpan = "0";
                    item.icolumn_Span = "0";
                }
                else {
                    item.selectedcolumnSpan = this.dataItem(e.item.index());
                    item.icolumn_Span = this.dataItem(e.item.index()).Value;
                    $row.find('td:nth-child(' + (col_index + 1) + ')').text(item.selectedcolumnSpan.Value);
                }
            }

        }
        else if (controlname == "selectedRow_selectedRow") {
            if (this.dataItem(e.item.index()) == undefined) {

                item.selectedRow = this.dataItem(0);
                item.irow_No = this.dataItem(0).Value;
            }
            else {
                if (e.item == undefined) {
                    item.selectedRow = "0";
                    item.irow_No = "0";
                }
                else {
                    item.selectedRow = this.dataItem(e.item.index());
                    item.irow_No = this.dataItem(e.item.index()).Value;
                    $row.find('td:nth-child(' + (col_index + 1) + ')').text(item.selectedRow.Value);
                }
            }
        }
        else if (controlname == "selectedcolumn_selectedcolumn") {

            if (this.dataItem(e.item.index()) == undefined) {

                item.selectedcolumn = this.dataItem(0);
                item.icolumn_No = this.dataItem(0).Value;
            }
            else {
                if (e.item == undefined) {
                    item.selectedcolumn = "0";
                    item.icolumn_No = "0";
                }
                else {
                    item.selectedcolumn = this.dataItem(e.item.index());
                    item.icolumn_No = this.dataItem(e.item.index()).Value;
                    $row.find('td:nth-child(' + (col_index + 1) + ')').text(item.selectedcolumn.Value);
                }
            }
        }
        //else if (controlname == "selectedRowTAB") {

        //    setTimeout(function () {
        //    item.selectedRowTAB = this.dataItem(e.item.index());
        //        item.iTAB_ID = this.dataItem(e.item.index()).sChoiceValue;
        //    }, 100);
        //    //alert(this.dataItem(e.item.index()).sChoiceValue);
        //    //if (this.dataItem(e.item.index()) == undefined) {

        //    //    item.selectedRowTAB = this.dataItem(0);
        //    //    item.iTAB_ID = this.dataItem(0).sChoiceValue;
        //    //}
        //    //else {
        //    //    if (e.item == undefined) {
        //    //        item.selectedRowTAB = "0";
        //    //        item.iTAB_ID = "0";
        //    //    }
        //    //    else {
        //    //        item.selectedRowTAB = this.dataItem(e.item.index());
        //    //        item.iTAB_ID = this.dataItem(e.item.index()).sChoiceValue;
        //    //    }
        //    //}
        //}
        EnableDisableField();
    }
    else {

        setTimeout(function () {
            $("#selectedDataType").parent().parent().prop("disabled", "disabled");
            var data = $("#selectedDataType").data("kendoDropDownList");
            // debugger;
            if (data != undefined) {
                data.select(6);
                data.trigger("select", {
                    item: $("li.k-state-selected", $("#selectedDataType-list"))
                });
            }
        }, 100);
    }

};

//function setValuesDublicate(txtValues) {
//    $("#GridWorkObject table tbody tr td:nth-child(3)").each(function () {
//        debugger;
//        $(this).nextAll().eq(3).text(txtValues);
//        $(this).nextAll().eq(3).innerText = txtValues;

//    });
//}

function setValues(txtValues) {
    debugger;
    $("#GridWorkObject table tbody tr td:nth-child(3)").each(function () {

        // $(this).next().children(":first").removeAttr("disabled");
        $(this).nextAll().eq(1).prop("disabled", "disabled");
        $(this).nextAll().eq(1).text(txtValues);
        // $(this).nextAll().eq(1).innerText = txtValues;
        $(this).nextAll().eq(2).prop("disabled", "disabled");
        $(this).nextAll().eq(2).text(txtValues);
        $(this).nextAll().eq(3).prop("disabled", "disabled");
        $(this).nextAll().eq(3).text(txtValues);

    });
}




/*Save All Data in DataBase. Store data, grid data, business justification data, choice, letter library*/
var SaveFlag = false;
var LetterLibraryFlag = false;
function SaveWorketails() {
    //debugger;
    if (SaveFlag == false) {
        if (($("input[name=GenerateLetter]:checked").val() == "true") && (LetterLibraryFlag == false)) {
            // FillletterLibrary();
            OpenLetterLibrary()
        }
        else {
            $("#WorkDefinationKeyBenefits").css("display", "block");
            OpenBusinessJustifications();
        }
    }
    else {
        SaveWorkData();
        SaveFlag = false;
        SaveConfirmationFlag = false;

    }
};

var _oTranslateList = [];
var bIsTranslate = false;
/*Save Data in DataBase*/
function SaveWorkData() {
    //debugger;
    kendo.ui.progress($('#formWorkMaster'), true);
    var WorkDefinitionViewModel = {};
    var objBEWorkObject = [];
    _oTranslateList = [];
    var postUrl;
    var MaxPostUrl;
    //var bIsTranslate;

    var abort = false;
    var gridData = $("#GridWorkObject").data("kendoGrid").dataSource.data();
    var token = $("#formWorkMaster input").val();
    var objBEWorkObjectChoice = [];
    for (var i = 0; i < gridData.length; i++) {
        debugger;
        var bVisible = $("#GridWorkObject tbody").find("tr").eq(i).find("td").eq(12).find("input").is(":checked");
        var bSearch = $("#GridWorkObject tbody").find("tr").eq(i).find("td").eq(13).find("input").is(":checked");
        var bEditable = $("#GridWorkObject tbody").find("tr").eq(i).find("td").eq(14).find("input").is(":checked");
        var bRequired = $("#GridWorkObject tbody").find("tr").eq(i).find("td").eq(15).find("input").is(":checked");
        var bDisabled = $("#GridWorkObject tbody").find("tr").eq(i).find("td").eq(16).find("input").is(":checked");
        var bUniqueID = $("#GridWorkObject tbody").find("tr").eq(i).find("td").eq(17).find("input").is(":checked");
        var bTransactionType = $("#GridWorkObject tbody").find("tr").eq(i).find("td").eq(18).find("input").is(":checked");
        var bLANID = $("#GridWorkObject tbody").find("tr").eq(i).find("td").eq(19).find("input").is(":checked");
        var bIsUpload = $("#GridWorkObject tbody").find("tr").eq(i).find("td").eq(20).find("input").is(":checked");
        var bIsReport = $("#GridWorkObject tbody").find("tr").eq(i).find("td").eq(21).find("input").is(":checked");
        var bCustomerIdentifier = $("#GridWorkObject tbody").find("tr").eq(i).find("td").eq(22).find("input").is(":checked");
        console.warn(ClientLanguagebol)
        if (ClientLanguagebol.toUpperCase() == "TRUE") {
            bIsTranslate = $("#GridWorkObject tbody").find("tr").eq(i).find("td").eq(31).find("input").is(":checked");
            // _oTranslateList.push(ConvertTextValues(gridData[i].sObjectName));
        }
        //var bIsSearchableSearch = $("#GridWorkObject tbody").find("tr").eq(i).find("td").eq(27).find("input").is(":checked");

        try {
            if ($("#GridWorkObject tbody").find("tr").eq(i).find("td")[24].outerText == 'Select' || $("#GridWorkObject tbody").find("tr").eq(i).find("td")[25].outerText == 'Select') {
                abort = true;
                kendo.ui.progress($('#formWorkMaster'), false);
                jAlert(msg_SameRowCol, 'Alert');
                return false;
            }
        }
        catch (ex) {

        }

        if (gridData[i].sObjectName == '' || gridData[i].sGridControlID == '') {

            kendo.ui.progress($('#formWorkMaster'), false);
            jAlert('Object Name can not be null', 'Alert');
            return false;
        }
        //if (gridData[i].sObjectDescription == '' || gridData[i].sGridControlID == '') {

        //    kendo.ui.progress($('#formWorkMaster'), false);
        //    jAlert('ObjectDescription can not be null', 'Alert');
        //    return false;
        //}
        if (gridData[i].sObjectLabel == '' || gridData[i].sGridControlID == '') {
            kendo.ui.progress($('#formWorkMaster'), false);
            jAlert('ObjectLabel can not be null', 'Alert');
            return false;
        }

        var GridList = {
            iObjectID: gridData[i].iObjectID,
            iStoreID: gridData[i].iStoreID,
            sObjectName: gridData[i].sGridControlID == null ? gridData[i].sObjectName : gridData[i].sGridControlID,//   gridData[i].sObjectName,
            sObjectDescription: gridData[i].sGridControlID == null ? gridData[i].sObjectDescription : gridData[i].sGridControlID,
            sObjectLabel: gridData[i].sGridControlID == null ? gridData[i].sObjectLabel : gridData[i].sGridControlID,
            iObjectType: gridData[i].iObjectType,
            sDataType: gridData[i].iObjectType == 14 ? "Character" : (gridData[i].iObjectType == 16 ? "Character" : (gridData[i].sDataType == null ? "Integer" : gridData[i].sDataType)),
            iLength: gridData[i].iObjectType == 14 ? "300" : (gridData[i].iObjectType == 16 ? "200" : gridData[i].iLength),
            // oChoice: JSON.stringify(gridData[i].oChoice),
            iValidationID: gridData[i].iValidationID,
            oTranslateList: ConvertTextValues(gridData[i].sGridControlID == null ? gridData[i].sObjectLabel : gridData[i].sGridControlID),
            bVisible: bVisible,
            bSearch: bSearch,
            bEditable: bEditable,
            bRequired: bRequired,
            bDisabled: bDisabled,
            bUniqueID: bUniqueID,
            bTransactionType: bTransactionType,
            bLANID: bLANID,
            bIsUpload: bIsUpload,
            bIsReport: bIsReport,
            bCustomerIdentifier: bCustomerIdentifier,
            iIsReportOrder: gridData[i].iIsReportOrder,
            irow_No: gridData[i].irow_No,
            icolumn_No: gridData[i].icolumn_No,
            icolumn_Span: gridData[i].icolumn_Span,
            iTAB_ID: gridData[i].iTAB_ID,
            sGridControlID: gridData[i].sGridControlID,
            bIsTranslate: bIsTranslate,
            bSearchableSearch: gridData[i].bSearchableSearch,
            iReportsOrderSearch: gridData[i].iReportsOrderSearch,
            UID: gridData[i].uid
        }

        if (typeof gridData[i].oChoice != 'undefined') {

            for (var j = 0; j < gridData[i].oChoice.length; j++) {
                if (GetTranslate.length > 0) {
                    console.warn('GetTranslate', ClientLanguagebol)
                    if (ClientLanguagebol.toUpperCase() == "TRUE") {
                        var lstval = ConvertTextValues(gridData[i].oChoice[j].sChoiceValue);
                        for (var k = 0; k < lstval.length; k++) {

                            var ChoiceData = {
                                iChoiceLanguageID: lstval[k].iLanguageID,
                                sCultureid: lstval[k].iLanguageID,
                                sLanguage: lstval[k].sCulture,
                                iObjectChoiceID: gridData[i].oChoice[j].iObjectChoiceID,
                                sChoiceValue: lstval[k].sConvertText,
                                iGroupID: gridData[i].oChoice[j].iGroupID,
                                iOrder: gridData[i].oChoice[j].iOrder,
                                bDisabled: gridData[i].oChoice[j].bDisabled,
                                iUid: gridData[i].uid,
                                iChoiceUid: gridData[i].oChoice[j].uid
                            }
                            objBEWorkObjectChoice.push(ChoiceData);
                        }
                    }
                    else {
                        var ChoiceData = {
                            iObjectChoiceID: gridData[i].oChoice[j].iObjectChoiceID,
                            sChoiceValue: gridData[i].oChoice[j].sChoiceValue,
                            iGroupID: gridData[i].oChoice[j].iGroupID,
                            iOrder: gridData[i].oChoice[j].iOrder,
                            bDisabled: gridData[i].oChoice[j].bDisabled,
                            iUid: gridData[i].uid,
                            iChoiceUid: gridData[i].oChoice[j].uid
                        }
                        objBEWorkObjectChoice.push(ChoiceData);
                    }
                } else {
                    var ChoiceData = {
                        iObjectChoiceID: gridData[i].oChoice[j].iObjectChoiceID,
                        sChoiceValue: gridData[i].oChoice[j].sChoiceValue,
                        iGroupID: gridData[i].oChoice[j].iGroupID,
                        iOrder: gridData[i].oChoice[j].iOrder,
                        bDisabled: gridData[i].oChoice[j].bDisabled,
                        iUid: gridData[i].uid,
                        iChoiceUid: gridData[i].oChoice[j].uid
                    }
                    objBEWorkObjectChoice.push(ChoiceData);
                }
                //  objBEWorkObjectChoice.push(ChoiceData);
            }
        }

        objBEWorkObject.push(GridList);
    }

    var token = $('input[name=__RequestVerificationToken]').val();//added by indresh 22-08-2017

    WorkDefinitionViewModel.ClientName = $("#ClientName").val();
    WorkDefinitionViewModel.ProcessName = $("#ProcessName").val();
    WorkDefinitionViewModel.CampaignName = $("#CampaignName").val();
    WorkDefinitionViewModel.WorkDefinitionName = $("#WorkDefinitionName").val();
    WorkDefinitionViewModel.Description = $("#Description").val();
    WorkDefinitionViewModel.NoOfRows = $("#NoOfRows").val();
    WorkDefinitionViewModel.NoOfColumns = $("#NoOfColumns").val();
    WorkDefinitionViewModel.DisableWork = $("#DisableWork").is(":checked");
    WorkDefinitionViewModel.GenerateLetter = $("input[name=GenerateLetter]:checked").val();
    WorkDefinitionViewModel.ErrorSnapshot = $("input[name=ErrorSnapshot]:checked").val();
    WorkDefinitionViewModel.TABMapping = $("input[name=TABMapping]:checked").val();
    WorkDefinitionViewModel.bIsRunTimeUploadRequired = $("input[name=bIsRunTimeUploadRequired]:checked").val();
    WorkDefinitionViewModel.IsDistributionBot = $("input[name=IsDistributionBot]:checked").val();
    WorkDefinitionViewModel.ErrorDuration = $("#ErrorDuration").val();
    WorkDefinitionViewModel.IncreaseSearch = $("#IncreaseSearch").val();
    WorkDefinitionViewModel.dModifyDate = "";
    WorkDefinitionViewModel.dCreateDate = "";
    WorkDefinitionViewModel.IsGridConfiguration = $("input[name=IsGridConfiguration]:checked").val();
    WorkDefinitionViewModel.IsForStaging = $("#IsForStaging").is(":checked");
    //var size = 20; var arrayOfArrays = [];
    //for (var i = 0; i < objBEWorkObject.length; i += size) {
    //    arrayOfArrays.push(objBEWorkObject.slice(i, i + size));
    //    WorkDefinitionViewModel.WorkDefinition = objBEWorkObject.slice(i, i + size);
    //    MaxPostUrl = urlMaxdatasave;
    //    $.ajax({
    //        url: MaxPostUrl,
    //        type: 'POST',
    //        async: false,
    //        dataType: 'json',
    //        data: JSON.stringify({ 'objMaxWorkDefinitionViewModel': WorkDefinitionViewModel }),
    //        contentType: 'application/json; charset=utf-8',
    //        success: function (result) {

    //            if (result == 'OK') {                    
    //                WorkDefinitionViewModel.WorkDefinition = [];                   
    //            }
    //        },
    //        error: function (err) {
    //            // kendo.ui.progress($('#formWorkMaster'), false);                 
    //        }
    //    });
    //}

    WorkDefinitionViewModel.WorkDefinition = objBEWorkObject;
    // WorkDefinitionViewModel.WorkDefinition.oChoice = objBEWorkObjectChoice;


    var postUrl = urlPathSaveWorkData;
    postChoiceUrl = urlChoicedatasave;
    if (!abort) {
        //async: false,
        $.ajax({
            url: postChoiceUrl,
            type: 'POST',
            dataType: 'json',
            data: { 'objchoiceData': objBEWorkObjectChoice },
            success: function (result) {
                $.ajax({
                    url: postUrl,
                    type: 'POST',
                    dataType: 'json',
                    data: { 'objWorkDefinitionViewModel': WorkDefinitionViewModel },
                    success: function (result) {
                        if (result.Successed == false) {
                            if (result.strMessage == 'Update') {
                                jAlert(display_msgupdatework, 'Alert', function () {
                                    window.location.href = urlPathIndex;
                                    kendo.ui.progress($('#formWorkMaster'), false);
                                });
                            }
                            else if (result.strMessage == 'insertObject') {
                                kendo.ui.progress($('#formWorkMaster'), false);
                                jAlert(display_MsgWork, 'Alert', function () {
                                    window.location.href = urlPathIndex + '?key=s';

                                });
                            }
                            else {
                                kendo.ui.progress($('#formWorkMaster'), false);
                                jAlert(result.strMessage, 'Alert');
                            }
                        }
                        else {
                            kendo.ui.progress($('#formWorkMaster'), false);
                            jAlert(result.msg, 'Alert');
                        }
                    },
                    error: function (err) {
                        kendo.ui.progress($('#formWorkMaster'), false);
                        //
                    }
                });
            },
            error: function (err) {
                kendo.ui.progress($('#formWorkMaster'), false);
                //
            }
        });
    }
    else {

        kendo.ui.progress($('#formWorkMaster'), false);
        jAlert("Select valid value for row no or column no!", 'Alert');
    }

}

/*Enable Disable Controle in grid EDIT*/
function EnableDisableField_EDIT() {
    // debugger;
    $("#GridWorkObject table tbody tr td:nth-child(8)").each(function () {
        //  debugger;
        var dataType = $(this)[0].innerText;
        // if (dataType == 'DropDownList' || dataType == 'CheckBoxList' || dataType == 'RadioButtonList' || dataType == 'ListBox' || dataType == 'DropDownList-MultiSelect') {
        if (dataType == 'DropDownList' || dataType == 'CheckBoxList' || dataType == 'RadioButtonList' || dataType == 'ListBox') {

            $(this).next().children(":first").removeAttr("disabled");
            $(this).nextAll().eq(1).prop("disabled", "disabled");
            $(this).nextAll().eq(1).text("Integer");
            $(this).nextAll().eq(2).prop("disabled", "disabled");
            $(this).nextAll().eq(2).text("0");


        }
        else if (dataType == 'DropDownList-MultiSelect') {


            //setTimeout(function () {
            $(this).next().children(":first").removeAttr("disabled");
            $(this).nextAll().eq(1).prop("disabled", "disabled");
            $(this).nextAll().eq(1).text("Character");
            //  $(this).nextAll().eq(2).prop("disabled", "disabled");
            $(this).nextAll().eq(2).text("300");
            //  }, 1000); 
        }
        else if (dataType == 'Grid Control') {
            $(this).next().children(":first").removeAttr("disabled");
            $(this).nextAll().eq(1).prop("disabled", "disabled");
            $(this).nextAll().eq(1).text("Character");
            $(this).nextAll().eq(2).prop("disabled", "disabled");
            $(this).nextAll().eq(2).text("300");

            $(this).prevAll().eq(2).prop("disabled", "disabled");
            $(this).prevAll().eq(1).prop("disabled", "disabled");
            $(this).prevAll().eq(0).prop("disabled", "disabled");
        }

        else {
            //  $(this).next().children(":first").prop("disabled", "disabled");
            $(this).nextAll().eq(1).removeAttr("disabled");
            $(this).nextAll().eq(2).removeAttr("disabled");
            $(this).prevAll().eq(2).removeAttr("disabled");
            $(this).prevAll().eq(1).removeAttr("disabled");
            $(this).prevAll().eq(0).removeAttr("disabled");
        }
    });


    /*Enable Disable Controle in grid*/
    //$(".ObjectType, .ddlDataType, .DataType").each(function () {

    //    if (($(this).text() != "display_Select" || "") && isEnable == "False") {
    //        $(this).prop("disabled", "disabled");
    //    }
    //});


    /*Enable Disable Controle in grid*/
    //$(".ObjectName").each(function () {
    //    debugger
    //    if (($(this).text() != "") && isEnable == "False") {
    //        $(this).prop("disabled", true);
    //    }
    //});

    /*Enable Disable Controle in grid*/
    var rows = $('#GridWorkObject table tbody tr').length;
    var data = $("#GridWorkObject").data("kendoGrid").dataSource.view();

    $(".BtnOpenChoice").find("a").each(function (index) {
        //debugger;
        var index = 0;
        //count = 0;
        if (isEnable == "False" && index < (rows - 1)) {
            if (!($(this).parent().prev().text() == "DropDownList-MultiSelect" || $(this).parent().prev().text() == "DropDownList" || $(this).parent().prev().text() == "CheckBoxList" || $(this).parent().prev().text() == "RadioButtonList" || $(this).parent().prev().text() == "<--Select-->")) {
                $(this).prop("disabled", "disabled");
            }
        }
        // Code to Enable Non Email Rows
        if ($("#IsEmail").is(':checked')) {

            var Total = searchStringInArray($(".ObjectName")[count].innerText, CheckMailObject)
            if (Total == -1 && ($(this).parent().prev().text() == "DropDownList-MultiSelect" || $(this).parent().prev().text() == "DropDownList" || $(this).parent().prev().text() == "CheckBoxList" || $(this).parent().prev().text() == "RadioButtonList" || $(this).parent().prev().text() == "<--Select-->")) {
                $(this).prop("disabled", false);
            }
            else {
                $(this).prop("disabled", "disabled");
            }
            count = count + 1;
        }
        {
            if (!($(this).parent().prev().text() == "DropDownList-MultiSelect" || $(this).parent().prev().text() == "DropDownList" || $(this).parent().prev().text() == "CheckBoxList" || $(this).parent().prev().text() == "RadioButtonList" || $(this).parent().prev().text() == "<--Select-->")) {
                $(this).prop("disabled", "disabled");
            }
        }
    });
    count = 0;
};

function DisableEnableFields() {
    //debugger;
    $("#GridWorkObject table tbody tr td:nth-child(8)").each(function () {
        var dataType = $(this)[0].innerText;
        // debugger
        if (dataType == 'DropDownList' || dataType == 'CheckBoxList' || dataType == 'RadioButtonList' || dataType == 'ListBox') {


            $(this).next().children(":first").removeAttr("disabled");
            $(this).nextAll().eq(1).attr("disabled", "disabled");
            $(this).nextAll().eq(1).text("Integer");
            $(this).nextAll().eq(2).attr("disabled", "disabled");
            $(this).nextAll().eq(2).text("0");
        }
        else if (dataType == 'DropDownList-MultiSelect') {
            $(this).next().children(":first").removeAttr("disabled");
            $(this).nextAll().eq(1).attr("disabled", "disabled");
            $(this).nextAll().eq(1).text("Character");
            $(this).nextAll().eq(2).attr("disabled", "disabled");
            $(this).nextAll().eq(2).text("300");
        }
        else {
            $(this).next().children(":first").attr("disabled", "disabled");
            $(this).nextAll().eq(1).removeAttr("disabled");
            $(this).nextAll().eq(2).removeAttr("disabled");
        }
    });
}


/*Enable Disable Controle in grid*/

/*Enable Disable Controle in grid*/
function EnableDisableField() {
    debugger;
    $("#GridWorkObject table tbody tr td:nth-child(8)").each(function () {
        // debugger;
        var dataType = $(this)[0].innerText;
        //// if (dataType == 'DropDownList' || dataType == 'CheckBoxList' || dataType == 'RadioButtonList' || dataType == 'ListBox' || dataType == 'DropDownList-MultiSelect') {
        // if (dataType == 'DropDownList' || dataType == 'CheckBoxList' || dataType == 'RadioButtonList' || dataType == 'ListBox'  ) {
        if (dataType == 'DropDownList' || dataType == 'RadioButtonList' || dataType == 'ListBox') {

            $(this).next().children(":first").removeAttr("disabled");
            $(this).nextAll().eq(1).attr("disabled", "disabled");
            $(this).nextAll().eq(1).text("Integer");
            $(this).nextAll().eq(2).attr("disabled", "disabled");
            $(this).nextAll().eq(2).text("0");


        }
        else if (dataType == 'DropDownList-MultiSelect' || dataType == 'CheckBoxList') {
            //setTimeout(function () {
            $(this).next().children(":first").removeAttr("disabled");
            $(this).nextAll().eq(1).attr("disabled", false);
            $(this).nextAll().eq(1).text("Character");
            //  $(this).nextAll().eq(2).prop("disabled", "disabled");
            $(this).nextAll().eq(2).text("300");
            //  }, 1000); 
        }
        else if (dataType == 'Grid Control') {
            $(this).next().children(":first").removeAttr("disabled");
            $(this).nextAll().eq(1).attr("disabled", "disabled");
            $(this).nextAll().eq(1).text("Character");
            $(this).nextAll().eq(2).attr("disabled", "disabled");
            $(this).nextAll().eq(2).text("300");

            $(this).prevAll().eq(2).attr("disabled", "disabled");
            $(this).prevAll().eq(1).attr("disabled", "disabled");
            $(this).prevAll().eq(0).attr("disabled", "disabled");
        }

        else {
            $(this).next().children(":first").attr("disabled", "disabled");
            $(this).nextAll().eq(1).removeAttr("disabled");
            $(this).nextAll().eq(2).removeAttr("disabled");
            $(this).prevAll().eq(2).removeAttr("disabled");
            $(this).prevAll().eq(1).removeAttr("disabled");
            $(this).prevAll().eq(0).removeAttr("disabled");
        }
    });


    /*Enable Disable Controle in grid*/
    //$(".ObjectType, .ddlDataType, .DataType").each(function () {

    //    //if (($(this).text() != "display_Select" || "") && isEnable == "False") {
    //    //    $(this).prop("disabled", "disabled");
    //    //}
    //});


    ///*Enable Disable Controle in grid*/
    $(".ObjectName").each(function () {
        debugger;
        if (($(this).text() != "") && isEnable == "False") {
            $(this).prop("disabled", true);
        }
    });

    /*Enable Disable Controle in grid*/
    var rows = $('#GridWorkObject table tbody tr').length;
    var data = $("#GridWorkObject").data("kendoGrid").dataSource.view();

    $(".BtnOpenChoice").find("a").each(function (index) {
        debugger;
        var index = 0;
        if (isEnable == "False" && index < (rows - 1)) {
            if (!($(this).parent().prev().text() == "DropDownList-MultiSelect" || $(this).parent().prev().text() == "DropDownList" || $(this).parent().prev().text() == "CheckBoxList" || $(this).parent().prev().text() == "RadioButtonList" || $(this).parent().prev().text() == "<--Select-->")) {
                $(this).prop("disabled", "disabled");
            }
        }
        // Code to Enable Non Email Rows
        if ($("#IsEmail").is(':checked')) {

            var Total = searchStringInArray($(".ObjectName")[count].innerText, CheckMailObject)
            if (Total == -1 && ($(this).parent().prev().text() == "DropDownList-MultiSelect" || $(this).parent().prev().text() == "DropDownList" || $(this).parent().prev().text() == "CheckBoxList" || $(this).parent().prev().text() == "RadioButtonList" || $(this).parent().prev().text() == "<--Select-->")) {
                $(this).prop("disabled", false);
            }
            else {
                $(this).prop("disabled", "disabled");
            }
            count = count + 1;
        }
        {
            if (!($(this).parent().prev().text() == "DropDownList-MultiSelect" || $(this).parent().prev().text() == "DropDownList" || $(this).parent().prev().text() == "CheckBoxList" || $(this).parent().prev().text() == "RadioButtonList" || $(this).parent().prev().text() == "<--Select-->")) {
                $(this).prop("disabled", "disabled");
            }
        }
    });
    count = 0;
};


$(document).on("change", ".ObjectType", function () {

    try {
        $("#GridWorkObject table tbody tr td:nth-child(8)").each(function () {
            //debugger;
            var dataType = "";
            if ($(this)[0].children.length > 0) {
                dataType = $(this)[0].children['0'].childNodes['0'].innerText;
            }
            else {
                dataType = null;

            }
            //// if (dataType == 'DropDownList' || dataType == 'CheckBoxList' || dataType == 'RadioButtonList' || dataType == 'ListBox' || dataType == 'ListBox - MultiSelect' || dataType == 'DropDownList-MultiSelect') {
            if (dataType == 'DropDownList' || dataType == 'CheckBoxList' || dataType == 'RadioButtonList' || dataType == 'ListBox' || dataType == 'ListBox - MultiSelect') {
                $(this).next().children(":first").removeAttr("disabled");
            }
            else {
                $(this).next().children(":first").prop("disabled", "disabled");
            }
        });
    }
    catch (err) {
        jAlert(err.message);
    }

});



$(document).on("keypress paste", "#WorkDefinitionName", function () {
    var regex = new RegExp("^[a-zA-Z_]+$");
    var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
    if (!regex.test(key)) {
        event.preventDefault();
        return false;
    }
});

//$(".ObjectName ").on("keypress", function (event) {
$(document).on("keypress", ".ObjectName", function () {
    //debugger;
    //var regex = new RegExp("^[a-zA-Z0-9]+$");
    var regex = new RegExp("^[a-zA-Z]+$");
    var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
    if (!regex.test(key)) {
        event.preventDefault();
        return false;
    }
});
//$(document).on('paste', ':text', function (evt) {
//   // evt.preventDefault();
//    debugger;
//    evt = evt || window.event;
//    var charCode = evt.which || evt.keyCode || evt.charCode;

//    if (charCode === 1) {
//        var charStr = clipboardData.getData('Text');//String.fromCharCode(charCode);
//       // clipboardData.clearData("Text");
//        charStr = charStr.replace(/[^A-Za-z0-9 '@@'#$%^*()_+=?/"!&,.:;~'\\-]/g, '');
//        if (charStr.match(/[A-Za-z0-9 '@@'#$%^*()_+=?/"!&,.:;~'\\-]/)) {
//           // this.value = this.value + charStr;
//        }
//    }
//});
/* Edit Row in Grid*/
function onEdit(e) {

    var index = 0;
    if (e.model.isNew()) {
        //e.sender.editable.element[0].find("input").val(oldVal);
        max = max + 1
        e.model.set("iColSpan", max);
    }
    if (e.model.ISExistingRow == "YES") {
        if ($('#IsEmail').is(':checked')) {
            index = searchStringInArray(e.model.sObjectName, CheckMailObject);
        }
        if (index > 0) {
            EnableDisableField_EDIT();
            // $('#GridWorkObject').data("kendoGrid").closeCell();
        }
    }
};
function onEditGrd(e) {

    //var index = 0;
    //if (e.model.isNew()) {
    //    max = max + 1
    //    e.model.set("iColSpan", max);
    //}
    //if (e.model.ISExistingRow == "YES") {
    //    //if ($('#IsEmail').is(':checked')) {
    //    //    index = searchStringInArray(e.model.sObjectName, CheckMailObject);
    //    //}
    //    if (index > 0) {
    //        $('#GridWorkObject').data("kendoGrid").closeCell();
    //    }
    //}
};

function searchStringInArray(str, strArray) {
    if (str != null) {
        for (var j = 0; j < strArray.length; j++) {
            if (strArray[j].toLowerCase() == str.toLowerCase()) return ++j;
        }
    }
    return -1;
}
/**/
function onGridDataBound(e) {
    //debugger;
    try {
        isWinOpen = $("#OpenPartialPopupChoice").is(":visible");
    }
    catch (err) {
        isWinOpen = false;
    }

    if (isWinOpen) {
        var grid = this;
        $(".templateCell").each(function () {
            eval($(this).children("script").last().html());
            var tr = $(this).closest('tr');
            var item = grid.dataItem(tr);
            kendo.bind($(this), item);
        });
    }
    //debugger;
    EnableDisableField();
};

/*Row Delete button in grid .............*/



function deleteObjectCode(e) {

    var dataItem = this.dataItem($(e.target).closest("tr"));
    jConfirm(display_MsgConfirm, display_Confirmation, function (r) {
        if (r) {


            var dataSource = $("#GridWorkObject").data("kendoGrid").dataSource;
            dataSource.remove(dataItem);
            var totalcount = dataSource.total();
            $("#spnTotalRecords").text(totalcount);
            //jAlert("total" + totalcount);
            // dataSource.sync();
            DisableEnableFields();



        }
        else {
            return false;
        }
    });
};
/*Open Choice ...............*/
var GetUid = '';
var preChoiceList = null;

function OpenChoice(e) {
    // debugger;
    preChoiceList = [];
    var objBEWorkObjectChoice = [];
    GetUid = this.dataItem($(e.currentTarget).closest("tr")).uid;
    var oChoice = (this.dataItem($(e.currentTarget).closest("tr")).oChoice);

    var accessWindow = $("#OpenPartialPopupChoice").kendoWindow({
        draggable: true,
        height: "400px",
        modal: true,
        resizable: false,
        title: "Fill Choice",
        width: "700px"
    }).data("kendoWindow").center().open();
    $("#OpenPartialPopupChoice").parent().find(".k-window-action").css("visibility", "hidden");
    if (oChoice == undefined) {

        $('#GridChoice').data('kendoGrid').dataSource.data(objBEWorkObjectChoice);
    }
    else {
        for (var i = 0; i < oChoice.length; i++) {
            var ChoiceData = {
                iObjectChoiceID: oChoice[i].iObjectChoiceID,
                sChoiceValue: oChoice[i].sChoiceValue,
                iGroupID: oChoice[i].iGroupID,
                iOrder: oChoice[i].iOrder,
                bDisabled: oChoice[i].bDisabled
            }
            preChoiceList.push(ChoiceData);
        }
        $('#GridChoice').data('kendoGrid').dataSource.data(preChoiceList);
    }
    $('#GridChoice').data('kendoGrid').refresh();
};

function find_duplicate_in_array(arra1) {
    var object = {};
    var result = [];

    arra1.forEach(function (item) {
        //debugger;
        if (!object[item.sChoiceValue.toUpperCase().trim()])
            object[item.sChoiceValue.toUpperCase().trim()] = 0;
        object[item.sChoiceValue.toUpperCase().trim()] += 1;
    })
    //debugger;
    for (var prop in object) {
        if (object[prop] >= 2) {
            result.push(prop);
        }
    }

    return result;

}

/*Save Choice value*/
function SaveChoiceClick() {
    debugger;
    var objBEWorkObjectChoice = [];
    var postUrl;
    var gridData = $("#GridChoice").data("kendoGrid").dataSource.data();

    if (gridData.length > 0) {
        for (var i = 0; i < gridData.length; i++) {

            if (gridData[i].sChoiceValue == "" || gridData[i].sChoiceValue == undefined) {
                jAlert("Please fill the choice value", display_Alert);
                return;
            }
            else {
                var bDisabled = $("#GridChoice tbody").find("tr").eq(i).find("td").eq(4).find("input").is(":checked");
                var ChoiceData = {
                    iObjectChoiceID: gridData[i].iObjectChoiceID,
                    sChoiceValue: gridData[i].sChoiceValue,
                    iGroupID: gridData[i].iGroupID,
                    iOrder: gridData[i].iOrder,
                    bDisabled: bDisabled
                }

                objBEWorkObjectChoice.push(ChoiceData);
            }
        }
        var results = find_duplicate_in_array(objBEWorkObjectChoice);
        if (results.length > 0) {

            jAlert("Object Name " + results + " Duplicate Value(One or more time) !", display_Alert);
            return;
        }
        var grid = $("#GridWorkObject").data("kendoGrid");
        var DataSource = grid.dataSource;
        for (var i = 0; i < DataSource.total(); i++) {
            if (DataSource.data()[i].uid == GetUid) {
                DataSource.data()[i].set("oChoice", objBEWorkObjectChoice);
            }
        }
        $("#OpenPartialPopupChoice").data("kendoWindow").close();
        $("#OpenPartialPopupChoice").parent().find(".k-window-action").css("visibility", "");
    }
    else {
        jAlert("Please add at least one choice", display_Alert);
        return;
    }

    DisableEnableFields();

};

function closeChoise() {
    //debugger;
    var grid = $("#GridWorkObject").data("kendoGrid");
    var DataSource = grid.dataSource;
    for (var i = 0; i < DataSource.total(); i++) {
        if (DataSource.data()[i].uid == GetUid) {
            DataSource.data()[i].set("oChoice", preChoiceList);
        }
    }
    $("#OpenPartialPopupChoice").data("kendoWindow").close();
    $("#OpenPartialPopupChoice").parent().find(".k-window-action").css("visibility", "");
    DisableEnableFields();
}

/*Save Tab Name value*/
function SaveTABNameClick() {

    var objBEWorkObjectChoice = [];
    var postUrl;
    var objTabNameWDGrdObject = [];
    var objTabChoiceObject = [];
    var gridData = $("#GridChoiceTAB").data("kendoGrid").dataSource.data();
    var MgridData = $("#GridWorkObject").data("kendoGrid").dataSource.data();
    for (var i = 0; i < MgridData.length; i++) {

        objTabNameWDGrdObject.push({ 'Id': MgridData[i].iTAB_ID });
    }

    if (gridData.length > 0) {
        for (var i = 0; i < gridData.length; i++) {

            if (gridData[i].sChoiceValue == "" || gridData[i].sChoiceValue == undefined) {
                jAlert("Please fill the choice value", display_Alert);
                return;
            }
            else {

                var ChoiceData = {
                    iObjectChoiceID: gridData[i].iObjectChoiceID,
                    sChoiceValue: gridData[i].sChoiceValue,
                    sTABNameValue: gridData[i].sTABNameValue,
                    iGroupID: 0,
                    iOrder: gridData[i].iOrder,
                    bDisabled: gridData[i].bDisabled
                }
                if (gridData[i].bDisabled == true) { objTabChoiceObject.push({ 'Id': gridData[i].sChoiceValue }); }

                objBEWorkObjectChoice.push(ChoiceData);
            }
        }
        debugger;
        var MatchTab = "";
        for (var j = 0; j < objTabChoiceObject.length; j++) {
            for (var k = 0; k < objTabNameWDGrdObject.length; k++) {
                if (objTabChoiceObject[j].Id === objTabNameWDGrdObject[k].Id) {
                    if (MatchTab == "") { MatchTab = objTabChoiceObject[j].Id } else { MatchTab = MatchTab != objTabChoiceObject[j].Id ? MatchTab + ', ' + objTabChoiceObject[j].Id : MatchTab }

                }
            }
        }
        if (MatchTab == "") {
            $.ajax({
                url: urlPathSaveTabName_Temp,
                type: 'POST',
                dataType: 'json',
                data: { 'objchoiceData': objBEWorkObjectChoice },
                success: function (result) {

                },
                error: function (err) {

                    alert(err)
                }
            });
        }
        else {
            var msg = 'You cannot disabled this TAB  ' + MatchTab + ', because Tab Name using in Work Master Grid!';
            jAlert(msg, display_Alert);
        }

        //var grid = $("#GridWorkObject").data("kendoGrid");
        //var DataSource = grid.dataSource;
        //for (var i = 0; i < DataSource.total(); i++) {
        //    if (DataSource.data()[i].uid == GetUid) {
        //        DataSource.data()[i].set("oChoice", objBEWorkObjectChoice);
        //    }
        //}
        $("#OpenPartialPopupTAB").data("kendoWindow").close();
    }
    else {
        jAlert("Please add at least one choice", display_Alert);
        return;
    }

    DisableEnableFields();

};

/* Convert Different Language */
//function SaveTranslateClick() {

//    var objBEWorkObjectChoice = [];
//    var postUrl;
//    var gridData = $("#GridTranslate").data("kendoGrid").dataSource.data();
//    if (gridData.length > 0) {
//        for (var i = 0; i < gridData.length; i++) {
//            var ChoiceData = {
//                iSDOBJLanID: gridData[i].iSDOBJLanID,
//                iObjID: gridData[i].iObjID,
//                iLanguageID: gridData[i].iLanguageID,
//                sCulture: gridData[i].sCulture,
//                sLanguage: gridData[i].sLanguage,
//                sConvertText: gridData[i].sConvertText

//            }
//            objBEWorkObjectChoice.push(ChoiceData);
//        }
//        var grid = $("#GridWorkObject").data("kendoGrid");
//        var DataSource = grid.dataSource;
//        for (var i = 0; i < DataSource.total(); i++) {
//            if (DataSource.data()[i].uid == GetUid) {
//                DataSource.data()[i].set("oTranslateList", objBEWorkObjectChoice);
//            }
//        }
//        $("#OpenPartialPopupTranslate").data("kendoWindow").close();
//    }
//    else {
//        //  jAlert("Please add at least one choice", display_Alert);
//        //   return;
//    }

//}




/*Business Justifications ..............*/
function OpenBusinessJustifications() {
    debugger;
    var ddl = $("#BusinessApprover").data("kendoDropDownList");
    if (ddl != undefined) { ddl.dataSource.read(); }

    var accessWindow = $("#OpenPartialWorkDefinationKeyBenefits").kendoWindow({
        actions: ["Close"],
        draggable: true,
        height: "450px", // change by manishdwivedi
        modal: true,
        resizable: false,
        title: "Business Justifications",
        width: "900px",
        visible: false
    }).data("kendoWindow").center().open();
};


/*Letter Library ..............*/
/* Letter Library  Poup Open*/
function OpenLetterLibrary() {
    var accessWindow = $("#OpenPopupLetterLibrary").kendoWindow({
        actions: ["Close"],
        draggable: true,
        height: "400px",
        modal: true,
        resizable: false,
        title: "Letter Library",
        width: "800px",
        visible: false
    }).data("kendoWindow").center().open();


    var dataSourceObject = [];
    $("#GridWorkObject table tbody tr").each(function () {
        var dataValue = $(this).attr("data-uid");

        if ($(this).find("td").eq(7).text() == "DropDownList" || $(this).find("td").eq(7).text() == "DropDownList-MultiSelect") {
            dataSourceObject.push({
                strObjectName: $(this).find("td").eq(4).text(),
                dataValue: dataValue
            });
        }
    });

    $("#ddlLetterMaping").kendoDropDownList({
        dataSource: dataSourceObject,
        dataTextField: "strObjectName",
        dataValueField: "dataValue",
        optionLabel: "--Select--"
    });
};

/*To navigate on Search View page*/
function OnClickView() {

    window.location.href = urlPathSearchView
};

/*To navigate on Work Object Apporvals page*/
function OnClickWorkApporval() {
    window.location.href = urlPathApprovalView
};

/*To navigate on PreView page*/
function OnClickPreView() {

    var errors = "";
    if ($("#ClientName").val() == "") {
        errors += required_Client + '\n';
    }
    if ($("#ProcessName").val() == "") {
        errors += required_Process + '\n';
    }
    if ($("#CampaignName").val() == "") {
        errors += MsgCampaign + '\n';
    }

    if (errors != "") {
        errors = display_msg_following + '\n' + errors;
        jAlert(errors);
        return false;
    }

    var iCampaignID = $("#CampaignName").val();
    var iClientID = $("#ClientName").val();
    var iProcessID = $("#ProcessName").val();
    var ccp = iCampaignID + "/" + iClientID + "/" + iProcessID;

    var token = $("#formWorkMaster input[name=__RequestVerificationToken]").val();
    $.ajax({
        type: "POST"
        , url: urlPathGetProcessID
        , dataType: 'json',
        data: { __RequestVerificationToken: token, ClientProcesscampId: ccp },
        //, data: JSON.stringify({ ClientProcesscampId: ccp })
        //, contentType: "application/json"
        success: function (result) {

            if (result == "1") {
                window.location.href = urlPath_WorkPreview
            }
        }
        , error: function (result) {
        }
    });
    window.location.href = urlPath_WorkPreview
};
function OnClickGridConfig() {
    //debugger;
    //var errors = "";
    //if ($("#ClientName").val() == "") {
    //    errors += required_Client + '\n';
    //}
    //if ($("#ProcessName").val() == "") {
    //    errors += required_Process + '\n';
    //}
    //if ($("#CampaignName").val() == "") {
    //    errors += MsgCampaign + '\n';
    //}

    //if (errors != "") {
    //    errors = display_msg_following + '\n' + errors;
    //    jAlert(errors);
    //    return false;
    //}

    //var iCampaignID = $("#CampaignName").val();
    //var iClientID = $("#ClientName").val();
    //var iProcessID = $("#ProcessName").val();
    //var ccp = iCampaignID + "/" + iClientID + "/" + iProcessID;

    //var token = $("#formWorkMaster input").val();
    //$.ajax({
    //    type: "POST"
    //    , url: urlPathGetGridProcessID
    //    , dataType: 'json',
    //    data: { __RequestVerificationToken: token, ClientProcesscampId: ccp },

    //    success: function (result) {
    //        //debugger;
    //        if (result == "1") {
    //            window.location.href = urlPath_GridConfigurations
    //        }
    //    }
    //    , error: function (result) {
    //    }
    //});
    window.location.href = urlPath_GridConfigurations
};
/* Object Type Chnage set Control*/
//$(".ddlObjectType").on("change", function (e) {
$(document).on("change", ".ddlObjectType", function () {
    //debugger;
    var curr = $(this).parents().eq(1);
    var vdr = $(this).val();

    if ($(this).val() == 6 || $(this).val() == 4 || $(this).val() == 8 || $(this).val() == 5 || $(this).val() == 14) {
        $(this).parents().eq(1).next().children(":first").removeAttr("disabled");
        $(this).parents().eq(1).nextAll().eq(2)[0].innerText = "0";
        $(this).parents().eq(1).nextAll().eq(2).prop("disabled", "disabled");
        $(this).parents().eq(1).nextAll().eq(1).prop("disabled", false);

        setTimeout(function () {
            curr.nextAll().eq(1).trigger("click");
            setTimeout(function () {
                $("#selectedDataType").parent().parent().prop("disabled", "disabled");
                var data = $("#selectedDataType").data("kendoDropDownList");
                // debugger;
                if (data != undefined) {
                    // data.select(6);
                    data.trigger("select", {
                        item: $("li.k-state-selected", $("#selectedDataType-list"))
                    });
                }
            }, 1000);
        }, 1000);
    }
});


/*Clear Control*/
function btnNewClick() {
    window.location.href = urlPathIndex;
};


/* if Character select  set Lenght*/
$(".ddlDataType ").on("change", function () {

    if ($(this).val() == "Character") {

        $(this).parent().parent().next().find("input").val("0");
        $(this).parent().parent().next().text("0");
    }
});

/*Check Row and column values are not in same position */
//$(".clsRow").on("change", function () {
$(document).on("change", ".clsRow", function () {
    debugger;
    var el = this;
    var currRowVal = $(this).find("[id=selectedRow]").val();
    var currColVal = $(this).next().text();
    var tabname = $(this).find("[id=selectedRowTAB]").val();
    var rowFlg = false;
    var colFlg = false;
    var IsTABFlag_ = $("input[name=TABMapping]:checked").val();
    var el = "";
    var vErr = 0;
    var Grid_List = [];
    var CurrTabName = "";
    if (IsTABFlag_ == "true") {
        //  if (vErr == 0) {
        // debugger;
        var RowCount = 0;
        if (bClientLanguage == 'True' || bClientLanguage == 'true') {
            $("#GridWorkObject table tbody tr td:nth-child(35)").each(function () {
                if ($(this).text() == "" || $(this).text() == "<--Select-->" || $(this).text() == "--Select--" || $(this).text() == "Select") {
                    el = this;
                    vErr = 4;
                }
                else {
                    if (RowCount == 0) {
                        //  alert($(this).text());
                        CurrTabName = $(this).text();
                    }

                }
                RowCount = RowCount + 1;
            });
            CurrTabName = $(this).next().next().next().next().next().next().next().next().next().next().text();// $(this).text();
        }
        else {
            $("#GridWorkObject table tbody tr td:nth-child(33)").each(function () {

                if ($(this).text() == "" || $(this).text() == "<--Select-->" || $(this).text() == "--Select--" || $(this).text() == "Select") {
                    el = this;
                    vErr = 4;
                }
                else {
                    if (RowCount == 0) {
                        //  alert($(this).text());
                        CurrTabName = $(this).text();
                    }


                }
                RowCount = RowCount + 1;
            });
            CurrTabName = $(this).next().next().next().next().next().next().next().next().text();// $(this).text();
        }

        if (vErr == 0) {
            var gridData = $("#GridWorkObject").data("kendoGrid").dataSource.data();


            for (var i = 0; i < gridData.length; i++) {

                var tname = gridData[i].selectedRowTAB.sTABNameValue;
                if (CurrTabName == tname) {
                    var GridList = {

                        irow_No: parseInt(gridData[i].irow_No),
                        icolumn_No: parseInt(gridData[i].icolumn_No),
                        iTAB_ID: gridData[i].iTAB_ID,
                        STabName: gridData[i].selectedRowTAB.sTABNameValue
                    }
                    Grid_List.push(GridList);
                }
            }

        }
        else
            if (vErr == 4) {

                jAlert("Select TAB Name ", display_Alert);
                $('#selectedRow').data('kendoDropDownList').value("0");
                // $(el).trigger("click");



                return false;
            }

        debugger;

        if (Grid_List.length > 1) {

            var duplicateColors = getDuplicateArrayElements(Grid_List);

            if (duplicateColors.length > 0) {
                jAlert(CurrTabName + ' , ' + msg_SameRowCol, display_Alert);
                $('#selectedRow').data('kendoDropDownList').value("0");
            }


        }
    }
    else {
        $("#GridWorkObject table tbody tr td:nth-child(25)").not(el).each(function () {
            if ($(this).text() == currRowVal && $(this).next().text() == currColVal && currRowVal != display_Select) {
                rowFlg = true;
            }
        });
        $("#GridWorkObject table tbody tr td:nth-child(26)").not($(el).next()).each(function () {
            if ($(this).text() == currColVal && $(this).prev().text() == currRowVal && currColVal != display_Select) {
                colFlg = true;
            }
        });

        if (rowFlg && colFlg && rowFlg > 0 && colFlg > 0) {
            jAlert(msg_SameRowCol, display_Alert);
            $('#selectedRow').data('kendoDropDownList').value("0");

        }
    }
});




/*Check Row and column values are not in same position in preview view */
$(".clsRowPre").on("change", function () {
    debugger;
    var el = this;
    var currRowVal = $(this).find("[id=PreViewselectedRow]").val();
    var currColVal = $(this).next().text();
    var rowFlg = false;
    var colFlg = false;

    $("#gdPreView table tbody tr td:nth-child(3)").not(el).each(function () {
        if ($(this).text() == currRowVal && $(this).next().text() == currColVal) {
            rowFlg = true;
        }
    });
    $("#gdPreView table tbody tr td:nth-child(4)").not($(el).next()).each(function () {
        if ($(this).text() == currColVal && $(this).prev().text() == currRowVal) {
            colFlg = true;
        }
    });

    if (rowFlg && colFlg && rowFlg > 0 && colFlg > 0) {
        jAlert(msg_SameRowCol, display_Alert);
        $('#PreViewselectedRow').data('kendoDropDownList').value("0");

    }
});

function getDuplicateArrayElements(arr) {
    var sorted_arr = arr.slice().sort();
    var results = [];
    for (var i = 0; i <= sorted_arr.length - 1; i++) {
        for (var j = 0; j <= sorted_arr.length - 1; j++) {

            if (i != j) {
                if (sorted_arr[j].irow_No === sorted_arr[i].irow_No && sorted_arr[j].icolumn_No === sorted_arr[i].icolumn_No) {
                    results.push(sorted_arr[i].icolumn_No);
                }
            }
        }
    }
    return results;
}
/*Check Row and column values are not in same position */
//$(".clsColumn").on("change", function () {
$(document).on("change", ".clsColumn", function (e) {
    e.preventDefault();
    debugger;
    var el = this;
    var currColVal = $(this).find("[id=selectedcolumn]").val();
    var currRowVal = $(this).prev().text();
    var rowFlg = false;
    var colFlg = false;
    var IsTABFlag_ = $("input[name=TABMapping]:checked").val();
    var el = "";
    var vErr = 0;
    var Grid_List = [];
    var CurrTabName = "";
    if (IsTABFlag_ == "true") {
        //  if (vErr == 0) {
        // debugger;
        var RowCount = 0;
        if (bClientLanguage == 'True' || bClientLanguage == 'true') {
            $("#GridWorkObject table tbody tr td:nth-child(35)").each(function () {
                if ($(this).text() == "" || $(this).text() == "<--Select-->" || $(this).text() == "--Select--" || $(this).text() == "Select") {
                    el = this;
                    vErr = 4;
                }
                else {
                    if (RowCount == 0) {
                        //  alert($(this).text());
                        CurrTabName = $(this).text();
                    }

                }
                RowCount = RowCount + 1;
            });

        }
        else {
            $("#GridWorkObject table tbody tr td:nth-child(33)").each(function () {

                if ($(this).text() == "" || $(this).text() == "<--Select-->" || $(this).text() == "--Select--" || $(this).text() == "Select") {
                    el = this;
                    vErr = 4;
                }
                else {
                    if (RowCount == 0) {
                        //  alert($(this).text());
                        CurrTabName = $(this).text();
                    }


                }
                RowCount = RowCount + 1;
            });
        }
        if (vErr == 0) {
            var gridData = $("#GridWorkObject").data("kendoGrid").dataSource.data();


            for (var i = 0; i < gridData.length; i++) {

                var tname = gridData[i].selectedRowTAB.sTABNameValue;
                if (CurrTabName == tname) {
                    var GridList = {

                        irow_No: parseInt(gridData[i].irow_No),
                        icolumn_No: parseInt(gridData[i].icolumn_No),
                        iTAB_ID: gridData[i].iTAB_ID,
                        STabName: gridData[i].selectedRowTAB.sTABNameValue
                    }
                    Grid_List.push(GridList);
                }
            }

        }
        else
            if (vErr == 4) {

                jAlert("Select TAB Name ", display_Alert);
                $('#selectedcolumn').data('kendoDropDownList').value("0");
                // $(el).trigger("click");



                return false;
            }

        // debugger;

        if (Grid_List.length > 1) {

            var duplicateColors = getDuplicateArrayElements(Grid_List);

            if (duplicateColors.length > 0) {
                jAlert(CurrTabName + ' , ' + msg_SameRowCol, display_Alert);
                $('#selectedcolumn').data('kendoDropDownList').value("0");
            }


        }
    }
    else {
        $("#GridWorkObject table tbody tr td:nth-child(25)").not($(el).prev()).each(function () {
            if ($(this).text() == currRowVal && $(this).next().text() == currColVal && currRowVal != display_Select) {
                rowFlg = true;
            }
        });

        $("#GridWorkObject table tbody tr td:nth-child(26)").not(el).each(function () {
            if ($(this).text() == currColVal && $(this).prev().text() == currRowVal && currColVal != display_Select) {
                colFlg = true;
            }
        });

        if (rowFlg && colFlg && rowFlg > 0 && colFlg > 0) {
            jAlert(msg_SameRowCol, display_Alert);
            $('#selectedcolumn').data('kendoDropDownList').value("0");

        }
    }

});
/*Check Row and column values are not in same position in preview view */
$(".clsColumnPre").on("change", function () {
    debugger;
    var el = this;
    var currColVal = $(this).find("[id=PreViewselectedcolumn]").val();
    var currRowVal = $(this).prev().text();
    var rowFlg = false;
    var colFlg = false;
    $("#gdPreView table tbody tr td:nth-child(3)").not($(el).prev()).each(function () {
        if ($(this).text() == currRowVal && $(this).next().text() == currColVal) {
            rowFlg = true;
        }
    });
    $("#gdPreView table tbody tr td:nth-child(4)").not(el).each(function () {
        if ($(this).text() == currColVal && $(this).prev().text() == currRowVal) {
            colFlg = true;
        }
    });

    if (rowFlg && colFlg && rowFlg > 0 && colFlg > 0) {
        //  jAlert("Same row and column combination can not be there!", display_Alert);
        jAlert("Same row and column combination can not be there!", display_Alert, function (r) {
            if (r) {
                $('#PreViewselectedcolumn').data('kendoDropDownList').value("0");
                $(el).trigger("click");
                setTimeout(function () {
                    //$('#PreViewselectedcolumn').data('kendoDropDownList').value("0");
                    var data = $("#PreViewselectedcolumn").data("kendoDropDownList");
                    if (data != undefined) {
                        data.select(0);
                        data.trigger("select", {
                            item: $("li.k-state-selected", $("#PreViewselectedcolumn   -list"))
                        });
                    }
                }, 100);
            }
        });
    }
});


/*Check Number Values*/
$(".NoOfRows").bind('keypress', function (e) {

    if (e.shiftKey && this.value.length >= 5) {
        if (String.fromCharCode(e.keyCode).match(/[^0-9]/g)) {
            this.value = this.value.slice(0, 5);
            return false;
        }
        else {
            return true;
        }


    }



});



/*Delete Grid Row*/
function deleteClick(e) {

    var dataItem = this.dataItem($(e.target).closest("tr"));
    jConfirm(display_MsgConfirm, display_Confirmation, function (r) {
        var dataSource = $("#GridChoice").data("kendoGrid").dataSource;
        dataSource.remove(dataItem);
        // dataSource.sync();
    });
};
/*Delete TAB Grid Row*/
function deleteTABClick(e) {

    var dataItem = this.dataItem($(e.target).closest("tr"));
    jConfirm(display_MsgConfirm, display_Confirmation, function (r) {
        var dataSource = $("#GridChoiceTAB").data("kendoGrid").dataSource;
        dataSource.remove(dataItem);
        // dataSource.sync();
    });
};
/*****Bussness Benefits*******/

/*Close Popup*/
function CloseClick() {
    ClosePopup();
};

function RefreshClick() {

};

/*Open Popup*/
function ClosePopup() {
    $("#OpenPartialWorkDefinationKeyBenefits").closest(".k-window-content").data("kendoWindow").close();
};


/*Open Popup*/
function CloseLetterLibraryPopup() {
    $("#OpenPopupLetterLibrary").closest(".k-window-content").data("kendoWindow").close();
};

/*Get process Id*/
function filterBusinessApprover() {
    return {
        iProcessID: $("#ProcessName").val()
    };
};

/*Get process Id*/
function filterTechnologyApprover() {
    return {
        iProcessID: $("#ProcessName").val()

    };
};

/******************Page Preview*************************/
function preview(e) {
    // e.preventDefault();
    $('#winOpen').data("kendoWindow").title("Object Formula")
        .refresh({ url: urlPathObjectFormula }).center().open();

    $.ajax({
        type: 'POST',
        url: urlGetWorkObjectForControlName,
        dataType: 'json',
        success: function (lstTeam) {
            //console.log(lstTeam);

            unmappedtag_datasource = new kendo.data.DataSource({ data: lstTeam });
            mappedtag_datasource = new kendo.data.DataSource({});

            $("#unmappedtag_listview").kendoListView(
                {
                    dataSource: unmappedtag_datasource, template: '<div  class="sortable" >  #:ObjName#</div>'
                });
            setTimeout(function () {
                var tabToActivate = $("#tabstrip-tab-1");
                $("#tabstrip").kendoTabStrip().data("kendoTabStrip").activateTab(tabToActivate);
            }, 5000);

        },
        error: function (ex) {

            jAlert(Dispaly_DataRetrieveError, display_Alert);
        }
    });

}

function onSelectWork() {
    ////
    fillPreViewData()
    $(".reslcode").show();
}
/*Fill PagePriview data*/
function fillPreViewData() {
    var WorkId = $("#PreViewWorkDefinitionName").val() == '' ? '0' : $("#PreViewWorkDefinitionName").val();
    var addAPIActionUrl = urlPathCreateRESTControls;
    var UrlText = $('#txtURLName').val();
    $.ajax({
        url: addAPIActionUrl,
        data: { WorkId: WorkId },
        dataType: "html",
        // contentType: "application/json; charset=utf-8",
        type: "GET",
        // async: false,
        // cache: false,
        error: function (ex) {
            console.log(ex);
            debugger
            jAlert(ex, "Alert");
        },
        success: function (data) {
            //console.log(data);
            $("#divPrivew").html('');
            $('#divPrivew').html(data);
            $("#gdPreView").data("kendoGrid").dataSource.read();
            $("#gdPreView").data("kendoGrid").refresh();
        }
    });
}
//Set Row and column value in grid
function onPreViewSelect(e) {

    var item = $('#gdPreView').data().kendoGrid.dataItem($(this.element).closest('tr'));
    var controlname = $(e.sender.wrapper[0].innerHTML)[1].id;
    //e.sender.wrapper.context.id;

    if (controlname == "PreViewselectedRow") {
        if (this.dataItem(e.item.index()) != undefined) {
            item.PreViewselectedRow = this.dataItem(e.item.index());
            item.irow_No = this.dataItem(e.item.index()).Value;
        }
        else {
            item.PreViewselectedRow = this.dataItem(0);
            item.irow_No = this.dataItem(0).Value;
        }
    }
    else if (controlname == "PreViewselectedcolumn") {
        if (this.dataItem(e.item.index()) != undefined) {
            item.PreViewselectedcolumn = this.dataItem(e.item.index());
            item.icolumn_No = this.dataItem(e.item.index()).Value;
        }
        else {
            item.PreViewselectedcolumn = this.dataItem(0);
            item.icolumn_No = this.dataItem(0).Value;
        }
    }
    else if (controlname == "PreViewselectedcolumnSpan") {

        item.selectedcolumn = this.dataItem(e.item.index());
        item.icolumn_Span = this.dataItem(e.item.index()).Value;
    }
}
//Get Store Value.
function GetStoreID() {
    return {
        iStoreId: $("#PreViewWorkDefinitionName").val() == '' ? 0 : $("#PreViewWorkDefinitionName").val()

    };
}

/*Save PreView Page data*/
function OnSaveButtonClick(event) {

    //$.ajax({
    //    url: "UpdatePreViewDataNew",
    //    type: "POST",
    //    data: { 'SubID': 'a' },
    //    success: function (result) {
    //        console.log(result);
    //    }
    //});
    //return false;
    var grid = $("#gdPreView").data("kendoGrid");
    var DataSource = grid.dataSource;
    var strJason = JSON.stringify(DataSource._data);
    if (DataSource._data.length > 0) {
        var token = $("#form_WorkPreview input[name=__RequestVerificationToken]").val();
        $.ajax({
            type: 'POST',
            url: urlPathUpdatePreViewData,
            dataType: 'json',
            //contentType: "application/json; charset=utf-8",
            data: { __RequestVerificationToken: token, SubJsonString: strJason },
            success: function (result) {

                if (result == "OK") {
                    jAlert(display_UdateDataPreView);
                    fillPreViewData();
                }
                else {
                    jAlert(result);
                }
            }
        });
    }
    else {
        jAlert('Please Select Work Defination  or No data in Grid !');
    }

}

function OnClickRefresh() {
    window.location.href = urlPath_WorkPreview;
}
function OnClickGridRefresh() {
    window.location.href = urlPath_GridConfigurations;
}

/*Formula*/
//function preview(e) {
//    console.warn('Dummy Method to avoid developer error');
//}
$(document).on("click", ".clsFx", function (e) {

    postUrl = ResourceLayout.partialURL + "SetTargetObjectId"; // Url to get Values
    var ObjID = $(this).attr("objid")

    $.ajax({
        type: "POST",
        data: { param: ObjID },
        url: postUrl,
        async: false,
        cache: false,
        dataType: "text",
        success: function (data, status, xhr) {
            //  preview1(e);
        }
    });

});

/************************ search************************/
function GridFillterValue() {
    return {
        iCampaignName: $('#CampaignName').val(),
        sname: $('#Name').val()
    };
}

//Client Selected Value
function WorkDefinition_filterProcess() {
    return {
        iClientID: $("#ClientName").val()
    };
}
//Process Selected Value
function WorkDefinition_filterCampaignName() {
    return {
        iProcessID: $("#ProcessName").val()

    };
}
/*Fill Search*/
function OnUserSearchClick() {
    debugger;
    var columnslist = [];
    //$("#gdFilter").data("kendoGrid").dataSource.read();
    //$("#gdFilter").data("kendoGrid").refresh();
    $.LoadingOverlay("show");
    $.ajax({
        type: "POST"
        , url: urlPathGetFilterList
        , data: {
            iCampaignName: $('#CampaignName').val(),
            sname: $('#Name').val()
        }
        , dataType: "json"
        , success: function (result) {
            $.LoadingOverlay("hide");
            if (result.Total > 0) {

                columnslist.push({ field: "iStoreId", edit: false, hidden: true, width: 130 });
                columnslist.push({ field: "sStoreName", edit: false, width: 300, title: "Work Defintion Name" });
                columnslist.push({ command: { text: "Edit", click: editWork, Class: "k-icon k-i-pencil k-primary", Class: " k-primary k-button k-button-icontext k-grid-Edit" }, });

                var dataSource = new kendo.data.DataSource({
                    pageSize: 10,
                    data: result.Data,
                    //autoSync: true,
                    schema: {
                        model: {
                            Id: "iStoreId",
                            fields: {
                                iStoreId: { editable: false },
                                sStoreName: { editable: false },

                            }

                        }
                    }
                });
                var grid = $('#gdFilter').data("kendoGrid");
                if (grid != undefined) {
                    grid.destroy();
                }

                $("#gdFilter").kendoGrid({
                    columns: columnslist,
                    dataSource: dataSource,
                    height: 500,
                    editable: true,
                    //selectable: "multiple row",
                    dataBound: onDataBound,
                    autoBind: true
                    , pageable: {
                        refresh: true,
                        pageSizes: true,
                        buttonCount: 10,
                        pageSizes: [5, 10, 20, 50, 100],
                    }
                });
            }
            else {
                $("#gdFilter").html("");
                jAlert("Record Not Found.");
            }
        },
        error: function (err) {
            $.LoadingOverlay("hide");
        }
    });



};

/* onDataBound */

function onDataBound(e) {
    $(".k-grid-Edit").addClass("k-primary k-grid-Edit");
    $(".k-grid-Edit").find("span").addClass("k-icon k-edit");
}
/*calling index*/
function editWork(e) {
    debugger;
    var token = $("#formWMSearch input[name=__RequestVerificationToken]").val();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    $.ajax({
        type: "post"
        , url: urlPatheditWorkDefination
        , data: { __RequestVerificationToken: token, iStoreId: dataItem.iStoreId }
        , dataType: "json"
        , success: function (result) {
            if (result == "1") {
                window.location.href = urlPathIndex;
            }
        },
        error: function (err) {

        }
    });
}


/* Get & Set checkBoxs value in grid*/
//$(".chkbxIsVisible,.chkbxbEditable,.chkbxbSearch,.chkbxbRequired,.chkbxbDisabled,.chkbxbTransactionType,.chkbxbbIsUpload,.chkbxbbIsReport,.chkbxbUniqueID,.chkbxbLANID,.chkbxbCustomerIdentifier,.chkbxbbIsTranslate").on("click", function () {

$(document).on("click", ".chkbxIsVisible,.chkbxbEditable,.chkbxbSearch,.chkbxbRequired,.chkbxbDisabled,.chkbxbTransactionType,.chkbxbbIsUpload,.chkbxbbIsReport,.chkbxbUniqueID,.chkbxbLANID,.chkbxbCustomerIdentifier,.chkbxbbIsTranslate", function () {
    var rin = $(this).parent().parent().index();
    var grid = $("#GridWorkObject").data("kendoGrid");

    /* Get & Set Visibility checkBox*/
    if (this.className.toLowerCase().indexOf('chkbxisvisible') >= 0) {
        $(this).is(":checked") == true ? grid.dataSource._data[rin].bVisible = true : grid.dataSource._data[rin].bVisible = false;
    }
    /* Get & Set Editable checkBox*/
    if (this.className.toLowerCase().indexOf('chkbxbeditable') >= 0) {
        $(this).is(":checked") == true ? grid.dataSource._data[rin].bEditable = true : grid.dataSource._data[rin].bEditable = false;

    }
    /* Get & Set Searchable checkBox*/
    if (this.className.toLowerCase().indexOf('chkbxbsearch') >= 0) {
        $(this).is(":checked") == true ? grid.dataSource._data[rin].bSearch = true : grid.dataSource._data[rin].bSearch = false;
    }
    /* Get & Set Reuired checkBox*/
    if (this.className.toLowerCase().indexOf('chkbxbrequired') >= 0) {
        $(this).is(":checked") == true ? grid.dataSource._data[rin].bRequired = true : grid.dataSource._data[rin].bRequired = false;

    }
    /* Get & Set Disabled checkBox*/
    if (this.className.toLowerCase().indexOf('chkbxbdisabled') >= 0) {
        $(this).is(":checked") == true ? grid.dataSource._data[rin].bDisabled = true : grid.dataSource._data[rin].bDisabled = false;
    }


    /* Get & Set Uniqueid checkBox*/
    if (this.className.toLowerCase().indexOf('chkbxbuniqueid') >= 0) {

        $(this).is(":checked") == true ? grid.dataSource._data[rin].bUniqueID = true : grid.dataSource._data[rin].bUniqueID = false;
        //$(".chkbxbUniqueID").not(this).prop("checked", false);
        //$(this).prop("checked", "checked");
    }
    /* Get & Set LANID checkBox*/
    if (this.className.toLowerCase().indexOf('chkbxblanid') >= 0) {
        $(this).is(":checked") == true ? grid.dataSource._data[rin].bLANID = true : grid.dataSource._data[rin].bLANID = false;
        //$(".chkbxbLANID").not(this).prop("checked", false);
        $(this).is(":checked") == true ? $(this).prop("checked", "checked") : $(this).prop("checked", false);
    }
    /* Get & Set Transactionaction Type checkBox*/
    if (this.className.toLowerCase().indexOf('chkbxbtransactiontype') >= 0) {
        $(this).is(":checked") == true ? grid.dataSource._data[rin].bTransactionType = true : grid.dataSource._data[rin].bTransactionType = false;
    }
    /* Get & Set Isupload checkBox*/
    if (this.className.toLowerCase().indexOf('chkbxbbisupload') >= 0) {
        $(this).is(":checked") == true ? grid.dataSource._data[rin].bIsUpload = true : grid.dataSource._data[rin].bIsUpload = false;

    }

    /* Get & Set Customer Identifier  checkBox*/
    if (this.className.toLowerCase().indexOf('customeridentifier') >= 0) {
        $(this).is(":checked") == true ? grid.dataSource._data[rin].bCustomerIdentifier = true : grid.dataSource._data[rin].bCustomerIdentifier = false;

    }


    /* Get & Set Is report checkBox*/
    if (this.className.toLowerCase().indexOf('chkbxbbisreport') >= 0) {
        $(this).is(":checked") == true ? grid.dataSource._data[rin].bIsReport = true : grid.dataSource._data[rin].bIsReport = false;
    }
    if (this.className.toLowerCase().indexOf('chkbxbbIsTranslate') >= 0) {
        $(this).is(":checked") == true ? grid.dataSource._data[rin].bIsTranslate = true : grid.dataSource._data[rin].bIsTranslate = false;
    }

});

/****Letter library*****/

/*--------------------------Begin-Work Approval----------------------*/

/* To set the check box value if any records mark for Approve/Reject/Cancel */
$(function () {

    $('#Approvalgrid').on('click', '.chkbox', function () {
        var checked = $(this).is(':checked');
        var grid = $('#Approvalgrid').data().kendoGrid;
        var dataItem = grid.dataItem($(this).closest('tr'));
        dataItem.IsChecked = checked;
        dataItem.dirty = true;
    })
})
/* To open Request view in pop up */
function Open_ApprovalDetail(level, id) {

    $.ajax({
        url: ResourceLayout.partialURL + "Details/",
        data: { level: level, iApprovalId: id },
        type: "get",
        dataType: "html",
        success: function (data) {
            $("#ApprovalDetail").html(data);
        },
        error: function () {
            jAlert("An error occured");
        }
    });
    var accessWindow = $("#ApprovalDetail").kendoWindow({
        actions: ["Close"],
        draggable: true,
        height: "600px",
        modal: true,
        resizable: false,
        title: title_WorkRequest,
        width: "1000px",
        visible: false,
    }).data("kendoWindow").center().open();

}
/* This genric method  which is used for Approve/Rejected/Canceled
   obj parameter denotes which button was clicked and id dendotes iApprovalId  */
function Work_Approval(obj, id) {
    kendo.ui.progress($('#formWorkApproval'), true);

    var msg = '';
    var token = $("#formWorkApproval input").val();

    $.ajax({
        type: "POST"
        , url: ResourceLayout.partialURL + "Approval"
        , data: { __RequestVerificationToken: token, iApprovalId: id, action: obj.value }
        , dataType: 'json'
        , success: function (result) {
            console.log(result);
            if (result.Value.Successed == true) {
                jAlert(result.msg);
            }
            else if (result.Value.Successed == false) {
                kendo.ui.progress($('#formWorkApproval'), false);
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
                kendo.ui.progress($('#formWorkApproval'), false);
                jAlert(result.Value.msg);
            }

        },
        error: function (err) {
            kendo.ui.progress($('#formWorkApproval'), false);

        }

    });
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
/*To Validate from date and to date*/
function Approval_DateValidation() {

    var fromdt = new Date($("#FromDate").val());
    var todt = new Date($("#ToDate").val());
    if (fromdt > todt) {
        jAlert("From date must be smaller than To date.");
        return false;
    }
    else if (!chkDate(fromdt, todt))
        return false;
    else { Bind_WorkApproval(); return true; }

}
/*This is databind event of grid used to check that any visible Approve/Canceled/Rejected */
function Binding_WorkApproval() {

    if ($("#Approvalgrid table tbody tr").length > 0) {
        var showApp = false;
        var showRej = false;
        var showCan = false;
        $("#spnNoRecord").hide();
        $(".btnAction").show();
        $("#Approvalgrid").show();
        $("#Approvalgrid table tbody tr").each(function () { if ($(this).find('td:eq(16)').html().length > 0) { showCan = true; } });
        $("#Approvalgrid table tbody tr").each(function () { if ($(this).find('td:eq(15)').html().length > 0) { showRej = true; } });
        $("#Approvalgrid table tbody tr").each(function () { if ($(this).find('td:eq(14)').html().length > 0) { showApp = true; } });
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

/*Select all check box for approval/reject/cancel*/
//$("#chkSelectAll").on('click', function (e) {
$(document).on("click", "#chkSelectAll", function () {

    if ($(this).is(":checked")) {
        $(".chkbox").prop("checked", "checked");
    }
    else {
        $(".chkbox").prop("checked", false);
    }
});

/*To apply approval/reject/cancel for multiple row at a time*/
function Save_multiWorkApproval(e) {

    var msg = '';
    var grid = $("#Approvalgrid").data("kendoGrid"),
        parameterMap = grid.dataSource.transport.parameterMap;
    var currentData = grid.dataSource.data();
    var updatedRecords = [];
    var CheckVal = 0;
    var token = $("#formWorkApproval input").val();
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

        $.ajax({
            url: ResourceLayout.partialURL + "ApprovalAction",
            data: { __RequestVerificationToken: token, updated: updatedRecords, modelData: modelData },
            type: "POST",
            error: function (err) {

                //Handle the server errors using the approach from the previous example
            },
            success: function (result) {

                if (result != null) {
                    if (result == display_Cancel) { msg = cancel_workApporval } else if (result == display_Reject) { msg = reject_workApproval } else if (result == display_Approve) { msg = approve_workApproval } else { msg = result.Value }
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
        })
    }
    else {
        jAlert("Please select at least one checkbox Option !");
    }
}

/*To apply filter to read Campaign approval records*/
function GetFilter() {

    return { dFrom: $("#FromDate").val(), dTo: $("#ToDate").val() };
}

/*To read campaign approval grid*/
function Bind_WorkApproval() {

    var grid = $("#Approvalgrid").data("kendoGrid")
    grid.dataSource._destroyed = [];
    grid.dataSource.read();
    if (grid.dataSource.data.length > 0) {
        $("#spnNoRecord").hide();
        $("#Approvalgrid").show();
        $(".btnAction").show();
    }
}
/*This is used for Request view when user want save change request*/
function Save_workRequest() {

    if ($("#ChangeRequest").val() == '' && $("#IsLevel").val() == 1) {
        jAlert(display_commentmsg);
        $("#ChangeRequest").focus();
        return false;
    }
    var BEWorkObjectApprover = {};
    var token = $("#formWorkMaster input[name=__RequestVerificationToken]").val();
    var postUrl;
    BEWorkObjectApprover.iApprovalId = $("#iApproverId").val();
    BEWorkObjectApprover.sLocations = $("#Location").val();
    BEWorkObjectApprover.sShiftwindows = $("#ShiftWindow").val();
    BEWorkObjectApprover.PurposeofcreationofWork = $("#Purposeofcreationofwork").val();
    BEWorkObjectApprover.sBusinessJustifications = $("#BusinessJustifications").val();
    BEWorkObjectApprover.sTargetq1 = $("#Q1").val();
    BEWorkObjectApprover.sTargetq2 = $("#Q2").val();
    BEWorkObjectApprover.sTargetq3 = $("#Q3").val();
    BEWorkObjectApprover.sTargety1 = $("#Y1").val();
    BEWorkObjectApprover.sTargety2 = $("#Y2").val();
    BEWorkObjectApprover.sTargety3 = $("#Y3").val();
    BEWorkObjectApprover.skeybenifits = $("#KeyBenefits").val();
    postUrl = ResourceLayout.partialURL + "SaveRequestDetail";
    // BEWorkObjectApprover = { 'objBEWorkObjectApprover': BEWorkObjectApprover, UserLevel: $("#IsLevel").val(), ChangeReq: $("#ChangeRequest").val() };//JSON.stringify(
    $.ajax({
        url: postUrl,
        type: 'POST',
        dataType: 'json',
        data: { __RequestVerificationToken: token, objBEWorkObjectApprover: BEWorkObjectApprover, UserLevel: $("#IsLevel").val(), ChangeReq: $("#ChangeRequest").val() },
        success: function (result) {
            if (result != null) {
                $("#btnAddBussinessJustification").attr("disabled", "disabled");
                jAlert(submitMsg_workRequest);
            }

        },
        error: function (err) {

        }
    });
}
/*--------------------------End-Work Approval----------------------*/




/*------------------------------------------------------------------------------------------------------------------------------------------
----------------------------------------------------------------------- Start Formula Builder ----------------------------------------------
--------------------------------------------------------------------------------------------------------------------------------------------*/

//// Variables
var Formula = "";
var HiddenFormula = "";

// To Undo Formula Text Box
var formula = [];
var hidden_formula = [];
var hformula = "";
var dataItem_lstObject = ""; // Object
var dataItem_Events = "";  // Event
var dataItem_TargetlstObject = ""; // Event Target Object
var dataItem_Operator = ""; // Operator
var dataItem_Property = "";
var dataItem_Constant = "";
var str1 = "";

// Method to edit formula
function editObjectFormula2(e) {
    // debugger;
    e.preventDefault();

    var Formula = "";
    var HiddenFormula = "";
    var Events = "";
    var iDObjID = "";
    var index = $(e.currentTarget).closest("tr")[0].rowIndex;
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var token = $("#formObjectFormula input[name=__RequestVerificationToken]").val();
    postUrl = ResourceLayout.partialURL + "getEditRecords";

    $.ajax({
        type: "POST",
        url: ResourceLayout.partialURL + "getEditRecords"
        , data: { __RequestVerificationToken: token, objID: dataItem.iObjectId }
        , datatype: "json"
        , success: function (result) {
            var Data = result.split('#$$');
            iDObjID = Data[0];
            Formula = Data[3];
            HiddenFormula = Data[4];
            hformula = Data[4];
            Events = Data[2];
            var dropdownlist = $("#Events").data("kendoDropDownList");
            dropdownlist.select(function (dataItem) {
                return dataItem.Text === Events;
            });
            debugger;
            $("#Formula").val(Formula);
            var editor = $("#HiddenFormula").data("kendoEditor");
            //editor.value(HiddenFormula); 
            editor.value(hformula);
            //$("#HiddenFormula").val(HiddenFormula);
            $("#ObjFormulaID").val(iDObjID);
            $("#btnUpdate").show();
            if (Data[5] == "True") { $("#Disable").attr('checked', true); } else { $("#Disable").attr('checked', false); }

            formula.push(Formula);
            hidden_formula.push(HiddenFormula);

        }
    });

};

// Method to Delete Formula
function deleteObjectFormula(e) {
    var index = $(e.currentTarget).closest("tr")[0].rowIndex;
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var token = $("#formObjectFormula input[name=__RequestVerificationToken]").val();
    jConfirm(display_Delete_Confirmation, display_Confirmation, function (r) {
        if (r) {
            e.preventDefault();

            $.ajax({
                type: "POST"
                , url: ResourceLayout.partialURL + "Delete"
                , data: { __RequestVerificationToken: token, iObjectId: dataItem.iObjectId }
                , dataType: 'json'
                , success: function (result) {

                    if (result == "2") {
                        $("#searchGrid  table > tbody tr:eq(" + index + ")").remove();
                        jAlert(display_Delete);
                        Refresh();
                    }
                }
            });
        }
        else {
            return false;
        }
    });
};

// Methods to get Dropdowns Text and values

////// For Event DDL
function onSelectEvents(e) {

    dataItem_Events = this.dataItem(e.item);
}

////// For Property Property DDL

function OnSelectProperty(e) {

    dataItem_Property = this.dataItem(e.item);
}

////// For TargetlstObject DDL
function onSelectTargetlstObject(e) {

    dataItem_TargetlstObject = this.dataItem(e.item);
}

/////  For Operator DDL 
function onSelectOperator(e) {

    dataItem_Operator = this.dataItem(e.item);
}

/////// For Constatns DDL
function onSelectConstants(e) {

    dataItem_Constant = this.dataItem(e.item);
}

///// End of Such Methods
///////////////// Populat Formula
populate = function () {
    //debugger;
    $("#Formula").val('');
    var editor = $("#HiddenFormula").data("kendoEditor");
    //if (editor != null) {
    //    editor.value('');
    //}
    //editor.value(''); 
    //$("#HiddenFormula").val('');

    $("#Formula").val(formula.join(''));
    if (editor != null) {
        if (editor.value() != '') {
            editor.value('');
            // editor.value(hidden_formula.join(''));
            editor.value(hformula);

        }
        else {
            var lt = hidden_formula.length;
            // editor.value(hidden_formula[lt-1]);
            editor.value(hformula);
        }

    }

    // $("#HiddenFormula").val(hidden_formula.join(''));


};
var GetTranslate = [];
/////// Document Ready Method
$(document).ready(function (e) {


    //$("#btnRef").hide();
    $("#btnPropertyValue").hide();
    $("#txtproperty").hide();
    $("#lblProperty").hide(); //Property label
    $("#Formula").val('');
    var editor = $("#HiddenFormula").data("kendoEditor");
    if (editor != null) {
        editor.value('');
    }

    //$("#HiddenFormula").val('');
    $("#ddlproperty").parent().hide(); //Property Dropdown

    formula = [];
    hidden_formula = [];
    hformula = "";
    populate();

    if (ClientLanguagebol.toUpperCase() == "TRUE") {
        $.ajax({
            type: "Get"
            , url: urlGetLanguage
            , dataType: 'json',
            success: function (result) {
                if (result != "") {
                    GetTranslate = result;
                }

            }
            , error: function (result) {
            }
        });
    }

});




///////////////////////////////////////////// Undo Formula /////////////////////////////////////////
$("#btnUndo").unbind("click").click(function () {
    if (formula.length > 0 && hidden_formula.length > 0) {
        // remove last element of the array
        formula.splice(-1, 1);
        hidden_formula.splice(-1, 1);
        hformula = hidden_formula.join('');//hformula.replace("");
        populate();
    }
});

///////////////////////////////////////////////////////////// btn Clear Formula //////////////////////////
$("#btnClearFormula").unbind("click").click(function () {
    debugger;

    if ($("#Formula").val() != "") {
        $("#Formula").val('');
        var editor = $("#HiddenFormula").data("kendoEditor");
        editor.value('');
        //$("#HiddenFormula").val('');
        formula = [];
        hidden_formula = [];
        hformula = "";
    }
    else if ($("#Formula").text() != "") {
        $("#Formula").text('');
        var editor = $("#HiddenFormula").data("kendoEditor");
        editor.value('');
        //$("#HiddenFormula").val('');
        formula = [];
        hidden_formula = [];
        hformula = "";
    }
    else {
        jAlert(displayNoFormulaToClear);
    }


});


/////////////////////////////////// btnAdd //////////////////////////////////////////////////////////
$("#btnAdd").unbind("click").click(function () {
    // debugger;
    if ($("#lstObject").val() == "") {
        jAlert(displayPleaseSelectObject);
        return false;
    }
    else if ($("#lstObject").val() == "0") {
        jAlert(displayPleaseSelectObject);
        return false;
    }
    else {
        var editor = $("#HiddenFormula").data("kendoEditor");
        hformula = editor.value();
        var ddl_obj_Value = $("#lstObject").val();
        var ddl_Operator_Value = $("#Operator").val();

        var ddl_obj_Text = dataItem_lstObject != "" ? dataItem_lstObject.Text.split('(')[0] : "";
        var ddl_Operator_Text = dataItem_Operator.Text;

        if (ddl_Operator_Value != "0") {

            if (ddl_Operator_Text == "+" || ddl_Operator_Text == "-" || ddl_Operator_Text == "*" || ddl_Operator_Text == "/") {


                Formula = Formula + ddl_obj_Text;
                HiddenFormula = HiddenFormula + ddl_obj_Value;
                formula.push(ddl_obj_Text);
                hidden_formula.push(ddl_obj_Value);
                hformula = hformula + ddl_obj_Value;
                populate();
            }
            else {
                Formula = Formula + ddl_obj_Text;
                HiddenFormula = HiddenFormula + ddl_obj_Value;

                formula.push(ddl_obj_Text);
                hidden_formula.push(ddl_obj_Value);
                hformula = hformula + ddl_obj_Value;
                populate();
            }
        }
        else {

            HiddenFormula = HiddenFormula + ddl_obj_Value;
            formula.push(ddl_obj_Text);
            hidden_formula.push(ddl_obj_Value);
            hformula = hformula + ddl_obj_Value;
            populate();

        }
    }


});

/////////////////////////////////// btnAddBussinessJustification //////////////////////////////////////////////////////////
$("#btnAddBussinessJustification").unbind("click").click(function () {
    //debugger;
    if ($("#lstObject").val() == "0") {
        jAlert(displayPleaseSelectObject);
        return false;
    }
    else {
        var ddl_obj_Value = $("#lstObject").val();
        var ddl_Operator_Value = $("#Operator").val();

        var ddl_obj_Text = dataItem_lstObject.Text;
        var ddl_Operator_Text = dataItem_Operator.Text;
        var editor = $("#HiddenFormula").data("kendoEditor");
        hformula = editor.value();
        if (ddl_Operator_Value != "0") {

            if (ddl_Operator_Text == "+" || ddl_Operator_Text == "-" || ddl_Operator_Text == "*" || ddl_Operator_Text == "/") {


                Formula = Formula + ddl_obj_Text;
                HiddenFormula = HiddenFormula + ddl_obj_Value;
                formula.push(ddl_obj_Text);
                hidden_formula.push(ddl_obj_Value);
                hformula = hformula + ddl_obj_Value;
                populate();
            }
            else {
                Formula = Formula + ddl_obj_Text;
                HiddenFormula = HiddenFormula + ddl_obj_Value;

                formula.push(ddl_obj_Text);
                hidden_formula.push(ddl_obj_Value);
                hformula = hformula + ddl_obj_Value;
                populate();
            }
        }
        else {

            HiddenFormula = HiddenFormula + ddl_obj_Value;
            formula.push(ddl_obj_Text);
            hidden_formula.push(ddl_obj_Value);
            hformula = hformula + ddl_obj_Value;
            populate();

        }
    }


});

/////////////////////////////// btnOperator  ////////////////////////////////////////////////////////
$("#btnOperator").unbind("click").click(function () {
    // debugger;
    var ddl_obj_Value = $("#lstObject").val();
    var ddl_Operator_Value = $("#Operator").val();
    var ddl_obj_Text = "";
    var ddl_Operator_Text = "";
    //debugger;
    if (ddl_Operator_Value == "") {
        jAlert(requiredOperator);
        return false;
    }
    else if (ddl_Operator_Value == "-1") {
        jAlert(requiredOperator);
        return false;
    }

    ddl_obj_Text = dataItem_lstObject.Text;
    ddl_Operator_Text = dataItem_Operator.Text;
    var editor = $("#HiddenFormula").data("kendoEditor");
    hformula = editor.value();
    // debugger;
    if (ddl_Operator_Text == "=") {

        //if (hvalue.substring(hvalue.length - 11, 11) == ".ToString()") {

        hidden_formula.push(' ' + ddl_Operator_Text);
        hformula = hformula + ' ' + ddl_Operator_Text;
        formula.push('\n' + ddl_Operator_Text + '\n');
        populate();
        //}
        //if ($("#HiddenFormula").val().substring($("#HiddenFormula").val().length - 11, 11) == ".ToString()") {

        //    hidden_formula.push(' ' + $("#HiddenFormula").val().substring(0, $("#HiddenFormula").val().length - 10) + ' ');
        //    populate();
        //}
    }
    else {
        if (formula.length > 0) {
            if (ddl_Operator_Text == "{" || ddl_Operator_Text == "}" || ddl_Operator_Text == "Else") {

                if (formula.length >= 2 && $("#Formula").val().substring($("#Formula").val().length - 2, 2) != '\n') {
                    formula.push('\n' + ddl_Operator_Text + '\n');
                    populate();
                }
                else {
                    formula.push(' ' + ddl_Operator_Text + ' ' + '\n');
                    populate();
                }

            }
            else if (ddl_Operator_Text == ";") {
                formula.push(ddl_Operator_Text + '\n');
                populate();
            }
            else if (ddl_Operator_Text.toUpperCase() == "IF" || ddl_Operator_Text.toUpperCase() == "ELSEIF") {

                formula.push('\n' + ' ' + ddl_Operator_Text + ' ');
                populate();
            }
            else {
                formula.push(' ' + ddl_Operator_Text + ' ');
                populate();
            }
        }
        else {
            formula.push(' ' + ddl_Operator_Text + ' ');
            populate();
        }

        if (ddl_Operator_Text == ".AddDays(" || ddl_Operator_Text == ".ToString()") {
            formula.push(' ' + ddl_Operator_Text + ' ');
            hidden_formula.push(ddl_Operator_Value);
            hformula = hformula + ddl_Operator_Value;
            populate();
        }
        else {

            hidden_formula.push(ddl_Operator_Value);
            hformula = hformula + ddl_Operator_Value;
            populate();
        }
    }


});

//////////////////////////// btnConstant ////////////////////////////////////////////////////////////
$("#btnConstant").unbind("click").click(function () {
    var Operator = dataItem_Operator.Text;
    var ddl_obj_Value = $("#lstObject").val();
    var ddl_Operator_Value = $("#Operator").val();
    var ddl_obj_Text = "";
    var ddl_Operator_Text = "";


    if ($("#Constant").val() == "") {
        jAlert(requiredConstant);
    }
    else if ($("#Constant").val() == "0") {
        jAlert(requiredConstant);
    }
    else {
        var editor = $("#HiddenFormula").data("kendoEditor");
        hformula = editor.value();
        formula.push(dataItem_Constant.Text);
        hidden_formula.push("(((TextBox)pg1.FindName(" + "\"" + $("#Constant").val() + "\"" + ")).Text)");
        hformula = hformula + "(((TextBox)pg1.FindName(" + "\"" + $("#Constant").val() + "\"" + ")).Text)";
        populate();
    }
});

/////////////////////////////// btnPropertyValue /////////////////////////////////////////////////////
$("#btnPropertyValue").unbind("click").click(function () {
    var Operator = dataItem_Operator.Text;
    var ddl_obj_Value = $("#lstObject").val();
    var ddl_Operator_Value = $("#Operator").val();
    var CheckValue = "Value";
    var strVisibility = "Visibility";
    var strEnable = "Enable";
    var strChecked = "Checked";
    var ddl_obj_text = dataItem_lstObject.Text;
    var sad = $("#btnPropertyValue").visible;
    if ($("#btnPropertyValue").is(":visible")) {
        if ($('#ddlproperty').is(":visible") && $('#ddlproperty').val() != "0") {
            jAlert("Please Select Property DDL.");
            return false;
        }

        if ($('#txtproperty').is(":visible") && $('#txtpro perty').val() == "") {
            jAlert("Please Provide Value.");
            return false;
        }
    }
    var ResultCheckValue = ddl_obj_text.search(CheckValue);
    var ResultCheckVisibility = ddl_obj_text.search(strVisibility);
    var ResultCheckEnable = ddl_obj_text.search(strEnable);
    var ResultCheckChecked = ddl_obj_text.search(strChecked);
    var editor = $("#HiddenFormula").data("kendoEditor");
    hformula = editor.value();
    if (ResultCheckValue != "-1" && $("#Operator").val() != ".AddDays(" && $("#Operator").val() != "MessageBox.Show(" && $("#Operator").val() != "throw new NotImplementedException(" && $("#Operator").val() != ".ToString()") {
        if (ddl_Operator_Value == "+" || ddl_Operator_Value == "-" || ddl_Operator_Value == "*" || ddl_Operator_Value == "/") {

            formula.push($("#txtproperty").val());
            hidden_formula.push("\"" + $("#txtproperty").val() + "\"");
            hformula = hformula + "\"" + $("#txtproperty").val() + "\"";
            populate();
        }

        else {

            formula.push($("#txtproperty").val());
            hidden_formula.push("\"" + $("#txtproperty").val() + "\"");
            hformula = hformula + "\"" + $("#txtproperty").val() + "\"";
            populate();
        }

    }
    else if (ResultCheckVisibility != "-1" || ResultCheckEnable != "-1" || ResultCheckChecked != "-1") {
        if ($("#ddlproperty").val() != "-1") {

            formula.push($("#ddlproperty").data("kendoDropDownList").text());
            hidden_formula.push($("#ddlproperty").val());
            hformula = hformula + $("#ddlproperty").val();
            populate();

        }
        else {

            formula.push($("#txtproperty").val());
            hidden_formula.push("\"" + $("#txtproperty").val() + "\"");
            hformula = hformula + "\"" + $("#txtproperty").val() + "\"";
            populate();

        }
    }
    else if ($("#Operator").val().trim() == "MessageBox.Show(" || $("#Operator").val().trim() == "throw new NotImplementedException(") {

        formula.push($("#txtproperty").val());
        hidden_formula.push("\"" + $("#txtproperty").val() + "\"");
        hformula = hformula + "\"" + $("#txtproperty").val() + "\"";
        populate();

    }
    else if ($("#Operator").val().trim() == ".AddDays(" || $("#Operator").val().trim() == ".ToString()") {

        formula.push($("#txtproperty").val());
        hidden_formula.push("\"" + $("#txtproperty").val() + "\"");
        hformula = hformula + "\"" + $("#txtproperty").val() + "\"";
        populate();

    }
    else if ($("#Operator").val().trim() == "DateTime.Parse(" || $("#Operator").val().trim() == "int.Parse()") {

        formula.push($("#txtproperty").val());
        hidden_formula.push("\"" + $("#txtproperty").val() + "\"");
        hformula = hformula + "\"" + $("#txtproperty").val() + "\"";
        populate();
    }

});

//------------------------------ Refresh ----------------------------------------------------------//
$("#btnRef").unbind("click").click(function () {
    Refresh();
});

function Refresh() {

    //$("#btnRef").hide(100);
    $("#remarks").val('');

    $("#txtproperty").hide();
    $("#lblProperty").hide();
    $("#txtproperty").val('');
    $("#Formula").text('');
    $("#lstObject").data("kendoDropDownList").value("0");
    $("#Events").data("kendoDropDownList").value("0");
    $("#Operator").data("kendoDropDownList").value("-1");
    $("#Constant").data("kendoDropDownList").value("0");
    $("#btnPropertyValue").hide(200);
    var editor = $("#HiddenFormula").data("kendoEditor");
    if (editor != null) {
        editor.value('');
    }

    //$("#HiddenFormula").val('');
    formula = [];
    hidden_formula = [];
    hformula = "";
    $("#Disable").attr('checked', false);
    $("#ddlproperty").parent().hide();
    $("#ddlproperty").hide();
    //$("#ddlProperty").hide();
};

/////// Submit Button Click 

$("#btnSubmitObjectFormula").unbind("click").click(function () {
    debugger;
    var WorkDefinitionViewModel = {};
    var token = $("#formObjectFormula input[name=__RequestVerificationToken]").val();
    WorkDefinitionViewModel.lstObject = $("#lstObject").val();
    WorkDefinitionViewModel.Events = $("#Events").val();
    WorkDefinitionViewModel.Events = $("#Events").data("kendoDropDownList").text();

    WorkDefinitionViewModel.Formula = $("#Formula").val();
    var editor = $("#HiddenFormula").data("kendoEditor");

    var str2 = editor.value();
    var str = str2.replace(/(<([^>]+)>)/ig, "");
    str = str.replace(/</g, "#$").replace(/>/g, "$#").replace(/&nbsp;/g, ' ').replace(/[\s(&nbsp;]+$/g, ' ');
    // str = str.replace(/&lt;/g, "<").replace(/&gt;/g, ">");
    str = str.replace(/&amp;/g, "&");

    // str = str.replace(/&lt;/g, '<').replace(/&gt;/g, '>')
    // str = str.replace(/\>/g, "&gt;")   //for >

    WorkDefinitionViewModel.HiddenFormula = str;

    if ($('#Disable').is(':checked')) {
        WorkDefinitionViewModel.Disable = true
    } else {
        WorkDefinitionViewModel.Disable = false
    }
    WorkDefinitionViewModel.EventName = dataItem_Events.Text;
    WorkDefinitionViewModel.ObjFormulaID = $("#ObjFormulaID").val();
    if ($("#Events").val() == "") {
        jAlert(requiredEvents);
        return false;
    }
    if ($("#Formula").val() == "") {
        jAlert(requiredFormula, display_Alert);
        return false;
    }

    postUrl = ResourceLayout.partialURL + "SaveObjectFormula";
    postUrl2 = ResourceLayout.partialURL + "UpdateObjectFormula";

    $.ajax({
        url: postUrl,
        type: 'POST',
        dataType: 'json',
        data: { __RequestVerificationToken: token, ObjectFormulaViewModel: WorkDefinitionViewModel },
        success: function (result) {

            if (result == "2") {
                jConfirm(confirmFormulaReplacement, display_Confirmation, function (r) {
                    if (r) {
                        $.ajax({
                            url: postUrl2,
                            type: 'POST',
                            dataType: 'json',
                            data: { __RequestVerificationToken: token, ObjectFormulaViewModel: ObjectFormulaViewModel },
                            success: function (result) {

                                if (result == "1") {
                                    jAlert(displayFormulaSavedSuccessfully);
                                    $("#searchGrid").data("kendoGrid").dataSource.read();
                                    Refresh()
                                }
                            },
                            error: function (err) {

                                jAlert(err.responseText);
                            }
                        });
                    }
                    else {
                        return false;
                    }
                });
            }
            else if (result == "1") {
                jAlert(displayFormulaSavedSuccessfully);
                $("#searchGrid").data("kendoGrid").dataSource.read();
                Refresh()
            }
            else if (result == null) {
                jAlert(displayFormilaUpdatedSuccessfully);
                $("#searchGrid").data("kendoGrid").dataSource.read(); Refresh();
            }
            else {
                jAlert("Invalid formula,So Please create valid formula (" + result + ")");
            }
        },
        error: function (err) {

            jAlert(err.responseText);
        }
    });
});

///// On Selection of Objects
function onSelectlstObject(e) {

    dataItem_lstObject = this.dataItem(e.item);
    // 
    var CheckValue = "Value";
    var strVisibility = "Visibility";
    var strEnable = "Enable";
    var strChecked = "Checked";
    var str1 = e.item[0].innerHTML;
    var ResultCheckValue = str1.search(CheckValue);
    var ResultCheckVisibility = str1.search(strVisibility);
    var ResultCheckEnable = str1.search(strEnable);
    var ResultCheckChecked = str1.search(strChecked);
    var param = ""; //  Parameter to bind ddlProperty

    postUrl = ResourceLayout.partialURL + "getSelectList"; // Url to get Values
    if (ResultCheckValue != '-1') {
        $("#btnRef").show(100);
        $("#remarks").show(100);
        $("#remarks").val(requiredFixValue);
        $("#remarks").prop("readonly", true);
        $("#ddlproperty").parent().hide();
        $("#txtproperty").parent().show();
        $("#txtproperty").show();
        $("#lblProperty").show();
        $("#btnPropertyValue").show();
        param = 0;
    }
    else if (ResultCheckVisibility != '-1' || ResultCheckEnable != '-1' || ResultCheckChecked != '-1') {

        if ($("#Operator").val() == null || $("#Operator").val() != "MessageBox.Show(" || $("#Operator").val() != "throw new NotImplementedException(") {
            $("#remarks").show(100);
            $("#remarks").val(requiredProoperty);
            $("#txtproperty").parent().show();
            $("#txtproperty").hide(100);
            $("#ddlproperty").parent().show();
            $("#btnPropertyValue").hide();
        }
        if (ResultCheckVisibility != '-1') {
            param = 1;
        }
        if (ResultCheckEnable != '-1' || ResultCheckChecked != '-1') {
            param = 2;
        }
    }
    else {
        //$("#btnRef").hide();
        $("#remarks").val('');
        $("#txtproperty").hide();
        $("#txtproperty").val('');
        $("#lblProperty").hide(); //Property label
        $("#ddlproperty").parent().hide();
        $("#btnPropertyValue").hide();//Property button
    }

    if (ResultCheckVisibility != '-1' || ResultCheckEnable != '-1' || ResultCheckChecked != '-1') {
        $.ajax({
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify({ param: param }),
            url: postUrl,
            dataType: "json",
            contentType: 'application/json; charset=utf-8',
            success: function (data) {

                $("#ddlproperty").children().remove();
                if (data) {

                    $("#ddlproperty").kendoDropDownList({
                        dataTextField: "Key",
                        dataValueField: "Value",
                        dataSource: data
                    });
                    $("#lblProperty").show(); //Property label

                    $("#btnPropertyValue").show();
                }

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {

                //$("#btnRef").hide();
                $("#remarks").val('');

            }
        });
    }
    $("#btnRef").show(200);
};

//Language Code Start

function OpenTranslateP(e) {

    var TranslateTxt = this.dataItem($(e.currentTarget).closest("tr")).sObjectLabel;//sObjectTranslateLabel;
    if (TranslateTxt != null) {
        var accessWindow = $("#OpenPartialPopupTranslate").kendoWindow({
            actions: ["Close"],
            draggable: true,
            height: "300px",
            modal: true,
            resizable: false,
            title: "Translate ",
            width: "700px"
        }).data("kendoWindow").center().open();
        var TranslateList = [];
        $.each(GetTranslate, function (key, value) {
            if (value.sCulture.split('-')[0] != 'en') {
                translate('en', value.sCulture.split('-')[0], TranslateTxt, function (err, result) {
                    if (!err) {

                        var lstval = {
                            iLanguageID: value.iLanguageID,
                            sCulture: value.sCulture,
                            sLanguage: value.sLanguage,
                            sConvertText: result
                        }
                        TranslateList.push(lstval);
                        $('#GridTranslate').data('kendoGrid').dataSource.data(TranslateList);
                        $('#GridTranslate').data('kendoGrid').refresh();
                    } else {

                        alert(err);
                    }
                });
            }
            else {
                var lstval = {
                    iLanguageID: value.iLanguageID,
                    sCulture: value.sCulture,
                    sLanguage: value.sLanguage,
                    sConvertText: TranslateTxt
                }
                TranslateList.push(lstval);
                $('#GridTranslate').data('kendoGrid').dataSource.data(TranslateList);
                $('#GridTranslate').data('kendoGrid').refresh();
            }
        });
    }
    else {
        jAlert("Please fill Object Label Text !");
    }
};

function translate(source, target, content, callback) {
    $.ajax({
        method: 'GET',
        url: LanguageUrl,
        dataType: 'text',
        async: false,
        data: {
            source: source,
            target: target,
            input: content
        },
        success: function (data) {
            if (typeof data === 'string')
                try {
                    data = JSON.parse(data);
                } catch (exp) {

                }

            var err;

            if (data && data.outputs && Array.isArray(data.outputs)) {
                data = data.outputs[0];

                if (data && data.output)
                    data = data.output;

                if (data && data.error)
                    err = data.error;
            }

            callback(err, data);
        },
        error: function (xhr, status, err) {

            callback(err);
        }
    });
};

function OpenTranslateChoice(e) {

    var TranslateTxt = this.dataItem($(e.currentTarget).closest("tr")).sChoiceValue;//sObjectTranslateLabel;
    if (TranslateTxt != null) {
        var accessWindow = $("#OpenPartialPopupChoiceTranslate").kendoWindow({
            actions: ["Close"],
            draggable: true,
            height: "300px",
            modal: true,
            resizable: false,
            title: "Translate ",
            width: "700px"
        }).data("kendoWindow").center().open();
        var TranslateList = [];
        $.each(GetTranslate, function (key, value) {
            if (value.sCulture.split('-')[0] != 'en') {
                translate('en', value.sCulture.split('-')[0], TranslateTxt, function (err, result) {
                    if (!err) {

                        var lstval = {
                            iLanguageID: value.iLanguageID,
                            sCulture: value.sCulture,
                            sLanguage: value.sLanguage,
                            sConvertText: result
                        }
                        TranslateList.push(lstval);
                        $('#GridChoiceTranslate').data('kendoGrid').dataSource.data(TranslateList);
                        $('#GridChoiceTranslate').data('kendoGrid').refresh();
                    } else {

                        alert(err);
                    }
                });
            }
            else {
                var lstval = {
                    iLanguageID: value.iLanguageID,
                    sCulture: value.sCulture,
                    sLanguage: value.sLanguage,
                    sConvertText: TranslateTxt
                }
                TranslateList.push(lstval);
                $('#GridChoiceTranslate').data('kendoGrid').dataSource.data(TranslateList);
                $('#GridChoiceTranslate').data('kendoGrid').refresh();
            }
        });
    }
    else {
        jAlert("Please fill Object Label Text !");
    }
};


function ConvertTextValues(Text) {

    var TranslateListTemp = [];

    $.each(GetTranslate, function (key, value) {
        if (value.sCulture.split('-')[0] != 'en') {
            if (bIsTranslate) {
                translate('en', value.sCulture.split('-')[0], Text, function (err, result) {
                    if (!err) {

                        var lstval = {
                            iLanguageID: value.iLanguageID,
                            sCulture: value.sCulture,
                            sLanguage: value.sLanguage,
                            sConvertText: result
                        }
                        TranslateListTemp.push(lstval);

                    } else {

                        alert(err);
                    }
                });
            }
            else {
                var lstval = {
                    iLanguageID: value.iLanguageID,
                    sCulture: value.sCulture,
                    sLanguage: value.sLanguage,
                    sConvertText: Text
                }
                TranslateListTemp.push(lstval);
            }
        }
        else {
            var lstval = {
                iLanguageID: value.iLanguageID,
                sCulture: value.sCulture,
                sLanguage: value.sLanguage,
                sConvertText: Text
            }
            TranslateListTemp.push(lstval);

        }
    });
    return TranslateListTemp;
}
//Language Code End
/*Delete  Grid Row*/
function delete_GRDClick(e) {

    var dataItem = this.dataItem($(e.target).closest("tr"));
    jConfirm(display_MsgConfirm, display_Confirmation, function (r) {
        var dataSource = $("#GridObjectName").data("kendoGrid").dataSource;
        dataSource.remove(dataItem);
        // dataSource.sync();
    });
};
//-------- Add Grid Control Name Below Grid Values-----------------
function GridAddControls(e) {
    //debugger;
    var errors = "";
    var gridData = $("#GridWorkObject").data("kendoGrid").dataSource.data();
    if (gridData.length == 1) {
        if (gridData[0].sObjectName != null) {
            errors += ObjectName + ", "
        }
    }
    if (errors != "") {
        errors = "Please Save Grid Control Data ! ";
        jAlert(errors, display_Alert);
        return false;
    }
    else {


        var dataItem = this.dataItem($(e.target).closest("tr"));
        //dataItem.sChoiceValue;

        $("#Grdobjname").html(dataItem.sChoiceValue)
        var gridObjName = $("#GridObjectName").data("kendoGrid").dataSource.data();
        var bDisabled = false;
        var bGrdEditable = false;
        for (var i = 0; i < gridObjName.length; i++) {

            //alert(dataItem.sChoiceValue);
            //alert(gridObjName[i].sChoiceValue);
            if (dataItem.iObjID == gridObjName[i].iObjID) {
                bGrdEditable = $("#GridObjectName tbody").find("tr").eq(i).find("td").eq(3).find("input").is(":checked");
                bDisabled = $("#GridObjectName tbody").find("tr").eq(i).find("td").eq(4).find("input").is(":checked");
            }

        }
        //debugger;
        // alert(bVisible);
        var GrdobjRowNum = dataItem.iObjectChoiceID != "0" ? dataItem.iObjectChoiceID : dataItem.iObjID;
        $("#GrdobjnameDisable").html(bDisabled)
        $("#GrdobjRowNum").html(GrdobjRowNum)
        $("#GrdobjnameID").html(dataItem.iObjectChoiceID != "0" ? dataItem.iObjectChoiceID : "0")
        $("#GrdobjEditable").html(bGrdEditable)
        $.LoadingOverlay("show");
        $.ajax({
            url: urlPathGetGridRefresh,
            type: 'POST',
            dataType: 'json',
            data: { GridID: GrdobjRowNum, AFlag: "", iProcessID: $("#ProcessName").val(), iCampaignID: $("#CampaignName").val(), sGridObjectName: dataItem.sChoiceValue },
            success: function (result) {
                $.LoadingOverlay("hide");
                if (result.displaymsg == 'REF') {
                    var dataSource = new kendo.data.DataSource({ data: result.lstWorkDefinitionGRD });
                    var grid = $('#GridWorkObject').data("kendoGrid");
                    dataSource.read();
                    grid.setDataSource(dataSource);

                }
                else if (result.displaymsg == 'YES') {
                    var dataSource = new kendo.data.DataSource({ data: result.lstWorkDefinitionGRD });
                    var grid = $('#GridWorkObject').data("kendoGrid");
                    dataSource.read();
                    grid.setDataSource(dataSource);

                }
            },
            error: function (err) {
                $.LoadingOverlay("hide");
                kendo.ui.progress($('#formWorkMaster'), false);
            }
        });

    }

}

function GRDSaveWorkData() {
    //kendo.ui.progress($('#formWorkMaster'), true);
    var WorkDefinitionViewModel = {};
    var objBEWorkObject = [];
    _oTranslateList = [];
    var postUrl;
    var MaxPostUrl;
    //var bIsTranslate;
    debugger;
    var gridData = $("#GridWorkObject").data("kendoGrid").dataSource.data();
    var token = $("#formWorkMaster input").val();
    var objBEWorkObjectChoice = [];
    for (var i = 0; i < gridData.length; i++) {
        //debugger;
        var bVisible = $("#GridWorkObject tbody").find("tr").eq(i).find("td").eq(12).find("input").is(":checked");
        var bSearch = $("#GridWorkObject tbody").find("tr").eq(i).find("td").eq(13).find("input").is(":checked");
        var bEditable = $("#GridWorkObject tbody").find("tr").eq(i).find("td").eq(14).find("input").is(":checked");
        var bRequired = $("#GridWorkObject tbody").find("tr").eq(i).find("td").eq(15).find("input").is(":checked");
        var bDisabled = $("#GridWorkObject tbody").find("tr").eq(i).find("td").eq(16).find("input").is(":checked");
        //var bUniqueID = $("#GridWorkObject tbody").find("tr").eq(i).find("td").eq(17).find("input").is(":checked");
        //var bTransactionType = $("#GridWorkObject tbody").find("tr").eq(i).find("td").eq(18).find("input").is(":checked");
        //var bLANID = $("#GridWorkObject tbody").find("tr").eq(i).find("td").eq(19).find("input").is(":checked");
        //var bIsUpload = $("#GridWorkObject tbody").find("tr").eq(i).find("td").eq(20).find("input").is(":checked");
        var bIsReport = $("#GridWorkObject tbody").find("tr").eq(i).find("td").eq(17).find("input").is(":checked");
        //var bCustomerIdentifier = $("#GridWorkObject tbody").find("tr").eq(i).find("td").eq(22).find("input").is(":checked");
        //if (ClientLanguagebol.toUpperCase() == "TRUE") {
        //    bIsTranslate = $("#GridWorkObject tbody").find("tr").eq(i).find("td").eq(31).find("input").is(":checked");

        //    // _oTranslateList.push(ConvertTextValues(gridData[i].sObjectName));
        //}

        var GridList = {
            iObjectID: gridData[i].iObjectID,
            iStoreID: gridData[i].iStoreID,
            sObjectName: gridData[i].sObjectName,
            sObjectDescription: gridData[i].sObjectDescription,
            sObjectLabel: gridData[i].sObjectLabel,
            iObjectType: gridData[i].iObjectType,
            sDataType: gridData[i].sDataType == null ? "Integer" : gridData[i].sDataType,
            iLength: gridData[i].iLength,
            iValidationID: gridData[i].iValidationID,
            bVisible: bVisible,
            bSearch: bSearch,
            bEditable: bEditable,
            bRequired: bRequired,
            bDisabled: bDisabled,
            UID: gridData[i].uid,
            iGrdColumeOrder: gridData[i].iIsReportOrder
        }
        if (typeof gridData[i].oChoice != 'undefined') {

            for (var j = 0; j < gridData[i].oChoice.length; j++) {

                var ChoiceData = {
                    iObjectChoiceID: gridData[i].oChoice[j].iObjectChoiceID,
                    sChoiceValue: gridData[i].oChoice[j].sChoiceValue,
                    iGroupID: gridData[i].oChoice[j].iGroupID,
                    iOrder: gridData[i].oChoice[j].iOrder,
                    bDisabled: gridData[i].oChoice[j].bDisabled,
                    iUid: gridData[i].uid,
                    iChoiceUid: gridData[i].oChoice[j].uid
                }
                objBEWorkObjectChoice.push(ChoiceData);

                //  objBEWorkObjectChoice.push(ChoiceData);
            }
        }

        objBEWorkObject.push(GridList);
    }

    var token = $('input[name=__RequestVerificationToken]').val();//added by indresh 22-08-2017

    //debugger;
    WorkDefinitionViewModel.GridObjectName = $("#Grdobjname")[0].innerText;
    WorkDefinitionViewModel.GridObjectNameID = $("#GrdobjnameID")[0].innerText;
    WorkDefinitionViewModel.DisableGridObject = $("#GrdobjnameDisable")[0].innerText == "" ? false : $("#GrdobjnameDisable")[0].innerText;
    WorkDefinitionViewModel.IsGrdEditable = $("#GrdobjEditable")[0].innerText == "" ? false : $("#GrdobjEditable")[0].innerText;
    WorkDefinitionViewModel.GridObjectRowNum = $("#GrdobjRowNum")[0].innerText;
    WorkDefinitionViewModel.gClientName = $("#ClientName").val();
    WorkDefinitionViewModel.gProcessName = $("#ProcessName").val();
    WorkDefinitionViewModel.gCampaignName = $("#CampaignName").val();


    WorkDefinitionViewModel.Grid_LstValues = objBEWorkObject;
    // WorkDefinitionViewModel.WorkDefinition.oChoice = objBEWorkObjectChoice;

    postUrl = urlPathSaveGridControlValues;
    postChoiceUrl = urlPathTempGRDChoiceDataSave;
    $.LoadingOverlay("show");
    //async: false,
    $.ajax({
        url: postChoiceUrl,
        async: false,
        type: 'POST',
        dataType: 'json',
        data: { 'objchoiceData': objBEWorkObjectChoice },


        success: function (result) {
            $.LoadingOverlay("hide");
        },
        error: function (err) {
            //  kendo.ui.progress($('#formWorkMaster'), false);
            $.LoadingOverlay("hide");
        }
    });
    try {
        $.LoadingOverlay("show");
        $.ajax({
            url: postUrl,
            async: false,
            type: 'POST',
            dataType: 'json',
            data: { 'GRDWorkObject': WorkDefinitionViewModel },

            success: function (result) {
                $.LoadingOverlay("hide");
                var dataSource = new kendo.data.DataSource({ data: result.response });
                var grid = $('#GridWorkObject').data("kendoGrid");
                dataSource.read();
                grid.setDataSource(dataSource);
            },
            error: function (err) {
                $.LoadingOverlay("hide");
                jAlert(err.text);
                // kendo.ui.progress($('#formWorkMaster'), false);
                //
            }
        });
    }
    catch (err1) {
        $.LoadingOverlay("hide");
        jAlert(err1.message);
    }


}

// Add New Row Grid Control Data 
function GRD_OnClickAddNewRow() {

    var errors = "";

    var GridObjectName = $("#Grdobjname")[0].innerText;
    if (GridObjectName == "") {

        jAlert("Please Select Grid Object Control ! ", display_Alert);
        return false;
    }
    var gridData = $("#GridWorkObject").data("kendoGrid").dataSource.data();

    for (var i = 0; i < gridData.length; i++) {

        if (gridData[i].sObjectName == null || gridData[i].sObjectName == "") {
            errors += ObjectName + ", "
        }
        if (gridData[i].iObjectType == null || gridData[i].iObjectType == "") {
            errors += ObjectType + ", "
        }
        switch (gridData[i].selectControlType.sControlType) {

            case 'DropDownList-MultiSelect':
                // code block
                break;
            case 'DropDownList':
                // code block
                break;
            case 'CheckBoxList':
                // code block
                break;
            case 'RadioButtonList':
                // code block
                break;
            default:
                if (gridData[i].sDataType == null || gridData[i].sDataType == "") {
                    errors += 'Object ' + Datatype
                }
            // code block
        }

        //if (gridData[i].selectControlType.sControlType != 'DropDownList' || gridData[i].selectControlType.sControlType != 'CheckBoxList') {
        //    if (gridData[i].sDataType == null || gridData[i].sDataType == "") {
        //        errors += 'Object ' + Datatype
        //    }
        //}
    }

    if (errors != "") {
        errors = msgBlankRow + errors;
        jAlert(errors, display_Alert);
        return false;
    }
    else {
        //debugger;
        var gr = $('#GridWorkObject').data('kendoGrid');
        max = $('#GridWorkObject table tbody tr:last td:first').text();
        max = parseInt(max) + 1;
        var datasource = gr.dataSource;
        var newRecord = {
            ISExistingRow: 'NO',
            iColSpan: '', bWorkID: '', sObjectName: '', sObjectDescription: '', sObjectLabel: '',
            selectControlType: { sControlType: display_Select, iControlTypeID: 0 },
            iLength: '0',
            selectedDataType: { Text: display_Select, Value: 0 },
            selectedValidation: { ValidationType: display_Select, ValidationId: 0 },
            selectedRow: { Text: display_Select, Value: 0 },
            selectedcolumn: { Text: display_Select, Value: 0 },
            selectedcolumnSpan: { Text: display_Select, Value: 0 },
            bVisible: false,
            bSearch: false,
            bEditable: false,
            bRequired: false,
            bDisabled: false,
            bUniqueID: false,
            bTransactionType: false,
            bLANID: false,
            bIsUpload: false,
            bIsReport: false,
            bCustomerIdentifier: false,
            iIsReportOrder: '0',
            iLengthReadonly: '0',
            bIsTranslate: false

        };
        /*Inserting new row*/
        var idx = $('#GridWorkObject table tbody tr').length;
        datasource.insert(0, newRecord);

        EnableDisableField();
        $("#iColSpan").val(max);
    };
}


// Save Control Data 
function GRD_OnClickSaveControlData() {
    //debugger;
    var errors = "";
    var gridData = $("#GridWorkObject").data("kendoGrid").dataSource.data();

    for (var i = 0; i < gridData.length; i++) {

        if (gridData[i].sObjectName == null || gridData[i].sObjectName == "") {
            errors += ObjectName + ", "
        }
        if (gridData[i].iObjectType == null || gridData[i].iObjectType == "") {
            errors += ObjectType + ", "
        }
        switch (gridData[i].selectControlType.sControlType) {

            case 'DropDownList-MultiSelect':
                // code block
                break;
            case 'DropDownList':
                // code block
                break;
            case 'CheckBoxList':
                // code block
                break;
            case 'RadioButtonList':
                // code block
                break;
            default:
                if (gridData[i].sDataType == null || gridData[i].sDataType == "") {
                    errors += 'Object ' + Datatype
                }
            // code block
        }

        //if (gridData[i].selectControlType.sControlType != 'DropDownList' || gridData[i].selectControlType.sControlType != 'CheckBoxList') {
        //    if (gridData[i].sDataType == null || gridData[i].sDataType == "") {
        //        errors += 'Object ' + Datatype
        //    }
        //}
    }

    if (errors != "") {
        errors = msgBlankRow + errors;
        jAlert(errors, display_Alert);
        return false;
    }
    else {
        GRDSaveWorkData();

    }
}

function GRD_OnClickResetControlData() {

    $("#Grdobjname").html('');

    //  GRDSaveWorkData();
    $.ajax({
        url: urlPathGetGridRefresh,
        type: 'POST',
        dataType: 'json',
        data: { GridID: "0", AFlag: "REF" },
        success: function (result) {

            // $('#GridWorkObject').data('kendoGrid').refresh();
            var dataSource = new kendo.data.DataSource({ data: result.lstWorkDefinitionGRD });
            var grid = $('#GridWorkObject').data("kendoGrid");
            dataSource.read();
            grid.setDataSource(dataSource);

        },
        error: function (err) {
            kendo.ui.progress($('#formWorkMaster'), false);
            //
        }
    });


}
// Grid Button set Visiblity true/false
function WorkDefinition_GridConfig_ShowHide(chkbx) {
    var errors = "";
    if ($("#ClientName").val() == "") {
        errors += required_Client + '\n';
    }
    if ($("#ProcessName").val() == "") {
        errors += required_Process + '\n';
    }
    if ($("#CampaignName").val() == "") {
        errors += MsgCampaign + '\n';
    }

    if (errors != "") {
        errors = display_msg_following + '\n' + errors;
        jAlert(errors);
        return false;
    }
    var iCampaignID = $("#CampaignName").val();
    var iClientID = $("#ClientName").val();
    var iProcessID = $("#ProcessName").val();
    var ccp = iCampaignID + "/" + iClientID + "/" + iProcessID;
    var grdView = $('#GridWorkObject').data('kendoGrid');
    if (chkbx.checked) {
        $.LoadingOverlay("show");
        $.ajax({
            url: urlPath_CheckGridConfigurations,
            type: 'POST',
            dataType: 'json',
            data: { ClientProcesscampId: ccp },
            success: function (result) {
                $.LoadingOverlay("hide");

                if (result == "Empty") {
                    jAlert("Please do the Grid Configuration (Click Grid Configuration Button ).", 'Alert', function () {
                        $("#IsGridConfiguration").prop("checked", false);
                        grdView.hideColumn("selectedGridControlObj");

                    });
                }
                else {
                    $("#DropFlag").val("GridControl");
                    grdView.showColumn("selectedGridControlObj");
                }

            },
            error: function (err) {
                $.LoadingOverlay("hide");
                //kendo.ui.progress($('#formWorkMaster'), false);
                //
            }
        });
    }
    else {
        $("#DropFlag").val("NoGridControl");
        grdView.hideColumn("selectedGridControlObj");
    }
    //var buttonObject = $("#btnGrdConfig").data("kendoButton");
    // if (chkbx.checked) {
    //   buttonObject.enable(true);
    //}
    //else {
    //buttonObject.enable(false); 
    //}
}
function GRD_OnClickAddNewRow1() {

    var errors = "";
    //var errors = "";
    if ($("#ClientName").val() == "") {
        errors += required_Client + '\n';
    }
    if ($("#ProcessName").val() == "") {
        errors += required_Process + '\n';
    }
    if ($("#CampaignName").val() == "") {
        errors += MsgCampaign + '\n';
    }
    if (errors != "") {
        errors = display_msg_following + '\n' + errors;
        jAlert(errors);
        return false;
    }
    var gridData = $("#GridObjectName").data("kendoGrid").dataSource.data();

    for (var i = 0; i < gridData.length; i++) {

        var bGrdEditable = $("#GridObjectName tbody").find("tr").eq(i).find("td").eq(3).find("input").is(":checked");
        var bDisabled = $("#GridObjectName tbody").find("tr").eq(i).find("td").eq(4).find("input").is(":checked");
        gridData[i].bGrdEditable = bGrdEditable;
        gridData[i].bDisabled = bDisabled;
        if (gridData[i].sChoiceValue == null || gridData[i].sChoiceValue == "") {
            errors += ObjectName + ", "
        }

    }

    if (errors != "") {
        errors = msgBlankRow + errors;
        jAlert(errors, display_Alert);
        return false;
    }
    else {
        //debugger;
        var gr1 = $('#GridObjectName').data('kendoGrid');
        var max1 = $('#GridObjectName table tbody tr:last td:first').text();
        max1 = parseInt(max1) + 1;
        var datasource = gr1.dataSource;
        var newRecord = {
            iObjectChoiceID: 0,
            sChoiceValue: '',
            iObjID: iObjTempID++,
            bGrdEditable: false,
            bDisabled: false
        };
        /*Inserting new row*/

        datasource.insert(0, newRecord);
        $("#iColSpan").val(max1);

    };
}
/*Get ControlType*/
function WorkDefinition_GetControlType() {
    return { DllFlag: $("#DropFlag").val() }
};
function DisableEnableGridFields() {
    debugger;
    $("#GridWorkObject table tbody tr td:nth-child(8)").each(function () {
        var dataType = $(this)[0].innerText;

        if (dataType != 'Grid Control') {
            //setTimeout(function () {
            //    $(this).next().children(":first").removeAttr("disabled");
            //    $(this).nextAll().eq(1).prop("disabled", "disabled");
            //      $(this).nextAll().eq(1).text("Integer");
            //    $(this).nextAll().eq(2).prop("disabled", "disabled");
            //    $(this).nextAll().eq(2).text("0");
            //}, 1000);

            $(this).next().children(":first").removeAttr("disabled");
            $(this).nextAll().eq(1).prop("disabled", "disabled");
            $(this).nextAll().eq(1).text("Integer");
            $(this).nextAll().eq(2).prop("disabled", "disabled");
            $(this).nextAll().eq(2).text("0");
        }
        else {
            $(this).next().children(":first").prop("disabled", "disabled");
            $(this).nextAll().eq(1).removeAttr("disabled");
            $(this).nextAll().eq(2).removeAttr("disabled");
        }
    });
}


function WorkDefinition_ValidationBeforeSave() {
    debugger;
    var errors = "OK";
    var gridData = $("#GridWorkObject").data("kendoGrid").dataSource.data();
    var GridObjectlabel = [];
    for (var i = 0; i < gridData.length; i++) {

        if (GridObjectlabel.indexOf(gridData[i].sObjectLabel.trim()) > -1) {
            errors = "Object label cannot be duplicate " + gridData[i].sObjectLabel;


        } else {
            GridObjectlabel.push(gridData[i].sObjectLabel.trim());
        }
    }

    return errors;

}