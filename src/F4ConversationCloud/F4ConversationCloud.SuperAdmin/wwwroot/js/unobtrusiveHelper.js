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

    // Override the remote method to ensure it always checks, even when the field is blank
    $.validator.addMethod("remote", function (value, element, param) {
        // Allow validation even for empty fields
        var previous = this.previousValue(element),
            validator, data;

        if (!this.settings.messages[element.name]) {
            this.settings.messages[element.name] = {};
        }
        previous.originalMessage = this.settings.messages[element.name].remote;
        this.settings.messages[element.name].remote = previous.message;

        param = typeof param === "string" && { url: param } || param;

        if (previous.old === value) {
            return previous.valid;
        }

        previous.old = value;
        validator = this;
        this.startRequest(element);

        data = {};
        data[element.name] = value;

        $.ajax($.extend(true, {
            url: param.url || param,
            mode: "abort",
            port: "validate" + element.name,
            dataType: "json",
            data: data,
            context: validator.currentForm,
            success: function (response) {
                var valid = response === true || response === "true",
                    errors, message, submitted;

                validator.settings.messages[element.name].remote = previous.originalMessage;
                if (valid) {
                    submitted = validator.formSubmitted;
                    validator.resetInternals();
                    validator.toHide = validator.errorsFor(element);
                    validator.formSubmitted = submitted;
                    validator.successList.push(element);
                    delete validator.invalid[element.name];
                    validator.showErrors();
                } else {
                    errors = {};
                    message = response || validator.defaultMessage(element, "remote");
                    errors[element.name] = previous.message = $.isFunction(message) ? message(value) : message;
                    validator.invalid[element.name] = true;
                    validator.showErrors(errors);
                }
                previous.valid = valid;
                validator.stopRequest(element, valid);
            }
        }, param));
        return "pending";
    });

    // Re-enable unobtrusive validation for the entire form
    $.validator.unobtrusive.parse('form');
});


$(document).ready(function () {
    $.validator.addMethod('requiredif', function (value, element, parameters) {
        var desiredvalue = parameters.desiredvalue;
        desiredvalue = (desiredvalue == null ? '' : desiredvalue).toString();
        var controlType = $("input[id$='" + parameters.dependentproperty + "']").attr("type");
        var actualvalue = {}
        if (controlType == "checkbox" || controlType == "radio") {
            var control = $("input[id$='" + parameters.dependentproperty + "']:checked");
            actualvalue = control.val();
        } else {
            actualvalue = $("#" + parameters.dependentproperty).val();
        }
        if ($.trim(desiredvalue).toLowerCase() === $.trim(actualvalue).toLocaleLowerCase()) {
            var isValid = $.validator.methods.required.call(this, value, element, parameters);
            return isValid;
        }
        return true;
    });
});

//override Unobtrusive Validation to work with Bootstrap

$.validator.setDefaults({
    highlight: function (element) {
        if ($(element).attr('type') === "file") {
            $(element).closest(".file-upload-input").addClass("is-invalid").removeClass("is-valid");
        } else {
            $(element).addClass("is-invalid").removeClass("is-valid");
        }
    },
    unhighlight: function (element) {
        if ($(element).attr('type') === "file") {
            $(element).closest(".file-upload-input").addClass("is-valid").removeClass("is-invalid");
        } else {
            $(element).addClass("is-valid").removeClass("is-invalid");
        }
    }
});

$.validator.unobtrusive.adapters.add('remote', function (options) {
    $(options.form).on('invalid-form.validate', function (event, validator) {
        if (validator.numberOfInvalids() > 0) {
            $(validator.invalidElements()).each(function () {
                const $element = $(this);

                $element.focus();
                $element.blur();
            });
        }
    });
});

$.validator.unobtrusive.adapters.add('requiredif', ['dependentproperty', 'desiredvalue'], function (options) {
    options.rules['requiredif'] = options.params;
    options.messages['requiredif'] = options.message;
});
