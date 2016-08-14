Number of Teams Winning a Medal
===============================

In the early days of Olympic games, much of the world was a part of European colonies and the 
number of teams winning Olympic medals reflects this. Before the First World War, the maximal
number of teams with a medal was 20. In the interwar period, the number grew to 32. Olympic games
started becoming more diverse after the Second World War and especially after the end of colonialism.
In London 2012, the number of teams winning a medal grew to 85.

```
let data = 
  olympics.data
    .'group data'.'by Year'.'count distinct Team'.then
    .'get series'.'with key Year'.'and value Team'
    .realign(series.rangeBy(1896, 2012, 4), 0)    

chart.area(data)
  .set(colors=["#3CB3EA"])
  .legend(position="none")
  .hAxis(format="")
```
