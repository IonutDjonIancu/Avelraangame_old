// URLs
const createPlayerURL = "/palantir/createplayer";
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

    var object = {
        name: "aaa",
        ward: 123,
        warcheck: 1234
    }
    var request = { 'requestPayload': `${JSON.stringify(object)}` };



    $.ajax({
        type: "POST",
        url: createPlayerURL,
        contentType: 'application/json',
        data: JSON.stringify(request),
        success: function (data, status, xhr) {
            console.log("yay");
        },
        error: function (err) {
            console.log(err);
        },
    });

}






