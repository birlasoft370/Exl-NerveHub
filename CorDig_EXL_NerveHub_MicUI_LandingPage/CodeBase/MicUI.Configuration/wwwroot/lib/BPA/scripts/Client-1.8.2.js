function OnClientClickNew() { window.location.href = ResourceClientIndex.urlPath_Index; }
function OnClientClickView() { window.location.href = ResourceClientIndex.urlPath_ShowClient; }
function OnClickNew() { window.location.href = ResourceClientSearch.urlPath_Index; }
var CheckedSBHU = [];
function DoTheCheck(aspCheckBoxID, oSrc) {

    re = new RegExp('' + aspCheckBoxID + '')  //generated control	name starts with a colon
    for (i = 0; i < document.forms[0].elements.length; i++) {

        elm = document.forms[0].elements[i]
        if (elm.type == 'checkbox') {
            if (re.test(elm.id)) {
                if (oSrc.checked) {
                    elm.checked = true;
                }
                else {
                    elm.checked = false;
                }
            }
        }
    }
}


///


function CheckGridValues() {
    if ($("#iClientID").val() != 0) {

        //var token = $("#frmClientIndex input").val();
        var form = $('#frmClientIndex');
        var token = $('input[name="__RequestVerificationToken"]', form).val();
        $.ajax({
            async: false,
            type: 'Post'
            , url: ResourceClientIndex.urlPath_GetErpSbuListById
            , data: { __RequestVerificationToken: token, ClientID: $("#iClientID").val() }
            , dataType: 'json'
            , success: function (result) {

                var DataClientSBU = $("#gridClientSBU").data("kendoGrid").dataSource.view();
                // var DataClientERP = $("#gridClientERP").data("kendoGrid").dataSource.view();
                var count = 0;
                var countERP = 0;
                $("#gridClientSBU table tbody tr").each(function (e) {

                    for (var j = 0; j < result.lSBUInfo.length; j++) {

                        if (DataClientSBU[count].iSBUID == result.lSBUInfo[j].iSBUID) {
                            $(this).find(".chkSBU").prop('checked', 'checked');
                        }
                    }
                    count++;
                });


            }

        });


    }
}


// Rstrict Special Characters in Client Text Box



// Rstrict Special Characters in Client Text Box at time of search
(function () {

    $('#SEARCHNAME').on('keypress', function (e) {
        if ($('#ClientName').val() == "" && (e.which == 123 || e.which == 125 || e.which == 91 || e.which == 92 || e.which == 124 || e.which == 93 || e.which == 63 || e.which == 32 || e.which == 37 || e.which == 126 || e.which == 96 || e.which == 33 || e.which == 64 || e.which == 35 || e.which == 36 || e.which == 37 || e.which == 94 || e.which == 38 || e.which == 42 || e.which == 40 || e.which == 41 || e.which == 45 || e.which == 95 || e.which == 61 || e.which == 43 || e.which == 42 || e.which == 47 || e.which == 46 || e.which == 44 || e.which == 60 || e.which == 62 || e.which == 59 || e.which == 58 || e.which == 34 || e.which == 39)) {
            return false;
        }

        if ($('#ClientName').val() != "" && (e.which == 123 || e.which == 125 || e.which == 92 || e.which == 93 || e.which == 91 || e.which == 124 || e.which == 63 || e.which == 37 || e.which == 126 || e.which == 96 || e.which == 33 || e.which == 64 || e.which == 35 || e.which == 36 || e.which == 37 || e.which == 94 || e.which == 38 || e.which == 42 || e.which == 40 || e.which == 41 || e.which == 45 || e.which == 95 || e.which == 61 || e.which == 43 || e.which == 42 || e.which == 47 || e.which == 46 || e.which == 44 || e.which == 60 || e.which == 62 || e.which == 59 || e.which == 58 || e.which == 34 || e.which == 39)) {
            return false;
        }
    });
});


function checkAllSBU(ele) {
    var state = $(ele).is(':checked');
    var grid = $('#gridClientSBU').data('kendoGrid');
    $('.chkSBU').prop('checked', state);
}



function editClient(e) {  // Function for Editing Client 

    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var value = dataItem.iClientID
    //var token = $("#__AjaxAntiForgeryForm input").val();
    var form = $('#__AjaxAntiForgeryForm');
    var token = $('input[name="__RequestVerificationToken"]', form).val();
    $.LoadingOverlay("show");

    $.ajax({
        url: ResourceClientSearch.urlPath_SetClientID,
        type: 'POST',
        dataType: 'json',
        data: {
            __RequestVerificationToken: token,
            id: dataItem.iClientID
        },
        error: function (jqXHR, textStatus, errorThrown) {
            jAlert(errorThrown);
        },
        success: function (data) {
            if (data == 1) {
                window.location.href = ResourceClientSearch.urlPath_Index;
            }
        }
    });
}

