AmCharts.ready(function () {
  createStockChart();
});

var chartData = [];
generateChartData();

function generateChartData() {
    var companyName = getParameterByName("stockTicker");
    if (companyName == "" || companyName == undefined) {
        companyName = window.companyNamePage;
    }
    var sUrl = '/data' + companyName + '.csv';
    $.ajax({
        type: "GET",
        url: sUrl,
        dataType: "text",
        success: function (allText) {
            var sDate = new Date();
            var sOpen = 0;
            var sClose = 0;
            var sHigh = 0;
            var sLow = 0;
            var sVolume = 0;
            var sValue = 0;
            
            var allTextLines = allText.split(/\r\n|\n/);
            var headers = allTextLines[0].split(',');
            var t = 0;
            for (var i = 1; i < allTextLines.length; i++) {
                var data = allTextLines[i].split(',');
                if (data.length == headers.length) {
                    for (var j = 0; j < headers.length; j++) {
                        if (headers[j] == "Date") {
                            sDate = new Date(data[j]);
                        }
                        else if (headers[j] == "Low") {
                            sLow = parseFloat(data[j]);
                        }
                        else if (headers[j] == "Open") {
                            sOpen = parseFloat(data[j]);
                        }
                        else if (headers[j] == "Volume") {
                            sVolume = parseInt(data[j]);
                        }
                        else if (headers[j] == "Close") {
                            sClose = parseFloat(data[j]);
                        }
                        else if (headers[j] == "High") {
                            sHigh = parseFloat(data[j]);
                        }
                    }
                    sValue = sClose * sVolume;
                    chartData[t] = ({
                        date: sDate,
                        open: sOpen,
                        close: sClose,
                        high: sHigh,
                        low: sLow,
                        volume: sVolume,
                        value: sValue
                    });
                    t = t + 1;
                }
            }
        }
    });
}

function createStockChart() {
    var chart = new AmCharts.AmStockChart();
    chart.pathToImages = "http://www.amcharts.com/lib/3/images/";

    // DATASET //////////////////////////////////////////
    var dataSet = new AmCharts.DataSet();
    dataSet.fieldMappings = [{
        fromField: "open",
        toField: "open"
    }, {
        fromField: "close",
        toField: "close"
    }, {
        fromField: "high",
        toField: "high"
    }, {
        fromField: "low",
        toField: "low"
    }, {
        fromField: "volume",
        toField: "volume"
    }, {
        fromField: "value",
        toField: "value"
    }];
    var companyName = getParameterByName("stockTicker");

    dataSet.color = "#7f8da9";
    dataSet.dataProvider = chartData;
    dataSet.title = companyName;
    dataSet.categoryField = "date";

    chart.dataSets = [dataSet];

    // PANELS ///////////////////////////////////////////                                                  
    var stockPanel = new AmCharts.StockPanel();
    stockPanel.title = "Value";
    stockPanel.showCategoryAxis = false;
    stockPanel.percentHeight = 60;

    var valueAxis = new AmCharts.ValueAxis();
    valueAxis.dashLength = 5;
    stockPanel.addValueAxis(valueAxis);

    stockPanel.categoryAxis.dashLength = 5;

    // graph of first stock panel
    var graph = new AmCharts.StockGraph();
    graph.type = "candlestick";
    graph.openField = "open";
    graph.closeField = "close";
    graph.highField = "high";
    graph.lowField = "low";
    graph.valueField = "close";
    graph.lineColor = "#7f8da9";
    graph.fillColors = "#7f8da9";
    graph.negativeLineColor = "#db4c3c";
    graph.negativeFillColors = "#db4c3c";
    graph.fillAlphas = 1;
    graph.useDataSetColors = false;
    graph.comparable = true;
    graph.compareField = "value";
    graph.showBalloon = false;
    stockPanel.addStockGraph(graph);

    var stockLegend = new AmCharts.StockLegend();
    stockLegend.valueTextRegular = undefined;
    stockLegend.periodValueTextComparing = "[[percents.value.close]]%";
    stockPanel.stockLegend = stockLegend;
    
    chart.panels = [stockPanel];

    // OTHER SETTINGS ////////////////////////////////////
    var sbsettings = new AmCharts.ChartScrollbarSettings();
    sbsettings.graph = graph;
    sbsettings.graphType = "line";
    sbsettings.usePeriod = "WW";
    chart.chartScrollbarSettings = sbsettings;

    // PERIOD SELECTOR ///////////////////////////////////
    var periodSelector = new AmCharts.PeriodSelector();
    periodSelector.position = "bottom";
    periodSelector.periods = [{
        period: "DD",
        count: 10,
        label: "10 days"
    }, {
        period: "MM",
        selected: true,
        count: 1,
        label: "1 month"
    }, {
        period: "YYYY",
        count: 1,
        label: "1 year"
    }, {
        period: "YTD",
        label: "YTD"
    }, {
        period: "MAX",
        label: "MAX"
    }];
    chart.periodSelector = periodSelector;

    chart.write('chartdiv');
}

function getParameterByName(name) {
    name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search);
    return results == null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
}