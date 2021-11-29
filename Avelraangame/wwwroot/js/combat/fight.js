// URLs
const GetFightById = "/api/palantir/GetFightById";
const Attack = "/api/palantir/Attack";
const Pass = "/api/palantir/Pass";
const Turn = "/api/palantir/Turn";

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


$(attackBtn).on("click", function () {

    var object = {
        FightId: fightId,
        PlayerId: playerId,
        AttackerId: attackerId,
        TargetId: targetId
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
                $(lastActionResultBtn).text(response.Error);
                console.log(response.Error);
                return;
            } else {
                if (response.Data) {
                    data = JSON.parse(response.Data);
                    console.log(data);
                    drawCombatantsAndAddEvents();
                    redrawGuys(data);
                    modalSplash(data.FightDetails.LastActionResult);
                }
            }
        },
        error: function (err) {
            console.log(err);
        }
    });
});

$(passBtn).on("click", function () {

    var object = {
        FightId: fightId,
        PlayerId: playerId
    }
    var request = {
        message: JSON.stringify(object)
    }

    $.ajax({
        type: "GET",
        url: Pass,
        contentType: "application/text",
        data: request,
        success: function (resp) {
            var response = JSON.parse(resp);

            if (response.Error) {
                $(lastActionResultBtn).text(response.Error);
                console.log(response.Error);
                return;
            } else {
                if (response.Data) {
                    data = JSON.parse(response.Data);
                    console.log(data);
                    drawCombatantsAndAddEvents();
                    redrawGuys(data);
                    modalSplash(data.FightDetails.LastActionResult);
                }
            }
        },
        error: function (err) {
            console.log(err);
        }
    });
});

$(turnBtn).on("click", function () {

    var object = {
        FightId: fightId,
        PlayerId: playerId
    }
    var request = {
        message: JSON.stringify(object)
    }

    $.ajax({
        type: "GET",
        url: Turn,
        contentType: "application/text",
        data: request,
        success: function (resp) {
            var response = JSON.parse(resp);

            if (response.Error) {
                $(lastActionResultBtn).text(response.Error);
                console.log(response.Error);
                return;
            } else {
                if (response.Data) {
                    data = JSON.parse(response.Data);
                    console.log(data);
                    drawCombatantsAndAddEvents();
                    redrawGuys(data);
                }
            }
        },
        error: function (err) {
            console.log(err);
        }
    });
});

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
                    drawCombatantsAndAddEvents();
                }
            }
        },
        error: function (err) {
            console.log(err);
        }
    });
}

