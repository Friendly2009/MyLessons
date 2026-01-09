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

