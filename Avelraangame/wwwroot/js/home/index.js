// URLs
const GetPlayerIdByName = "/api/palantir/GetPlayerIdByName";
const Logon = "/api/palantir/Logon";

// variables
const playerNameDiv = "#playerNameDiv";
const powerWardInput = "#powerWardInput";
const nameInput = "#nameInput";
const loggedDiv = "#loggedDiv";
const nonLoggedDiv = "#nonLoggedDiv";
const logonBtn = "#logonBtn";
const symbolWard = "#symbolWard";
const createPlayerBtn = "#createPlayerBtn";
const logoutBtn = "#logoutBtn";
let playerId;
let playerName;



// on page load
playerId = establishPlayerId_base();
playerName = establishPlayerName_base();

if (playerName) {
    $(loggedDiv).show();
    $(loggedDiv).empty();
    $(loggedDiv).append(`Welcome, ${playerName} ...`);
} else {
    $(nonLoggedDiv).show();
}


// events
//$(createPlayerBtn).on("click", function () {
//    window.location = "/Player";
//});

$(logoutBtn).on("click", function () {
    localStorage.clear();
    location.reload();
});

$(logonBtn).on("click", function () {
    var object = {
        Symbol: $(symbolWard).val(),
        Ward: $(powerWardInput).val(),
        PlayerName: $(nameInput).val(),
    }
    var request = {
        message: JSON.stringify(object)
    }

    $.ajax({
        type: "POST",
        url: Logon,
        contentType: "application/json",
        data: JSON.stringify(request),
        success: function (resp) {
            var response = JSON.parse(resp);

            if (response.Error) {
                console.log(response.Error);
                alert(response.Error);
                return;
            }

            var data = JSON.parse(response.Data);

            localStorage.setItem("playerId", data.PlayerId);
            localStorage.setItem("playerName", data.PlayerName);

            location.reload();
        },
        error: function (err) {
            console.log(err);
        }
    });


});


// functions





