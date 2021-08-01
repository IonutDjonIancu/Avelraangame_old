// URLs
const GetCharactersByPlayer = "/api/palantir/GetCharactersByPlayer";

// divs
const readyToFight = "#readyToFight";
let playerName;
let playerId;

// on page load
establishPlayer();
getAllCharactersToFight();

// events

// functions    
function getAllCharactersToFight() {
    var object = {
        PlayerId: playerId,
        PlayerName: playerName
    }
    var request = {
        message: JSON.stringify(object)
    }

    $.ajax({
        type: "GET",
        url: GetCharactersByPlayer,
        contentType: "application/text",
        data: request,
        success: function (resp) {
            var response = JSON.parse(resp);

            if (response.Error) {
                console.log(response.Error);
                return;
            }

            $(readyToFight).empty();

            var data = JSON.parse(response.Data);

            if (data.length) {
                console.log(data);
                playerId = data[0].PlayerId;

                for (var i = 0; i < data.length; i++) {
                    drawCharacter(data[i].CharacterId,
                        data[i].Race,
                        data[i].Culture,
                        data[i].Name,
                        data[i].Logbook.PortraitNr,
                        data[i].HasLevelup);
                }
            }


        },
        error: function (err) {
            console.log(err);
        }
    });
}



function drawCharacter(id, race, culture, name, portraitNr, hasLevelup) {
    var btnStyle = `
            <button id="${id}" title="${culture} ${race}" class="btn btn-outline-info characterBtn">
                ${name}
                <span>
                    <img style="border-radius:10px" src="../media/images/humans/human${portraitNr}.png"/>
                </span>
            </button>
            `;
    $(readyToFight).append(btnStyle);
}

function establishPlayer() {
    playerName = localStorage.getItem("playerName");
    playerId = localStorage.getItem("playerId");
}


