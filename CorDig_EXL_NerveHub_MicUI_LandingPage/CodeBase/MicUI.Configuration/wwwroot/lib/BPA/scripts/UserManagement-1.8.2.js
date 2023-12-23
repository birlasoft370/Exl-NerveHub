
/* --- index Page ----- */
var SearchFlage = 'SBLID';
$(document).ready($("#srhSupwiserPop").on("click", function (e) {

    openeIndexUserPopup(0);
}));
$(document).ready($("#srhLanID").on("click", function (e) {

    openeIndexUserPopup(1)
}));
$(document).ready($("#UserDetail").on("click", function (e) {

    openeIndexUserPopup(2)
}));
//Change Width DK
function openeIndexUserPopup(id) {

    var title;
    if (id == 1) {
        $("#OprionSerchBy").css("display", "none");
        title = ResourceUserManagementIndex.display_SearchUserTypeLink
    }
    else if (id == 2) {
        $("#OprionSerchBy").css("display", "block");
        title = ResourceUserManagementIndex.display_UserDetailLink
    }
    $("#ClickId").val(id);
    $("#PopBody").css("display", "block");
    $("input[name=FSerachCondetion][value=1]").prop('checked', true);
    SearchFlage = 'SBLID';
    $("#srhText").val("");
    $("#lstSuperwiser").empty();
    var accessWindow = $("#dialog").kendoWindow({
        actions: ["Close"],
        draggable: true,
        height: "700px",
        modal: true,
        resizable: false,
        //title: title,
        width: "750px",
        visible: false,
        stack: true
    }).data("kendoWindow");
    accessWindow.title(title);
    accessWindow.center().open()

}

$(':radio[name=FSerachCondetion]').on("click", function (e) {

    SearchFlage = $(this)[0].value == 2 ? 'SBFN' : $(this)[0].value == 3 ? 'SBLN' : 'SBLID';
});

$(document).ready($("#btnSearchIndex").on("click", function () {
    // debugger;
    var token = $("#form1 input[name=__RequestVerificationToken]").val();
    $("#lstSuperwiser").empty();
    if ($("#srhText").val() == "") {
        jAlert(ResourceUserManagementIndex.required_sSearchText, "Alert", function () {

            $("#srhText").focus();
        });
        $("#srhText").focus();
    }
    else {
        kendo.ui.progress($('#dialog'), true);
        //  alert( );
        if ($("#ClickId").val() == "1") {

            $("#Message").show();
            $.ajax({
                type: 'POST',
                url: ResourceUserManagementIndex.urlPath_GetLanUser,
                dataType: 'json',
                data: { __RequestVerificationToken: token, Text: $("#srhText").val() },
                success: function (LanUsers) {
                    if (LanUsers.LanUsers.length > 0) {
                        // debugger;
                        //jAlert(Superwisers);
                        $.each(LanUsers.LanUsers, function (i, LanUser) {
                            $("#lstSuperwiser").append('<option value="' + LanUser.EmailID + '">' +
                                LanUser.UserName + '</option>');
                        });
                        kendo.ui.progress($('#dialog'), false);
                    } else {
                        jAlert('Failed to retrieve Lan Users.');
                        kendo.ui.progress($('#dialog'), false);
                    }
                },
                error: function (ex) {
                    jAlert('Failed to retrieve Lan Users.' + ex);
                    kendo.ui.progress($('#dialog'), false);
                }
            });
            $("#Message").hide();
            // kendo.ui.progress($('#form1'), false);
        }
        else {

            $.ajax({
                type: 'POST',
                url: ResourceUserManagementIndex.urlPath_GetSuperwiser,
                dataType: 'json',
                data: { Text: $("#srhText").val(), SearchCondition: SearchFlage },
                success: function (Superwisers) {
                    if (Superwisers.lstSuperwiser.length > 0) {
                        // debugger;
                        //jAlert(Superwisers);
                        $.each(Superwisers.lstSuperwiser, function (i, Superwiser) {
                            $("#lstSuperwiser").append('<option isclient=' + Superwiser.bClientUser + ' value="' + Superwiser.iUserID + '">' +
                                Superwiser.Name + '</option>');
                        });
                    }
                    else {
                        jAlert('Failed to retrieve Lan Users.');
                    }
                    kendo.ui.progress($('#dialog'), false);
                },
                error: function (ex) {
                    jAlert('Failed to retrieve Superwisers.' + ex);
                    kendo.ui.progress($('#dialog'), false);
                }
            });
            // kendo.ui.progress($('#form1'), false);
        }
    }
}));

$(document).ready($("#lstSuperwiser").on("click", function () {
    //
    debugger;
    if ($(this).find(':selected').val() != undefined) {
        if ($("#ClickId").val() == "0") {
            //
            $("#sSupervisorName").val($(this).find(':selected').text());
            $("#iSupervisorID").val($(this).find(':selected').val());
        }
        else if ($("#ClickId").val() == "1") {

            if ($("#srhText").val() == "") {
                jAlert(ResourceUserManagementIndex.required_sSearchText);
                return false;
            }

            var UserName = $(this).find(':selected').text().split(" ");
            var LoginName = $(this).find(':selected').val().split(":");

            //$("#sLoginName").val(LoginName[0]);
            //$("#sFirstName").val(UserName[0]);
            $("#sLoginName").val(LoginName[0]);
            if (LoginName[4] == '') {
                $("#sFirstName").val(UserName[0]);
            } else {
                $("#sFirstName").val(LoginName[4]);

            }
            var isSIsclient = $(this).find(':selected').attr('isclient');
            if (isSIsclient == true) {
                $("input[name='RbtnsearchLAN'][value='3']").prop('checked', true);
            }
            else {
                $("input[name='RbtnsearchLAN'][value='1']").prop('checked', true);
            }
            if (UserName.length > 2) {
                $("#sMiddleName").val(LoginName[3]);
                $("#sLastName").val(LoginName[2]);
            }
            else {
                $("#sMiddleName").val(LoginName[3]);
                $("#sLastName").val(LoginName[2]);
            }
            if ($("#sLastName").val() == "-")
                $("#sLastName").val("");
            if ($("#sMiddleName").val() == "-")
                $("#sMiddleName").val("");
            $("#bLanID").val("true");
            $("#sEmail").val(LoginName[1]);
        }
        else if ($("#ClickId").val() == "2") {
            window.open(ResourceUserManagementIndex.urlPath_UserDetailsView + "?UserID=" + $(this).find(':selected').val(), "_self");

        }
        $("#dialog").closest(".k-window-content").data("kendoWindow").close();
    }
}))

