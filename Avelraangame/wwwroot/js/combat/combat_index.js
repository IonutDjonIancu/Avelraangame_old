// js template
// URLs
const GetCharacter = "/api/palantir/GetCharacter";
// divs
const storyModeBtn = "#storyModeBtn";
const dungeonBtn = "#dungeonBtn";
const arenaBtn = "#arenaBtn";
let playerId;
let playerName;
let characterId;

// objects

// on page load

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