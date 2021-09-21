// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


//// js template
//// URLs
//const GetCharacter = "/api/palantir/GetCharacter";
//// divs
//const nameDiv = "#nameDiv";
//let playerId;
//let playerName;
//let characterId;
//// on page load
//// events
//// functions
//function getCharacter(playerId, characterId) {

//    var object = {
//        PlayerId: playerId,
//        CharacterId: characterId
//    }
//    var request = {
//        message: JSON.stringify(object)
//    }

//    $.ajax({
//        type: "GET",
//        url: GetCharacter,
//        contentType: "text",
//        data: request,
//        success: function (resp) {
//            var response = JSON.parse(resp);

//            if (response.Error) {
//                console.log(response.Error);
//                return;
//            }

//            var data = JSON.parse(response.Data);
//            console.log(data);
//            drawCharacter(data);
//        },
//        error: function (err) {
//            console.log(err);
//        }
//    });

//}

function drawCharacters_base(data, whereTo) {
    $(whereTo).empty();
    var pp = `<p>Select character</p>`;
    $(whereTo).append(pp);

    for (var i = 0; i < data.length; i++) {

        if (data[i].InFight == true) {
            var html = `
                <button title="in a fight" id="${data[i].CharacterId}" class="btn btn-outline-warning characterBtn">
                    ${data[i].Name}
                    <span>
                        <img style="border-radius:10px" src="../media/images/humans/human${data[i].Logbook.PortraitNr}.png"/>
                    </span>
                </button>
            `;
        } else {
            var html = `
                <button id="${data[i].CharacterId}" class="btn btn-outline-info characterBtn">
                    ${data[i].Name}
                    <span>
                        <img style="border-radius:10px" src="../media/images/humans/human${data[i].Logbook.PortraitNr}.png"/>
                    </span>
                </button>
            `;
        }

        $(whereTo).append(html);
    }
}

function drawCharactersInFights_base(data, whereTo) {
    $(whereTo).empty();

    for (var i = 0; i < data.length; i++) {

        if (data[i].InFight == true) {
            var html = `
                <button id="${data[i].CharacterId}" class="btn btn-outline-warning characterBtn">
                    ${data[i].Name}
                    <span>
                        <img style="border-radius:10px" src="../media/images/humans/human${data[i].Logbook.PortraitNr}.png"/>
                    </span>
                </button>
            `;

            $(whereTo).append(html);
        } 
    }
}

function getCharactersByPlayerNameAndId_base(playerName, playerId, callback) {

    var object = {
        PlayerId: playerId,
        PlayerName: playerName
    }
    var request = {
        message: JSON.stringify(object)
    }

    $.ajax({
        type: "GET",
        url: "/api/palantir/GetCharactersByPlayer",
        contentType: "text",
        data: request,
        success: function (resp) {
            var response = JSON.parse(resp);

            if (response.Error) {
                console.log(response.Error);
                return;
            }

            var data = JSON.parse(response.Data);
            callback(data);
        },
        error: function (err) {
            console.log(err);
        }
    });
}

function establishPlayerId_base() {
    var playerId = localStorage.getItem("playerId");

    return playerId;
}

function establishPlayerName_base() {
    var playerName = localStorage.getItem("playerName");

    return playerName;
}

function setSessionCredentials_base(playerId, playerName) {
    localStorage.clear();
    localStorage.setItem("playerId", playerId);
    localStorage.setItem("playerName", playerName);
}