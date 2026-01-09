using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyLessons.ConverterSQLClass;
using MyLessons.Models;
using System.Diagnostics;
namespace MyLessons.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext context;
		DbSet<user> UsersTable;
		public HomeController(ILogger<HomeController> logger, ApplicationDbContext _context)
        {
            _logger = logger;
            context = _context;
			UsersTable = _context.user;
		}
		public IActionResult Index()
		{
			HttpContext.Session.Clear();
			return View();
		}
		[HttpGet]
		public IActionResult Table(user model)
		{
			if (ModelState.IsValid)
			{
				foreach(var user in UsersTable)
				{
					if(user.login == model.login && user.password == model.password)
					{
						HttpContext.Session.SetInt32("id", user.id);
						return RedirectToAction("Table","Main");
					}
				}
			}
			return View("Index");
		}




		[HttpGet]
		public IActionResult AddLessonThroughPanel(user us, string selectedSubject, string clas, string room, string num, string day)
		{
			var obj = context.data.Find(us.id);

			if (string.IsNullOrEmpty(selectedSubject) || string.IsNullOrEmpty(clas) || string.IsNullOrEmpty(room) || string.IsNullOrEmpty(num) || string.IsNullOrEmpty(day))
			{
				HttpContext.Session.SetString("message", "введите необходимые параметры");
				return RedirectToAction("MainPanel", us);
			}
			lesson NewLesson = new lesson(day, num, selectedSubject, clas, room);
			string NewLessonStr = ControllerConvert.ConvertLessonToString(NewLesson);
			string BD;
			ViewBag.res = NewLessonStr;
			BD = obj.text += "|" + NewLessonStr;
			BD = ControllerConvert.CleanStringForBase(BD);
			obj.text = BD;
			context.SaveChanges();
			try
			{
				ViewBag.objec = ControllerConvert.SelectTeachersItem(obj.teacher);
				ViewBag.AvaliableItemsTeachAndObjec = ControllerConvert.GetListTeacherWithObjec(obj.teacher);
			}
			catch { }
			ViewBag.teacher = ControllerConvert.SelectTeachersName(obj.teacher);
			ViewBag.user = us;
			ViewBag.lesson = ControllerConvert.ConvertToLesson(obj.text);

			return RedirectToAction("MainPanel", us);
		}

		
        [HttpGet]
		public IActionResult DeleteLesson(user us, string less,string day,string number, string teacher,string clas,string room)
		{
			var obj = context.data.Find(us.id);
			lesson les = new lesson(day, number, less,teacher, clas,room);
			string data = ControllerConvert.ConvertLessonToString(les);
			obj.text = obj.text.Replace(data, "");
			obj.text = ControllerConvert.CleanStringForBase(obj.text);
			context.SaveChanges();
			return RedirectToAction("MainPanel", us);
		}
		public IActionResult AddUser(string log, string pass)
		{
			if (string.IsNullOrEmpty(log) && string.IsNullOrEmpty(pass))
			{
				ViewBag.MessageLog = "¬ведите Ћогин";
				ViewBag.MessagePass = "¬ведите ѕароль";
				return View("Index");
			}
			else if (string.IsNullOrEmpty(log))
			{
				ViewBag.MessageLog = "¬ведите Ћогин";
				return View("Index");
			}
			else if (string.IsNullOrEmpty(pass))
			{
				ViewBag.MessagePass = "¬ведите ѕароль";
				return View("Index");
			}
			else
			{
				ViewBag.MessageLog = "";
				ViewBag.MessagePass = "";
			}
			user new_user = new user { password = pass, login = log };
			foreach(var user in UsersTable)
			{
				if(user.login == log)
				{
					ViewBag.msg = "Ётот пользователь уже существует";
					return View("Index");
				}
			}
			context.user.Add(new_user);
			context.data.Add(new Data(context.user.Count() + 1));
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

