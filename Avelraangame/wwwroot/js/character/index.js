// URLs
const GetPlayerIdByName = "/api/palantir/GetPlayerIdByName";

// divs
const players = "#players";
const welcome = "#welcome";


// on page load
establishPlayer();






// events
$(players).on("change", function () {

    var playerName = $(players)[0].value;

    if (!playerName) {
        return;
    }

    var object = {
        PlayerName: playerName
    }
    var request = {
        message: JSON.stringify(object)
    }

    $.ajax({
        type: "GET",
        url: GetPlayerIdByName,
        contentType: "text",
        data: request,
        success: function (resp) {
            var response = JSON.parse(resp);

            if (response.Error) {
                console.log(response.Error);
                return;
            }

            setSessionCredentials(playerName, response.Data);
            $(welcome).empty();
            $(welcome).append(`Welcome: ${playerName}`);

        },
        error: function (err) {
            console.log(err);
        }
    });







});

// functions
function setSessionCredentials(playerName, playerId) {
    localStorage.setItem("playerName", playerName);
    localStorage.setItem("playerId", playerId);
}

function establishPlayer() {
    var name = localStorage.getItem("playerName");

    if (name) {
        $(welcome).append(`Welcome: ${name}`);
    }
}
