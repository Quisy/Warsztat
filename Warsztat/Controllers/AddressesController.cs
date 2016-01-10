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
    public class AddressesController : Controller
    {
        private WarsztatDataEntities db = new WarsztatDataEntities();

        // GET: Addresses
        public ActionResult Index()
        {
            var addresses = db.Addresses.Include(a => a.Users);
            return View(addresses.ToList());
        }

        // GET: Addresses/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Addresses addresses = db.Addresses.Find(id);
            if (addresses == null)
            {
                return HttpNotFound();
            }
            return View(addresses);
        }

        // GET: Addresses/Create
        public ActionResult Create()
        {
            ViewBag.ID_user = new SelectList(db.Users, "ID_user", "Name");
            return View();
        }

        // POST: Addresses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_address,ID_user,Street,Number,City,PostCode")] Addresses addresses)
        {
            if (ModelState.IsValid)
            {
                db.Addresses.Add(addresses);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ID_user = new SelectList(db.Users, "ID_user", "Name", addresses.ID_user);
            return View(addresses);
        }

        // GET: Addresses/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Addresses addresses = db.Addresses.Find(id);
            if (addresses == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID_user = new SelectList(db.Users, "ID_user", "Name", addresses.ID_user);
            return View(addresses);
        }

        // POST: Addresses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_address,ID_user,Street,Number,City,PostCode")] Addresses addresses)
        {
            if (ModelState.IsValid)
            {
                db.Entry(addresses).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID_user = new SelectList(db.Users, "ID_user", "Name", addresses.ID_user);
            return View(addresses);
        }

        // GET: Addresses/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Addresses addresses = db.Addresses.Find(id);
            if (addresses == null)
            {
                return HttpNotFound();
            }
            return View(addresses);
        }

        // POST: Addresses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Addresses addresses = db.Addresses.Find(id);
            db.Addresses.Remove(addresses);
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
