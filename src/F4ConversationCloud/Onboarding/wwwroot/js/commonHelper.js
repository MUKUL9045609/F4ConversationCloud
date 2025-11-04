var commonHelper = function () {
    var that = {};
    that.ToasterSeconds = 3;

    var loaderHtml = '<div id="custom-loader" class="position-fixed top-0 h-100 w-100 d-flex align-items-center justify-content-center bg-dark bg-opacity-50 overflow-hidden" style="z-index:1060"><div class="spinner-border text-primary" role="status"><span class="visually-hidden">Loading...</span></div></div>';

    //usage commonHelper.showSuccess('Success Message');
    that.showHideLoader = function (toBeShown) {
        if (toBeShown) {
            if ($('body').find('#custom-loader').length == 0) {
                $('body').append(loaderHtml);

                $('body').addClass('overflow-hidden');
            }
        }
        else {
            if ($('body').find('#custom-loader').length > 0) {
                $('body').find('#custom-loader').each(function () {
                    $(this).remove();
                });

                $('body').removeClass('overflow-hidden');
            }
        }
    }
    that.showSuccess = function (Message) {
       
        var id = that.generateGUID();
        var html = that.getToasterHTML();
        html = html.replace('{class}', 'toast--green'); // Keep Toastify green class
        html = html.replace('{Id}', id);
        html = html.replace('{message}', Message);

        $('.toastWrap').append(html);

        // Auto-hide after delay
        setTimeout(function () {
            $('#' + id).closest('.toastify').fadeOut(400, function () {
                $(this).remove();
            });
        }, that.ToasterSeconds * 1000);
    };

    that.showError = function (Message) {
        var id = that.generateGUID();
        var html = that.getToasterHTML();
        html = html.replace('{class}', 'toast--danger');
        html = html.replace('{Id}', id);
        html = html.replace('{message}', Message);

        $('.toastWrap').append(html);

        setTimeout(function () {
            $('#' + id).closest('.toastify').fadeOut(400, function () {
                $(this).remove();
            });
        }, that.ToasterSeconds * 1000);
    };

    that.showInfo = function (Message) {
        var id = that.generateGUID();
        var html = that.getToasterHTML();
        html = html.replace('{class}', 'toast--blue');
        html = html.replace('{Id}', id);
        html = html.replace('{message}', Message);

        $('.toastWrap').append(html);

        setTimeout(function () {
            $('#' + id).closest('.toastify').fadeOut(400, function () {
                $(this).remove();
            });
        }, that.ToasterSeconds * 1000);
    };

    that.showWarning = function (Message) {
        var id = that.generateGUID();
        var html = that.getToasterHTML();
        html = html.replace('{class}', 'toast--yellow');
        html = html.replace('{Id}', id);
        html = html.replace('{message}', Message);

        $('.toastWrap').append(html);

        setTimeout(function () {
            $('#' + id).closest('.toastify').fadeOut(400, function () {
                $(this).remove();
            });
        }, that.ToasterSeconds * 1000);
    };

    that.getToasterHTML = function () {
        var html = '<div class="toastify on toastify-right toastify-top" aria-live="polite">';
        html += '<div class="toast {class}" id="{Id}">';
        html += '<div class="toast__icon"></div>';
        html += '<div class="toast__content"><p class="toast__message">{message}</p></div>';
        html += '</div>';
        html += '<button type="button" aria-label="Close" class="toast-close">✖</button>';
        html += '</div>';

       
        return html;
    };

    that.generateGUID = function () {
        // Create a GUID using a combination of numbers and letters
        function s4() {
            return Math.floor((1 + Math.random()) * 0x10000).toString(16).substring(1);
        }
        return s4() + s4() + '-' + s4() + '-' + s4() + '-' + s4() + '-' + s4() + s4() + s4();
    }

    that.setTimeOut = function (timeOutSec) {
        return new Promise(resolve => {
            setTimeout(resolve, timeOutSec * 1000);
        });
    }

    that.handleFileUpload = function (fileInputId, fileLinkContainerId, validationFieldName, fileNameInput) {
        const fileInput = document.getElementById(fileInputId);
        const file = fileInput.files[0];
        const fileLinkContainer = document.getElementById(fileLinkContainerId);
        const validationMessage = document.querySelector(`[data-valmsg-for="${validationFieldName}"]`);

        // Clear old link
        fileLinkContainer.innerHTML = '';

        if (file) {
            const tempUrl = URL.createObjectURL(file);
            const link = document.createElement('a');
            link.href = tempUrl;
            link.target = '_blank';
            link.textContent = file.name;

            fileLinkContainer.appendChild(link);

            // Remove validation error
            fileInput.classList.remove('is-invalid');
            fileInput.classList.add('is-valid');

            if (validationMessage) {
                validationMessage.textContent = '';
            }

            document.getElementById(fileNameInput).value = file.name;
        }
    }

    that.setMinAndMaxDate = function (fromDateId, toDateId) {
        // Set min date to today
        var dtToday = new Date();
        var month = dtToday.getMonth() + 1;
        var day = dtToday.getDate();
        var year = dtToday.getFullYear();

        if (month < 10) month = '0' + month;
        if (day < 10) day = '0' + day;

        var maxDate = year + '-' + month + '-' + day;

        $('#' + fromDateId).attr('min', maxDate);
        $('#' + toDateId).attr('min', maxDate);
    }

    return that;
}();