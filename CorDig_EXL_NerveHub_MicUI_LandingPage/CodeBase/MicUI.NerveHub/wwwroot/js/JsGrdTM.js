/*
     * FileName: jsGrdTM.js
     * ClassName: jsGrdTM
     * Purpose: This file contains all the TMSetup Free field scripts
     * Description:  
     * Created By: Deepak Kumar Maurya
     * Created Date: 17 July 2015
     * Modified By: 
     * Modified Date:
     * Modified Purpose:
     * Modified Description: 
 */
//var brands;
//var models;
//var ValidationArray;
//var BindData;
//var _DataType;
//var test;
//var LastRow;
//var RowNumber=1;
//$(document).ready(function () {
//    $.ajax({
//        url: '/TMSetup/FillObjectType',
//        type: 'Post',
//        cache: false,
//        success: function (data) {
//            brands = data;
//        }
//    });
//    $.ajax({
//        url: '/TMSetup/FillObjectDataType_Data',
//        type: 'Post',
//        cache: false,
//        success: function (data) {
//            models = data.ListBind;
//        }
//    });
//    $.ajax({
//        url: '/TMSetup/FillObjectDataType_DataValidation',
//        type: 'Post',
//        cache: false,
//        success: function (data) {
//            ValidationArray = data.ListBindValidation;
//        }
//    });

//    $.ajax({
//        url: '/TMSetup/FillGridTeamList',
//        type: 'Post',
//        cache: false,
//        success: function (data) {
//            BindData = data;

//            var val = data.FeeFieldList;
//            if (val.length > 0) {
//                $("#grid").kendoGrid({
//                    dataSource: {
//                      //data:data.FeeFieldList,
//                        transport: {
//                            read: function (e) {
//                                if (data.FeeFieldList[0].FreeName != "FR")
//                                    e.success(data.FeeFieldList);
//                            },
//                            update: function (e) {
                                
//                                var dataItem = e.data, i, j;
//                                e.success(dataItem);
//                            },
//                            destroy: function (e) {

//                                e.success(e.data);
//                            },
//                            create: function (e) {
                                
//                                var item = e.data;
//                                    if ($("#isChoiceFilled").val() == "true") {
//                                       // RowNumber = 1;
//                                        e.success(item);
//                                    }
//                                    else {
//                                        jAlert('Please Define atleast one choice for the free field');
//                                      //  RowNumber = 1;
//                                        return false;
//                                    }
//                            }

//                        },
//                        schema: {
//                            model: {
//                                id: "FreeName",
//                                fields: {
//                                    FreeFieldID: { editable: false }, // the id field is not editable
//                                    FreeName: { validation: { required: true } },
//                                    FreeDescription: { validation: { required: true } },
//                                    iControlTypeID: { type: "string" },
//                                    DataTypeValue: { type: "string" },
//                                    ValidateId: { type: "string" },
//                                    IsRequired: { type: 'boolean' },
//                                    IsVasible: { type: 'boolean' },
//                                    IsCTC: { type: 'boolean' },
//                                    IsAuditEmail: { type: 'boolean' },
//                                    Disabled: { type: 'boolean' },
//                                    rowNumber: { type: "string" }
//                                }
//                            }
//                        }
//                    },
//                    // use inline mode so both dropdownlists are visible (required for cascading)
//                    columns: [
//                     { title: "Name", field: "FreeName" },
//                    { title: "Description", field: "FreeDescription" },
//                    {
//                        // the brandId column
//                        title: "Control Type",
//                        field: "iControlTypeID", // bound to the brandId field
//                        template: "#= brandName(iControlTypeID) #", // the template shows the name corresponding to the brandId field
//                        editor: function (container) { // use a dropdownlist as an editor
//                            // create an input element with id and name set as the bound field (brandId)
//                            var input = $('<input id="iControlTypeID" name="iControlTypeID">');
//                            // append to the editor container
//                            input.appendTo(container);

