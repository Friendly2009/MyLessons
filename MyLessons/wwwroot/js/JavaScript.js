class NewLesson {
    constructor(day, number, less, name, room, clas) {
        this.day = day;
        this.number = number;
        this.less = less;
        this.teacher = name;
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
var deleteLessonButton = document.getElementById("deleteLessonButton");
let Tracking_for_Add_Lesson = false;
let Tracking_for_Delete_Lesson = false;
var MainJson = document.getElementById('data').value;
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
        if (Tracking_for_Delete_Lesson === true) {
            DeleteLesson(td);
        }
    })
});
AddLesson_Button.forEach(button => {
    button.addEventListener("click", clickable => {
        Tracking_for_Add_Lesson = true;
        Tracking_for_Delete_Lesson = false;
        var ParentTeacher = button.parentNode;
        Newteach = ParentTeacher.querySelector("#IdTeachersForNewLesson").textContent;
        NewLess = ParentTeacher.querySelector("#IdObjectForNewLesson").textContent;
        //visual changes table
        reader.forEach(td => {
            td.style.background = "#EE0000";
            td.style.color = "#000000";
            try {
                var check_Less = td.querySelector("#less").getAttribute("value");
            } catch{
                td.style.background = "#90EE90";
                td.style.color = "#2e8b57";
            }
        })
    })
});
deleteLessonButton.addEventListener("click", clickable => {
    Tracking_for_Delete_Lesson = true;
    Tracking_for_Add_Lesson = false;
});

function AddLessonInBase(td) {
    Tracking_for_Add_Lesson = false;
    try {
        var Replaceroom = td.querySelector("#paragraph_Room").textContent;
        var paragraph_Less = td.querySelector("#paragraph_Less").textContent;
        var paragraph_Teacher = td.querySelector("#paragraph_Teacher").textContent;

    } catch {
        var Newday = td.getAttribute("day");
        var Newnumber = td.getAttribute("lesson");
        var Newroom = document.getElementById("NumberRoomInput").value;
        var Newclas = document.getElementById("CurrentClass").textContent;
        const NewLessonForAdd = new NewLesson(Newday, Newnumber, NewLess, Newteach, Newroom, Newclas);
        let JsonFormatNewLesson = NewLessToJson(NewLessonForAdd);
        MainJson = MainJson + "|" + JsonFormatNewLesson;
        //select the new lesson in table
        var less_paragraph = document.createElement("p");
        less_paragraph.textContent = NewLess;
        less_paragraph.setAttribute("class", "m-1");
        less_paragraph.setAttribute("id", "paragraph_Less");
        var room_paragraph = document.createElement("p");
        room_paragraph.textContent = Newroom;
        room_paragraph.setAttribute("class", "m-1");
        room_paragraph.setAttribute("id", "paragraph_Room");
        var teach_paragraph = document.createElement("p");
        teach_paragraph.textContent = Newteach;
        teach_paragraph.setAttribute("class", "m-1");
        teach_paragraph.setAttribute("id", "paragraph_Teacher");
        td.appendChild(less_paragraph);
        td.appendChild(room_paragraph);
        td.appendChild(teach_paragraph);
        document.getElementById("data").setAttribute("value", MainJson);
    }
}
function DeleteLesson(td) {
    Tracking_for_Delete_Lesson = false;
    var Oldday = td.getAttribute("day");
    var Oldnumber = td.getAttribute("lesson");
    var Oldroom = td.querySelector("#paragraph_Room");
    var OldTeacher = td.querySelector("#paragraph_Teacher");
    var OldLesson = td.querySelector("#paragraph_Less");
    var OldClass = document.getElementById("CurrentClass").textContent;
    const OldLessonForDel = new NewLesson(Oldday, Oldnumber, OldLesson.textContent, OldTeacher.textContent, Oldroom.textContent, OldClass);
    let JsonFormatOldLesson = NewLessToJson(OldLessonForDel);
    MainJson = MainJson.replace(JsonFormatOldLesson, "");
    document.getElementById("data").setAttribute("value", MainJson);
    RemoveObjectTd(td);
}

function SelectClick(td) {
    reader.forEach(ReaderObject => {
        ReaderObject.style.color = "black";
        ReaderObject.style.background = "none";
    })
    td.style.background = "#90EE90";
    td.style.color = "#2e8b57";
}

function RemoveObjectTd(td) {
    td.querySelector("#paragraph_Room").remove();
    td.querySelector("#paragraph_Teacher").remove();
    td.querySelector("#paragraph_Less").remove();
}
