var LoginCtrl = function ($modal) {
    this.logIn = function (form, loginData) {
        if (!form.$valid) {
            alert('here')
        }
    }

    this.showErrors = function (errors, $modal) {
        var modalInstance = $modal.open({
            templateUrl: 'myModalContent.html',
            controller: 'ModalInstanceCtrl',
            size: size,
            resolve: {
                errors: function () {
                    return errors;
                }
            }
        });

        modalInstance.result.then(function (selectedItem) {
            this.password = ''
        });
    }
};

LoginCtrl.$inject = ['$modal'];

angular.module('angApp').controller('LoginCtrl', LoginCtrl);







angular.module('angApp').controller('ModalDemoCtrl', function ($scope, $modal, $log) {

    $scope.items = ['item1', 'item2', 'item3'];

    $scope.open = function (size) {

    };
});

angular.module('angApp').controller('ModalInstanceCtrl', function ($scope, $modalInstance, items) {

    $scope.items = items;
    $scope.selected = {
        item: $scope.items[0]
    };

    $scope.ok = function () {
        $modalInstance.close($scope.selected.item);
    };

    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };
});