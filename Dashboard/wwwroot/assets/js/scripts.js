(function (window, undefined) {
    'use strict';

    /*
    NOTE:
    ------
    PLACE HERE YOUR OWN JAVASCRIPT CODE IF NEEDED
    WE WILL RELEASE FUTURE UPDATES SO IN ORDER TO NOT OVERWRITE YOUR JAVASCRIPT CODE PLEASE CONSIDER WRITING YOUR SCRIPT HERE.  */

    $('.chkall').on("click", function (e) {
        var id = "#" + $(this).closest('div').find('.select2')[0].id;
        if ($(this).is(':checked')) {
            $(id + " > option").prop("selected", "selected");
            $(id).trigger("change");
        } else {
            $(id + " > option").prop("selected", "");
            $(id).trigger("change");
        }
    });
})(window);


