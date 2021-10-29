// URLs
const GetFightById = "/api/palantir/GetFightById";

// variables
const lastActionResultBtn = "#lastActionResultBtn";
const goodGuysDiv = "#goodGuysDiv";
const badGuysDiv = "#badGuysDiv";
const attackerDiv = "#attackerDiv";
const targetDiv = "#targetDiv";
const attackBtn = "#attackBtn";
const passBtn = "#passBtn";
const turnBtn = "#turnBtn";
let playerName;
let playerId;
let characterId;
let fightId;
let data;
let attackerId;
let targetId;


// on page load
playerId = establishPlayerId_base();
playerName = establishPlayerName_base();
characterId = establishCharacterId_base();
getFight();







// events


// functions 
function getFight() {
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
        url: GetFightById,
        contentType: "application/text",
        data: request,
        success: function (resp) {
            var response = JSON.parse(resp);

            if (response.Error) {
                $(lastActionResultBtn).text("Everything seems peaceful...");
                console.log(response.Error);
                return;
            } else {
                if (response.Data) {
                    data = JSON.parse(response.Data);
                    console.log(data);

                    fightId = data.FightId;
                    displayLastActionResult(data);
                    displayGoodGuys(data);
                    displayBadGuys(data);



                }
            }
        },
        error: function (err) {
            console.log(err);
        }
    });
}

function displayLastActionResult(data) {
    $(lastActionResultBtn).text(data.FightDetails.LastActionResult);
}

function displayGoodGuys(data) {
    base_drawGoodGuys(data.GoodGuys, goodGuysDiv);
    setGoodGuysClickEvent();
}

function displayBadGuys(data) {
    base_drawBadGuys(data.BadGuys, badGuysDiv);
    setBadGuysClickEvent();
}


function setGoodGuysClickEvent() {
    $(".goodGuy").on("click", function () {
        for (var i = 0; i < $(".goodGuy").length; i++) {
            $($(".goodGuy")[i]).removeClass("btn-light");
            $($(".goodGuy")[i]).addClass("btn-outline-dark");
        }

        $(this).removeClass("btn-outline-dark");
        $(this).addClass("btn-light");
        attackerId = this.id;

        for (var i = 0; i < data.GoodGuys.length; i++) {
            if (data.GoodGuys[i].CharacterId == this.id) {
                drawGuy(data.GoodGuys[i], true);
            }
        }


    });
}

function setBadGuysClickEvent() {
    $(".badGuy").on("click", function () {
        for (var i = 0; i < $(".badGuy").length; i++) {
            $($(".badGuy")[i]).removeClass("btn-light");
            $($(".badGuy")[i]).addClass("btn-outline-dark");
        }

        $(this).removeClass("btn-outline-dark");
        $(this).addClass("btn-light");
        targetId = this.id;

        for (var i = 0; i < data.BadGuys.length; i++) {
            if (data.BadGuys[i].CharacterId == this.id) {
                drawGuy(data.BadGuys[i], false);
            }
        }

    });
}


function drawGuy(guy, isGood) {

    if (isGood) {
        $(attackerDiv).empty();

        var html = `
        <div style="border:solid; border-color:darkslategray; border-width:2px; border-radius:5px">
            <div class="container" style="margin:10px">
                <div class="row">
                    <div class="col-3">
                        <img style="border-radius:10px" src="../media/images/humans/human${guy.Logbook.PortraitNr}.png" />
                    </div>
                    <div class="col-9">
                        <div class="row">
                            <div class="col-6">
                                <div class="btn-group-vertical">
                                    <button class="btn btn-sm btn-dark" style="width:120px">HP ${guy.Assets.Health}</button>
                                    <button class="btn btn-sm btn-dark" style="width:120px">Harm ${guy.Assets.Harm}</button>
                                    <button class="btn btn-sm btn-dark" style="width:120px">DRM ${guy.Expertise.DRM}</button>
                                    <button class="btn btn-sm btn-dark" style="width:120px">Mana ${guy.Assets.Mana}</button>
                                    <button class="btn btn-sm btn-dark" style="width:120px">${guy.AttackToken == true ? "has token" : "no token"}</button>
                                </div>
                            </div>
                            <div class="col-6">
                                <div class="btn-group-vertical">
                                    <button class="btn btn-sm btn-dark" style="width:120px">Melee ${guy.Skills.Melee}</button>
                                    <button class="btn btn-sm btn-dark" style="width:120px">Ranged ${guy.Skills.Ranged}</button>
                                    <button class="btn btn-sm btn-dark" style="width:120px">Hide ${guy.Skills.Hide}</button>
                                    <button class="btn btn-sm btn-dark" style="width:120px">Spot ${guy.Skills.Spot}</button>
                                    <button class="btn btn-sm btn-dark" style="width:120px">Resistance ${guy.Skills.Resistance}</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>`;

        $(attackerDiv).append(html);

    } else {
        $(targetDiv).empty();

        var html = `
        <div style="border:solid; border-color:darkslategray; border-width:2px; border-radius:5px">
            <div class="container" style="margin:10px">
                <div class="row">
                    <div class="col-9">
                        <div class="row">
                            <div class="col-6">
                                <div class="btn-group-vertical">
                                    <button class="btn btn-sm btn-dark" style="width:120px">Melee ${guy.Skills.Melee}</button>
                                    <button class="btn btn-sm btn-dark" style="width:120px">Ranged ${guy.Skills.Ranged}</button>
                                    <button class="btn btn-sm btn-dark" style="width:120px">Hide ${guy.Skills.Hide}</button>
                                    <button class="btn btn-sm btn-dark" style="width:120px">Spot ${guy.Skills.Spot}</button>
                                    <button class="btn btn-sm btn-dark" style="width:120px">Resistance ${guy.Skills.Resistance}</button>
                                </div>
                            </div>
                            <div class="col-6">
                                <div class="btn-group-vertical">
                                    <button class="btn btn-sm btn-dark" style="width:120px">HP ${guy.Assets.Health}</button>
                                    <button class="btn btn-sm btn-dark" style="width:120px">Harm ${guy.Assets.Harm}</button>
                                    <button class="btn btn-sm btn-dark" style="width:120px">DRM ${guy.Expertise.DRM}</button>
                                    <button class="btn btn-sm btn-dark" style="width:120px">Mana ${guy.Assets.Mana}</button>
                                    <button class="btn btn-sm btn-dark" style="width:120px">${guy.AttackToken == true ? "has token" : "no token"}</button>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-3">
                        <img style="border-radius:10px" src="../media/images/npcs/npc${guy.Logbook.PortraitNr}.png" />
                    </div>
                </div>
            </div>
        </div>`;

        $(targetDiv).append(html);
    }
}
