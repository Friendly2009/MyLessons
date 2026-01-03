using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyLessons;
using MyLessons.ConverterSQLClass;
using MyLessons.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MyLessons.Views.Shared.Controllers
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
			ViewBag.data = obj.text;
			ViewBag.socials = ControllerConvert.FindAllClass(obj.text);
			ViewBag.teachers = ControllerConvert.SelectTeachersName(obj.teacher);
			ViewBag.objects = ControllerConvert.SelectTeachersItem(obj.teacher);
            if(string.IsNullOrEmpty(obj.text) || string.IsNullOrEmpty(obj.teacher))
            {
                return RedirectToAction("MainPanel");
            }
			return View();
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
