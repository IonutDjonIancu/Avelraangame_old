// URLs
const GetFight = "/api/palantir/GetFight";
const GetAliveCharactersByPlayerId = "/api/palantir/GetAliveCharactersByPlayerId";
const GetItemsByCharacter = "/api/palantir/GetItemsByCharacter";
const SellItem = "/api/palantir/SellItem";

// variables
let playerName;
let playerId;
let characterId;
const charactersListDiv = "#charactersListDiv";
const itemsListDiv = "#itemsListDiv";
const coinSound = "#coinSound";


// on page load
establishPlayer();
getCharacters();






// events




// functions
function callCoinSound() {
    $(`audio${coinSound}`)[0].play()
}

function drawItems(data) {

    $(itemsListDiv).empty();

    for (var i = 0; i < data.length; i++) {

        if (data[i].Level >= 3) {
            var html = `
                <div class="btn-group">
                    <button class="btn btn-outline-secondary" style="width:500px">${data[i].Name}</button>
                    <button title="Item level" class="btn btn-danger">${data[i].Level}</button>
                    <button title="Item worth" class="btn btn-secondary" style="width:60px">${data[i].Worth}</button>
                    <button id="${data[i].Id}" title="${data[i].Worth}" class="btn btn-outline-success sellItem">sell</button>
                </div> <br />
            `;
        } else if (data[i].Level == 2) {
            var html = `
                <div class="btn-group">
                    <button class="btn btn-outline-secondary" style="width:500px">${data[i].Name}</button>
                    <button title="Item level" class="btn btn-warning">${data[i].Level}</button>
                    <button title="Item worth" class="btn btn-secondary" style="width:60px">${data[i].Worth}</button>
                    <button id="${data[i].Id}" title="${data[i].Worth}" class="btn btn-outline-success sellItem">sell</button>
                </div> <br />
            `;
        } else {
            var html = `
                <div class="btn-group">
                    <button class="btn btn-outline-secondary" style="width:500px">${data[i].Name}</button>
                    <button title="Item level" class="btn btn-outline-secondary">${data[i].Level}</button>
                    <button title="Item worth" class="btn btn-secondary" style="width:60px">${data[i].Worth}</button>
                    <button id="${data[i].Id}" title="${data[i].Worth}" class="btn btn-outline-success sellItem">sell</button>
                </div> <br />
            `;
        }

        $(itemsListDiv).append(html);
    }

    addSellItemEvents();
}

function addSellItemEvents() {
    $(".sellItem").on("click", function () {

        var itemId = this.id;
        var worth = parseInt(this.title);

        var object = {
            Id: itemId,
            CharacterId: characterId
        }
        var request = {
            message: JSON.stringify(object)
        }

        $.ajax({
            type: "GET",
            url: SellItem,
            contentType: "application/text",
            data: request,
            success: function (resp) {
                var response = JSON.parse(resp);

                if (response.Error) {
                    console.log(response.Error);
                    return;
                } else {

                    $(`#${itemId}`).parent().remove();
                    var imgWealth = parseInt($(`#${characterId}span`).text());
                    var result = imgWealth + worth;

                    $(`#${characterId}span`).text(result);
                    callCoinSound();
                }
            },
            error: function (err) {
                console.log(err);
            }
        });
    });
}

function getItems() {
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
        url: GetItemsByCharacter,
        contentType: "application/text",
        data: request,
        success: function (resp) {
            var response = JSON.parse(resp);

            if (response.Error) {
                console.log(response.Error);
                return;
            } else {
                var data = JSON.parse(response.Data);

                console.log(data);
                drawItems(data);
            }
        },
        error: function (err) {
            console.log(err);
        }
    });
}

function getCharacters() {
    var object = {
        PlayerId: playerId,
        PlayerName: playerName,
    }
    var request = {
        message: JSON.stringify(object)
    }

    $.ajax({
        type: "GET",
        url: GetAliveCharactersByPlayerId,
        contentType: "application/text",
        data: request,
        success: function (resp) {
            var response = JSON.parse(resp);

            if (response.Error) {
                console.log(response.Error);
                return;
            } else {
                var data = JSON.parse(response.Data);

                console.log(data);
                drawDropdown(data);
            }
        },
        error: function (err) {
            console.log(err);
        }
    });
}

function drawDropdown(data) {

    $(charactersListDiv).empty();

    for (var i = 0; i < data.length; i++) {

        if (data[i].InFight == false) {
            var html = `
                <img class="characterImg" title="${data[i].Name}" id="${data[i].CharacterId}" style="border-radius:30px; opacity:0.5" src="../media/images/humans/human${data[i].Logbook.PortraitNr}.png" />
                <span id="${data[i].CharacterId}span">${data[i].Logbook.Wealth}</span>
            
            `;

            $(charactersListDiv).append(html);
        }
    }

    addMouseoverEvents();
    addOnClickEvents();
}

function addOnClickEvents() {
    $(".characterImg").on("click", function () {

        characterId = this.id;

        getItems(characterId);
    });
}

function addMouseoverEvents() {
    $(".characterImg").on("mouseover", function () {
        this.style = "border-radius: 10px; opacity: 1";
    });

    $(".characterImg").on("mouseout", function () {
        this.style = "border-radius: 30px; opacity: 0.5";
    });
}

function establishPlayer() {
    playerName = localStorage.getItem("playerName");
    playerId = localStorage.getItem("playerId");
}