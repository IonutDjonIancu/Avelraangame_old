// URLs
const GetCharactersByPlayer = "/api/palantir/GetCharactersByPlayer";
const GoToParty = "/api/palantir/GoToParty";

// divs
const readyToFightDiv = "#readyToFight";
const inPartyDiv = "#inParty";
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

            $(readyToFightDiv).empty();

            var data = JSON.parse(response.Data);

            if (data.length) {
                console.log(data);
                playerId = data[0].PlayerId;

                for (var i = 0; i < data.length; i++) {
                    drawCharacter(data[i].CharacterId,
                        data[i].Logbook.Race,
                        data[i].Logbook.Culture,
                        data[i].Name,
                        data[i].Logbook.PortraitNr,
                        data[i].InParty);
                }

                addGoToPartyEvent();
            }


        },
        error: function (err) {
            console.log(err);
        }
    });
}



function drawCharacter(id, race, culture, name, portraitNr, inParty) {
    var btnStyle = `
            <button id="${id}" title="${culture} ${race}" class="btn btn-outline-info characterBtn">
                ${name}
                <span>
                    <img style="border-radius:10px" src="../media/images/humans/human${portraitNr}.png"/>
                </span>
            </button>
            `;

    if (inParty == false) {
        $(readyToFightDiv).append(btnStyle);
    } else {
        $(inPartyDiv).append(btnStyle);
    }


}

function addGoToPartyEvent() {
    $(".characterBtn").on("click", function () {
        var id = this.id;

        var charVm = {
            PlayerId: playerId,
            CharacterId: id,
        };

        var request = {
            message: JSON.stringify(charVm)
        };

        $.ajax({
            url: GoToParty,
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify(request),
            success: function (resp) {

                location.reload();
            },
            error: function (err) {
                console.log(err);
            }
        });


    });
}


function establishPlayer() {
    playerName = localStorage.getItem("playerName");
    playerId = localStorage.getItem("playerId");
}


