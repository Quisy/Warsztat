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
    public class CartController : Controller
    {
        private WarsztatDataEntities db = new WarsztatDataEntities();

        // GET: Cart
        public ActionResult Index()
        {
            if (Session["User"] == null)
            {
                //return View("~/Views/Users/Login.cshtml");
                return RedirectToAction("Login", "Users");
            }
            var user = (Users)Session["User"];
            var cart = db.Cart.Include(c => c.Parts).Include(c => c.Users).Where(c => c.ID_user == user.ID_user);
            decimal price = 0;

            if (cart != null)
            {
                price = (decimal)cart.Sum(c => c.Price);
                price = Math.Round(price, 2);
            }

            ViewBag.TotalPrice = price.ToString();
            return View(cart.ToList());
        }

        [HttpPost]
        public ActionResult Index(FormCollection form, string ddl)
        {
            return View();
        }

        // GET: Cart/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cart cart = db.Cart.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            return View(cart);
        }

        // GET: Cart/Create
        public ActionResult Create()
        {
            ViewBag.ID_part = new SelectList(db.Parts, "ID_part", "PartName");
            ViewBag.ID_user = new SelectList(db.Users, "ID_user", "Name");
            return View();
        }

        // POST: Cart/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_cart,ID_user,ID_part,Quantity,Price")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                db.Cart.Add(cart);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ID_part = new SelectList(db.Parts, "ID_part", "PartName", cart.ID_part);
            ViewBag.ID_user = new SelectList(db.Users, "ID_user", "Name", cart.ID_user);
            return View(cart);
        }

        // GET: Cart/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cart cart = db.Cart.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID_part = new SelectList(db.Parts, "ID_part", "PartName", cart.ID_part);
            ViewBag.ID_user = new SelectList(db.Users, "ID_user", "Name", cart.ID_user);
            return View(cart);
        }

        // POST: Cart/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_cart,ID_user,ID_part,Quantity,Price")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cart).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID_part = new SelectList(db.Parts, "ID_part", "PartName", cart.ID_part);
            ViewBag.ID_user = new SelectList(db.Users, "ID_user", "Name", cart.ID_user);
            return View(cart);
        }

        // GET: Cart/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cart cart = db.Cart.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            return View(cart);
        }

        // POST: Cart/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Cart cart = db.Cart.Find(id);
            db.Cart.Remove(cart);
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

        public ActionResult ChangeQuantity(int id, int quantity)
        {
            Cart cart = db.Cart.Find(id);
            cart.Quantity = quantity;
            cart.Price = quantity * cart.Parts.Price;
            db.Entry(cart).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
