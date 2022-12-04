var Utils = (function () {
    var SOURCES = [
        '/images/dark/marble.png',
        '/images/dark/corridor.png',
        '/images/dark/door.png',
        '/images/dark/room.png',
        '/images/dark/entry.png',
        '/images/dark/trap.png',
        '/images/dark/room_edge.png',
        '/images/dark/nc_door.png',
        '/images/dark/door_locked.png',
        '/images/dark/door_trapped.png',
        '/images/dark/nc_door_locked.png',
        '/images/dark/nc_door_trapped.png',
        '/images/dark/monster.png',
        '/images/light/marble.png',
        '/images/light/room.png',
        '/images/light/room_edge.png',
        '/images/light/nc_door.png',
        '/images/light/nc_door_locked.png',
        '/images/light/nc_door_trapped.png',
        '/images/white/marble.png',
        '/images/white/room.png',
        '/images/white/nc_door.png',
        '/images/white/nc_door_locked.png',
        '/images/white/nc_door_trapped.png'
    ];
    var IMAGEOBJECT = [];
    var THEMEINDEX = [];
    var getFontSize = function (dungeonSize) {
        if (dungeonSize > 30) {
            return "9pt Calibri bold";
        } else {
            return "10pt Calibri bold";
        }
    };
    var createTableNode = function (nodeText, isRoot, parent) {
        var tr;
        if (parent === undefined) {
            tr = document.createElement('tr');
        } else {
            tr = parent;
        }
        var td = document.createElement('td');
        var text = document.createTextNode(nodeText);
        if (isRoot) {
            td.colSpan = 5;
            td.setAttribute('class', 'font-weight-bold text-center root');
        }
        td.appendChild(text);
        tr.appendChild(td);
        return tr;
    };
    var addDescription = function (roomDescription, trapDescription, roamingDescription, descId) {
        var table = document.getElementById(descId);
        var tr;
        table.innerHTML = "";
        for (var i = 0; i < roomDescription.length; i++) {
            tr = createTableNode(roomDescription[i].Name, true);
            table.appendChild(tr);
            tr = createTableNode(roomDescription[i].Monster, false);
            table.appendChild(tr);
            tr = createTableNode(roomDescription[i].Treasure, false);
            table.appendChild(tr);
            tr = createTableNode(roomDescription[i].Doors, false);
            table.appendChild(tr);
        }
        for (i = 0; i < trapDescription.length; i++) {
            tr = createTableNode(trapDescription[i].Name, true);
            table.appendChild(tr);
            tr = createTableNode(trapDescription[i].Description, false);
            table.appendChild(tr);
        }
        for (i = 0; i < roamingDescription.length; i++) {
            tr = createTableNode(roamingDescription[i].Name, true);
            table.appendChild(tr);
            tr = createTableNode(roamingDescription[i].Description, false);
            table.appendChild(tr);
        }
    };
    var getDegree = function (tiles, i, j) {
        if (tiles[i][j - 1].Texture === 4) { // left tile is room
            return -90;
        } else if (tiles[i][j + 1].Texture === 4) { // right tile is room
            return 90;
        } else if (tiles[i + 1][j].Texture === 4) { // below tile is room
            return 180;
        }
        return 0; // above tile is room
    };
    var rotateImage = function (context, image, degree, x, y, width, height) {
        if (degree === 90) { // rotate right
            context.translate(x, y);
            context.rotate(degree * Math.PI / 180);
            context.drawImage(image, 0, -height, width, height);
            context.rotate(-degree * Math.PI / 180);
            context.translate(-x, -y);
        } else if (degree === -90) { // rotate left
            context.translate(x, y);
            context.rotate(degree * Math.PI / 180);
            context.drawImage(image, -width, 0, width, height);
            context.rotate(-degree * Math.PI / 180);
            context.translate(-x, -y);
        } else if (degree === 180) { // rotate down
            context.translate(x, y);
            context.rotate(degree * Math.PI / 180);
            context.drawImage(image, -width, -height, width, height);
            context.rotate(-degree * Math.PI / 180);
            context.translate(-x, -y);
        } else { // do not rotate
            context.drawImage(image, x, y, width, height);
        }
    };
    var setBase = function (corridorIndex, doorIndex, entryIndex, trapIndex, doorLockedIndex, doorTrappedIndex, monsterIndex) {
        THEMEINDEX[1] = corridorIndex;
        THEMEINDEX[2] = doorIndex;
        THEMEINDEX[4] = entryIndex;
        THEMEINDEX[5] = trapIndex;
        THEMEINDEX[8] = doorLockedIndex;
        THEMEINDEX[9] = doorTrappedIndex;
        THEMEINDEX[12] = monsterIndex;
    };
    var setTheme = function (marbleIndex, roomIndex, roomEdgeIndex, ncDoorIndex, ncDoorLockedIndex, ncDoorTrappedIndex) {
        THEMEINDEX[0] = marbleIndex;
        THEMEINDEX[3] = roomIndex;
        THEMEINDEX[6] = roomEdgeIndex;
        THEMEINDEX[7] = ncDoorIndex;
        THEMEINDEX[10] = ncDoorLockedIndex;
        THEMEINDEX[11] = ncDoorTrappedIndex;
    };
    var getTheme = function (themeID) {
        THEMEINDEX = [];
        setBase(1, 2, 4, 5, 8, 9, 12);
        switch (themeID) {
            case "0": // dark
                setTheme(0, 3, 6, 7, 10, 11);
                break;
            case "1": // light
                setTheme(13, 14, 15, 16, 17, 18);
                break;
            case "2": // minimal
                setTheme(19, 20, 19, 21, 22, 23);
                break;
            default:
                break;
        }
    };
    var drawMap = function (tiles, context, contextFont, hasCorridor) {
        for (var i = 1; i < tiles.length - 1; i++) {
            for (var j = 1; j < tiles[i].length - 1; j++) {
                switch (tiles[i][j].Texture) {
                    case 1: // marble
                        context.drawImage(IMAGEOBJECT[THEMEINDEX[0]], tiles[i][j].X, tiles[i][j].Y, tiles[i][j].Width, tiles[i][j].Height);
                        break;
                    case 2: // corridor
                        context.drawImage(IMAGEOBJECT[THEMEINDEX[1]], tiles[i][j].X, tiles[i][j].Y, tiles[i][j].Width, tiles[i][j].Height);
                        break;
                    case 3: // door
                        rotateImage(context, IMAGEOBJECT[THEMEINDEX[2]], getDegree(tiles, i, j), tiles[i][j].X, tiles[i][j].Y, tiles[i][j].Width, tiles[i][j].Height);
                        break;
                    case 4: // room
                        context.drawImage(IMAGEOBJECT[THEMEINDEX[3]], tiles[i][j].X, tiles[i][j].Y, tiles[i][j].Width, tiles[i][j].Height);
                        context.font = contextFont;
                        context.fillText(tiles[i][j].Description, tiles[i][j].X + Math.round(tiles[i][j].Width * 0.1), tiles[i][j].Y + Math.round(tiles[i][j].Height * 0.65));
                        break;
                    case 5: // entry
                        context.drawImage(IMAGEOBJECT[THEMEINDEX[4]], tiles[i][j].X, tiles[i][j].Y, tiles[i][j].Width, tiles[i][j].Height);
                        break;
                    case 6: // trap
                        context.drawImage(IMAGEOBJECT[THEMEINDEX[5]], tiles[i][j].X, tiles[i][j].Y, tiles[i][j].Width, tiles[i][j].Height);
                        context.font = contextFont;
                        context.fillText(tiles[i][j].Description, tiles[i][j].X + Math.round(tiles[i][j].Width * 0.1), tiles[i][j].Y + Math.round(tiles[i][j].Height * 0.5));
                        break;
                    case 7: // room_edge
                        context.drawImage(hasCorridor ? IMAGEOBJECT[THEMEINDEX[0]] : IMAGEOBJECT[THEMEINDEX[6]], tiles[i][j].X, tiles[i][j].Y, tiles[i][j].Width, tiles[i][j].Height);
                        break;
                    case 8: // nc_Door
                        rotateImage(context, IMAGEOBJECT[THEMEINDEX[7]], getDegree(tiles, i, j), tiles[i][j].X, tiles[i][j].Y, tiles[i][j].Width, tiles[i][j].Height);
                        break;
                    case 9: // door_locked
                        rotateImage(context, IMAGEOBJECT[THEMEINDEX[8]], getDegree(tiles, i, j), tiles[i][j].X, tiles[i][j].Y, tiles[i][j].Width, tiles[i][j].Height);
                        break;
                    case 10: // door_trapped
                        rotateImage(context, IMAGEOBJECT[THEMEINDEX[9]], getDegree(tiles, i, j), tiles[i][j].X, tiles[i][j].Y, tiles[i][j].Width, tiles[i][j].Height);
                        break;
                    case 11: // nc_door_locked
                        rotateImage(context, IMAGEOBJECT[THEMEINDEX[10]], getDegree(tiles, i, j), tiles[i][j].X, tiles[i][j].Y, tiles[i][j].Width, tiles[i][j].Height);
                        break;
                    case 12: // nc_door_trapped
                        rotateImage(context, IMAGEOBJECT[THEMEINDEX[11]], getDegree(tiles, i, j), tiles[i][j].X, tiles[i][j].Y, tiles[i][j].Width, tiles[i][j].Height);
                        break;
                    case 13: // roaming monster
                        context.drawImage(IMAGEOBJECT[THEMEINDEX[12]], tiles[i][j].X, tiles[i][j].Y, tiles[i][j].Width, tiles[i][j].Height);
                        context.font = contextFont;
                        context.fillText(tiles[i][j].Description, tiles[i][j].X + Math.round(tiles[i][j].Width * 0.1), tiles[i][j].Y + Math.round(tiles[i][j].Height * 0.4));
                        break;
                    default:
                        context.drawImage(IMAGEOBJECT[THEMEINDEX[0]], tiles[i][j].X, tiles[i][j].Y, tiles[i][j].Width, tiles[i][j].Height);
                        break;
                }
            }
        }
    };
    var drawPlainMap = function (tiles, context, hasCorridor) {
        for (var i = 1; i < tiles.length - 1; i++) {
            for (var j = 1; j < tiles[i].length - 1; j++) {
                switch (tiles[i][j].Texture) {
                    case 1: // marble
                        context.drawImage(IMAGEOBJECT[THEMEINDEX[0]], tiles[i][j].X, tiles[i][j].Y, tiles[i][j].Width, tiles[i][j].Height);
                        break;
                    case 2: // corridor
                    case 6: // trap
                    case 13: // roaming monster
                        context.drawImage(IMAGEOBJECT[THEMEINDEX[1]], tiles[i][j].X, tiles[i][j].Y, tiles[i][j].Width, tiles[i][j].Height);
                        break;
                    case 3: // door
                    case 9: // door_locked
                    case 10: // door_trapped
                        rotateImage(context, IMAGEOBJECT[THEMEINDEX[2]], getDegree(tiles, i, j), tiles[i][j].X, tiles[i][j].Y, tiles[i][j].Width, tiles[i][j].Height);
                        break;
                    case 4: // room
                        context.drawImage(IMAGEOBJECT[THEMEINDEX[3]], tiles[i][j].X, tiles[i][j].Y, tiles[i][j].Width, tiles[i][j].Height);
                        break;
                    case 5: // entry
                        context.drawImage(IMAGEOBJECT[THEMEINDEX[4]], tiles[i][j].X, tiles[i][j].Y, tiles[i][j].Width, tiles[i][j].Height);
                        break;
                    case 7: // room_edge
                        context.drawImage(hasCorridor ? IMAGEOBJECT[THEMEINDEX[0]] : IMAGEOBJECT[THEMEINDEX[6]], tiles[i][j].X, tiles[i][j].Y, tiles[i][j].Width, tiles[i][j].Height);
                        break;
                    case 8: // nc_Door
                    case 11: // nc_door_locked
                    case 12: // nc_door_trapped
                        rotateImage(context, IMAGEOBJECT[THEMEINDEX[7]], getDegree(tiles, i, j), tiles[i][j].X, tiles[i][j].Y, tiles[i][j].Width, tiles[i][j].Height);
                        break;
                    default:
                        break;
                }
            }
        }
    };
    var generatePlainMap = function (tiles, hasCorridor, canvasPlainId) {
        var canvasPlain = document.getElementById(canvasPlainId);
        var contextPlain = canvasPlain.getContext("2d");
        contextPlain.clearRect(0, 0, canvasPlain.width, canvasPlain.height); // clear canvas
        drawPlainMap(tiles, contextPlain, hasCorridor.toLowerCase() === "true");
    };
    var drawDungeonOneCanvas = function (tiles, roomDescription, trapDescription, canvasID, dungeonSize, hasCorridor, themeID, roamingDescription, descId) {
        var canvas = document.getElementById(canvasID);
        var contextFont = getFontSize(dungeonSize);
        var context = canvas.getContext("2d"); // get canvas context
        context.clearRect(0, 0, canvas.width, canvas.height); // clear canvas
        addDescription(roomDescription, trapDescription, roamingDescription, descId);
        getTheme(themeID);
        drawMap(tiles, context, contextFont, hasCorridor.toLowerCase() === "true");
    };
    var downloadHTML = function (linkID) {
        var link = document.getElementById(linkID);
        link.hidden = false;
        link.style.display = "inline-block";
        var canvas = document.getElementById("mapArea");
        var table = document.getElementById("table_description");
        var tableClone = table.cloneNode(true);
        var doc = document.implementation.createHTMLDocument("DungeonMap");
        var img = document.createElement("img");
        var style = document.createElement('style');
        var head = doc.getElementsByTagName("head")[0];
        var css = "table, th, td {border-collapse: collapse;} " +
            "th, td {padding: 8px; text-align: left; border-bottom: 1px solid #ddd;}" +
            ".wrap { white-space: pre-wrap;}";
        img.src = canvas.toDataURL();
        doc.body.appendChild(img);
        doc.body.appendChild(tableClone);
        style.innerHTML = css;
        head.appendChild(style);
        link.href = "data:text/html;charset=UTF-8," + encodeURIComponent(doc.documentElement.outerHTML);
        link.download = "dungeonmap.html";
    };
    var donwloadCSV = function (linkID, csv, fileName) {
        var csvFile = new Blob([csv], {
            type: "text/csv"
        });
        var link = document.getElementById(linkID);
        link.hidden = false;
        link.style.display = "inline-block";
        link.download = fileName;
        link.href = window.URL.createObjectURL(csvFile);
    };
    var downloadDescription = function (linkID, fileName) {
        var csv = [];
        var description = document.getElementById("table_description");
        var rows = description.querySelectorAll("table tr");
        for (var i = 0; i < rows.length; i++) {
            var row = [],
                cols = rows[i].querySelectorAll("td, th");
            for (var j = 0; j < cols.length; j++)
                row.push(cols[j].innerText);
            csv.push(row.join(","));
        }
        donwloadCSV(linkID, csv.join("\n"), fileName);
    };
    var downloadImg = function (linkID, canvas) {
        var link = document.getElementById(linkID);
        link.hidden = false;
        link.style.display = "inline-block";
        link.href = canvas.toDataURL();
        link.download = "dungeonmap.png";
    };
    var corridorOnchange = function (e) {
        if (e.value === "true") {
            document.getElementById("RoomDensity").disabled = false;
            document.getElementById("TrapPercent").disabled = false;
            document.getElementById("DeadEnd").disabled = false;
            monsterTypeOnChange(document.getElementById("MonsterType"));
        } else {
            document.getElementById("RoomDensity").disabled = true;
            document.getElementById("TrapPercent").disabled = true;
            document.getElementById("DeadEnd").disabled = true;
            document.getElementById("RoamingPercent").disabled = true;
        }
    };
    var monsterTypeOnChange = function (e) {
        if (e.selectedOptions.length === 0) {
            document.getElementById("RoamingPercent").disabled = true;
            document.getElementById("RoamingPercent").selectedIndex = 0;
        } else if (document.getElementById("Corridor").value === "true") {
            document.getElementById("RoamingPercent").disabled = false;
        }
    };
    var preloadImages = function () {
        IMAGEOBJECT = [];
        for (var i = 0; i < SOURCES.length; i++) {
            var j = IMAGEOBJECT.length;
            IMAGEOBJECT[j] = new Image();
            IMAGEOBJECT[j].src = SOURCES[i];
        }
    };
    var setCanvasSize = function (canvasDiv, canvasId) {
        var size = Math.round((canvasDiv.width() / 100)) * 100;
        var canvas = document.getElementById(canvasId);
        canvas.width = size;
        canvas.height = size;
        var context = canvas.getContext("2d");
        context.clearRect(0, 0, canvas.width, canvas.height);
    };
    return {
        corridorOnchange: corridorOnchange,
        monsterTypeOnChange: monsterTypeOnChange,
        downloadImg: downloadImg,
        downloadDescription: downloadDescription,
        downloadHTML: downloadHTML,
        preloadImages: preloadImages,
        drawDungeonOneCanvas: drawDungeonOneCanvas,
        setCanvasSize: setCanvasSize,
        generatePlainMap: generatePlainMap
    };
})();