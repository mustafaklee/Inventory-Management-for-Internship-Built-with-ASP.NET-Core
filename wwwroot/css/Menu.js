$(document).ready(function () {
    // Jquery for toggle sub menus
    $('.sub-btn').click(function () {
        // Tüm sub-menülerde açık olanları kapat
        $('.sub-menu').not($(this).next('.sub-menu')).slideUp();
        $('.sub-btn').not(this).find('.dropdown').removeClass('rotate');

        // Şu an tıklanan sub-menüyü aç/kapat
        $(this).next('.sub-menu').slideToggle();
        $(this).find('.dropdown').toggleClass('rotate');
    });

    // Jquery for expand and collapse the sidebar
    $('.menu-btn').click(function () {
        $('.side-bar').addClass('active');
        $('.menu-btn').css("visibility", "hidden");
    });
});