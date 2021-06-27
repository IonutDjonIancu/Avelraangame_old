// URLs
const getCharactersURL = "/api/palantir/Character_GetCharacters";

// divs
const players = "#players";
const levelUpCharactersPool = "#levelUpCharactersPool"
const charactersPool = "#charactersPool"
const characterBtn = ".characterBtn";
let characters;
let playerId;




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

            $(charactersPool).empty();
            $(levelUpCharactersPool).empty();

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

                $(characterBtn).on("click", function () {

                    var character = {
                        CharacterId: this.id,
                        PlayerId: playerId
                    }

                    var request2 = JSON.stringify(character)


                    if (this.hasAttribute("draggable")) {
                        window.location = `/Character/Character_levelup?request=${request2}`;
                    } else {
                        window.location = `/Character/Character_model?request=${request2}`;
                    }


                });
            }


        },
        error: function (err) {
            console.log(err);
        }
    });

}


function drawCharacter(id, race, culture, name, portraitNr, hasLevelup) {

    if (hasLevelup == true) {
        var btnStyle = `
            <button draggable="true" id="${id}" title="${culture} ${race}" class="btn btn-outline-info characterBtn">
                ${name}
                <span>
                    <img style="border-radius:10px" src="../media/images/humans/human${portraitNr}.png"/>
                </span>
            </button>
            `;
        $(levelUpCharactersPool).append(btnStyle);
    } else {
        var btnStyle = `
            <button id="${id}" title="${culture} ${race}" class="btn btn-outline-info characterBtn">
                ${name}
                <span>
                    <img style="border-radius:10px" src="../media/images/humans/human${portraitNr}.png"/>
                </span>
            </button>
            `;
        $(charactersPool).append(btnStyle);
    }
}








