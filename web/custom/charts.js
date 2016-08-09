var chartsToDraw = [];
var googleLoaded = false;

function drawChartOnLoad(f) {
  if (googleLoaded) f();
  else chartsToDraw.push(f);
};

google.load('visualization', '1', { 'packages': ['corechart'] });
google.setOnLoadCallback(function () {
  googleLoaded = true;
  for (var i = 0; i < chartsToDraw.length; i++) chartsToDraw[i]();
  chartsToDraw = undefined;
});
  
function drawChart(f) {
  drawChartOnLoad(function() {
    f(function (info) {
      var chart = info[0];
      var data = info[1];
      var id = info[2];
      var ctor = eval("(function(a) { return new google.visualization." + chart.typeName + " (a); })");
      var ch = ctor(document.getElementById(id));
      if (chart.options.height == undefined) chart.options.height = 400;
      ch.draw(data, chart.options);
    });
  });
}
