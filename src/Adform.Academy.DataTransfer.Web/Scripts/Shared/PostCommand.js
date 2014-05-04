function PostCommand(relativeUrl, command, callback) {
    $.post(window.RootUrl + relativeUrl, command,
       function (data) {
           if (data.Success == true || data.Success == null) { //ok
               ShowErrorPane('');
               if (callback != null)
                   callback(true, data);
           }
           else if (data.Success == false) { //error
               ShowErrorPane(data.Message);
               if (callback != null)
                   callback(false, null);
            }
       })
       .fail(function () {
           ShowErrorPane("Unable to connect!");
           if (callback != null)
               callback(false);
       });
}

function OpenWindow(url, title) {
    var w = 830;
    var h = 600;
    var left = (screen.width / 2) - (w / 2);
    var top = (screen.height / 2) - (h / 2);
    return window.open(url, title, 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=yes, resizable=no, copyhistory=no, width=' + w + ', height=' + h + ', top=' + top + ', left=' + left);
}