function drawCombatantsAndAddEvents() {
    fightId = data.FightId;
    displayLastActionResult(data);
    displayGoodGuys(data);
    displayBadGuys(data);
    $(attackerDiv).empty();
    $(targetDiv).empty();
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

function redrawGuys(data) {

    for (var i = 0; i < data.GoodGuys.length; i++) {
        if (data.GoodGuys[i].CharacterId == attackerId) {
            drawGuy(data.GoodGuys[i], true);
        }
    }

    for (var i = 0; i < data.BadGuys.length; i++) {
        if (data.BadGuys[i].CharacterId == targetId) {
            drawGuy(data.BadGuys[i], false);
        }
    }
}

function drawGuy(guy, isGood) {

    if (isGood) {
        $(attackerDiv).empty();

        var tokenBtn;
        if (guy.AttackToken) {
            tokenBtn = `<button title="action token" class="btn btn-sm btn-outline-light" style="width:120px">
                            <img style="border-radius:10px; height:20px; width:auto" src="../media/images/white_aquila.jpeg" />
                        </button>
            `;
        } else {
            tokenBtn = `<button class="btn btn-sm btn-outline-dark" style="width:120px">no token</button>`;
        }

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
                                    ${tokenBtn}
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
                    <div class="col-3">
                        <img style="border-radius:10px" src="../media/images/npcs/npc${guy.Logbook.PortraitNr}.png" />
                    </div>
                </div>
            </div>
        </div>`;

        $(targetDiv).append(html);
    }
}


function modalSplash(lastActionResult) {
    var npcMiss = 'You deflect glancing blows';
    var npcLowDmg = 'You sustain superficial wounds';
    var npcNormalDmg = 'You\'re injured';
    var npcHighDmg = 'You suffer heavy injuries';
    var charMiss = 'You missed';
    var charLowDmg = 'You cause minor damage';
    var charNormalDmg = 'You deal some direct strikes';
    var charHighDmg = 'You critically hit';

    switch (lastActionResult) {
        case npcMiss:
            break;
        case npcLowDmg:
            break;
        case npcNormalDmg:
            break;
        case npcHighDmg:
            break;
        case charMiss:
            attack0();
            break;
        case charLowDmg:
            attack1();
            break;
        case charNormalDmg:
            attack2();
            break;
        case charHighDmg:
            attack3();
            break;
        default:
            return;
    }

}

function attack0() {
    $(modalDiv).empty();

    var textHtml = `
        <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true" style="opacity:0.9">
            <div class="modal-dialog modal-dialog-centered" role="document">
                <div class="modal-content">
                    <div class="modal-body" style="text-align:center">
                        <img style="border-radius:10px; height:180px; width:auto" src="../media/images/animations/attack0.gif" />
                    </div>
                </div>
            </div>
        </div>
    `;

    var audioHtml = `
    <audio id="sword_hit_kill_audio" preload="none" controls="" style="margin-top: 5px;" hidden="hidden">
        <source src="/media/sounds/attack0.ogg" type="audio/ogg; codecs=&quot;vorbis&quot;">
    </audio>`;

    $(modalDiv).append(audioHtml);
    $(modalDiv).append(textHtml);

    $('audio').each(function () {
        this.pause(); // Stop playing
    });
    setTimeout(function () {
        $('#sword_hit_kill_audio')[0].play();

    }, 300);

    $("#myModal").modal("toggle");
    setTimeout(function () {
        $("#myModal").modal("toggle");
    }, 1200);

    setTimeout(function () {
        $(modalDiv).empty();
    }, 1700);
}

function attack1() {
    $(modalDiv).empty();

    var textHtml = `
        <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true" style="opacity:0.9">
            <div class="modal-dialog modal-dialog-centered" role="document">
                <div class="modal-content">
                    <div class="modal-body" style="text-align:center">
                        <img style="border-radius:10px; height:180px; width:auto" src="../media/images/animations/attack1.gif" />
                    </div>
                </div>
            </div>
        </div>
    `;

    var audioHtml = `
    <audio id="sword_hit_kill_audio" preload="none" controls="" style="margin-top: 5px;" hidden="hidden">
        <source src="/media/sounds/attack1.ogg" type="audio/ogg; codecs=&quot;vorbis&quot;">
    </audio>`;

    $(modalDiv).append(audioHtml);
    $(modalDiv).append(textHtml);

    $('audio').each(function () {
        this.pause(); // Stop playing
    });
    setTimeout(function () {
        $('#sword_hit_kill_audio')[0].play();

    }, 2250);

    $("#myModal").modal("toggle");
    setTimeout(function () {
        $("#myModal").modal("toggle");
    }, 4000);

    setTimeout(function () {
        $(modalDiv).empty();
    }, 4500);
}

function attack2() {
    $(modalDiv).empty();

    var textHtml = `
        <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true" style="opacity:0.9">
            <div class="modal-dialog modal-dialog-centered" role="document">
                <div class="modal-content">
                    <div class="modal-body" style="text-align:center">
                        <img style="border-radius:10px; height:180px; width:auto" src="../media/images/animations/attack2.gif" />
                    </div>
                </div>
            </div>
        </div>
    `;

    var audioHtml = `
    <audio id="sword_hit_kill_audio" preload="none" controls="" style="margin-top: 5px;" hidden="hidden">
        <source src="/media/sounds/attack2.ogg" type="audio/ogg; codecs=&quot;vorbis&quot;">
    </audio>`;

    $(modalDiv).append(audioHtml);
    $(modalDiv).append(textHtml);

    $('audio').each(function () {
        this.pause(); // Stop playing
    });
    setTimeout(function () {
        $('#sword_hit_kill_audio')[0].play();

    }, 1800);

    $("#myModal").modal("toggle");
    setTimeout(function () {
        $("#myModal").modal("toggle");
    }, 5000);

    setTimeout(function () {
        $(modalDiv).empty();
    }, 6000);
}

function attack3() {
    $(modalDiv).empty();

    var textHtml = `
        <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true" style="opacity:0.9">
            <div class="modal-dialog modal-dialog-centered" role="document">
                <div class="modal-content">
                    <div class="modal-body" style="text-align:center">
                        <img style="border-radius:10px; height:180px; width:auto" src="../media/images/animations/attack3.gif" />
                    </div>
                </div>
            </div>
        </div>
    `;

    var audioHtml = `
    <audio id="sword_hit_kill_audio" preload="none" controls="" style="margin-top: 5px;" hidden="hidden">
        <source src="/media/sounds/attack3.ogg" type="audio/ogg; codecs=&quot;vorbis&quot;">
    </audio>`;

    $(modalDiv).append(audioHtml);
    $(modalDiv).append(textHtml);

    $('audio').each(function () {
        this.pause(); // Stop playing
    });
    setTimeout(function () {
        $('#sword_hit_kill_audio')[0].play();

    }, 1200);

    $("#myModal").modal("toggle");
    setTimeout(function () {
        $("#myModal").modal("toggle");
    }, 2800);

    setTimeout(function () {
        $(modalDiv).empty();
    }, 3200);
}

