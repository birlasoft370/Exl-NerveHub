/* An extra request to store parameter in Temp */
function SBUKeepTemp(e) {
   
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var token = $("#formSBU input").val();
    var fgr = ResourceSearchSubProcess.urlPath_EditingCustom_Edit;
    $.ajax({
        type: "POST"
    , url: ResourceSearchSubProcess.urlPath_EditingCustom_Edit
    , data: { __RequestVerificationToken: token, iSBUID: dataItem.iSBUID }
    ,   dataType: 'json'
    , success: function (result) {
       
        if (result == "OK") {
            window.location.href = ResourceSearchSubProcess.urlPath_Index;
        }
    }
        , error: function (jqXHR, textStatus, errorThrown) {
           
            jAlert(errorThrown);
        }
    });
}
/*To Delete SBU */
function deleteSBU(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var index = $(e.currentTarget).closest("tr")[0].rowIndex;
    jConfirm(SBUDeleteConfirmation, 'Confirmation', function (r) {
        if (r) {
            
            var token = $("#formSBU input").val();
            $.ajax({
                type: "DELETE"
            , url: ResourceSearchSubProcess.urlPath_Delete
            , data: { __RequestVerificationToken: token, sSBUID: dataItem.iSBUID }
            ,  dataType: 'json'
            , success: function (result) {

                if (result == "OK") {
                    jAlert(SBUDeleteMsg);
                    $("#gridViewSBU  table > tbody tr:eq(" + index + ")").remove();

                }
            }
            });
        }
        else {
            return false;
        }
    });
}
/* To Filter the grid based on SBU name*/
function SUBFilterGrid() {
    return { sSBUName: $.trim($("#SBUSeachName").val()) }
}
/* To Search the data based on parameter */
function SBUSearch()
{
    $("#SBUgrid").show();
    SearchGridView();
}


function OnClickNewSubProcess()
{
    window.location.href = ResourceSubProcess.urlPath_Index;
}
function OnClickSearchSubProcess()
{
    window.location.href = ResourceSubProcess.urlPath_SearchView;

    //var token = $("#Sbuform input").val();
    //var token2 = $("#formSBU input").val();
    //alert(token);
    //alert(token2);

    //$.ajax({
    //    type: "POST"
    //        , url: ResourceSubProcess.urlPath_SearchView
    //        , data: { __RequestVerificationToken: token}

    //        , datatype: "json",
    //    success: function (data) {
           
    //    },
    //    error: function () {
    //        jAlert("An error occured");
    //    }
    //});


}
function OnClickSearchNew() {
    window.location.href = ResourceSearchSubProcess.urlPath_Index;
}
