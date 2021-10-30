// URLs
const CRUDEpisode = "/api/palantir/CRUDEpisode";
const CRUDAct = "/api/palantir/CRUDAct";
const GetEpisodes = "/api/palantir/GetEpisodes";
const GetDifficulty = "/api/palantir/GetDifficulty";

// divs
const sigmaWard = "#sigmaWard";
const create_episodeBtn = "#create_episodeBtn";
const create_actBtn = "#create_actBtn";
const episode_name_input = "#episode_name_input";
const episode_story_input = "#episode_story_input";
const episode_date_input = "#episode_date_input";
const episode_prologue_input = "#episode_prologue_input";
const episode_epilogue_input = "#episode_epilogue_input";
const episode_crudAction = "#episode_crudAction";
const act_episode = "#act_episode";
const act_difficulty = "#act_difficulty";
const act_name_input = "#act_name_input";
const act_description_input = "#act_description_input";
const act_crudAction = "#act_crudAction";


// objs


// on page load
getEpisodesList();
getDifficultyList();



// events
$(create_episodeBtn).on("click", function () {

    if ($(sigmaWard).val().length < 1) {
        alert("sigma ward");
        return;
    }

    if ($(episode_date_input).val()) {
        var toSetIntVariable = parseInt($(episode_date_input).val());
    } else {
        var toSetIntVariable = 0; 
    }

    var object = {
        Name: $(episode_name_input).val(),
        Story: $(episode_story_input).val(),
        Date: toSetIntVariable,
        Prologue: $(episode_prologue_input).val(),
        Epilogue: $(episode_epilogue_input).val(),
        EpisodeCrudOperation: $(episode_crudAction).val(),
        SigmaWard: $(sigmaWard).val()
    }
    var request = {
        message: JSON.stringify(object)
    }

    $.ajax({
        type: "POST",
        url: CRUDEpisode,
        contentType: "application/json",
        data: JSON.stringify(request),
        success: function (resp) {
            var response = JSON.parse(resp);

            if (response.Error) {
                console.log(response.Error);
                alert(response.Error);
                return;
            }

            alert(response.Data);
        },
        error: function (err) {
            console.log(err);
        }
    });

});

$(create_actBtn).on("click", function () {

    if ($(sigmaWard).val().length < 1) {
        alert("sigma ward");
        return;
    }

    var object = {
        Name: $(act_name_input).val(),
        Description: $(act_description_input).val(),
        Difficulty: $(act_difficulty).val(),
        EpisodeName: $(act_episode).val(),
        ActCrudOperation: $(act_crudAction).val(),
        SigmaWard: $(sigmaWard).val()
    }
    var request = {
        message: JSON.stringify(object)
    }

    $.ajax({
        type: "POST",
        url: CRUDAct,
        contentType: "application/json",
        data: JSON.stringify(request),
        success: function (resp) {
            var response = JSON.parse(resp);

            if (response.Error) {
                console.log(response.Error);
                alert(response.Error);
                return;
            }

            alert(response.Data);
        },
        error: function (err) {
            console.log(err);
        }
    });

});

// functions
function getEpisodesList() {
    $.ajax({
        type: "GET",
        url: GetEpisodes,
        contentType: "text",
        //data: request,
        success: function (resp) {
            var response = JSON.parse(resp);

            if (response.Error) {
                console.log(response.Error);
                return;
            }

            var data = JSON.parse(response.Data);

            console.log(data);
            drawListOfEpisodes(data);
        },
        error: function (err) {
            console.log(err);
        }
    });
}

function getDifficultyList() {
    $.ajax({
        type: "GET",
        url: GetDifficulty,
        contentType: "text",
        //data: request,
        success: function (resp) {
            var response = JSON.parse(resp);

            if (response.Error) {
                console.log(response.Error);
                return;
            }

            var data = JSON.parse(response.Data);

            console.log(data);
            drawListOfDifficulty(data);
        },
        error: function (err) {
            console.log(err);
        }
    });
}

function drawListOfEpisodes(data) {
    $(act_episode).empty();

    for (var i = 0; i < data.length; i++) {
        var html = `
            <option value="${data[i].Name}">${data[i].Name}</option>
        `;

        $(act_episode).append(html);
    }
}

function drawListOfDifficulty(data) {
    $(act_difficulty).empty();

    for (var i = 0; i < data.length; i++) {
        var html = `
            <option value="${data[i]}">${data[i]}</option>
        `;

        $(act_difficulty).append(html);
    }
}


