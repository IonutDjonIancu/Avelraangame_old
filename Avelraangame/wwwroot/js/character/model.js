// URLs
const GetCharacter = "/api/palantir/GetCharacter";
const EquipItem = "/api/palantir/EquipItem";
// divIDs
const modelJumbo = "#modelJumbo";
const nameDiv = "#nameDiv";
const statsDiv = "#statsDiv";
const expertiseDiv = "#expertiseDiv";
const assetsDiv = "#assetsDiv";
const suppliesDiv = "#suppliesDiv";
const equipmentDiv = "#equippmentDiv";
const skillsDiv = "#skillsDiv";
const suppliesNr = "#suppliesNr";
let playerId;
let playerName;
let characterId;


// on page load





// on page load
establishPlayer();
establishCharacter();
getCharacter(playerId, characterId);


// events

// functions

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
        url: GetCharacter,
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
    showInventory(data.Equippment);
    showSkills(data.Skills);
    addMouseoverEvents();
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


function showInventory(equipment) {
    $(equipmentDiv).empty();

    if (!equipment) {
        return;
    }

    if (equipment.Mainhand) {
        var mainhandHtml = `
                <img class="equipped" id="mainhand" title="Mainhand" style="border-radius:10px; opacity:0.5" src="../media/images/items/${equipment.Mainhand.Type}.png" />`;

        $(equipmentDiv).append(mainhandHtml);
    }

    if (equipment.Offhand) {
        var offhandHtml = `
                <img class="equipped" id="offhand" title="Offhand" style="border-radius:10px; opacity:0.5" src="../media/images/items/${equipment.Offhand.Type}.png" />`;

        $(equipmentDiv).append(offhandHtml);
    }

    if (equipment.Armour) {
        var armourHtml = `
                <img class="equipped" id="armour" title="Armour" style="border-radius:10px; opacity:0.5" src="../media/images/items/${equipment.Armour.Type}.png" />`;

        $(equipmentDiv).append(armourHtml);
    }

    if (equipment.Ranged) {
        var rangedHtml = `
                <img class="equipped" id="ranged" title="Ranged" style="border-radius:10px; opacity:0.5" src="../media/images/items/${equipment.Ranged.Type}.png" />`;

        $(equipmentDiv).append(rangedHtml);
    }

    if (equipment.Trinkets.length > 0) {
        var trinketHtml = `
                <img class="equipped trinkets" id="trinkets" title="${equipment.Trinkets.length} Trinkets" style="border-radius:10px; opacity:0.5" src="../media/images/items/Apparatus.png" />`;

        $(equipmentDiv).append(trinketHtml);
    }

    addEquippmentClickEvent(equipment);
}

function showSupplies(supplies) {
    $(suppliesDiv).empty();
    $(suppliesNr).empty();
    $(suppliesNr).append(`${supplies.length} items`);

    for (var i = 0; i < supplies.length; i++) {

        var html = `
            <img class="details" id="details_${supplies[i].Id}" title="${supplies[i].Name}" style="border-radius:30px; opacity:0.5" src="../media/images/items/${supplies[i].Type}.png" />
            <div class="btn-group-vertical">
                <button id="equip_${supplies[i].Id}" class="btn btn-sm btn-outline-dark equip">&#x2191;</button>
            </div>
        `;

        $(suppliesDiv).append(html);
    }

    addDetailsEvent(supplies);
    addEquipEvent(supplies);
}

function showSkills(skills) {

    var html = `
        <button class="btn btn-sm btn-outline-secondary">Apothecary ${skills.Apothecary}</button>
        <button class="btn btn-sm btn-outline-secondary">Arcane ${skills.Arcane}</button>
        <button class="btn btn-sm btn-outline-secondary">Dodge ${skills.Dodge}</button>
        <button class="btn btn-sm btn-outline-secondary">Hide ${skills.Hide}</button>
        <button class="btn btn-sm btn-outline-secondary">Melee ${skills.Melee}</button>
        <button class="btn btn-sm btn-outline-secondary">Navigation ${skills.Navigation}</button>
        <button class="btn btn-sm btn-outline-secondary">Psionics ${skills.Psionics}</button>
        <button class="btn btn-sm btn-outline-secondary">Ranged ${skills.Ranged}</button>
        <button class="btn btn-sm btn-outline-secondary">Resistance ${skills.Resistance}</button>
        <button class="btn btn-sm btn-outline-secondary">Scouting ${skills.Scouting}</button>
        <button class="btn btn-sm btn-outline-secondary">Social ${skills.Social}</button>
        <button class="btn btn-sm btn-outline-secondary">Spot ${skills.Spot}</button>
        <button class="btn btn-sm btn-outline-secondary">Survival ${skills.Survival}</button>
        <button class="btn btn-sm btn-outline-secondary">Tactics ${skills.Tactics}</button>
        <button class="btn btn-sm btn-outline-secondary">Traps ${skills.Traps}</button>
        <button class="btn btn-sm btn-outline-secondary">Unarmed ${skills.Unarmed}</button>`;

    $(skillsDiv).empty();
    $(skillsDiv).append(html);
}


function addDetailsEvent(supplies) {
    $(".details").on("click", function () {
        var itemId = this.id.split("_")[1];


        for (var i = 0; i < supplies.length; i++) {
            if (supplies[i].Id == itemId) {
                console.log(supplies[i]);
                break;
            }
        }

    });
}

function addEquipEvent(supplies) {
    $(".equip").on("click", function () {
        var itemId = this.id.split("_")[1];

        var object = {
            Id: itemId,
            CharacterId: characterId,
            PlayerId: playerId
        }
        var request = {
            Message: JSON.stringify(object)
        }


        $.ajax({
            type: "POST",
            url: EquipItem,
            contentType: 'application/json',
            data: JSON.stringify(request),
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
                window.alert(err);
                console.log(err);
            },
        });


    });
}

function addEquippmentClickEvent(equipment) {
    $(".equipped").on("click", function () {
        var itemId = this.id;

        if (itemId === "mainhand") {
            console.log(equipment.Mainhand);
        } else if (itemId === "offhand") {
            console.log(equipment.Offhand);
        } else if (itemId === "ranged") {
            console.log(equipment.Ranged);
        } else if (itemId === "armour") {
            console.log(equipment.Armour);
        }
    });

    $(".trinkets").on("click", function () {
        console.log(equipment.Trinkets);
    });
}

function addMouseoverEvents() {
    $("img").on("mouseover", function () {

        this.style = "border-radius: 10px; opacity: 1";
    });

    $("img").on("mouseout", function () {

        this.style = "border-radius: 30px; opacity: 0.5";
    });

    $(".equipped").on("mouseover", function () {
        this.style = "border-radius: 10px; opacity: 1";
    });

    $(".equipped").on("mouseout", function () {

        this.style = "border-radius: 10px; opacity: 0.5";
    });
}

function establishPlayer() {
    playerName = localStorage.getItem("playerName");
    playerId = localStorage.getItem("playerId");
}

function establishCharacter() {
    characterId = localStorage.getItem("characterId");
}
















