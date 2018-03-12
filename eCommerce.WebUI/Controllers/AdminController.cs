using eCommerce.Contracts.Repositories;
using eCommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eCommerce.WebUI.Controllers
{
   // [Authorize(Roles ="Administrator")]
    public class AdminController : Controller
    {

        IRepositoryBase<Customer> customers;
        IRepositoryBase<Product> products;
        public AdminController(IRepositoryBase<Customer> customers, IRepositoryBase<Product>products)
        {
            this.customers = customers;
            this.products = products;
        }
        // GET: Admin
        
        public ActionResult Index()
        {
            User.IsInRole("kwwhdy");
            return View();
        }

       // [Authorize(Roles ="Administrator")]
        public ActionResult ProductList()
        {
            var model = products.GetAll();

            return View(model);
        }

        public ActionResult CreateProduct()
        {
            var model = new Product();
            return View(model);
        }


        [HttpPost]
        public ActionResult CreateProduct(Product product)
        {
            products.Insert(product);
            products.Commit();

            return RedirectToAction("ProductList");
        }

        public ActionResult EditProduct(int id)
        {
            Product product = products.GetById(id);
            return View(product);
        }

        [HttpPost]
        public ActionResult EditProduct(Product product)
        {
            products.Update(product);
            products.Commit();

            return RedirectToAction("ProductList");
        }

        public ActionResult Delete(int id)
        {
            products.Delete(id);
            products.Commit();
            // Delete(id);
            return RedirectToAction("Index", "Admin");

            //Product product = products.Delete();
        }
    }
}