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