using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyLessons.ConverterSQLClass;
using MyLessons.Models;
using System;
using System.Collections.Generic;
using System.Data;
namespace MyLessons.Controllers
{
    public class MainController : Controller
    {
        private readonly ApplicationDbContext _context;
        private DbSet<Data> DataTable;

        public MainController(ApplicationDbContext context)
        {
            _context = context;
			DataTable = _context.data;
        }
        public IActionResult Table()
        {
			Data obj = DataTable.Find(HttpContext.Session.GetInt32("id"));
			if( obj == null) { return RedirectToAction("Index", "Home"); }
			ViewBag.data = obj.text;
			ViewBag.socials = ControllerConvert.FindAllClass(obj.text);
			ViewBag.teachers = ControllerConvert.SelectTeachersName(obj.teacher);
			ViewBag.objects = ControllerConvert.SelectTeachersItem(obj.teacher);
			if (string.IsNullOrEmpty(obj.text) || string.IsNullOrEmpty(obj.teacher))
			{
				return RedirectToAction("MainPanel");
			}
			return View();
        }
        public IActionResult MainPanel()
        {
            int id = Convert.ToInt32(HttpContext.Session.GetInt32("id"));
            Data obj = DataTable.Find(id);
			if (obj == null) { return RedirectToAction("Index", "Home"); }
			ViewBag.objec = ControllerConvert.SelectTeachersItem(obj.teacher);
			ViewBag.AvaliableItemsTeachAndObjec = ControllerConvert.GetListTeacherWithObjec(obj.teacher);
			ViewBag.teacher = ControllerConvert.SelectTeachersName(obj.teacher);
			ViewBag.user = _context.user.Find(id);
			ViewBag.lesson = ControllerConvert.ConvertToLesson(obj.text);
			return View();
        }
		public IActionResult AddTeacher(string name, string teacher)
		{
            int id = Convert.ToInt32(HttpContext.Session.GetInt32("id"));
            Data obj = DataTable.Find(id);
			obj.teacher = ControllerConvert.TeachersParametrToBase(obj.teacher, name, teacher);
			_context.SaveChanges();
            return RedirectToAction("MainPanel");
		}
		public IActionResult DeleteTeacher(string subject, string name, string adres)
		{
            int id = Convert.ToInt32(HttpContext.Session.GetInt32("id"));
            var obj = DataTable.Find(id);
            List<string> teach = ControllerConvert.SelectTeachersName(obj.teacher);
            List<string> objects = ControllerConvert.SelectTeachersItem(obj.teacher);
            teach.Remove(name);
            objects.Remove(subject);
            string resultTeach = ControllerConvert.ConvertTeachersToString(teach, objects);
            obj.teacher = resultTeach;
            _context.SaveChanges();
            List<lesson> ChekListLesson = ControllerConvert.ConvertToLesson(obj.text);
            List<lesson> resultLess = new List<lesson>();
            foreach (var item in ChekListLesson)
            {
                if (item == null)
                {
                    return RedirectToAction("MainPanel");
                }
                if (item.teacher == name && item.less == subject)
                {
                    resultLess.Add(item);
                }
            }
            obj.text = ControllerConvert.ConvertLessonsArrayToString(resultLess);
            _context.SaveChanges();
            return RedirectToAction(adres);
        }
        public IActionResult AddLesson(string selectedSubject, string clas, string room, string num, string day)
        {
            int id = Convert.ToInt32(HttpContext.Session.GetInt32("id"));
            var obj = DataTable.Find(id);
            if (string.IsNullOrEmpty(selectedSubject) || string.IsNullOrEmpty(clas) || string.IsNullOrEmpty(room) || string.IsNullOrEmpty(num) || string.IsNullOrEmpty(day))
            {
                return RedirectToAction("MainPanel");
            }
            lesson NewLesson = new lesson(day, num, selectedSubject, clas, room);
            string NewLessonStr = ControllerConvert.ConvertLessonToString(NewLesson);
            string BD = obj.text += "|" + NewLessonStr;
            BD = ControllerConvert.CleanStringForBase(BD);
            obj.text = BD;
            _context.SaveChanges();
            return RedirectToAction("MainPanel");
        }
        public IActionResult Choose(string clas)
        {
			Data obj = DataTable.Find(HttpContext.Session.GetInt32("id"));
			ViewBag.data = obj.text;
			ViewBag.socials = ControllerConvert.FindAllClass(obj.text);
			ViewBag.teachers = ControllerConvert.SelectTeachersName(obj.teacher);
			ViewBag.objects = ControllerConvert.SelectTeachersItem(obj.teacher);
			ViewBag.Lessons = ControllerConvert.ConvertToLesson(obj.text);
			ViewBag.Clas = clas;
            return View("Table");
		}
        public async Task<IActionResult> SaveChanges(string data, string clas)
        {
            data = ControllerConvert.CleanStringForBase(data);
            DataTable.Find(HttpContext.Session.GetInt32("id")).text = data;
			await _context.SaveChangesAsync();
            return Choose(clas);
		}
    }
}
