Individual Medalists of All Time
================================

Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore 
et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut 
aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum
dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui 
officia deserunt mollit anim id est laborum.

```
let data =
  olympics.data
    .'group data'.'by Athlete'
      .'sum Gold'.'sum Silver'.'sum Bronze'
      .'concatenate values of Country'.then
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
}, new _restruntime.RuntimeContext("http://127.0.0.1:10042/pivot", "source=http://127.0.0.1:10042/olympics", "").addTrace("pivot-source=/data").addTrace("pivot-take=pgid-0").addTrace("count=" + 10).addTrace("pivot-tfs=group/Athlete/concat-vals/Country/sum/Bronze/sum/Silver/sum/Gold/key/then/sort/Bronze/desc/Silver/desc/Gold/desc/then/page/pgid-0/take").getValue("/pivot/data")), "key", "value", "");

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

Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore 
et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut 
aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum
dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui 
officia deserunt mollit anim id est laborum.

----------------------------------------------------------------------------------------------------

Alternatives
