$(function () {

    if (typeof deleteSuccessMessage !== "undefined" && deleteSuccessMessage) {

        toastr.options = {
            closeButton: true,
            progressBar: true,
            positionClass: "toast-top-right",
            timeOut: 3000,
            extendedTimeOut: 1000,
            showDuration: 500,
            hideDuration: 400,
            showMethod: "fadeIn",
            hideMethod: "fadeOut",
            newestOnTop: true,
            preventDuplicates: true
        };

        toastr.success(deleteSuccessMessage);
    }

});
