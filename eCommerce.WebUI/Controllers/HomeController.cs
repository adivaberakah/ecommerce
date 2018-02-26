using eCommerce.Contracts.Repositories;
using eCommerce.DAL.Data;
using eCommerce.DAL.Repositories;
using eCommerce.Models;
using eCommerce.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eCommerce.WebUI.Controllers
{
    public class HomeController : Controller
    {
        IRepositoryBase<Customer> customers;
        IRepositoryBase<Product> products;
        IRepositoryBase<Basket> baskets;
        BasketService basketService;

        public HomeController(IRepositoryBase<Customer> customers, IRepositoryBase<Product> products, IRepositoryBase<Basket>baskets)
        {
            this.customers = customers;
            this.products = products;
            this.baskets = baskets;
            basketService = new BasketService(this.baskets);
        }
        public ActionResult Index()
        {
            var product = products.GetAll();
            return View(product);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult BasketSummary()
        {
            var model = basketService.GetBasket(this.HttpContext);
            return View(model.BasketItems);
        }
        [HttpGet]
        public ActionResult AddtoBasket(int id)
        {
            basketService.AddToBasket(this.HttpContext, id, 1);
            return RedirectToAction("BasketSummary");
        }

        public ActionResult Delete(int id)
        {
            products.Delete(id);
            products.Commit();
           // Delete(id);
            return RedirectToAction ("Index","Home");

            //Product product = products.Delete();
        }

        public ActionResult Details(int id)
        {
            var product = products.GetById(id);

            return View(product);
        }
    }
}