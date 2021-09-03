// URLs
const CreateEpisode = "/api/palantir/CreateEpisode";

// divs
const sigmaWard = "#sigmaWard";
const create_epilogueBtn = "#create_epilogueBtn";
const episode_name_input = "#episode_name_input";
const episode_date_input = "#episode_date_input";
const episode_prologue_input = "#episode_prologue_input";
const episode_epilogue_input = "#episode_epilogue_input";


// objs


// on page load

// events
$(create_epilogueBtn).on("click", function () {

    if ($(sigmaWard).val().length < 1) {
        alert("sigma ward");
        return;
    }

    var object = {
        Name: $(episode_name_input).val(),
        Date: $(episode_date_input).val(),
        Prologue: $(episode_prologue_input).val(),
        Epilogue: $(episode_epilogue_input).val(),
        SigmaWard: $(sigmaWard).val()
    }
    var request = {
        message: JSON.stringify(object)
    }

    $.ajax({
        type: "POST",
        url: CreateEpisode,
        contentType: "application/json",
        data: JSON.stringify(request),
        success: function (resp) {
            var response = JSON.parse(resp);

            if (response.Error) {
                console.log(response.Error);
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



