function NNToNN(urlList, select_all_counter, select_all_length, table) {
    this.urlList = urlList;
    this.init = function () {
            table = $('#example').DataTable({
                'ajax': {
                    "type": "POST",
                    "url": this.urlList,
                    "contentType": 'application/json; charset=utf-8',
                    //'data': function(data) {//modify sending data
                    //     return JSON.stringify(data);
                    //},
                    "dataSrc": function(sa_counter, sa_length) {
                        return function(json) {
                            sa_counter = 0;
                            sa_length = json.data.length;
                            for (var i = 0; i < json.data.length; i++) {
                                if (json.data[i].IsChecked) {
                                    UpdateSelectAll(sa_counter, sa_length, 1);
                                }
                            }

                            return json.data;
                        }//end function(json)
                    }(select_all_counter, select_all_length, json)
        

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
        $('#example-select-all').on('click', function () {
            if (this.checked) {
                select_all_counter = select_all_length;
            } else {
                select_all_counter = 0;
            }
            // Get all rows with search applied
            var rows = table.rows({ 'search': 'applied' }).nodes();
            // Check/uncheck checkboxes for all rows in the table
            $('input[type="checkbox"]', rows).prop('checked', this.checked);
        });

        // Handle click on checkbox to set state of "Select all" control
        $('#example tbody').on('change', 'input[type="checkbox"]', function () {
            // If checkbox is not checked
            if (!this.checked) {
                UpdateSelectAll(select_all_counter, select_all_length, - 1);
            } else {
                UpdateSelectAll(select_all_counter, select_all_length, 1);
            }
        });

    };



    


}

function UpdateSelectAll(select_all_counter, select_all_length, updateValue) {
    console.log("before: " + select_all_counter + " " + select_all_length);
    select_all_counter += updateValue;
    console.log(select_all_counter + " " + select_all_length);
    if (select_all_counter === select_all_length) {
        $('#example-select-all').prop("indeterminate", false);
        $('#example-select-all').prop("checked", true);
    } else {
        if (select_all_counter === 0) {
            console.log("inde = false");
            $('#example-select-all').prop("indeterminate", false);
            $('#example-select-all').prop("checked", false);
        } else {
            $('#example-select-all').prop("indeterminate", true);
        }
    }
}