// URLs
const GetCharactersByPlayer = "/api/palantir/GetCharactersByPlayer";
const GoToParty = "/api/palantir/GoToParty";

// divs
let playerName;
let playerId;
const aquila = "#aquila";
const episodes = "#episodes";
const characters = "#characters";

// on page load
establishPlayer();

// events
$(aquila).on("click", function () {

    $(episodes).slideUp(1500).after(function () {
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

                $(characters).empty();

                var data = JSON.parse(response.Data);

                if (data.length) {
                    console.log(data);

                    for (var i = 0; i < data.length; i++) {
                        if (data[i].IsAlive == true && data[i].InParty == true) {
                            drawCharacter(data[i]);
                        }
                    }
                }
            },
            error: function (err) {
                console.log(err);
            }
        }).then(function () {

            $(characters).fadeIn(4000);

        });
    });
});


// functions    
function drawCharacter(data) {
    var fighter = `
<button id="${data.CharacterId}" class="btn btn-outline-info fighterBtn">
    ${data.Name}
    <span>
        <img style="border-radius:10px" src="../media/images/humans/human${data.Logbook.PortraitNr}.png"/>
    </span>
</button>
`;

    $(characters).append(fighter);
}


function establishPlayer() {
    playerName = localStorage.getItem("playerName");
    playerId = localStorage.getItem("playerId");
}


