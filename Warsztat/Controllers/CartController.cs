using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Warsztat.Models;
using Warsztat.ViewModels;

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

            if (cart.Count() > 0 )
            {
                price = (decimal)cart.Sum(c => c.Price);
                price = Math.Round(price, 2);
            }

            ViewBag.TotalPrice = price.ToString();
            return View(cart.ToList());
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

        public ActionResult Order()
        {
            var user = (Users)Session["User"];

            return RedirectToAction("Index","Orders");
        }

        public ActionResult Submit()
        {
            var user = (Users)Session["User"];

            //Addresses selectedAddress;
            //if (addressid == null)
            //{
            //     selectedAddress = db.Addresses.FirstOrDefault();
            //}
            //else
            //{
            //     selectedAddress = db.Addresses.Find(addressid);
            //}


            var selectedAddress = db.Addresses.FirstOrDefault();
            var addressesList = db.Addresses.Where(a => a.ID_user == user.ID_user).ToList();

            var model = new AddressViewModel
            {
                SelectedAddressId = selectedAddress.ID_address,
                Addresses = addressesList.Select(x => new SelectListItem
                {
                    Value = x.ID_address.ToString(),
                    Text = x.Street + " " + x.Number.ToString()
                })
            };

            ViewBag.address = model;

            
            //ViewBag.ID_address = new SelectList(db.Addresses.Where( a => a.ID_user == user.ID_user), "ID_address", "Street");
           
            var cart = db.Cart.Include(c => c.Parts).Include(c => c.Users).Where(c => c.ID_user == user.ID_user);
            decimal price = 0;

            if (cart.Count() > 0)
            {
                price = (decimal)cart.Sum(c => c.Price);
                price = Math.Round(price, 2);
            }

            ViewBag.TotalPrice = price.ToString();
            return View(cart.ToList());
        }

        [HttpPost]
        public ActionResult Submit(int ID_address)
        {
            var user = (Users)Session["User"];
            var cart = db.Cart.Where(c => c.ID_user == user.ID_user).ToList();

            Orders order = new Orders
            {
                ID_user = user.ID_user,
                ID_address = ID_address,
                OrderDate = DateTime.Now,
                Price = cart.Sum(c => c.Price),
                ID_status = 1
            };

           

            db.Orders.Add(order);
            db.SaveChanges();

            var orderID = db.Orders.Find(db.Orders.Max(o => o.ID_order)).ID_order;
            
            foreach (var item in cart)
            {
                Parts part = db.Parts.Find(item.ID_part);
                part.Quantity = part.Quantity - item.Quantity;

                db.Entry(part).State = EntityState.Modified;

                OrderDetails od = new OrderDetails
                {
                    ID_order = orderID,
                    ID_part = item.ID_part,
                    quantity = item.Quantity,
                    Price = item.Price
                };
                db.Cart.Remove(item);
                db.OrderDetails.Add(od);              
            }
            db.SaveChanges();

            return RedirectToAction("Index","Orders");
        }


        public ActionResult GetAddress(int id)
        {
            var selectedaddress = db.Addresses.Find(id);


            var user = (Users)Session["User"];
            ViewBag.ID_address = new SelectList(db.Addresses.Where(a => a.ID_user == user.ID_user), "ID_address", "Street");
            var cart = db.Cart.Include(c => c.Parts).Include(c => c.Users).Where(c => c.ID_user == user.ID_user);
            decimal price = 0;

            if (cart.Count() > 0)
            {
                price = (decimal)cart.Sum(c => c.Price);
                price = Math.Round(price, 2);
            }

            ViewBag.TotalPrice = price.ToString();

            return RedirectToAction("Submit", new { id = 2 });
        }
    }
}
