Medals per Country Table
========================

This visualization creates a simple table showing the countries sorted by the total number of 
medals. However, it is written in a way that makes it easy to adapt it to sort the countries by
medals in a specific discipline. USA has the most medals overall, but what if you instead look
at road and track cycling? Or perhaps [Circket](https://en.wikipedia.org/wiki/Cricket_at_the_1900_Summer_Olympics),
which was at Olympic games only in 1900 with exactly one match?

```
let countries = 
  olympics.'by disciplines'.then.data
    .'group data'.'by Team'.'count all'.'sum Gold'.'sum Silver'.'sum Bronze'.then
    .'sort data'.'by count descending'.then
    .'filter columns'.'drop count'.then
    .paging.take(10)
    .'get the data'

table.create(countries).set(showKey=false)
```

```
"use strict";

var countries = _series.series.ordinal(_restruntime.convertSequence(function (v) {
  return v;
}, new _restruntime.RuntimeContext("http://thegamma-services.azurewebsites.net/pivot", "source=http://thegamma-services.azurewebsites.net/olympics", "").addTrace("pivot-source=/data").addTrace("pivot-take=pgid-0").addTrace("count=" + 10).addTrace("pivot-tfs=group/Team/sum/Bronze/sum/Silver/sum/Gold/count-all/key/then/sort/count/desc/then/drop/count/then/page/pgid-0/take").getValue("/pivot/data")), "key", "value", "");

_tables.table.create(countries).set(null, false).show("outmedals-per-country");
```

If you want to adapt the table to show different disciplines, you can just use the "options" user
interface. The first control lets you choose one or more disciplines that you are interested in.
You can also filter the table in a number of different ways, but for that you'll need to modify 
the source code on the "source" tab.

For example, if you want to see the results for specific Olympic games, you can change the second line 
of the source code to filter data `by games`. For London 2012 games, the code would be
`olympics.'by games'.'London (2012)'.then.data`. Once you do this, new option in the 
"options" tab appears letting you choose another games.
