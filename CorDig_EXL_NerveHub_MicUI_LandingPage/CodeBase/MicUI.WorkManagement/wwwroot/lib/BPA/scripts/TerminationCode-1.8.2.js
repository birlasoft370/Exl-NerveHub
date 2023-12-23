/*
 Programm Name : TerminationCode-1.8.2
 Created by    : Nabin Kumar
 Create Date   : 12/04/2014
 Modifued Date :
 Modified By   : 
 Description   : To manage Termination Code

*/


// Functions to Navigation
function OnTerminationCodeViewClick() {
    window.location.href = Resources.url_ShowTerminationCode;
}

function OnTerminationCodeClickNew() {
    window.location.href = Resources.url_Index;
}
//Function to  Edit Termination Code
function editTermCode(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    $.ajax({
        type: 'Post',
        url: Resources.url_SetTerminationCodeID,
        data: { id: dataItem.iTerminationCodeID },
        dataType: 'json',
        success: function (result) {
            if (result == "1") {
                window.location.href = Resources.url_Index;
            }
        }
    });
}
// Function To delete Termination code
function deleteTermCode(e) {

    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var index = $(e.currentTarget).closest("tr")[0].rowIndex;
    var token = $("#formTerminationCode input").val();
    jConfirm(Confirm_Delete, 'Confirmation', function (r) {
        if (r) {
            e.preventDefault();
            $.ajax({
                type: "POST",
                url: Resources.url_DeleteTerminationCode,
                data: {
                    __RequestVerificationToken: token,
                    id: JSON.stringify(dataItem.iTerminationCodeID)
                },
                dataType: 'json',
                success: function (data) {
                    if (data == OK) {
                        $("#searchGrid  table > tbody tr:eq(" + index + ")").remove();
                        jAlert(display_Delete_Message)
                    }
                }
            });
        }
        else {
            return false;
        }
    })
}

function GoTerminationCode() {

    $("#formTerminationCode").submit();
}