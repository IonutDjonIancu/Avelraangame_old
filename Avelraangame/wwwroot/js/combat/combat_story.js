// URLs
const GetEpisodes = "/api/palantir/GetEpisodes";

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
clearCharacterAndActFromStorage();
playerId = establishPlayerId_base();
playerName = establishPlayerName_base();

base_getAliveCharactersByPlayerId(playerId, playerName, function (res) {
    base_drawCharactersInParty(res, charactersDiv);
    setCharacterClickEvent();
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


// functions
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
                <button id="${data[i].Name}_prologue" value="${data[i].Prologue}" title="${data[i].Name} prologue" class="btn btn-outline-secondary prologue">Prologue</button>
            </div>
            <br />
            <br />
        `;

        $(episodesDiv).append(html);

        for (var j = 0; j < data[i].Acts.length; j++) {

            var actDisplayName = data[i].Acts[j].Name.split(":")[1];
            // because fucking javascript doesnt fucking have a fucking first letter to capital ohhh my fucking gawd
            var resultActDisplayName = actDisplayName.charAt(0).toUpperCase() + actDisplayName.slice(1);
            // end of because

            var html2 = `
                <button id="${data[i].Acts[j].Id}" value="${data[i].Acts[j].Description}" title="${data[i].Acts[j].Name}" class="btn btn-outline-warning act">Act ${data[i].Acts[j].ActNumber}: ${resultActDisplayName}</button>
            `;

            $(`#${data[i].Id}_name`).append(html2);
        }

        var html3 = `
            <button id="${data[i].Name}_epilogue" value="${data[i].Epilogue}" title="${data[i].Name} epilogue" class="btn btn-outline-secondary epilogue">Epilogue</button>
        `;

        $(`#${data[i].Id}_name`).append(html3);
    }

    addPrologueClickEvent();
    addActClickEvent();
    addEpilogueClickEvent();
    setActClickEvent();
}

function addPrologueClickEvent() {
    $(".prologue").on("click", function () {

        $(modalDiv).empty();
        var name = this.title;
        var text = $(this).val();

        var textHtml = `
        <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="exampleModalLabel">${name}</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        ${text}
                    </div>
                </div>
            </div>
        </div>`;

        var audioName = name.replaceAll(" ", "_");
        var audioHtml = `
        <audio id="${audioName}_audio" preload="none" controls="" style="margin-top: 5px;" hidden="hidden">
            <source src="/media/sounds/${audioName}.ogg" type="audio/ogg; codecs=&quot;vorbis&quot;">
        </audio>`;


        $(modalDiv).append(audioHtml);
        $(modalDiv).append(textHtml);
        $('audio').each(function () {
            this.pause(); // Stop playing
        });
        $(`#${audioName}_audio`)[0].play();
        $("#myModal").modal("toggle");
    });

}

function addActClickEvent() {
    $(".act").on("click", function () {

        $(modalDiv).empty();
        var name = this.title;
        var text = $(this).val();

        var textHtml = `
        <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="exampleModalLabel">${name}</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        ${text}
                    </div>
                    <button class="btn btn-light mute">Mute narrator</button>
                </div>
            </div>
        </div>`;

        var audioName1 = name.replace(":", "");
        var audioName = audioName1.replaceAll(" ", "_");
        var audioHtml = `
        <audio id="${audioName}_audio" preload="none" controls="" style="margin-top: 5px;" hidden="hidden">
            <source src="/media/sounds/${audioName}.ogg" type="audio/ogg; codecs=&quot;vorbis&quot;">
        </audio>`;

        $(modalDiv).append(audioHtml);
        $(modalDiv).append(textHtml);
        $('audio').each(function () {
            this.pause(); // Stop playing
        });
        $(`#${audioName}_audio`)[0].play();

        $(".mute").on("click", function () {
            $('audio').each(function () {
                this.pause(); // Stop playing
                this.currentTime = 0; // Reset time
            });
        });

        $("#myModal").modal("toggle");
    });
}

function addEpilogueClickEvent() {
    $(".epilogue").on("click", function () {

        $(modalDiv).empty();
        var name = this.title;
        var text = $(this).val();

        var textHtml = `
        <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="exampleModalLabel">${name}</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        ${text}
                    </div>
                </div>
            </div>
        </div>`;

        var audioName = name.replaceAll(" ", "_");
        var audioHtml = `
        <audio id="${audioName}_audio" preload="none" controls="" style="margin-top: 5px;" hidden="hidden">
            <source src="/media/sounds/${audioName}.ogg" type="audio/ogg; codecs=&quot;vorbis&quot;">
        </audio>`;


        $(modalDiv).append(audioHtml);
        $(modalDiv).append(textHtml);
        $('audio').each(function () {
            this.pause(); // Stop playing
        });
        $(`#${audioName}_audio`)[0].play();
        $("#myModal").modal("toggle");
    });

}

function showBeginBtn() {
    var hasCharacter = localStorage.getItem("characterId");
    var hasAct = localStorage.getItem("actId");

    if (hasCharacter && hasAct) {
        $(beginBtn).show(300);
    }
}

function setCharacterClickEvent() {
    $(".characterBtn").on("click", function () {

        for (var i = 0; i < $(".characterBtn").length ; i++) {
            $($(".characterBtn")[i]).removeClass("btn-info").addClass("btn-outline-info");
        }
        $(this).removeClass("btn-outline-info").addClass("btn-info");

        localStorage.setItem("characterId", this.id);
        showBeginBtn();
    });
}

function setActClickEvent() {
    $(".act").on("click", function () {

        for (var i = 0; i < $(".act").length; i++) {
            $($(".act")[i]).removeClass("btn-warning").addClass("btn-outline-warning");
        }
        $(this).removeClass("btn-outline-warning").addClass("btn-warning");

        localStorage.setItem("actId", this.id);
        showBeginBtn();
    });
}

function clearCharacterAndActFromStorage() {
    localStorage.removeItem("characterId");
    localStorage.removeItem("actId");
}
