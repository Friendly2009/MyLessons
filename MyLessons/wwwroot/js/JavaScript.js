class NewLesson {
    constructor(day, number, less, name, room, clas) {
        this.day = day;
        this.number = number;
        this.less = less;
        this.name = name;
        this.clas = clas;
        this.room = room;
    }
}
function NewLessToJson(NewLessonArgument) {
    return JSON.stringify(NewLessonArgument);
}

var Buttons_Star = document.querySelectorAll('.mx-1');
var AddLesson_Button = document.querySelectorAll('#AddLesson');
var reader = document.querySelectorAll('td');
let Tracking_for_Add_Lesson = false;
var MainJson = document.getElementById('data');
var NewLess = "";
var NewTeach = "";
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
AddLesson_Button.forEach(button => {
    button.addEventListener("click", clickable => {
        Tracking_for_Add_Lesson = true;
        var ParentTeacher = button.parentNode;
        Newteach = ParentTeacher.querySelector("#IdTeachersForNewLesson").textContent;
        NewLess = ParentTeacher.querySelector("#IdObjectForNewLesson").textContent;
    })
});

function AddLessonInBase(td) {
    Tracking_for_Add_Lesson = false;
    var Newday = td.getAttribute("day");
    var Newnumber = td.getAttribute("lesson");
    var Newroom = document.getElementById("NumberRoomInput").value;
    var Newclas = document.getElementById("CurrentClass").textContent;
    const NewLessonForAdd = new NewLesson(Newday, Newnumber, NewLess, Newteach, Newroom, Newclas);
    let JsonFormatNewLesson = NewLessToJson(NewLessonForAdd);
    alert(Newday);
    alert(JsonFormatNewLesson);
}

function SelectClick(td) {
    for (let i = 0; i < reader.length; i++) {
        reader[i].style.color = "black";
        reader[i].style.background = "none";
    }
    td.style.background = "#90EE90";
    td.style.color = "#2e8b57";

   
}
