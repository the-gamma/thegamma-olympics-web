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
// Logging user events
// ----------------------------------------------------------------------------------------

function guid(){
  var d = new Date().getTime();
  if (window.performance && typeof window.performance.now === "function") d += performance.now();
  return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function(c) {
      var r = (d + Math.random()*16)%16 | 0;
      d = Math.floor(d/16);
      return (c=='x' ? r : (r&0x3|0x8)).toString(16);
  });
}

var ssid = guid();
var logStarted = false;
var pendingEvents = [];
var logTimer = -1;

function writeLog() {
  logTimer = -1;
  if (pendingEvents.length > 0) 
    $.ajax({ type: 'POST', url: "http://thegamma-logs.azurewebsites.net/log/olympics",
      data:pendingEvents.join("\n"), dataType: "text", success:function(r) { } });
  pendingEvents = [];
}

function logEvent(category, evt, article, data) {
  if (!logStarted) return;
  var usrid = document.cookie.replace(/(?:(?:^|.*;\s*)coeffusrid\s*\=\s*([^;]*).*$)|^.*$/, "$1");
  if (usrid == "") {
    usrid = guid();
    document.cookie = "coeffusrid=" + usrid;
  }
  var logObj =
    { "user":usrid, "session":ssid,
      "time":(new Date()).toISOString(),
      "url":window.location.toString(),
      "article":article,
      "category":category, "event":evt, "data": data };
  
  pendingEvents.push(JSON.stringify(logObj));
  if (logTimer != 1) clearTimeout(logTimer);
  logTimer = setTimeout(writeLog, 4000);  
}

// ----------------------------------------------------------------------------------------
// Sharing of snippets
// ----------------------------------------------------------------------------------------

var sharedCode = "";
var sharedCompiled = "";

function cannotShareSnippet() {
  $("#cannot-share-dialog").modal("show");
}

function shareSnippet(code, compiled) {
  sharedCode = code;
  sharedCompiled = compiled;

  document.getElementById("submit-title").value = "";
  document.getElementById("submit-author").value = "";
  document.getElementById("submit-info").value = "";
  document.getElementById("submit-twitter").value = "";
  $("#modal-share").css("display", "block");
  $("#modal-done").css("display", "none");
  $("#submit-error").css('visibility', 'hidden');

  $("#share-dialog").modal("show");
}

// Validate title, author and info values. If everything is good,
// send /share request to the server, wait & display confirmation
function saveAndShare()
{
  // Validate that all values have been set
  var title = document.getElementById("submit-title").value;
  var author = document.getElementById("submit-author").value;
  var twitter = document.getElementById("submit-twitter").value;
  var info = document.getElementById("submit-info").value;
  if (title == "" || author == "" || info == "")
  {
    $("#submit-error").css('visibility', 'visible');
    return;
  }

  // Send data using AJAX to the server
  var data = {
    "title": title, "author": author, "twitter": twitter,
    "description": info, "code": sharedCode, "compiled": sharedCompiled
  };
  
  $.ajax({
    url: "http://thegamma-snippets.azurewebsites.net/olympics", data: JSON.stringify(data),
    contentType: "application/json", type: "POST", dataType: "JSON"
  }).done(function (res) {
    // Display the confirmation window with links
    var link = "/shared/" + res + "/" + title.toLowerCase().replace(/[^a-z0-9]+/g,"-").replace(/^-+|-+$/g,"");    
    var ut = encodeURIComponent(title);
    var ul = encodeURIComponent("http://rio2016.thegamma.net" + link);
    document.getElementById("share-lnk-tw").href = "https://twitter.com/intent/tweet?url=" + ul + "&text=" + ut;
    document.getElementById("share-lnk-fb").href = "https://www.facebook.com/sharer.php?u=" + ul;
    document.getElementById("share-lnk-rd").href = "http://www.reddit.com/submit?url=" + ul + "&title=" + ut;
    document.getElementById("share-lnk-em").href = "mailto:?subject=" + ut + "&body=" + ul; 
    document.getElementById("result-link").href = link;
    document.getElementById("result-link").innerText = title;
    $("#modal-share").css("display", "none");
    $("#modal-done").css("display", "block");
  });
}

// ----------------------------------------------------------------------------------------
// Displaying content & URL hacking
// ----------------------------------------------------------------------------------------

var sections = [];
var sectionUrls = [];
var hiddenArticles = [];
var loaders = {};
var primaryArticle = "";
var currentSection = "";
var originalLocation = window.location.pathname;

function registerArticle(id, visible, primary) {
  if (primary) primaryArticle = id;
  if (!visible) hiddenArticles.push(id.replace(/\//g,"-"));
  sectionUrls.push(id);
  sections.push(id.replace(/\//g,"-"));
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
    logEvent("navigation", "dislay", hiddenArticles[0]);
    var f = loaders[hiddenArticles[0]];
    if (f) f();
    hiddenArticles = hiddenArticles.slice(1);
  }
}

function setCurrentSection(top) {
  var selDist = Number.MAX_SAFE_INTEGER;
  var selId = "";
  var selUrl = "";
  for(var i = 0; i < sections.length; i++) {
    var el = document.getElementById(sections[i]);
    if (el.offsetTop == 0) continue;
    var dist = Math.abs(top - el.offsetTop);
    if (dist < selDist) { selDist = dist; selId = sections[i]; selUrl = sectionUrls[i]; }
  }
  var newSection = selId == primaryArticle ? originalLocation : ("/" + selId);
  if (currentSection == "") currentSection = newSection;
  if (newSection != currentSection) {
    currentSection = newSection;
    logEvent("navigation", "scroll", newSection);
    history.replaceState({}, "/" + selUrl, "/" + selUrl);
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
