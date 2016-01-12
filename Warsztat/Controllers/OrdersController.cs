using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Warsztat.Models;

namespace Warsztat.Controllers
{
    public class OrdersController : Controller
    {
        private WarsztatDataEntities db = new WarsztatDataEntities();

        // GET: Orders
        public ActionResult Index()
        {
            if (Session["User"] == null)
            {
                //return View("~/Views/Users/Login.cshtml");
                return RedirectToAction("Login", "Users");
            }

            var user = (Users)Session["User"];
            IQueryable<Orders> orders;

            if (user.ID_user == 2)
            {
                orders = db.Orders.Include(o => o.Users).Include(o => o.OrderStatus).Include(o => o.Addresses).Where(o => o.ID_user == user.ID_user);
            }
            else
            {
                orders = db.Orders.Include(o => o.Users).Include(o => o.OrderStatus).Include(o => o.Addresses);
            }

            return View(orders.ToList());
        }

        // GET: Orders/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Orders orders = db.Orders.Find(id);
            OrdersModel orders = new OrdersModel
            {
                Order = db.Orders.Find(id),
                OrderDetails = db.OrderDetails.Where(od => od.ID_order == id).ToList()
            };
            if (orders == null)
            {
                return HttpNotFound();
            }
            decimal price = 0;

            price = (decimal)orders.OrderDetails.Sum(od => od.Price);
            price = Math.Round(price, 2);

            ViewBag.TotalPrice = price.ToString();
            return View(orders);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            ViewBag.ID_user = new SelectList(db.Users, "ID_user", "Name");
            ViewBag.ID_status = new SelectList(db.OrderStatus, "ID_status", "StatusName");
            ViewBag.ID_address = new SelectList(db.Addresses, "ID_address", "Street");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_order,OrderNumber,ID_user,OrderDate,CompleteDate,Price,ID_status,ID_address")] Orders orders)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(orders);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ID_user = new SelectList(db.Users, "ID_user", "Name", orders.ID_user);
            ViewBag.ID_status = new SelectList(db.OrderStatus, "ID_status", "StatusName", orders.ID_status);
            ViewBag.ID_address = new SelectList(db.Addresses, "ID_address", "Street", orders.ID_address);
            return View(orders);
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orders orders = db.Orders.Find(id);
            if (orders == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID_user = new SelectList(db.Users, "ID_user", "Name", orders.ID_user);
            ViewBag.ID_status = new SelectList(db.OrderStatus, "ID_status", "StatusName", orders.ID_status);
            ViewBag.ID_address = new SelectList(db.Addresses, "ID_address", "Street", orders.ID_address);
            return View(orders);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int ID_order, int ID_status)//([Bind(Include = "ID_order,OrderNumber,ID_user,OrderDate,CompleteDate,Price,ID_status,ID_address")] Orders orders)
        {
            var order = db.Orders.Find(ID_order);
            if (ModelState.IsValid)
            {
                order.ID_status = ID_status;
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID_user = new SelectList(db.Users, "ID_user", "Name", order.ID_user);
            ViewBag.ID_status = new SelectList(db.OrderStatus, "ID_status", "StatusName", order.ID_status);
            ViewBag.ID_address = new SelectList(db.Addresses, "ID_address", "Street", order.ID_address);
            return View(order);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orders orders = db.Orders.Find(id);
            if (orders == null)
            {
                return HttpNotFound();
            }
            return View(orders);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Orders orders = db.Orders.Find(id);
            db.Orders.Remove(orders);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
