$(document).ready(function () {

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
        const variableTypeElement = $(`[name="${param}"]`);
        const variableType = variableTypeElement.find("option:selected").text().trim();

        const trimmedValue = value.trim();

        // Allow empty or single brackets as valid
        if (trimmedValue === "" || trimmedValue === "{}" || trimmedValue === "{" || trimmedValue === "}") {
            return true;
        }

        // Reject malformed patterns
        if (trimmedValue.includes("{{}}") || trimmedValue.includes("{{}") || trimmedValue.includes("{}}")) {
            return false;
        }

        // Match all {{...}} patterns
        const matches = trimmedValue.match(/\{\{[^}]+\}\}/g);

        // Must contain exactly one valid variable
        if (!matches || matches.length !== 1) {
            return false;
        }

        const match = matches[0];

        if (variableType === "Number") {
            // Only allow {{1}} exactly
            return match === "{{1}}";
        } else if (variableType === "Name") {
            // Must start with a letter, and only contain lowercase letters, numbers, underscores
            return /^\{\{[a-z][a-z0-9_]*\}\}$/.test(match);
        }

        return false;
    });

    //$.validator.addMethod("headervariableformat", function (value, element, param) {
    //    const variableTypeElement = $(`[name="${param}"]`);
    //    const variableType = variableTypeElement.find("option:selected").text().trim();

    //    const trimmedValue = value.trim();

    //    if (variableType === "Number") {
    //        // Allow only {{1}} anywhere in the string, and no other {{number}}
    //        const matches = trimmedValue.match(/\{\{\d+\}\}/g);

    //        if (!matches) return false;

    //        return matches.every(match => match === "{{1}}");
    //    } else if (variableType === "Name") {
    //        // Allow only lowercase letters, numbers, and underscores inside {{ }}
    //        return /^\{\{[a-z]+[a-z0-9_]*\}\}$/.test(trimmedValue);
    //    }

    //    return false;
    //});

    $.validator.unobtrusive.adapters.add("headervariableformat", ["VariableType"], function (options) {
        options.rules["headervariableformat"] = options.params.VariableType;
        options.messages["headervariableformat"] = options.message;
    });

    $("form").validate();
});