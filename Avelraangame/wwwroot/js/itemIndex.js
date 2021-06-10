// URLs
const generateItemPalantirURL = "api/palantir/generateitem"
const getOkUrl = "/palantir/getok"
// divIDs
let clickBtn = '#clickBtn';
let displayDiv = '#display';








//events
$(clickBtn).on('click', function () {

    createItem();

});





//functions
function createItem() {

    $.ajax({
        type: "GET",
        url: generateItemPalantirURL,
        //data: data,
        success: function (data, status, xhr) {

            logInConsole(data);
            displayItem(data);
        },
        error: function (err) {
            logInConsole(err);
        },
        dataType: 'json'
    });

}


function getOk() {

    $.ajax({
        url: getOkUrl,
        //data: data,
        success: logInConsole("great success"),
        dataType: 'text'
    });

}

function logInConsole(res) {
    console.log(res);

    //console.log(JSON.parse(res.BonusProps));
}

function displayItem(itemData) {
    var html = `
<p style="color:navajowhite">Name: ${itemData.Name}</p>
<p style="color:navajowhite">Level: ${itemData.Level}</p>
<button class="btn ${returnItemLevel(itemData.Level)}">${itemData.Name}</button>
`;

    function returnItemLevel(level) {

        var returnLevel = "";

        switch (level) {
            case 2:
                returnLevel = "btn-warning";
                break;
            case 3:
                returnLevel = "btn-success";
                break;
            case 4:
                returnLevel = "btn-info";
                break;
            case 5:
                returnLevel = "btn-dark";
                break;
            case 6:
                returnLevel = "btn-danger";
                break;
            default:
                returnLevel = "btn-secondary";
                break;
        }

        return returnLevel;
    }






    $(displayDiv).html(html);
}
