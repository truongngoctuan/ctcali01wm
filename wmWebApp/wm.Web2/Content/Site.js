function postHelper(url, data, successCallback) {
    $.ajax({
        url: url,
        data: data,
        cache: false,
        type: "POST",
        dataType: "html",
        success: successCallback
    });
}

function postHelperJson(url, data, successCallback) {
    $.ajax({
        url: url,
        data: data,
        cache: false,
        type: "POST",
        dataType: "json",
        success: successCallback
    });
}

function getHelper(url, data, successCallback) {
    $.ajax({
        url: url,
        data: data,
        cache: false,
        type: "GET",
        dataType: "html",
        success: successCallback
    });
}

function changeWindowLocation(url) {
    window.location.href = url;
}

//for calendar common
function configButtonPresNextCalendar() {
    //config button move pres or next for calendar
    $('.btn-group button[Data-calendar-nav]').each(function () {
        var $this = $(this);
        $this.click(function () {
            calendar.navigate($this.data('calendar-nav'));
        });
    });
}