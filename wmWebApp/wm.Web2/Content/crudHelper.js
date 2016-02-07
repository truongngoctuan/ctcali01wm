function callDeleteConfirmation(dataObject, urlDelete) {
    bootbox.dialog({
        message: "Are you sure to delete this item?",
        title: "Delete confirmation",
        buttons: {
            success: {
                label: "Delete",
                className: "btn btn-danger",
                callback: function () {
                    $.ajax({
                        url: urlDelete,
                        data: dataObject,
                        cache: false,
                        type: "POST",
                        dataType: "html",
                        success: function (data, textStatus, XMLHttpRequest) {
                            var dataObject = JSON.parse(data);
                            if (dataObject["status"] == "ok") {
                                document.oTable.draw();
                            }
                        }
                    });

                }
            },
            cancel: {
                label: "Cancel",
                className: "btn",
                callback: function () {
                }
            }
        }
    });
}