Where Phelps Got All His Medals
===============================

yo

```
let data =
  olympics.'by athlete'.'United States'.'Michael Phelps'.data
  .'group data'.'by Games'.'count all'.then
  .'get series'.'with key Games'.'and value count'

chart.pie(data)
  .set(colors=["#F4C300","#D16159","#3CB3EA"])
```

Yo
