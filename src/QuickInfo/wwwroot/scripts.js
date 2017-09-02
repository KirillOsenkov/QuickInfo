function onPageLoad() {
    lastQuery = null;

    inputBox.focus();

    inputBox.onkeyup = function () {
        if (event && event.keyCode == 13) {
            search();
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
            inputBox.value = query;
            lastSearchString = query;
            search();
        }
    }
}

function onSearchChange() {
    if (inputBox.value.length > 0) {
        lastInputTime = new Date();
        setTimeout(checkTimeout, 400);
    } else {
        lastQuery = "";
        loadResults("");
    }
}

function checkTimeout() {
    var current = new Date();
    if ((current.getTime() - lastInputTime.getTime()) <= 300) {
        return;
    }

    search();
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
        container.innerHTML = data;
        updateUrl();
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