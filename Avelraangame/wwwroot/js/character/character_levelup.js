// URLs
const GetCharacterLevelup = "/api/palantir/GetCharacterLevelup";
const LevelupCharacter = "/api/palantir/LevelupCharacter";
// divs
const modelJumbo = "#modelJumbo";
const nameDiv = "#nameDiv";
const statsDiv = "#statsDiv";
const statsRollDiv = "#statsRollDiv";
const skillsRollDiv = "#skillsRollDiv";
const skillsDiv = "#skillsDiv";
const saveBtn = "#saveBtn";
let playerId;
let playerName;
let characterId;
// objs



// on page load
establishPlayer();
establishCharacter();

if (!characterId) {
    window.location = `/Character/Character_select`;
} else {
    getCharacterLevelUp(playerId, characterId);
}



// events


// functions
function getCharacterLevelUp(playerId, characterId) {
    var object = {
        PlayerId: playerId,
        CharacterId: characterId
    }
    var request = {
        message: JSON.stringify(object)
    }

    $.ajax({
        type: "GET",
        url: GetCharacterLevelup,
        contentType: "text",
        data: request,
        success: function (resp) {
            var response = JSON.parse(resp);

            if (response.Error) {
                console.log(response.Error);
                window.location = `/Character/Character_select`;
                return;
            }

            var data = JSON.parse(response.Data);
            console.log(data);

            if (data.HasLevelup == false) {
                window.location = `/Character/Character_select`;
            }

            drawCharacter(data);
            addPlusMinusEventsStats();
            addPlusMinusEventsSkills();
            addSaveBtnEvent();

        },
        error: function (err) {
            console.log(err);
        }
    });

}

function drawCharacter(data) {
    showName(data.Name, data.Logbook.PortraitNr);

    if (data.Bonuses[0].BonusTo === "Stats") {
        showStatsRoll(data.Bonuses[0].Value);
        showSkillsRoll(data.Bonuses[1].Value);
    } else {
        showSkillsRoll(data.Bonuses[0].Value);
        showStatsRoll(data.Bonuses[1].Value);
    }

    showStats(data.Stats);
    showSkills(data.Skills);
}

function showName(name, portraitNr) {
    var html = `
<h5>
    <img style="border-radius:10px" src="../media/images/humans/human${portraitNr}.png"/>
    ${name}
</h5>
<br />
`;
    $(nameDiv).empty();
    $(nameDiv).append(html);
}

function showStatsRoll(roll) {
    var html = `
<h5 id="statsRoll">${roll}</h5>
`;

    $(statsRollDiv).append(html);
}

function showSkillsRoll(roll) {
    var html = `
<h5 id="skillsRoll">${roll}</h5>
`;

    $(skillsRollDiv).append(html);
}

function showStats(stats) {
    var html = `
<div>
    <div>
        <div class="btn-group">
            <button class="btn btn-info" style="width:100px">Strength</button>
            <button id="valueStr" class="btn btn-outline-info">${stats.Strength}</button>
            <button id="minus.Str" class="btn sminus btn-outline-danger">-</button>
            <button id="plus.Str" class="btn splus btn-outline-success">+</button>
        </div>
    </div>
    <br />
    <div>
        <div class="btn-group">
            <button class="btn btn-info" style="width:100px">Toughness</button>
            <button id="valueTou" class="btn btn-outline-info">${stats.Toughness}</button>
            <button id="minus.Tou" class="btn sminus btn-outline-danger">-</button>
            <button id="plus.Tou" class="btn splus btn-outline-success">+</button>
        </div>
    </div>
    <br />
    <div>
        <div class="btn-group">
            <button class="btn btn-info" style="width:100px">Awareness</button>
            <button id="valueAwa" class="btn btn-outline-info">${stats.Awareness}</button>
            <button id="minus.Awa" class="btn sminus btn-outline-danger">-</button>
            <button id="plus.Awa" class="btn splus btn-outline-success">+</button>
        </div>
    </div>
    <br />
    <div>
        <div class="btn-group">
            <button class="btn btn-info" style="width:100px">Abstract</button>
            <button id="valueAbs" class="btn btn-outline-info">${stats.Abstract}</button>
            <button id="minus.Abs" class="btn sminus btn-outline-danger">-</button>
            <button id="plus.Abs" class="btn splus btn-outline-success">+</button>
        </div>
    </div>
</div>
`;

    $(statsDiv).empty();
    $(statsDiv).append(html);
}

