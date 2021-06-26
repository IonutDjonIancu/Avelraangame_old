// URLs
const getCharacterURL = "/api/palantir/Character_GetCharacter";




// divIDs
const character = "#modelPool";
let playerId;
let characterId;


// on page load





// on page load
getURIinfo();
getCharacter(playerId, characterId);


// events













// functions
function getURIinfo() {
    var a = window.location.href.split("=")[1];
    var b = decodeURIComponent(a);
    var c = JSON.parse(b);

    playerId = c.PlayerId;
    characterId = c.CharacterId;

    console.log(playerId);
    console.log(characterId);

}

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
        url: getCharacterURL,
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














