// URLs
const CreatePlayer = "api/palantir/CreatePlayer";
// divIDs
const nameField = "#nameField";
const wardField = "#wardField";
const wardcheckField = "#wardcheckField";
const saveBtn = "#saveBtn";

// events
$(saveBtn).on('click', function () {

    createPlayer();
});


// functions
function createPlayer() {

    var nameValue = $(nameField)[0].value;
    var wardValue = $(wardField)[0].value;
    var wardcheckValue = $(wardcheckField)[0].value;

    var object = {
        PlayerName: nameValue,
        Ward: wardValue,
        Wardcheck: wardcheckValue
    }
    var request = {
        Message: JSON.stringify(object)
    }


    $.ajax({
        type: "POST",
        url: CreatePlayer,
        contentType: 'application/json',
        data: JSON.stringify(request),
        success: function () {
            localStorage.clear();
            window.location = "/character/character_index";
        },
        error: function (err) {
            window.alert(err);
            console.log(err);
        },
    });

}






