$(document).ready(function () {
    $("#SourceDatabaseId").change(function () {
        var val = $("#SourceDatabaseId").val();
        var valOther = $("#DestinationDatabaseId").val();
        if (val != "" && val == valOther) {
            $("#DestinationDatabaseId").val("");
        }
    });

    $("#DestinationDatabaseId").change(function () {
        var val = $("#DestinationDatabaseId").val();
        var valOther = $("#SourceDatabaseId").val();
        if (val != "" && val == valOther) {
            $("#SourceDatabaseId").val("");
        }
    });
});