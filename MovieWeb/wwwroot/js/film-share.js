(function () {
    "use strict";

    const button = document.getElementById("share-film");
    if (!button) return;

    const status = document.getElementById("film-share-status");
    const manual = document.getElementById("film-share-manual");
    const input = document.getElementById("film-share-url");
    // Share only the film route, without temporary query parameters or fragments.
    const url = new URL(button.dataset.shareUrl, window.location.origin).href;
    input.value = url;

    function copyWithSelection() {
        manual.hidden = false;
        input.focus();
        input.select();
        input.setSelectionRange(0, input.value.length);
        try {
            return document.execCommand("copy");
        } catch (_) {
            return false;
        }
    }

    button.addEventListener("click", async function () {
        button.disabled = true;
        status.textContent = "";
        manual.hidden = true;

        let copied = false;
        try {
            if (navigator.clipboard && window.isSecureContext) {
                await navigator.clipboard.writeText(url);
                copied = true;
            }
        } catch (_) {
            // Clipboard permissions vary; offer selection-based copying below.
        }

        if (!copied) copied = copyWithSelection();

        button.disabled = false;
        if (copied) {
            manual.hidden = true;
            button.focus();
            status.textContent = "Đã sao chép liên kết phim!";
        } else {
            status.textContent = "Không thể tự động sao chép. Hãy sao chép liên kết đã chọn bên dưới.";
        }
    });
}());
