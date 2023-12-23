/* To clear the control data after save/update*/
function SkillMaster_Clear()
{
    $("#IsDisable").attr("checked", false);
    $("#SkillDescription").val('');
    $("#SkillName").val('');
}

/*--------Skill Master Search----------*/

/* To set skill Name as Parameter to fetch grid record */
function SkillMasterFilter() {
    return { skillName: $.trim($("#SkillName").val()) }
}
/*To Set the checkbox value inside grid*/
$(function () {
    $('#gridViewSkillMaster').on('click', '.chkbx', function () {
        var checked = $(this).is(':checked');
        var grid = $('#gridViewSkillMaster').data().kendoGrid;
        var dataItem = grid.dataItem($(this).closest('tr'));
        dataItem.IsDisable = checked;
    })
})

/*To update skill */
function SkillMasterUpdate(e) {

    $("#sSkillName").addClass("input-validation-error");
   // var token = $('input[name=__RequestVerificationToken]').val()
    var token = $("#formSkillMasterSearchView input").val();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    jConfirm(ResourceSkillSearch.skillMasterUpdateConfirmation, 'Confirmation', function (r) {
        if (r) {
   
            var SkillMasterViewModel = {}
            SkillMasterViewModel.IsDisable = dataItem.IsDisable;
            SkillMasterViewModel.SkillDescription = dataItem.SkillDescription;
            SkillMasterViewModel.SkillName = dataItem.SkillName;
            SkillMasterViewModel.SkillID = dataItem.SkillID;
           // SkillMasterViewModel = JSON.stringify({ 'skillMaster': SkillMasterViewModel });

            $.ajax({
                url: "/SkillMaster/Update",
                data: { __RequestVerificationToken: token, skillMaster: SkillMasterViewModel},
                type: "POST",
                datatype: "json",
               error: function () { 
                },
                success: function () {
                    jAlert(ResourceSkillSearch.skillMasterSaveMsg);
                    $("#gridSearch").data("kendoGrid").dataSource.read();
                }
            })
        }

    });
}
/* Search skill master*/
function SkillMasterSearch()
{
    
    $("#SkillMastergrid").show();
    var grid = $("#gridViewSkillMaster").data("kendoGrid")
    grid.dataSource._destroyed = [];
    grid.dataSource.read();
}

function onRowBoundSkilSearchView(e) {

    $(".k-grid-Update").find("span").addClass("k-icon k-update");

}
function OnClickRefresh()
{
    window.location.href = ResourceSkillMaster.urlPath_Index;
}
function OnClickSearchView()
{
    window.location.href = ResourceSkillMaster.urlPath_SearchView;
}
function OnClickNew()
{
    window.location.href = ResourceSkillSearch.urlPath_Index;
}
function SBUKeepTemp(e) {
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
   // var token= $('input[name=__RequestVerificationToken]').val()
    var token = $("#formSkillMasterSearchView input").val();
    $.ajax({
        type: "POST"
    , url: ResourceSkillSearch.urlPath_EditingCustom_Edit
    , data: { __RequestVerificationToken: token, iSkillID: dataItem.iSkillID }
    , dataType: 'json'
    , success: function (result) {

        if (result == "OK") {
            window.location.href = ResourceSkillSearch.urlPath_Index;
        }
    }
    });
}