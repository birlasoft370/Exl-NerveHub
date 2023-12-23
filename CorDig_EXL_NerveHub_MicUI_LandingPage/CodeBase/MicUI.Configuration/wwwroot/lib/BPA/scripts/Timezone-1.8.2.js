


function GoTimeZone(e) {
    $("#formTimeZone").submit();
}

function OnClickNewTimeZone(e) { window.location.href = ResourceTimeZoneSearch.urlPath_Index; }


function onClickRefreshTimeZone(e) { window.location.href = ResourceTimeZoneIndex.urlPath_Index; }

function onClickSearchTimeZone(e) {
    window.location.href = ResourceTimeZoneIndex.urlPath_TimeZoneSearchView;
}


function editTimeZone(e) {  // Function for Editing TimeZone

    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var value = dataItem.iTimeZoneID;
    console.log(value);
    $.ajax({
        type: 'POST'
        , url: ResourceTimeZoneSearch.urlPath_SetTimeZoneID
        , data: { Timneid: value },//JSON.stringify(
        dataType: 'json'
        , success: function (result) {
            if (result == "1") {
                window.location.href = ResourceTimeZoneSearch.urlPath_Index;
            }
        }
    });
}

function deleteTimeZone(e) { // Function for Deleting TimeZone 

    var token = $("#formTimeZone input").val();
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var index = $(e.currentTarget).closest("tr")[0].rowIndex;
    jConfirm(display_Comfirm_Delete, 'Confirmation', function (r) {
        if (r) {

            $.ajax({
                type: 'Post'
                , url: ResourceTimeZoneSearch.urlPath_SetTimeZoneID
                , data: { __RequestVerificationToken: token, id: dataItem.iTimeZoneID }
                , dataType: 'json'
                , success: function (result) {
                    if (result == "1") {
                        $.ajax({
                            type: "Post"
                            , url: ResourceTimeZoneSearch.urlPath_Delete
                            , data: { __RequestVerificationToken: token }
                            , dataType: 'json'
                            , success: function (result) {

                                if (result == "") {
                                    $("#gvTimeZoneList  table > tbody tr:eq(" + index + ")").remove();
                                    jAlert(display_deleted);
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


function GoClient() { // Function for Submitting form
    $("#formTimeZone").submit();
}
