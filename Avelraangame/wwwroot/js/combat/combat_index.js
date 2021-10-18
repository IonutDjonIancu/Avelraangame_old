// URLs
const JoinParty = "/api/palantir/JoinParty";

// divs
const aloneDiv = "#aloneDiv";
const inPartyDiv = "#inPartyDiv";
const inFightDiv = "#inFightDiv";
let playerId;
let playerName;
let characterId;

// objects

// on page load
localStorage.removeItem("characterId");
playerId = establishPlayerId_base();
playerName = establishPlayerName_base();



base_getAliveCharactersByPlayerId(playerId, playerName, function (res) {

    console.log(res);

    let aloneArr = [];
    let inParty = [];
    let inFight = [];

    for (var i = 0; i < res.length; i++) {
        if (res[i].InFight) {
            inFight.push(res[i]);
        } else if (res[i].InParty) {
            inParty.push(res[i]);
        } else {
            aloneArr.push(res[i]);
        }
    }

    base_drawCharacters(aloneArr, aloneDiv);
    base_drawCharactersInParty(inParty, inPartyDiv);
    base_drawCharactersInFight(inFight, inFightDiv);

    addCharacterClickEvent();
});


// events




// functions
function addCharacterClickEvent() {
    $(".characterBtn").on("click", function () {

        var object = {
            PlayerId: playerId,
            PlayerName: playerName,
            CharacterId: this.id
        }
        var request = {
            message: JSON.stringify(object)
        }

    $.ajax({
        type: "POST",
        url: JoinParty,
        contentType: "application/json",
        data: JSON.stringify(request),
        success: function (resp) {
            var response = JSON.parse(resp);

            if (response.Error) {
                console.log(response.Error);
                return;
            } else {
                window.location.reload();
            }
        },
        error: function (err) {
            console.log(err);
        }
    });




    });
}

