window.onscroll = function () { scrollFunction(); };

var scrollFunction = function () {
    if (document.body.scrollTop > 120 || document.documentElement.scrollTop > 120) {
        $(".toparrow").show();
    } else {
        $(".toparrow").hide();
    }
};

var topFunction = function () {
    document.body.scrollTop = 0; // For Safari
    document.documentElement.scrollTop = 0; // For Chrome, Firefox, IE and Opera
};

var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
    return new bootstrap.Tooltip(tooltipTriggerEl);
});