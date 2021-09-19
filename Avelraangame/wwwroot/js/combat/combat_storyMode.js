// URLs
const GetCharacter = "/api/palantir/GetCharacter";
// divs
const nameDiv = "#nameDiv";
const charactersSelectDiv = "#charactersSelectDiv";
let playerId;
let playerName;
let characterId;
let characters;

// on page load
playerId = establishPlayerIdBase();
playerName = establishPlayerNameBase();

if (!characterId) {
    getCharactersByPlayerNameAndIdBase(playerName, playerId, function (res) {
        characters = res;

        drawCharactersBase(res, charactersSelectDiv);



    });
}




// events
// functions


function getCharacter(playerId, characterId) {

    var object = {
        PlayerId: playerId,
        CharacterId: characterId
    }
    var request = {
        message: JSON.stringify(object)
    }

    $.ajax({
        type: "GET",
        url: GetCharacter,
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
            drawCharacter(data);
        },
        error: function (err) {
            console.log(err);
        }
    });

}