$(document).ready(
    $("form").each(function (i, form) {

        $(this).find(".k-grid").each(function (_i, div) {
            $(form).submit(div, kendoGridSubmitData);
        });
    })
);
function kendoGridSubmitData(e) {
    //
    var lModel = e.data.id;
    var lKendoGrid = $(e.data).data("kendoGrid");

    var allIndex = 0;
    if (lKendoGrid.dataItems().length > 0) {
        // Iterate over all rows
        lKendoGrid.dataItems().forEach(function (_row, _rowIndex) {
            // Iterate over all fields
            _row.forEach(function (_value, _name) {
                //jAlert(_value, _name);
                allIndex = parseInt(_rowIndex, 10);
                // Add the input hidden
                $('<input>').attr({
                    type: 'hidden',
                    id: lModel,
                    name: lModel + '[' + _rowIndex + '].' + _name,
                    value: _value
                }).appendTo('form');
            });
            $('<input>').attr({
                type: 'hidden',
                id: "TotalIndex",
                name: "TotalIndex",
                value: allIndex
            }).appendTo('form');

        });
    }
}

function OnClientChange() {
    //jAlert(("#iClientID").val());
    $.ajax({
        type: 'POST',
        url: ResourceUserManagementIndex.urlPath_jSonGetGridProcess,
        dataType: 'json',
        data: { ClientId: $("#iClientID").val() },
        success: function (result) {
            //jAlert(result);
            var grid = $('#ProcessGrid').getKendoGrid();
            grid.dataSource.data(result);
            grid.refresh();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            jAlert(textStatus, errorThrown);
        }
    });
    return false;
}

function OnclickIndexUAAV() {

    window.location.href = ResourceUserManagementIndex.urlPath_UserAccessApprovalView;

}

function OnClickUserManagementIndex() {
    window.location.href = window.location.href = ResourceUserManagementIndex.urlPath_Index;
}

function OnClickIndexSearch() {
    SearchFlage = 'SBLID';
    window.location.href = ResourceUserManagementIndex.urlPath_UserManagementSearchView;
}

var dataItem; //This should be Global variable
function getParentID() {
    //

    var row = $(event.srcElement).closest("tr");
    var grid = $(event.srcElement).closest("[data-role=grid]").data("kendoGrid");
    dataItem = grid.dataItem(row);
    //var arr = [];
    //$(dataItem.lstApprover).each(function () {
    //    arr.push(this);
    //});
    return { values: dataItem.iProcessID };
}

function CheckChangedIndex(ctrl, pId) {
    //
    var grid = $("#ProcessGrid").data("kendoGrid");
    var DataSource = grid.dataSource;
    for (var i = 0; i < DataSource.total(); i++) {
        if (DataSource.data()[i].iProcessID == pId) {
            DataSource.data()[i].set("bSelected", ctrl.checked);
            break;
        }
    }
}

function onSelectChange(ctrl, pId) {
    //
    var grid = $("#ProcessGrid").data("kendoGrid");
    var DataSource = grid.dataSource;
    for (var i = 0; i < DataSource.total(); i++) {
        if (DataSource.data()[i].iProcessID == pId) {
            DataSource.data()[i].iApproverID = ctrl.value;
            break;
        }
    }
}

function onSelectIndex(e) {
    //
    var selectedValue = this.dataItem(e.item.index());
    //jAlert(selectedValue);
    if (dataItem != null) {
        dataItem.SelectedApprover = selectedValue;
        //jAlert(selectedValue.iApproverId);
        dataItem.iApproverID = selectedValue.iApproverId;
    }
}

function onOpen() {
    $.ajax({
        url: ResourceUserManagementIndex.urlPath_JsonGetLOB,
        type: 'Post',
        cache: false,
        success: function (result) {
            $('#iLOBID').data('kendoDropDownList').dataSource.data(result);

        }
    });
}

function onOpeniSBUID() {
    $.ajax({
        url: ResourceUserManagementIndex.urlPath_JsonGetSBU,
        type: 'Post',
        cache: false,
        success: function (result) {
            $('#iSBUID').data('kendoDropDownList').dataSource.data(result);

        }
    });
}

/* -----------------User Details View -----------------------------------*/

$(document).ready($("#SrchUser").click(function (e) {
    openeUserDetailsViewPopup(2)
}));

function gvClientProcessDetailDataBound() {
    $("#gvClientProcessDetail").data("kendoGrid").hideColumn(3);
    $("#gvClientProcessDetail").data("kendoGrid").hideColumn(1);
}
function openeUserDetailsViewPopup() {

    $("#PopBody").css("display", "block");
    $("#srhText").val("");
    $("#lstSuperwiser").empty();
    var accessWindow = $("#dialog").kendoWindow({
        actions: ["Close"],
        draggable: true,
        height: "200px",
        modal: true,
        resizable: false,
        title: "",
        width: "630px",
        visible: false
    }).data("kendoWindow").center().open();
}

