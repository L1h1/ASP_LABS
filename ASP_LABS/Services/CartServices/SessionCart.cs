using ASP_LABS.Domain.Entities;
using ASP_LABS.Extensions;
using System.Text.Json.Serialization;

namespace ASP_LABS.Services.CartServices
{
    public class SessionCart : Cart
    {
        [JsonIgnore]
        public ISession? Session { get; set; }


        public static Cart GetCart(IServiceProvider services)
        {
            ISession? session = services.GetRequiredService<IHttpContextAccessor>()
            .HttpContext?.Session;
            SessionCart cart = session?.Get<SessionCart>("cart")
            ?? new SessionCart();
            cart.Session = session;
            return cart;
        }

        /// <summary>
        /// Добавить объект в корзину
        /// </summary>
        /// <param name="book">Добавляемый объект</param>
        public override void AddToCart(Book book)
        {

            base.AddToCart(book);
            Session.Set<SessionCart>("cart", this);

        }
        /// <summary>
        /// Удалить объект из корзины
        /// </summary>
        /// <param name="id"> id удаляемого объекта</param>
        public override void RemoveItems(int id)
        {
            base.RemoveItems(id);
            Session.Set<SessionCart>("cart", this);
        }
        /// <summary>
        /// Очистить корзину
        /// </summary>
        public override void ClearAll()
        {
            base.ClearAll();
            Session.Set<SessionCart>("cart", this);
        }
    }
}
