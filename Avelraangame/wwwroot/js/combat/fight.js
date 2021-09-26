//// URLs
//const GenerateWeakNpcFight = "/api/palantir/GenerateWeakNpcFight";
//const GetFight = "/api/palantir/GetFight";
//const Attack = "/api/palantir/Attack";
//const Defend = "/api/palantir/Defend";
//const EndCombat = "/api/palantir/EndCombat";

//// variables
//let playerName;
//let playerId;
//let characterId;
//let npcId;
//let fightId;
//let attackerId;
//let defenderId;
//let respData;
//const weak = "#weak";
//const normal = "#normal";
//const strong = "#strong";
//const combatants = "#combatants";
//const goodGuys = "#goodGuys";
//const badGuys = "#badGuys";
//const attackBtn = "#attackBtn";
//const endTurnBtn = "#endTurnBtn";
//const endFightBtn = "#endFightBtn";
//const generateFightBtns = "#generateFightBtns";
//const attackerDefender = "#attackerDefender";
//const attackerDiv = "#attackerDiv";
//const defenderDiv = "#defenderDiv";
//const lastActionResultBtn = "#lastActionResultBtn";

//// on page load
//establishPlayer();
//establishCharacter();
//getFight();







//// events
//$(endFightBtn).on("click", function () {
//    if (fightId == undefined) {
//        return;
//    }

//    var object = {
//        FightId: fightId
//    }
//    var request = {
//        message: JSON.stringify(object)
//    }

//    $.ajax({
//        type: "GET",
//        url: EndCombat,
//        contentType: "application/text",
//        data: request,
//        success: function (resp) {
//            var response = JSON.parse(resp);

//            if (response.Error) {
//                $(lastActionResultBtn).text(response.Error);
//                console.log(response.Error);
//                return;
//            }

//            window.location = `/Combat/Combat_index`;
//        },
//        error: function (err) {
//            console.log(err);
//        }
//    })
//});

//$(weak).on("click", function () {
//    var object = {
//        PlayerId: playerId,
//        PlayerName: playerName,
//        CharacterId: characterId
//    }
//    var request = {
//        message: JSON.stringify(object)
//    }

//    $.ajax({
//        type: "GET",
//        url: GenerateWeakNpcFight,
//        contentType: "application/text",
//        data: request,
//        success: function (resp) {
//            var response = JSON.parse(resp);

//            if (response.Error) {
//                $(lastActionResultBtn).text(response.Error);
//                console.log(response.Error);
//                return;
//            }

//            location.reload();
//        },
//        error: function (err) {
//            console.log(err);
//        }
//    })
//});

//$(attackBtn).on("click", function () {

//    if (fightId == undefined) {
//        return;
//    }

//    var object = {
//        FightId: fightId,
//        Attacker: characterId,
//        Defender: defenderId
//    }
//    var request = {
//        message: JSON.stringify(object)
//    }

//    $.ajax({
//        type: "GET",
//        url: Attack,
//        contentType: "application/text",
//        data: request,
//        success: function (resp) {
//            var response = JSON.parse(resp);

//            if (response.Error) {
//                $(lastActionResultBtn).text(response.Error);
//                console.log(response.Error);
//                return;
//            } else {
//                var data = JSON.parse(response.Data);

//                console.log(data);
//                drawFight(data);
//            }
//        },
//        error: function (err) {
//            console.log(err);
//        }
//    })
//});

//$(endTurnBtn).on("click", function () {

//    if (fightId == undefined) {
//        return;
//    }

//    var object = {
//        FightId: fightId,
//        MainCharacterId: characterId,
//        PlayerId: playerId
//    }
//    var request = {
//        message: JSON.stringify(object)
//    }

//    $.ajax({
//        type: "GET",
//        url: Defend,
//        contentType: "application/text",
//        data: request,
//        success: function (resp) {
//            var response = JSON.parse(resp);

//            if (response.Error) {
//                $(lastActionResultBtn).text(response.Error);
//                console.log(response.Error);
//                return;
//            } else {
//                var data = JSON.parse(response.Data);

//                console.log(data);
//                drawFight(data);
//            }
//        },
//        error: function (err) {
//            console.log(err);
//        }
//    })
//});


//// functions 
//function hideGenerateFightButtons() {
//    $(generateFightBtns).remove();
//}

//function getFight() {
//    var object = {
//        PlayerId: playerId,
//        PlayerName: playerName,
//        CharacterId: characterId
//    }
//    var request = {
//        message: JSON.stringify(object)
//    }

//    $.ajax({
//        type: "GET",
//        url: GetFight,
//        contentType: "application/text",
//        data: request,
//        success: function (resp) {
//            var response = JSON.parse(resp);

//            if (response.Error) {
//                $(lastActionResultBtn).text("Everything seems peaceful...");
//                console.log(response.Error);
//                return;
//            } else {
//                if (response.Data) {
//                    var data = JSON.parse(response.Data);
//                    hideGenerateFightButtons();
//                    console.log(data);
//                    fightId = data.FightId;
//                    respData = data;
//                    drawFight(data);
//                    addFightingCharactersClickEvent();
//                }
//            }
//        },
//        error: function (err) {
//            console.log(err);
//        }
//    });
//}

//function addFightingCharactersClickEvent() {

//    $(".goodChr").on("click", function () {
//        attackerId = this.id;
//        showAttacker(attackerId);
//    });

