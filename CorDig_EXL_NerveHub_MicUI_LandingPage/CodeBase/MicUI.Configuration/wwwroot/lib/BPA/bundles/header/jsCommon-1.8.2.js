function OpenSmallModalPopup(html) {

    var strHtml = "";
    strHtml = strHtml + '<div class="header1">'
    + '<div style="float:left;">'
    + '<span id="spnHeaderTitle"></span>'
    + '</div>'
    + '<div style="float:right;">'
    + '<div class="close1" style="color:white;cursor:pointer;">x</div>'
    + '</div>'
    + '</div>'
    + '<div id="divPopupContainer">'
    + html
    + '</div>'
    + '</div>'
    $("#divPopup").html(strHtml);
    $("#divOverlay").show();
    $("#divPopup").show();

    $("#divPopup").draggable({ handle: $(".header1") });
    $(".header1").css({ cursor: 'move' });
}

$(document).ready(function () {

    $("form").kendoValidator();
    $(":submit").click(function () {
        $(":submit").removeClass('clickedButton');
        $(this).addClass('clickedButton');
    });

});

function dataBound(e) {
   
    if (e != undefined) {
        if (e.sender.dataSource._data.length == 1) {
             e.sender.select(1);
            //hide the validation messages when hide button is clicked
            $('form').kendoValidator().data("kendoValidator").hideMessages();
        }
    }

    //if (e.sender.element[0].id == "WorkObjName") {
    //    onChangeStoreID();
    //}
    if (e.sender._form != undefined) {
      
        var ActionName_ = e.sender._form[0].action.split('/');
        var ActionName = ActionName_[ActionName_.length - 1]
      
        if (e.sender.element[0].id == "CampaignName" && ActionName == "SkillCampaignMap") {
            Get_SkillList();
        } else
            if (e.sender.element[0].id == "ProcessId" && ActionName == "SaveWorkFilter") {
                onCampaignIDChange();
            }
            else if (e.sender.element[0].id == "CampaignId" && ActionName == "SaveWorkFilter") {
                onCampaignIDChange();
            }
            else if (e.sender.element[0].id == "WorkObjName" && ActionName != "LEtterMAppingSearchView") {
                e.sender.select(0);
                // onChangeStoreID();
            }
            else if (e.sender.element[0].id == "ProcessName" && ActionName == "Calibration") {
                CalibrationSetup_ShowSampleGrid();
            }
            else if (e.sender.element[0].id == "TeamName" && ActionName == "SkillUserMap") {
                onSelect();
            }
            else if (e.sender.element[0].id == "ProcessName" && ActionName == "ProcessConfig") {
                Create_Breaktype();
            }
            else if (e.sender.element[0].id == "ProcessName" && ActionName == "SampleSetup") {
                
                ShowSampleGrid();
            }
        
    }
    

}

$(function () {

    $('.k-textbox').on('keypress', function (e) {

        if ($('.k-textbox').val() == "" && (e.which == 40 || e.which == 123 || e.which == 125 || e.which == 91 || e.which == 92 || e.which == 124 || e.which == 93 || e.which == 63 || e.which == 32 || e.which == 37 || e.which == 126 || e.which == 96 || e.which == 33 || e.which == 35 || e.which == 36 || e.which == 37 || e.which == 94 || e.which == 38 || e.which == 42 || e.which == 41 || e.which == 45 || e.which == 61 || e.which == 43 || e.which == 42 || e.which == 47 || e.which == 44 || e.which == 60 || e.which == 62 || e.which == 59 || e.which == 58 || e.which == 34 || e.which == 39)) {
            return false;
        }

        if ($('.k-textbox').val() != "" && (e.which == 40 || e.which == 123 || e.which == 125 || e.which == 92 || e.which == 93 || e.which == 91 || e.which == 124 || e.which == 63 || e.which == 37 || e.which == 126 || e.which == 96 || e.which == 33 || e.which == 35 || e.which == 36 || e.which == 37 || e.which == 94 || e.which == 38 || e.which == 42 || e.which == 41 || e.which == 45 || e.which == 61 || e.which == 43 || e.which == 42 || e.which == 47 || e.which == 44 || e.which == 60 || e.which == 62 || e.which == 59 || e.which == 58 || e.which == 34 || e.which == 39)) {
            return false;
        }
    });
});



$(".Numberonly").on("keypress", function (e) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (charCode != 46 && charCode > 31
      && (charCode < 48 || charCode > 57))
        return false;

    return true;
});


$(".alphaonly").on("keypress", function (e) {
    var key;
    key = e.which ? e.which : e.keyCode;
    if ((key >= 65 && key <= 91) || (key >= 97 && key < 123) || key == 8 || key == 9 || (key >= 37 && key <= 40) || key == 46 || /*key == 35 || key == 36 ||*/key == 116) {
        return true;
    }
    else {
        return false;
    }
});

function onRowBoundSearchGrid(e) {
    setTimeout(function () {
        $(".k-grid-Edit, .k-grid-Redigere, .k-grid-Edytować, .k-grid-Editar").find("span").addClass("k-icon k-edit");
        $(".k-grid-Delete").find("span").addClass("k-icon k-delete");
    }, 100)
}

function AllowTabOnly(e) {

    var keyCode = e.keyCode || e.which;
    if (keyCode == 9) {

        return true;
    }
    else {
        return false;
    }
}


/* To open a given url(View) */
function MoveToUrl(url)
{ window.location.href = $.trim(url); }

/*Read the gridView */
function SearchGridView() {
    var grid = $("#gridView" + controller).data("kendoGrid")
    grid.dataSource._destroyed = [];
    grid.dataSource.read();
}

