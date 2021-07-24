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
        dataType: 'text',
        success: function (res, status, xhr) {
            var response = JSON.parse(res);

            var data = response.Data;
            var err = response.Error;

            if (err) {
                console.log(err);
                return;
            }

            var parsedData = JSON.parse(data);

            console.log(parsedData);
            displayItem(parsedData);
        },
        error: function (err) {

            console.log(err);

        },
    });

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
