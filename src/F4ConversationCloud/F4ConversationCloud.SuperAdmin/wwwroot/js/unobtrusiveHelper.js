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

    $.validator.addMethod("headervariableformat", function (value, element) {
        const trimmedValue = (value || "").trim();

        if (trimmedValue === "" || trimmedValue === "{}" || trimmedValue === "{" || trimmedValue === "}") {
            return true;
        }

        if (!trimmedValue.includes("{{1}}")) {

            const malformedPatterns = ["{{}}", "{{1", "{1}}", "{{}", "{}}"];
            for (const pattern of malformedPatterns) {
                if (trimmedValue.includes(pattern)) {
                    return false;
                }
            }
        }

        // Match all {{number}} patterns
        const variableMatches = trimmedValue.match(/{{\d+}}/g);

        if (variableMatches) {
            // If any variable is not {{1}}, reject
            if (variableMatches.some(v => v !== "{{1}}")) {
                return false;
            }

            // If more than one {{1}}, reject
            if (variableMatches.length > 1) {
                return false;
            }
        }

        return true;
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

    $.validator.unobtrusive.adapters.add("headervariableformat", [], function (options) {
        options.rules["headervariableformat"] = true; 
        options.messages["headervariableformat"] = options.message;
    });

    $("form").validate();
});