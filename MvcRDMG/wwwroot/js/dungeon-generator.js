(function () {
    "use strict";
    angular.module("dungeon-generator", ["simpleControls", "ngRoute"])
        .config(function ($routeProvider) {
            $routeProvider
                .when("/", {
                    controller: "dungeonDrawController",
                    controllerAs: "vm",
                    templateUrl: "/views/generateView.html"
                });
            $routeProvider.otherwise({
                redirectTo: "/"
            });
        });
})();