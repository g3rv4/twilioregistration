(function () {
    app.factory('deviceService', function ($resource, baseUrl) {
        resource = $resource(baseUrl + 'devices/:id', { id: "@id" })
        return {
            resource: resource
        }
    })
})();