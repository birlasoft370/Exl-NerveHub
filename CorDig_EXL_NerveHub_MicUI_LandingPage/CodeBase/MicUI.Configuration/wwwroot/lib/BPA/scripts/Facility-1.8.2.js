/* Index Page */
function FillGrid() {
   
    $.ajax({
        type: 'POST',
        url: ResourceFacilityIndex.urlpath_FillGrid,// '@Url.Action("FillGrid")',
        dataType: 'json',
        data: { },
        success: function (result) {
           
            //jAlert(result);
            var grid = $('#gvTest').getKendoGrid();
            grid.dataSource.data(result);
            grid.refresh();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            jAlert(textStatus, errorThrown);
        }
    });
}
function FillGrid2() {
   
    $.ajax({
        type: 'POST',
        url: ResourceFacilityIndex.urlpath_FillGrid2,//'@Url.Action("FillGrid2")',
        dataType: 'json',
        data: {},
        success: function (result) {
           
             $('#gvTest2').html("")
          
            $("#gvTest2").kendoGrid({
                dataSource: { data: result, pageSize: 20, },
                sortable: true,
                pageable: true,
                width: 400,
                height: 500,
            });

            RegisterToTimeZone($('#gvTest2').data("kendoGrid"));


        },
        error: function (jqXHR, textStatus, errorThrown) {
            jAlert(textStatus, errorThrown);
        }
    });

}
/*Controle disabled for edit case*/
function OnFacilityReset() { window.location.href = ResourceFacilityIndex.urlPath_Index; }
function OnFacilityNew() { window.location.href = ResourceFacilitySearch.urlPath_Index; }
function OnFacilitySearch() {    window.location.href = ResourceFacilityIndex.urlPath_FacilitySearchView;}


/* Search Page */
//function OnFacilityNew() { window.location.href = "/AppConfiguration/Facility/Index"; }

function PostForm() {
    var form = $('#form1');
    form.data('validator').settings.ignore = '';
    $("#form1").submit();
}
function FillSearch(e) {
    // 
    //;
    e.preventDefault();
    var token = $("#formFacilitySearchView input").val();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    $.ajax({
        type: 'POST',
        url: ResourceFacilitySearch.urlPath_SetTempFacility, //'@Url.Action("SetTempFacility")',
        dataType: 'json',
        data: { __RequestVerificationToken: token, iFacilityID: dataItem.iFacilityID },
        success: function (result) {
            window.location.href = ResourceFacilitySearch.urlPath_Index
        },
        error: function (jqXHR, textStatus, errorThrown) {
            jAlert(textStatus, errorThrown);
        }
    }); 
   
   // window.location.href = "@Url.Action("FillSearch", "Facility")";
}
function DeleteFacility(e) { // Function for Deleting Client 
    //;
    e.preventDefault();
    var token = $("#formFacilitySearchView input").val();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var index = $(e.currentTarget).closest("tr")[0].rowIndex;
    jConfirm(ResourceFacilitySearch.confirmDelete, 'Confirmation', function (r) {
        //;
        if (r) {
            $.ajax({
                type: 'Post'
   , url: ResourceFacilitySearch.urlPath_SetTempFacility
   , data: { __RequestVerificationToken: token, iFacilityID: dataItem.iFacilityID }
   ,  dataType: 'json'
   , success: function (result) {
       ;
       if (result == "1") {
           $.ajax({
               type: "Post"
            , url: ResourceFacilitySearch.urlPath_DeleteFacility
            , data: { __RequestVerificationToken: token }
            , dataType: 'json'
            , success: function (result) {
                ;
                if (result == "OK") {
                    $("#searchGrid  table > tbody tr:eq(" + index + ")").remove();
                    jAlert(ResourceFacilitySearch.msgDeletedFacility);
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
