// URLs
const GetCharacter = "/api/palantir/GetCharacter";

// divs
const nameDiv = "#nameDiv";
let playerId;
let playerName;
let characterId;

// on page load
playerId = establishPlayerId_base();
playerName = establishPlayerName_base();

base_getAliveCharactersByPlayerId(playerName, playerId, function (res) {
    drawCharactersNonFight_base(res, charactersDiv);
    drawCharactersInFights_base(res, charactersInFightDiv);
    addLoadCharacterClickEvent();
});



// events



// functions













