// URLs
const CharacterCreationRoll20 = "/api/palantir/CharacterCreationRoll20";
const StoreRoll = "/api/palantir/StoreRoll";
const CreateCharacter = "/api/palantir/CreateCharacter";


// divIDs
const playerPlaceholder = "#playerPlaceholder";
const rollStatsBtn = "#rollStatsBtn";
const storeStatsBtn = "#storeStatsBtn";
const saveBtn = "#saveBtn";
const statsPlaceholder = "#statsPlaceholder";
const statsPlaceholder_1 = "#statsPlaceholder-1";
const statsPlaceholder_2 = "#statsPlaceholder-2";
const storeStatsPlaceholder = "#storeStatsPlaceholder";
const players = "#players";
const charName = "#charName";
let playerName;
let playerId;


// on page load
establishPlayer();
if (!playerId) {
    window.location = `/Character/Character_index`;
}







// events
$(saveBtn).on("click", function () {

    var name = $(charName).val();

    if (!playerId) {
        return;
    }

    if (name.length < 1) {
        window.alert("Character name?");
        return;
    }

    var object = {
        PlayerId: playerId,
        PlayerName: playerName,
        Name: name
    }
    var request = {
        message: JSON.stringify(object)
    }


    $.ajax({
        type: "POST",
        url: CreateCharacter,
        contentType: "application/json",
        data: JSON.stringify(request),
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

$(rollStatsBtn).on("click", function () {

    charRoll20();

});

$(storeStatsBtn).on("click", function () {

    if (!playerId) {
        return;
    }

    var object = {
        PlayerId: playerId,
        PlayerName: playerName
    }
    var request = {
        message: JSON.stringify(object)
    }

    $.ajax({
        type: "GET",
        url: StoreRoll,
        contentType: "text",
        data: request,
        success: function (resp) {
            var response = JSON.parse(resp);

            if (response.Error) {
                console.log(response.Error);
                return;
            }

            var data = JSON.parse(response.Data);

            $(storeStatsPlaceholder).text(data);

        },
        error: function (err) {
            console.log(err);
        }
    });
});




// functions

function charRoll20() {

    if (!playerId) {
        window.location = "/Character/Character_index";
        return;
    }

    var object = {
        PlayerId: playerId,
        PlayerName: playerName
    }
    var request = {
        message: JSON.stringify(object)
    }

    $.ajax({
        type: "GET",
        url: CharacterCreationRoll20,
        contentType: "text",
        data: request,
        success: function (resp) {
            var response = JSON.parse(resp);

            if (response.Error) {
                console.log(response.Error);
                return;
            }

            data = JSON.parse(response.Data);

            console.log(data.PlayerId, data.Logbook.StatsRoll);

            $(statsPlaceholder_2).text($(statsPlaceholder_1).text());
            $(statsPlaceholder_1).text($(statsPlaceholder).text());
            $(statsPlaceholder).text(data.Logbook.StatsRoll);
            colorNumbers();
        },
        error: function (err) {
            console.log(err);
        }
    });
}


function establishPlayer() {
    playerName = localStorage.getItem("playerName");
    playerId = localStorage.getItem("playerId");
}

function colorNumbers() {
    var par = $('p');

    for (var i = 0; i < par.length; i++) {
        var num = parseInt(par[i].textContent);

        if (num) {
            if (num < 20) {
                par[i].setAttribute('style', 'color:gray');
            } else if (num > 20 && num < 40) {
                par[i].setAttribute('style', 'color:aqua');
            } else if (num > 40 && num < 60) {
                par[i].setAttribute('style', 'color:yellow');
            } else if (num > 60 && num < 80) {
                par[i].setAttribute('style', 'color:mediumpurple'); 
            } else if (num > 80 && num < 100) {
                par[i].setAttribute('style', 'color:darkred');
            } else {
                par[i].setAttribute('style', 'color:red');
            }
        }
    }
}










