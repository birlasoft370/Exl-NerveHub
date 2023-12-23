function btnNewLocationClick() {
    window.location.href = ResourceLocationIndex.urlPath_Index;
};
function OnClickViewLocation() {

    window.location.href = ResourceLocationIndex.urlPath_LocationSearchView;

};


function btnNewLocation() {
    window.location.href = ResourceLocationSearch.urlPath_Index;
};

function LocationSearch() {
    $("#GrdLocation").data("kendoGrid").dataSource.read();
    $("#GrdLocation").data("kendoGrid").refresh();
}
function GridFillterValue() {
    return {
        LocationName: $('#SearchLocationName').val()

    };
}

function editLocation(e) {

    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var token = $("#formLocationSearchView input").val();
    $.ajax({
        type: "POST"
        , url: ResourceLocationSearch.urlPath_editLocation
        , data: { __RequestVerificationToken: token, iLocationID: dataItem.iLocationID, LocationName: dataItem.sLocationName, LocationDescription: dataItem.sLocationDescription, Disabled: dataItem.bDisabled }
        , dataType: 'json'
        , success: function (result) {
            if (result == ResourceLocationSearch.display_Ok) {
                window.location.href = ResourceLocationSearch.urlPath_Index;
            }
        }
    });


}

function deleteLocation(e) { // Function for Deleting Location

    e.preventDefault();
    var token = $("#formLocationSearchView input").val();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var index = $(e.currentTarget).closest("tr")[0].rowIndex;
    jConfirm(ResourceLocationSearch.display_DeleteConfirmation, 'Confirmation', function (r) {
        if (r) {

            $.ajax({
                type: 'POST'
   , url: ResourceLocationSearch.urlPath_editLocation
   , data: { __RequestVerificationToken: token, iLocationID: dataItem.iLocationID, LocationName: dataItem.sLocationName, LocationDescription: dataItem.sLocationDescription, Disabled: dataItem.bDisabled }
   , dataType: 'json'
   , success: function (result) {
       if (result == ResourceLocationSearch.display_Ok) {

           $.ajax({
               type: "Post"
            , url: ResourceLocationSearch.urlPath_DeleteLocation
            , data: { __RequestVerificationToken: token }
            , dataType: 'json'
            , success: function (result) {

                if (result == ResourceLocationSearch.display_Ok) {

                    $("#GrdLocation  table > tbody tr:eq(" + index + ")").remove();
                    jAlert(ResourceLocationSearch.display_deleteok);
                }
                else {
                    jAlert(result);

                }
            }
               , error: function error(err) {

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


function onRowBoundGrdLocation(e) {

    $(".k-grid-Edit").find("span").addClass("k-icon k-edit");
    $(".k-grid-Delete").find("span").addClass("k-icon k-delete");
}