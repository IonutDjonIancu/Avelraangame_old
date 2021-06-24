// URLs
const createPlayerURL = "api/palantir/createplayer";
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
        name: nameValue,
        ward: wardValue,
        wardcheck: wardcheckValue
    }
    var request = {
        message: JSON.stringify(object)
    }


    $.ajax({
        type: "POST",
        url: createPlayerURL,
        contentType: 'application/json',
        data: JSON.stringify(request),
        success: function (data, status, xhr) {
            window.location = "/character/character_index";
        },
        error: function (err) {
            console.log(err);
        },
    });

}