function deleteClient(e) { // Function for Deleting Client 

    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var index = $(e.currentTarget).closest("tr")[0].rowIndex;
    jConfirm(ResourceClientSearch.display_Comfirm_Delete, 'Confirmation', function (r) {
        if (r) {
            var token = $("#__AjaxAntiForgeryForm input").val();
            var dataObject = {
                __RequestVerificationToken: token,
                id: dataItem.iClientID
            };

            $.ajax({
                url: ResourceClientSearch.urlPath_SetClientID,
                type: 'POST',
                dataType: 'json',
                data: {
                    __RequestVerificationToken: token,
                    id: dataItem.iClientID
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    jAlert(errorThrown);
                },
                success: function (data) {
                    if (data == 1) {
                        $.ajax({
                            url: ResourceClientSearch.urlPath_DeleteClient,
                            type: 'POST',
                            dataType: 'json',
                            data: {
                                __RequestVerificationToken: token
                            },

                            success: function (result) {
                                if (result == ResourceClientSearch.display_OK) {

                                    $("#ClientGrid  table > tbody tr:eq(" + (index - 1) + ")").remove();
                                    jAlert(ResourceClientSearch.display_deleted);
                                }

                            }
                            , error: function (jqXHR, textStatus, errorThrown) {

                                jAlert(errorThrown);
                            }
                        });
                    }

                }
            });
        }
        else {
            return false;
        }
    });
}


function GoClient() { // Function for Submitting form
    $("#formClient").submit();
}


function checkAllSBU(ele) {

    var state = $(ele).is(':checked');
    $('.chkSBU').prop('checked', state);
}


function checkAllERPClient(ele) {
    var state = $(ele).is(':checked');
    $('.chkERP').prop('checked', state);
}


function btnSaveOnClickCleint(e) {

    var validator = $("form").data("kendoValidator");
    //if (validator.validate()) {
    if (GetFormValidate($('form'), "btnSaveClient")) {
        var CampaignViewModel = {};
        var chkSelectedArr = [];
        var chkSelectedERPArr = [];
        var chkClientid = "";
        var chkSelected = ""
        //var countERP = 0;
        var count = 0;
        var Chkcount = 0;
        CampaignViewModel.iClientID = $("#iClientID").val();
        CampaignViewModel.ClientName = $("#ClientName").val();
        CampaignViewModel.VerticalID = $("#VerticalID").val();
        CampaignViewModel.Description = $("#Description").val();
        CampaignViewModel.EXLSpecificClient = $('#EXLSpecificClient').is(':checked') == true ? true : false;
        CampaignViewModel.Disabled = $('#Disabled').is(':checked') == true ? true : false;
        var DataClientSBU = $("#gridClientSBU").data("kendoGrid").dataSource.view();

        $("#gridClientSBU table tbody tr").each(function () {

            if ($(this).find(".chkSBU").is(":checked")) {
                chkSelectedArr.push(DataClientSBU[count].iSBUID);
                Chkcount++;
            }
            count = count + 1;
        });

        chkSelected = chkSelectedArr.join(',');

        if (Chkcount == 0) {
            jAlert(ResourceClientIndex.displayReguiredSBU);
            return false;
        }

        //var token = $("#frmClientIndex input").val();
        var form = $('#frmClientIndex');
        var token = $('input[name="__RequestVerificationToken"]', form).val();
        $.LoadingOverlay("show");
        $.ajax({
            url: ResourceClientIndex.urlPath_Index,
            type: 'POST',
            dataType: 'json',
            data: {
                __RequestVerificationToken: token,
                ObjClient: CampaignViewModel,
                chkSelected: chkSelectedArr
            },
            success: function (data) {
                $.LoadingOverlay("hide");
                if (data == "save") {

                    jAlert(ResourceClientIndex.display_Save, "Alert", function (r) {
                        window.location.href = ResourceClientIndex.urlPath_Index;

                    });
                }
                else if (data == "update") {

                    jAlert(ResourceClientIndex.display_Updated, "Alert", function (r) {
                        window.location.href = ResourceClientIndex.urlPath_Index;
                    });
                }
                else {
                    jAlert(data);
                    return;
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $.LoadingOverlay("hide");
                jAlert(errorThrown);
            }
        });
    }
    else {
        $('form').submit();
    }
}
