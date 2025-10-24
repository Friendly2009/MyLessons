using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using MyLessons.ConverterSQLClass;
using MyLessons.Models;
using Newtonsoft.Json;
namespace MyLessons.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext context;
        private string key = "12vui323x67r8wetfx0932";
		private string count = "21763gr6uxqi78ygq3f";
		public HomeController(ILogger<HomeController> logger, ApplicationDbContext _context)
        {
            _logger = logger;
            context = _context;
        }
		public IActionResult Index()
        {
			HttpContext.Session.SetInt32(count, 0);
			ViewBag.isReg = false;
			return View();
        }
		[HttpPost]
		public IActionResult Privacy(user us)
		{
			HttpContext.Session.SetInt32(count, 0);
			if (ModelState.IsValid)
			{
				ViewBag.isReg = true;
				var user = context.user.FirstOrDefault(t => t.login == us.login);
				var text = context.data.FirstOrDefault(t => t.Id == user.id).text;
                if (user.password == us.password)
				{
					string json = JsonConvert.SerializeObject(user);
					HttpContext.Session.SetString(key, json);

                    ViewBag.user = user;
					ViewBag.Lessons = GetLessons(text);
				
                    return View("privacy");
				}
			}
			return View("Index");
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
			if(Newlogin == null && HttpContext.Session.GetInt32(count) == 1)
			{
				ViewBag.n = "¬ведите логин";
			}
			if (Newpassword == null && HttpContext.Session.GetInt32(count) == 1)
			{
				ViewBag.num = "¬ведите пароль";
			}
			return View("Index");
        }
		public List<lesson> GetLessons(string data)
		{
			List<lesson> list = new List<lesson>();
			string[] lessonArray = data.Split('|');	
			for(int i = 0; i < lessonArray.Length;i++)
			{
				list.Add(JsonConvert.DeserializeObject<lesson>(lessonArray[i]));
			}
			return list;
		}
		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
