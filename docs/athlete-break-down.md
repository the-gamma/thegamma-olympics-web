Where Phelps Got All His Medals
===============================

Another way of looking at the data is to group it by the different games. Michael Phelps competed
in 3 different games (excluding Rio 2016) and got 8 medals in Athens 2004, 8 medals in Beijing 2008
and 6 medals in London 2012. How many medals will he get in Rio 2016? We'll update the visualization
when we know!


```
let data =
  olympics.'by athlete'.'United States'.'Michael Phelps'.data
  .'group data'.'by Games'.'count all'.then
  .'get series'.'with key Games'.'and value count'

chart.pie(data)
  .set(colors=["#F4C300","#D16159","#3CB3EA"])
```

```
"use strict";

var data = _series.series.create(_restruntime.convertTupleSequence(function (v) {
  return v;
}, function (v) {
  return Number(v);
}, new _restruntime.RuntimeContext("http://thegamma-services.azurewebsites.net/pivot", "source=http://thegamma-services.azurewebsites.net/olympics", "").addTrace("athlete/noc-3=Michael+Phelps").addTrace("pivot-source=/data").addTrace("pivot-tfs=group/Games/count-all/key/then/series/Games/count").getValue("/pivot/data")), "key", "value", "");

_charts.chart.pie(data).set(null, ["#F4C300", "#D16159", "#3CB3EA"], null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null).show("outathlete-break-down");
```

If you sort athletes based on the number of Olympic games from which they got a medal, you'll find
that there is a number of athletes who got medals from 6 different Olympic games. (You might think
that [Istvan Szivos](https://en.wikipedia.org/wiki/Istv%C3%A1n_Sz%C3%ADv%C3%B3s_(water_polo,_born_1920)) 
has medals from 7 games, but that's because he and his father of the same name both got Olympic 
medals from water polo!)

To look at some of the other athletes, you can easily adapt the visualization and look, for example,
at the [6 different Olympic games of Aldar Garevich](shared/8/where-aldar-gerevich-got-all-his-medals),
who is a Hungarian fencer, German kayaker Birgit Fischer, Romanian rower Elisabeta Lipa and German 
show jumping rider Hans Gunter Winkler.
