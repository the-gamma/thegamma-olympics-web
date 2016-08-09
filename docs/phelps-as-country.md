If Michael Phelps Were a Country
================================

Back in 2012, The Guardian put together an amazing table treating [Michael Phelps as a 
country](https://www.theguardian.com/sport/datablog/2012/aug/01/if-michael-phelps-were-a-country).
We can do the same and count the total number of medals per country and total number of medals 
for Michael Phelps. Sorted by the number of gold medals, Michael Phelps beats for example Belarus
and Kazakhstan. And after Rio 2016, probably also Zimbabwe, Nigeria and a few more countries!

```
let phelps =
  olympics.'by athlete'.'United States'.'Michael Phelps'.data
    .'group data'.'by Athlete'.'sum Gold'.then
    .'get series'.'with key Athlete'.'and value Gold'

let data =
  olympics.data
    .'group data'.'by Team'.'sum Gold'.then
    .'sort data'.'by Gold descending'.then
    .paging.skip(47).take(10)
    .'get series'.'with key Team'.'and value Gold'

chart.columns([data, phelps], ["#F4C300","#3CB3EA"])
  .legend(position="none")
```

```
"use strict";

var data = _series.series.create(_restruntime.convertTupleSequence(function (v) {
  return v;
}, function (v) {
  return Number(v);
}, new _restruntime.RuntimeContext("http://thegamma-services.azurewebsites.net/pivot", "source=http://thegamma-services.azurewebsites.net/olympics", "").addTrace("pivot-source=/data").addTrace("pivot-skip=pgid-0").addTrace("count=" + 47).addTrace("pivot-take=pgid-0").addTrace("count=" + 10).addTrace("pivot-tfs=group/Team/sum/Gold/key/then/sort/Gold/desc/then/page/pgid-0/skip/take/then/series/Team/Gold").getValue("/pivot/data")), "key", "value", "");

var phelps = _series.series.create(_restruntime.convertTupleSequence(function (v) {
  return v;
}, function (v) {
  return Number(v);
}, new _restruntime.RuntimeContext("http://thegamma-services.azurewebsites.net/pivot", "source=http://thegamma-services.azurewebsites.net/olympics", "").addTrace("athlete/noc-3=Michael+Phelps").addTrace("pivot-source=/data").addTrace("pivot-tfs=group/Athlete/sum/Gold/key/then/series/Athlete/Gold").getValue("/pivot/data")), "key", "value", "");

_charts.chart.columns([data, phelps], ["#F4C300", "#3CB3EA"]).legend(null, null, "none", null).show("outphelps-as-country");
```

This visualization involves a bit more logic, so it is not as easy to modify, but you can easily 
change it to look at another athlete or even add multiple athletes. When you look at "options", 
you will see a number of parameters for both of the parts of the calculation, but the very first
one lets you select a different athlete. When you do that, you'll also need to change what range
of countries you are selecting. In the code, look for `skip(47).take(10)`. This skips the first
47 countries (who have way more medals than Michael Phelps) and takes the next 10, so that we
get a nice chart. You'll need to guess the right number for your favorite athlete.

Aside from showing one athlete, you can also modify the visualization to include multiple athletes.
For example, see the visualization [If Phelps and Latynina were Countries](/shared/6/if-phelps-and-latynina-were-countries),
which shows a similar chart with the two top athletes when sorted by gold medals.
