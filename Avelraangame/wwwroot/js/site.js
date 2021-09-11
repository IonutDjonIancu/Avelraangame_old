// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.



// URLs
// divs
// objects
// on page load
// events
// functions
function establishPlayerIdBase() {
    var playerId = localStorage.getItem("playerId");

    return playerId;
}

function establishPlayerNameBase() {
    var playerName = localStorage.getItem("playerName");

    return playerName;
}
