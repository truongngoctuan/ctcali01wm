//http://stackoverflow.com/questions/939032/jquery-pass-more-parameters-into-callback
function NNToNN(objectId, urlList, urlSave,
    tableId
    ) {
    this.objectId = objectId;

    this.urlList = urlList;
    this.urlSave = urlSave;

    this.tableId = tableId;


    this.sa_counter = 0;
    this.sa_length = 0;
    this.init = function () {
        this.table = $(this.tableId).DataTable({
            'ajax': {
                "type": "POST",
                "url": this.urlList,
                "contentType": 'application/json; charset=utf-8',
                //'data': function(data) {//modify sending data
                //     return JSON.stringify(data);
                //},
                "dataSrc": function (oo) {
                    return function (json) {
                        oo.sa_counter = 0;
                        oo.sa_length = json.data.length;
                        for (var i = 0; i < json.data.length; i++) {
                            if (json.data[i].IsChecked) {
                                oo.UpdateSelectAll(1);
                            }
                        }

                        return json.data;
                    }//end function(json)
                }(this)


            },
            "columns": [
                {
                    "data": "Id",
                    'searchable': false,
                    'orderable': false,
                    'className': 'dt-body-center',
                    'render': function (data, type, full, meta) {
                        if (full.IsChecked) {
                            return '<input type="checkbox" name="id[]" value="' + data + '" checked="checked">';
                        }
                        return '<input type="checkbox" name="id[]" value="' + data + '">';
                    }
                },
                //{ "data": "AccountantCode" },
                { "data": "Id" },
                { "data": "Name" },
                //{ "data": "UnitName" },
                //{ "data": "GoodType" }
            ],
            'order': [[1, 'asc']]
        });


        // Handle click on "Select all" control
        $(this.tableId + ' input[name="select-all"]').on('click',
            function (oo) {
                return function () {
                    if (this.checked) {
                        oo.sa_counter = oo.sa_length;
                    } else {
                        oo.sa_counter = 0;
                    }
                    // Get all rows with search applied
                    var rows = oo.table.rows({ 'search': 'applied' }).nodes();
                    // Check/uncheck checkboxes for all rows in the table
                    $('input[type="checkbox"]', rows).prop('checked', this.checked);
                }//end of function()
            }(this)

    );

        // Handle click on checkbox to set state of "Select all" control
        $(this.tableId + ' tbody').on('change', 'input[type="checkbox"]', function (oo) {
            return function () {
                // If checkbox is not checked
                if (!this.checked) {
                    oo.UpdateSelectAll(-1);
                } else {
                    oo.UpdateSelectAll(1);
                }
            }//end of function()
        }(this)

        );

    };


    this.UpdateSelectAll = function (updateValue) {
        console.log("before: " + this.sa_counter + " " + this.sa_length);
        this.sa_counter += updateValue;
        console.log(this.sa_counter + " " + this.sa_length);
        if (this.sa_counter === this.sa_length) {
            $(this.tableId + ' input[name="select-all"]').prop("indeterminate", false);
            $(this.tableId + ' input[name="select-all"]').prop("checked", true);
        } else {
            if (this.sa_counter === 0) {
                $(this.tableId + ' input[name="select-all"]').prop("indeterminate", false);
                $(this.tableId + ' input[name="select-all"]').prop("checked", false);
            } else {
                $(this.tableId + ' input[name="select-all"]').prop("indeterminate", true);
            }
        }
    };

    this.Save = function () {
        var dataOutputFiltered = [];
        $(this.tableId + ' input[type="checkbox"]').each(function () {
            if (this.checked) {
                dataOutputFiltered.push({ Id: this.value, IsChecked: true, Name: "", Ranking: 0 });
            };
        });

        console.log(dataOutputFiltered);

        postHelper(this.urlSave,
            { id: this.objectId, data: dataOutputFiltered },
            function (data, textStatus, XMLHttpRequest) {
                var dataObject = JSON.parse(data);
                if (dataObject.Status === "Ok") {
                    location.reload();
                    //window.location.href = '@Url.Action("Edit", new {id = Model.Id})';
                } else {
                    console.log("error IncludeExcludeNnData");
                }
            });
    }


}