//    $(".badChr").on("click", function () {
//        defenderId = this.id;
//        showDefender(defenderId);
//    });
//}

//function showAttacker(attackerId) {
//    for (var i = 0; i < respData.GoodGuys.length; i++) {
//        if (respData.GoodGuys[i].CharacterId == attackerId) {
//            var html = `
//                <img class="float-right" title="${respData.GoodGuys[i].Name}" style="border-radius:10px" src="../media/images/humans/human${respData.GoodGuys[i].Logbook.PortraitNr}.png" />
//            `;
//            $(attackerDiv).empty();
//            $(attackerDiv).append(html);
//        }
//    }
//}

//function showDefender(defenderId) {
//    for (var i = 0; i < respData.BadGuys.length; i++) {
//        if (respData.BadGuys[i].CharacterId == defenderId) {
//            var html = `
//                <img class="float-left" style="border-radius:10px" src="../media/images/npcs/npc${respData.BadGuys[i].Logbook.PortraitNr}.png" />
//            `;
//            $(defenderDiv).empty();
//            $(defenderDiv).append(html);
//        }
//    }
//}

//function drawFight(data) {
//    $(goodGuys).empty();
//    var goodHtml = `
//<button id="${data.GoodGuys[0].CharacterId}" title="${data.GoodGuys[0].Name}" class="btn btn-outline-success goodChr">
//    <img class="float-left" style="border-radius:10px" src="../media/images/humans/human${data.GoodGuys[0].Logbook.PortraitNr}.png" />
//    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-heart-fill" viewBox="0 0 16 16">
//        <path fill-rule="evenodd" d="M8 1.314C12.438-3.248 23.534 4.735 8 15-7.534 4.736 3.562-3.248 8 1.314z"/>
//    </svg> ${data.GoodGuys[0].Assets.Health}
//    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-shield-shaded" viewBox="0 0 16 16">
//        <path fill-rule="evenodd" d="M8 14.933a.615.615 0 0 0 .1-.025c.076-.023.174-.061.294-.118.24-.113.547-.29.893-.533a10.726 10.726 0 0 0 2.287-2.233c1.527-1.997 2.807-5.031 2.253-9.188a.48.48 0 0 0-.328-.39c-.651-.213-1.75-.56-2.837-.855C9.552 1.29 8.531 1.067 8 1.067v13.866zM5.072.56C6.157.265 7.31 0 8 0s1.843.265 2.928.56c1.11.3 2.229.655 2.887.87a1.54 1.54 0 0 1 1.044 1.262c.596 4.477-.787 7.795-2.465 9.99a11.775 11.775 0 0 1-2.517 2.453 7.159 7.159 0 0 1-1.048.625c-.28.132-.581.24-.829.24s-.548-.108-.829-.24a7.158 7.158 0 0 1-1.048-.625 11.777 11.777 0 0 1-2.517-2.453C1.928 10.487.545 7.169 1.141 2.692A1.54 1.54 0 0 1 2.185 1.43 62.456 62.456 0 0 1 5.072.56z" />
//    </svg> ${data.GoodGuys[0].Expertise.DRM}
//</button>
//`;

//    $(badGuys).empty();

//    var badHtml = `
//<button id="${data.BadGuys[0].CharacterId}" class="btn btn-outline-danger badChr float-right">
//    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-heart-fill" viewBox="0 0 16 16">
//        <path fill-rule="evenodd" d="M8 1.314C12.438-3.248 23.534 4.735 8 15-7.534 4.736 3.562-3.248 8 1.314z"/>
//    </svg> 
//    ${data.BadGuys[0].Assets.Health}
//    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-shield-shaded" viewBox="0 0 16 16">
//        <path fill-rule="evenodd" d="M8 14.933a.615.615 0 0 0 .1-.025c.076-.023.174-.061.294-.118.24-.113.547-.29.893-.533a10.726 10.726 0 0 0 2.287-2.233c1.527-1.997 2.807-5.031 2.253-9.188a.48.48 0 0 0-.328-.39c-.651-.213-1.75-.56-2.837-.855C9.552 1.29 8.531 1.067 8 1.067v13.866zM5.072.56C6.157.265 7.31 0 8 0s1.843.265 2.928.56c1.11.3 2.229.655 2.887.87a1.54 1.54 0 0 1 1.044 1.262c.596 4.477-.787 7.795-2.465 9.99a11.775 11.775 0 0 1-2.517 2.453 7.159 7.159 0 0 1-1.048.625c-.28.132-.581.24-.829.24s-.548-.108-.829-.24a7.158 7.158 0 0 1-1.048-.625 11.777 11.777 0 0 1-2.517-2.453C1.928 10.487.545 7.169 1.141 2.692A1.54 1.54 0 0 1 2.185 1.43 62.456 62.456 0 0 1 5.072.56z" />
//    </svg>
//    ${data.BadGuys[0].Expertise.DRM}
//    <img class="float-right" style="border-radius:10px" src="../media/images/npcs/npc${data.BadGuys[0].Logbook.PortraitNr}.png" />
//</button>
//`;

//    $(goodGuys).append(goodHtml);
//    $(badGuys).append(badHtml);
//    $(lastActionResultBtn).text(data.LastActionResult)
//}

//function establishPlayer() {
//    playerName = localStorage.getItem("playerName");
//    playerId = localStorage.getItem("playerId");
//}

//function establishCharacter() {
//    characterId = localStorage.getItem("characterId");
//}

