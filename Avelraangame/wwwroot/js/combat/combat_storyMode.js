// URLs
const GetCharacter = "/api/palantir/GetCharacter";
const GetEpisodes = "/api/palantir/GetEpisodes";
// divs
const nameDiv = "#nameDiv";
const charactersSelectDiv = "#charactersSelectDiv";
const episodeSelectDiv = "#episodeSelectDiv";
const episodeBtnDiv = "#episodeBtnDiv";
let playerId;
let playerName;
let characterId;
let characters;

// on page load
playerId = establishPlayerId_base();
playerName = establishPlayerName_base();
getEpisodes();


if (!characterId) {
    getCharactersByPlayerNameAndId_base(playerName, playerId, function (res) {
        characters = res;

        drawCharacters_base(res, charactersSelectDiv);
        addCharacterClickEvent();


    });
}




// events
// functions
function addCharacterClickEvent() {
    $(".characterBtn").on("click", function () {

        if (this.title == "in a fight") {
            localStorage.setItem("characterId", this.id);
            window.location = `/Combat/Fight_storyMode`;
        } else {
            localStorage.setItem("characterId", this.id);
            characterId = this.id;

            $(charactersSelectDiv).toggle(600);
            $(episodeSelectDiv).delay(600).toggle(600);
        }
    });
}

function getEpisodes() {
    $.ajax({
        type: "GET",
        url: GetEpisodes,
        contentType: "text",
        success: function (resp) {
            var response = JSON.parse(resp);

            if (response.Error) {
                console.log(response.Error);
                return;
            }

            var data = JSON.parse(response.Data);
            drawEpisodes(data);


        },
        error: function (err) {
            console.log(err);
        }
    });
}

function drawEpisodes(data) {

    $(episodeBtnDiv).empty();

    for (var i = 0; i < data.length; i++) {
        var html = `
            <button id="${data[i].Id}" class="btn btn-block btn-outline-secondary episode">${data[i].Name}</button>
        `;

        $(episodeBtnDiv).append(html);
    }

    addEpisodeClickEvent();
}

function addEpisodeClickEvent() {
    $(".episode").on("click", function () {
        localStorage.setItem("episodeId", this.id);

        window.location = `/Combat/Fight_storyMode`;
    });
}





