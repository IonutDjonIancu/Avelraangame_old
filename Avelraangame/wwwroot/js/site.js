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



// functions
function drawOneCharacter_base(data, whereTo) {
    $(whereTo).empty();
    var pp = `<p>Selected character</p>`;
    $(whereTo).append(pp);

    if (data.InFight == true) {
        window.location = `/Combat/Combat_index`;
    } else {
        var html = `
            <button id="${data.CharacterId}" class="btn btn-outline-info characterBtn">
                ${data.Name}
                <span>
                    <img style="border-radius:10px" src="../media/images/humans/human${data.Logbook.PortraitNr}.png"/>
                </span>
            </button>
        `;
    }

    $(whereTo).append(html);
}

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

function base_drawCharacters(data, whereTo) {
    $(whereTo).empty();

    for (var i = 0; i < data.length; i++) {

        var html = `
            <button id="${data[i].CharacterId}" class="btn btn-outline-dark characterBtn">
                ${data[i].Name}
                <span>
                    <img style="border-radius:10px" src="../media/images/humans/human${data[i].Logbook.PortraitNr}.png"/>
                </span>
            </button>
        `;

        $(whereTo).append(html);
    }
}

function base_drawCharactersInParty(data, whereTo) {
    $(whereTo).empty();

    for (var i = 0; i < data.length; i++) {
        if (data[i].InParty) {
            var html = `
                <button id="${data[i].CharacterId}" class="btn btn-outline-info characterBtn">
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

function base_drawCharactersInFight(data, whereTo) {
    $(whereTo).empty();

    for (var i = 0; i < data.length; i++) {

        var html = `
            <button id="${data[i].CharacterId}" value="${data[i].FightId}" class="btn btn-outline-warning characterBtn">
                ${data[i].Name}
                <span>
                    <img style="border-radius:10px" src="../media/images/humans/human${data[i].Logbook.PortraitNr}.png"/>
                </span>
            </button>
        `;

        $(whereTo).append(html);
    }
}


function base_getAliveCharactersByPlayerId(playerId, playerName, callback) {

    var object = {
        PlayerId: playerId,
        PlayerName: playerName
    }
    var request = {
        message: JSON.stringify(object)
    }

    $.ajax({
        type: "GET",
        url: "/api/palantir/GetAliveCharactersByPlayerId",
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





// texts
let huurlingTexts = {
    "Huurling_prologue": "Duty, honor or payment, they’re all the same to you.",
    "Huurling_act1_sellsword": "The world turns, the days pass...",
    "Huurling_epilogue": "You go about your day."
};








