function ShowErrorPane(errorMsg) {
    var pane = $("#error-pane");
    if (errorMsg == '')
        pane.css('display', 'none');
    else {
        pane.css('display', '');
        pane.html(errorMsg);
    }
}