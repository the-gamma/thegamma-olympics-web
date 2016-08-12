Countries Dominating Long Distance Runs
=======================================

This visualization adapts the [Visual History of Olympics](/top-5-countries) to only show medals 
of long distance runs, including the marathon, 10km and 5km runs for both men and women. The 
visualization is based on the nice article [A Visual History of Which Countries Have Dominated the 
Summer Olympics](http://www.nytimes.com/interactive/2016/08/08/sports/olympics/history-olympic-dominance-charts.html)
in the New York Times. The difference is that now you can verify how we processed the, but you can  
also add other disciplines to the list!

```
let medals = 
  olympics.'by sports'.Athletics
    .'marathon men'.'or marathon women'
    .'or 10000m men'.'or 10000m women'
    .'or 5000m men'.'or 5000m women'.then

let countries = 
  [ medals.'by teams'.'United States'.then.data,
    medals.'by teams'.'United Kingdom (Great Britain)'.then.data,
    medals.'by teams'.Finland.then.data,
    medals.'by teams'.Ethiopia.then.data,
    medals.'by teams'.Kenya.then.data ]

let names = 
  [ "US", "UK", "Finland", "Ethiopia", "Kenya" ]

let medals = 
  series.values(countries).map(fun x -> 
    x.'group data'.'by Year'.'count distinct Event'.then
     .'get series'.'with key Year'.'and value Event'
     .realign(series.rangeBy(1896,2012,4), 0))

chart.areas(medals, names=names)
  .hAxis(format="")
  .vAxis(viewWindowMode="maximized")
  .set(colors=["#959595","#B5B5B5","#27884C","#CC454E","#F4C300"],isStacked=true)
  .titleTextStyle(fontName="Roboto", bold=false, fontSize=17)
  .legend(position="top")
```

```
"use strict";

var medals = new _restruntime.RuntimeContext("http://thegamma-services.azurewebsites.net/pivot", "source=http://thegamma-services.azurewebsites.net/olympics", "").addTrace("sports/sport-1=marathon+men").addTrace("sports/sport-1=marathon+women").addTrace("sports/sport-1=10000m+men").addTrace("sports/sport-1=10000m+women").addTrace("sports/sport-1=5000m+men").addTrace("sports/sport-1=5000m+women");
var countries = [medals.addTrace("teams=United+States").addTrace("pivot-source=/data"), medals.addTrace("teams=United+Kingdom+(Great+Britain)").addTrace("pivot-source=/data"), medals.addTrace("teams=Finland").addTrace("pivot-source=/data"), medals.addTrace("teams=Ethiopia").addTrace("pivot-source=/data"), medals.addTrace("teams=Kenya").addTrace("pivot-source=/data")];
var names = ["US", "UK", "Finland", "Ethiopia", "Kenya"];

var medals = _series.series.values(countries).map(function (x) {
  return _series.series.create(_restruntime.convertTupleSequence(function (v) {
    return Number(v);
  }, function (v) {
    return Number(v);
  }, x.addTrace("pivot-tfs=group/Year/count-dist/Event/key/then/series/Year/Event").getValue("/pivot/data")), "key", "value", "").realign(_series.series.rangeBy(1896, 2012, 4), 0);
});

_charts.chart.areas(medals, names).hAxis(null, null, null, "", null, null, null, null, null, null, null, null, null, null, null, null, null, null).vAxis(null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, "maximized").set(null, null, null, null, ["#959595", "#B5B5B5", "#27884C", "#CC454E", "#F4C300"], null, null, null, null, null, null, null, true, null, null, null, null, null, null, null, null, null, null, null).titleTextStyle("Roboto", 17, false, null, null, null, null).legend(null, null, "top", null).show("outlong-distance-medals");
```

To find out which countries dominate the Olympic games in specific sports, you'll first need to
go to our [Medals per Team Table](/medals-per-country). When you click "options", you can easily
add or remove disciplines or sports. Then you can modify the visualization here to include 
different sports and different countries. Try to recreate some of the [New York Times
Visualizations](http://www.nytimes.com/interactive/2016/08/08/sports/olympics/history-olympic-dominance-charts.html)
and share your results by clicking the "share" button!
