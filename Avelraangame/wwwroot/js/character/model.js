// URLs
const Character_GetCharacter = "/api/palantir/Character_GetCharacter";




// divIDs
const modelJumbo = "#modelJumbo";
const nameDiv = "#nameDiv";
const statsDiv = "#statsDiv";
const expertiseDiv = "#expertiseDiv";
const assetsDiv = "#assetsDiv";
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
<div class="list-group">
  <a href="#" class="list-group-item list-group-item-action list-group-item-success">Strength <b>${stats.Strength}</b></a>
  <a href="#" class="list-group-item list-group-item-action list-group-item-success">Toughness <b>${stats.Toughness}</b></a>
  <a href="#" class="list-group-item list-group-item-action list-group-item-success">Awareness <b>${stats.Awareness}</b></a>
  <a href="#" class="list-group-item list-group-item-action list-group-item-success">Abstract <b>${stats.Abstract}</b></a>
</div>
`;

    $(statsDiv).empty();
    $(statsDiv).append(html);
}

function showExpertise(expertise, wealth) {
    var html = `
<div class="list-group">
  <a href="#" class="list-group-item list-group-item-action list-group-item-info">Experience <b>${expertise.Experience}</b></a>
  <a href="#" class="list-group-item list-group-item-action list-group-item-info">DRM <b>${expertise.DRM}</b></a>
  <a href="#" class="list-group-item list-group-item-action list-group-item-info">Wealth <b>${wealth}</b></a>
</div>
`;

    $(expertiseDiv).empty();
    $(expertiseDiv).append(html);
}

function showAssets(assets) {
    var html = `
<div class="list-group">
  <a href="#" class="list-group-item list-group-item-action list-group-item-warning">Health <b>${assets.Health}</b></a>
  <a href="#" class="list-group-item list-group-item-action list-group-item-warning">Mana <b>${assets.Mana}</b></a>
  <a href="#" class="list-group-item list-group-item-action list-group-item-warning">Harm <b>${assets.Harm}</b></a>
</div>
`;

    $(assetsDiv).empty();
    $(assetsDiv).append(html);
}


















