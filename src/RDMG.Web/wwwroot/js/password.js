(function ($) {
    $(document).on("click", ".input-group-password a", function (event) {
        event.preventDefault();

        var wrapper = $(this).closest(".input-group-password");
        var input = $("input", wrapper);
        var i = $("i", wrapper);

        if (input.attr("type") === "text") {
            input.attr("type", "password");
            i.addClass("fa-eye-slash");
            i.removeClass("fa-eye");
        }
        else if (input.attr("type") === "password") {
            input.attr("type", "text");
            i.removeClass("fa-eye-slash");
            i.addClass("fa-eye");
        }
    });
}(jQuery));