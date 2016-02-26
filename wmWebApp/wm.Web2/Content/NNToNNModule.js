function NNToNN(urlList) {
    this.urlList = urlList;
    this.select_all_counter = 0;
    this.select_all_length = 0;
    this.init = function () {
        this.table = $('#example').DataTable({
            'ajax': {
                "type": "POST",
                "url": this.urlList,
                "contentType": 'application/json; charset=utf-8',
                //'data': function(data) {//modify sending data
                //     return JSON.stringify(data);
                //},
                "dataSrc": function (json) {
                    this.select_all_counter = 0;
                    this.select_all_length = json.data.length;
                    for (var i = 0; i < json.data.length; i++) {
                        if (json.data[i].IsChecked) {
                            UpdateSelectAll(1);
                        }
                    }

                    return json.data;
                }
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
            var rows = this.table.rows({ 'search': 'applied' }).nodes();
            // Check/uncheck checkboxes for all rows in the table
            $('input[type="checkbox"]', rows).prop('checked', this.checked);
        });

        // Handle click on checkbox to set state of "Select all" control
        $('#example tbody').on('change', 'input[type="checkbox"]', function () {
            // If checkbox is not checked
            if (!this.checked) {
                UpdateSelectAll(-1);
            } else {
                UpdateSelectAll(1);
            }
        });

    };

    this.UpdateSelectAll = function (updateValue) {
        this.select_all_counter += updateValue;
        if (this.select_all_counter === this.select_all_length) {
            $('#example-select-all').prop("indeterminate", false);
            $('#example-select-all').prop("checked", true);
        } else {
            if (this.select_all_counter === 0) {
                $('#example-select-all').prop("indeterminate", false);
                $('#example-select-all').prop("checked", false);
            } else {
                $('#example-select-all').prop("indeterminate", true);
            }
        }
    }

    


}