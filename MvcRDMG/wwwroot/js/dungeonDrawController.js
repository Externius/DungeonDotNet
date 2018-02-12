(function () {

    "use strict";

    angular.module("dungeon-generator")
        .controller("dungeonDrawController", dungeonDrawController);

    function dungeonDrawController($http) {
        var vm = this;
        vm.newDungeon = {
            dungeonSize: $("#dungeonSize").val(),
            dungeonDifficulty: $("#dungeonDifficulty").val(),
            partyLevel: $("#partyLevel").val(),
            partySize: $("#partySize").val(),
            treasureValue: $("#treasureValue").val(),
            itemsRarity: $("#itemsRarity").val(),
            roomDensity: $("#roomDensity").val(),
            roomSize: $("#roomSize").val(),
            monsterType: $("#monsterType").val(),
            trapPercent: $("#trapPercent").val(),
            deadEnd: $("#deadEnd").val(),
            corridor: $("#corridor").val()
        };
        vm.errorMessage = "";
        vm.isBusy = false;
        vm.hidden = true;
        vm.generateDungeon = function () {
            vm.themeID = $("#theme").val();
            vm.reloadVal();
            Utils.preloadImages();
            vm.isBusy = true;
            vm.errorMessage = "";
            $http.post("/api/generate", vm.newDungeon)
                .then(function (response) {
                    vm.tiles = response.data[0].dungeonTiles;
                    vm.roomDescription = response.data[0].roomDescription;
                    vm.trapDescription = response.data[0].trapDescription;
                    Utils.drawDungeonOneCanvas(JSON.parse(response.data[0].dungeonTiles),
                        JSON.parse(response.data[0].roomDescription), vm.trapDescription === null ? [] : JSON.parse(response.data[0].trapDescription),
                        "mapArea",
                        vm.newDungeon.dungeonSize,
                        vm.newDungeon.corridor === "true",
                        vm.themeID);
                    Utils.downloadImg("download_map", document.getElementById("mapArea"));
                    Utils.downloadDescription("download_description", "DungeonRooms.csv");
                    Utils.downloadHTML("download_html");
                    vm.hidden = false;
                }, function (error) {
                    vm.errorMessage = "Failed to generate dungeon " + error;
                })
                .finally(function () {
                    vm.isBusy = false;
                });
        };
        vm.reloadVal = function () {
            vm.newDungeon.dungeonSize = $("#dungeonSize").val();
            vm.newDungeon.dungeonDifficulty = $("#dungeonDifficulty").val();
            vm.newDungeon.partyLevel = $("#partyLevel").val();
            vm.newDungeon.partySize = $("#partySize").val();
            vm.newDungeon.treasureValue = $("#treasureValue").val();
            vm.newDungeon.itemsRarity = $("#itemsRarity").val();
            vm.newDungeon.roomDensity = $("#roomDensity").val();
            vm.newDungeon.roomSize = $("#roomSize").val();
            vm.newDungeon.monsterType = $("#monsterType").val();
            vm.newDungeon.trapPercent = $("#trapPercent").val();
            vm.newDungeon.deadEnd = $("#deadEnd").val();
            vm.newDungeon.corridor = $("#corridor").val();
        };
        vm.saveDungeon = function () {
            vm.isBusy = true;
            vm.errorMessage = "";
            vm.newSavedDungeon = {
                dungeonTiles: vm.tiles,
                roomDescription: vm.roomDescription,
                trapDescription: vm.trapDescription
            };
            $http.post("/api/options", vm.newDungeon)
                .then(function () {
                    $http.post("/api/options/" + vm.newDungeon.dungeonName + "/saveddungeons", vm.newSavedDungeon)
                        .then(function () {
                            vm.newSavedDungeon = {};
                        }, function (error) {
                            vm.errorMessage = "Failed to add new dungeon: " + error;
                        });
                }, function () {
                    vm.errorMessage = "There is already a dungeon named:" + vm.newDungeon.dungeonName;
                })
                .finally(function () {
                    vm.isBusy = false;
                });
        };
    }
})();