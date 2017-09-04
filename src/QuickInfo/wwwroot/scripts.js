var settingTextProgrammatically = false;

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
    settingTextProgrammatically = true;
    inputBox.value = query;
    settingTextProgrammatically = false;
    search(query);
}

function onSearchChange() {
    if (settingTextProgrammatically) {
        return;
    }

    inputBoxText = inputBox.value;

    if (inputBoxText.length > 0) {
        setTimeout(function (capturedText) {
            if (capturedText === inputBox.value) {
                search(capturedText);
            }
        }, 400, inputBoxText);
    } else {
        search("");
    }

    updateInputBoxWidth();
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

    if (pageWidth < 452) {
        inputBox.style.width = "calc(100% - 52px)";
        return;
    }

    var length = inputBox.value.length;
    if (length > 15) {
        inputBox.style.width = "calc(100% - 52px)";
    }
    else {
        inputBox.style.width = "400px";
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
