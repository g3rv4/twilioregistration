(function () {
    app.factory('accountService', function ($resource, $http, $q, $log, baseUrl) {
        resource = $resource(baseUrl + 'accounts/:id', { id: "@id" }, null, {stripTrailingSlashes: false})
        return {
            logIn: function (email, password) {
                var deferred = $q.defer()
                data = "grant_type=password&username=" + email + "&password=" + password;
                $http.post(baseUrl + 'token', data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } })
                    .success(function (response, status) {
                        deferred.resolve(response.access_token)
                    })
                    .error(function (data, code, headers, config, status) {
                        if (data.error) {
                            deferred.reject(data.error)
                        } else {
                            $log.error('Code: ' + code + '\nData: ' + data + '\nStatus: ' + status)
                            deferred.reject(code)
                        }
                    })
                return deferred.promise
            },
            resource: resource
        }
    })
})();