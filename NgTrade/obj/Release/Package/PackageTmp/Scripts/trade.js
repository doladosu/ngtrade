$("#Symbol").blur(function() {
    var symb = $("#Symbol").val();
    if (symb != '') {
        $.getJSON('/Account/GetSymbolInfo?symbol=' + symb, function (data) {
            if (data != null) {
                $('#Price').val(data.Close);
            }
        });
    }
});