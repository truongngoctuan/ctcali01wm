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

moment.fn.toASP = function () {
    return '/Date(' + (+this) + this.format('ZZ') + ')/';
}
//how to use:
//var input = "2012-10-20T20:45:30";
//var asp = moment(input).toASP();