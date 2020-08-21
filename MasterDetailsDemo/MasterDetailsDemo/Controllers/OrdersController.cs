using MasterDetailsDemo.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MasterDetailsDemo.Controllers
{
    public class OrdersController : Controller
    {
        OnlineShopEntities db = new OnlineShopEntities();

        public ActionResult Index()
        {
        
                List<Customer> OrderAndCustomerList = db.Customers.OrderByDescending(s=>s.OrderDate).ToList();
            return View(OrderAndCustomerList);
        
        }

        [HttpPost]
        public ActionResult Index(MyOrderViewModel model)
        {
            if (model.Id > 0)
            {
                Order or = db.Orders.SingleOrDefault(x => x.Id == model.Id);

                or.Id = model.Id;
                or.ProductName = model.ProductName;
                or.Quantity = model.Quantity;
                or.Price = model.Price;
                or.Amount = model.Amount;
                db.SaveChanges();
            }

            return View(model);
        }





       
        public ActionResult AddMoreOrder(int Id)
        {
            MyOrderViewModel model = new MyOrderViewModel();

            Customer cus = db.Customers.SingleOrDefault(c => c.Id == Id);
            model.Id = cus.Id;
            model.CustomerId = cus.CustomerId;
            return PartialView("Partial2", model);

        }


        public ActionResult AddMoreOrderSave(MyOrderViewModel model)
        {
            Order or = new Order();
            or.OrderId = Guid.NewGuid();

            or.ProductName = model.ProductName;
            or.Quantity = model.Quantity;
            or.Price = model.Price;
            or.Amount = model.Amount;
            or.CustomerId = model.CustomerId;
            db.Orders.Add(or);
            db.SaveChanges();
            return View(model);

        }


        public ActionResult SaveOrder(string name, String address, Order[] order)
        {
          
                string result = "Error! Order Is Not Complete!";
            if (name != null && address != null && order != null)
            {
                var cutomerId = Guid.NewGuid();
                Customer model = new Customer();
                model.CustomerId = cutomerId;
                model.Name = name;
                model.Address = address;
                model.OrderDate = DateTime.Now;
                db.Customers.Add(model);

                foreach (var item in order)
                {
                    var orderId = Guid.NewGuid();
                    Order O = new Order();
                    O.OrderId = orderId;
                    O.ProductName = item.ProductName;
                    O.Quantity = item.Quantity;
                    O.Price = item.Price;
                    O.Amount = item.Amount;
                    O.CustomerId = cutomerId;
                    db.Orders.Add(O);
                }
                db.SaveChanges();
                result = "Success! Order Is Complete!";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
          
        }



        public ActionResult EditOrder(int Id)
        {
            MyOrderViewModel model = new MyOrderViewModel();

            Order or = db.Orders.SingleOrDefault(c => c.Id == Id);
            model.Id = or.Id;
            model.ProductName = or.ProductName;
            model.Quantity = or.Quantity;
            model.Price = or.Price;
            model.Amount = or.Amount;
            model.CustomerId = or.CustomerId;
            return PartialView("Partial1", model);
        }



        // GET: Order/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "Name", order.CustomerId);
            return View(order);
        }

        // POST: Order/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderId,ProductName,Quantity,Price,Amount,CustomerId")] Order order)
        {
            if (ModelState.IsValid)
            {

                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "Name", order.CustomerId);
            return View(order);
        }




        // GET: Customer/Edit/5
        public ActionResult EditCustomer(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customer/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCustomer([Bind(Include = "CustomerId,Name,Address,OrderDate")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customer);
        }
    }
}