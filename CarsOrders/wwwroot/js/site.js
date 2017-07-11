$("#orderdate").datetimepicker({
    format: 'DD/MM/YYYY HH:mm'
});

function parseDate(stringDate) {
    var dateTime = stringDate.split(" ");
    if (dateTime.length !== 2)
        return;
    var dateStr = dateTime[0];
    var date = dateStr.split("/");
    if (date.length !== 3)
        return;
    var timeStr = dateTime[1];
    var time = timeStr.split(":");
    if (time.length !== 2)
        return;
    return new Date(date[0], date[1], date[2], time[0], time[1]);
};