function showSkills(skills) {
    var html = `
<div>
    <div>
        <div class="btn-group">
            <button class="btn btn-warning" style="width:100px; opacity:80%">Apothecary</button>
            <button id="valueApo" class="btn btn-outline-info">${skills.Apothecary}</button>
            <button id="minus.Apo" class="btn minus btn-outline-danger">-</button>
            <button id="plus.Apo" class="btn plus btn-outline-success">+</button>
        </div>
    </div>
    <br />
     <div>
        <div class="btn-group">
            <button class="btn btn-warning" style="width:100px; opacity:80%">Arcane</button>
            <button id="valueArc" class="btn btn-outline-info">${skills.Arcane}</button>
            <button id="minus.Arc" class="btn minus btn-outline-danger">-</button>
            <button id="plus.Arc" class="btn plus btn-outline-success">+</button>
        </div>
    </div>
    <br />
     <div>
        <div class="btn-group">
            <button class="btn btn-warning" style="width:100px; opacity:80%">Dodge</button>
            <button id="valueDod" class="btn btn-outline-info">${skills.Dodge}</button>
            <button id="minus.Dod" class="btn minus btn-outline-danger">-</button>
            <button id="plus.Dod" class="btn plus btn-outline-success">+</button>
        </div>
    </div>
    <br />
     <div>
        <div class="btn-group">
            <button class="btn btn-warning" style="width:100px; opacity:80%">Hide</button>
            <button id="valueHid" class="btn btn-outline-info">${skills.Hide}</button>
            <button id="minus.Hid" class="btn minus btn-outline-danger">-</button>
            <button id="plus.Hid" class="btn plus btn-outline-success">+</button>
        </div>
    </div>
    <br />
     <div>
        <div class="btn-group">
            <button class="btn btn-warning" style="width:100px; opacity:80%">Melee</button>
            <button id="valueMel" class="btn btn-outline-info">${skills.Melee}</button>
            <button id="minus.Mel" class="btn minus btn-outline-danger">-</button>
            <button id="plus.Mel" class="btn plus btn-outline-success">+</button>
        </div>
    </div>
    <br />
     <div>
        <div class="btn-group">
            <button class="btn btn-warning" style="width:100px; opacity:80%">Navigation</button>
            <button id="valueNav" class="btn btn-outline-info">${skills.Navigation}</button>
            <button id="minus.Nav" class="btn minus btn-outline-danger">-</button>
            <button id="plus.Nav" class="btn plus btn-outline-success">+</button>
        </div>
    </div>
    <br />
     <div>
        <div class="btn-group">
            <button class="btn btn-warning" style="width:100px; opacity:80%">Psionics</button>
            <button id="valuePsi" class="btn btn-outline-info">${skills.Psionics}</button>
            <button id="minus.Psi" class="btn minus btn-outline-danger">-</button>
            <button id="plus.Psi" class="btn plus btn-outline-success">+</button>
        </div>
    </div>
    <br />
     <div>
        <div class="btn-group">
            <button class="btn btn-warning" style="width:100px; opacity:80%">Ranged</button>
            <button id="valueRan" class="btn btn-outline-info">${skills.Ranged}</button>
            <button id="minus.Ran" class="btn minus btn-outline-danger">-</button>
            <button id="plus.Ran" class="btn plus btn-outline-success">+</button>
        </div>
    </div>
    <br />
     <div>
        <div class="btn-group">
            <button class="btn btn-warning" style="width:100px; opacity:80%">Resistance</button>
            <button id="valueRes" class="btn btn-outline-info">${skills.Resistance}</button>
            <button id="minus.Res" class="btn minus btn-outline-danger">-</button>
            <button id="plus.Res" class="btn plus btn-outline-success">+</button>
        </div>
    </div>
    <br />
     <div>
        <div class="btn-group">
            <button class="btn btn-warning" style="width:100px; opacity:80%">Scouting</button>
            <button id="valueSco" class="btn btn-outline-info">${skills.Scouting}</button>
            <button id="minus.Sco" class="btn minus btn-outline-danger">-</button>
            <button id="plus.Sco" class="btn plus btn-outline-success">+</button>
        </div>
    </div>
    <br />
     <div>
        <div class="btn-group">
            <button class="btn btn-warning" style="width:100px; opacity:80%">Social</button>
            <button id="valueSoc" class="btn btn-outline-info">${skills.Social}</button>
            <button id="minus.Soc" class="btn minus btn-outline-danger">-</button>
            <button id="plus.Soc" class="btn plus btn-outline-success">+</button>
        </div>
    </div>
    <br />
     <div>
        <div class="btn-group">
            <button class="btn btn-warning" style="width:100px; opacity:80%">Spot</button>
            <button id="valueSpo" class="btn btn-outline-info">${skills.Spot}</button>
            <button id="minus.Spo" class="btn minus btn-outline-danger">-</button>
            <button id="plus.Spo" class="btn plus btn-outline-success">+</button>
        </div>
    </div>
    <br />
    <div>
        <div class="btn-group">
            <button class="btn btn-warning" style="width:100px; opacity:80%">Survival</button>
            <button id="valueSur" class="btn btn-outline-info">${skills.Survival}</button>
            <button id="minus.Sur" class="btn minus btn-outline-danger">-</button>
            <button id="plus.Sur" class="btn plus btn-outline-success">+</button>
        </div>
    </div>
    <br />
    <div>
        <div class="btn-group">
            <button class="btn btn-warning" style="width:100px; opacity:80%">Tactics</button>
            <button id="valueTac" class="btn btn-outline-info">${skills.Tactics}</button>
            <button id="minus.Tac" class="btn minus btn-outline-danger">-</button>
            <button id="plus.Tac" class="btn plus btn-outline-success">+</button>
        </div>
    </div>
    <br />
    <div>
        <div class="btn-group">
            <button class="btn btn-warning" style="width:100px; opacity:80%">Traps</button>
            <button id="valueTra" class="btn btn-outline-info">${skills.Traps}</button>
            <button id="minus.Tra" class="btn minus btn-outline-danger">-</button>
            <button id="plus.Tra" class="btn plus btn-outline-success">+</button>
        </div>
    </div>
    <br />
    <div>
        <div class="btn-group">
            <button class="btn btn-warning" style="width:100px; opacity:80%">Unarmed</button>
            <button id="valueUna" class="btn btn-outline-info">${skills.Unarmed}</button>
            <button id="minus.Una" class="btn minus btn-outline-danger">-</button>
            <button id="plus.Una" class="btn plus btn-outline-success">+</button>
        </div>
    </div>
</div>
`;

    $(skillsDiv).empty();
    $(skillsDiv).append(html);
}