//                            // initialize a dropdownlist
//                            input.kendoDropDownList({
//                                optionLabel: "Select Control Type",
//                                dataTextField: "sControlType",
//                                dataValueField: "iControlTypeID",
//                                select: onSelect,
//                                dataSource: brands // bind it to the brands array
//                            }).appendTo(container);
//                            var input1 = $('<a onclick="return OpenChoice(test);"  style= "cursor: pointer;  display: none; color: #829094;" id="linkChoice">Add Choice</a>');
//                            input1.appendTo(container);
//                            var input1 = $('<a onclick="return OpenChoice(test);"  style= "cursor: pointer; display: none; color: #829094;" id="linkModifyChoice">Modify Choice</a>');
//                            input1.appendTo(container);
//                        }
//                    },
//                    {
//                        //The modelId column
//                        title: "Data Type",
//                        field: "DataTypeValue",  // bound to the modelId field
//                        template: "#= modelName(DataTypeValue) #", //the template shows the name corresponding to the modelId field
//                        editor: function (container) { // use a dropdownlist as an editor
//                            var input = $('<input id="Value" name="DataTypeValue">');
//                            input.appendTo(container);
//                            input.kendoDropDownList({
//                                optionLabel: "Select Data Type",
//                                dataTextField: "DataTypeText",
//                                dataValueField: "DataTypeValue",
//                                select: onSelectDataType,
//                                cascadeFrom: "iControlTypeID", // cascade from the brands dropdownlist
//                                dataSource: models // bind it to the models array
//                            }).appendTo(container);
//                            var input1 = $('<input  id="txtmax_length" type="text" style= "cursor: pointer;display: none;" >');
//                            input1.appendTo(container);
//                        }
//                    },
//                    {
//                        //The Validation column
//                        title: "Validation",
//                        field: "ValidateId",  // bound to the ValidateId field
//                        template: "#= ValidateName(ValidateId) #", //the template shows the name corresponding to the ValidateId field
//                        editor: function (container) { // use a dropdownlist as an editor
//                            var input = $('<input id="ValidateId" name="ValidateId">');
//                            //'<input required data-text-field="ValidateText" data-value-field="ValidateId"/>');//'<input id="ValidateId" name="ValidateId">');
//                            input.appendTo(container);
//                            input.kendoDropDownList({
//                                optionLabel: "Select Validation",
//                                dataTextField: "ValidateText",
//                                dataValueField: "ValidateId",
//                                cascadeFrom: "iControlTypeID", // cascade from the brands dropdownlist
//                                dataSource: ValidationArray // bind it to the ValidationArray array
//                            }).appendTo(container);
//                        }
//                    },
//                     {
//                         title: 'Is Required',
//                         field: 'IsRequired',
//                         template: '<input name="IsRequired" type="checkbox" disabled #=IsRequired === true ? "checked" : ""# />'
//                     },
//{
//    title: 'Is Visible',
//    field: 'IsVasible',
//    template: '<input name="IsVasible" type="checkbox" disabled #=IsVasible === true ? "checked" : ""# />'
//},
//{
//    title: 'Is CTC',
//    field: 'IsCTC',
//    template: '<input name="IsCTC" type="checkbox" disabled #=IsCTC === true ? "checked" : ""# />'
//},
//{
//    title: 'Is Audit E-Mail',
//    field: 'IsAuditEmail',
//    template: '<input name="IsAuditEmail" type="checkbox" disabled #=IsAuditEmail === true ? "checked" : ""# />'
//},
//{
//    title: 'Disable',
//    field: 'Disabled',
//    template: '<input name="Disabled" type="checkbox" disabled #=Disabled === true ? "checked" : ""# />'
//},
//                    { command: "edit", title: "&nbsp;", width: 100 },
//                    { command: "destroy", title: "&nbsp;", width: 100 }
//                    ],
//                    dataBound: function () {
//                          var rows = this.items();
//                         $(rows).each(function () {
//                            var index = $(this).index() + 1;
//                            LastRow = index;
//                        });
//                    },
//                    editable: {
//                        mode: "popup",
//                        createAt: "bottom"
//                    },
//                    //"inline",
//                    toolbar: ["create"],
//                    edit: function (e) {
                         
//                        // var er = e.rownumber();
//                        if (e.model.FreeFieldID == "") {
//                           // $("#isChoiceFilled").val(false);
//                            test = e.model.uid + ',' + 0;
                         
//                        }
//                        else {
//                            $("#isChoiceFilled").val(true);
//                            test = e.model.uid + ',' + e.model.FreeFieldID;
                            
//                        }

