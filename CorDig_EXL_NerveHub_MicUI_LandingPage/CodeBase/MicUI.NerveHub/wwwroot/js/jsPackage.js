/*
     * FileName: jsAdmin.js
     * ClassName: jsAdmin
     * Purpose: This file contains all the Licensing Module Methods scripts
     * Description: Basically the JQuery Methods used to validate the controls or other Client Side scripting or Manipulation. Also on Documentready
     * Created By: Manoj Yadav
     * Created Date: 17 Mar 2015
     * Modified By: 
     * Modified Date:
     * Modified Purpose:
     * Modified Description: 
 */

var flage = 0;


/*
This function used to get the detail of a package lisence by a package id = "id"
in view page: Package/CreatePackageLicense.cshtml
*/
function GetLicensePackageDeatilById(id) {
    $.ajax({
        url: GlobalVaribale.SiteUrl + "/Package/GetLicensePackageDeatilById/" + id,
        dataType: "json",
        type: "post",
        error: function () {
            jAlert("An error occurred.", "Alert");
        },
        success: function (data) {

            $("#tblModule").show();

            if (data.length > 0) {
                for (var i = 0; i < data.length; i++) {
                    $("#tblModule > tbody").append("<tr><td width='50%'><div FeatureId=" + data[i].FeatureId + " ModuleId=" + data[i].ModuleId + " >" + data[i].ModuleName + "</div></td><td width='50%'>" + data[i].FeatureName + "</td><td><input title='Remove' class='deleteIcon FeatureReomve' type='button' /> </td></tr>");
                }
            }
        }
    });
}

/*
This event used to get Disbale the the PackageName dropdown once it has been selected
in view page: Package/CreatePackageLicense.cshtml
*/
$("select#PackageName").on("change", function () {
    $("#PackageName").prop("disabled", true);
});


/*
This event used to get the all the features a module by ModuleId
in view page: Package/CreatePackageLicense.cshtml
*/
$("#ModuleName").on("change", function () {
    var ModuleId = $("#ModuleName").val();
    $.ajax({
        url: GlobalVaribale.SiteUrl + "/Package/GetModuleFeature",
        data: { ModuleId: ModuleId },
        dataType: "json",
        type: "GET",
        error: function () {
            jAlert(" An error occurred.", "Alert");
        },
        success: function (data) {
            var Html = "<div id='divFeatures' style='float: left'>";
            for (var i = 0; i < data.length; i++) {
                Html = Html + "<input  style='float: left' type='checkbox' id='chkFeature" + data[i].ModuleFeatureID + "'  FeatureId='" + data[i].ModuleFeatureID + "' FeatureName='" + data[i].FeatureName + "' >"
                Html = Html + "<span>" + data[i].FeatureName + "</span>"
                Html = Html + "</br></br>";
            }
            Html = Html + "</div>";
            $("#divFeatures").html(Html);
        }
    });
});

/*
This function is called once the Package Name saved and which is return
the PackageId
in view page: Package/CreatePackage.cshtml
*/
function OnSuccessAddPackage(retValue) {
    if (retValue > 0) {
        jAlert("Package name saved successfully", "Alert", function (r) {
            if (r) {
                window.location.href = GlobalVaribale.SiteUrl + "/Package/PackageList";
            }
        });
    }
    else if (retValue < 0) {
        jAlert("Package name is already exist, please enter some other package name.", "Alert");
    }
    else if (retValue == 0) {
        jAlert("There is an error while saving package name", "Alert");
        $("#PackageName").val("");
        $("#PackageDescription").val("");
    }
}

/*
This event is used to add the the features in a temprary table of the different modules before the final 
submission of a package lisence 
in view page: Package/CreatePackageLicense.cshtml
*/
$("#btnAddFeatures").on("click", function () {
    var countSelectedFeature = 0;
    $("#tblModule").show();



    $("#divFeatures input[type=checkbox]").each(function () {
        if ($(this).is(":checked")) {
            countSelectedFeature++;
        }
    });

    if (countSelectedFeature == 0 && $("select#ModuleName").val() != "") {
        jAlert("Please select one feature", "Alert");
        return false;
    }

    $("#divFeatures input[type=checkbox]").each(function () {
        if ($(this).is(":checked")) {
            var existFeaturesId = $(this).attr("FeatureId");
            var existFeaturesName = $(this).attr("FeatureName");

            $("#tblModule > tbody tr td div").each(function () {
                if (existFeaturesId == $(this).attr("FeatureId")) {
                    jAlert("You have already selected <b>" + existFeaturesName + "</b>", "Alert");
                    flage = 1;
                    return false;
                }
            });
        }
    });

    if (flage == 0) {
        $("#divFeatures input[type=checkbox]").each(function () {
            if ($(this).is(":checked")) {
                var existFeaturesId = $(this).attr("FeatureId");
                var existFeaturesName = $(this).attr("FeatureName");
                $("#tblModule > tbody").append("<tr><td width='50%'><div FeatureId=" + existFeaturesId + " ModuleId=" + ($("#ModuleName").val() + ">" + $("#ModuleName :selected").text() + "</div></td><td width='50%'>" + existFeaturesName + "</td><td><input title='Remove' class='deleteIcon FeatureReomve' type='button' /> </td></tr>"));
            }
        });
    }

    if ($("#tblModule > tbody tr td div").length < 1) {
        jAlert("Please select at least one module and feature", "Alert");
        return flage;
    }
    flage = 0;
});

