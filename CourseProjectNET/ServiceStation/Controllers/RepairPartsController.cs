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
    public class RepairPartsController : Controller
    {
        private MyModel db = new MyModel();

        // GET: RepairParts
        public ActionResult Index(int? id)
        {
            var repairParts = db.RepairParts.Include(r => r.Schedule).Where(w => w.ScheduleId == id);
            ViewBag.ScheduleId = id;
            return View(repairParts.ToList());
        }

        // GET: RepairParts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RepairPart repairPart = db.RepairParts.Find(id);
            if (repairPart == null)
            {
                return HttpNotFound();
            }
            return View(repairPart);
        }

        // GET: RepairParts/Create
        public ActionResult Create(int? id)
        {
            ViewBag.ScheduleId = id;
            return View();
        }

        // POST: RepairParts/Create/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,PartName,Amount,Price,ScheduleId")] RepairPart repairPart, int id)
        {
            repairPart.ScheduleId = id;
            if (ModelState.IsValid)
            {
                db.RepairParts.Add(repairPart);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = repairPart.ScheduleId });
            }

            ViewBag.ScheduleId = new SelectList(db.Schedules, "Id", "Car", repairPart.ScheduleId);
            ViewBag.OrderId = id;

            return RedirectToAction("Index", new { id = repairPart.ScheduleId });
        }

        // GET: RepairParts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RepairPart repairPart = db.RepairParts.Find(id);
            if (repairPart == null)
            {
                return HttpNotFound();
            }
            ViewBag.ScheduleId = new SelectList(db.Schedules, "Id", "Car", repairPart.ScheduleId);
            return View(repairPart);
        }

        // POST: RepairParts/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,PartName,Amount,Price,ScheduleId")] RepairPart repairPart)
        {
            if (ModelState.IsValid)
            {
                db.Entry(repairPart).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = repairPart.ScheduleId });
            }
            ViewBag.ScheduleId = new SelectList(db.Schedules, "Id", "Car", repairPart.ScheduleId);
            return View(repairPart);
        }

        // GET: RepairParts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RepairPart repairPart = db.RepairParts.Find(id);
            if (repairPart == null)
            {
                return HttpNotFound();
            }
            return View(repairPart);
        }

        // POST: RepairParts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RepairPart repairPart = db.RepairParts.Find(id);
            db.RepairParts.Remove(repairPart);
            db.SaveChanges();
            return RedirectToAction("Index", new { id = repairPart.ScheduleId });
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