$(document).ready($("#btnSearch_UDV").click(function () {

    $("#lstSuperwiser").empty();
    // 
    if ($("#srhText").val() == "") {
        jAlert(ResourceUserDetailsView.required_sSearchText, "Alert", function () {

            $("#srhText").focus();
        });
        $("#srhText").focus();
    }
    else {
        $.ajax({
            type: 'POST',
            url: '@Url.Action("GetSuperwiser")',
            dataType: 'json',
            data: { Text: $("#srhText").val() },
            success: function (Superwisers) {
                //alert(Superwisers);
                $.each(Superwisers.lstSuperwiser, function (i, Superwiser) {
                    $("#lstSuperwiser").append('<option value="' + Superwiser.iUserID + '">' +
                        Superwiser.Name + '</option>');
                });
            },
            error: function (ex) {
                //alert('Failed to retrieve Superwisers.' + ex);
            }
        });
    }

}))

$(document).ready($("#lstSuperwiser").click(function () {

    // window.location.href = ResourceUserDetailsView.urlPath_UserDetailsView + "?UserID=" + $(this).find(':selected').val();
    $("#dialog").closest(".k-window-content").data("kendoWindow").close();

}))

function OnClickUserManagement_view() {
    window.location.href = ResourceUserDetailsView.urlPath_Index;
}

/*--- User Access Approval View ---*/

function OnClickUserManagement() {
    window.location.href = ResourceUserAccessApprovalView.urlPath_Index;
}


function gvRequestStatusDataBound() {

};

$(document).ready($("#srhUserPop").on("click", function (e) {
    openeUserPopup(0);
}));

function startChange() {

    var endPicker = $("#ToDate").data("kendoDatePicker"),
        startDate = this.value();

    if (startDate) {
        startDate = new Date(startDate);
        startDate.setDate(startDate.getDate());
        endPicker.min(startDate);
    }
}

function endChange() {

    var startPicker = $("#FromDate").data("kendoDatePicker"),
        endDate = this.value();

    if (endDate) {
        endDate = new Date(endDate);
        endDate.setDate(endDate.getDate());
        startPicker.max(endDate);
    }
}

function openeUserPopup(id) {

    $("#PopBody").css("display", "block");
    $("input[name=FSerachCondetion][value=1]").prop('checked', true);
    SearchFlage = 'SBLID';
    $("#srhText").val("");
    $("#lstUsers").empty();
    var accessWindow = $("#dialog").kendoWindow({
        actions: ["Close"],
        draggable: true,
        height: "370px",
        modal: true,
        resizable: false,
        title: "",
        width: "650px",

        visible: false
    }).data("kendoWindow").center().open();
}

$(document).ready($("#btnSearch").on("click", function () {
    // debugger;
    $("#lstUsers").empty();
    if ($("#srhText").val() == "") {
        jAlert(ResourceUserAccessApprovalView.required_sSearchText, "Alert", function () {

            $("#srhText").focus();
        });
        $("#srhText").focus();
    }
    else {
        $.ajax({
            type: 'POST',
            url: ResourceUserAccessApprovalView.urlPath_GetSuperwiser,
            dataType: 'json',
            data: { Text: $("#srhText").val(), SearchCondition: SearchFlage },
            success: function (lstUsers) {
                if (lstUsers.lstSuperwiser.length > 0) {
                    //  debugger;

                    $.each(lstUsers.lstSuperwiser, function (i, user) {
                        //debugger;
                        $("#lstUsers").append('<option isclient=' + user.bClientUser + ' value=' + user.iUserID + '>' +
                            user.Name + '</option>');
                    });
                    $("#iUserID").val(lstUsers.iUserID);

                }
                else {
                    jAlert('Failed to retrieve Users.');
                }
            },
            error: function (ex) {
                jAlert('Failed to retrieve Users.' + ex);
            }
        });
    }
    //return false;
}))
var isclient = false;
$(document).ready($("#lstUsers").on("click", function (e) {
    var token = $("#formUserAccessApprovalView input[name=__RequestVerificationToken]").val();
    $("#sLoginName").val($(this).find(':selected').text());
    if ($(this).find(':selected').val() == $("#iUserID").val()) {

        jAlert(ResourceUserAccessApprovalView.display_CanNotAssignYouSelf);
        $("#sLoginName").val("");
        $("#iRoleID").data("kendoDropDownList").select(0);
        return false;
    }
    else {
        $("#iUserID").val($(this).find(':selected').val());
    }
    if ($("#iUserID").val() == undefined || $("#iUserID").val() == "") {
        jAlert('Please select a Users.');
        return;
    }

    $("#dialog").closest(".k-window-content").data("kendoWindow").close();
    $("#iRoleID").empty();
    //  debugger;
    isclient = $(this).find(':selected').attr('isclient');

    $.ajax({
        type: 'POST',
        url: ResourceUserAccessApprovalView.urlPath_JsonGetRole,
        dataType: 'json',
        data: { __RequestVerificationToken: token, iAgentID: $("#iUserID").val(), IsclientUser: isclient },
        success: function (lstRole) {
            //debugger;
            if (lstRole.ERPRole != "") {
                $("#iRoleID").data("kendoDropDownList").dataSource.data(lstRole.oUserSetting);
                $("#iRoleID").data("kendoDropDownList").search(lstRole.ERPRole);
                OnRoleChange();
            }
            else {
                $("#iRoleID").data("kendoDropDownList").dataSource.data(lstRole.oUserSetting);
                $("#iRoleID").data("kendoDropDownList").select(0);
                OnRoleChange();
            }

        },
        error: function (ex) {
            jAlert('Failed to retrieve Users.' + ex);
        }
    });

}))

