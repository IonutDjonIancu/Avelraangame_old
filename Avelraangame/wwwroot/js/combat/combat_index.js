// js template
// URLs
const GetCharacter = "/api/palantir/GetCharacter";
// divs
const storyModeBtn = "#storyModeBtn";
const charactersInFightDiv = "#charactersInFightDiv";
const dungeonBtn = "#dungeonBtn";
const arenaBtn = "#arenaBtn";
let playerId;
let playerName;
let characterId;

// objects

// on page load
playerId = establishPlayerId_base();
playerName = establishPlayerName_base();

getCharactersByPlayerNameAndId_base(playerName, playerId, function (res) {

    drawCharactersInFights_base(res, charactersInFightDiv);
    addLoadCharacterClickEvent();
});


// events
$(storyModeBtn).on("click", function () {
    window.location = `/Combat/Combat_storyMode`;
});

$(dungeonBtn).on("click", function () {
    window.location = `/Combat/Combat_dungeon`;
});

$(arenaBtn).on("click", function () {
    window.location = `/Combat/Combat_arena`;
});


// functions
function addLoadCharacterClickEvent() {
    $(".characterBtn").on("click", function () {
        localStorage.setItem("characterId", this.id);
        window.location = `/Combat/Fight_storyMode`;
    });
}