//                        if (e.model.FreeName != "")
//                        {
//                          //  $('.k-grid-cancel').css('display', 'none');
//                            $('.k-window-action').css('display', 'none');
//                            e.container.data("kendoWindow").title('Edit Free Field');
//                            switch (e.model.iControlTypeID) {
//                                case "6":
//                                    $("#linkModifyChoice").show();
//                                    $("#txtmax_length").hide();
//                                    $("#txtmax_length").val('0');
//                                    break;
//                                case "5":
//                                    $("#linkModifyChoice").show();
//                                    $("#txtmax_length").hide();
//                                    $("#txtmax_length").val('0');
//                                    break;
//                                case "4":
//                                    $("#linkModifyChoice").show();
//                                    $("#txtmax_length").show();
//                                    $("#txtmax_length").val('300');
//                                    document.getElementById('txtmax_length').readOnly = true;
//                                    break;
//                                case "9":
//                                    $("#linkModifyChoice").show();
//                                    $("#txtmax_length").show();
//                                    $("#txtmax_length").val('300');
//                                    document.getElementById('txtmax_length').readOnly = true;
//                                    break;
//                                case "8":
//                                    $("#linkModifyChoice").show();
//                                    $("#txtmax_length").hide();
//                                    $("#txtmax_length").val('0');
//                                    break;
//                                case "1":
//                                    if (e.model.DataTypeValue == "Character") {
//                                        $("#txtmax_length").show();
//                                        $("#txtmax_length").val('300');
//                                        document.getElementById('txtmax_length').readOnly = true;
//                                    }
//                                    else {
//                                        $("#txtmax_length").hide();
//                                        $("#txtmax_length").val('0');
//                                    }
//                                    break;
//                                case "10":
//                                    if (e.model.DataTypeValue == "Character") {
//                                        $("#txtmax_length").show();
//                                        $("#txtmax_length").val('300');
//                                        document.getElementById('txtmax_length').readOnly = true;
//                                    }
//                                    else {
//                                        $("#txtmax_length").hide();
//                                        $("#txtmax_length").val('0');
//                                    }
//                                    break;
//                                default:
//                                    $("#linkModifyChoice").hide();
//                                    $("#txtmax_length").hide();
//                                    $("#txtmax_length").val('0');
//                                    break;
//                            }

//                        }
//                        else {

//                            e.container.data("kendoWindow").title('Add New Free Field');

//                        }
//                    }

//                });
//            }
//        }
//    });
//});
//function onSelect(e) {

//    var dataItem = this.dataItem(e.item.index() + 1);
//    switch (dataItem.sControlType) {
//        case "TextBox":
//            _DataType = "TextBox";
//            $("#linkChoice").hide();
//            $("#txtmax_length").hide();
//            $("#txtmax_length").val('0');
//            $("#isChoiceFilled").val(true);
//            break;
//        case "CheckBox":
//            $("#linkChoice").hide();
//            $("#txtmax_length").hide();
//            $("#txtmax_length").val('0');
//            $("#isChoiceFilled").val(true);
//            break;
//        case "DropDownList":
//            $("#linkChoice").show();
//            $("#txtmax_length").hide();
//            $("#txtmax_length").val('0');
//            $("#isChoiceFilled").val(false);
//            break;
//        case "Label":
//            $("#linkChoice").hide();
//            $("#txtmax_length").hide();
//            $("#txtmax_length").val('0');
//            $("#isChoiceFilled").val(true);
//            break;
//        case "ListBox":
//            $("#linkChoice").show();
//            $("#txtmax_length").hide();
//            $("#txtmax_length").val('0');
//            $("#isChoiceFilled").val(false);
//            break;
//        case "CheckBoxList":
//            $("#linkChoice").show();
//            $("#txtmax_length").show();
//            $("#txtmax_length").val('300');
//            document.getElementById('txtmax_length').readOnly = true;
//            $("#isChoiceFilled").val(false);
//            break;
//        case "ListBox - MultiSelect":
//            $("#linkChoice").show();
//            $("#txtmax_length").show();
//            $("#txtmax_length").val('300');
//            document.getElementById('txtmax_length').readOnly = true;
//            $("#isChoiceFilled").val(false);
//            break;
//        case "RadioButton":
//            $("#linkChoice").hide();
//            $("#txtmax_length").hide();
//            $("#txtmax_length").val('0');
//            $("#isChoiceFilled").val(true);
//            break;
//        case "RadioButtonList":
//            $("#linkChoice").show();
//            $("#txtmax_length").hide();
//            $("#txtmax_length").val('0');
//            $("#isChoiceFilled").val(false);
//            break;
//        case "TextBox - Multiline":
//            _DataType = "TextBox - Multiline";
//            $("#linkChoice").hide();
//            $("#txtmax_length").hide();
//            $("#txtmax_length").val('0');
//            $("#isChoiceFilled").val(true);
//            break;
//        default:
//            $("#linkChoice").hide();
//            $("#txtmax_length").hide();
//            $("#txtmax_length").val('0');
//            $("#isChoiceFilled").val(true);
//            break;
//    }

