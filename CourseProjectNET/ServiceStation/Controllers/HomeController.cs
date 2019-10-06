using ServiceStation.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ServiceStation.Controllers
{
    public class HomeController : Controller
    {
        private MyModel db = new MyModel();

        // GET: Schedules
        public ActionResult Index(DateTime? startRange = null, DateTime? finishRange = null, int workerId = 0)
        {
            DateTime startDate = startRange ?? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime endDate = finishRange ?? new DateTime(DateTime.Now.Year, DateTime.Now.Month + 1, 1);

            var schedules = db.Schedules.Where(w => w.TimeFrom >= startDate && w.TimeTo <= endDate).
                                         Include(s => s.Place);

            if (workerId > 0)
                schedules = schedules.Where(o => o.Workers.Any(a => a.Id == workerId));

            ViewBag.WorkerId = new SelectList(db.Workers, "Id", "WorkerName");
            ViewBag.StartDate = startDate.ToString("yyyy-MM-ddTHH:mm");
            ViewBag.EndDate = endDate.ToString("yyyy-MM-ddTHH:mm");

            return View(schedules.ToList());
        }

        [Authorize]
        public ActionResult NewOrder()
        {
            ViewBag.StartDate = DateTime.Now.ToString("yyyy-MM-ddTHH:mm");
            ViewBag.PlaceId = new SelectList(db.Places, "Id", "PlaceName");
            return View();
        }

        // POST: Home/NewOrder
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewOrder([Bind(Include = "Id,PlaceId,UserId,Car,OrderStatusId,TimeFrom,TimeTo,Comment")] Schedule schedule)
        {
            User user = db.Users.FirstOrDefault(u => u.Email == User.Identity.Name);

            if (user == null)
                return Redirect("/Account/Login");

            schedule.UserId = user.Id;
            schedule.OrderStatusId = db.OrderStatuses.Where(s => s.Name == "заказ в обработке").FirstOrDefault().Id;
            schedule.TimeTo = schedule.TimeFrom;

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

        [Authorize]
        public ActionResult OrderHistory()
        {
            User user = db.Users.FirstOrDefault(u => u.Email == User.Identity.Name);

            if (user == null)
                return Redirect("/Account/Login");

            var orderHistory = db.Schedules.Where(w => w.UserId == user.Id).Include(i => i.OrderStatus)
                .Include(i => i.Place).Include(i => i.Workers);

            return View(orderHistory.ToList());
        }

        public ActionResult Services()
        {
            var services = db.Services.Include(s => s.Category).OrderBy(o => o.Category.CategoryName);
            return View(services.ToList());
        }

        public ActionResult Workers()
        {
            return View(db.Workers.OrderBy(o => o.WorkerName).ToList());
        }

        public ActionResult About()
        {
            ViewBag.Message = "О нас";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Как нас найти";

            return View();
        }
    }
}