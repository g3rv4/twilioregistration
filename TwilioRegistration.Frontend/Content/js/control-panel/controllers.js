(function () {
    app.controller('ControlPanelCtrl', function (accountService, $window, $route) {
        var _this = this

        _this.logOut = function () {
            $window.sessionStorage.removeItem('token')
            $window.location.href = '/'
        }

        _this.refresh = function ($route) {
            var query = accountService.resource.current()
            query.$promise.then(function (result) {
                _this.account = result
            });
        }

        _this.route = $route

        _this.refresh()
    });

    app.controller('DashboardCtrl', function () {
        var _this = this
    });

    app.controller('DevicesCtrl', function (deviceService, $routeParams, $modal, $location) {
        var _this = this

        _this.devices = []
        _this.config = {
            itemsPerPage: 5,
            fillLastPage: false
        }
        _this.action = $routeParams.action || 'list'

        _this.refresh = function () {
            var query = deviceService.resource.query()
            query.$promise.then(function (result) {
                _this.devices = result
            });
        }

        _this.deleteDevice = function (device) {
            deviceService.resource.delete(device, function () {
                _this.refresh()
            }, function(){
                errors = ['Error deleting device']
                _this.showErrors(errors)
            })
        }

        _this.addDevice = function (form) {
            if (form.$valid) {
                var device = {
                    username: _this.newDevice.username,
                    password: _this.newDevice.password
                }
                deviceService.resource.save(device, function (data) {
                    $location.url('/control-panel/devices')
                }, function (response) {
                    errors = [response.data.message]
                    _this.showErrors(errors)
                })
            } else {
                errors = []
                if (form.username.$error.required) {
                    errors.push('The username is required')
                } else if (form.username.$error.pattern) {
                    errors.push('The username can only contain letters and numbers')
                }
                if (form.password.$error.required) {
                    errors.push('The password is required')
                } else if (form.password.$error.minlength) {
                    errors.push('The password should be at least 8 characters long')
                }
                _this.showErrors(errors)
            }
        }

        _this.changePassword = function (item) {
            var modalInstance = $modal.open({
                templateUrl: 'change-passwords.html',
                controller: 'ChangePasswordModalCtrl',
                controllerAs: 'ctrl',
                resolve: {
                    item: function () {
                        return item;
                    }
                }
            });


        }

        _this.showErrors = function (errors) {
            var modalInstance = $modal.open({
                templateUrl: '/html/errors.html',
                controller: 'ModalInstanceCtrl',
                controllerAs: 'ctrl',
                resolve: {
                    errors: function () {
                        return errors;
                    }
                }
            });
        }

        if (_this.action == 'list') {
            _this.refresh()
        }
    });

    app.controller('CallsCtrl', function () {
        var _this = this
    });
})();