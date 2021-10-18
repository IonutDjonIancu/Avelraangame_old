// js template
// URLs
const GetCharacter = "/api/palantir/GetCharacter";
// divs
const aliveBtn = "#aliveBtn";
const deadBtn = "#deadBtn";
let playerId;
let playerName;

// objects

// on page load
playerId = establishPlayerId_base();
playerName = establishPlayerName_base();

base_getAliveCharactersByPlayerId(playerName, playerId, function (data) {

    var alive = 0;
    var dead = 0;

    for (var i = 0; i < data.length; i++) {
        if (data[i].IsAlive == true) {
            alive++;
        } else {
            dead++;
        }
    }

    $(aliveBtn).text(alive);
    $(deadBtn).text(dead);
});


// events

// functions