//};
//function onSelectDataType(e) {


//    var dataItem = this.dataItem(e.item.index() + 1);
//    switch (_DataType) {
//        case "TextBox":
//            switch (dataItem.DataTypeValue) {
//                case "Character":
//                    $("#txtmax_length").show();
//                    $("#txtmax_length").val('300');
//                    document.getElementById('txtmax_length').readOnly = true;
//                    break;
//                default:
//                    $("#txtmax_length").hide();
//                    $("#txtmax_length").val('0');
//                    break;
//            }
//            break;
//        case "TextBox - Multiline":
//            switch (dataItem.DataTypeValue) {
//                case "Character":
//                    $("#txtmax_length").show();
//                    $("#txtmax_length").val('300');
//                    document.getElementById('txtmax_length').readOnly = true;
//                    break;
//                default:
//                    $("#txtmax_length").hide();
//                    $("#txtmax_length").val('0');
//                    break;
//            }
//            break;
//    }


//};
//function OpenChoice(val) {

//    var IDval = val.split(",");
//    $("#hdnOperationalId").val(val[0]);
//    $("#hdnFreeFieldlId").val(val[1]);
//    var uid = IDval[0];
//    var choiceid = IDval[1];
//    $.ajax({
//        url: '/TMSetup/KeepID',
//        // url: '/QualityManagement/TMSetup/DisplayChoiceData',
//        data: { UIID: uid, ChoiceID: choiceid },
//        type: 'Post',
//        cache: false,
//        success: function (data) {
//            $("#GridChoice").data("kendoGrid").dataSource.read();

//            var accessWindow = $("#OpenPartialPopupChoice").kendoWindow({
//               // actions: ["Close"],
//                draggable: true,
//                height: "400px",
//                modal: true,
//                content: data.Choice,//this little thing does magic 
//                resizable: false,
//                title: "Fill Choice",
//                width: "500px",
//                visible: false
//            }).data("kendoWindow").center().open();
//        }
//    });
//}
//function brandName(iControlTypeID) {
//    if (brands != null) {
//        for (var i = 0; i < brands.length; i++) {
//            if (brands[i].iControlTypeID == iControlTypeID) {
//                return brands[i].sControlType;
//            }
//        }
//    }
//    else {

//        $.ajax({
//            url: '/TMSetup/FillObjectType',
//            type: 'Post',
//            cache: false,
//            success: function (data) {
//                brands = data;

//                for (var i = 0; i < brands.length; i++) {
//                    if (brands[i].iControlTypeID == iControlTypeID) {
//                        return brands[i].sControlType;
//                    }
//                }
//            }
//        });

//    }
//}
//function modelName(DataTypeValue) {

//    if (models != null) {
//        for (var i = 0; i < models.length; i++) {
//            if (models[i].DataTypeValue == DataTypeValue) {
//                return models[i].DataTypeValue;
//            }
//        }
//    }
//    else {
//        try {
//            var ddlTeamID = "";
//            $.ajax({
//                url: '/TMSetup/FillObjectDataType_Data',
//                type: 'Post',
//                cache: false,
//                success: function (result) {
//                    models = result.ListBind;
//                    for (var i = 0; i < models.length; i++) {
//                        if (models[i].DataTypeValue == DataTypeValue) {
//                            return models[i].DataTypeValue;
//                        }
//                    }

//                }
//            });
//        }
//        catch (err) {
//            alert(err);
//        }

//    }

//}
//function ValidateName(ValidateId) {

//    if (ValidationArray != null) {
//        for (var i = 0; i < ValidationArray.length; i++) {
//            if (ValidationArray[i].ValidateId == (ValidateId == null ? 'NS' : ValidateId)) {
//                return ValidationArray[i].ValidateText;
//            }
//        }

//    }
//    else {


//        try {
//            var ddlTeamID = "";
//            $.ajax({
//                url: '/TMSetup/FillObjectDataType_DataValidation',
//                type: 'Post',
//                cache: false,
//                success: function (result) {
//                    ValidationArray = result.ListBindValidation;
//                    for (var i = 0; i < ValidationArray.length; i++) {
//                        if (ValidationArray[i].ValidateId == ValidateId) {
//                            return ValidationArray[i].ValidateText;
//                        }
//                    }

//                }
//            });
//        }
//        catch (err) {
//            alert(err);
//        }
//    }

//}