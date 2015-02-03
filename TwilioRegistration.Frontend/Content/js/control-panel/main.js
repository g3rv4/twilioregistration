(function(){
    app.config(function ($routeProvider, $locationProvider, $httpProvider) {
        $locationProvider.html5Mode(true);

        $routeProvider.when('/control-panel', {
            templateUrl: '/html/control-panel/dashboard.html',
            controller: 'DashboardCtrl',
            controllerAs: 'ctrl',
            activeTab: 'dashboard'
        });
        $routeProvider.when('/control-panel/devices', {
            templateUrl: '/html/control-panel/devices.html',
            controller: 'DevicesCtrl',
            controllerAs: 'ctrl',
            activeTab: 'devices'
        });
        $routeProvider.when('/control-panel/calls', {
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


    app.controller('ControlPanelCtrl', function (accountService, $window, $route) {
        var _this = this

        _this.logOut = function () {
            $window.sessionStorage.removeItem('token')
            $window.location.href = '/'
        }

        _this.refresh = function ($route) {
            var query = accountService.resource.query()
            query.$promise.then(function (result) {
                _this.account = result[0]
            });
        }

        _this.route = $route

        _this.refresh()
    });

    app.controller('DashboardCtrl', function (token) {
        var _this = this
    });

    app.controller('DevicesCtrl', function () {
        var _this = this
    });

    app.controller('CallsCtrl', function () {
        var _this = this
    });
})();