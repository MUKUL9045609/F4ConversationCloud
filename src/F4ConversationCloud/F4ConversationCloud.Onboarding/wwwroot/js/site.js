// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.



function showToast(message, type = "success", duration = 3000) {
    const toastEl = document.getElementById("liveToast");
    const toastBody = document.getElementById("toastMessage");

    // Safety check
   /* if (!toastEl || !toastBody) {
        console.error("Toast element not found in DOM!");
        return;
    }*/

    // Remove existing bg classes
    toastEl.classList.remove("bg-success", "bg-danger", "bg-warning", "bg-info", "bg-secondary", "text-white", "text-dark");

    // Add new type class
    switch (type) {
        case "success":
            toastEl.classList.add("bg-success", "text-white");
            break;
        case "danger":
            toastEl.classList.add("bg-danger", "text-white");
            break;
        case "warning":
            toastEl.classList.add("bg-warning", "text-dark");
            break;
        case "info":
            toastEl.classList.add("bg-info", "text-dark");
            break;
        default:
            toastEl.classList.add("bg-secondary", "text-white");
    }

    // Set message
    toastBody.textContent = message;

    // Show toast
    const toast = new bootstrap.Toast(toastEl, { delay: duration });
    toast.show();
}

//*window.onload = function () {
   /* const loader = document.getElementById('loader');
    if (loader) {
        loader.style.display = 'none';
    }
};*/

// Show loader immediately when script runs
//$("#loader").show();

// Hide loader when page fully loaded
window.onload = function () {
    $("#loader").fadeOut("slow");
};
