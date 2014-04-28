window.SelectedDatabaseId = -1;

function DatabaseClick(databaseId) {
    window.SelectedDatabaseId = databaseId;
    $('[id^=database-]').css('font-weight', '');
    $('#database-' + databaseId).css('font-weight', 'bold');

    $('#edit-button').css('display', '');
    $('#delete-button').css('display', '');
}

function DeleteClick() {
    PostCommand(
        "Databases/Delete",
        { databaseId: window.SelectedDatabaseId },
        function (result) {
            if (result == true)
                location.reload(true);
        }
    );
}

