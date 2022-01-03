(function () {
    "use strict";
    angular.module("dungeon-options", ["simpleControls", "ngRoute"])
        .config(function ($routeProvider) {
            $routeProvider
                .when("/", {
                    controller: "dungeonsController",
                    controllerAs: "vm",
                    templateUrl: "/views/dungeonsView.html"
                });
            $routeProvider
                .when("/editor/:dungeonName", {
                    controller: "dungeonLoaderController",
                    controllerAs: "vm",
                    templateUrl: "/views/dungeonLoaderView.html"
                });
            $routeProvider.otherwise({
                redirectTo: "/"
            });
        });
})();