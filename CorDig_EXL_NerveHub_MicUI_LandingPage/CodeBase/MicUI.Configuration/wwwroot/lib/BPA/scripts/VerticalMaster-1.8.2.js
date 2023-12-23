
/*-- Vertical Search View--*/

function OnClickAddVartical() { window.location.href = ResourceVirticalSearch.urlPath_Index; }

function Vartical_ClearData() { window.location.href = ResourceVirticalSearch.urlPath_VerticalSearchView; }

function Go() { var form = $('#formVerticalSearchView'); form.data('validator').settings.ignore = ''; $("#formVerticalSearchView").submit(); }

function editVerticalMaster(e) { e.preventDefault(); var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    window.location.href = ResourceVirticalSearch.urlPath_Index + "?iVerticalMasterID=" + dataItem.EncryptVerticaMasterID
}
function DeleteVerticalMaster(e) {
    jConfirm(ResourceVirticalSearch.display_Delete_Confirmation, 'Confirmation', function (r) {
        if (r) {
            e.preventDefault();
            var currentDataItem = $("#searchGrid").data("kendoGrid").dataItem($(e.currentTarget).closest("tr"));

            var token = $("#formVerticalSearchView input").val();
            $.ajax({
                type: "POST"
            , url: ResourceVirticalSearch.urlPath_DeleteVerticalMaster
            , data: { __RequestVerificationToken: token, sVerticalMasterID: currentDataItem.EncryptVerticaMasterID }
            , dataType: 'json'
            , success: function (result) {
                jAlert(ResourceVirticalSearch.display_deleteSuccessfully, "Alert", function () {
                    window.location.href = ResourceVirticalSearch.urlPath_VerticalSearchView;
                });
            }
                    , error: function(err)
                    {
                        jAlert(err);
                    }
            });
        }
        else {return false;}
    });
}
/*-- Vertical Index --*/


function OnClickVarticalMaster() {
    window.location.href = ResourceIndex.urlPath_Index;
}
function Vartical_ViewData() {
    window.location.href = ResourceIndex.urlPath_VerticalSearchView;
}