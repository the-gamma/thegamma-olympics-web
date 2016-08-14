Olympic Medals Timeline
=======================

How has the geographical distribution of medals in Olympic games changed over the last century?
In the first Olympic games in 1896, medals were awarded to 11 teams and all were either from 
Europe or from the United States. The number of teams with medals started growing rapidly after 
1980 from 36 teams to 85 teams in 2012. The visualization tracks the number of medals awarded to
different countries over time.

```
let data = 
  olympics.'by disciplines'.then.data
    .'group data'.'by Year'.'and Team'
      .'count all'.'sum Gold'.'sum Silver'.'sum Bronze'.then
      .'sort data'.'by count descending'.then
    .'get the data'

timeline.create(data)
  .set(fill="#e0e0e0", title="Olympic Medals in %title", 
    delay=400, overflowDelay = 2000,
    colors=["#CC454E","#0085C7","#27884C","#F4C300"]) 
  .using(
    coordinates = fun x -> geo.lookup(x.Team),
    time = fun x -> x.Year,
    size = fun x -> math.add(3, math.pow(x.count, 0.5)),
    info = fun x -> x.Team)
```

```
"use strict";

var data = _series.series.ordinal(_restruntime.convertSequence(function (v) {
  return v;
}, new _restruntime.RuntimeContext("http://thegamma-services.azurewebsites.net/pivot", "source=http://thegamma-services.azurewebsites.net/olympics", "").addTrace("pivot-source=/data").addTrace("pivot-tfs=group/by-Team/by-Year/sum/Bronze/sum/Silver/sum/Gold/count-all/key/then/sort/count/desc").getValue("/pivot/data")), "key", "value", "");

_maps.timeline.create(data).set("#e0e0e0", ["#CC454E", "#0085C7", "#27884C", "#F4C300"], "Olympic Medals in %title", 400, 2000).using(function (x) {
  return _maps.geo.lookup(x.Team);
}, function (x) {
  return Number(x.Year);
}, function (x) {
  return _maps.math.add(3, _maps.math.pow(Number(x.count), 0.5));
}, function (x) {
  return x.Team;
}).show("outcountries-timeline");
```

As the visualization shows, the number of different countries winning medals in the Olympic games
started growing rapidly after 1980. You can see this visualized [in a separate chart](/disciplines-timeline).
The visualization above is also easily adapted to show medals in different disciplines. 

 * To see the timeline for a specific discipline, you can go to "options" and select disciplines
   you want to include in the first control. This lets you choose one or more disciplines.
   This will make the bubbles smaller - you can make them bigger by changing the `size` function
   in the code (change `0.5` to a bigger number between `0.5` and `2.0`).
   
 * You can also edit the code to show not just specific disciplines, but individual events. For
   example, to see [medals in long-distance running](/distance-run-timeline). To do this, you 
   need to change `'by disciplines'.then` on the second line to `'by sport'` and then choose
   the sports you want to visualize. You can also use this to see only women Olympic medalists
   by using `olympics.'by gender'.Women.data`.
