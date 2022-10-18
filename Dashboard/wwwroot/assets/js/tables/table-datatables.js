'use strict';

$(function () {
    var isRtl = $('html').attr('data-textdirection') === 'rtl';
    var dt_ajax_table = $('.datatables');

    //$('.datatables').on('processing.dt', function (e, settings, processing) {
    //    if ($("#LoadingModal").length > 0) {
    //        $("#LoadingModal").modal(processing ? "show" : "hide");
    //    }

    //});


    $('.datatables').on('search.dt', function (e, settings, processing) {
        var value = $('.dataTables_filter input').val();
        if ($("#searchBy").length > 0) {
            $("#searchBy").val(value);
        }

    });


    if (dt_ajax_table.length) {
        $.extend(true, $.fn.dataTable.defaults, {
            "scrollX": true,
            stateSave: true,
            autoWidth: true,
            // ServerSide Setups
            processing: true,
            serverSide: true,
            // Paging Setups
            paging: true,
            // Searching Setups
            searching: { regex: true },
            // Column Definitions
            columnDefs: [
                { targets: "no-sort", orderable: false },
                { targets: "no-search", searchable: false },
                {
                    targets: "trim",
                    render: function (data, type, full, meta) {
                        if (type === "display" && data != null) {
                            data = strtrunc(data, 15);
                        }
                        return data;
                    }
                },
                {
                    // Actions
                    targets: -1,
                    title: feather.icons['settings'].toSvg({ class: 'me-50 font-small-4' }) + $("#ActionsLbl").val(),
                    orderable: false,
                    render: function (data, type, full, meta) {
                        var actions = '<div class="d-inline-flex">' +
                            '<a class="pe-1 dropdown-toggle hide-arrow text-primary" data-bs-toggle="dropdown">' +
                            feather.icons['more-vertical'].toSvg({ class: 'font-small-4' }) +
                            '</a>' +
                            '<div class="dropdown-menu dropdown-menu-end">';

                        if ($('#profile').attr('href') != undefined) {
                            actions += '<a href="' + $('#profile').attr('href') + "/" + full.id + '" class="dropdown-item">' +
                                feather.icons['file-text'].toSvg({ class: 'font-small-4 me-50' }) +
                                '' + $("#ProfileLbl").val() + '</a>';
                        }
                        if ($('#details').attr('href') != undefined) {
                            actions += '<a href="' + $('#details').attr('href') + "/" + full.id + '" class="dropdown-item modalbtn">' +
                                feather.icons['file-text'].toSvg({ class: 'font-small-4 me-50' }) +
                                '' + $("#DetailsLbl").val() + '</a>';
                        }
                        if ($('#customDetails').attr('href') != undefined) {
                            var modalType = 'modalbtn';
                            if ($("#ModalType").length > 0) {
                                modalType = $("#ModalType").val();
                            }
                            actions += '<a href="' + $('#customDetails').attr('href') + "/" + full.id + '" class="dropdown-item  ' + modalType + '">' +
                                feather.icons['file-text'].toSvg({ class: 'font-small-4 me-50' }) +
                                '' + $("#customDetailsLabel").val() + '</a>';
                        }

                        if ($('#delete').attr('href') != undefined) {
                            actions += '<a href="' + $('#delete').attr('href') + "/" + full.id + '" class="dropdown-item modalbtn">' +
                                feather.icons['trash-2'].toSvg({ class: 'font-small-4 me-50' }) +
                                '' + $("#RemoveLbl").val() + '</a>';
                        }
                        if ($('#deleteCustom').attr('href') != undefined) {
                            actions += '<a href="' + $('#deleteCustom').attr('href') + "/" + full.id + '?returnToIndex=true" class="dropdown-item modalbtn">' +
                                feather.icons['trash-2'].toSvg({ class: 'font-small-4 me-50' }) +
                                '' + $("#RemoveLbl").val() + '</a>';
                        }

                        actions += '</div>' +
                            '</div>';

                        if ($('#edit').attr('href') != undefined) {
                            var routeData = '';
                            if ($("#routeData").length > 0) {
                                routeData = $("#routeData").val();
                            }
                            actions += '<a href="' + $('#edit').attr('href') + "/" + full.id + '' + routeData + '" class="item-edit">' +
                                feather.icons['edit'].toSvg({ class: 'font-small-4' }) +
                                '</a>';
                        }


                        if ($('#editModal').attr('href') != undefined) {
                            var routeData = '';
                            if ($("#routeData").length > 0) {
                                routeData = $("#routeData").val();
                            }
                            actions += '<a href="' + $("#editModal").attr('href') + "/" + full.id + '' + routeData + '" class="item-edit modalbtn">' +
                                feather.icons['edit'].toSvg({ class: 'font-small-4' }) +
                                '</a>';
                        }


                        if ($('#editCustom').attr('href') != undefined) {
                            actions += '<a href="' + $('#editCustom').attr('href') + "/" + full.id + '?returnToIndex=true"  class="item-edit">' +
                                feather.icons['edit'].toSvg({ class: 'font-small-4' }) +
                                '</a>';
                        }
                        if ($('#editProfile').attr('href') != undefined) {
                            actions += '<a href="' + $('#edit').attr('href') + "/" + full.id + '?IsProfile=true" class="item-edit">' +
                                feather.icons['edit'].toSvg({ class: 'font-small-4' }) +
                                '</a>';
                        }
                        return (actions);
                    }
                }
            ],
            dom: '<"card-header p-1"<"head-label"><"dt-action-buttons text-end"B>><"d-flex justify-content-between align-items-center mx-0 row"<"col-sm-12 col-md-6"l><"col-sm-12 col-md-6"f>>t<"d-flex justify-content-between mx-0 row"<"col-sm-12 col-md-6"i><"col-sm-12 col-md-6"p>>',
            displayLength: 7,
            lengthMenu: [7, 10, 25, 50, 75, 100,200,500],
            buttons: [
                {
                    extend: 'collection',
                    className: 'btn btn-outline-secondary dropdown-toggle me-2',
                    text: feather.icons['share'].toSvg({ class: 'font-small-4 me-50' }) + $("#ExportLbl").val(),
                    buttons: [
                        {
                            extend: 'print',
                            text: feather.icons['printer'].toSvg({ class: 'font-small-4 me-50' }) + $("#PrintLbl").val(),
                            className: 'dropdown-item'
                        },
                        {
                            extend: 'csv',
                            text: feather.icons['file-text'].toSvg({ class: 'font-small-4 me-50' }) + $("#CsvLbl").val(),
                            className: 'dropdown-item'
                        },
                        {
                            extend: 'excel',
                            text: feather.icons['file'].toSvg({ class: 'font-small-4 me-50' }) + $("#ExcelLbl").val(),
                            className: 'dropdown-item'
                        }
                    ],
                    init: function (api, node, config) {
                        $(node).removeClass('btn-secondary');
                        $(node).parent().removeClass('btn-group');
                        setTimeout(function () {
                            $(node).closest('.dt-buttons').removeClass('btn-group').addClass('d-inline-flex');
                        }, 50);
                    }
                },
                {
                    text: feather.icons['edit'].toSvg({ class: 'me-50 font-small-4' }) + $("#EditSpecLbl").val(),
                    className: 'edit-rows btn btn-info me-2',
                    init: function (api, node, config) {
                        if ($('#editSpecific').attr('href') == undefined) {
                            $(node).hide();
                        } else {
                            $(node).removeClass('btn-secondary');
                        }
                    },
                    action: function (e, dt, button, config) {
                        
                    }
                },
                {
                    text: feather.icons['plus'].toSvg({ class: 'me-50 font-small-4' }) + $("#AddNewLbl").val(),
                    className: 'create-new btn btn-primary',
                    init: function (api, node, config) {
                        if ($('#create').attr('href') == undefined) {
                            $(node).hide();
                        } else {
                            $(node).removeClass('btn-secondary');
                        }
                    },
                    action: function (e, dt, button, config) {
                        window.location = $('#create').attr('href');
                    }
                },
                {
                    text: feather.icons['plus'].toSvg({ class: 'me-50 font-small-4' }) + $("#AddNewLbl").val(),
                    className: 'create-new btn btn-primary',
                    init: function (api, node, config) {
                        if ($('#createModal').attr('href') == undefined) {
                            $(node).hide();
                        } else {
                            $(node).removeClass('btn-secondary');
                            $(node).addClass('modalbtn');
                        }
                    }
                }
            ],
            language: {
                paginate: {
                    // remove previous & next text from pagination
                    previous: '&nbsp;',
                    next: '&nbsp;'
                },
                "search": $("#SearchLbl").val(),
                "sLengthMenu": $("#DispalyLbl").val() + "_MENU_" + $("#EntitiesLbl").val(),
            }
        });


        $('body').on('click', '.modalbtn', function () {
            event.preventDefault();
            var href = $(this).attr('href');
            $('.form-content').load(href);
            $("#Modal").modal("show");
        });


        $('body').on('click', '.xl-modalbtn', function () {
            event.preventDefault();
            var href = $(this).attr('href');
            $('.form-content').load(href);
            $("#ModalXl").modal("show");
        });

        $('body').on('submit', '.modal', function () {
            event.preventDefault();
            $.ajax({
                type: $('.form-content form').attr('method'),
                url: $('.form-content form').attr('action'),
                data: $('.form-content form').serialize(),
                success: function () {
                    dt_ajax_table.DataTable().draw();
                    // Display message back to the user here
                    $("#Modal").modal("hide");
                    $("#success").modal("show");
                },
                error: function () {
                    // Display message back to the user here
                    $("#Modal").modal("hide");

                    $("#danger").modal("show");
                }
            });
        });


    }

    function strtrunc(str, num) {
        if (str.length > num) {
            return str.slice(0, num) + "...";
        }
        else {
            return str;
        }
    }


    // Filter form control to default size for all tables
    $('.dataTables_filter .form-control').removeClass('form-control-sm');
    $('.dataTables_length .form-select').removeClass('form-select-sm').removeClass('form-control-sm');

});

