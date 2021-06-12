// URLs
const createPlayerURL = "api/palantir/CreateCharacter";
const characterRoll20URL = "api/palantir/CharacterRoll20";
// divIDs
const choosePlayerBtn = "#choosePlayerBtn";
const playerPlaceholder = "#playerPlaceholder";
const rollStatsBtn = "#rollStatsBtn";
const statsPlaceholder = "#statsPlaceholder";
let dice;
let playerAccount;



// events
$(choosePlayerBtn).on("click", function () {

    var account = "Djon";

    $(playerPlaceholder).text(account);
    playerAccount = account;
});

$(rollStatsBtn).on("click", function () {

    charRoll20();

});




function createItem() {







    var object = {
        name: nameValue,
        ward: wardValue,
        wardcheck: wardcheckValue
    }
    var request = {
        message: JSON.stringify(object)
    }


    //$.ajax({
    //    type: "POST",
    //    url: createPlayerURL,
    //    contentType: 'application/json',
    //    data: JSON.stringify(request),
    //    success: function (data, status, xhr) {
    //        console.log(data);
    //    },
    //    error: function (err) {
    //        console.log(err);
    //    },
    //});

};

function charRoll20() {

    if (!playerAccount) {
        window.alert("Set the account first.");
        return;
    }

    var object = {
        PlayerName: playerAccount
    }
    var request = {
        message: JSON.stringify(object)
    }

    $.ajax({
        type: "GET",
        url: characterRoll20URL,
        contentType: 'text/plain',
        data: request,
        success: function (data, status, xhr) {
            console.log(data);
            $(statsPlaceholder).text(data);
        },
        error: function (err) {
            console.log(err);
        }
    });
}


