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
    public class RepairsController : Controller
    {
        private WarsztatDataEntities db = new WarsztatDataEntities();

        // GET: Repairs
        public ActionResult Index()
        {
            var repairs = db.Repairs.Include(r => r.Cars).Include(r => r.Services);
            return View(repairs.ToList());
        }

        // GET: Repairs/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Repairs repairs = db.Repairs.Find(id);
            if (repairs == null)
            {
                return HttpNotFound();
            }
            return View(repairs);
        }

        // GET: Repairs/Create
        public ActionResult Create(int carID)
        {
            ViewBag.ID_car = carID;
            ViewBag.ID_service = new SelectList(db.Services, "ID_service", "ServiceName");
            return View();
        }

        // POST: Repairs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_repair,ID_car,ID_service,StartDate,FinishDate")] Repairs repairs)
        {
            if (ModelState.IsValid)
            {
                repairs.ID_car = Convert.ToInt64(Request.QueryString["carID"]);
                db.Repairs.Add(repairs);
                db.SaveChanges();
                return RedirectToAction("Details", "Cars", new { id = repairs.ID_car });
            }

            ViewBag.ID_car = new SelectList(db.Cars, "ID_car", "Brand", repairs.ID_car);
            ViewBag.ID_service = new SelectList(db.Services, "ID_service", "ServiceName", repairs.ID_service);
            return View(repairs);
        }

        // GET: Repairs/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Repairs repairs = db.Repairs.Find(id);
            if (repairs == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID_car = new SelectList(db.Cars, "ID_car", "Brand", repairs.ID_car);
            ViewBag.ID_service = new SelectList(db.Services, "ID_service", "ServiceName", repairs.ID_service);
            return View(repairs);
        }

        // POST: Repairs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_repair,ID_car,ID_service,StartDate,FinishDate")] Repairs repairs)
        {
            if (ModelState.IsValid)
            {
                db.Entry(repairs).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID_car = new SelectList(db.Cars, "ID_car", "Brand", repairs.ID_car);
            ViewBag.ID_service = new SelectList(db.Services, "ID_service", "ServiceName", repairs.ID_service);
            return View(repairs);
        }

        // GET: Repairs/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Repairs repairs = db.Repairs.Find(id);
            if (repairs == null)
            {
                return HttpNotFound();
            }
            return View(repairs);
        }

        // POST: Repairs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Repairs repairs = db.Repairs.Find(id);
            db.Repairs.Remove(repairs);
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
