using ASP_LABS.Domain.Entities;
using ASP_LABS.Extensions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;

namespace ASP_LABS.ViewComponents
{
    [ViewComponent]
    public class CartViewComponent
    {
        private readonly Cart _cart;
        public CartViewComponent(Cart cart) 
        {
            _cart = cart;
        }

        public HtmlString Invoke()
        {
            double price = _cart.TotalSum;
            int amount = _cart.Count;
            return new HtmlString($"{price} руб <i class=\"fa-solid fa-cart-shopping\"></i>({amount})");

        }
    }
}
