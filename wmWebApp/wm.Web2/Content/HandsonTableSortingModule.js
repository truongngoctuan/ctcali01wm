//http://stackoverflow.com/questions/939032/jquery-pass-more-parameters-into-callback
function HandsonTableSorting(objectId, urlLoad, urlSave,
    tableId,
    headersPredefine, columnsPredefine
    ) {
    this.objectId = objectId;

    this.urlLoad = urlLoad;
    this.urlSave = urlSave;

    this.tableId = tableId;

    this.headersPredefine = headersPredefine;
    this.columnsPredefine = columnsPredefine;

    this.init = function () {
        if (this.isInit) {
            return;
        }
        this.isInit = true;

        this.hotOrder = new Handsontable(document.getElementById(tableId), {
            startRows: 1,
            startCols: 2,
            rowHeaders: true,
            manualRowMove: true,
            //dataSchema: { Id: null, Name: null, RecommendedQuantity: null, Quantity: null },
            colHeaders: this.headersPredefine,
            columns: this.columnsPredefine,
            afterRowMove: function (oo) {
                return function (startRow, endRow) {
                    if (startRow > endRow) {
                        oo.dataInputOrder[startRow].Ranking = oo.dataInputOrder[endRow].Ranking;
                        for (var i = endRow; i <= startRow - 1; i++) {
                            oo.dataInputOrder[i].Ranking++;
                        }
                    } else {
                        oo.dataInputOrder[startRow].Ranking = oo.dataInputOrder[endRow].Ranking;
                        for (var i = startRow + 1; i <= endRow; i++) {
                            oo.dataInputOrder[i].Ranking--;
                        }
                    }
                    oo.dataInputOrder.sort(SortByRanking);
                    console.log("Move row: " + startRow + " " + endRow);
                }//end of function (startRow, endRow)
            }(this)
        });

        postHelper(this.urlLoad, { id: this.objectId, isInEx: false },
            function (oo) {
                return function (data, textStatus, XMLHttpRequest) {
                    console.log("client received: " + data);

                    oo.dataInputOrder = JSON.parse(data);
                    oo.hotOrder.loadData(oo.dataInputOrder);

                }
            }(this));

    };

    this.Save = function () {
        postHelper(this.urlSave,
                        { id: this.objectId, data: this.dataInputOrder },
                        function (oo) {
                            return function (data, textStatus, XMLHttpRequest) {
                                var dataObject = JSON.parse(data);
                                if (dataObject.Status === "Ok") {
                                    location.reload();
                                } else {
                                    console.log("error SortNnData");
                                }
                            }//end of function (data, textStatus, XMLHttpRequest)
                        }(this)
                        );
    };


}

function SortByRanking(a, b) {
    var aName = a.Ranking;
    var bName = b.Ranking;
    return ((aName < bName) ? -1 : ((aName > bName) ? 1 : 0));
}