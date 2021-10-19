// URLs
const GetEpisodes = "/api/palantir/GetEpisodes";
const GetCharacter = "/api/palantir/GetCharacter";

// divs
const stopAudio = "#stopAudio";
const charactersDiv = "#charactersDiv";
const beginBtn = "#beginBtn";
const episodesDiv = "#episodesDiv";
const modalDiv = "#modalDiv";
let playerId;
let playerName;
let characterId;

// on page load
playerId = establishPlayerId_base();
playerName = establishPlayerName_base();

base_getAliveCharactersByPlayerId(playerId, playerName, function (res) {
    base_drawCharactersInParty(res, charactersDiv);
});

getEpisodes();








// events
$(stopAudio).on("click", function () {
    $('audio').each(function () {
        this.pause(); // Stop playing
        this.currentTime = 0; // Reset time
    }); 
});

$(beginBtn).on("click", function () {
    window.location = `/Combat/Fight`;
});

$(stopAudio).on("click", function () {
    $('audio').each(function () {
        this.pause(); // Stop playing
        this.currentTime = 0; // Reset time
    });
});






//$(modalBtn).on("click", function () {

//    $(modalPlaceholderDiv).empty();

//    var html = `
//    <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
//        <div class="modal-dialog" role="document">
//            <div class="modal-content">
//                <div class="modal-header">
//                    <h5 class="modal-title" id="exampleModalLabel">Modal title</h5>
//                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
//                        <span aria-hidden="true">&times;</span>
//                    </button>
//                </div>
//                <div class="modal-body">
//                    ${text}
//                </div>
//            </div>
//        </div>
//    </div>`;

//    $(modalPlaceholderDiv).append(html);
//    $("#myModal").modal("toggle");
//    callSound();
//});






// functions
function callSound() {
    $(`audio${oldKingSound}`)[0].play();
}



//function getCharacter() {
//    var object = {
//        PlayerId: playerId,
//        PlayerName: playerName,
//        CharacterId: characterId
//    }
//    var request = {
//        message: JSON.stringify(object)
//    }

//    $.ajax({
//        type: "GET",
//        url: GetCharacter,
//        contentType: "text",
//        data: request,
//        success: function (resp) {
//            var response = JSON.parse(resp);

//            if (response.Error) {
//                console.log(response.Error);
//                return;
//            }

//            var data = JSON.parse(response.Data);
//            console.log(data);
//            drawOneCharacter_base(data, selectedCharDiv);
//        },
//        error: function (err) {
//            console.log(err);
//        }
//    });
//}

function getEpisodes() {
    $.ajax({
        type: "GET",
        url: GetEpisodes,
        contentType: "text",
        success: function (resp) {
            var response = JSON.parse(resp);

            if (response.Error) {
                console.log(response.Error);
                return;
            }

            var data = JSON.parse(response.Data);
            console.log(data);
            drawEpisodes(data);
        },
        error: function (err) {
            console.log(err);
        }
    });
}

function drawEpisodes(data) {

    $(episodesDiv).empty();

    for (var i = 0; i < data.length; i++) {
        var html = `
            <div id="${data[i].Id}_name" class="btn-group">
                <button class="btn btn-outline-info episode">${data[i].Name}</button>
                <button id="${data[i].Name}_prologue" class="btn btn-outline-secondary prologue">Prologue</button>
            </div>
        `;

        $(episodesDiv).append(html);

        for (var j = 0; j < data[i].Acts.length; j++) {

            var actDisplayName = data[i].Acts[j].Name.split("_")[2];
            // because fucking javascript doesnt fucking have a fucking first letter to capital ohhh my fucking gawd
            var resultActDisplayName = actDisplayName.charAt(0).toUpperCase() + actDisplayName.slice(1);
            // end of because

            var html2 = `
                <button id="${data[i].Acts[j].Name}" title="Act: ${data[i].Acts[j].ActNumber}" value="${data[i].Acts[j].Id}" class="btn btn-outline-warning act">Act ${data[i].Acts[j].ActNumber}: ${resultActDisplayName}</button>
            `;

            $(`#${data[i].Id}_name`).append(html2);
        }

        var html3 = `
            <button id="${data[i].Name}_epilogue" class="btn btn-outline-secondary epilogue">Epilogue</button>
        `;

        $(`#${data[i].Id}_name`).append(html3);
    }

    //addPrologueClickEvent();
    //addActClickEvent();
    //addEpilogueClickEvent();
}

