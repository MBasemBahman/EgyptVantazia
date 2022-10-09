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