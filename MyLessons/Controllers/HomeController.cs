using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using MyLessons.ConverterSQLClass;
using MyLessons.Models;
using Newtonsoft.Json;
using System.Diagnostics;
namespace MyLessons.Controllers
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
						ViewBag.availableItems = ControllerConvert.ConvertToObject(obj.Object);
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
					ViewBag.availableItems = ControllerConvert.ConvertToObject(obj.Object);
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
		public IActionResult AddObject(user us, string NewItem)
		{
			if (NewItem == null)
			{
				HttpContext.Session.SetInt32("ChekNullStringItem", 1);
				return RedirectToAction("MainPanel", us);
			}
			HttpContext.Session.SetInt32("ChekNullStringItem", 0);
			var obj = context.data.Find(us.id);
			var list = ControllerConvert.ConvertToObject(obj.Object);
			foreach ( var item in list )
			{
				if(item == NewItem)
				{
					HttpContext.Session.SetInt32("CheckHave",1);
					return RedirectToAction("MainPanel", us);
				}
			}
			obj.Object += "|" + NewItem;
			obj.Object = ControllerConvert.CleanStringForBase(obj.Object);
			context.SaveChanges();
			return RedirectToAction("MainPanel", us);
		}
		[HttpGet]
		public IActionResult DeleteItem(user us, string DeletedObject)
		{
			var obj = context.data.Find(us.id);
            List<lesson> AllLessons = ControllerConvert.ConvertToLesson(obj.text);
            List<lesson> result = new List<lesson>();
            List<string> TeachWithItem = obj.teacher.Split("|").ToList();
            List<string> resultTeach = new List<string>();

			var list = ControllerConvert.ConvertToObject(obj.Object);
			list.Remove(DeletedObject);
			obj.Object = ControllerConvert.ConvertObjectToString(list);
			context.SaveChanges();
			HashSet<string> secondSetLower = new HashSet<string>(list.Select(s => s.ToLower()));
			TeachWithItem = TeachWithItem
				.Where(item =>
				{
					var parts = item.Split('`');
					if (parts.Length < 2) return true;
					string subject = parts[1].ToLower();
					return secondSetLower.Contains(subject);
				})
				.ToList();
			string resultTeacherWithItem = "";
			foreach (var item in TeachWithItem)
			{
				resultTeacherWithItem += item + "|";
			}
			resultTeacherWithItem = ControllerConvert.CleanStringForBase(resultTeacherWithItem);
			obj.teacher = resultTeacherWithItem;
			context.SaveChanges();
			if (AllLessons == null || AllLessons[0] == null)
			{
				return RedirectToAction("MainPanel", us);
			}
			foreach (lesson les in AllLessons)
			{
				if (les.less != DeletedObject)
				{
					result.Add(les);
				}
			}
			obj.text = ControllerConvert.ConvertLessonsArrayToString(result);
			context.SaveChanges();
			return RedirectToAction("MainPanel", us);
        }




		[HttpGet]
		public IActionResult AddLesson(user us, string selectedSubject, string clas,string room,string num,string day)
		{
            var obj = context.data.Find(us.id);

            if (string.IsNullOrEmpty(selectedSubject) || string.IsNullOrEmpty(clas) || string.IsNullOrEmpty(room) || string.IsNullOrEmpty(num) || string.IsNullOrEmpty(day))
			{
				ViewBag.ChekNull = true;
				return RedirectToAction("MainPanel", us);
            }
			lesson NewLesson = new lesson(day, num, selectedSubject, clas,room);
			string NewLessonStr = ControllerConvert.ConvertLessonToString(NewLesson);
			string BD;
			ViewBag.res = NewLessonStr;
			BD = obj.text += "|" +  NewLessonStr;
			BD = ControllerConvert.CleanStringForBase(BD);
            obj.text = BD;
            context.SaveChanges();
			try
			{
				ViewBag.objec = ControllerConvert.SelectTeachersItem(context.data.Find(us.id).teacher);
				ViewBag.AvaliableItemsTeachAndObjec = ControllerConvert.GetListTeacherWithObjec(context.data.Find(us.id).teacher);
			}
			catch { }
			ViewBag.teacher = ControllerConvert.SelectTeachersName(obj.teacher);
			ViewBag.availableItems = ControllerConvert.ConvertToObject(obj.Object);
			ViewBag.user = us;
			ViewBag.lesson = ControllerConvert.ConvertToLesson(obj.text);

			return RedirectToAction("MainPanel",us);
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
				string res = context.data.Find(us.id).teacher + "|" + name + "`" + selectedSubject;
				res = res.Trim('|');
				context.data.Find(us.id).teacher = res;
				context.SaveChanges();
			}
			try
			{
				ViewBag.objec = ControllerConvert.SelectTeachersItem(context.data.Find(us.id).teacher);
                ViewBag.AvaliableItemsTeachAndObjec = ControllerConvert.GetListTeacherWithObjec(context.data.Find(us.id).teacher);
            }
			catch { }
			ViewBag.teacher = ControllerConvert.SelectTeachersName(context.data.Find(us.id).teacher);
			ViewBag.availableItems = ControllerConvert.ConvertToObject(obj.Object);
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
			string resultTeach = ControllerConvert.ConvertTeachersToString(teach,objects);
			context.data.Find(us.id).teacher = resultTeach;
			context.SaveChanges();
			List<lesson> ChekListLesson = ControllerConvert.ConvertToLesson(context.data.Find(us.id).text);
			List<lesson> resultLess = new List<lesson>();
			foreach(var obj in ChekListLesson)
			{
				if (obj == null)
				{
					return RedirectToAction("MainPanel", us);
				}
				if (obj.teacher == name && obj.less == objec)
				{
                    resultLess.Add(obj);
                } 
			}
            context.data.Find(us.id).text = ControllerConvert.ConvertLessonsArrayToString(resultLess);
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
						ViewBag.objec = ControllerConvert.SelectTeachersItem(context.data.Find(us.id).teacher);
						ViewBag.AvaliableItemsTeachAndObjec = ControllerConvert.GetListTeacherWithObjec(context.data.Find(us.id).teacher);
					}
					catch { }
					ViewBag.teacher = ControllerConvert.SelectTeachersName(obj.teacher);
					ViewBag.availableItems = ControllerConvert.ConvertToObject(obj.Object);
					ViewBag.user = us;
					ViewBag.lesson = ControllerConvert.ConvertToLesson(obj.text);
					ViewBag.CheckHave = HttpContext.Session.GetInt32("CheckHave");
				}
			}
			ViewBag.ChekNullStringItem = HttpContext.Session.GetInt32("ChekNullStringItem");
            return View(us);
		}
		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

