using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using MyLessons.ConverterSQLClass;
using MyLessons.Models;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using static System.Net.Mime.MediaTypeNames;
namespace MyLessons.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext context;
		private readonly string count = "aldhkvf";
		public HomeController(ILogger<HomeController> logger, ApplicationDbContext _context)
        {
            _logger = logger;
            context = _context;
        }
		public IActionResult Index()
		{
			return View();
		}
		[HttpGet]
		public IActionResult Privacy(user us)
		{
			if (!ModelState.IsValid)
			{
				return View("Index", us);
			}
			
			ViewBag.res = $"{us.id}";
			var obj = context.data.FirstOrDefault(t => t.Id == us.id + 1);

			ViewBag.data = obj.text;
			ViewBag.availableItems = new List<string>() { "����������", "������� ����" };
			ViewBag.socials = ControllerConvert.FindAllClass(obj.text);
			ViewBag.teachers = ControllerConvert.SelectTeachers(obj.teacher);
			ViewBag.objects = ControllerConvert.SelectObjects(obj.teacher);

			return View(us);
		}
		[HttpGet]
		public IActionResult Choose(user us,string clas)
		{			
			var obj = context.data.FirstOrDefault(t => t.Id == us.id + 1);

			ViewBag.data = obj.text;
			ViewBag.availableItems = new List<string>() { "����������","������� ����"};
			ViewBag.socials = ControllerConvert.FindAllClass(obj.text);
			ViewBag.teachers = ControllerConvert.SelectTeachers(obj.teacher);
			ViewBag.objects = ControllerConvert.SelectObjects(obj.teacher);
			ViewBag.Lessons = ControllerConvert.ConvertToData(obj.text);
			ViewBag.Clas = clas;
			ViewBag.res = $"{us.id} {obj.Id}";
			return View("Privacy", us);
		}
		[HttpPost]
		public IActionResult AddAccount(newuser newusers, string Newlogin, string Newpassword)
		{
			HttpContext.Session.SetInt32(count, 1);
			if (ModelState.IsValid)
			{
				string name = newusers.NewLogin;
				string pas = newusers.NewPassword;
				user us = new user { login = name, password = pas };
				context.user.Add(us);
				context.SaveChanges();
			}
			if (Newlogin == null && HttpContext.Session.GetInt32(count) == 1)
			{
				ViewBag.n = "������� �����";
			}
			if (Newpassword == null && HttpContext.Session.GetInt32(count) == 1)
			{
				ViewBag.num = "������� ������";
			}
			return RedirectToAction("Index");
		}
		public IActionResult DeleteTeacher(user us, string name, string objec)
		{
			string data = context.data.FirstOrDefault(t => t.Id == us.id + 1).teacher;
            var teach = ControllerConvert.SelectTeachers(data);
			var objects = ControllerConvert.SelectObjects(data);
			teach.Remove(name);
			objects.Remove(objec);
			string result = ControllerConvert.ConvertToTeachers(teach, objects);
			context.data.FirstOrDefault(t => t.Id == us.id + 1).teacher = result;
			context.SaveChanges();
            return RedirectToAction("Privacy", us);
		}
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
