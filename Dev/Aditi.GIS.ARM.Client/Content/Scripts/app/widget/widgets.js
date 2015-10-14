var ARM = ARM || {};
ARM.Widget = ARM.Widget || {};
ARM.Widget.EmployeeUtilization = function () {
    var html = ko.observable();

    var lci = new ARM.Model.location();
    function updateData(geoDataArray, locationIdArray, skillsetIds) {
        var totalEmp = 0, billedEmp = 0;
        for (var i = 0, ii = locationIdArray.length; i < ii; i++)
            lci.EmployeeUtilization(locationIdArray[i], skillsetIds, function (result) {
                if (result) {
                    for (key in result.extprops) {
                        var rs = result.extprops[key];
                        var data = rs.split(',');
                        totalEmp += parseInt(data[0]);
                        billedEmp += parseInt(data[1]);
                        html("Total Emp " + totalEmp + " , Billed " + billedEmp);
                        break;
                    }
                }
            });
    };
    function clear() { html('');}
    return { Title: "Employee Utilization", HTML: html, updateData: updateData,clear:clear }
}
ARM.Widget.Weather = function () {
    var html = ko.observable();
    function updateData(geoDataArray, locationIdArray, SkillsetIds) {
        var rs = '';
        for (var i = 0, ii = geoDataArray.length; i < ii; i++) {
            Aditi.Core.Api.instance.get("http://api.openweathermap.org/data/2.5/weather?mode=html&lat=" + geoDataArray[i].lat + "&lon=" + geoDataArray[i].lon, null,
                function (result) {
                    rs += result;
                    html(rs);
                },
                function (error) {

                });
        }
    }
    function clear() { html('');}
    return { Title: "Weather", updateData: updateData, HTML: html,clear:clear }
}
ARM.Widget.Employees = function ()
{
    var html = ko.observable();
    var summary = ko.observable();
    var lci = new ARM.Model.location();
    
    function updateData(geoDataArray, locationIdArray, skillsetIds) {
        var htmlstring = '';
        var count = 0;
        for (var i = 0, ii = locationIdArray.length; i < ii; i++)
            lci.EmployeeList(locationIdArray[i], skillsetIds, function (result) {
                if (result) {
                    for (var i = 0, ii = result.length; i < ii; i++) {
                        htmlstring += result[i].fn + '<br></br>';
                        count++;
                    };
                }
                summary(count);
                html(htmlstring);
            });

    };
    function clear()
    { html('');}

    return { Title: "Employee List", HTML: html,Summary:summary, updateData: updateData ,clear:clear}
}
ARM.Widget.EmployeesUnBilled = function () {
    var html = ko.observable();
    var lci = new ARM.Model.location();
    var summary = ko.observable();
    function updateData(geoDataArray, locationIdArray, skillsetIds) {
        var htmlstring = '';
        var count = 0;
        for (var i = 0, ii = locationIdArray.length; i < ii; i++)
            lci.EmployeeListUnBilled(locationIdArray[i], skillsetIds, function (result) {
                if (result) {
                    for (var i = 0, ii = result.length; i < ii; i++) {
                        htmlstring += result[i].fn + '<br></br>';
                        count++;
                    };
                }
                summary(count);
                html(htmlstring);
            });

    };
    function clear()
    { html('');}

    return { Title: "Un-Billed Employee List", HTML: html, Summary:summary,updateData: updateData,clear:clear }
}