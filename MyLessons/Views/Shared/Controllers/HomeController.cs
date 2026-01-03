using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyLessons.ConverterSQLClass;
using MyLessons.Models;
using System.Diagnostics;
namespace MyLessons.Views.Shared.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext context;
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





		[HttpGet]
		public IActionResult AddTeacher(user us,string name, string selectedSubject)
		{
			var obj = context.data.Find(us.id);
			if(!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(selectedSubject))
			{
				string res = obj.teacher + "|" + name + "`" + selectedSubject;
				res = res.Trim('|');
				obj.teacher = res;
				context.SaveChanges();
			}
			else { HttpContext.Session.SetString("message", "¬ведите данные"); }
			try
			{
				ViewBag.objec = ControllerConvert.SelectTeachersItem(obj.teacher);
                ViewBag.AvaliableItemsTeachAndObjec = ControllerConvert.GetListTeacherWithObjec(obj.teacher);
            }
			catch { }
			ViewBag.teacher = ControllerConvert.SelectTeachersName(obj.teacher);
			ViewBag.user = us;
			return RedirectToAction("MainPanel",us);
		}
        [HttpGet]
		public IActionResult DeleteTeacher(user us, string name, string objec, string adres)
		{
			var obj = context.data.Find(us.id);
			List<string> teach = ControllerConvert.SelectTeachersName(obj.teacher);
			List<string> objects = ControllerConvert.SelectTeachersItem(obj.teacher);
			teach.Remove(name);
			objects.Remove(objec);
			string resultTeach = ControllerConvert.ConvertTeachersToString(teach,objects);
			obj.teacher = resultTeach;
			context.SaveChanges();
			List<lesson> ChekListLesson = ControllerConvert.ConvertToLesson(obj.text);
			List<lesson> resultLess = new List<lesson>();
			foreach(var item in ChekListLesson)
			{
				if (item == null)
				{
					return RedirectToAction("MainPanel", us);
				}
				if (item.teacher == name && item.less == objec)
				{
                    resultLess.Add(item);
                } 
			}
            obj.text = ControllerConvert.ConvertLessonsArrayToString(resultLess);
            context.SaveChanges();
			return RedirectToAction("MainPanel",us);
		}




		[HttpPost]
		public IActionResult AddAccount(string log, string pass)
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



		[HttpGet]
		public IActionResult MainPanel(user us)
		{
			if(!ModelState.IsValid)
			{
				return View("Index");
			}
			var thisUs = context.user.Where(t => t.login == us.login).ToList();
			for (int i = 0; i < thisUs.Count; i++)
			{
				if (thisUs[i].login == us.login && thisUs[i].password == us.password)
				{
					us = thisUs[i];
					var obj = context.data.FirstOrDefault(t => t.Id == us.id);
					try
					{
						ViewBag.objec = ControllerConvert.SelectTeachersItem(obj.teacher);
						ViewBag.AvaliableItemsTeachAndObjec = ControllerConvert.GetListTeacherWithObjec(obj.teacher);
					}
					catch { }
					ViewBag.teacher = ControllerConvert.SelectTeachersName(obj.teacher);
					ViewBag.user = us;
					ViewBag.lesson = ControllerConvert.ConvertToLesson(obj.text);
					ViewBag.CheckHave = HttpContext.Session.GetInt32("CheckHave");
				}
			}
			TempData["Message"] = HttpContext.Session.GetString("message");
            return View(us);
		}
		[HttpGet]
		public IActionResult UpdateTable(user us,string data, string clas)
		{
			var obj = context.data.Find(us.id);
			data = ControllerConvert.CleanStringForBase(data);
			obj.text = data;
			ViewBag.data = obj.text;
			ViewBag.socials = ControllerConvert.FindAllClass(obj.text);
			ViewBag.teachers = ControllerConvert.SelectTeachersName(obj.teacher);
			ViewBag.objects = ControllerConvert.SelectTeachersItem(obj.teacher);
			ViewBag.Lessons = ControllerConvert.ConvertToLesson(obj.text);
			ViewBag.Clas = clas;
			context.SaveChanges();
			return View("Table", us);
		}
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {		
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

