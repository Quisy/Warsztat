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
    public class UsersController : Controller
    {
        private WarsztatDataEntities db = new WarsztatDataEntities();

        // GET: Users
        public ActionResult Index()
        {
            if (Session["User"] == null)
            {
                //return View("~/Views/Users/Login.cshtml");
                return RedirectToAction("Login", "Users");
            }
            var user = (Users)Session["User"];

            if (user.ID_role == 1)
            {
                return RedirectToAction("Index", "Home");
            }
            var users = db.Users.Include(u => u.Roles);
            return View(users.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var tempuser = db.Users.Find(id);
            UserModel user = new UserModel
            {
                Users = db.Users.Find(id),
                Addresses = db.Addresses.Where(a => a.ID_user == tempuser.ID_user).ToList(),
                Cars = db.Cars.Where(c => c.ID_user == tempuser.ID_user).ToList(),
                Orders = db.Orders.Where(o => o.ID_user == tempuser.ID_user).ToList()
            };
            //Users users = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            ViewBag.ID_role = new SelectList(db.Roles, "ID_role", "RoleName");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_user,ID_role,Name,Surname,Login,Password")] Users users)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(users);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ID_role = new SelectList(db.Roles, "ID_role", "RoleName", users.ID_role);
            return View(users);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID_role = new SelectList(db.Roles, "ID_role", "RoleName", users.ID_role);
            return View(users);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_user,ID_role,Name,Surname,Login,Password")] Users users)
        {
            if (ModelState.IsValid)
            {
                db.Entry(users).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID_role = new SelectList(db.Roles, "ID_role", "RoleName", users.ID_role);
            return View(users);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Users users = db.Users.Find(id);
            db.Users.Remove(users);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Users/Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string login, string password)
        {
            //var query = from a in db.Users where (a.Login.Equals(login)) && (a.Password.Equals(password)) select a;
            Users user = db.Users.Include(u => u.Roles).FirstOrDefault(u => (u.Login == login) && (u.Password == password));
            if (user != null)
            {
                Session["User"] = user;
                //Session["UserName"] = user.Login;
                //Session["UserID"] = user.ID_user;
                //Session["UserRole"] = user.Roles.RoleName;
                Session.Timeout = 20;

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.ErrorCode = "Wrong username or password";
                return View();
            }

        }

        public ActionResult Logout()
        {
            Session["User"] = null;
            //Session["UserName"] = null;
            //Session["UserID"] = null;
            //Session["UserRole"] = null;
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Register()
        {
            var users = new UserModel();
            return View();
        }

        [HttpPost]
        public ActionResult Register([Bind(Include = "ID_user,ID_role,Name,Surname,Login,Password")] Users users)//, [Bind(Include = "ID_address,ID_user,Street,Number,City,PostCode")] Addresses addresses, string Street, int Number, string City, string PostCode)
        {
            if (ModelState.IsValid)
            {
                users.ID_role = 1;
                db.Users.Add(users);
                db.SaveChanges();

                //var user =  db.Users.Find(db.Users.Max(p => p.ID_user));
                //addresses.ID_user = user.ID_user;

                //db.Addresses.Add(addresses);
                //db.SaveChanges();

                return RedirectToAction("Login");
            }
            return View();
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
