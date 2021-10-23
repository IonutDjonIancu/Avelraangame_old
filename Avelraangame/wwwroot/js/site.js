// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


//// js template
//// URLs
//const GetCharacter = "/api/palantir/GetCharacter";
//// divs
//const nameDiv = "#nameDiv";
//let playerId;
//let playerName;
//let characterId;
//// on page load
//// events
//// functions
//function getCharacter(playerId, characterId) {

//    var object = {
//        PlayerId: playerId,
//        CharacterId: characterId
//    }
//    var request = {
//        message: JSON.stringify(object)
//    }

//    $.ajax({
//        type: "GET",
//        url: GetCharacter,
//        contentType: "text",
//        data: request,
//        success: function (resp) {
//            var response = JSON.parse(resp);

//            if (response.Error) {
//                console.log(response.Error);
//                return;
//            }

//            var data = JSON.parse(response.Data);
//            console.log(data);
//            drawCharacter(data);
//        },
//        error: function (err) {
//            console.log(err);
//        }
//    });

//}



// functions
function drawOneCharacter_base(data, whereTo) {
    $(whereTo).empty();
    var pp = `<p>Selected character</p>`;
    $(whereTo).append(pp);

    if (data.InFight == true) {
        window.location = `/Combat/Combat_index`;
    } else {
        var html = `
            <button id="${data.CharacterId}" class="btn btn-outline-info characterBtn">
                ${data.Name}
                <span>
                    <img style="border-radius:10px" src="../media/images/humans/human${data.Logbook.PortraitNr}.png"/>
                </span>
            </button>
        `;
    }

    $(whereTo).append(html);
}

function drawCharacters_base(data, whereTo) {
    $(whereTo).empty();
    var pp = `<p>Select character</p>`;
    $(whereTo).append(pp);

    for (var i = 0; i < data.length; i++) {

        if (data[i].InFight == true) {
            var html = `
                <button title="in a fight" id="${data[i].CharacterId}" class="btn btn-outline-warning characterBtn">
                    ${data[i].Name}
                    <span>
                        <img style="border-radius:10px" src="../media/images/humans/human${data[i].Logbook.PortraitNr}.png"/>
                    </span>
                </button>
            `;
        } else {
            var html = `
                <button id="${data[i].CharacterId}" class="btn btn-outline-info characterBtn">
                    ${data[i].Name}
                    <span>
                        <img style="border-radius:10px" src="../media/images/humans/human${data[i].Logbook.PortraitNr}.png"/>
                    </span>
                </button>
            `;
        }

        $(whereTo).append(html);
    }
}

function base_drawCharacters(data, whereTo) {
    $(whereTo).empty();

    for (var i = 0; i < data.length; i++) {

        var html = `
            <button id="${data[i].CharacterId}" class="btn btn-outline-dark characterBtn">
                ${data[i].Name}
                <span>
                    <img style="border-radius:10px" src="../media/images/humans/human${data[i].Logbook.PortraitNr}.png"/>
                </span>
            </button>
        `;

        $(whereTo).append(html);
    }
}

function base_drawCharactersInParty(data, whereTo) {
    $(whereTo).empty();

    for (var i = 0; i < data.length; i++) {
        if (data[i].InParty) {
            var html = `
                <button id="${data[i].CharacterId}" class="btn btn-outline-info characterBtn">
                    ${data[i].Name}
                    <span>
                        <img style="border-radius:10px" src="../media/images/humans/human${data[i].Logbook.PortraitNr}.png"/>
                    </span>
                </button>
            `;

            $(whereTo).append(html);
        }
    }
}

function base_drawCharactersInFight(data, whereTo) {
    $(whereTo).empty();

    for (var i = 0; i < data.length; i++) {

        var html = `
            <button id="${data[i].CharacterId}" value="${data[i].FightId}" class="btn btn-outline-warning characterBtn">
                ${data[i].Name}
                <span>
                    <img style="border-radius:10px" src="../media/images/humans/human${data[i].Logbook.PortraitNr}.png"/>
                </span>
            </button>
        `;

        $(whereTo).append(html);
    }
}


function base_getAliveCharactersByPlayerId(playerId, playerName, callback) {

    var object = {
        PlayerId: playerId,
        PlayerName: playerName
    }
    var request = {
        message: JSON.stringify(object)
    }

    $.ajax({
        type: "GET",
        url: "/api/palantir/GetAliveCharactersByPlayerId",
        contentType: "text",
        data: request,
        success: function (resp) {
            var response = JSON.parse(resp);

            if (response.Error) {
                console.log(response.Error);
                return;
            }

            var data = JSON.parse(response.Data);
            callback(data);
        },
        error: function (err) {
            console.log(err);
        }
    });
}

