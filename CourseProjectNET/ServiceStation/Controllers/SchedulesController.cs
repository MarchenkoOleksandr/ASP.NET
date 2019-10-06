using ServiceStation.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace ServiceStation.Controllers
{
    [Authorize(Roles = "Admin, Worker")]
    public class SchedulesController : Controller
    {
        private MyModel db = new MyModel();

        // GET: Schedules
        public ActionResult Index(DateTime? startRange = null, DateTime? finishRange = null, int workerId = 0)
        {
            DateTime startDate = startRange ?? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime endDate = finishRange ?? new DateTime(DateTime.Now.Year, DateTime.Now.Month + 1, 1);

            var schedules = db.Schedules.Where(w => w.TimeFrom >= startDate && w.TimeTo <= endDate).
                                         Include(s => s.OrderStatus).Include(s => s.Place).Include(s => s.User);

            if (workerId > 0)
                schedules = schedules.Where(o => o.Workers.Any(a => a.Id == workerId));

            ViewBag.WorkerId = new SelectList(db.Workers, "Id", "WorkerName");
            ViewBag.StartDate = startDate.ToString("yyyy-MM-ddTHH:mm");
            ViewBag.EndDate = endDate.ToString("yyyy-MM-ddTHH:mm");

            return View(schedules.ToList());
        }

        // GET: Schedules/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Schedule schedule = db.Schedules.Find(id);
            if (schedule == null)
            {
                return HttpNotFound();
            }
            return View(schedule);
        }

        // GET: Schedules/Create
        public ActionResult Create()
        {
            ViewBag.OrderStatusId = new SelectList(db.OrderStatuses, "Id", "Name");
            ViewBag.PlaceId = new SelectList(db.Places, "Id", "PlaceName");
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email");
            return View();
        }

        // POST: Schedules/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,PlaceId,UserId,Car,OrderStatusId,TimeFrom,TimeTo,Comment")] Schedule schedule)
        {
            if (ModelState.IsValid)
            {
                db.Schedules.Add(schedule);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OrderStatusId = new SelectList(db.OrderStatuses, "Id", "Name", schedule.OrderStatusId);
            ViewBag.PlaceId = new SelectList(db.Places, "Id", "PlaceName", schedule.PlaceId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", schedule.UserId);
            return View(schedule);
        }

        // GET: Schedules/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Schedule schedule = db.Schedules.Find(id);
            if (schedule == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrderStatusId = new SelectList(db.OrderStatuses, "Id", "Name", schedule.OrderStatusId);
            ViewBag.PlaceId = new SelectList(db.Places, "Id", "PlaceName", schedule.PlaceId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", schedule.UserId);
            return View(schedule);
        }

        // POST: Schedules/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,PlaceId,UserId,Car,OrderStatusId,TimeFrom,TimeTo,Comment")] Schedule schedule)
        {
            if (ModelState.IsValid)
            {
                db.Entry(schedule).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OrderStatusId = new SelectList(db.OrderStatuses, "Id", "Name", schedule.OrderStatusId);
            ViewBag.PlaceId = new SelectList(db.Places, "Id", "PlaceName", schedule.PlaceId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", schedule.UserId);
            return View(schedule);
        }

        // GET: Schedules/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Schedule schedule = db.Schedules.Find(id);
            if (schedule == null)
            {
                return HttpNotFound();
            }
            return View(schedule);
        }

        // POST: Schedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Schedule schedule = db.Schedules.Find(id);
            db.Schedules.Remove(schedule);
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

        // GET: Schedules/AddWorker
        public ActionResult AddWorker()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var item in db.Workers.ToList())
            {
                items.Add(new SelectListItem
                {
                    Text = item.WorkerName,
                    Value = item.Id.ToString()
                });
            }
            return View(items);
        }

        // POST: Schedules/AddWorker
        [HttpPost]
        public ActionResult AddWorker(int? id, List<SelectListItem> items)
        {
            Schedule schedule = db.Schedules.Find(id);

            foreach (SelectListItem item in items)
            {
                if (item.Selected)
                    schedule.Workers.Add(db.Workers.Find(int.Parse(item.Value)));
                else if (schedule.Workers.FirstOrDefault(w => w.Id == int.Parse(item.Value)) != null)
                    schedule.Workers.Remove(db.Workers.Find(int.Parse(item.Value)));
            }

            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
