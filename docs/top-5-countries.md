Top 5 countries
==================

Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore 
et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut 
aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum
dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui 
officia deserunt mollit anim id est laborum.

```
let countries = 
  [ olympics.'by countries'.'United States'.then.data,
    olympics.'by countries'.'Russian Empire'.'or Russian Federation'
      .'or Soviet Union'.'or Unified Team'.then.data,
    olympics.'by countries'.'United Kingdom (Great Britain)'.then.data,
    olympics.'by countries'.Germany.'or East Germany'.'or West Germany'
      .'or Unified Team of Germany'.then.data,
    olympics.'by countries'.France.then.data ]

let medals = 
  series.values(countries).map(fun x -> 
    x.'group data'.'by Edition'.'count distinct Event'.then
     .'get series'.'with key Edition'.'and value Event'
     .realign([1896 to 2012 by 4], 0))

chart.lines(medals)
```

Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore 
et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut 
aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum
dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui 
officia deserunt mollit anim id est laborum.

----------------------------------------------------------------------------------------------------

Alternatives
