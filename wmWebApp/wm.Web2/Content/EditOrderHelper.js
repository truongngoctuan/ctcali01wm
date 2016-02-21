function clickGoodCategoryItem(url) {
    if (window.isDataChanged) {
        //confirm before change page
        bootbox.confirm("There are unsave data, do you want to leave this page ? ", function (result) {
            if (result) {
                console.log("still want to leave this page");
                window.location.href = url;
            }
        });


    } else {
        window.location.href = url;
    }
}

//@Url.Action("PopulateData")
function loadIncludeExclude(url) {
    postHelperJson(url, { orderId: window.OrderId, goodCategoryId: window.GoodCategoryId },
        function (data) {
            window.dataInput = data;
            hot.loadData(window.dataInput);
        });
}

//'@Url.Action("Place")'
function saveIncludeExclude(url) {
    postHelper(url,
        { id: window.OrderId, data: window.dataInput },
        function (data) {
            var dataObject = JSON.parse(data);
            if (dataObject.Status === "Ok") {
                window.isDataChanged = false;
                $("#success-message").html("update success");
                $("#error-message").html("");
            }
            else {
                console.log("error saveIncludeExclude");
                $("#success-message").html("");
                $("#error-message").html("update fail");
            }
        });
}


function InitStaffHandsontables() {
    hot = new Handsontable(container, {
        startRows: 1,
        startCols: 2,
        allowInvalid: false,
        rowHeaders: true,
        colHeaders: ['Name', 'InStock', 'RecommendQuantity', 'Quantity', 'Note', 'YourNote'],
        dataSchema: { Id: null, Name: null, InStock: null, RecommendedQuantity: null, Quantity: null, Note: null, YourNote: null },
        columns: [//TODO: change colorbackground for readonly
            //{ Data: 'CategoryId', readOnly: true },
            { data: 'Name', readOnly: true },
            { data: 'InStock', readOnly: false },
            { data: 'RecommenedQuantity', readOnly: true },
            { data: 'Quantity', readOnly: false, type: 'numeric', format: '0,0.00', allowInvalid: true },
            { data: 'Note', readOnly: true },
            { data: 'YourNote', readOnly: false }

            //{ Data: 'IsChecked', type: 'checkbox' },
        ]
    });
}

