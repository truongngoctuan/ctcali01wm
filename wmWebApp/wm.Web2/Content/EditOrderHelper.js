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