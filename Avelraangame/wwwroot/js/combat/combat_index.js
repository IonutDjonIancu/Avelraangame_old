// js template
// URLs
const GetCharacter = "/api/palantir/GetCharacter";
// divs
const storyModeBtn = "#storyModeBtn";
const charactersInFightDiv = "#charactersInFightDiv";
const charactersDiv = "#charactersDiv";
const dungeonBtn = "#dungeonBtn";
const arenaBtn = "#arenaBtn";
let playerId;
let playerName;
let characterId;

// objects

// on page load
localStorage.removeItem("characterId");
playerId = establishPlayerId_base();
playerName = establishPlayerName_base();

getCharactersByPlayerNameAndId_base(playerName, playerId, function (res) {
    drawCharactersNonFight_base(res, charactersDiv);
    drawCharactersInFights_base(res, charactersInFightDiv);
    addLoadCharacterClickEvent();
});


// events
$(storyModeBtn).on("click", function () {
    if (characterId) {
        window.location = `/Combat/Combat_storyMode`;
    } else {
        console.log("set character first");
    }


});

$(dungeonBtn).on("click", function () {
    if (characterId) {
        window.location = `/Combat/Combat_dungeon`;
    } else {
        console.log("set character first");
    }
});

$(arenaBtn).on("click", function () {
    if (characterId) {
        window.location = `/Combat/Combat_arena`;
    } else {
        console.log("set character first");
    }

});


// functions
function addLoadCharacterClickEvent() {
    $(".characterBtn").on("click", function () {

        if (this.title == "loadFight") {
            localStorage.setItem("characterId", this.id);
            window.location = `/Combat/Fight`;
        } else {
            var otherChars = $(".characterBtn");
            for (var i = 0; i < otherChars.length; i++) {
                if (otherChars[i].title != "loadFight") {
                    otherChars[i].className = "btn btn-outline-info characterBtn";
                }
            }
            this.className = "btn btn-info characterBtn";

            characterId = this.id;
            localStorage.setItem("characterId", this.id);
        }

    });
}
