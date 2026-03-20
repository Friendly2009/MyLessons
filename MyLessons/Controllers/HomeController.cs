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
		[HttpPost]
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
		public IActionResult ToReg()
		{
			return View("Registration");
		}
		public IActionResult ToAuth()
		{
			return View("Authorization");
		}
		public IActionResult Support()
		{
			return View();
		}
        public IActionResult UsersQuestion()
        {
            return View();
        }
        public IActionResult AddUser(user model)
		{
            if (ModelState.IsValid)
            {
                foreach (var user in UsersTable)
                {
                    if (user.login == model.login)
                    {
						ViewBag.message = "ѕользователь с этим логином уже существует.";
						return Index();
                    }
                }
                UsersTable.Add(model);
                context.data.Add(new Data(context.user.Count() + 1));
				context.data.Find(context.user.Count() + 1).text = "[]";
				context.SaveChanges();
				return Table(model);
            }
            return Index();
		}
        public IActionResult Error()
        {
			#if DEBUG
				return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
			#else
				return View("Ooops");
			#endif
        }
    }
}

