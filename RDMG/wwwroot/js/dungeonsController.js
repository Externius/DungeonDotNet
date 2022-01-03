(function () {

    "use strict";

    angular.module("dungeon-options")
        .controller("dungeonsController", dungeonsController);

    function dungeonsController($http) {
        var vm = this;
        vm.options = [];
        vm.errorMessage = "";
        vm.isBusy = true;
        $http.get("/api/options") // get saved dungeons 
            .then(function (response) {
                angular.copy(response.data, vm.options);
            }, function (error) {
                vm.errorMessage = "Error getting the data: " + error;
            })
            .finally(function () {
                vm.isBusy = false;
            });
    }
})();