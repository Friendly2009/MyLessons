var Buttons_Star = document.querySelectorAll('.mx-1');
for (let i = 0; i < Buttons_Star.length; i++) {
    Buttons_Star[i].setAttribute("num", (i + 1).toString());
}
Buttons_Star.forEach(function (element) {
    element.addEventListener('click', function () {
        let count = Number(element.getAttribute("num"));
        for (let i = 0; i < Buttons_Star.length; i++) {
            Buttons_Star[i].setAttribute('src', "/css/star.png");
        }
        for (let i = 0; i < count; i++) {
            Buttons_Star[i].setAttribute('src', "/css/start.png");
        }
/*        price.setAttribute('value', count);*/
    });
});