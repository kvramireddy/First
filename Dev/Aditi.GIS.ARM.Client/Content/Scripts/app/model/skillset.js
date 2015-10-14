var ARM = ARM || {};
ARM.Model = ARM.Model || {};
(function (namespace, $jquery) {
    namespace.skillset = function () {
        var self = this;
        self.pull = function (callback) {
            Aditi.Core.Api.instance.get('http://localhost:1393/gis/arm/skillset', null,
         function (result, status, xhr) {
             if (result && callback) callback(result.Result);
         },
         function (error, status, xhr) {
             //log it
         });
        };
        self.locations = function (id, callback)
        {
            Aditi.Core.Api.instance.get('http://localhost:1393/gis/arm/skillset/' + id + '/locations?groupby=emp', null,
         function (result, status, xhr) {
             if (result && callback) callback(result.Result);
         },
         function (error, status, xhr) {
             //log it
         });
        }
    };
})(ARM.Model, $);

(function (namespace, $jquery) {
    namespace.location = function () {
        var self = this;
        self.EmployeeUtilization = function (locationId,skillsetId,callback) {
            Aditi.Core.Api.instance.get('http://localhost:1393/gis/arm/location/' + locationId + '/EmployeeUtilization?skill=' + skillsetId, null,
         function (result, status, xhr) {
             if (result && callback) callback(result.Result);
         },
         function (error, status, xhr) {
             //log it
         });
        
        }

        self.EmployeeList = function (locationId, skillsetId, callback)
        {
            Aditi.Core.Api.instance.get('http://localhost:1393/gis/arm/location/' + locationId + '/employeebyskills?skill=' + skillsetId, null,
         function (result, status, xhr) {
             if (callback) {
                 if (result)
                     callback(result.Result);
                 else
                     callback(null);
             }
         },
         function (error, status, xhr) {
             if (callback) //check status 404
                 callback(null);
         });
        }

        self.EmployeeListUnBilled = function (locationId, skillsetId, callback) {
            Aditi.Core.Api.instance.get('http://localhost:1393/gis/arm/location/' + locationId + '/employeebyskills?flag=unbilled&skill=' + skillsetId, null,
         function (result, status, xhr) {
             if (callback) {
                 if (result)
                     callback(result.Result);
                 else
                     callback(null);
             }
         },
         function (error, status, xhr) {
             if (callback)//check status 404
                 callback(null);
         });
        }
    };
})(ARM.Model, $);