function OnRoleChange() {
    var token = $("#formUserAccessApprovalView input[name=__RequestVerificationToken]").val();
    var iroleID = $("#iRoleID").val() == "" ? "0" : $("#iRoleID").val();
    var iUserID = $("#iUserID").val();
    $.LoadingOverlay("show");
    $.ajax({
        type: 'Post'
        , url: ResourceUserAccessApprovalView.urlPath_MakeTree
        , data: { __RequestVerificationToken: token, iAgentID: iUserID, iRoleID: iroleID, IsCLientUser: isclient }
        , dataType: "json"
        , success: function (result) {
            $.LoadingOverlay("hide");
            if (iroleID == "0") { $("#treeview").html(""); return false; }
            if ($("#treeview").html() != "") {
                $("#treeview").data("kendoTreeView").dataSource.data(result.treeData);
                var treeView = $("#treeview").data("kendoTreeView");

                for (var i = 0; i < result.UserMapData.length; i++) {

                    for (var j = 0; j < treeView.dataSource.view().length; j++) {

                        var dataSource = treeView.dataSource;

                        var dataItem = dataSource.get((result.UserMapData[i].ClientID + "|1"));
                        var node = treeView.dataSource.getByUid(dataItem.uid);
                        if (node) {
                            node.set("checked", true);
                        }
                        else {
                            node.set("checked", false);
                        }

                        var dataItem = dataSource.get((result.UserMapData[i].ProcessID + "|2"));
                        var node = treeView.dataSource.getByUid(dataItem.uid);
                        if (node) {
                            node.set("checked", true);
                        }
                        else {
                            node.set("checked", false);
                        }
                    }
                }
            }
            else {
                $("#treeview").kendoTreeView({
                    animation: false,
                    checkboxes: {
                        checkChildren: false
                    },
                    dataSource: result.treeData
                    , check: onCheck
                });
                var treeView = $("#treeview").data("kendoTreeView");

                for (var i = 0; i < result.UserMapData.length; i++) {

                    for (var j = 0; j < treeView.dataSource.view().length; j++) {

                        var dataSource = treeView.dataSource;

                        var dataItem = dataSource.get((result.UserMapData[i].ClientID + "|1"));
                        var node = treeView.dataSource.getByUid(dataItem.uid);
                        if (node) {
                            node.set("checked", true);
                        }
                        else {
                            node.set("checked", false);
                        }

                        var dataItem = dataSource.get((result.UserMapData[i].ProcessID + "|2"));
                        var node = treeView.dataSource.getByUid(dataItem.uid);
                        if (node) {
                            node.set("checked", true);
                        }
                        else {
                            node.set("checked", false);
                        }
                    }
                }

            }
            $(".k-in").css("border-left-width: 10px;");
        },
        error: function (err) {
            $.LoadingOverlay("hide");
        }
    });
}

function OnRoleDropdownChange() {
    $.LoadingOverlay("show");
    var token = $("#formUserAccessApprovalView input[name=__RequestVerificationToken]").val();
    var iroleID = $("#iRoleID").val() == "" ? "0" : $("#iRoleID").val();
    var iUserID = $("#iUserID").val();

    $.ajax({
        type: 'Post'
        , url: ResourceUserAccessApprovalView.urlPath_MakeTree
        , data: { __RequestVerificationToken: token, iAgentID: iUserID, iRoleID: iroleID, IsCLientUser: isclient }      //, data: JSON.stringify({ UserID: UserIDVal, Password: PasswordVal, EmailID: EmailIDVal, MailServerTypeID: MailServerTypeIDVal, AutoDiscoveryPath: AutoDiscoveryPathVal, FolderType: FolderTypeVal, LotusPath: LotusPathVal,NFSFILEPath:NFSFILEPathVal })
        , dataType: "json"
        , success: function (result) {
            $.LoadingOverlay("hide");
            if (iroleID == "0") { $("#treeview").html(""); return false; }
            if ($("#treeview").html() != "") {

            }
            else {
                $("#treeview").kendoTreeView({
                    animation: false,
                    checkboxes: {
                        checkChildren: false
                    },
                    dataSource: result.treeData
                    , check: onCheck
                });
                var treeView = $("#treeview").data("kendoTreeView");

                for (var i = 0; i < result.UserMapData.length; i++) {

                    for (var j = 0; j < treeView.dataSource.view().length; j++) {

                        var dataSource = treeView.dataSource;

                        var dataItem = dataSource.get((result.UserMapData[i].ClientID + "|1"));
                        var node = treeView.dataSource.getByUid(dataItem.uid);
                        if (node) {
                            node.set("checked", true);
                        }
                        else {
                            node.set("checked", false);
                        }

                        var dataItem = dataSource.get((result.UserMapData[i].ProcessID + "|2"));
                        var node = treeView.dataSource.getByUid(dataItem.uid);
                        if (node) {
                            node.set("checked", true);
                        }
                        else {
                            node.set("checked", false);
                        }
                    }
                }

            }
            $(".k-in").css("border-left-width: 10px;");
        },
        error: function (err) {
            $.LoadingOverlay("hide");
        }
    });
}


