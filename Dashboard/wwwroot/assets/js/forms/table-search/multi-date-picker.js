

// Datepicker for advanced filter
var separator = ' - ',
    rangePickr = $('.flatpickr-range'),
    rangePickr2 = $('.flatpickr-range2'),
    rangePickr3 = $('.flatpickr-range3'),
    dateFormat = 'MM/DD/YYYY';
var options = {
    autoUpdateInput: false,
    autoApply: true,
    locale: {
        format: dateFormat,
        separator: separator
    },
    opens: $('html').attr('data-textdirection') === 'rtl' ? 'left' : 'right'
};

//
if (rangePickr.length) {
    rangePickr.flatpickr({
        mode: 'range',
        dateFormat: 'm/d/Y',
        onClose: function (selectedDates, dateStr, instance) {
            var startDate = '',
                endDate = new Date();
            if (selectedDates[0] != undefined) {
                startDate =
                    selectedDates[0].getMonth() + 1 + '/' + selectedDates[0].getDate() + '/' + selectedDates[0].getFullYear();
                $('.start_date').val(startDate);
            }
            if (selectedDates[1] != undefined) {
                endDate =
                    selectedDates[1].getMonth() + 1 + '/' + selectedDates[1].getDate() + '/' + selectedDates[1].getFullYear();
                $('.end_date').val(endDate);
            }
            $(rangePickr).trigger('change').trigger('keyup');
        }
    });
}


if (rangePickr2.length) {
    rangePickr2.flatpickr({
        mode: 'range',
        dateFormat: 'm/d/Y',
        onClose: function (selectedDates, dateStr, instance) {
            var startDate = '',
                endDate = new Date();
            if (selectedDates[0] != undefined) {
                startDate =
                    selectedDates[0].getMonth() + 1 + '/' + selectedDates[0].getDate() + '/' + selectedDates[0].getFullYear();
                $('.start_date2').val(startDate);
            }
            if (selectedDates[1] != undefined) {
                endDate =
                    selectedDates[1].getMonth() + 1 + '/' + selectedDates[1].getDate() + '/' + selectedDates[1].getFullYear();
                $('.end_date2').val(endDate);
            }
            $(rangePickr).trigger('change').trigger('keyup');
        }
    });
}


if (rangePickr3.length) {
    rangePickr3.flatpickr({
        mode: 'range',
        dateFormat: 'm/d/Y',
        onClose: function (selectedDates, dateStr, instance) {
            var startDate = '',
                endDate = new Date();
            if (selectedDates[0] != undefined) {
                startDate =
                    selectedDates[0].getMonth() + 1 + '/' + selectedDates[0].getDate() + '/' + selectedDates[0].getFullYear();
                $('.start_date3').val(startDate);
            }
            if (selectedDates[1] != undefined) {
                endDate =
                    selectedDates[1].getMonth() + 1 + '/' + selectedDates[1].getDate() + '/' + selectedDates[1].getFullYear();
                $('.end_date3').val(endDate);
            }
            $(rangePickr).trigger('change').trigger('keyup');
        }
    });
}


// converts date strings to a Date object, then normalized into a YYYYMMMDD format (ex: 20131220). Makes comparing dates easier. ex: 20131220 > 20121220
var normalizeDate = function (dateString) {
    var date = new Date(dateString);
    var normalized =
        date.getFullYear() + '' + ('0' + (date.getMonth() + 1)).slice(-2) + '' + ('0' + date.getDate()).slice(-2);
    return normalized;
};
