var Aditi = Aditi || {};
Aditi.Core = Aditi.Core || {};
(function (namespace, $jquery)
{
    namespace.Api = function () {
        var self = this;
        self.get = function (uri, query, success, error) {
            doAjax(uri, 'GET', null, success, error);
        }
        function doAjax(uri, methodType, data, successCallback, errorCallback) {
            return $jquery.ajax(
                {
                    url: uri,
                    type: methodType,
                    //contentType: 'application/json',
                    data: data,
                    success: function (result, textStatus, jqXHR) {
                        successCallback && successCallback(result, textStatus, jqXHR);
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        errorCallback && errorCallback(errorThrown, textStatus, jqXHR);
                    }
                });
        }
        
    };

    namespace.Api.instance = (function () {
        var instance;
        if (!instance)
            instance = new Aditi.Core.Api();
        return instance;
    })();
}
)(Aditi.Core,$);

