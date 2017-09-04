function onPageLoad() {
    window.onresize = onWindowResize;
    updateInputBoxWidth();

    lastQuery = null;

    inputBox.focus();

    inputBox.onkeyup = function () {
        if (event && event.keyCode == 13) {
            searchCore(inputBox.value);
        }
    };

    inputBox.oninput = function () {
        onSearchChange();
    };

    var query = document.location.search;
    if (query) {
        query = query.slice(1);
        if (query) {
            query = decodeURIComponent(query);
            searchFor(query);
        }
    }
}

function onWindowResize() {
    updateInputBoxWidth();
}

function onSearchChange() {
    inputBoxText = inputBox.value;
    if (inputBoxText.length > 0) {
        setTimeout(function (capturedText) {
            if (capturedText === inputBox.value) {
                search(capturedText);
            }
        }, 400, inputBoxText);
    } else {
        lastQuery = "";
        loadResults("");
    }

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

// this is called from generated onclick handlers on hyperlinks
function searchFor(query) {
    inputBox.value = query;
    search(query);
}

function search(query) {
    if (!query) {
        query = inputBox.value;
    }

    if (lastQuery === query) {
        return;
    }

    lastQuery = query;

    searchCore(query);
}

function searchCore(query) {
    query = "api/answers/?query=" + encodeURIComponent(query);
    getUrl(query, loadResults);
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

function loadResults(data) {
    var container = document.getElementById("outputDiv");
    if (container) {
        if (container.innerHTML !== data) {
            container.innerHTML = data;
            updateUrl();
        }
    }
}

function updateUrl() {
    var query = inputBox.value;
    if (query) {
        query = "?" + encodeURIComponent(query);
        if (document.location.search !== query) {
            top.history.replaceState(null, top.document.title, query);
        }
    } else {
        top.history.replaceState(null, top.document.title, "");
    }
}