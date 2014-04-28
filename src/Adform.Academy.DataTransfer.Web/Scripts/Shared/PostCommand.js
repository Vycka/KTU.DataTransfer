function PostCommand(relativeUrl, command, callback) {
    $.post(window.RootUrl + relativeUrl, command,
       function (data) {
           if (data.Success == true) { //ok
               ShowErrorPane('');
               if (callback != null)
                   callback(true);
           }
           else if (data.Success == false) { //error
               ShowErrorPane(data.Message);
               if (callback != null)
                   callback(false);
            }
       })
       .fail(function () {
           ShowErrorPane("Unable to connect!");
           if (callback != null)
               callback(false);
       });
}