function myobject() {
};
function onCheck(e) {
    var checkedNodes = [];
    var treeView = $("#treeview").data("kendoTreeView");
    var CureentTree = $("#treeview").data('kendoTreeView').dataItem(e.node);
    var Id = $("#treeview").data('kendoTreeView').dataItem(e.node).id;
    var uid = $("#treeview").data('kendoTreeView').dataItem(e.node).uid;
    var FlagRoot = Id.split('|')[1];
    getCheckedNodes(CureentTree, checkedNodes, FlagRoot, treeView);

}
function getCheckedNodes(nodes, checkedNodes, FlageRoot, treeView) {

    var node;
    if (FlageRoot == 1) {

        checkedNodes.push({ text: nodes.text, id: nodes.id });

        if (nodes.hasChildren) {
            for (var i = 0; i < nodes.children.view().length; i++) {
                node = nodes.children.view()[i];
                var dataItem = treeView.dataSource.get(node.id);
                var node_ = treeView.dataSource.getByUid(dataItem.uid);
                if (node_) {
                    if (nodes.checked == true) {
                        node_.set("checked", true);
                    }
                    else {
                        node_.set("checked", false);
                    }

                    checkedNodes.push({ text: node.text, id: node.id });
                }
            }

        }

    }

}

function InsertUserMapping() {
    var token = $("#formUserAccessApprovalView input[name=__RequestVerificationToken]").val();

    //if (GetFormValidate($('form'), event.srcElement.name)) {
    if (GetFormValidate($('form'), "UserMapping")) {
        var treeview = $("[data-role=treeview]").data("kendoTreeView");
        //var strJason =JSON.stringify(treeToJson(treeview.dataSource.view()));
        if (treeview != undefined && treeview.dataSource._data != undefined && treeview.dataSource._data != null) {
            var strJason = JSON.stringify(treeview.dataSource._data);
            $.LoadingOverlay("show");
            $.ajax({
                url: ResourceUserAccessApprovalView.urlPath_InsertUserMapping,      // Url.Action("InsertUserMapping")',
                type: 'POST',
                data: {
                    __RequestVerificationToken: token,
                    iAgentID: $("#iUserID").val(),
                    iRoleID: $("#iRoleID").val(),
                    EffectiveDate: $("#dEffectiveDate").val(),
                    Disable: $("#bDisabled").is(":checked"),
                    JsonString: JSON.stringify(treeview.dataSource._data)

                },
                datatype: "json",
                success: function (result) {
                    $.LoadingOverlay("hide");
                    if (result.success == true) {
                        // debugger;
                        if (result.lstUserApproval != "") {
                            $("#divUserMapping").css("display", "none");
                            $("#divRequestApprover").css("display", "block");

                            var grid = $('#gvUserApproval').getKendoGrid();
                            grid.dataSource.data(result.lstUserApproval);
                            grid.refresh();

                        }
                        else {
                            var treeview = $("[data-role=treeview]").data("kendoTreeView");
                            treeview.dataSource.data([]);
                            $("#sLoginName").val("");
                            $("#iRoleID").data("kendoDropDownList").dataSource.data([]);
                            $("#divUserMapping").css("display", "block");
                            $("#divRequestApprover").css("display", "none");
                            jAlert("Changes done successfully");
                            location.reload(); // added by manish dwivedi- for resolve bug no 318
                        }
                    }
                    else {
                        jAlert(result.message, "Alert", function () {

                            location.reload();
                        });

                        // location.reload(); // added by manish dwivedi- for resolve bug no 318
                    }

                },
                error: function (jqXhr, textStatus, errorThrown) {
                    $.LoadingOverlay("hide");
                    jAlert(errorThrown);
                }
            });
        }

    }
    else {
        $('form').submit();
    }
};


$("#Clear").on("click", function (e) {
    location.href = ResourceUserAccessApprovalView.urlPath_UserAccessApprovalView;
});

function treeToJson(nodes) {
    return $.map(nodes, function (n, i) {
        var result = { text: n.text, id: n.id, expanded: n.expanded, checked: n.checked };
        if (n.hasChildren)
            result.items = treeToJson(n.children.view());
        return result;
    });
}



