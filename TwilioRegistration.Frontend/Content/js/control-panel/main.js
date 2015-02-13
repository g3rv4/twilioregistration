(function(){
    app.config(function ($routeProvider, $locationProvider, $httpProvider) {
        $locationProvider.html5Mode(true);

        $routeProvider.when('/control-panel', {
            templateUrl: '/html/control-panel/dashboard.html',
            controller: 'DashboardCtrl',
            controllerAs: 'ctrl',
            activeTab: 'dashboard'
        }).when('/control-panel/devices/:action?', {
            templateUrl: '/html/control-panel/devices.html',
            controller: 'DevicesCtrl',
            controllerAs: 'ctrl',
            activeTab: 'devices'
        }).when('/control-panel/calls', {
            templateUrl: '/html/control-panel/calls.html',
            controller: 'CallsCtrl',
            controllerAs: 'ctrl',
            activeTab: 'calls'
        });

        // if we receive a 401, delete the token and redirect to the homepage
        $httpProvider.interceptors.push(function ($q, $window) {
            return {
                'responseError': function (response) {
                    var status = response.status;
                    if (status == 401) {
                        $window.sessionStorage.removeItem('token');
                        $window.location.href = '/';
                    }
                    return $q.reject(response);
                },
            };
        });
    })

    app.run(function ($rootScope, $window, $http) {
        if (!$window.sessionStorage.token) {
            $window.location.href = '/'
        }

        $http.defaults.headers.common.Authorization = 'Bearer ' + $window.sessionStorage.token
        if ($window.sessionStorage.actingAs) {
            $http.defaults.headers.common['Acting-As'] = $window.sessionStorage.actingAs
        }
    });
})();