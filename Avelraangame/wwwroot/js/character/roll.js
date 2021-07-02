﻿// URLs
const Character_Roll20URL = "/api/palantir/Character_Roll20";
const Character_StoreRollURL = "/api/palantir/Character_StoreRoll";
const Character_AddCharacterURL = "/api/palantir/Character_AddCharacter";


// divIDs
const playerPlaceholder = "#playerPlaceholder";
const rollStatsBtn = "#rollStatsBtn";
const storeStatsBtn = "#storeStatsBtn";
const saveBtn = "#saveBtn";
const statsPlaceholder = "#statsPlaceholder";
const storeStatsPlaceholder = "#storeStatsPlaceholder";
const players = "#players";
const charName = "#charName";
let playerAccount;
let playerThatCallsId;


// objects
let contentTypeObj = {
    json: "application/json",
    text: "text"
}



// on page load








// events


$(saveBtn).on("click", function () {

    var name = $(charName).val();

    if (!playerThatCallsId) {
        return;
    }

    if (name.length < 1) {
        window.alert("Name?");
        return;
    }

    var object = {
        PlayerId: playerThatCallsId,
        PlayerName: playerAccount,
        Name: name
    }
    var request = {
        message: JSON.stringify(object)
    }


    $.ajax({
        type: "GET",
        url: Character_AddCharacterURL,
        contentType: contentTypeObj.text,
        data: request,
        success: function (resp) {
            var response = JSON.parse(resp);

            if (response.Error) {
                console.log(response.Error);
                return;
            }

            window.location = "/Character/Character_select";
        },
        error: function (err) {
            console.log(err);
        }
    });

});

$(players).on("change", function () {

    var account = $(players)[0].value;

    $(playerPlaceholder).text(account);
    playerAccount = account;
    playerThatCallsId = "00000000-0000-0000-0000-000000000000";
});

$(rollStatsBtn).on("click", function () {

    charRoll20();

});

$(storeStatsBtn).on("click", function () {

    if (!playerThatCallsId) {
        return;
    }

    var object = {
        PlayerId: playerThatCallsId,
        PlayerName: playerAccount
    }
    var request = {
        message: JSON.stringify(object)
    }

    $.ajax({
        type: "GET",
        url: Character_StoreRollURL,
        contentType: contentTypeObj.text,
        data: request,
        success: function (resp) {
            var response = JSON.parse(resp);

            if (response.Error) {
                console.log(response.Error);
                return;
            }

            var data = JSON.parse(response.Data);

            $(storeStatsPlaceholder).text(data.Logbook.StatsRoll);

        },
        error: function (err) {
            console.log(err);
        }
    });
});




// functions

function charRoll20() {

    if (!playerAccount) {
        window.alert("Set the account first.");
        return;
    }

    var object = {
        PlayerId: playerThatCallsId,
        PlayerName: playerAccount
    }
    var request = {
        message: JSON.stringify(object)
    }

    $.ajax({
        type: "GET",
        url: Character_Roll20URL,
        contentType: contentTypeObj.text,
        data: request,
        success: function (resp) {
            var response = JSON.parse(resp);

            if (response.Error) {
                console.log(response.Error);
                return;
            }

            data = JSON.parse(response.Data);

            playerThatCallsId = data.PlayerId; // once this is set all the other calls make sense, otherwise they will return

            console.log(data.PlayerId, data.Logbook.StatsRoll);

            $(statsPlaceholder).text(data.Logbook.StatsRoll);

        },
        error: function (err) {
            console.log(err);
        }
    });
}











