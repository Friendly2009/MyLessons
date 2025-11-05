using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using MyLessons.ConverterSQLClass;
using MyLessons.Models;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Linq;
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
		public IActionResult Table(user us)
		{
			if (!ModelState.IsValid)
			{
				return View("Index", us);
			}
			var thisUs = context.user.Where(t => t.login == us.login).ToList();
			for(int i = 0; i < thisUs.Count; i++)
			{
				if (thisUs[i].login == us.login && thisUs[i].password == us.password)
				{
					us = thisUs[i];
					ViewBag.res = $"{thisUs[i].id}";
					try
					{
						var obj = context.data.FirstOrDefault(t => t.Id == us.id);
						ViewBag.data = obj.text;
						ViewBag.availableItems = new List<string>() { "Математика", "Русский язык" };
						ViewBag.socials = ControllerConvert.FindAllClass(obj.text);
						ViewBag.teachers = ControllerConvert.SelectTeachersName(obj.teacher);
						ViewBag.objects = ControllerConvert.SelectTeachersItem(obj.teacher);
						if(obj.text == "")
						{
							return RedirectToAction("MainPanel", us);
						}
						return View(us);
					}
					catch
					{
						return RedirectToAction("MainPanel", us);
					}				
				}
			}
			return View("Index");
		}
		[HttpGet]
		public IActionResult Choose(user us,string clas)
		{
			var thisUs = context.user.Where(t => t.login == us.login).ToList();
			for (int i = 0; i < thisUs.Count; i++)
			{
				if (thisUs[i].login == us.login && thisUs[i].password == us.password) 
				{ 
					us = thisUs[i];
					var obj = context.data.FirstOrDefault(t => t.Id == us.id);

					ViewBag.data = obj.text;
					ViewBag.availableItems = new List<string>() { "Математика", "Русский язык" };
					ViewBag.socials = ControllerConvert.FindAllClass(obj.text);
					ViewBag.teachers = ControllerConvert.SelectTeachersName(obj.teacher);
					ViewBag.objects = ControllerConvert.SelectTeachersItem(obj.teacher);
					ViewBag.Lessons = ControllerConvert.ConvertToLesson(obj.text);
					ViewBag.Clas = clas;
					return View("Table", us);
				}
			}
			return View("Index",us);
		}
		[HttpGet]
		public IActionResult MainPanel(user us)
		{
			var thisUs = context.user.Where(t => t.login == us.login).ToList();
			for (int i = 0; i < thisUs.Count; i++)
			{
				if (thisUs[i].login == us.login && thisUs[i].password == us.password)
				{
					us = thisUs[i];
					var obj = context.data.Find(us.id);
					try { ViewBag.objec = ControllerConvert.SelectTeachersItem(context.data.Find(us.id).teacher);}catch { }
					ViewBag.teacher = ControllerConvert.SelectTeachersName(obj.teacher);
					ViewBag.availableItems = new List<string>() { "Математика", "Русский язык" };
					ViewBag.user = us;
					ViewBag.lesson = ControllerConvert.ConvertToLesson(obj.text);
				}
			}
			ViewBag.r = HttpContext.Session.GetString("a");
			return View(us);
		}
		[HttpGet]
		public IActionResult AddTeacher(user us,string name, string selectedSubject)
		{
			var obj = context.data.Find(us.id);
			if(!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(selectedSubject))
			{
				string res = context.data.Find(us.id).teacher + "|" + name + "`" + selectedSubject;
				res = res.Trim('|');
				context.data.Find(us.id).teacher = res;
				context.SaveChanges();
			}
			try
			{
				ViewBag.objec = ControllerConvert.SelectTeachersItem(context.data.Find(us.id).teacher);
			}
			catch { }
			ViewBag.teacher = ControllerConvert.SelectTeachersName(context.data.Find(us.id).teacher);
			ViewBag.availableItems = new List<string>() { "Математика", "Русский язык" };
			ViewBag.user = us;
			return RedirectToAction("MainPanel",us);
		}
		[HttpGet]
		public IActionResult DeleteTeacher(user us, string name, string objec, string adres)
		{
			List<string> teach = ControllerConvert.SelectTeachersName(context.data.Find(us.id).teacher);
			List<string> objects = ControllerConvert.SelectTeachersItem(context.data.Find(us.id).teacher);
			teach.Remove(name);
			objects.Remove(objec);
			string result = ControllerConvert.ConvertTeachersToString(teach,objects);
			context.data.Find(us.id).teacher = result;
			context.SaveChanges();
			return RedirectToAction(adres, us);
		}
		[HttpPost]
		public IActionResult AddAccount(string log, string pass)
		{
			if (string.IsNullOrEmpty(log) && string.IsNullOrEmpty(pass))
			{
				ViewBag.MessageLog = "Введите Логин";
				ViewBag.MessagePass = "Введите Пароль";
				return View("Index");
			}
			else if (string.IsNullOrEmpty(log))
			{
				ViewBag.MessageLog = "Введите Логин";
				return View("Index");
			}
			else if (string.IsNullOrEmpty(pass))
			{
				ViewBag.MessagePass = "Введите Пароль";
				return View("Index");
			}
			else
			{
				ViewBag.MessageLog = "";
				ViewBag.MessagePass = "";
			}
			user us = new user { password = pass, login = log };
			context.user.Add(us);
			context.SaveChanges();
			var thisUs = context.user.Where(t => t.login == us.login).ToList();
			foreach(var User in thisUs)
			{
				if (User.login == us.login && User.password == us.password)
				{
					us = User;
				}
			}
			context.data.Add(new Data(us.id));
			context.SaveChanges();
			return View("Index");
		}
		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