/*
This event is used to remove a last feature from the temperary tables
in view page: Package/CreatePackageLicense.cshtml
--Not using currently
*/
$("#btnRemoveFeatures").on("click", function () {
    if ($("#tblModule > tbody tr").length != 1) {
        $("#tblModule > tbody tr:last").remove();
    }
});


/*
This event is used to save the all modules and features for the selected package
in view page: Package/CreatePackageLicense.cshtml
*/
$("#btnSavePackageLicense").on("click", function () {

    if ($("select#PackageName").val() == "") {

        jAlert("Please select package first", "Alert", function (r) {
            if (r) {
                $("select#PackageName").focus();
            }
        });
        return false;
    }

    var arrModules = [];
    var AdminPackageEntity = [];
    var AdminModuleFeatureEntity = [];
    var packageId = $("select#PackageName").val();

    $("#tblModule > tbody tr td div").each(function () {
        var moduleId = $(this).attr("ModuleId");
        var featureId = $(this).attr("FeatureId");
        AdminPackageEntity.push({ PackageId: packageId, ModuleId: moduleId, FeatureId: featureId });
    });
    var postUrl = "";

    if ($("#ActionType").val() == "Insert") {
        postUrl = GlobalVaribale.SiteUrl + "/Package/InsertPackageLisence";
    }
    else {
        postUrl = GlobalVaribale.SiteUrl + "/Package/UpdatePackageLisence";
    }
    if (AdminPackageEntity.length > 0) {
        $.ajax({
            url: postUrl,
            data: JSON.stringify(AdminPackageEntity),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            type: "post",
            error: function () {
                jAlert(" An error occurred.", "Alert");
            },
            success: function (data) {
                if (parseInt(data) > 0) {
                    jAlert("Module package is saved successfully", "Alert", function (r) {
                        if (r) {
                            window.location.href = GlobalVaribale.SiteUrl + "/Package/LicensePackageList";
                        }
                    });
                }
                else if (parseInt(data) == -1) {
                    jAlert("This package already added", "Alert");
                }
                else {
                    jAlert("An error occurred while saving module package", "Alert");
                }
            }
        });
    }
    else {
        jAlert("Please select atleast one module and feature", "Alert", function (r) {

            if (r) {
                $("select#ModuleName").focus();
            }
        });
        return false;
    }


});

/*
This function called once the clinet package subscription has been saved
in view page: Package/CreatePackageSubscription.cshtml
*/
function OnSuccessSavePackageSubscription(subscriptionId) {

    if (subscriptionId < 0) {
        jAlert("Client have already subscried for a package", "Alert");
    }
    else if (subscriptionId == 0) {
        jAlert("An error occurred while saving clinet package subscription", "Alert");
    }
    else if (subscriptionId > 0) {
        jAlert("Client subscription saved successfully", "Alert", function (r) {
            if (r) {
                window.location.href = GlobalVaribale.SiteUrl + "/Package/PackageSubscriptionList";
            }
        });

    }
    return false;
}

/*
This this function is used to delete the the package subscription
in view page: Package/PackageSubscriptionList.cshtml
*/
function DeleteSubscription(id) {

    jConfirm('Are you sure, you want to delete this subscription?', 'Confirmation', function (r) {
        if (r) {
            $.ajax({
                url: GlobalVaribale.SiteUrl + "/Package/DeleteSubscription/" + id,
                dataType: "json",
                type: "Post",
                error: function () {
                    jAlert("An error occurred.", "Alert");
                },
                success: function (data) {
                    if (data > 0) {
                        jAlert("Subscription has been deleted successfully", "Alert", function () {
                            window.location.href = GlobalVaribale.SiteUrl + "/Package/PackageSubscriptionList";
                        });
                    }
                    else {
                        jAlert("An error occurred.", "Alert");
                    }
                }
            });
        }
        else { return false; }
    });

}


/*
This this function is used to delete the the package lisence
in view page: Package/LicensePackageList.cshtml
*/
function DeletePackageLicenseFeatures(id) {

    jConfirm('Are you sure, you want to delete this package license?', 'Confirmation', function (r) {
        if (r) {
            $.ajax({
                url: GlobalVaribale.SiteUrl + "/Package/DeletePackageLicense/" + id,
                dataType: "json",
                type: "POST",
                error: function () {
                    jAlert("An error occurred.", "Alert");
                },
                success: function (data) {
                    if (data > 0) {
                        jAlert('Package license has been deleted successfully', 'Alert', function (r) {
                            if (r) {
                                window.location.href = GlobalVaribale.SiteUrl + "/Package/LicensePackageList";
                            }
                        });
                    }
                    else {
                        jAlert("An error occurred.", "Alert");
                    }
                }
            });
        }
        else {
            return false;
        }
    });

}

