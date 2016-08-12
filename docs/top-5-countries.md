A Visual History of the Olympics
================================

In this visualization, we draw a timeline showing the number of medals of the top 5 countries
over the entire history of the Olympic games. To find who the top medalists are, you can
[look at our 'medals by country' table](/medals-per-country). The visualization is inspired by
the fantastic article [A Visual History of Which Countries Have Dominated the Summer 
Olympics](http://www.nytimes.com/interactive/2016/08/08/sports/olympics/history-olympic-dominance-charts.html)
by the New York Times. We're drawing the chart using a simple area chart, so it is not as beautiful,
but it shows many of the interesting facts:

```
let countries = 
  [ olympics.'by teams'.'United States'.then.data,
    olympics.'by teams'.'Russian Empire'.'or Russian Federation'
      .'or Soviet Union'.'or Unified Team'.then.data,
    olympics.'by teams'.'United Kingdom (Great Britain)'.then.data,
    olympics.'by teams'.Germany.'or East Germany'.'or West Germany'
      .'or Unified Team of Germany'.then.data,
    olympics.'by teams'.France.then.data ]

let names = 
  [ "USA", "Russia", "United Kingdom", "Germany", "France" ]

let medals = 
  series.values(countries).map(fun x -> 
    x.'group data'.'by Year'.'count distinct Event'.then
     .'get series'.'with key Year'.'and value Event'
     .realign(series.rangeBy(1896,2012,4), 0))

chart.areas(medals, names=names)
  .hAxis(format="")
  .vAxis(viewWindowMode="maximized")
  .set(colors=["#0085C7","#CC454E","#27884C","#F4C300","#B5B5B5"],
       curveType="function",isStacked=true)
  .titleTextStyle(fontName="Roboto", bold=false, fontSize=17)
  .legend(position="top")
```

```
"use strict";

var countries = [new _restruntime.RuntimeContext("http://thegamma-services.azurewebsites.net/pivot", "source=http://thegamma-services.azurewebsites.net/olympics", "").addTrace("teams=United+States").addTrace("pivot-source=/data"), new _restruntime.RuntimeContext("http://thegamma-services.azurewebsites.net/pivot", "source=http://thegamma-services.azurewebsites.net/olympics", "").addTrace("teams=Russian+Empire").addTrace("teams=Russian+Federation").addTrace("teams=Soviet+Union").addTrace("teams=Unified+Team").addTrace("pivot-source=/data"), new _restruntime.RuntimeContext("http://thegamma-services.azurewebsites.net/pivot", "source=http://thegamma-services.azurewebsites.net/olympics", "").addTrace("teams=United+Kingdom+(Great+Britain)").addTrace("pivot-source=/data"), new _restruntime.RuntimeContext("http://thegamma-services.azurewebsites.net/pivot", "source=http://thegamma-services.azurewebsites.net/olympics", "").addTrace("teams=Germany").addTrace("teams=East+Germany").addTrace("teams=West+Germany").addTrace("teams=Unified+Team+of+Germany").addTrace("pivot-source=/data"), new _restruntime.RuntimeContext("http://thegamma-services.azurewebsites.net/pivot", "source=http://thegamma-services.azurewebsites.net/olympics", "").addTrace("teams=France").addTrace("pivot-source=/data")];
var names = ["USA", "Russia", "United Kingdom", "Germany", "France"];

var medals = _series.series.values(countries).map(function (x) {
  return _series.series.create(_restruntime.convertTupleSequence(function (v) {
    return Number(v);
  }, function (v) {
    return Number(v);
  }, x.addTrace("pivot-tfs=group/Year/count-dist/Event/key/then/series/Year/Event").getValue("/pivot/data")), "key", "value", "").realign(_series.series.rangeBy(1896, 2012, 4), 0);
});

_charts.chart.areas(medals, names).hAxis(null, null, null, "", null, null, null, null, null, null, null, null, null, null, null, null, null, null).vAxis(null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, "maximized").set(null, null, null, null, ["#0085C7", "#CC454E", "#27884C", "#F4C300", "#B5B5B5"], null, null, null, null, null, null, null, true, null, null, null, null, null, null, null, null, null, null, null).titleTextStyle("Roboto", 17, false, null, null, null, null).legend(null, null, "top", null).show("outtop-5-countries");
```

The visualizations shows a number of interesting facts that go well beyond the history of
Olympic games, but reveal something about the last century:

 * World Wars — As you can see, no medals were awarded in 1916, 1940 and 1944. During the
   first and second world war, the Olympic games were cancelled.

 * Olympic Boycotts — The United States did not get any medals in 1980 because of the 
   [Moscow Olympic games boycott](https://en.wikipedia.org/wiki/1980_Summer_Olympics_boycott) and
   Soviet Union did not get any medals in 1984 because of the [L.A. Olympic games 
   boycott](https://en.wikipedia.org/wiki/1984_Summer_Olympics_boycott).

To build the visualization (see the "source"), we had to explicitly list the countries to show
and also add their names as labels for the chart. This could be a bit easier, but you can still
quite easily modify the visualization to see different aspects of the history. Below, you can
see one modification which uses the same method to visualize the countries dominating long distance
runs. Alternatively, you can choose to compare different countries by modifying the list of countries
written as `[ .. ]`. For example, compare medals by the teams of German, West Germany and East
Germany as separate entities!
