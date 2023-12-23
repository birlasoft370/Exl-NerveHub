
$.validator.defaults.ignore = "";

$.validator.unobtrusive.adapters.add('requiredfield', ['buttonkeys'], function (options) {
    options.rules['requiredfield'] = options.params;
    options.messages['requiredfield'] = options.message;
});
$.validator.addMethod("requiredfield", function (value, element, params) {
    if (params.buttonkeys == undefined || params.buttonkeys == "" || $(".clickedButton").prop("name") == undefined || params.buttonkeys.indexOf($(".clickedButton").prop("name")) > -1 || $(".clickedButton").prop("name") === "action:" + params.buttonkeys) {
        if (value == "undefined" || value == "")
            return false;
    }
    return true;
}, '');



$.validator.unobtrusive.adapters.add('dropdownrequired', ['isfirstdefault', 'buttonkeys'], function (options) {
    options.rules['dropdownrequired'] = options.params;
    options.messages['dropdownrequired'] = options.message;
});
$.validator.addMethod("dropdownrequired", function (value, element, params) {
    //alert(params.isfirstdefault);
    // 
    if (params.buttonkeys == undefined || params.buttonkeys == "" || $(".clickedButton").prop("name") == undefined || params.buttonkeys.indexOf($(".clickedButton").prop("name")) > -1 || $(".clickedButton").prop("name") === "action:" + params.buttonkeys) {
        if (value == "undefined")
            return false;
        else if (params.isfirstdefault && (value == "" || value == 0))
            return false;
        else if (!params.isfirstdefault && value == "")
            return false;
    }
    return true;
}, '');



$.validator.unobtrusive.adapters.add('formatecheck', ['mask', 'buttonkeys'], function (options) {
    options.rules['formatecheck'] = options.params;
    options.messages['formatecheck'] = options.message;
});
$.validator.addMethod("formatecheck", function (value, element, params) {
    //alert(params.mask);
    if (params.buttonkeys == undefined || params.buttonkeys == "" || $(".clickedButton").prop("name") == undefined || params.buttonkeys.indexOf($(".clickedButton").prop("name")) > -1 || $(".clickedButton").prop("name") === "action:" + params.buttonkeys) {
        if (value != null) {
            if (params.mask.length != value.length) {
                // Length mismatch.
                return false;
            }
            for (var i = 0; i < params.mask.length; i++) {
                // 
                if (!isNaN(params.mask.charAt(i)) && isNaN(value.charAt(i))) {
                    // Digit expected at this position.
                    return false;
                }
                if (params.mask[i] == '-' && value[i] != '-') {
                    // Spacing character expected at this position.
                    return false;
                }
            }
            return true;
        }
        return false;
    }
    return true;
}, '');



$.validator.unobtrusive.adapters.add('filterexpression', ['filtertype', 'validchar', 'invalidchar', 'buttonkeys'], function (options) {
    options.rules['filterexpression'] = options.params;
    options.messages['filterexpression'] = options.message;
});
$.validator.addMethod("filterexpression", function (value, element, params) {
    //alert(params.filtertype);
    // 
    if (params.buttonkeys == undefined || params.buttonkeys == "" || $(".clickedButton").prop("name") == undefined || params.buttonkeys.indexOf($(".clickedButton").prop("name")) > -1 || $(".clickedButton").prop("name") === "action:" + params.buttonkeys) {
        var strRegx = "";
        switch (params.filtertype) {
            case 'alphanum':
                strRegx = new RegExp("^[A-Za-z0-9" + params.validchar + "]+$");
                break;
            case 'int':
                strRegx = new RegExp("^[0-9" + params.validchar + "]+$");
                break;
            case 'string':
                strRegx = new RegExp("^[A-Za-z" + params.validchar + "]+$");
                break;
            case 'timeformat':
                strRegx = new RegExp("^([01]\d|2[0-3]):?([0-5]\d):?([0-5]\d)$");
                break;
            case 'email':
                strRegx = /\w+([-+.]\w+)*@exlservice.com/
                break;
            case 'spclchar':
                strRegx = new RegExp("[" + params.invalidchar + "]+$")
                break;
        }
        if (strRegx.test(value))
            return true;
        return false;
    }
    return true;
}, '');


$.validator.unobtrusive.adapters.add('maxlengthcheck', ['length', 'buttonkeys'], function (options) {
    options.rules['maxlengthcheck'] = options.params;
    options.messages['maxlengthcheck'] = options.message;
});
$.validator.addMethod("maxlengthcheck", function (value, element, params) {
    if (params.buttonkeys == undefined || params.buttonkeys == "" || $(".clickedButton").prop("name") == undefined || params.buttonkeys.indexOf($(".clickedButton").prop("name")) > -1 || $(".clickedButton").prop("name") === "action:" + params.buttonkeys) {
        if (params.maxlength != "" && value.length > params.length)
            return false;
    }
    return true;
}, '');


$.validator.unobtrusive.adapters.add('checkboxrequired', ['propertyname', 'buttonkeys'], function (options) {
    options.rules['checkboxrequired'] = options.params;
    options.messages['checkboxrequired'] = options.message;
});
$.validator.addMethod("checkboxrequired", function (value, element, params) {
    if (params.buttonkeys == undefined || params.buttonkeys == "" || $(".clickedButton").prop("name") == undefined || params.buttonkeys.indexOf($(".clickedButton").prop("name")) > -1 || $(".clickedButton").prop("name") === "action:" + params.buttonkeys) {
        var controls = params.propertyname.split(',');
        var bValid = false;
        for (var i = 0; i < controls.length; i++) {
            if (($('#' + controls[i])).prop('checked') == true) {
                bValid = true;
                break;
            }
        }
        if (bValid)
            return true;
        return false;
    }
    return true;
}, '');

var pageValidationRule = [];
var pageValidationRuleName = [];

//Turn off Specific validation on the basis of Key

function GetFormValidate(form, btnKey) {
    //
     
    if (pageValidationRule == undefined || pageValidationRule == null || pageValidationRule <= 0) {
        for (var Index in form.validate().settings.rules) {
            pageValidationRule.push(form.validate().settings.rules[Index]);
            pageValidationRuleName.push(Index);
        }
    }
    for (ruleIndex = 0 ; ruleIndex < pageValidationRuleName.length; ruleIndex++) {
        var el = $("#" + pageValidationRuleName[ruleIndex]);
        for (var i = 0, atts = el[0].attributes, n = atts.length, arr = []; i < n; i++) {
            if (atts[i].nodeName.indexOf("buttonkeys") > -1) {
                if (atts[i].nodeValue.indexOf(btnKey) == -1) {
                    // 
                    delete form.validate().settings.rules[pageValidationRuleName[ruleIndex]];
                } else {
                    form.validate().settings.rules[pageValidationRuleName[ruleIndex]] = pageValidationRule[ruleIndex];
                }
            }
        }
    }
    $(form).validate();
    return $(form).valid();
}
