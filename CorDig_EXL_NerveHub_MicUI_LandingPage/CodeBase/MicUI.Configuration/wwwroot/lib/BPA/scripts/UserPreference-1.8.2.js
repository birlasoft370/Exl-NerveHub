
$("#btnCancelUserPreference").on("click", function () {
    window.location.href = urlPathIndex;
});

function OnSuccessSaveUserPreference(ret) {
    jAlert("User Preference saved successfully!", "alert");
}

$("#btnSaveUserPreference").on("click", function () {
    if (GetFormValidate($('form'), "btnSaveUserPreference")) {
        var UserPreferenceViewModel = {
            TimeZoneID: $("#TimeZoneID").val(),
            Language: $("#Language").val(),
            Disable: $("#Disable").is(":checked"),
            sTimeZone: $("#TimeZoneID").data("kendoDropDownList").text(),
        }

        var form = $('#form1');
        var token = $('input[name="__RequestVerificationToken"]', form).val();

        // var token = $("#form1 input").val();

        $.ajax({
            type: 'POST',
            url: ResourceLayout.partialURL + "SaveUpdateUserPreference",
            dataType: 'json',
            data: { __RequestVerificationToken: token, objUserPreferenceViewModel: UserPreferenceViewModel },
            success: function (result) {
                if (parseInt(result) == 1) {
                    jAlert("User Preference Save Successfully", "alert", function (r) {
                        window.location.href = ResourceLayout.partialURL + "UserPreference";
                    });
                }
                else {
                    jAlert(result, "alert");
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {

            }
        });
    }
    else {
        $('form').submit();
    }
});