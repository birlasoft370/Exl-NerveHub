


function OnMasterValueClickView(e) {

    window.location.href = ResourceMasterValueIndex.urlPath_MasterValueSearchView
};


function OnMasterValueClickNew(e) {
    window.location.href = ResourceMasterValueIndex.urlPath_Index;
};

function OnClickNewMasterValue(e) {
    window.location.href = ResourceMasterValueSearch.urlPath_Index;
}


function GoMasterValue(e) {
    $("#formMasterValue").submit();
};



//function Go() {
//    var form = $('#form1');
//    form.data('validator').settings.ignore = '';
//    $("#form1").submit();
//}

function editMasterValue(e) {

    e.preventDefault();
    var token = $("#formMasterValue input").val();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var value = dataItem.MasterValueID
    $.ajax({
        type: 'Post'
        , url: ResourceMasterValueSearch.urlPath_SetMasterValueID
        , data: { __RequestVerificationToken: token, id: dataItem.MasterValueID }
        , dataType: 'json'
        , success: function (result) {
            if (result == "1") {
                window.location.href = ResourceMasterValueSearch.urlPath_Index;
            }
        }
    });

}

function deleteMasterValue(e) {
    e.preventDefault();
    var token = $("#formMasterValue input").val();
    var index = $(e.currentTarget).closest("tr")[0].rowIndex;
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    jConfirm(ResourceMasterValueSearch.display_DeleteConfirmation, 'Confirmation', function (r) {
        if (r) {

            $.ajax({
                type: 'Post'
                , url: ResourceMasterValueSearch.urlPath_SetMasterValueID
                , data: { __RequestVerificationToken: token, id: dataItem.MasterValueID }
                , dataType: 'json'
                , success: function (result) {
                    if (result == "1") {
                        $.ajax({
                            type: "POST"
                            , url: ResourceMasterValueSearch.urlPath_EditingCustom_Destroy
                            , data: { __RequestVerificationToken: token, sMasterValueID: dataItem.MasterValueID }
                            , success: function (result) {

                                if (result == "OK") {
                                    $("#searchGrid table > tbody tr:eq(" + index + ")").remove();
                                    jAlert("Master Value  Deleted successfully!");
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









/*------------Index Page--------------*/


$("#btnsave").on("click", function (e) {

    var grid = $("#gvMasterValue").data("kendoGrid"),
        parameterMap = grid.dataSource.transport.parameterMap;
    //get the new and the updated records
    var currentData = grid.dataSource.data();
    for (var i = 0; i < currentData.length; i++) {
        currentData[i].dirty = true;
    }
    $("#gvMasterValue").data("kendoGrid").saveChanges();
});
var max = 0
$('#gvAddrow').on("click", function (e) {

    var gr = $('#gvMasterValue').data('kendoGrid');
    //max = $('#gvMasterValue table tbody tr:last td:first').text();
    //max = parseInt(max) + 1
    gr.addRow();
})

function _delete(e) {

    var index = $(e.currentTarget).closest("tr")[0].rowIndex - 1;
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    e.preventDefault();
    $("#gvMasterValue  table > tbody tr:eq(" + index + ")").hide();
};


function OnMasterValueClickSave(e) {

    $("#MasterType").val($("#MasterType").val().trim());
    var token = $("#formMaster input").val();
    if (GetFormValidate($('form'), "btnSaveMasterValue")) {
        var Result = "YES";
        var data = $("#gvMasterValue").data("kendoGrid").dataSource.view();
        if (data.length <= 0) {
            jAlert("Please Add New Row !");
            return;
        }
        else {
            for (var i = 0; i < data.length; i++) {
                if (data[i].Values == '') {
                    // jAlert("Please Add New Row !");
                    // return;
                    Result = "NO";
                }

            }
        }

        if (Result != 'NO') {
            var MasterValueID = $("#MasterValueID").val();
            var MasterType = $("#MasterType").val();
            var Disable = $("#bDisable").is(':checked') ? 1 : 0;
            if (MasterValueID == undefined) {
                MasterValueID = 0;
            }
            var strParameter = MasterValueID + ";" + MasterType + ";" + Disable;

            var data = $("#gvMasterValue").data("kendoGrid").dataSource.view();
            if (data.length <= 0) {
                jAlert("Please Add New Row !");
                return;
            }

            var MasterValue = JSON.stringify(data);
            var grid = $("#gvMasterValue").data("kendoGrid");


            for (var i = 0; i < data.length; i++) {
                var FieldID = FieldID + ";" + data[i].FieldID;
                var Values = Values + ";" + data[i].Values;
                var Disable = Disable + ";" + data[i].Disable;
            }

            $.ajax({
                url: ResourceMasterValueIndex.urlPath_InsertData,
                type: 'Post',
                data: { __RequestVerificationToken: token, strParameter: strParameter, FieldID: FieldID, Values: Values, Disable: Disable },
                cache: false,
                success: function (result) {

                    if (result.success == true) {
                        if (result.strResult == "Updated") {
                            $("#MasterType").val("");
                            $("#bDisable").prop("checked", false);
                            $("#gvMasterValue").data("kendoGrid").dataSource.read();
                            $('#gvMasterValue').data('kendoGrid').refresh();
                            //location.reload(true);
                            jAlert(ResourceMasterValueIndex.disp_UpdatedMasterValue);

                            window.location.href = ResourceMasterValueIndex.urlPath_Index;
                        }
                        else if (result.strResult == "Saved") {
                            $("#MasterType").val("");
                            $("#bDisable").prop("checked", false);
                            $("#gvMasterValue").data("kendoGrid").dataSource.read();
                            $('#gvMasterValue').data('kendoGrid').refresh();
                            //location.reload(true);
                            jAlert(ResourceMasterValueIndex.disp_SaveMasterValue);
                            window.location.href = ResourceMasterValueIndex.urlPath_Index;
                        }
                        //else if(result=="0")
                        //{

                        //    jAlert(ResourceMasterValueIndex.MasterValueExist);
                        //    return;
                        //}
                        else {
                            jAlert(result.strResult, 'Alert', function () {
                                window.location.href = ResourceMasterValueIndex.urlPath_Index;
                            });
                        }
                    }
                    else {
                        jAlert(result.message);
                    }
                }
            });
        }
    }
    else {
        $("form").submit();
        return;
    }
}

function onRowBoundsearchGrid(e) {

    $(".k-grid-Edit").find("span").addClass("k-icon k-edit");
    $(".k-grid-Delete").find("span").addClass("k-icon k-delete");
}



