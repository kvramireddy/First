var ARM = ARM || {};
ARM.Model = ARM.Model || {};
ARM.Model.GeoData = function ()
{
    var lat, lon;
    var _data = new Object();
    var myType = function () { return "Point";}
    return {lat:lat,lon:lon,metadata:_data,getType:myType}
}

