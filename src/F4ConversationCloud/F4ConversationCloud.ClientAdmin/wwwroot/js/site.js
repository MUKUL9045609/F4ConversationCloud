/*var swiper = new Swiper(".useCasesSwiper", {
    spaceBetween: 30,
    pagination: {
        el: ".swiper-pagination",
        clickable: true
    },
    autoplay: {
        delay: 3000,
        disableOnInteraction: false,
    },
    loop: true
});*/
$('nav ul ul').hide();
$('nav > ul > li > a').on('click', function (e) {
    e.stopPropagation();
    $('nav ul ul').slideUp();
    $(this).next().is(":visible") || $(this).next().slideDown();
});

$('li').has('ul').find('a').not('ul ul a').addClass('caret');
$(".toggle-password").click(function () {
    $(this).toggleClass("fa-eye fa-eye-slash");
    input = $(this).parent().find("input");
    if (input.attr("type") == "password") {
        input.attr("type", "text");
    } else {
        input.attr("type", "password");
    }
});