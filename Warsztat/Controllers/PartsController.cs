using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Warsztat.Models;

namespace Warsztat.Controllers
{
    public class PartsController : Controller
    {
        private WarsztatDataEntities db = new WarsztatDataEntities();

        // GET: Parts
        public ActionResult Index()
        {
            return View(db.Parts.ToList());
        }

        // GET: Parts/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Parts parts = db.Parts.Find(id);
            if (parts == null)
            {
                return HttpNotFound();
            }
            return View(parts);
        }

        // GET: Parts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Parts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_part,PartName,Quantity,Price")] Parts parts)
        {
            if (ModelState.IsValid)
            {
                db.Parts.Add(parts);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(parts);
        }

        // GET: Parts/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Parts parts = db.Parts.Find(id);
            if (parts == null)
            {
                return HttpNotFound();
            }
            return View(parts);
        }

        // POST: Parts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_part,PartName,Quantity,Price")] Parts parts)
        {
            if (ModelState.IsValid)
            {
                db.Entry(parts).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(parts);
        }

        // GET: Parts/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Parts parts = db.Parts.Find(id);
            if (parts == null)
            {
                return HttpNotFound();
            }
            return View(parts);
        }

        // POST: Parts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Parts parts = db.Parts.Find(id);
            db.Parts.Remove(parts);
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

        public ActionResult AddToCart(int? id)
        {
            if (Session["User"] == null)
            {
                //return View("~/Views/Users/Login.cshtml");
                return RedirectToAction("Login", "Users");
            }
            var user = (Users)Session["User"];
            Parts part = db.Parts.Find(id);
            Cart cart = new Cart();
            cart.ID_part = id;
            cart.ID_user = user.ID_user;
            cart.Quantity = 1;
            cart.Price = part.Price;

            db.Cart.Add(cart);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
