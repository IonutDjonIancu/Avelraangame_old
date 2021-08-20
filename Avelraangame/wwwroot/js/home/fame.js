// URLs
const GetFame = "/api/palantir/GetFame";


// variables
const fightsDiv = "#fightsDiv";
const wealthDiv = "#wealthDiv";

// on page load
getFame();






// events

// functions
function getFame() {
    $.ajax({
        type: "GET",
        url: GetFame,
        contentType: "application/text",
        //data: request,
        success: function (resp) {
            var response = JSON.parse(resp);

            if (response.Error) {
                console.log(response.Error);
                return;
            } else {
                var data = JSON.parse(response.Data);

                console.log(data);
                drawCharacters(data);
            }
        },
        error: function (err) {
            console.log(err);
        }
    })
}

function drawCharacters(data) {

    $(fightsDiv).empty();
    $(wealthDiv).empty();

    for (var i = 0; i < data.Fights.length; i++) {
        var html = `
            <button class="btn btn-outline-secondary" title="${data.Fights[i].PlayerName}" style="width:200px">
                ${data.Fights[i].Name}
                <img style="border-radius:10px" src="../media/images/humans/human${data.Fights[i].Logbook.PortraitNr}.png" />
                ${data.Fights[i].Logbook.Fights}
            </button> <br />
        `;

        $(fightsDiv).append(html);
    }

    for (var j = 0; j < data.Wealth.length; j++) {
        var html2 = `
            <button class="btn btn-outline-secondary" title="${data.Wealth[j].PlayerName}" style="width:200px">
                ${data.Wealth[j].Name}
                <img style="border-radius:10px" src="../media/images/humans/human${data.Wealth[j].Logbook.PortraitNr}.png" />
                ${data.Wealth[j].Logbook.Wealth}
            </button> <br />
        `;

        $(wealthDiv).append(html2);
    }



}
