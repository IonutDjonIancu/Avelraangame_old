// divs
const aliveBtn = "#aliveBtn";
const deadBtn = "#deadBtn";
let playerId;
let playerName;

// on page load
playerId = establishPlayerId_base();
playerName = establishPlayerName_base();

localStorage.removeItem("characterId");

base_getCharactersByPlayerId(playerId, playerName, function (data) {

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

