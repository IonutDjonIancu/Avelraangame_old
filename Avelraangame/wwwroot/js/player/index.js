// URLs
const CreatePlayer = "api/palantir/CreatePlayer";
// divIDs
const nameField = "#nameField";
const wardField = "#wardField";
const wardcheckField = "#wardcheckField";
const saveBtn = "#saveBtn";
const symbolWard = "#symbolWard";

// events
$(saveBtn).on('click', function () {
    createPlayer();
});


// functions
function createPlayer() {

    var nameValue = $(nameField).val();
    var wardValue = $(wardField).val();
    var wardcheckValue = $(wardcheckField).val();
    var symbol = $(symbolWard).val();

    var object = {
        PlayerName: nameValue,
        Ward: wardValue,
        Wardcheck: wardcheckValue,
        Symbol: symbol
    }
    var request = {
        Message: JSON.stringify(object)
    }

    $.ajax({
        type: "POST",
        url: CreatePlayer,
        contentType: 'application/json',
        data: JSON.stringify(request),
        success: function (resp) {

            var response = JSON.parse(resp);

            if (response.Error) {
                console.log(response.Error);
                window.alert(response.Error);
                return;
            }

            var data = JSON.parse(response.Data);

            setSessionCredentials_base(data.PlayerId, data.PlayerName);
            window.location = "/home/index";
        },
        error: function (err) {
            window.alert(err);
            console.log(err);
        },
    });
}






