var ajaxHelper = function () {
    var urlpre = '';
    var that = {};

    var handleUnCoughtException = function (xhr, errorType, error) {
        //console.log(xhr);
        // commonHelper.showHideLoader(false);
    };

    //Usage of Ajax Get

    //var url = "/Masters/activeInactiveCampaign";
    //ajaxHelper.ajaxGet(url, null, null, function (response) {
    //    if (response == "1") {
    //        window.location.reload();
    //    } else if (!response.Status) {
    //        alert(response.Response);
    //    }
    //    else {
    //        alert("Something went wrong");
    //    }
    //}, null);

    that.ajaxGet = function (url, dataType, data, callback, errorcallback) {
        commonHelper.showHideLoader(true);
        $.ajax({
            url: urlpre + url,
            type: 'GET',
            dataType: dataType,
            data: data,
            success: function (data) {
                commonHelper.showHideLoader(false);
                callback(data);
            },
            error: function (xhr, errorType, error) {
                commonHelper.showHideLoader(false);
                if (errorcallback != null && errorcallback != undefined) {
                    errorcallback(xhr, errorType, error);
                }
                else {
                    handleUnCoughtException(xhr, errorType, error);
                }
            },
            cache: false
        })
    };


    //Usage of Ajax Post

    //var url = "/Masters/activeInactiveCampaign";
    //var data = { id: id };
    //ajaxHelper.ajaxPost(url, null, data, function (response) {
    //    if (response == "1") {
    //        window.location.reload();
    //    } else if (!response.Status) {
    //        alert(response.Response);
    //    }
    //    else {
    //        alert("Something went wrong");
    //    }
    //}, null);

    that.ajaxPost = function (url, dataType, data, callback, errorcallback) {
        var request = { url: urlpre + url, type: "POST" };
        if (dataType != null || dataType != undefined) {
            request.dataType = dataType;
        }

        request.data = data;

        request.success = function (data) {
            callback(data);
        };

        request.error = function (xhr, errorType, error) {
            if (errorcallback != null && errorcallback != undefined) {
                errorcallback(xhr, errorType, error);
            }
            else {
                handleUnCoughtException(xhr, errorType, error);
            }
        };
        if (data instanceof FormData) {
            request.contentType = false;
            request.processData = false;
        }
        request.cache = false;
        $.ajax(request);
    };

    return that;

}();