using Microsoft.AspNetCore.Mvc;
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
		user MyUser = new user();
		public HomeController(ILogger<HomeController> logger, ApplicationDbContext _context)
        {
            _logger = logger;
            context = _context;
        }
		public IActionResult Index()
		{
			return View();
		}
		[HttpPost]
		public IActionResult Privacy(user us)
		{
			if (!ModelState.IsValid)
			{
				ViewBag.name = us.login;
				return View("Index", us);
			}
			ViewBag.name = us.login;
			return View(us);
		}
		[HttpGet]
		public IActionResult Choose(user us)
		{
			ViewBag.name = us.login;
			return View("Privacy", us);
		}
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
				ViewBag.n = "¬ведите логин";
			}
			if (Newpassword == null && HttpContext.Session.GetInt32(count) == 1)
			{
				ViewBag.num = "¬ведите пароль";
			}
			return RedirectToAction("Privacy");
		}
		private List<lesson> ConvertToData(string data)
		{
			var list = new List<lesson>();
			string[] lessons = data.Split('|');
			for (int i = 0; i < lessons.Length; i++)
			{
				list.Add(JsonConvert.DeserializeObject<lesson>(lessons[i]));
			}
			List<string> socials = new List<string>();
			for (int i = 0; i < list.Count; i++)
			{
				socials.Add(Convert.ToString(list[i].clas));
			}
			socials = socials.Distinct().ToList();
			ViewBag.socials = socials;
			return list;
		}
		[HttpPost]
		private List<string> ConvertToTeachers(string data, int type)
		{
			List<string> Names = new List<string>();
			List<string> Objects = new List<string>();
			string[] Teachers = data.Split("|");
			for (int i = 0; i < Teachers.Length; i++)
			{
				string[] name = Teachers[i].Split("`");
				Names.Add(name[0].Trim());
				Objects.Add(name[1].Trim());
			}
			if (type == 0)
			{
				return Names;
			}
			else
			{
				return Objects;
			}
		}
		private string FindLastClass(List<lesson> less)
		{
			string result = "";
			for (int i = 0; i < less.Count; i++)
			{
				try
				{
					result = less[i - 1].clas;
				}
				catch { }
			}
			return result;
		}
		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
