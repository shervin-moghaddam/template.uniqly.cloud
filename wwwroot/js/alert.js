$(document).ready(function () {
    $(".close-alert").click(function () {
        $(this).parent().parent().removeClass("show");
    });

    $(".close-popup").click(function () {
        $(this).parent().parent().parent().removeClass("show");
    });
});

function alerts() {
    $('#dialogs button').each(function() {
        $(this).on('click', function () {
            const el = this;
            const design = el.getAttribute('design');
            const id = el.getAttribute('id');
            if (el.hasAttribute('dialog')) {
            };
        });
    });
}