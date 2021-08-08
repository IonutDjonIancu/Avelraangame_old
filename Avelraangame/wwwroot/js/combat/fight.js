// URLs
const GenerateWeakNpcFight = "/api/palantir/GenerateWeakNpcFight";
const GetFightByCharacter = "/api/palantir/GetFightByCharacter";
const Attack = "/api/palantir/Attack";

// variables
let playerName;
let playerId;
let characterId;
let npcId;
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
            localStorage.setItem("fightId", data.FightId);

            fightId = data.FightId;
            npcId = data.BadGuys[0].CharacterId;

            console.log(data);
            drawFight(data);
        },
        error: function (err) {
            console.log(err);
        }
    })
});

$("#attackBtn").on("click", function () {

    var object = {
        FightId: fightId,
        Attacker: characterId,
        Defender: npcId
    }
    var request = {
        message: JSON.stringify(object)
    }

    $.ajax({
        type: "GET",
        url: Attack,
        contentType: "application/text",
        data: request,
        success: function (resp) {
            var response = JSON.parse(resp);

            if (response.Error) {
                console.log(response.Error);
                return;
            }

            var data = JSON.parse(response.Data);

            console.log(data);
            updateFight(data);
        },
        error: function (err) {
            console.log(err);
        }
    })
});


// functions 
function drawFight(data) {
    $(fighterDiv).empty();
    var fighterHtml = `
<br />
<img class="float-left" style="border-radius:10px" src="../media/images/humans/human${data.GoodGuys[0].Logbook.PortraitNr}.png" />
<br />
<br />
<div class="btn-group-vertical">
    <div class="btn-group">
        <button class="btn btn-outline-primary" style="width:70px">HP</button>
        <button class="btn btn-primary" style="width:50px">${data.GoodGuys[0].Assets.Health}</button>
    </div>
    <div class="btn-group">
        <button class="btn btn-outline-primary" style="width:70px">DRM</button>
        <button class="btn btn-primary" style="width:50px">${data.GoodGuys[0].Expertise.DRM}</button>
    </div>
</div>
<hr />
`;

    $(npcDiv).empty();
    var npcHtml = `
<br />
<img class="float-right" style="border-radius:10px" src="../media/images/npcs/npc${data.BadGuys[0].Logbook.PortraitNr}.png" />
<br />
<br />
    <div class="btn-group-vertical">
    <div class="btn-group">
        <button class="btn btn-primary" style="width:50px">${data.BadGuys[0].Assets.Health}</button>
        <button class="btn btn-outline-primary" style="width:70px">HP</button>
    </div>
    <div class="btn-group">
        <button class="btn btn-primary" style="width:50px">${data.BadGuys[0].Expertise.DRM}</button>
        <button class="btn btn-outline-primary" style="width:70px">DRM</button>
    </div>
</div>
<hr />
`;

    $(fighterDiv).append(fighterHtml);
    $(npcDiv).append(npcHtml);
    $(combatants).toggle(500);
}

function updateFight(data) {
    $(fighterDiv).empty();
    var fighterHtml = `
<br />
<img class="float-left" style="border-radius:10px" src="../media/images/humans/human${data.GoodGuys[0].Logbook.PortraitNr}.png" />
<br />
<br />
<div class="btn-group-vertical">
    <div class="btn-group">
        <button class="btn btn-outline-primary" style="width:70px">HP</button>
        <button class="btn btn-primary" style="width:50px">${data.GoodGuys[0].Assets.Health}</button>
    </div>
    <div class="btn-group">
        <button class="btn btn-outline-primary" style="width:70px">DRM</button>
        <button class="btn btn-primary" style="width:50px">${data.GoodGuys[0].Expertise.DRM}</button>
    </div>
</div>
<hr />
`;

    $(npcDiv).empty();
    var npcHtml = `
<br />
<img class="float-right" style="border-radius:10px" src="../media/images/npcs/npc${data.BadGuys[0].Logbook.PortraitNr}.png" />
<br />
<br />
    <div class="btn-group-vertical">
    <div class="btn-group">
        <button class="btn btn-primary" style="width:50px">${data.BadGuys[0].Assets.Health}</button>
        <button class="btn btn-outline-primary" style="width:70px">HP</button>
    </div>
    <div class="btn-group">
        <button class="btn btn-primary" style="width:50px">${data.BadGuys[0].Expertise.DRM}</button>
        <button class="btn btn-outline-primary" style="width:70px">DRM</button>
    </div>
</div>
<hr />
`;

    $(fighterDiv).append(fighterHtml);
    $(npcDiv).append(npcHtml);
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

