using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;

namespace ASP_LABS.ViewComponents
{
    [ViewComponent]
    public class Cart
    {
        public HtmlString Invoke()
        {
            double price = 0;
            int amount = 0;
            return new HtmlString($"{price} руб <i class=\"fa-solid fa-cart-shopping\"></i>({amount})");

        }
    }
}
