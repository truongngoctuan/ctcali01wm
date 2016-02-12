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