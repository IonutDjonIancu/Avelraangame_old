// URLs
const GenerateWeakNpcFight = "/api/palantir/GenerateWeakNpcFight";
const GetFightByCharacter = "/api/palantir/GetFightByCharacter";

// variables
let playerName;
let playerId;
let characterId;
let fightId;
const weak = "#weak";
const normal = "#normal";
const strong = "#strong";
const combatants = "#combatants";
const fighterDiv = "#fighterDiv";
const npcDiv = "#npcDiv";
const attackBtn = "#attackBtn";
const endTurnBtn = "#endTurnBtn";
const getFight = "#getFight";

// on page load
establishPlayer();
establishCharacter();







// events
$(weak).on("click", function () {
    var object = {
        PlayerId: playerId,
        PlayerName: playerName,
        CharacterId: characterId
    }
    var request = {
        message: JSON.stringify(object)
    }

    $.ajax({
        type: "GET",
        url: GenerateWeakNpcFight,
        contentType: "application/text",
        data: request,
        success: function (resp) {
            var response = JSON.parse(resp);

            if (response.Error) {
                alert(response.Error);
                return;
            }

            alert(response.Data);
        },
        error: function (err) {
            console.log(err);
        }
    })
});

$(getFight).on("click", function () {
    var object = {
        PlayerId: playerId,
        PlayerName: playerName,
        CharacterId: characterId
    }
    var request = {
        message: JSON.stringify(object)
    }

    $.ajax({
        type: "GET",
        url: GetFightByCharacter,
        contentType: "application/text",
        data: request,
        success: function (resp) {
            var response = JSON.parse(resp);

            if (response.Error) {
                alert(response.Error);
                return;
            }

            var data = JSON.parse(response.Data);

            console.log(data);
        },
        error: function (err) {
            console.log(err);
        }
    })
});



// functions 
function drawFight(npcData) {
    $(combatants).empty();

    var combatHtml = `
<div class="container">
    <div class="row">
        <div id="fighterDiv" class="col text-center" style="border-color:burlywood; border-style:solid; border-radius:7px">
            <button class="btn btn-primary">aaa</button>
        </div>
        <div class="col col-2 text-center">
            <button id="attackBtn" class="btn btn-outline-light">Attack</button> <br /> <br />
            <button id="endTurnBtn" class="btn btn-outline-light">End turn</button>
        </div>
        <div id="npcDiv" class="col text-center" style="border-color:coral; border-style:solid; border-radius:7px">
            <h5>
                <img style="border-radius:10px" src="../media/images/npcs/npc${npcData.Logbook.PortraitNr}.png"/>
                ${npcData.Name}
            </h5>
        </div>
    </div>
</div>
`;

    $(combatants).append(combatHtml);


    $(combatants).toggle(500);
}

function getCharacter() {
    console.log("get character");
}


function establishPlayer() {
    playerName = localStorage.getItem("playerName");
    playerId = localStorage.getItem("playerId");
}

function establishCharacter() {
    characterId = localStorage.getItem("characterId");
}

