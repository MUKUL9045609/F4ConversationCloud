// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.



/*function showToast(message, type = "info", duration = 3000) {
    const toastEl = document.createElement("div");
    toastEl.className = "toast align-items-center border-0 show";
    toastEl.setAttribute("role", "alert");
    toastEl.setAttribute("aria-live", "assertive");
    toastEl.setAttribute("aria-atomic", "true");
    toastEl.style.position = "fixed";
    toastEl.style.top = "20px";
    toastEl.style.right = "20px";
    toastEl.style.zIndex = "9999";

    let bgClass = "bg-secondary text-white";
    switch (type) {
        case "success":
            bgClass = "bg-success text-white";
            break;
        case "danger":
            bgClass = "bg-danger text-white";
            break;
        case "warning":
            bgClass = "bg-warning text-dark";
            break;
        case "info":
            bgClass = "bg-info text-dark";
            break;
    }

    toastEl.innerHTML = `
        <div class="d-flex ${bgClass}">
            <div class="toast-body">${message}</div>
            <button type="button" class="btn-close btn-close-white me-2 m-auto"
                data-bs-dismiss="toast" aria-label="Close"></button>
        </div>`;

    document.body.appendChild(toastEl);

    const bsToast = new bootstrap.Toast(toastEl, { delay: duration });
    bsToast.show();


    setTimeout(() => toastEl.remove(), duration + 500);
}
*/

document.addEventListener('DOMContentLoaded', function () {
    const messages = document.querySelectorAll('.temp-message');
    messages.forEach(el => {
        const message = el.value;
        const type = el.getAttribute('data-type') || 'info';
        if (message) {
            showToast(message, type, 3000);
        }
    });
});


window.onload = function () {
    $("#loader").fadeOut("slow");
};


$(".toggle-password").click(function () {
    $(this).toggleClass("fa-eye fa-eye-slash");
    input = $(this).parent().find("input");
    if (input.attr("type") == "password") {
        input.attr("type", "text");
    } else {
        input.attr("type", "password");
    }
});
