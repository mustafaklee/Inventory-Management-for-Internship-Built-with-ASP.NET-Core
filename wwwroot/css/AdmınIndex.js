$(document).ready(function () {
    $('.menu-link').click(function (e) {
        e.preventDefault();
        $(this).parent().toggleClass('active');
    });
});