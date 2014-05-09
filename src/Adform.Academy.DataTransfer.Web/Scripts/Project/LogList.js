window.AuditLogTimer = setInterval(RefreshAuditLog, 2000);
window.AuditLogPostActive = false;
window.AuditLogLastItem = 0;

$(document).ready(function () {
    RefreshAuditLog();
});

function RefreshAuditLog() {
    if (AuditLogPostActive)
        return;
    window.AuditLogPostActive = true;

    PostCommand(
        "SystemLog/Get",
        { BeginFromId: window.AuditLogLastItem, ProjectId: window.ProjectId },
        function (result, data) {
            if (result == true) {
                //var logs = $.parseJSON(JSON.stringify(data.Logs), true);
                var logs = data.Logs;
                var itemsLen = logs.length - 1;
                var logsPane = document.getElementById('log-body');
                for (var x = itemsLen; x >= 0; x--) {
                    var logItem = logs[x];

                    var tr = logsPane.insertRow(0);
                    tr.className = "logrow";

                    var td1 = document.createElement('td');
                    var td2 = document.createElement('td');
                    var td3 = document.createElement('td');

                    td1.className = "td1";
                    td2.className = "td2";
                    td3.className = "td3";

                    var time = moment(logItem.TimeStamp).add('m',moment().zone()*-1);
                    td1.innerHTML = time.format("YYYY-MM-DD HH:mm:ss");
                    if (logItem.UserName != null)
                        td2.innerHTML = logItem.UserName;
                    td3.innerHTML = logItem.Message.split("\n").join("<br />");
                    tr.appendChild(td1);
                    tr.appendChild(td2);
                    tr.appendChild(td3);

                    window.AuditLogLastItem = logItem.LogId;
                }
            }
        }
    );

    window.AuditLogPostActive = false;
}


/*!
 * jQuery.parseJSON() extension (supports ISO & Asp.net date conversion)
 *
 * Version 1.0 (13 Jan 2011)
 *
 * Copyright (c) 2011 Robert Koritnik
 * Licensed under the terms of the MIT license
 * http://www.opensource.org/licenses/mit-license.php
 */
(function ($) {

    // JSON RegExp
    var rvalidchars = /^[\],:{}\s]*$/;
    var rvalidescape = /\\(?:["\\\/bfnrt]|u[0-9a-fA-F]{4})/g;
    var rvalidtokens = /"[^"\\\n\r]*"|true|false|null|-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?/g;
    var rvalidbraces = /(?:^|:|,)(?:\s*\[)+/g;
    var dateISO = /\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}(?:[.,]\d+)?Z/i;
    var dateNet = /\/Date\((\d+)(?:-\d+)?\)\//i;

    // replacer RegExp
    var replaceISO = /"(\d{4})-(\d{2})-(\d{2})T(\d{2}):(\d{2}):(\d{2})(?:[.,](\d+))?Z"/i;
    var replaceNet = /"\\\/Date\((\d+)(?:-\d+)?\)\\\/"/i;

    // determine JSON native support
    var nativeJSON = (window.JSON && window.JSON.parse) ? true : false;
    var extendedJSON = nativeJSON && window.JSON.parse('{"x":9}', function (k, v) { return "Y"; }) === "Y";

    var jsonDateConverter = function (key, value) {
        if (typeof (value) === "string") {
            if (dateISO.test(value)) {
                return new Date(value);
            }
            if (dateNet.test(value)) {
                return new Date(parseInt(dateNet.exec(value)[1], 10));
            }
        }
        return value;
    };

    $.extend({
        parseJSON: function (data, convertDates) {
            /// <summary>Takes a well-formed JSON string and returns the resulting JavaScript object.</summary>
            /// <param name="data" type="String">The JSON string to parse.</param>
            /// <param name="convertDates" optional="true" type="Boolean">Set to true when you want ISO/Asp.net dates to be auto-converted to dates.</param>

            if (typeof data !== "string" || !data) {
                return null;
            }

            // Make sure leading/trailing whitespace is removed (IE can't handle it)
            data = $.trim(data);

            // Make sure the incoming data is actual JSON
            // Logic borrowed from http://json.org/json2.js
            if (rvalidchars.test(data
                .replace(rvalidescape, "@")
                .replace(rvalidtokens, "]")
                .replace(rvalidbraces, ""))) {
                // Try to use the native JSON parser
                if (extendedJSON || (nativeJSON && convertDates !== true)) {
                    return window.JSON.parse(data, convertDates === true ? jsonDateConverter : undefined);
                }
                else {
                    data = convertDates === true ?
                        data.replace(replaceISO, "new Date(parseInt('$1',10),parseInt('$2',10)-1,parseInt('$3',10),parseInt('$4',10),parseInt('$5',10),parseInt('$6',10),(function(s){return parseInt(s,10)||0;})('$7'))")
                            .replace(replaceNet, "new Date($1)") :
                        data;
                    return (new Function("return " + data))();
                }
            } else {
                $.error("Invalid JSON: " + data);
            }
        }
    });
})(jQuery);