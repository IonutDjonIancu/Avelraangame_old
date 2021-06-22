// URLs
const getCharactersURL = "/api/palantir/Character_GetCharacters";

// divIDs
const players = "#players";





// on page load




// events
$(players).on("change", function () {

    var playerName = $(players)[0].value;

    getCharactersByPlayerName(playerName);
});



// functions
function getCharactersByPlayerName(playerName) {

    var object = {
        PlayerName: playerName
    }
    var request = {
        message: JSON.stringify(object)
    }


    $.ajax({
        type: "GET",
        url: getCharactersURL,
        contentType: "text",
        data: request,
        success: function (resp) {
            var response = JSON.parse(resp);

            if (response.Error) {
                console.log(response.Error);
                return;
            }

            var data = JSON.parse(response.Data);

            console.log(data);
        },
        error: function (err) {
            console.log(err);
        }
    });


}