function establishPlayerId_base() {
    var playerId = localStorage.getItem("playerId");

    return playerId;
}

function establishPlayerName_base() {
    var playerName = localStorage.getItem("playerName");

    return playerName;
}

function setSessionCredentials_base(playerId, playerName) {
    localStorage.clear();
    localStorage.setItem("playerId", playerId);
    localStorage.setItem("playerName", playerName);
}





//// texts
//let storyTexts = {
//    // Huurling
//    // Prologue
//    "Huurling_prologue": "Duty, honor or payment, they’re all the same to you.",
//    // Act 1
//    "Huurling act 1: sellsword": "The world turns, the days pass...",
//    // Epilogue
//    "Huurling_epilogue": "You go about your day.",

//    // Aquila Foedus
//    // Prologue
//    "Aquila prologue": "After the death of the old king, his children fought over the crown of Farlindor. Quintus and Maedra, having the backing of the northern duchies, took over the majority of the land army, while Lisandros and Eneer fled to the south of the large island kingdom, while also retaining control over an impressive fleet. The conflict that followed led to all out civil war between the brothers, that took place mostly in the large valleys of Tael’dorthir mountains, neither gaining an upper hand during the early years. The established states in the south, Londorth and Ennuy, controlled the trade routes between the fractured old kingdom with the rest of the continent, depleting the resources of the northern side and isolating Quintus and his armies to only what the windswept plains of Farlindor could produce. Any land assault was always repulsed by the well prepared troops of Haelva, capital city of Farlindor, leading to significant losses on the southern side, as was the case during the Battle of Ruotsgen Valley approximately 900 years before the Aeldanic calendar. The battle was made famous by the charge led by Quintus of Haelva, clad in a striking golden armour, shining in the bright afternoon sun. Although being unhorsed soon after, he made his way back to the camp, took another charger and repeated the attack. His retinue hacked their way to Lisandros which was waiting on a hill further away at the edge of the battle. When Quintus found himself close enough he pulled out his father’s crown and threw it towards the banner bearers of his brother, only to see his horse die under him once again. The act itself was a statement of how things would go for the young prince, while also causing massive disarray in between the allies nobility, since both of the older brothers wished to ascend to the throne of Haelva. Lisandros’ forces withdrew from the battle soon after, and the ensuing military confrontation ended with haelvan victory.",
//    // Act 1
//    "Aquila act 1: the eagle and the sea": "Aware that the southern kingdoms will reorganize their armies and attack into Farlindor, Quintus expected a seaborne invasion. In an attempt to prevent being outnumbered in his own lands, he ordered the transport of ships from the eastern ocean to the fiery sea of Gaerlith. He boarded his finest warriors and set sail along the western coast to the Kingdom of Ennuy. The hazardous trip through plumes of ash coming from the underwater volcanoes cost him dearly as the ships entered one by one into the darkened lands of Mount Moot. The air was noxious and barely breathable, the winds burned the skin. Some of the ships fell under the water due to the gases rising upwards, disappearing forever under the waves. Others, more fortunate, would see their masts set ablaze, watching with dread how the fire spreads slowly against their efforts. It was an impossible feat to navigate these waters, and the men of Haelva were losing hope. As customary, Quintus would travel with a golden eagle, a beast indigenous to the peaks of Farlindor. In a desperate attempt to find a path towards land, he used a small lantern tied to the eagles’ legs and set it free, ordering every ship to row towards the light. As the eagle made its way towards the sky, trying to get above the fumes, it quickly found its way towards the west and the men rowed with renewed hope. Quintus’s ship survived the trip and landed on the tall shores of Ennuy, surprising the small watchtowers built ages ago. The bay where his troops landed was later called the Ashen Swords, due to the fact that his troops covered in ash were in stark contrast with the white sands of Ennuy. Soon after debarking his men, Quintus found the eagle lying dead on the sand, its wings spread to their fullest in a last attempt to fly. This event, witnessed by many around him, was taken as an omen and soon after became the symbol of his house, the black eagle on a white background. Quintus took water and ordered the eagle to be cleaned, but not removed, it’s golden feathers shining in the afternoon sky for all troops to see as they came ashore.",
//    // Epilogue
//    "AquilaFoedus_epilogue": "The black ships of Haelva reached the lands of Seracleea.",

//};








