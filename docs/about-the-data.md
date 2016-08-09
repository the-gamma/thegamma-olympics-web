About the data
==============

The ultimate goal of The Gamma project is to make data-driven articles such as the ones presented
on this page fully open and reproducible. This means that they should contain all the code needed
to obtain the data from the _original source_. The current version is not quite there yet - it 
focuses on letting readers reproduce all the computations that were done when building 
visualizations and also to create and share custom perspectives on the data.

However, you can still get the [raw data in a CSV format](https://github.com/the-gamma/thegamma-services/blob/master/src/olympics/medals-expanded.csv)
from the project GitHub. This was obtained by combining data from [The 
Guardian](https://www.theguardian.com/sport/datablog/2012/jun/25/olympic-medal-winner-list-data),
which has a fantastic data set of medals until 2008 and adding results from 2012 by scraping
data from the BBC. If you are interested, you can find the [F# source code 
here](https://github.com/the-gamma/workyard/blob/master/olympics.fsx#L444) (the file also
tries to get data from [olympic.org](https://www.olympic.org/), but ironically, this is not
nearly as complete as the Guardian table...).

When you run any visualization on this site, it accesses data live from a [simple REST 
service](http://thegamma-services.azurewebsites.net/olympics) that exposes the raw data and
a more sophisticated [REST service](http://thegamma-services.azurewebsites.net/pivot) that 
implements the grouping operations. The services follow the [protocol described 
here](https://fsprojects.github.io/RestProvider/protocol.html) and can be also 
[called from F# via the REST provider](https://fsprojects.github.io/RestProvider/).
