using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ServiceStation.Models;

namespace ServiceStation.Controllers
{
    [Authorize(Roles = "Admin, Worker")]
    public class OrdersController : Controller
    {
        private MyModel db = new MyModel();

        // GET: Orders
        public ActionResult Index(int? id)
        {
            var orders = db.Orders.Include(o => o.Schedule).Include(o => o.Service).Where(w => w.ScheduleId == id);
            ViewBag.OrderId = id;
            return View(orders.ToList());
        }

        // GET: Orders/Details/5
        public ActionResult Details(int? id)
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
            return View(order);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            int i = 0;

            foreach (var item in db.Services.ToList())
            {
                items.Add(new SelectListItem
                {
                    Text = item.ServiceName,
                    Value = item.Id.ToString()
                });

                ViewData["time" + i] = item.NecessaryTime / 60 + " ч.  " + item.NecessaryTime % 60 + " мин.";
                ViewData["price" + i] = item.Price;
                i++;
            }

            return View(items);
        }

        // POST: Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int? id, List<SelectListItem> items)
        {
            List<Order> orders = db.Orders.Where(o => o.ScheduleId == id).ToList();
            int serviceId = 0;

            foreach (SelectListItem item in items)
            {
                serviceId = int.Parse(item.Value);
                if (item.Selected && !orders.Exists(o => o.ServiceId == serviceId))
                {
                    decimal price = db.Services.Find(serviceId).Price;
                    db.Orders.Add(new Order { ScheduleId = (int)id, ServiceId = serviceId, Price = price });
                }
            }

            db.SaveChanges();

            return RedirectToAction("Index", new { id });
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.Schedule = db.Schedules.Find(id);
            return View(order);
        }

        // POST: Orders/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ScheduleId,ServiceId,Price")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = order.ScheduleId });
            }
            ViewBag.ScheduleId = new SelectList(db.Schedules, "Id", "Car", order.ScheduleId);
            ViewBag.ServiceId = new SelectList(db.Services, "Id", "ServiceName", order.ServiceId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Order order = db.Orders.Find(id);
            if (order == null)
                return HttpNotFound();

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Index", new { id=order.ScheduleId });
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
