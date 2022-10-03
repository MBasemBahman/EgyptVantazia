$(function () {
  'use strict';

    var isRtl = $('html').attr('data-textdirection') === 'rtl';
    // heat chart data generator
    function generateDataHeat(count, yrange) {
        var i = 0;
        var series = [];
        while (i < count) {
            var x = 'w' + (i + 1).toString();
            var y = Math.floor(Math.random() * (yrange.max - yrange.min + 1)) + yrange.min;

            series.push({
                x: x,
                y: y
            });
            i++;
        }
        return series;
    }

    function loadDonutChart(labels, series, total, key, Urls) {
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
                                    label: $("#TotalLbl").text(),
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

    function loadColumnChart(categories, series, key, Urls) {
        var columnChartEl = document.querySelector('#' + key),
            columnChartConfig = {
                chart: {
                    height: 400,
                    type: 'bar',
                    stacked: true,
                    stackType: '100%',
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
                        columnWidth: '15%',
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
});
