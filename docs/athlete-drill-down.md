All Medals of Michael Phelps
============================

Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore 
et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut 
aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum
dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui 
officia deserunt mollit anim id est laborum.

```
let data =
  olympics.'by athlete'.'United States'.'Michael Phelps'.data
  .'filter columns'
    .'drop Athlete'.'drop Gender'.'drop Gold'.'drop Discipline'
    .'drop Silver'.'drop Bronze'.'drop Sport'.'drop Team'.then
  .'get the data'

table.create(data).set(showKey=false)
```

```
"use strict";

var data = _series.series.ordinal(_restruntime.convertSequence(function (v) {
  return v;
}, new _restruntime.RuntimeContext("http://127.0.0.1:10042/pivot", "source=http://127.0.0.1:10042/olympics", "")
.addTrace("athlete/noc-3=Michael+Phelps").addTrace("pivot-source=/data")
.addTrace("pivot-tfs=drop/Team/Sport/Bronze/Silver/Discipline/Gold/Gender/Athlete")
.getValue("/pivot/data")), "key", "value", "");

_tables.table.create(data).set(null, false).show("outathlete-drill-down");
```

Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore 
et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut 
aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum
dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui 
officia deserunt mollit anim id est laborum.

----------------------------------------------------------------------------------------------------

Alternatives
