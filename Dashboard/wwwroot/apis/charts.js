var isRtl = $('html').attr('data-textdirection') === 'rtl';

function LoadBarChart(labels, series, total, key) {
    var barChartEl = document.querySelector('#' + key),
        barChartConfig = {
            chart: {
                height: 400,
                type: 'bar',
                stacked: true,

                toolbar: {
                    show: true
                },
                zoom: {
                    enabled: true
                }
            },
            responsive: [{
                breakpoint: 480,
                options: {
                    legend: {
                        position: 'bottom',
                        offsetX: -10,
                        offsetY: 0
                    }
                }
            }],
            plotOptions: {
                bar: {
                    columnWidth: '20%',

                }
            },
            legend: {
                show: true,
                position: 'top',
                horizontalAlign: 'start'
            },
            grid: {
                xaxis: {
                    lines: {
                        show: true
                    }
                }
            },
            theme: {
                palette: 'palette8' // upto palette10
            },
            stroke: {
                show: true,
                colors: ['transparent']
            },
            dataLabels: {
                enabled: true,
                formatter: function (val, opts) {
                    return val;
                }
            },
            series: [
                {
                    data: series
                }
            ],
            xaxis: {
                categories: labels
            },
            yaxis: {
                opposite: isRtl
            }
        };
    if (typeof barChartEl !== undefined && barChartEl !== null) {
        var barChart = new ApexCharts(barChartEl, barChartConfig);
        barChart.render();

    }
}

function LoadDonutChart(labels, series, total, key, Urls) {
    var donutChartEl = document.querySelector('#' + key),
        donutChartConfig = {
            chart: {
                height: 350,
                type: 'donut',
                events: {
                    dataPointSelection: function (event, chartContext, config) {
                        window.open(Urls[config.dataPointIndex], '_blank').focus();
                    }
                }
            },
            legend: {
                show: false,
                position: 'bottom'
            },
            labels: labels,
            series: series,
            theme: {
                palette: 'palette8' // upto palette10
            },
            dataLabels: {
                enabled: true,
                formatter: function (val, opt) {
                    return parseInt(val) + '%';
                }
            },
            plotOptions: {
                pie: {
                    donut: {
                        labels: {
                            show: true,
                            name: {
                                fontSize: '2rem',
                                fontFamily: 'Montserrat'
                            },
                            value: {
                                fontSize: '1rem',
                                fontFamily: 'Montserrat',
                                formatter: function (val) {
                                    return parseInt(val);
                                }
                            },
                            total: {
                                show: true,
                                fontSize: '1.5rem',
                                label: isRtl ? "الكل" : "Total",
                                formatter: function (w) {
                                    return total;
                                }
                            }
                        }
                    }
                }
            },
            responsive: [
                {
                    breakpoint: 992,
                    options: {
                        chart: {
                            height: 380
                        }
                    }
                },
                {
                    breakpoint: 576,
                    options: {
                        chart: {
                            height: 320
                        },
                        plotOptions: {
                            pie: {
                                donut: {
                                    labels: {
                                        show: true,
                                        name: {
                                            fontSize: '1.5rem'
                                        },
                                        value: {
                                            fontSize: '1rem'
                                        },
                                        total: {
                                            fontSize: '1.5rem'
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            ]
        };
    if (typeof donutChartEl !== undefined && donutChartEl !== null) {
        var donutChart = new ApexCharts(donutChartEl, donutChartConfig);
        donutChart.render();
    }
}

function LoadColumnChart(categories, series, key, Urls) {
    var columnChartEl = document.querySelector('#' + key),
        columnChartConfig = {
            chart: {
                height: 400,
                type: 'bar',
                stacked: true,
                parentHeightOffset: 0,
                toolbar: {
                    show: true
                },
                zoom: {
                    enabled: true
                },
                events: {
                    dataPointSelection: function (event, chartContext, config) {
                        var url = series[config.seriesIndex].url + Urls[config.dataPointIndex]
                        window.open(url, '_blank').focus();
                    }
                }
            },
            responsive: [{
                breakpoint: 480,
                options: {
                    legend: {
                        position: 'bottom',
                        offsetX: -10,
                        offsetY: 0
                    }
                }
            }],
            plotOptions: {
                bar: {
                    columnWidth: '20%',
                    horizontal: false,

                }
            },
            dataLabels: {
                enabled: true
            },
            legend: {
                show: true,
                position: 'top',
                horizontalAlign: 'start'
            },
            theme: {
                palette: 'palette8' // upto palette10
            },
            stroke: {
                show: true,
                colors: ['transparent']
            },
            grid: {
                xaxis: {
                    lines: {
                        show: true
                    }
                }
            },
            series: series,
            xaxis: {
                categories: categories
            },
            fill: {
                opacity: 1
            },
            yaxis: {
                opposite: isRtl
            }
        };
    if (typeof columnChartEl !== undefined && columnChartEl !== null) {
        var columnChart = new ApexCharts(columnChartEl, columnChartConfig);
        columnChart.render();
    }
}


function LoadDonut() {

    $(".Donut").each(function () {
        $(this).empty();
        $.ajax({
            url: $(this).attr('id'),
            type: "POST",
            data: $('#FilterForm').serialize(),
        }).done(function (result) {
            LoadDonutChart(result.labels, result.series, result.total, result.key, result.urls)
        }).fail(function () {
        });
    });

}
function LoadColumn() {
    $(".Column").each(function () {
        $(this).empty();
        $.ajax({
            url: $(this).attr('id'),
            type: "POST",
            data: $('#FilterForm').serialize(),
        }).done(function (result) {
            LoadColumnChart(result.categories, result.series, result.key, result.urls);
        }).fail(function () {
        });
    });
}

function LoadBar() {
    $(".Bar").each(function () {
        $(this).empty();
        $.ajax({
            url: $(this).attr('id'),
            type: "POST",
            data: $('#FilterForm').serialize(),
        }).done(function (result) {
            LoadBarChart(result.labels, result.series, result.total, result.key);
        }).fail(function () {
        });
    });
}


function GetCounts() {

    $(".count-data").each(function () {
        $.ajax({
            url: $(this).attr('id'),
            type: "POST",
            data: $('#FilterForm').serialize(),
        }).done(function (data) {
            $(this).val(data);
        }).fail(function () {
           
        });
    });

}
function LoadCharts() {
    LoadDonut();
    LoadBar();
    LoadColumn();
    
}