/* to select all checkbox of grid */
$("#chkSelectAll").on('click', function (e) {
    if ($(this).is(":checked")) {
        $(".chkbox").prop("checked", "checked");
    }
    else {
        $(".chkbox").prop("checked", false);
    }
});

$("#chkCateSelectAll").on('click', function (e) {
    if ($(this).is(":checked")) {
        $(".chk").prop("checked", "checked");
    }
    else {
        $(".chk").prop('checked', false);
    }
});

function DropDownError(e) {
    
  ///  jAlert(e.errorThrown);
}

function KendoGridError(e) {
    
    jAlert(e.errorThrown);
}

function ErroPopup(error, body) {

    var erropopup = "<div id='popup_container' class='errorSummary' style='position: fixed; z-index: 99999; padding: 0px; margin: 0px; min-width: 460px; max-width: 460px; top: 195.5px; left: 401.5px;'" +
       "class='ui-draggable'>" +
    "<div id='divPopup_title'>" +
    "<h1 id='popup_title' class='ui-draggable-handle' style='cursor: move;'>error Summary</h1></div><div id='popup_content' class='confirm'><div style='width: 365px; color: rgb(255, 87, 95);' id='popup_message'>" +
   error +
    "</div><div id='popup_panel'><span><button tabindex='0' aria-disabled='false' role='button' data-role='button' class='k-primary k-button k-button-icontext claOkay'  id='popup_ok' type='button'><span class='k-icon k-i-tick'></span>&nbsp;Ok&nbsp;</button></span></div></div></div>"
    + "<div id='popup_overlay' ></div>"
    $("body").append(erropopup);
    $("#popup_overlay").css({
        position: 'fixed',
        zIndex: 99998,
        top: '0px',
        left: '0px',
        width: '100%',
        height: '100%',
        background: 'gray',
        opacity: '0.6',
        overflow: 'auto'
    });
    $("#popup_container").draggable({ handle: $("#popup_title") });
}

function validationForm() {
    var validator = $("form").data("kendoValidator");
    if (!validator.validate()) {
        if ($("#ClientName").val() == "") {
            $("#ClientName_validationMessage").ready(function () {
                setTimeout(function () {

                    $("#ClientName_validationMessage").removeAttr("style");
                }
                , 0);
            })
        }
        if ($("#ProcessName").val() == "") {
            $("#ProcessName_validationMessage").ready(function () {
                setTimeout(function () {
                    $("#ProcessName_validationMessage").removeAttr("style");
                }, 0);
            })
        }

        if ($("#CampaignName").val() == "") {
            $("#CampaignName_validationMessage").ready(function () {
                setTimeout(function () {
                    $("#CampaignName_validationMessage").removeAttr("style");
                }, 0);
            })
        }
    }
}


function ClientSideValidatePage(formId) {
    var html = "";

    $(formId).find(".rfv").each(function () {
        if (($(this).is("input") || $(this).is("select") || $(this).is("textarea") || $(this).is("file")) && $(this).attr("validationmessage") != undefined)
            html += "<li>" + $(this).attr("validationmessage"); + "</li>";
    });
    if (html == "") {
        return true;
    }
    else {
        ErroPopup(html);
        return false;
    }
}

$(".claOkay").on("click", function () {
    $(".errorSummary").remove();
    $("#popup_overlay").remove();
})


var dateFields = [];
var columns = [];
function generateGrid(gridData, ProxyUrl, ExcelFileName) {
    
    if (gridData != null) {
        var model = generateModel(gridData[0]);
        var parseFunction;
        if (dateFields.length > 0) {
            parseFunction = function (response) {
                for (var i = 0; i < response.length; i++) {
                    for (var fieldIndex = 0; fieldIndex < dateFields.length; fieldIndex++) {
                        var record = response[i];
                        record[dateFields[fieldIndex]] = kendo.parseDate(record[dateFields[fieldIndex]]);
                    }
                }
                return response;
            };
        }
        //}
        var Rgrid = $('#grid').data("kendoGrid");
        if (Rgrid != undefined) {
            Rgrid.destroy();
        }

        var grid = $("#grid").kendoGrid({
            toolbar: ["excel"],
            excel: {
                fileName: ExcelFileName,
                proxyURL: ProxyUrl,
                filterable: true
            },
            columns: columns,
            dataSource: {
                data: gridData
            }
        });
    }
    else {
        var Rgrid = $('#grid').data("kendoGrid");
        if (Rgrid != undefined) {
            Rgrid.destroy();
            $("#grid").empty();
        }
    }
}

function generateModel(gridData) {
    var model = {};
    model.id = "ID";
    var fields = {};
    columns = [];
    for (var property in gridData) {

        var propType = typeof gridData[property];

        if (propType == "number") {
            fields[property] = {
                type: "number",
                validation: {
                    required: true
                }
            };
        } else if (propType == "boolean") {
            fields[property] = {
                type: "boolean",
                validation: {
                    required: true
                }
            };
        } else if (propType == "string") {
            var parsedDate = kendo.parseDate(gridData[property]);
            if (parsedDate) {
                fields[property] = {
                    type: "string",
                    validation: {
                        required: true
                    }
                };
                // dateFields.push(property);
            } else {
                fields[property] = {
                    validation: {
                        required: true
                    },

                };
            }
        } else {
            fields[property] = {
                validation: {
                    required: true
                },

            };
        }
        columns.push({
            field: property,
            width: 200
        });
    }
    model.fields = fields;
    return model;
}
$(document).click(function (event) {
    $(".k-grid-Edit, .k-grid-Redigere, .k-grid-Edytować, .k-grid-Editar").find("span").addClass("k-icon k-edit");
});
