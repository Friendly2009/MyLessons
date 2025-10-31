var Buttons_Star = document.querySelectorAll('.mx-1');
var reader = document.querySelectorAll('td');
for (var i = 0; i < Buttons_Star.length; i++) {
    Buttons_Star[i].setAttribute("num", (i + 1).toString());
}
Buttons_Star.forEach(function (element) {
    element.addEventListener('click', function () {
        var count = element.getAttribute("num");
        for (var i = 0; i < Buttons_Star.length; i++) {
            Buttons_Star[i].setAttribute('src', "/css/star.png");
        }
        for (var i = 0; i < Number(count); i++) {
            Buttons_Star[i].setAttribute('src', "/css/start.png");
        }
        //    price.setAttribute('value', count);
    });
});
reader.forEach(function (td) {
    td.addEventListener("click", function (clickable) {
        SelectClick(td);
    });
});
function SelectClick(td) {
    for (var i = 0; i < reader.length; i++) {
        reader[i].style.color = "black";
        reader[i].style.background = "none";
    }
    td.style.background = "#90EE90";
    td.style.color = "#2e8b57";
}
//# sourceMappingURL=file.js.map