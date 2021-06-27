// URLs
const Character_GetCharacterLevelUp = "/api/palantir/Character_GetCharacterLevelUp";

// divs
const modelJumbo = "#modelJumbo";
let playerId;
let characterId;



// on page load
getURIinfo();
getCharacterLevelUp(playerId, characterId);


// events


// functions
function getURIinfo() {
    var a = window.location.href.split("=")[1];
    var b = decodeURIComponent(a);
    var c = JSON.parse(b);

    playerId = c.PlayerId;
    characterId = c.CharacterId;
}

function getCharacterLevelUp(playerId, characterId) {
    var object = {
        PlayerId: playerId,
        CharacterId: characterId
    }
    var request = {
        message: JSON.stringify(object)
    }

    $.ajax({
        type: "GET",
        url: Character_GetCharacterLevelUp,
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






