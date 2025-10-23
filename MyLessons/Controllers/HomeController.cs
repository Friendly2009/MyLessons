using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using MyLessons.Models;
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
		public IActionResult Index()//главна€
        {
			HttpContext.Session.SetInt32(count, 0);
			ViewBag.isReg = false;
			return View();
        }
		[HttpPost]
		public IActionResult Privacy(user us)//вход
		{
			HttpContext.Session.SetInt32(count, 0);
			if (ModelState.IsValid)
			{
				ViewBag.isReg = true;
				var obj = context.user.FirstOrDefault(t => t.login == us.login);
				if (obj.password == us.password)
				{
					string json = JsonSerializer.Serialize(obj);
					HttpContext.Session.SetString(key, json);
					ViewBag.obj = obj;
					return View("privacy");
				}
			}
			return View("Index");
		}
		[HttpPost]
        public IActionResult AddAccount(newuser newusers, string login1, string password1)// добавление аккаунта
        {
			HttpContext.Session.SetInt32(count, 1);
			if (ModelState.IsValid)
			{
				string name = newusers.login1;
				string pas = newusers.password1;
				user us = new user { login = name, password = pas };
				context.user.Add(us);
				context.SaveChanges();
			}
			if(login1 == null && HttpContext.Session.GetInt32(count) == 1)
			{
				ViewBag.n = "¬ведите логин";
			}
			if (password1 == null && HttpContext.Session.GetInt32(count) == 1)
			{
				ViewBag.num = "¬ведите пароль";
			}
			return View("Index");
        }
		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
