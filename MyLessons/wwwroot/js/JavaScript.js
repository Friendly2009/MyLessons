var Buttons_Star = document.querySelectorAll('.mx-1');
var AddLesson_Button = document.getElementById('AddLesson');
var reader = document.querySelectorAll('td');
let Tracking_for_Add_Lesson = false;
for (let i = 0; i < Buttons_Star.length; i++) {
    Buttons_Star[i].setAttribute("num", (i + 1).toString());
}
Buttons_Star.forEach(function (element) {
    element.addEventListener('click', function () {
        let count = element.getAttribute("num");
        for (let i = 0; i < Buttons_Star.length; i++) {
            Buttons_Star[i].setAttribute('src', "/css/star.png");
        }
        for (let i = 0; i < Number(count); i++) {
            Buttons_Star[i].setAttribute('src', "/css/start.png");
        }
        //    price.setAttribute('value', count);
    });
});
reader.forEach(td => {
    td.addEventListener("click", clickable => {
        SelectClick(td);
        if (Tracking_for_Add_Lesson === true) {
            AddLessonInBase(td);
        }
    })
});

AddLesson_Button.addEventListener("click", clicable => {
    Tracking_for_Add_Lesson = true;
});

function AddLessonInBase(td) {
    Tracking_for_Add_Lesson = false;
    var ItemForNewLesson = td.querySelectorAll('Input');
    let Newday = document.getElementsByName('Newday');
    let Newnumber = document.getElementsByName('Newnumber');
    let Newless = document.getElementsByName('Newless');
    let Newteacher = document.getElementsByName('Newteacher');
    let Newroom = document.getElementsByName('Newroom');
    let Newclas = document.getElementsByName('Newclas');

}

function SelectClick(td) {
    for (let i = 0; i < reader.length; i++) {
        reader[i].style.color = "black";
        reader[i].style.background = "none";
    }
    td.style.background = "#90EE90";
    td.style.color = "#2e8b57";
}