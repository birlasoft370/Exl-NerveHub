$("#btnSave").click(function () {
    validationForm();
});

function OnBreakClickView() {
    window.location.href = Resources.url_BreakMasterSearchView;
}

function OnBreakNewClick() {
    window.location.href = Resources.url_Index;
}

function OnBreakMasterNew() {
    window.location.href = Resources.url_Index;
}

function editBreakCode(e) {

    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    $.ajax({
        type: 'POST',
        url: Resources.url_SetBreakID,
        data: { breakId: dataItem.iBreakID },
        dataType: 'json',
        success: function (result) {
            if (result == "1") {
                window.location.href = Resources.url_Index;
            }
        }
    });
}

function deleteBreakCode(e) {

    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var index = $(e.currentTarget).closest("tr")[0].rowIndex;
    var token = $("#formBreakMaster input").val();

    jConfirm(Resources.display_Comfirm_Delete_BreakCode, 'Confirmation', function (r) {
        if (r) {
            $.ajax({
                type: 'Post',
                url: Resources.url_SetBreakID,
                data: { __RequestVerificationToken: token, id: JSON.stringify(dataItem.iBreakID) },
                dataType: 'json',
                success: function (result) {
                    if (result == "1") {
                        $.ajax({
                            type: "Post",
                            url: Resources.url_DeleteBreakMaster,
                            data: { __RequestVerificationToken: token },
                            dataType: 'json',
                            success: function (result) {
                                if (result == "OK") {
                                    $("#BEBreakInfoList  table > tbody tr:eq(" + index + ")").remove();
                                    jAlert(Resources.display_Deleted_BreakCode);
                                }
                                else {
                                    jAlert(result);
                                }
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

function GoBreakMaster() {
    $("#formBreakMaster").submit();
}

function Datainfo() {

    var gt = $("#mBreakSearchName").val();
    return {
        mBreakName: $("#mBreakSearchName").val()
    };
}

function GoBreakMasterView() {
    $("#BreakMastergrid").show();
    SearchGridView();
}