function GetAndCancelRequestStatus(RequestId) {

    var token = $("#formUserAccessApprovalView input[name=__RequestVerificationToken]").val();
    var Data = { FromDate: $("#FromDate").val(), ToDate: $("#ToDate").val(), __RequestVerificationToken: token };
    if (RequestId != undefined)
        Data = { FromDate: $("#FromDate").val(), ToDate: $("#ToDate").val(), RequestID: RequestId, __RequestVerificationToken: token }
    $.LoadingOverlay("show");
    $.ajax({
        type: 'POST',
        url: ResourceUserAccessApprovalView.urlPath_GetAndCancelRequestStatus,
        dataType: 'json',
        data: Data,
        success: function (result) {
            $.LoadingOverlay("hide");
            if (result.ID != null) {
                jAlert(ResourceUserAccessApprovalView.display_RequestCanceledSuccessfully);
            }

            var grid = $('#gvRequestStatus').getKendoGrid();
            grid.dataSource.data(result.lstReqStatus);
            grid.refresh();

            var grid = $("#gvRequestStatus").data("kendoGrid");

            var gridData = grid.dataSource.view();
            for (var i = 0; i < gridData.length; i++) {
                var currentUid = gridData[i].uid;
                if (gridData[i].Cancelable == "0") {
                    var currenRow = grid.table.find("tr[data-uid='" + currentUid + "']");
                    var editButton = $(currenRow).find(".k-primary");
                    editButton.hide();
                }
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $.LoadingOverlay("hide");
            jAlert(textStatus, errorThrown);
        }
    });
}

function CancelRequest(e) {
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    //var row = $(e.srcElement).closest("tr");
    //var grid = $(e.srcElement).closest("[data-role=grid]").data("kendoGrid");
    //dataItem = grid.dataItem(row);
    GetAndCancelRequestStatus(dataItem.RequestId);

}

$(document).ready(function () {
    $("#tabstrip").kendoTabStrip({
        activate: onActivate,
        animation: {
            open: {
                effects: "fadeIn"
            }
        }
    });
});

function onActivate(e) {
    //

    var a = $(e.item)[0].id;
    //$(e.item).find("> .k-link").text();
    if (a == "tap3") {
        fillRequestForApprovalList();
    }

}

function fillRequestForApprovalList(RequestIds, RequestTypeIDs, RequestTypes, ApprovalLevels, isApprove) {

    var token = $("#formUserAccessApprovalView input[name=__RequestVerificationToken]").val();

    var Data = {};
    if (RequestIds != undefined) {
        Data = { RequestIds: RequestIds, RequestTypeIDs: RequestTypeIDs, RequestTypes: RequestTypes, ApprovalLevels: ApprovalLevels, isApproved: isApprove, __RequestVerificationToken: token }
    }
    else {
        Data = { RequestIds: '', RequestTypeIDs: '', RequestTypes: '', ApprovalLevels: '', isApproved: false, __RequestVerificationToken: token }
    }
    var urlpath = ResourceUserAccessApprovalView.urlPath_GetRequestForApproval;
    
    $.ajax({
        type: 'POST',
        async: false,
        url: ResourceUserAccessApprovalView.urlPath_GetRequestForApproval,
        dataType: 'json',
        data: Data,
        success: function (result) {
            $.LoadingOverlay("hide");
            if (result.Meassage != "") {
                jAlert(result.Meassage);
            }
            var grid = $('#gvRequestApproval').getKendoGrid();
            grid.dataSource.data(result.lstReqApproval);
            grid.refresh();
            $("#checkAll").attr("checked", false);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $.LoadingOverlay("hide");
            jAlert(textStatus, errorThrown);
        }

    });
}

function CheckChanged(ctrl, Id) {
    //

    var isAllchecked = true;
    var grid = $("#gvRequestApproval").data("kendoGrid");
    var DataSource = grid.dataSource;
    for (var i = 0; i < DataSource.total(); i++) {
        if (DataSource.data()[i].RequestTypeID == Id) {
            DataSource.data()[i].set("bSelected", ctrl.checked);
        }
        if (DataSource.data()[i].bSelected == false)
            isAllchecked = false;
    }
    $("#checkAll").attr("checked", isAllchecked);
}

$(document).on("click", "#checkAll", function () {
    //

    var grid = $("#gvRequestApproval").data("kendoGrid");
    var DataSource = grid.dataSource;
    for (var i = 0; i < DataSource.total(); i++) {
        DataSource.data()[i].set("bSelected", this.checked);
    }
    $("#gvRequestApproval tbody input:checkbox").attr("checked", this.checked);
});

function ApproveRequest(e) {
    $.LoadingOverlay("show");
    var row = $(event.srcElement).closest("tr");
    var grid = $(event.srcElement).closest("[data-role=grid]").data("kendoGrid");
    dataItem = grid.dataItem(row);
    fillRequestForApprovalList(dataItem.RequestId, dataItem.RequestTypeID, dataItem.RequestType, dataItem.ApprovalLevel, true);
}
function RejectRequest(e) {
    $.LoadingOverlay("show");
    var row = $(event.srcElement).closest("tr");
    var grid = $(event.srcElement).closest("[data-role=grid]").data("kendoGrid");
    dataItem = grid.dataItem(row);
    fillRequestForApprovalList(dataItem.RequestId, dataItem.RequestTypeID, dataItem.RequestType, dataItem.ApprovalLevel, false);
}

function ApproveRejectBulkRequest(isApproved) {
    //
    $.LoadingOverlay("show");
    var RequestIds = ""; var RequestTypeIDs = ""; var RequestTypes = ""; var ApprovalLevels = "";
    var grid = $("#gvRequestApproval").data("kendoGrid");
    var DataSource = grid.dataSource;
    for (var i = 0; i < DataSource.total(); i++) {
        if (DataSource.data()[i].bSelected == true) {
            RequestIds = RequestIds + DataSource.data()[i].RequestId + ",";
            RequestTypeIDs = RequestTypeIDs + DataSource.data()[i].RequestTypeID + ",";
            RequestTypes = RequestTypes + DataSource.data()[i].RequestType + ",";
            ApprovalLevels = ApprovalLevels + DataSource.data()[i].ApprovalLevel + ",";
        }
    }
    if (RequestIds != "")
        fillRequestForApprovalList(RequestIds, RequestTypeIDs, RequestTypes, ApprovalLevels, isApproved);

}


function getData(ddl) {
    //
    var row = $(event.srcElement).closest("tr");
    var grid = $(event.srcElement).closest("[data-role=grid]").data("kendoGrid");
    dataItem = grid.dataItem(row);

    var iUserId = 0;
    var iClientId = 0;
    var iProcessId = 0;
    var iFlag = 0;
    if (dataItem.RequestType == 1) {
        iUserId = dataItem.Id;
        iFlag = 1;

    }
    else if (dataItem.RequestType == 2) {
        iUserId = dataItem.Id;
        iProcessId = dataItem.Id;
        iUserId = $("#iUserID").val();
        iFlag = 2;
    }
    else if (dataItem.RequestType == "5") {
        iClientId = dataItem.Id;
        iProcessId = dataItem.Id;
        iUserId = $("#iUserID").val();
        iFlag = 5;
    }
    else if (dataItem.RequestType == "4" || dataItem.RequestType == "3") {
        iFlag = 4;
        iClientId = dataItem.Id;
        iProcessId = dataItem.Id;
        iUserId = $("#iUserID").val();
    }
    if (ddl == "L2")
        iFlag = 5;
    return { UserId: iUserId, ClientId: iClientId, ProcessId: iProcessId, Flag: iFlag };
}
function onSelectL1(e) {
    //
    var selectedValue = this.dataItem(e.item.index());
    if (dataItem != null) {
        dataItem.SelectedApproverL1 = selectedValue;
    }
}
function onSelectL2(e) {
    //
    var selectedValue = this.dataItem(e.item.index());
    if (dataItem != null) {
        dataItem.SelectedApproverL2 = selectedValue;
    }
}

function CancelMappingRequest() {

    var token = $("#formUserAccessApprovalView input[name=__RequestVerificationToken]").val();
    var reqId = 0;
    var DataSource = $("#gvUserApproval").data("kendoGrid").dataSource;
    if (DataSource != undefined && DataSource.data()[0] != undefined) {
        reqId = DataSource.data()[0].RequestId;
        $.ajax({
            type: 'POST',
            url: ResourceUserAccessApprovalView.urlPath_CancelMappingRequest,
            dataType: 'json',
            data: { RequestId: reqId, __RequestVerificationToken: token },
            success: function (result) {

                var treeview = $("[data-role=treeview]").data("kendoTreeView");
                treeview.dataSource.data([]);
                $("#sLoginName").val("");
                $("#iRoleID").data("kendoDropDownList").dataSource.data([]);
                $("#divUserMapping").css("display", "block");
                $("#divRequestApprover").css("display", "none");
                jAlert("Request cancelled successfully.");
            },
            error: function (jqXHR, textStatus, errorThrown) {
                //
                jAlert(textStatus, errorThrown);
            }

        });
    }

}

function UserMappingapproval() {
    var Req = 'OK';
    var token = $("#formUserAccessApprovalView input[name=__RequestVerificationToken]").val();
    var grid = $("#gvUserApproval").data("kendoGrid");
    var DataSource = grid.dataSource;
    for (var i = 0; i < DataSource.total(); i++) {
        if (DataSource.data()[i].SelectedApproverL1.iApproverId == "0") {
            Req = 'NO';
        }
    }
    if (Req == 'OK') {


        var strJason = JSON.stringify(DataSource._data);
        $.LoadingOverlay("show");
        $.ajax({
            type: 'POST',
            url: ResourceUserAccessApprovalView.urlPath_UserMappingapproval,
            dataType: 'json',
            data: { JsonString: strJason, __RequestVerificationToken: token },
            success: function (result) {
                $.LoadingOverlay("hide");
                var treeview = $("[data-role=treeview]").data("kendoTreeView");
                treeview.dataSource.data([]);
                $("#sLoginName").val("");
                $("#iRoleID").data("kendoDropDownList").dataSource.data([]);
                $("#divUserMapping").css("display", "block");
                $("#divRequestApprover").css("display", "none");
                jAlert('Request raised successfully', 'Alert', function (r) {
                    if (r) {
                        window.location.href = ResourceLayout.partialURL + "UserAccessApprovalView";
                    }
                });
                //  jAlert("Request raised successfully");
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $.LoadingOverlay("hide");
                jAlert(textStatus, errorThrown);
            }

        });
    }
    else {
        jAlert(ResourceUserAccessApprovalView.required_ApproverLevel);
    }
}

/* --- User Management Search View -----------*/



function onDataBound(e) {
    $(".k-grid-Edit").addClass("k-primary k-grid-Edit");
    $(".k-grid-Edit").find("span").addClass("k-icon k-edit");
}
function editClient(e) {

    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    $.ajax({
        type: "POST"
        , url: ResourceUserManagementSearchView.urlPath_SetTagID
        , data: JSON.stringify({ iTagID: dataItem.iTagID })
        , contentType: "application/json"
        , success: function (result) {
            if (result == "OK") {
                window.location.href = ResourceUserManagementSearchView.urlPath_Index
            }
        }
    });
}

//$(document).keypress(function (e) {

//    var key = e.which;

//    if (key == 13)  // the enter key code
//    {
//        $("#btnSearch").trigger("click");
//        return false;
//    }
//});


function OnClicSearch_UMSV() {
    $("#gridSearchPage").html("");
    $("#gridSearchPage").show();
    var columnslist = [];

    var validator = $("#formUserManagementSearchView").kendoValidator().data("kendoValidator");
    if (validator.validate()) {
        var str = $("#sSearchText").val();
        var count = str.length;
        if (count >= 3) {
            token = $("#formUserManagementSearchView input[name=__RequestVerificationToken]").val();
            $.LoadingOverlay("show");
            $.ajax({
                type: 'POST',
                url: ResourceUserManagementSearchView.urlPath_GetSearchUserList,
                dataType: 'json',
                data: { __RequestVerificationToken: token, Text: $("#sSearchText").val(), SearchCondition: SearchFlage },
                success: function (result) {
                    $.LoadingOverlay("hide");
                    if (result != null) {

                        columnslist.push({ field: "iUserID", edit: false, hidden: true, width: 10 });
                        columnslist.push({ field: "Name", edit: false, width: 350, title: "User Name" });
                        columnslist.push({ command: { text: "Edit", click: FillSearch } });

                        var dataSource = new kendo.data.DataSource({
                            pageSize: 10,
                            data: result,
                            //autoSync: true,
                            schema: {
                                model: {
                                    Id: "iUserID",
                                    fields: {
                                        iUserID: { editable: false },
                                        Name: { editable: false },

                                    }

                                }
                            }
                        });

                        $("#gridSearchPage").kendoGrid({
                            columns: columnslist,
                            dataSource: dataSource,
                            width: 500,
                            height: 500,
                            editable: true,
                            //selectable: "multiple row",
                            dataBound: onDataBound,
                            autoBind: true
                            , pageable: {
                                refresh: true,
                                pageSizes: true,
                                buttonCount: 10,
                                pageSizes: [5, 10, 20, 50, 100],
                            }
                        });
                    }
                    else {
                        jAlert("No Record Found");
                        columnslist.push({ field: "iUserID", edit: false, hidden: true, width: 10 });
                        columnslist.push({ field: "Name", edit: false, width: 250, title: "User Name" });

                        $("#gridSearchPage").kendoGrid({
                            columns: columnslist,
                            dataSource: [],
                            height: 500,
                            editable: true,
                            //selectable: "multiple row",
                            dataBound: onDataBound,
                            autoBind: true
                            , pageable: {
                                refresh: true,
                                pageSizes: true,
                                buttonCount: 10,
                                pageSizes: [5, 10, 20, 50, 100],
                            }
                        });

                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {

                    $.LoadingOverlay("hide");
                    alert(textStatus, errorThrown);

                }
            });
        }
        else {
            $("#gridSearchPage").hide();
            jAlert(ResourceUserManagementSearchView.required_ThreeCharsForSearch);
        }
    }
    else {
        return false;
    }
}
//);

$(document).ready($("#lstUsers").click(function () {

    //window.location.href = ResourceUserManagementSearchView.urlPath_FillSelectedUserDetail + "?UserID=" + $(this).find(':selected').val();
    return false;
}))

function FillSearch(e) {
    //var grid = $("#gridSearchPage").getKendoGrid();
    //var dataItem1 = grid.dataItem($(e.currentTarget).closest("tr"));
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var uid1 = dataItem.iUserID;
    $.ajax({
        type: "POST"
        , url: ResourceUserManagementSearchView.urlPath_SetUserID
        , data: { UserID: dataItem.iUserID }
        , dataType: 'json'
        , success: function (result) {
            if (result == "1") {
                window.location.href = ResourceUserManagementSearchView.urlPath_FillSelectedUserDetail;
            }
        }
    });

}
function DeleteUser(e) {
    var token = $("#formUserManagementSearchView input").val();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    jConfirm(ResourceUserManagementSearchView.confirm_DeleteUser, 'Confirmation', function (r) {
        //
        if (r) {
            e.preventDefault();
            window.location.href = ResourceUserManagementSearchView.urlPath_DeleteUser + "?UserID=" + dataItem.iUserID;
        }
        else {
            return false;
        }
    });

}


//$(document).ready(function () {
//    $(":submit").click(function () {
//        $(":submit").removeClass('clickedButton');
//        $(this).addClass('clickedButton');
//    });

//});
function OnClickNew() { window.location.href = ResourceUserManagementSearchView.urlPath_Index; }

function OnClickCreateUser() {
    var validator = $("#form1").kendoValidator().data("kendoValidator");
    if (validator.validate()) {
        kendo.ui.progress($('#form1'), true);
        var SupID = $("#sSupervisorName").val().trim();
        if (SupID == "") {
            jAlert("Please Enter Supervisor Name", "Alert", function () {

                $("#sSupervisorName").focus();
            });
            $("#sSupervisorName").focus();
            kendo.ui.progress($('#form1'), false);
            return false;
        }
        var UserManagementViewModel = {};
        UserManagementViewModel.sLoginName = $("#sLoginName").val();
        UserManagementViewModel.iEmployeeID = $("#iEmployeeID").val();
        UserManagementViewModel.sFirstName = $("#sFirstName").val();
        UserManagementViewModel.sMiddleName = $("#sMiddleName").val();
        UserManagementViewModel.sLastName = $("#sLastName").val();
        UserManagementViewModel.iJobID = $("#iJobID").val();
        UserManagementViewModel.sEmail = $("#sEmail").val();
        UserManagementViewModel.iSupervisorID = $("#iSupervisorID").val();
        UserManagementViewModel.dDOJ = $("#dDOJ").val();
        UserManagementViewModel.iFacilityId = $("#iFacilityId").val();
        UserManagementViewModel.sSupervisorName = $("#sSupervisorName").val();
        UserManagementViewModel.iLOBID = $("#iLOBID").val();
        UserManagementViewModel.iSBUID = $("#iSBUID").val();
        UserManagementViewModel.bClientUser = $("input[name='RbtnsearchLAN']:checked").val() == 3 ? true : false;
        UserManagementViewModel.bDisabled = $('#bDisabled').is(':checked') == true ? true : false;
        UserManagementViewModel.bIsBot = $('#bIsBot').is(':checked') == true ? true : false;
        UserManagementViewModel.iUserID = $("#iUserID").val();
        UserManagementViewModel.bLanID = $("#bLanID").val();
        UserManagementViewModel.iUserLevel = $("#iUserLevel").val();
        UserManagementViewModel.iExistingRoleID = $("#iExistingRoleID").val();

        postUrl = ResourceUserView.urlPath_CreateUser;
        var token = $("#form1 input").val();
        $.LoadingOverlay("show");
        $.ajax({
            url: postUrl,
            type: 'POST',
            dataType: 'json',
            data: { 'objUserManagementViewModel': UserManagementViewModel },

            // data: {  'objWorkDefinitionViewModel': WorkDefinitionViewModel  },

            //   contentType: 'application/json; charset=utf-8',
            //{ __RequestVerificationToken: token, objWorkDefinitionViewModel: JSON.stringify(WorkDefinitionViewModel) },
            success: function (result) {
                $.LoadingOverlay("hide");
                var ConfMsg = result.split(',')[result.split(',').length - 1];
                if (ConfMsg == "OK") {

                    jAlert(result.split(',')[0], 'Alert', function () {
                        window.location.href = ResourceUserView.urlPath_Index;
                        kendo.ui.progress($('#form1'), false);
                    });

                }
                else {
                    jAlert(result.split(',')[0]);
                    kendo.ui.progress($('#form1'), false);
                }

            },
            error: function (err) {
                kendo.ui.progress($('#form1'), false);
                $.LoadingOverlay("hide");
            }
        });
    }
    else { return false; }
}