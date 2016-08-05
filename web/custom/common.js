// ----------------------------------------------------------------------------------------
// Loading spinner
// ----------------------------------------------------------------------------------------

function startSpinning(id) {
  var counter = 0;  
  function spin() {
    var el = document.getElementById(id);
    if (el == null) return;
    
    var offset = counter * -21;
    el.style.display = "block";
    el.style.backgroundPosition = "0px " + offset + "px";
    counter++; if (counter >= 19) counter = 0;
    setTimeout(spin, 100);
  };
  setTimeout(spin, 500);
}

// ----------------------------------------------------------------------------------------
// Loading spinner
// ----------------------------------------------------------------------------------------

var sections = [];
var hiddenArticles = [];
var loaders = {};
var primaryArticle = "";
var currentSection = "";
var originalLocation = window.location.pathname;

function registerArticle(id, visible, primary) {
  if (primary) primaryArticle = id;
  if (!visible) hiddenArticles.push(id);
  sections.push(id);
}

function setRunner(article, f) {
  if (hiddenArticles.indexOf(article) == -1) f();
  else {
    var g = loaders[article];
    if (g == null) loaders[article] = f;
    else loaders[article] = function() { f(); g(); };
  }
}

function displayNext() {
  if (hiddenArticles.length > 0) {
    document.getElementById(hiddenArticles[0]).style.display = "block";
    var f = loaders[hiddenArticles[0]];
    if (f) f();
    hiddenArticles = hiddenArticles.slice(1);
  }
}

function setCurrentSection(top) {
  var selDist = Number.MAX_SAFE_INTEGER;
  var selId = "";
  for(var i = 0; i < sections.length; i++) {
    var el = document.getElementById(sections[i]);
    if (el.offsetTop == 0) continue;
    var dist = Math.abs(top - el.offsetTop);
    if (dist < selDist) { selDist = dist; selId = sections[i]; }
  }
  var newSection = selId == primaryArticle ? originalLocation : ("/" + selId);
  if (currentSection == "") currentSection = newSection;
  if (newSection != currentSection) {
    currentSection = newSection;
    history.replaceState({}, newSection, newSection);
  }
}

window.onscroll = function() {
  if (sections.length == 0) return;
  
  var body = document.body, html = document.documentElement;
  var height = -window.innerHeight + 
    Math.max(body.scrollHeight, body.offsetHeight, 
      html.clientHeight, html.scrollHeight, html.offsetHeight);
  var top = document.body.scrollTop;    

  setCurrentSection(top);
  if (height - top < 200) displayNext(); 
}