function addPrologueClickEvent() {
    $(".prologue").on("click", function () {

        $(modalDiv).empty();
        var name = this.id.split("_")[0];
        var nameSuffix = this.id.split("_")[1];

        var textHtml = `
        <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="exampleModalLabel">${name} ${nameSuffix}</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        ${huurlingTexts[this.id]}
                    </div>
                </div>
            </div>
        </div>`;

        var audioHtml = `
        <audio id="${this.id}_audio" preload="none" controls="" style="margin-top: 5px;" hidden="hidden">
            <source src="/media/sounds/${this.id}.ogg" type="audio/ogg; codecs=&quot;vorbis&quot;">
        </audio>`;


        $(modalDiv).append(audioHtml);
        $(modalDiv).append(textHtml);
        $('audio').each(function () {
            this.pause(); // Stop playing
        });
        $(`#${this.id}_audio`)[0].play();
        $("#myModal").modal("toggle");
    });

}

function addActClickEvent() {
    $(".act").on("click", function () {

        $(modalDiv).empty();
        var name = this.id.split("_")[0];
        var nameSuffix = this.id.split("_")[1];
        var nameSuffixName = this.id.split("_")[2];
        var actId = `#${this.id}`;

        var textHtml = `
        <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="exampleModalLabel">${name} ${nameSuffix}: ${nameSuffixName}</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        ${huurlingTexts[this.id]}
                    </div>
                </div>
            </div>
        </div>`;

        var audioHtml = `
        <audio id="${this.id}_audio" preload="none" controls="" style="margin-top: 5px;" hidden="hidden">
            <source src="/media/sounds/${this.id}.ogg" type="audio/ogg; codecs=&quot;vorbis&quot;">
        </audio>`;

        var otherActs = $(".act");
        for (var i = 0; i < otherActs.length; i++) {
            otherActs[i].className = "btn btn-outline-warning act";
        }
        this.className = "btn btn-warning act";

        localStorage.setItem("actId", $(actId).val());
        showBeginBtn();

        $(modalDiv).append(audioHtml);
        $(modalDiv).append(textHtml);
        $('audio').each(function () {
            this.pause(); // Stop playing
        });
        $(`#${this.id}_audio`)[0].play();
        $("#myModal").modal("toggle");
    });
}

function addEpilogueClickEvent() {
    $(".epilogue").on("click", function () {

        $(modalDiv).empty();
        var name = this.id.split("_")[0];
        var nameSuffix = this.id.split("_")[1];

        var textHtml = `
        <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="exampleModalLabel">${name} ${nameSuffix}</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        ${huurlingTexts[this.id]}
                    </div>
                </div>
            </div>
        </div>`;

        var audioHtml = `
        <audio id="${this.id}_audio" preload="none" controls="" style="margin-top: 5px;" hidden="hidden">
            <source src="/media/sounds/${this.id}.ogg" type="audio/ogg; codecs=&quot;vorbis&quot;">
        </audio>`;


        $(modalDiv).append(audioHtml);
        $(modalDiv).append(textHtml);
        $('audio').each(function () {
            this.pause(); // Stop playing
        });
        $(`#${this.id}_audio`)[0].play();
        $("#myModal").modal("toggle");
    });

}

function showBeginBtn() {
    var hasCharacter = localStorage.getItem("characterId");
    var hasAct = localStorage.getItem("actId");

    if (hasCharacter && hasAct) {
        $(beginBtn).show();
    }
}



