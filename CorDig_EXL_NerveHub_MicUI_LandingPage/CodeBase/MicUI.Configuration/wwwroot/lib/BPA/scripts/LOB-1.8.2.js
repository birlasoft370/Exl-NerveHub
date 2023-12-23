/*To pass parameter to read LOB List */
function LOBFilterGrid()
{ return { LOBname: $.trim($("#LOBSeachName").val()) } }
/* To delete the LOB */
function LOBdelete(e) {
    e.preventDefault();
    var index = $(e.currentTarget).closest("tr")[0].rowIndex;
    var token = $("#formLOB input").val();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    jConfirm(ResourceLOBSearchView.LOBDeleteConfirmation, 'Confirmation', function (r) {
        if (r) {
            ;
            $.ajax({
                type: "POST"
   , url: ResourceLOBSearchView.urlPath_EditingCustom_Edit
   , data: { __RequestVerificationToken: token, iLOBId: dataItem.iLOBID }
   , dataType: 'json'
   , success: function (result) {
       ;
       if (result == null) {
           $.ajax({
               type: "POST"
           , url: urlPath_Delete
           , data: { __RequestVerificationToken: token, sLOBID: dataItem.iLOBID }
           , dataType: 'json'
           , success: function (result) {
               ;
               if (result == "OK") {
                   ;
                   $("#gridViewLOB  table > tbody tr:eq(" + index + ")").remove();
                   
                   //SearchGridView();
                   jAlert(ResourceLOBSearchView.LOBDeleteMsg);
               }
           }
           });
           //window.location.href = "/AppConfiguration/LOB/Index";
       }
   }
  });
          
        }
        else {
            return false;
        }
    });
}
/* An extra request to store parameter in Temp */
function LOBKeepTemp(e) {
   
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var token = $("#formLOB input").val();
    $.ajax({
        type: "POST"
    , url: ResourceLOBSearchView.urlPath_EditingCustom_Edit
    , data: { __RequestVerificationToken: token, iLOBId: dataItem.iLOBID }
    , dataType: 'json'
    , success: function (result) {
        if (result == "OK") {
            window.location.href = ResourceLOBSearchView.urlPath_Index;
        }
    }
       , error: function (e) {
        jAlert("An error occured");
    }
    });
}

/* Search the LOB list based on parameter*/
function LOBSearch()
{
    $("#LOBgrid").show();
    SearchGridView();
}


function OnClickLOBRefresh()
{
    window.location.href = ResourceLOBIndex.urlPath_Index
}
function OnClickLOBView()
{
    window.location.href = ResourceLOBIndex.urlPath_LOBSearchView
}
function OnClickLOBNew()
{
    window.location.href = ResourceLOBSearchView.urlPath_Index
}