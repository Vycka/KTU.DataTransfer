window.ProjectListUpdateTimer = setInterval(RefreshProjectStates, 1500);
window.ProjectListPostActive = false;

window.ProjectListRawUrl = (RequestForAllProjects ? "GetListAllRaw" : "GetListRaw");

function RefreshProjectStates() {
    if (ProjectListPostActive)
        return;

    window.ProjectListPostActive = true;
    $.post(
        window.RootUrl + 'ProjectList/' + ProjectListRawUrl,
        {  },
        function (data) {
            console.debug(data);
        })
        .always(function () {
            window.ProjectListPostActive = false;
        });
}