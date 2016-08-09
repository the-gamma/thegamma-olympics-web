All Medals of Michael Phelps
============================

Until Rio 2016, Michael Phelps got 22 medals including 18 gold ones. This makes you wonder if he
can remember all the medals he got. To make his life easier, we can easily generate a table with 
all the medals. To do this, we use `'by athlete'` and filter all the Olympic medals to look only
at Michael Phelps and then we display the result in a table.

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
}, new _restruntime.RuntimeContext("http://thegamma-services.azurewebsites.net/pivot", "source=http://thegamma-services.azurewebsites.net/olympics", "")
.addTrace("athlete/noc-3=Michael+Phelps").addTrace("pivot-source=/data")
.addTrace("pivot-tfs=drop/Team/Sport/Bronze/Silver/Discipline/Gold/Gender/Athlete")
.getValue("/pivot/data")), "key", "value", "");

_tables.table.create(data).set(null, false).show("outathlete-drill-down");
```

If you want to change the athlete, you can modify the source code and type `.` after `'by athlete'`
on the second line. You can also do the same in "options" by picking a country and then choosing
an athlete.

For a more complicated modification, you can change the code to use grouping (similarly to the 
visualizations counting medals per athlete) to find out that Michael Phelps medals from 8 different 
disciplines, 3 of them being relay events and the remaining 5 being individual. This can by done
by using `'group data'.'by Event' after getting the data. The visualization [Medals by Event and 
Games](shared/7/medals-by-event-and-games) shows the result.
