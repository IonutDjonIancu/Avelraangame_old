// URLs
const Character_GetCharacter = "/api/palantir/Character_GetCharacter";




// divIDs
const modelJumbo = "#modelJumbo";
const nameDiv = "#nameDiv";
const statsDiv = "#statsDiv";
const expertiseDiv = "#expertiseDiv";
const assetsDiv = "#assetsDiv";
const suppliesDiv = "#suppliesDiv";
let playerId;
let characterId;


// on page load





// on page load
getURIinfo();
getCharacter(playerId, characterId);


// events













// functions
function getURIinfo() {
    var a = window.location.href.split("=")[1];
    var b = decodeURIComponent(a);
    var c = JSON.parse(b);

    playerId = c.PlayerId;
    characterId = c.CharacterId;
}

function getCharacter(playerId, characterId) {

    var object = {
        PlayerId: playerId,
        CharacterId: characterId
    }
    var request = {
        message: JSON.stringify(object)
    }

    $.ajax({
        type: "GET",
        url: Character_GetCharacter,
        contentType: "text",
        data: request,
        success: function (resp) {
            var response = JSON.parse(resp);

            if (response.Error) {
                console.log(response.Error);
                return;
            }

            var data = JSON.parse(response.Data);
            console.log(data);
            drawCharacter(data);
        },
        error: function (err) {
            console.log(err);
        }
    });

}

function drawCharacter(data) {
    showName(data.Name, data.Logbook.PortraitNr);
    showStats(data.Stats);
    showExpertise(data.Expertise, data.Logbook.Wealth);
    showAssets(data.Assets);
    showSupplies(data.Supplies);
}

function showName(name, portraitNr) {
    var html = `
<h5>
    <img style="border-radius:10px" src="../media/images/humans/human${portraitNr}.png"/>
    ${name}
</h5>
`;
    $(nameDiv).empty();
    $(nameDiv).append(html);
}

function showStats(stats) {
    var html = `
<div class="btn-group">
    <button class="btn btn-sm btn-success">Strength <b>${stats.Strength}</b></button>
    <button class="btn btn-sm btn-success">Toughness <b>${stats.Toughness}</b></button>
    <button class="btn btn-sm btn-success">Awareness <b>${stats.Awareness}</b></button>
    <button class="btn btn-sm btn-success">Abstract <b>${stats.Abstract}</b></button>
</div>
`;

    $(statsDiv).empty();
    $(statsDiv).append(html);
}

function showExpertise(expertise, wealth) {
    var html = `
<div class="btn-group">
    <button class="btn btn-sm btn-info">Experience <b>${expertise.Experience}</b></button>
    <button class="btn btn-sm btn-info">DRM <b>${expertise.DRM}</b></button>
    <button class="btn btn-sm btn-info">Wealth <b>${wealth}</b></button>
</div>
`;

    $(expertiseDiv).empty();
    $(expertiseDiv).append(html);
}

function showAssets(assets) {
    var html = `
<div class="btn-group">
    <button class="btn btn-sm btn-warning">Health <b>${assets.Health}</b></button>
    <button class="btn btn-sm btn-warning">Mana <b>${assets.Mana}</b></button>
    <button class="btn btn-sm btn-warning">Harm <b>${assets.Harm}</b></button>
</div>
`;

    $(assetsDiv).empty();
    $(assetsDiv).append(html);
}

function showSupplies(supplies) {
    $(suppliesDiv).empty();

    for (var i = 0; i < supplies.length; i++) {

        var nr = getRandomNr(50);

        var html = `
            <img id="item_${supplies[i].Id}" title="${supplies[i].Name}" style="border-radius:10px" src="../media/images/items/item${nr}.png" />
            <div class="btn-group-vertical">
                <button id="equipp" class="btn btn-sm btn-outline-dark">Equipp</button>
                <button id="details_${supplies[i].Id}" class="btn btn-sm btn-outline-dark details">Details</button>
            </div>
        `;

        $(suppliesDiv).append(html);
    }

    addDetailsEvent(supplies);
}

function getRandomNr(max) {
    return Math.floor(Math.random() * max + 1);
}

function addDetailsEvent(supplies) {
    $(".details").on("click", function () {
        var itemId = this.id.split("_")[1];


        for (var i = 0; i < supplies.length; i++) {
            if (supplies[i].Id == itemId) {

                var text = `${supplies[i].Name}\n
Type: ${supplies[i].Type}\n
Worth: ${supplies[i].Worth}`;

                window.alert(text);

                break;
            }
        }

    });
}















