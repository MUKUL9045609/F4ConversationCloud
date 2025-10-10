$(document).ready(function () {
    console.log($.validator);
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
        console.log('Hello');

        //if (!value.includes("{{")) return true;

        if (variableType === "Number") {
            return /^\{\{\d+\}\}$/.test(value.trim());
        } else if (variableType === "Name") {
            return /^\{\{[a-z]+[a-z0-9_]*\}\}$/.test(value.trim());
        }

        return false;
    });

    $.validator.unobtrusive.adapters.add("headervariableformat", ["VariableType"], function (options) {
       
        options.rules["headervariableformat"] = options.params.VariableType;
        options.messages["headervariableformat"] = options.message;
        console.log(options)
    });

    $("form").validate({
        rules: {
            Header: {
                headervariableformat: true // Apply your custom method
            }
        },
        messages: {
            Header: {
                headervariableformat: "check."
            }
        }
    });
});