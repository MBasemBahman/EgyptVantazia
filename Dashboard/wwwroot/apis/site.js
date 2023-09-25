function getGameWeak(elem, fk_Season, removeFirstelem = true) {
    var serviceUrl = "/Dashboard/Services/GetGameWeak?fk_Season=" + fk_Season;
    $.ajax({
        type: "Get",
        url: serviceUrl,
        success: function (result) {
            if (removeFirstelem) {
                $(elem).empty();
            }
            else {
                $(elem).find('option').not(':first').remove();
            }
            var options = '';
            if (result.length > 0) {
                for (var i = 0; i < result.length; i++) {
                    options += '<option value="' + result[i].id + '">' + result[i].name + '</option>'
                }
            }
            $(elem).append(options);
        }
    });
}


function getPlayers(elem, fk_Team, removeFirstelem = true) {
    var serviceUrl = "/Dashboard/Services/GetPlayers?fk_Team=" + fk_Team;
    $.ajax({
        type: "Get",
        url: serviceUrl,
        success: function (result) {
            if (removeFirstelem) {
                $(elem).empty();
            }
            else {
                $(elem).find('option').not(':first').remove();
            }
            var options = '';
            if (result.length > 0) {
                for (var i = 0; i < result.length; i++) {
                    options += '<option value="' + result[i].id + '">' + result[i].name + '</option>'
                }
            }
            $(elem).append(options);
        }
    });
}

function getTeams(elem, fk_Season, removeFirstelem = true) {
    var serviceUrl = "/Dashboard/Services/GetTeams?fk_Season=" + fk_Season;
    $.ajax({
        type: "Get",
        url: serviceUrl,
        success: function (result) {
            if (removeFirstelem) {
                $(elem).empty();
            }
            else {
                $(elem).find('option').not(':first').remove();
            }
            var options = '';
            if (result.length > 0) {
                for (var i = 0; i < result.length; i++) {
                    options += '<option value="' + result[i].id + '">' + result[i].name + '</option>'
                }
            }
            $(elem).append(options);
        }
    });
}


function getTeamGameWeak(elem, fk_Team, fk_GameWeak, removeFirstelem = true) {
    var serviceUrl = "/Dashboard/Services/GetTeamGameWeak?fk_Team=" + fk_Team + "&fk_GameWeak=" + fk_GameWeak;
    $.ajax({
        type: "Get",
        url: serviceUrl,
        success: function (result) {
            if (removeFirstelem) {
                $(elem).empty();
            }
            else {
                $(elem).find('option').not(':first').remove();
            }
            var options = '';
            if (result.length > 0) {
                for (var i = 0; i < result.length; i++) {
                    options += '<option value="' + result[i].id + '">' + result[i].name + '</option>'
                }
            }
            $(elem).append(options);
        }
    });
}



function getStatisticScores(elem, fk_StatisticCategory, removeFirstelem = true) {
    var serviceUrl = "/Dashboard/Services/GetStatisticScores?Fk_StatisticCategory=" + fk_StatisticCategory;
    $.ajax({
        type: "Get",
        url: serviceUrl,
        success: function (result) {
            if (removeFirstelem) {
                $(elem).empty();
            }
            else {
                $(elem).find('option').not(':first').remove();
            }
            var options = '';
            if (result.length > 0) {
                for (var i = 0; i < result.length; i++) {
                    options += '<option value="' + result[i].id + '">' + result[i].name + '</option>'
                }
            }
            $(elem).append(options);
        }
    });
}