function addPlusMinusEventsStats() {

    $(".splus").on("click", function () {
        var statsRoll = $("#statsRoll");
        var current = parseInt(statsRoll.text());

        if (current < 1) {
            return;
        }

        var id = this.id;
        var stat = id.split(".")[1];

        var identifier = $(`#value${stat}`);

        var value = parseInt(identifier.text());
        value++;
        increaseDecreaseStatsRollDiv(true, current, statsRoll);

        identifier.text(value);

    });

    $(".sminus").on("click", function () {
        var statsRoll = $("#statsRoll");
        var current = parseInt(statsRoll.text());

        var id = this.id;
        var stat = id.split(".")[1];

        var identifier = $(`#value${stat}`);

        var value = parseInt(identifier.text());
        value--;

        if (value < 1) {
            return;
        }

        increaseDecreaseStatsRollDiv(false, current, statsRoll);

        identifier.text(value);
    });
}

function addPlusMinusEventsSkills() {

    $(".plus").on("click", function () {
        var skillsRoll = $("#skillsRoll");
        var current = parseInt(skillsRoll.text());

        if (current < 1) {
            return;
        }

        var id = this.id;
        var skill = id.split(".")[1];

        var identifier = $(`#value${skill}`);

        var value = parseInt(identifier.text());
        value++;
        increaseDecreaseSkillsRollDiv(true, current, skillsRoll);

        identifier.text(value);
    });

    $(".minus").on("click", function () {
        var skillsRoll = $("#skillsRoll");
        var current = parseInt(skillsRoll.text());

        var id = this.id;
        var stat = id.split(".")[1];

        var identifier = $(`#value${stat}`);

        var value = parseInt(identifier.text());
        value--;

        if (value < 10) {
            return;
        }

        increaseDecreaseSkillsRollDiv(false, current, skillsRoll);

        identifier.text(value);
    });
}

