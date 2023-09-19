using ASP_LABS.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ASP_LABS.Extensions;
using ASP_LABS.Services.BookService;

namespace ASP_LABS.Controllers
{
	public class CartController : Controller
	{
		private readonly IBookService _bookService;
		private readonly Cart _cart;
		public CartController(IBookService bookService, Cart cart)
		{
			_bookService = bookService;
			_cart = cart;
		}


		public IActionResult Index()
		{
			return View(_cart.CartItems);
		}

		[Authorize]
		[Route("[controller]/add/{id:int}")]
		public async Task<ActionResult> Add(int id,string returnurl)
		{ 
			var data = await _bookService.GetBookByIdAsync(id);
			if (data.Success)
			{
				_cart.AddToCart(data.Data);
			}

			return Redirect(returnurl);
		}
        [Authorize]
        public IActionResult Remove(int id)
        {
            _cart.RemoveItems(id);
            return View("Index",_cart.CartItems);
        }
        [Authorize]
        public IActionResult Clear()
        {
            _cart.ClearAll();
            return View("Index",_cart.CartItems);
        }
    }
}
