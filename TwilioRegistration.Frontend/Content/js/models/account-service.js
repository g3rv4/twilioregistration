﻿(function () {
    app.factory('accountService', function ($resource, $http, $q, $log, baseUrl) {
        resource = $resource(baseUrl + 'accounts/:id', { id: "@Id" }, null, {stripTrailingSlashes: false})
        return {
            logIn: function (email, password) {
                var deferred = $q.defer()
                $http.post(baseUrl + 'accounts/log-in', { 'Email': email, 'Password': password })
                    .success(function (response) {
                        if (response.Status == 'SUCCESS') {
                            deferred.resolve(response.Token)
                        } else {
                            deferred.reject(response.Status)
                        }
                    })
                    .error(function (data, code, headers, config, status) {
                        $log.error('Code: ' + code + '\nData: ' + data + '\nStatus: ' + status)
                        deferred.reject(code)
                    })
                return deferred.promise
            },
            resource: resource
        }
    })
})();