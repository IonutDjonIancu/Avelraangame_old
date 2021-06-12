// URLs
const createPlayerURL = "api/palantir/CreateCharacter";
const characterRoll20URL = "api/palantir/CharacterRoll20";
// divIDs
const choosePlayerBtn = "#choosePlayerBtn";
const playerPlaceholder = "#playerPlaceholder";
const rollStatsBtn = "#rollStatsBtn";
const storeStatsBtn = "#storeStatsBtn";
const statsPlaceholder = "#statsPlaceholder";
const storeStatsPlaceholder = "#storeStatsPlaceholder";
let lastRoll;
let playerAccount;
let playerThatCalls;



// events
$(choosePlayerBtn).on("click", function () {

    var account = "Djon";
    //var account = "aaaasdas";

    $(playerPlaceholder).text(account);
    playerAccount = account;
});

$(rollStatsBtn).on("click", function () {

    charRoll20();

});




function charRoll20() {

    if (!playerAccount) {
        window.alert("Set the account first.");
        return;
    }

    var object = {
        PlayerId: playerThatCalls,
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

            if (data.error) {
                console.log(data.error);
                return;
            }

            var response = JSON.parse(data.data);

            playerThatCalls = response.PlayerId;
            lastRoll = response.DiceRoll;

            console.log(playerThatCalls);
            console.log(lastRoll);
            $(statsPlaceholder).text(lastRoll);
        },
        error: function (err) {
            console.log(err);
        }
    });
}


