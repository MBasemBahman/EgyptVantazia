$(document).ready(function () {
    $(".navigation navigation-main ul li .active").each(function () {
        $(this).removeClass("active");
    });

    $("#d1").load($(".tab-item.active").eq(0).attr("id"));
    $("#TeamGameWeakList").addClass("active");

    $(".tab-item").each(function () {
        $(this).on('click', function () {
            $("#d1").load($(this).attr('id'));
            $(".active").each(function () {
                $(this).removeClass("active");
            });
            $(this).addClass("active");
            $("#TeamGameWeakList").addClass("active");
        });
    });

});

$('body').on('click', '.modalbtn', function (e) {
    e.preventDefault();
    let href = $(this).attr('href');
    
    $('.form-content').load(href);
    $("#Modal").modal("show");
});

// START add_scores

$(document).on("click", ".add_scores", function(e) {
    e.preventDefault();

    let fk_PlayerGameWeak = $(this).data('id');

    $('input[name=AddScoresFk_PlayerGameWeak]').val(fk_PlayerGameWeak);

    $("select[name=AddScoresIsOut]").val('');
    $("input[name=AddScoresGameTime]").val(0);
    $("input[name=AddScoresPoints]").val(0);
    $("input[name=AddScoresFinalValue]").val(0);

    $(".add_scores_modal").modal('show');
});

$(document).on("click", ".add_scores_submit", function(e) {
    e.preventDefault();

    let fk_PlayerGameWeak = $("input[name=AddScoresFk_PlayerGameWeak]").val();

    let fk_ScoreType = $("select[name=AddScoresFk_ScoreType]").val();
    let isOut = $("select[name=AddScoresIsOut]").val();
    let gameTime = $("input[name=AddScoresGameTime]").val();

    let points = $("input[name=AddScoresPoints]").val();
    let finalValue = $("input[name=AddScoresFinalValue]").val();

    $.ajax({
        url: '/SeasonEntity/TeamGameWeak/AddScore',
        method: 'post',
        data: {
            fk_ScoreType: fk_ScoreType,
            points: points,
            finalValue: finalValue,
            isOut: isOut,
            gameTime: gameTime,
            fk_PlayerGameWeak: fk_PlayerGameWeak,
        },
        success: function(result) {
            $("#success").modal('show');
        }
    });
});

// END add_scores

// START update_game_result
$(document).on("click", ".update_game_result", function(e) {
    e.preventDefault();

    $(".update_game_result_modal").modal('show');
});
// END update_game_result

// START update_account_team_points
$(document).on("click", ".update_account_team_points", function(e) {
    e.preventDefault();

    $(".update_account_team_points_modal").modal('show');
});

// START edit player_ranking
$(document).on("blur", ".player_ranking_input", function (e) {
    let fk_PlayerGameWeak = $(this).data("fk_playergameweak");
    let ranking = $(this).val();

    $.ajax({
        url: '/PlayerScoreEntity/PlayerGameWeak/UpdatePlayerGameWeakRanking',
        method: 'post',
        data: {
            fk_PlayerGameWeak: fk_PlayerGameWeak,
            ranking: ranking
        },
        success: function(data) {
            $("#success").modal('show');
        }
    });
});
// END edit player_ranking
