All Time Olympic Medals Table
=============================

Everyone who has been following Olympic Games in London 2012 or Rio 2016 knows that the person with
the largest number of medals of all time is [Michael Phelps](https://en.wikipedia.org/wiki/Michael_Phelps),
but do you know who is the second and third? As you can see in the following table, the second
person (for summer Olympic games) is [Larisa Latynina](https://en.wikipedia.org/wiki/Larisa_Latynina) 
who won 9 gold medals for Soviet Union between 1956 and 1964 and the third is Finnish runner
[Paavo Nurmi](https://en.wikipedia.org/wiki/Paavo_Nurmi), also with 9 gold medals from 1920s.

```
let data =
  olympics.data
    .'group data'.'by Athlete'
      .'sum Gold'.'sum Silver'.'sum Bronze'
      .'concatenate values of Team'.then
    .'sort data'
      .'by Gold descending'.'and by Silver descending'
      .'and by Bronze descending'.then
    .paging
      .take(10)
    .'get the data'

table.create(data)
  .hideColumns(["Gold", "Silver", "Bronze"])
  .addColumn("Medals", fun v ->
    series.range(1, v.Gold).map(fun i -> html.img("/img/gold.png")).append(
    series.range(1, v.Silver).map(fun i -> html.img("/img/silver.png")).append(
    series.range(1, v.Bronze).map(fun i -> html.img("/img/bronze.png")) )) )
  .set("", false)
```

```compiled
"use strict";

var data = _series.series.ordinal(_restruntime.convertSequence(function (v) {
  return v;
}, new _restruntime.RuntimeContext("http://thegamma-services.azurewebsites.net/pivot", "source=http://thegamma-services.azurewebsites.net/olympics", "").addTrace("pivot-source=/data").addTrace("pivot-take=pgid-0").addTrace("count=" + 10).addTrace("pivot-tfs=group/Athlete/concat-vals/Team/sum/Bronze/sum/Silver/sum/Gold/key/then/sort/Bronze/desc/Silver/desc/Gold/desc/then/page/pgid-0/take").getValue("/pivot/data")), "key", "value", "");

_tables.table.create(data).hideColumns(["Gold", "Silver", "Bronze"]).addColumn("Medals", function (v) {
  return _series.series.range(1, Number(v.Gold)).map(function (i) {
    return _tables.html.img("/img/gold.png");
  }).append(_series.series.range(1, Number(v.Silver)).map(function (i) {
    return _tables.html.img("/img/silver.png");
  }).append(_series.series.range(1, Number(v.Bronze)).map(function (i) {
    return _tables.html.img("/img/bronze.png");
  })));
}).set("", false).show("outmedals-per-athlete");
```

The table shows the most important facts, but there is a lot more information that you can get from
the data if you change options of the visualization or if you change the source code that generates
it. Here is a couple of simple things you can try on your own:

 - Find out where each athlete competed — To do this, click on the "options" button. This analyzes
   the visualization and automatically lets you change some parameters. In the "Group by athlete"
   table, you can add aggregated attributes for the table. Add "concatenate values of Games"
   and drop "concatenate values of Teams".
   
 - Who is the least lucky athlete — Counting gold medals is easy, but who has the largest number of
   bronze and silver medals? To find out, remove all items from "Sort the data" in "options" and
   specify your own criteria. Choose "by Bronze descending" to find the person with most bronze
   medals!
 
 - Look at medals from London 2012 only — You can find this in an [alternative version of the 
   visualization](/shared/5/top-medalists-of-london-2012), but to do this on your own, click on 
   "source" and change the second line from  `olympics.data` to `olympics.'by game'.'London (2012)'.data`.
   This filters the data to only medals from London 2012. As you type `olympics.`, the editor
   will let you specify other filters too. You can, for example, look at [specific 
   teams](/shared/4/czech-and-slovak-medalists-of-all-time) rather than specific games.
