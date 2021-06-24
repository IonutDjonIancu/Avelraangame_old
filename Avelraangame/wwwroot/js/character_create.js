// URLs
const getCharactersURL = "/api/palantir/Character_GetCharactersDraft";

// divIDs
const players = "#players";
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


//$(characterBtn).on("click", function () {

//    console.log(this.id);

//});


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
            var data = JSON.parse(response.Data);

            if (data.length) {
                console.log(data);
                playerId = data[0].PlayerId;

                for (var i = 0; i < data.length; i++) {
                    drawCharacter(data[i].CharacterId,
                        data[i].Race,
                        data[i].Culture,
                        data[i].Name,
                        data[i].Logbook.PortraitNr);
                }

                $(characterBtn).on("click", function () {

                    console.log({
                        charId: this.id,
                        player: playerId
                    });

                });
            }


        },
        error: function (err) {
            console.log(err);
        }
    });

}


function drawCharacter(id, race, culture, name, portraitNr) {
    var btnStyle = `
<button id="${id}" title="${culture, race}" class="btn btn-outline-info characterBtn">${name}</button>
`;

    var imgStyle = `
<img id="${id}" style="border-radius:20px" title="${name}, ${culture} ${race}" src="../media/images/humans/human${portraitNr}.png"/>
`;

    $(charactersPool).append(imgStyle);

}








