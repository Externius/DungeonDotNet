(function () {
    "use strict";
    angular.module("dungeon-options")
        .controller("dungeonLoaderController", dungeonLoaderController);

    function dungeonLoaderController($routeParams, $http) {
        var vm = this;
        vm.dungeonName = $routeParams.dungeonName;
        vm.savedDungeons = [];
        vm.errorMessage = "";
        vm.isBusy = false;
        vm.hidden = true;
        vm.themeID = $("#theme").val();
        Utils.preloadImages();
        vm.load = function () {
            vm.isBusy = true;
            vm.themeID = $("#theme").val();
            $http.get("/api/options/" + vm.dungeonName + "/saveddungeons")
            .then(function (response) {
                Utils.drawDungeonOneCanvas(JSON.parse(response.data.savedDungeons[0].dungeonTiles),
                    JSON.parse(response.data.savedDungeons[0].roomDescription),
                    response.data.savedDungeons[0].trapDescription === null ? [] : JSON.parse(response.data.savedDungeons[0].trapDescription),
                    "mapArea",
                    response.data.dungeonSize,
                    response.data.corridor,
                    vm.themeID,
                    response.data.savedDungeons[0].roamingMonsterDescription === null ? [] : JSON.parse(response.data.savedDungeons[0].roamingMonsterDescription));
                Utils.downloadImg("download_map", document.getElementById("mapArea"));
                Utils.downloadDescription("download_description", "DungeonRooms.csv");
                Utils.downloadHTML("download_html");
                vm.hidden = false;
            }, function (error) {
                vm.errorMessage = "Error getting the saved dungeon: " + error;
            })
            .finally(function () {
                vm.isBusy = false;
            });
        };
        vm.substr = function (param) {
            return param.substr(0, 8) + "...";
        };
    }
})();