(function(){
    app.controller('LoginCtrl', function ($modal, $window, accountService) {
        var _this = this

        _this.logIn = function (loginForm) {
            if (loginForm.$valid) {
                accountService.logIn(_this.email, _this.password).then(
                    function (token) {
                        $window.sessionStorage.token = token
                        $window.location.href = '/control-panel'
                    }, function (reason) {
                        errors = []
                        if (isFinite(reason)) {
                            errors.push('HTTP Error: ' + reason)
                        } else {
                            switch (reason) {
                                case 'INVALID_USER_PWD': reason = 'Invalid email or password'; break
                                case 'INACTIVE': reason = 'Your account is inactive'; break
                                case 'TEMPORARILY_DISABLED': reason = 'Your account has been temporarily disabled due to many unsuccessful login attempts. Try again later.'; break
                                default: reason = 'Unknown code: ' + reason
                            }
                            errors.push(reason)
                        }
                        _this.showErrors(errors)
                    }
                )
            }
            else
            {
                errors = []
                if (loginForm.email.$error.required) {
                    errors.push('The email is required')
                } else if (loginForm.email.$error.email) {
                    errors.push('The email entered is invalid')
                }
                if (loginForm.password.$error.required) {
                    errors.push('The password is required')
                }
                _this.showErrors(errors)
            }
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

            onclose = function () {
                _this.password = ''
            }

            modalInstance.result.then(onclose, onclose);
        }
    });
})();