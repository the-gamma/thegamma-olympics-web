Long-distance Running Medalists
===============================

This is a variation on the Olympic Medals Timeline visualization, showing the countries winning
Olympic medal in long-distance running (marathon, 10k or 5km, men or women). You can see the 
dominance of the [Flying Finns](https://en.wikipedia.org/wiki/Flying_Finn) in 1920s and 1930s.
Starting with 1960s, independent African countries start competing in Olympic games and Ethiopian
and Kenyan athletes become dominant winners in 1980s and even more so in 1990s.

```
let data = 
  olympics.'by sports'.Athletics
      .'marathon men'.'or marathon women'
      .'or 10000m men'.'or 10000m women'
      .'or 5000m men'.'or 5000m women'.then.data
    .'group data'.'by Year'.'and Team'
      .'count all'.'sum Gold'.'sum Silver'.'sum Bronze'.then
      .'sort data'.'by count descending'.then
    .'get the data'

timeline.create(data)
  .set(fill="#e0e0e0", title="Long Distance Running Medalists in %title", 
    delay=400, overflowDelay = 2000,
    colors=["#CC454E","#0085C7","#27884C","#F4C300"]) 
  .using(
    coordinates = fun x -> geo.lookup(x.Team),
    time = fun x -> x.Year,
    size = fun x -> math.add(5, math.pow(x.count, 1.5)),
    info = fun x -> x.Team)
```

```
"use strict";

var data = _series.series.ordinal(_restruntime.convertSequence(function (v) {
  return v;
}, new _restruntime.RuntimeContext("http://thegamma-services.azurewebsites.net/pivot", "source=http://thegamma-services.azurewebsites.net/olympics", "").addTrace("sports/sport-1=marathon+men").addTrace("sports/sport-1=marathon+women").addTrace("sports/sport-1=10000m+men").addTrace("sports/sport-1=10000m+women").addTrace("sports/sport-1=5000m+men").addTrace("sports/sport-1=5000m+women").addTrace("pivot-source=/data").addTrace("pivot-tfs=group/by-Team/by-Year/sum/Bronze/sum/Silver/sum/Gold/count-all/key/then/sort/count/desc").getValue("/pivot/data")), "key", "value", "");

_maps.timeline.create(data).set("#e0e0e0", ["#CC454E", "#0085C7", "#27884C", "#F4C300"], "Long Distance Running Medalists in %title", 400, 2000).using(function (x) {
  return _maps.geo.lookup(x.Team);
}, function (x) {
  return Number(x.Year);
}, function (x) {
  return _maps.math.add(5, _maps.math.pow(Number(x.count), 1.5));
}, function (x) {
  return x.Team;
}).show("outdistance-run-timeline");
```

Are you interested in other sports instead? This visualization is configured to let you easily
pick other sports in athletics. Just click on "options" and pick short-distance running sports 
instead! If you want to experiment more, you can change `'by sports'.Athletics` in the source
code to another category and then choose whatever sports you want to see. For example,
[table tennis](/shared/12/table-tennis-medalists-timeline)!
