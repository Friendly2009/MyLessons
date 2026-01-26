using Microsoft.AspNetCore.Mvc;

namespace MyLessons.Controllers
{
	public class TrialController : Controller
	{
		public IActionResult TrialSched()
		{
			return View();
		}
	}
}
