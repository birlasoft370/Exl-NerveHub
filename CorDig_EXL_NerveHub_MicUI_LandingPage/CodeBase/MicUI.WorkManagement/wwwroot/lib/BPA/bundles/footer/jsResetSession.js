
var objInterval;
var secondInterval;
/*
function KeepSession() {
    resetTimer();
    $("#ExpireConfirm_Submit").hide();
    AntiforgeryToken = $('input[name=__RequestVerificationToken]').val();
    var reseturl = window.location.protocol + "//" + window.location.host + ResourceLayout.tenantName + "/Home/SessionReset";
    $.ajax({
        url: reseturl,
        type: 'POST',
        dataType: 'html',
        data: { tenantName: ResourceLayout.tenantName.replace("/", "") },
        success: function (_response) {
            var url = window.location.href;
            if (url.indexOf("Logout") == -1) {
                //  // $("#ExpireConfirm_Submit").hide();                
                // //resetTimer();
            }
        },
        error: function (err) {
            jAlert(err.statusText);
        }

    });
}
*/
function showTimeout(logoutWithin) {
    console.log('Called showTimeout:');
    var i = 0;
    var mapSeconds = logoutWithin * 60 * 1000;
    $("#ExpireConfirm_Submit").fadeIn(2000);
    var appSetting = $("#HFAppTenantName").val();


    if (appSetting != undefined) {
        if (ResourceLayout.tenantName.replace("#", "").replace("/", "").toUpperCase() == appSetting) {
            if ($("#HFUserRoleID").val() == '305') {
                AutoSaveGridDataAudtee('DRAFT');
            }
            else if ($("#HFUserRoleID").val() == '304') {
                AutosavegrdchecklistdataAuditor('DRAFT');
            }
        }
    }
    objInterval = setInterval(function () {
        //i++;
        ////console.log('Called showTimeout: i value>'+i);
        //if (i >= mapSeconds) { 
        var reseturl = window.location.protocol + "//" + window.location.host + ResourceLayout.tenantName.replace("#", "") + "/Home/Logout";
        reseturl.submit();
        //}
    }, mapSeconds);
    var seconds = logoutWithin * 60;
    secondInterval = setInterval(function () {
        $('.Timer').text(fancyTimeFormat(seconds--))
    }, 1000);

}

function resetInterval() {
    console.log('Called resetInterval: ');
    clearInterval(timeoutID);
    clearInterval(objInterval);
    clearInterval(secondInterval);
    //setTimeout(function () {
    //    showTimeout(timeoutDuration);
    //}, timeoutDuration);
}

function fancyTimeFormat(time) {
    // Hours, minutes and seconds
    var hrs = ~~(time / 3600);
    var mins = ~~((time % 3600) / 60);
    var secs = ~~time % 60;
    // Output like "1:01" or "4:03:59" or "123:03:59"
    var ret = "";
    if (hrs > 0) {
        ret += "" + hrs + ":" + (mins < 10 ? "0" : "");
    }
    ret += "" + mins + ":" + (secs < 10 ? "0" : "");
    ret += "" + secs;
    return ret;
}

var timeoutID;

function setup() {
    startTimer();
    this.addEventListener("mousemove", resetTimer, false);
    this.addEventListener("mousedown", resetTimer, false);
    this.addEventListener("keypress", resetTimer, false);
    this.addEventListener("DOMMouseScroll", resetTimer, false);
    this.addEventListener("mousewheel", resetTimer, false);
    this.addEventListener("touchmove", resetTimer, false);
    this.addEventListener("MSPointerMove", resetTimer, false);

}
//we get sTimeout(from web config as session timeout)
var sTimeout = 18; //18 Minutes
var msgBefore = 5;
$(document).ready(function () {
    var surl = ResourceLayout.partiaArea + 'Home/GetSessionTimeout';
    $.getJSON(surl).done(function (data) {
        sTimeout = data;
        //setup();
    }).fail(function (jqxhr, textStatus, error) {
        //setup();
    });
});

function startTimer() {
    //here we setup logout warning and in showTimeout function 
    //we pass msgBefore minutes, as warning msg show before msgBefore minutes
    var url = window.location.href;
    var callAfter = (sTimeout - msgBefore) * 60 * 1000;
    if (url.indexOf("Logout") == -1) {
        timeoutID = setInterval(function () {
            showTimeout(msgBefore);
        }, callAfter);
    }
}
function resetTimer() {

    beLive();
    $("#ExpireConfirm_Submit").hide();
}
function beLive() {
    resetInterval();
    startTimer();
}


