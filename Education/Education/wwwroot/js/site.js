// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    // show when page load
    //toastr.info('Toastr Page Loaded!',
    //    toastr.options = {
    //        "positionClass": "toast-top-center"
    //    });

    $('#justabutton').click(function () {
    // show when the button is clicked
    toastr.success('Clicked the Just a Button',
        'Clicked',
        toastr.options = {
            "positionClass": "toast-top-right",
            "fadeIn": 300,
            "fadeOut": 100,
            "timeOut": 1000,   // 5000 is default
            "extendedTimeOut": 1000
        })

    });
});
