using Microsoft.AspNetCore.Mvc;

namespace ASP_LABS.Controllers
{
	public class CartController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Add(int id,string returnurl)
		{
			return View();
		}
	}
}