/*
This this function is used to delete the the package name
in view page: Package/PackageList.cshtml
*/
function DeletePackage(id) {

    jConfirm('Are you sure, you want to delete this package?\nAll the reference records will deleted!', 'Confirmation', function (r) {
        if (r) {
            $.ajax({
                url: GlobalVaribale.SiteUrl + "/Package/DeletePackage/" + id,
                dataType: "json",
                type: "POST",
                error: function () {
                    jAlert("An error occurred.", "Alert");
                },
                success: function (data) {
                    if (data > 0) {
                        jAlert('Package has been deleted successfully', 'Alert', function (r) {
                            if (r) {
                                window.location.href = GlobalVaribale.SiteUrl + "/Package/PackageList";
                            }
                        });
                    }
                    else {
                        jAlert("An error occurred.", "Alert");
                    }
                }
            });
        }
        else {
            return false;
        }
    });

}


/*
This event is used to remove a specific feature from the temperary tables
in view page: Package/CreatePackageLicense.cshtml
*/
$(".FeatureReomve").on("click", function () {
    $(this).parent().parent().remove();
});

/*
This function is used to validate the CreatePackageSubscription.cshtml
in view page: Package/CreatePackageSubscription.cshtml
*/
function ValidatePackageSubscriptionPage() {
    var LicenseStartDate = $("#LicenseStartDate").val();
    var LicenseEndDate = $("#LicenseEndDate").val();

    if (new Date(LicenseEndDate) <= new Date(LicenseStartDate)) {
        $("#errLicenseEndDate").text("End date can not be less or equal to start date");
        $("#errLicenseEndDate").show();
        return false;
    }
    else {
        $("#errLicenseEndDate").hide();
        return true;
    }
}

$("#LicenseEndDate").on("blur change", function () {
    ValidatePackageSubscriptionPage();
});


/*
This function is used to validate the CreatePackageSubscription.cshtml
in view page: Package/CreatePackageSubscription.cshtml
*/

$("#ancSearch").on("click", function () {
    $("#form1").submit();
});

$(document).keypress(function (e) {
    if (e.which == 13) {
        if ($("#SearchText").is(":focus")) {
            $("#form1").submit();
        }
        else {
            $("#SearchText").focus();
        }
    }
});

//====================================

/*
This function is used to validate the CreatePackageSubscription.cshtml
in view page: Package/CreatePackageSubscription.cshtml
*/


$(".close1, .btnCancel").on("click", function () {
    $("#divPopup").hide();
    $("#divOverlay").hide();
});

$("#ancPackageAdvanceSearch,#ancPackageLicenseAdvanceSearch,#ancSubscriptionAdvanceSearch").on("click", function () {

    $("#SearchText").val("");
    var fullUrl = "";
    var strTitle = "";
    if ($(this).context.id == "ancPackageAdvanceSearch") {
        fullUrl = GlobalVaribale.SiteUrl + "/Package/AdvanceSearch/Package";
        varTitle = "Package advance search";
    }
    else if ($(this).context.id == "ancPackageLicenseAdvanceSearch") {
        fullUrl = GlobalVaribale.SiteUrl + "/Package/AdvanceSearch/PackageLicense";
        varTitle = "Package license advance search";
    }
    else if ($(this).context.id == "ancSubscriptionAdvanceSearch") {
        fullUrl = GlobalVaribale.SiteUrl + "/Package/AdvanceSearch/Subscription";
        varTitle = "Package subscription advance search";
    }

    $.ajax({
        url: fullUrl,
        type: "get",
        dataType: "html",
        success: function (data) {
            OpenSmallModalPopup(data);
            $("#spnHeaderTitle").text(strTitle);
        },
        error: function () {
            jAlert("An error occured");
        }
    });

});

/*
This event is used to validate the advance serach popup for all the view

*/

$("#btnAdvanceSearch").on("click", function () {
    var strAdvanceSearchText = $.trim($("#AdvanceSearchText").val());

    if ($("#AdvanceSearchColumn").val() == "") {
        jAlert("Please select a column to search", "Alert", function (r) {
            if (r) {
                $("#AdvanceSearchColumn").focus();
            }
        });
        return false;
    }

    if ($("#AdvanceSearchColumn").val() != "" && strAdvanceSearchText == "") {
        jAlert("Please enter text to search", "Alert", function (r) {
            if (r) {
                $("#AdvanceSearchText").val("");
                $("#AdvanceSearchText").focus();
            }
        });
        return false;
    }
    else {
        $("#form1").submit();
    }
});


$("#ancClearSearch").on("click", function () {
    $("#SearchText").val("");
    $("#AdvanceSearchText").val("");
    $("#form1").submit();
});