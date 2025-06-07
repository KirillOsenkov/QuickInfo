function onPageLoad() {
    window.onresize = onWindowResize;
    window.onpopstate = onPopState;

    updateInputBoxWidth();

    inputBox.focus();

    inputBox.onkeyup = function () {
        if (event && event.keyCode == 13) {
            search(inputBox.value);
        }
    };

    inputBox.oninput = function () {
        onSearchChange();
    };

    var query = document.location.search;
    if (query === "?demo") {
        startDemo();
        return;
    }

    searchFromCurrentUrl();
}

function searchFromCurrentUrl() {
    var query = document.location.search;
    if (query) {
        query = query.slice(1);
        if (query) {
            query = decodeURIComponent(query);
            searchFor(query);
        }
    }
    else {
        searchFor("");
    }
}

// this is called from generated onclick handlers on hyperlinks
// it is only needed to avoid the 400ms delay otherwise we could
// just set the inputBox.value
function searchFor(query) {
    if (inputBox.value !== query) {
        inputBox.value = query;
        inputBox.focus();
        updateInputBoxWidth();
    }

    search(query);
}

function onSearchChange() {
    updateInputBoxWidth();

    inputBoxText = inputBox.value;

    if (inputBoxText === "demo") {
        startDemo();
        return;
    }

    if (inputBoxText.length > 0) {
        setTimeout(function (capturedText) {
            if (capturedText === inputBox.value) {
                search(capturedText);
            }
        }, 400, inputBoxText);
    } else {
        search("");
    }
}

async function onPasteClick() {
    var text = await navigator.clipboard.readText();
    if (text && text.length < 1024) {
        searchFor(text);
    }
}

var currentDemoTerm = 0;
var terms = [
    "color",
    "lightpink",
    "lightpink|",
    "lightpink|peachpuff",
    "RGB(23,145,175)",
    "rgb 23 145 175|rgb 0 191 255",
    "rnd",
    "rnd * 100",
    "2520",
    "3 * (4 + 5)",
    "e^pi|pi^e",
    "0x42a",
    "guid",
    "3.2 7 11 5.5 19.7 0.4",
    "9,1,100,42,0,0,19",
    "ascii",
    "cake",
    "\\U0001F352",
    "F0 9F 8D 87",
    "3%2B2%2F(2%2B3)",
    "167 lb",
    "30 ounces to grams",
    "75 f",
    "26.2 miles",
    "900 ft in yards",
    "60mph",
    "5 gallons in litres",
    "1670 sq.ft",
    "?"
];

function startDemo() {
    currentDemoTerm = 0;
    searchFor(terms[0]);
    setTimeout(advanceDemo, 3000);
}

function advanceDemo() {

    if (!inputBox.value) {
        // stop the demo when the user clears the search text
        return;
    }

    if (currentDemoTerm < terms.length) {
        var term = terms[currentDemoTerm];
        searchFor(term);
        currentDemoTerm++;
        setTimeout(advanceDemo, 2000);
    }
}

function search(query) {
    if (query) {
        query = "api/answers/?query=" + encodeURIComponent(query);
        getUrl(query, serverCallback);
    }
    else {
        serverCallback("");
    }
}

function serverCallback(data) {
    displayResults(data);
    updateUrl();
}

function displayResults(data) {
    var container = document.getElementById("outputDiv");
    if (container) {
        if (container.innerHTML !== data) {
            container.innerHTML = data;
        }
    }
}

function onPopState() {
    searchFromCurrentUrl();
}

function updateUrl() {
    var query = inputBox.value;
    if (query) {
        query = "?" + encodeURIComponent(query);
        if (document.location.search !== query) {
            top.history.pushState(null, top.document.title, query);
        }
    } else {
        if (top.location.pathname !== "/" || top.location.search) {
            top.history.pushState(null, top.document.title, "/");
        }
    }
}

function onWindowResize() {
    updateInputBoxWidth();
}

function updateInputBoxWidth() {

    var pageWidth = window.innerWidth || document.body.clientWidth;
    var pasteButtonWidth = pasteButton.scrollWidth + 25;

    var minInputBoxWidth = 400;

    if (pageWidth < minInputBoxWidth + pasteButtonWidth) {
        inputBox.style.width = `calc(100% - ${pasteButtonWidth}px)`;
        return;
    }

    var length = inputBox.value.length;
    if (length > 15) {
        inputBox.style.width = `calc(100% - ${pasteButtonWidth}px)`;
    }
    else {
        inputBox.style.width = `${minInputBoxWidth}px`;
    }
}

function getUrl(url, callback) {
    var xhr = new XMLHttpRequest();
    xhr.open("GET", url, true);
    xhr.setRequestHeader("Accept", "text/html");
    xhr.onreadystatechange = function () {
        if (xhr.readyState == 4) {
            var data = xhr.responseText;
            if (typeof data === "string" && data.length > 0) {
                callback(data);
            }
        }
    };
    xhr.send();
    return xhr;
}
