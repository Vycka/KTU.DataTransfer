window.ProjectListUpdateTimer = setInterval(RefreshProjectStates, 2000);
window.ProjectListPostActive = false;

window.ProjectListRawUrl = (RequestForAllProjects ? "GetListAllRaw" : "GetListRaw");
window.DetailsAreShown = false;

function RefreshProjectStates() {
    if (ProjectListPostActive)
        return;

    window.ProjectListPostActive = true;

    PostCommand('ProjectList/' + ProjectListRawUrl, {}, function (result, data) {
        if (result == true) {
            data = data.Projects;
            $.each(data, function(index) {
                UpdateProjectState(data[index]);
            });
        }
        UpdateContextButtons();
    });

    window.ProjectListPostActive = false;
}

function UpdateProjectState(stateInfo) {
    //data-state="-1" data-step="-1"
    //itemlist-name-@item.ProjectId
    //itemlist-right-state-@item.ProjectId

    var id = stateInfo["ProjectId"];
    var state = stateInfo["ProjectState"];
    var step = stateInfo["ExecutionStep"];
    var projectSelector = $("#itemlist-name-" + id);
    var stateSelector = $("#itemlist-right-state-" + id);

    if (projectSelector.length == 0)
        return;

    projectSelector.attr("data-state", state);
    projectSelector.attr("data-step", step);

    var targetColor = window.ProjectStateColors[state];
    var targetName = window.ProjectStateNames[state] + " [" + window.ProjectStepNames[step] + "]";
    if (state == 0) {
        targetName = window.ProjectStepNames[step];
    }
    else if (state == 4) {
        targetName = window.ProjectStateNames[state];
    }

    stateSelector.css("color", targetColor);
    stateSelector.html(targetName);
}

function UpdateContextButtons() {
    var state = $("#itemlist-name-" + window.SelectedItemId).attr("data-state");
    var step = $("#itemlist-name-" + window.SelectedItemId).attr("data-step");
    var buttonsToDisplay = {};
    if (state == 0) // Stopped
    {
        buttonsToDisplay = { "edit": true, "start": true, "details": true, "archive": true, "logs": true };
    }
    else if (state == 1) // Running
    {
        buttonsToDisplay = { "cancel": true, "pause": true, "details": true, "logs": true };
    }
    else if (state == 2) // Paused
    {
        buttonsToDisplay = { "cancel": true, "continue": true, "details": true, "logs": true };
    }
    else if (state == 3) // Error
    {
        buttonsToDisplay = { "cancel": true, "continue": true, "details": true, "logs": true };
    }
    else if (state == 4) // Archived
    {
        buttonsToDisplay = { "delete": true, "logs": true };
    }

    SetButtonVisibility("edit", buttonsToDisplay["edit"]);
    SetButtonVisibility("start", buttonsToDisplay["start"] == true && step != 6);
    SetButtonVisibility("continue", buttonsToDisplay["continue"]);
    SetButtonVisibility("cancel", buttonsToDisplay["cancel"]);
    SetButtonVisibility("pause", buttonsToDisplay["pause"]);
    SetButtonVisibility("details", buttonsToDisplay["details"] == true && window.DetailsAreShown == false);
    SetButtonVisibility("archive", buttonsToDisplay["archive"]);
    SetButtonVisibility("delete", buttonsToDisplay["delete"] == true && window.ActiveUserIsAdmin == true);
    SetButtonVisibility("logs", buttonsToDisplay["logs"]);
}

function SetButtonVisibility(buttonName, isVisible) {
    $("#pcontrol-" + buttonName + "-button").css("display", isVisible == true ? "" : "none");
}

window.ItemClickCallBack = function (itemId) {
    HideProjectDetails();
    $("#bottom-panel").css("display", "");
    UpdateContextButtons();
};

$(document).ready(function() {
    if (window.ActiveUserIsAdmin == true) {
        $("#bottom-panel").css("display", "");
        if (window.RequestForAllProjects == true)
            $("#showmine-button").css("display", "");
        else
            $("#showall-button").css("display", "");
    }
    

    RefreshProjectStates();
});

function HideProjectDetails() {
    window.DetailsAreShown = false;
    $("#project-details-pane").css("display", "none");
}

function ShowProjectDetails() {

    var detailsPane = $("#project-details-filters");
    detailsPane.css("display", "none");
    PostCommand('ProjectEditor/Details', { projectId: window.SelectedItemId }, function(result, data) {
        if (result == true) {
            detailsPane.html(data);
            detailsPane.css("display", "");
        }
    });

    window.DetailsAreShown = true;
    $("#project-details-pane").css("display", "");
}

function StartProjectClick() {
    PostCommand(
        "ProjectExecutor/Start",
        { id: window.SelectedItemId },
        function (result, data) {
            if (result == true) {
                UpdateProjectState(data.ProjectState);
                UpdateContextButtons();
            }
        }
    );
}

function ContinueProjectClick() {
    PostCommand(
        "ProjectExecutor/Continue",
        { id: window.SelectedItemId },
        function (result, data) {
            if (result == true) {
                UpdateProjectState(data.ProjectState);
                UpdateContextButtons();
            }
        }
    );
}

function ArchiveProjectClick() {
    if (!confirm('Are you sure you want to archive project: ' + $('#itemlist-name-' + window.SelectedItemId).html() + '?'))
        return;
    PostCommand(
        "ProjectExecutor/Archive",
        { id: window.SelectedItemId },
        function (result, data) {
            if (result == true) {
                UpdateProjectState(data.ProjectState);
                UpdateContextButtons();
            }
        }
    );
}

function DeleteProjectClick() {
    if (!confirm('Are you sure you want to delete project: ' + $('#itemlist-name-' + window.SelectedItemId).html() + '?' +
        '\r\n All information related to this project, including logs will be removed permanently!'))
        return;

    PostCommand(
        "ProjectExecutor/Delete",
        { id: window.SelectedItemId },
        function (result, data) {
            if (result == true) {
                location.reload(true);
            }
        }
    );
}


function ProjectDetailsClick() {
    ShowProjectDetails();
    UpdateContextButtons();
}

window.ProjectStateNames = {};
window.ProjectStateNames[-1] = "N/A";
window.ProjectStateNames[0] = "Stopped";
window.ProjectStateNames[1] = "Running";
window.ProjectStateNames[2] = "Paused";
window.ProjectStateNames[3] = "Error";
window.ProjectStateNames[4] = "Archived";

window.ProjectStepNames = {};
window.ProjectStepNames[-1] = "N/A";
window.ProjectStepNames[0] = "Not Started";
window.ProjectStepNames[1] = "Analyzing Source Database";
window.ProjectStepNames[2] = "Analyzing Source Database";
window.ProjectStepNames[3] = "Copying";
window.ProjectStepNames[4] = "Verifying";
window.ProjectStepNames[5] = "Deleting data from source";
window.ProjectStepNames[6] = "Completed";
window.ProjectStepNames[7] = "Canceled";
window.ProjectStepNames[8] = "Creating tables";

window.ProjectStateColors = {};
window.ProjectStateColors[-1] = "lightgray";
window.ProjectStateColors[0] = "lightgray";
window.ProjectStateColors[1] = "lightseagreen";
window.ProjectStateColors[2] = "lightsalmon";
window.ProjectStateColors[3] = "red";
window.ProjectStateColors[4] = "lightgray";