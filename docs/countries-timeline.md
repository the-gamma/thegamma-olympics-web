Foo
===

```
let data = 
  olympics.data
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

b
