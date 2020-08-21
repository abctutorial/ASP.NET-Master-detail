using MasterDetailsDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MasterDetailsDemo.Controllers
{
    public class HomeController : Controller
    {
        OnlineShopEntities db = new OnlineShopEntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UpdateCustomer(Customer customer)
        {
            Customer UpdateCustom = (from c in db.Customers
                                     where c.CustomerId == customer.CustomerId
                                     select c).FirstOrDefault();

            UpdateCustom.Name = customer.Name;
            UpdateCustom.Address = customer.Address;
            db.SaveChanges();
            return new EmptyResult();
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
    }
}