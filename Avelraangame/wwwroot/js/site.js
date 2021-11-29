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
            <button id="${data[i].CharacterId}" class="btn btn-outline-dark characterBtn" value="inTavern">
                ${data[i].Name}
                <span>
                    <img style="border-radius:10px" src="../media/images/humans/human${data[i].Logbook.PortraitNr}.png"/>
                </span>
            </button>
        `;

        $(whereTo).append(html);
    }
}

function base_drawGoodGuys(data, whereTo) {
    $(whereTo).empty();

    for (var i = 0; i < data.length; i++) {

        var hp;
        if (data[i].Assets.Health <= 500) {
            hp = "black";
        } else if (data[i].Assets.Health > 500 && data[i].Assets.Health <= 1000) {
            hp = "darkgreen";
        } else if (data[i].Assets.Health > 1000 && data[i].Assets.Health <= 3000) {
            hp = "yellow";
        } else if (data[i].Assets.Health > 3000 && data[i].Assets.Health <= 5000) {
            hp = "coral";
        } else if (data[i].Assets.Health > 5000 && data[i].Assets.Health <= 9000) {
            hp = "darkred";
        } else {
            hp = "white";
        }

        var drm;
        if (data[i].Expertise.DRM <= 10) {
            drm = "black";
        } else if (data[i].Expertise.DRM > 10 && data[i].Expertise.DRM <= 25) {
            drm = "darkgreen";
        } else if (data[i].Expertise.DRM > 25 && data[i].Expertise.DRM <= 50) {
            drm = "yellow";
        } else if (data[i].Expertise.DRM > 50 && data[i].Expertise.DRM <= 75) {
            drm = "coral";
        } else if (data[i].Expertise.DRM > 75 && data[i].Expertise.DRM <= 89) {
            drm = "darkred";
        } else {
            drm = "white";
        }

        var harm;
        if (data[i].Assets.Harm <= 500) {
            harm = "black";
        } else if (data[i].Assets.Harm > 500 && data[i].Assets.Harm <= 1000) {
            harm = "darkgreen";
        } else if (data[i].Assets.Harm > 1000 && data[i].Assets.Harm <= 3000) {
            harm = "yellow";
        } else if (data[i].Assets.Harm > 3000 && data[i].Assets.Harm <= 5000) {
            harm = "coral";
        } else if (data[i].Assets.Harm > 5000 && data[i].Assets.Harm <= 9000) {
            harm = "darkred";
        } else {
            harm = "white";
        }

        var mana;
        if (data[i].Assets.Mana <= 10) {
            mana = "black";
        } else if (data[i].Assets.Mana > 10 && data[i].Assets.Mana <= 100) {
            mana = "darkgreen";
        } else if (data[i].Assets.Mana > 100 && data[i].Assets.Mana <= 500) {
            mana = "yellow";
        } else if (data[i].Assets.Mana > 500 && data[i].Assets.Mana <= 1000) {
            mana = "coral";
        } else if (data[i].Assets.Mana > 1000 && data[i].Assets.Mana <= 3000) {
            mana = "darkred";
        } else {
            mana = "white";
        }

        var token;
        if (data[i].AttackToken) {
            token = "white";
        } else {
            token = "black";
        }

        var exp;
        if (data[i].Expertise.Experience <= 10) {
            exp = "black";
        } else if (data[i].Expertise.Experience > 10 && data[i].Expertise.Experience <= 100) {
            exp = "darkgreen";
        } else if (data[i].Expertise.Experience > 100 && data[i].Expertise.Experience <= 500) {
            exp = "yellow";
        } else if (data[i].Expertise.Experience > 500 && data[i].Expertise.Experience <= 1000) {
            exp = "coral";
        } else if (data[i].Expertise.Experience > 1000 && data[i].Expertise.Experience <= 3000) {
            exp = "darkred";
        } else {
            exp = "white";
        }

        if (data[i].IsAlive) {
            var html = `
                <div id="${data[i].CharacterId}" class="btn btn-outline-dark goodGuy" style="padding:5px; float:right" title="${data[i].Name}">
                    <div class="container">
                        <div class="row">
                            <div class="col">
                                <div class="row" style="padding:0px">
                                    <span class="material-icons" style="color:${hp}">
                                        favorite_border
                                    </span>
                                    <span class="material-icons" style="color:${drm}">
                                        shield
                                    </span>
                                </div>
                                <div class="row" style="padding:0px">
                                    <span class="material-icons" style="color:${harm}">
                                        fitness_center
                                    </span>
                                    <span class="material-icons" style="color:${mana}">
                                        star_half
                                    </span>
                                </div>
                                <div class="row" style="padding:0px">
                                    <span class="material-icons" style="color:${token}">
                                        radio_button_checked
                                    </span>
                                    <span class="material-icons" style="color:${exp}">
                                        military_tech
                                    </span>
                                </div>
                            </div>
                            <div class="col">
                                <span>
                                    <img style="border-radius:10px" src="../media/images/humans/human${data[i].Logbook.PortraitNr}.png" />
                                </span>
                            </div>
                        </div>
                    </div>
                </div>
            `;
        } else {
            html = `
                <div id="${data[i].CharacterId}" class="btn btn-outline-danger" style="padding:5px; float:right" title="dead">
                    <div class="container">
                        <div class="row">
                            <div class="col">
                                Dead    
                                <span>
                                    <img style="border-radius:10px" src="../media/images/humans/human${data[i].Logbook.PortraitNr}.png" />
                                </span>
                            </div>
                        </div>
                    </div>
                </div>
            `;
        }
        

        $(whereTo).append(html);
    }
}

function base_drawBadGuys(data, whereTo) {
    $(whereTo).empty();

    for (var i = 0; i < data.length; i++) {

        var hp;
        if (data[i].Assets.Health <= 500) {
            hp = "black";
        } else if (data[i].Assets.Health > 500 && data[i].Assets.Health <= 1000) {
            hp = "darkgreen";
        } else if (data[i].Assets.Health > 1000 && data[i].Assets.Health <= 3000) {
            hp = "yellow";
        } else if (data[i].Assets.Health > 3000 && data[i].Assets.Health <= 5000) {
            hp = "coral";
        } else if (data[i].Assets.Health > 5000 && data[i].Assets.Health <= 9000) {
            hp = "darkred";
        } else {
            hp = "white";
        }

        var drm;
        if (data[i].Expertise.DRM <= 10) {
            drm = "black";
        } else if (data[i].Expertise.DRM > 10 && data[i].Expertise.DRM <= 25) {
            drm = "darkgreen";
        } else if (data[i].Expertise.DRM > 25 && data[i].Expertise.DRM <= 50) {
            drm = "yellow";
        } else if (data[i].Expertise.DRM > 50 && data[i].Expertise.DRM <= 75) {
            drm = "coral";
        } else if (data[i].Expertise.DRM > 75 && data[i].Expertise.DRM <= 89) {
            drm = "darkred";
        } else {
            drm = "white";
        }

        var harm;
        if (data[i].Assets.Harm <= 500) {
            harm = "black";
        } else if (data[i].Assets.Harm > 500 && data[i].Assets.Harm <= 1000) {
            harm = "darkgreen";
        } else if (data[i].Assets.Harm > 1000 && data[i].Assets.Harm <= 3000) {
            harm = "yellow";
        } else if (data[i].Assets.Harm > 3000 && data[i].Assets.Harm <= 5000) {
            harm = "coral";
        } else if (data[i].Assets.Harm > 5000 && data[i].Assets.Harm <= 9000) {
            harm = "darkred";
        } else {
            harm = "white";
        }

        var mana
        if (data[i].Assets.Mana <= 10) {
            mana = "black";
        } else if (data[i].Assets.Mana > 10 && data[i].Assets.Mana <= 100) {
            mana = "darkgreen";
        } else if (data[i].Assets.Mana > 100 && data[i].Assets.Mana <= 500) {
            mana = "yellow";
        } else if (data[i].Assets.Mana > 500 && data[i].Assets.Mana <= 1000) {
            mana = "coral";
        } else if (data[i].Assets.Mana > 1000 && data[i].Assets.Mana <= 3000) {
            mana = "darkred";
        } else {
            mana = "white";
        }

        var token;
        if (data[i].AttackToken) {
            token = "white";
        } else {
            token = "black";
        }

        var exp;
        if (data[i].Expertise.Experience <= 10) {
            exp = "black";
        } else if (data[i].Expertise.Experience > 10 && data[i].Expertise.Experience <= 50) {
            exp = "darkgreen";
        } else if (data[i].Expertise.Experience > 50 && data[i].Expertise.Experience <= 500) {
            exp = "yellow";
        } else if (data[i].Expertise.Experience > 500 && data[i].Expertise.Experience <= 1000) {
            exp = "coral";
        } else if (data[i].Expertise.Experience > 1000 && data[i].Expertise.Experience <= 3000) {
            exp = "darkred";
        } else {
            exp = "white";
        }

        var html;

        if (data[i].IsAlive) {
            html = `
                <div id="${data[i].CharacterId}" class="btn btn-outline-dark badGuy" style="padding:5px" title="${data[i].Name}">
                    <div class="container">
                        <div class="row">
                            <div class="col">
                                <span>
                                    <img style="border-radius:10px" src="../media/images/npcs/npc${data[i].Logbook.PortraitNr}.png" />
                                </span>
                            </div>
                            <div class="col">
                                <div class="row" style="padding:0px">
                                    <span class="material-icons" style="color:${hp}">
                                        favorite_border
                                    </span>
                                    <span class="material-icons" style="color:${drm}">
                                        shield
                                    </span>
                                </div>
                                <div class="row" style="padding:0px">
                                    <span class="material-icons" style="color:${harm}">
                                        fitness_center
                                    </span>
                                    <span class="material-icons" style="color:${mana}">
                                        star_half
                                    </span>
                                </div>
                                <div class="row" style="padding:0px">
                                    <span class="material-icons" style="color:black">
                                        radio_button_checked
                                    </span>
                                    <span class="material-icons" style="color:${exp}">
                                        military_tech
                                    </span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            `;
        } else {
            html = `
                <div id="${data[i].CharacterId}" class="btn btn-outline-danger" style="padding:5px" title="dead">
                    <div class="container">
                        <div class="row">
                            <div class="col">
                                <span>
                                    <img style="border-radius:10px" src="../media/images/npcs/npc${data[i].Logbook.PortraitNr}.png" />
                                </span>
                                Dead
                            </div>
                        </div>
                    </div>
                </div>
            `;
        }

        $(whereTo).append(html);
    }
}

function base_drawCharactersInParty(data, whereTo) {
    $(whereTo).empty();

    for (var i = 0; i < data.length; i++) {
        if (data[i].InParty && !data[i].InFight) {
            var html = `
                <button id="${data[i].CharacterId}" class="btn btn-outline-info characterBtn" value="inParty">
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
            <button id="${data[i].CharacterId}" class="btn btn-outline-warning characterBtn" value="inFight">
                ${data[i].Name}
                <span>
                    <img style="border-radius:10px" src="../media/images/humans/human${data[i].Logbook.PortraitNr}.png"/>
                </span>
            </button>
        `;

        $(whereTo).append(html);
    }
}


function base_getCharactersByPlayerId(playerId, playerName, callback) {

    var object = {
        PlayerId: playerId,
        PlayerName: playerName
    }
    var request = {
        message: JSON.stringify(object)
    }

    $.ajax({
        type: "GET",
        url: "/api/palantir/GetCharactersByPlayerId",
        contentType: "application/text",
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

function establishCharacterId_base() {
    var characterId = localStorage.getItem("characterId");

    return characterId;
}

function setSessionCredentials_base(playerId, playerName) {
    localStorage.clear();
    localStorage.setItem("playerId", playerId);
    localStorage.setItem("playerName", playerName);
}









