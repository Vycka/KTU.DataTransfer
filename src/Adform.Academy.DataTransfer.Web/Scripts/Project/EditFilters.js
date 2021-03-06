﻿function IndexSelected(tableName) {
    var elementType = $("#index-selector-" + tableName).find(":selected").attr("data-field-type");
    var dropdown = $("#index-range-" + tableName);
    dropdown.html("");
    $.each(FilterSteps[elementType], function(index) {
        dropdown.append($('<option/>', {
            value: FilterSteps[elementType][index]['value'],
            html: FilterSteps[elementType][index]['text'],
            'data-name': FilterSteps[elementType][index]['text']
        }));
    });
}

// Next
function SubmitSelections() {
    $("[id^=index-range-]").each(function() {
        var picker = $(this);
        var table = picker.attr("data-tablename");
        var column = $("#index-selector-" + table).find(":selected");
        var selected = picker.find(":selected");
        var range = selected.val();
        SetFilterBatchRange(table, column.val(), range, selected.attr("data-name"), column.attr("data-field-type"));
    });

    $("#modelstate-form").submit();
}

function SetFilterBatchRange(table, column, value, name, columnType) {
    var state = JSON.parse($("#FiltersJson").val());
    $.each(state, function (index) {
        if (state[index]["TableName"] == table) {
            state[index]["FilterValue"]["IndexColumn"] = column;
            state[index]["FilterValue"]["IndexStepName"] = name;
            state[index]["FilterValue"]["IndexStep"] = value;
            state[index]["FilterValue"]["IndexColumnType"] = columnType;
            return false;
        }
        return true;
    });
    $("#FiltersJson").val(JSON.stringify(state));
}

$(document).ready(function () {
    var filters = JSON.parse($("#FiltersJson").val());

    $.each(filters, function (index) {
        var filter = filters[index];
        var indexDropDown = $("#index-selector-" + filter.TableName);
        var stepDropDown = $("#index-range-" + filter.TableName);
        if (filter.FilterValue.IndexColumn == null) {
            indexDropDown.find('option:first-child').attr("selected", "selected");
            indexDropDown.change();
        } else {
            indexDropDown.val(filter.FilterValue.IndexColumn);
            indexDropDown.change();
            stepDropDown.val(filter.FilterValue.IndexStep);
        }
        
        
    });
});

StepsForInt = [];
StepsForInt.push({ value: "1000", text: "1K" });
StepsForInt.push({ value: "10000", text: "10K" });
StepsForInt.push({ value: "100000", text: "100K" });
StepsForInt.push({ value: "1000000", text: "1M" });
StepsForInt.push({ value: "10000000", text: "10M" });

StepsForDate = [];
StepsForDate.push({ value: "86400", text: "1 Day" });
StepsForDate.push({ value: "100000", text: "10 Days" });
StepsForDate.push({ value: "2592000", text: "30 Days" });
StepsForDate.push({ value: "7776000", text: "90 Days" });
StepsForDate.push({ value: "31536000", text: "1 Year" });

FilterSteps = {};
FilterSteps["int"] = StepsForInt;
FilterSteps["datetime"] = StepsForDate;