function increaseDecreaseStatsRollDiv(subtract, current, rollDiv) {

    if (subtract == true) {
        current--;
        rollDiv.text(current);
    } else {
        current++;
        rollDiv.text(current);
    }
}

function increaseDecreaseSkillsRollDiv(subtract, current, rollDiv) {

    if (subtract == true) {
        current--;
        rollDiv.text(current);
    } else {
        current++;
        rollDiv.text(current);
    }
}

function addSaveBtnEvent() {

    $(saveBtn).on("click", function () {

        var str = parseInt($("#valueStr").text());
        var tou = parseInt($("#valueTou").text());
        var awa = parseInt($("#valueAwa").text());
        var abs = parseInt($("#valueAbs").text());

        var apo = parseInt($("#valueApo").text());
        var arc = parseInt($("#valueArc").text());
        var dod = parseInt($("#valueDod").text());
        var hid = parseInt($("#valueHid").text());
        var mel = parseInt($("#valueMel").text());
        var nav = parseInt($("#valueNav").text());
        var psi = parseInt($("#valuePsi").text());
        var ran = parseInt($("#valueRan").text());
        var res = parseInt($("#valueRes").text());
        var sco = parseInt($("#valueSco").text());
        var soc = parseInt($("#valueSoc").text());
        var spo = parseInt($("#valueSpo").text());
        var sur = parseInt($("#valueSur").text());
        var tac = parseInt($("#valueTac").text());
        var tra = parseInt($("#valueTra").text());
        var una = parseInt($("#valueUna").text());

        var stats = {
            Strength: str,
            Toughness: tou,
            Awareness: awa,
            Abstract: abs
        };

        var skills = {
            Apothecary: apo,
            Arcane: arc,
            Dodge: dod,
            Hide: hid,
            Melee: mel,
            Navigation: nav,
            Psionics: psi,
            Ranged: ran,
            Resistance: res,
            Scouting: sco,
            Social: soc,
            Spot: spo,
            Survival: sur,
            Tactics: tac,
            Traps: tra,
            Unarmed: una
        };


        var charVm = {
            PlayerId: playerId,
            CharacterId: characterId,
            Stats: stats,
            Skills: skills
        };

        var request = {
            message: JSON.stringify(charVm)
        };

        $.ajax({
            url: LevelupCharacter,
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify(request),
            success: function (resp) {

                window.location = "/Character/Character_select";

            },
            error: function (err) {
                console.log(err);
            }
        });
    });
}

function establishPlayer() {
    playerName = localStorage.getItem("playerName");
    playerId = localStorage.getItem("playerId");
}

function establishCharacter() {
    characterId = localStorage.getItem("characterId");
}
