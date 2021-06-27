// URLs
const getCharacterURL = "/api/palantir/Character_GetCharacter";




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
        url: getCharacterURL,
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
    showStats(data.Strength, data.Toughness, data.Awareness, data.Abstract);
    showExpertise(data.Experience, data.DRM, data.Wealth);
    showAssets(data.Health, data.Mana, data.Harm);
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

function showStats(str, tou, awa, abs) {
    var html = `
<div class="list-group">
  <a href="#" class="list-group-item list-group-item-action list-group-item-success">Strength <b>${str}</b></a>
  <a href="#" class="list-group-item list-group-item-action list-group-item-success">Toughness <b>${tou}</b></a>
  <a href="#" class="list-group-item list-group-item-action list-group-item-success">Awareness <b>${awa}</b></a>
  <a href="#" class="list-group-item list-group-item-action list-group-item-success">Abstract <b>${abs}</b></a>
</div>
`;

    $(statsDiv).empty();
    $(statsDiv).append(html);
}

function showExpertise(exp, drm, wealth) {
    var html = `
<div class="list-group">
  <a href="#" class="list-group-item list-group-item-action list-group-item-success">Experience <b>${exp}</b></a>
  <a href="#" class="list-group-item list-group-item-action list-group-item-success">DRM <b>${drm}</b></a>
  <a href="#" class="list-group-item list-group-item-action list-group-item-success">Wealth <b>${wealth}</b></a>
</div>
`;

    $(expertiseDiv).empty();
    $(expertiseDiv).append(html);
}

function showAssets(health, mana, harm) {
    var html = `
<div class="list-group">
  <a href="#" class="list-group-item list-group-item-action list-group-item-success">Health <b>${health}</b></a>
  <a href="#" class="list-group-item list-group-item-action list-group-item-success">Mana <b>${mana}</b></a>
  <a href="#" class="list-group-item list-group-item-action list-group-item-success">Harm <b>${harm}</b></a>
</div>
`;

    $(assetsDiv).empty();
    $(assetsDiv).append(html);
}


















