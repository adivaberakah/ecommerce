using eCommerce.Contracts.Repositories;
using eCommerce.Models;
using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Service
{
    public class BasketService
    {
        IRepositoryBase<Basket> baskets;

        public const string BasketSessionName = "eCommerceBasket";

        public BasketService(IRepositoryBase<Basket> baskets)
        {
            this.baskets = baskets;

        }

        private Basket createNewBasket(HttpContextBase httpContext)
        {
            HttpCookie cookie = new HttpCookie(BasketSessionName);
            Basket basket = new Basket();
            basket.Date = DateTime.Now;
            basket.BasketId = Guid.NewGuid();
            basket.BasketItems = new List<BasketItem>();

            baskets.Insert(basket);
            baskets.Commit();
            cookie.Value = basket.BasketId.ToString();
            cookie.Expires = DateTime.Now.AddDays(1);
            httpContext.Response.Cookies.Add(cookie);
            cookie.Expires = DateTime.Now.AddDays(1);
            httpContext.Response.Cookies.Add(cookie);
            return basket;

        }
        public bool AddToBasket(HttpContextBase httpContext, int productId, int quantity)
        {
            bool success = true;
            Basket basket = GetBasket(httpContext);
            BasketItem item = basket.BasketItems.FirstOrDefault(i => i.ProductId == productId);

            if (item == null)
            {
                item = new BasketItem()
                {
                    BasketId = basket.BasketId,
                    ProductId = productId,
                    Quantity = quantity
                };
                basket.BasketItems.Add(item);

            }
            else
            {
                item.Quantity = item.Quantity + quantity;
            }
            baskets.Commit();
            return success;

        }

        public Basket GetBasket(HttpContextBase httpContect)
        {
            HttpCookie cookie = httpContect.Request.Cookies.Get(BasketSessionName);
            Basket basket;


            Guid basketId;
            if (cookie != null)
            {
                if (Guid.TryParse(cookie.Value, out basketId))
                {
                    basket = baskets.GetById(basketId);
                }
                else
                {
                    basket = createNewBasket(httpContect);
                }
            }
            else
            {
                basket = createNewBasket(httpContect);
            }
            return basket;
        }
    }
}

