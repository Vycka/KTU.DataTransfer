window.SelectedTableName = "";

function TableClick(tableName) {
    if (SelectedTableName == tableName)
        return;
    $("#right-panel-" + tableName).css("display", "");
    $("#right-panel-" + SelectedTableName).css("display", "none");

    $("#table-item-" + tableName).css("font-weight", "bold");
    $("#table-item-" + SelectedTableName).css("font-weight", "");

    window.SelectedTableName = tableName;
}

function FieldClick(tableName, fieldName) {
    ValidateTable(tableName);
}

function ValidateTable(tableName) {
    var checkedCount = GetCheckedFieldsCount(tableName);
    if (checkedCount > 0) {
        if (SupportedFieldSelected(tableName)) {
            $('#table-item-' + tableName).css("color", "lightgreen");
        }
        else {
            $('#table-item-' + tableName).css("color", "red");
        }
    } else {
        $('#table-item-' + tableName).css("color", "");
        
    }
}

function GetCheckedFieldsCount(tableName) {
    return $('[id^=fieldcheckbox-' + tableName + '-]:checked').length;
}

function GetTotalCheckedFieldsCount() {
    return $('[id^=fieldcheckbox-]:checked').length;
}

function ClickAll(tableName) {
    var checkboxes = $('[id^=fieldcheckbox-' + tableName + '-]');
    checkboxes.each(function() {
        $(this).prop("checked", true);
    });
    ValidateTable(tableName);
}

function ClickNone(tableName) {
    var checkboxes = $('[id^=fieldcheckbox-' + tableName + '-]');
    checkboxes.each(function () {
        $(this).prop("checked", false);
    });
    ValidateTable(tableName);
}

function CorrectFieldsSelected() {
    var correctFieldSelected = true;
    $.each(
        window.TableNames,
        function (index,value) {
            if (GetCheckedFieldsCount(value) > 0 && SupportedFieldSelected(value) == false)
                correctFieldSelected = false;
        }
    );
    return correctFieldSelected;
}

function SubmitSelections() {
    if (GetTotalCheckedFieldsCount() == 0) {
        ShowErrorPane("At least one field has to be selected for copying!");
        return;
    }
    if (CorrectFieldsSelected() == false) {
        ShowErrorPane("at least one int or datetime field must be selected in the selected tables");
        return;
    }
    ShowErrorPane("");

    var selectedElementsArray = [];

    var selectedElements = $('[id^=fieldcheckbox-]:checked');
    selectedElements.each(function() {
        var checkBoxData = GetCheckBoxData($(this));

        selectedElementsArray.push({
            TableName: checkBoxData['TableName'],
            ColumnName: checkBoxData['ColumnName'],
            ColumnType: checkBoxData["ColumnType"]
        });
    });
    $("#SelectedFieldsRawJson").val(JSON.stringify(selectedElementsArray));

    $("#modelstate-form").submit();
}

function SupportedFieldSelected(tableName) {
    var checkboxes = $('[id^=fieldcheckbox-' + tableName + '-]:checked');
    var isSupported = false;
    checkboxes.each(function () {
        var checkBoxData = GetCheckBoxData($(this));
        var fieldType = checkBoxData["ColumnType"];
        if (fieldType == "int" || fieldType == "datetime")
            isSupported = true;
    });
    return isSupported;
}

function GetCheckBoxData(checkbox) {

    return {
        TableName: checkbox.attr("data-table-name"),
        ColumnName: checkbox.attr("data-field-name"),
        ColumnType: checkbox.attr("data-field-type")
    }
}