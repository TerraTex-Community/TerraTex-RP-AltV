function copyToClipboard(text) {
    if (document.queryCommandSupported && document.queryCommandSupported("copy")) {
        const textarea = document.createElement("textarea");
        textarea.textContent = text;
        // Prevent scrolling to bottom of page in MS Edge.
        textarea.style.position = "fixed";
        document.body.appendChild(textarea);
        textarea.select();
        try {
            // Security exception may be thrown by some browsers.
            document.execCommand("copy");
        } catch (ex) {
            console.warn("Copy to clipboard failed.", ex);
        } finally {
            document.body.removeChild(textarea);
        }
    }
}

$(document).ready(function() {
    $("html").on("click",
        "a.web",
        function() {
            const href = $(this).attr("href");
            copyToClipboard(href);
            resourceEval(
                `API.sendNotification("${href
                } wurde in dein Zwischenspeicher gelegt. Du kannst es nun im Browser mit Einfügen oder Strg + v nutzen.");`);
        });
});