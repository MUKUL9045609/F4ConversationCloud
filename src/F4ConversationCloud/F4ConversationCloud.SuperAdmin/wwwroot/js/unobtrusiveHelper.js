$(document).ready(function () {
    // Set default highlighting/unhighlighting behavior for Bootstrap
    $.validator.setDefaults({
        highlight: function (element) {
            $(element).addClass('is-invalid');  // Add Bootstrap 'is-invalid' class
            $(element).removeClass('input-validation-valid');
        },
        unhighlight: function (element) {
            $(element).removeClass('is-invalid');  // Remove Bootstrap 'is-invalid' class
            $(element).addClass('input-validation-valid');
        }
    });

    $.validator.addMethod("headervariableformat", function (value, element, param) {
        const variableType = $(`[name="${param}"]`).find("option:selected").text().trim();

        if (!value.includes("{{")) return true;

        if (variableType === "Number") {
            return /^\{\{\d+\}\}$/.test(value.trim());
        } else if (variableType === "Name") {
            return /^\{\{[a-z]+[a-z0-9_]*\}\}$/.test(value.trim());
        }

        return false;
    });

    $.validator.unobtrusive.adapters.add("headervariableformat", ["variabletype"], function (options) {
        options.rules["headervariableformat"] = options.params.variabletype;
        options.messages["headervariableformat"] = options.message;
    });
});