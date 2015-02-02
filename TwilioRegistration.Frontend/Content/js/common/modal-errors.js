(function(){
    app.controller('ModalInstanceCtrl', function ($modalInstance, errors) {
        var _this = this
        _this.errors = errors
        _this.ok = function () {
            $modalInstance.close();
        };
    });
})();