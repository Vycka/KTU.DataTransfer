function DeleteClick() {
    if (confirm('Are you sure you want to delete database: ' + $('#itemlist-item-' + window.SelectedItemId).html() + '?')) {
        PostCommand(
            "Databases/Delete",
            { databaseId: window.SelectedItemId },
            function(result) {
                if (result == true)
                    location.reload(true);
            }
        );
    }
}

